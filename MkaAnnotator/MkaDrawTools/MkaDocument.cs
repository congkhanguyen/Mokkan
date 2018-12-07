using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using MokkAnnotator;
using WeifenLuo.WinFormsUI.Docking;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using MokkAnnotator.MkaCommon;
using MokkAnnotator.MkaDockToolkit;
using MokkAnnotator.MkaImageProcess;
using System.IO;
using Microsoft.Win32;
using System.Xml;
using MokkAnnotator.MkaDocToolkit.Xml;
using MokkAnnotator.MkaWindow;
using MokkAnnotator.MkaToolsData;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;

namespace MokkAnnotator.MkaDrawTools
{
    partial class MkaDocument : DockContent
    {
        static log4net.ILog _log = MkaDefine.Logger;    // log manager   

        // create array of drawing tools
        static Tool[] tools = new Tool[(int)ToolType.ToolsCount] {
                                            new ToolPointer(),
                                            new ToolRectangle(),
                                            new ToolEllipse(),
                                            new ToolPolygon()};

        #region Members

        private MkaFrmAnnotator _owner;
        private UndoManager _undoManager;

        private Bitmap _image;       // glass's image
        private Bitmap _bkImg;
        private int _width;
        private int _height;
        private Rectangle _rect;

        private float _zoom;         // zoom scale
        private bool _dirty = false;

        private MkaGlassInfo _glassInfo;    // glass information
        private MokkanList _mokkanList;             // list of draw objects

        private List<DrawObject> _copyItems = new List<DrawObject>();    // copy item list        
        private bool _isCopy = true;
        private int _continousPaste = 0;

        private ToolType _activeTool;            // active drawing tool        
        private DisplayType _activeDisplay;      // active display type        
        #endregion

        #region Properties

        /// <summary>
        /// Bat information
        /// </summary>
        public MkaBatInfo BatInfo
        {
            get { return _glassInfo.BatInfo; }
            set { _glassInfo.BatInfo = value; }
        }

        /// <summary>
        /// Glass information
        /// </summary>
        public MkaGlassInfo GlassInfo
        {
            get { return _glassInfo; }
            set { _glassInfo = value; }
        }

        /// <summary>
        /// Document filename
        /// </summary>
        public String FileName
        {
            get { return GlassInfo.GlassFileName; }
        }

        /// <summary>
        /// Document file fullname
        /// </summary>
        public String FileFullName
        {
            get { return GlassInfo.GlassFilePath; }
        }

        /// <summary>
        /// Glass's image
        /// </summary>
        public Bitmap Image
        {
            get { return _image; }
            set { _image = value; }
        }

        /// <summary>
        /// Image's filename
        /// </summary>
        public string GlassImage
        {
            get { return GlassInfo.ImageFilePath; }
            set
            {
                // change display image                      
                this._image = new Bitmap(Bitmap.FromFile(value));
                this._bkImg = (Bitmap)_image.Clone();

                // update glass information
                GlassInfo.ImageFilePath = value;
                GlassInfo.ImageFileName = Path.GetFileName(value);
                GlassInfo.Width = _image.Width;
                GlassInfo.Height = _image.Height;

                // update size of window
                UpdateSize();
            }
        }

        /// <summary>
        /// Width of image
        /// </summary>
        public int ImageWidth
        {
            get { return _width; }
        }

        /// <summary>
        /// Height of image
        /// </summary>
        public int ImageHeight
        {
            get { return _height; }
        }

        /// <summary>
        /// Boundary rectangle of image
        /// </summary>
        public Rectangle ImageRectangle
        {
            get { return _rect; }
        }

        /// <summary>
        /// Zoom scale
        /// </summary>
        public int Zoom
        {
            get { return (int)(_zoom * 100); }
            set { _zoom = (float)(value / 100); }
        }

        /// <summary>
        /// Active drawing tool.
        /// </summary>
        public ToolType ActiveTool
        {
            get { return _activeTool; }
            set { _activeTool = value; _owner.ToolbarChangeCommand(value); }
        }

        /// <summary>
        /// Active display type
        /// </summary>
        public DisplayType ActiveDisplay
        {
            get { return _activeDisplay; }
            set { _activeDisplay = value; }
        }

        /// <summary>
        /// List of graphics objects.
        /// </summary>
        public MokkanList MokkanList
        {
            get { return _mokkanList; }
            set
            {
                _mokkanList = value;
                _undoManager.MokkanList = value;
            }
        }

        /// <summary>
        /// Return True if Undo operation is possible
        /// </summary>
        public bool CanUndo
        {
            get
            {
                if (_undoManager != null)
                {
                    return _undoManager.CanUndo;
                }

                return false;
            }
        }

        /// <summary>
        /// Return True if Redo operation is possible
        /// </summary>
        public bool CanRedo
        {
            get
            {
                if (_undoManager != null)
                {
                    return _undoManager.CanRedo;
                }

                return false;
            }
        }

        /// <summary>
        /// Dirty property (true when document has unsaved changes).
        /// </summary>
        public bool Dirty
        {
            get
            {
                return _dirty;
            }
            set
            {
                _dirty = value;
                UpdateCaption();
            }
        }

        /// <summary>
        /// Used to fix bug open dialog -> double click
        /// </summary>
        public bool ReOpen { get; set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public MkaDocument(MkaFrmAnnotator owner)
        {
            // Init components
            InitializeComponent();

            _owner = owner;

            // Init members
            this._image = null;
            this._width = 0;
            this._height = 0;
            this._glassInfo = new MkaGlassInfo();
            this._mokkanList = new MokkanList();
            this.ReOpen = false;

            this.MouseWheel += new MouseEventHandler(DocumentArea_MouseWheel);

            // set default tool
            _activeTool = ToolType.Pointer;

            // Create undo manager
            _undoManager = new UndoManager(_mokkanList);

            // Configure form's style
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.AutoScroll = true;

            // Init zoom scale
            this._zoom = 1;
        }

        /// <summary>
        /// Renew document when overwrite
        /// </summary>
        /// <param name="glassInfo"></param>
        public void ReNew(MkaGlassInfo glassInfo)
        {
            // Init members                                    
            this._glassInfo = glassInfo;
            this._mokkanList = new MokkanList();
            this._mokkanList.KaishiRBangou = glassInfo.KaishiRBangou;
            this.GlassImage = glassInfo.ImageFilePath;

            // set default tool
            _activeTool = ToolType.Pointer;

            // Create undo manager
            _undoManager = new UndoManager(_mokkanList);
            Dirty = false;
            ReOpen = false;

            // Init zoom scale
            ZoomToWindow();
        }

        /// <summary>
        /// Update size of window
        /// </summary>
        private void UpdateSize()
        {
            // update size of image
            _width = _image.Width;
            _height = _image.Height;
            _rect = new Rectangle(0, 0, _width, _height);

            // update size of scroll bar
            this.AutoScrollMinSize = new Size((int)(_width * _zoom), (int)(_height * _zoom));

            // redraw
            this.Invalidate();
        }

        #region Zoom Processing
        /// <summary>
        /// Zoom image
        /// </summary>
        /// <param name="zoom">zoom scale</param>
        public void ZoomImage(float zoom)
        {
            // update zoom scale
            this._zoom = zoom;

            // update size after zoom
            UpdateZoom();
        }

        /// <summary>
        /// Update size after zoom
        /// </summary>
        public void UpdateZoom()
        {
            DrawObject.Ratio = this._zoom;

            _owner.DocAreaZoomChange();

            // update size of scroll bar
            this.AutoScrollMinSize = new Size((int)(_width * _zoom), (int)(_height * _zoom));

            // redraw
            this.Invalidate();
        }

        /// <summary>
        /// Expand image
        /// </summary>
        public void ZoomIn()
        {
            // enlarge by next scale between 0.25 and 5 times
            if (_zoom < 5)
            {
                for (int i = 0; i < MkaDefine.Zooms.Length; i++)
                {
                    if (this._zoom < (float)MkaDefine.Zooms[i] / 100)
                    {
                        _zoom = (float)MkaDefine.Zooms[i] / 100;
                        break;
                    }
                }
            }
            else // otherwise, enlarge by unit of 1
                _zoom = (int)_zoom + 1f;

            // maximum 10 times
            if (_zoom > 10)
                _zoom = 10;

            // update size after zoom
            UpdateZoom();
        }

        /// <summary>
        /// Zoom to fix window size
        /// </summary>
        public void ZoomToWindow()
        {
            // get scale to fix window size
            _zoom = Math.Min((float)this.Width / (_width + 2), (float)this.Height / (_height + 2));

            // update size after zoom
            UpdateZoom();
        }

        /// <summary>
        /// Zoom to original size
        /// </summary>
        public void ZoomToActual()
        {
            // update zoom scale
            this._zoom = 1f;

            // update size after zoom
            UpdateZoom();
        }

        /// <summary>
        /// Shrink image
        /// </summary>
        public void ZoomOut()
        {
            // Minimum 0.25 times
            if (_zoom == 0.25) return;

            // Shrink by previous scale between 0.25 and 6 times
            if (_zoom < 6)
            {
                for (int i = MkaDefine.Zooms.Length - 1; i >= 0; i--)
                {
                    if (this._zoom > (float)MkaDefine.Zooms[i] / 100)
                    {
                        _zoom = (float)MkaDefine.Zooms[i] / 100;
                        break;
                    }
                }
            }
            else if (_zoom > (int)_zoom)  // otherwise, shrink by unit of 1
                _zoom = (float)(int)_zoom;
            else
                _zoom -= 1f;

            // update size after zoom
            UpdateZoom();
        }

        /// <summary>
        /// Zoom when wheel mouse
        /// </summary>        
        private void DocumentArea_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta == 0)
                return;

            if (Control.ModifierKeys != Keys.Control)
                return;

            if (e.Delta > 0)
                ZoomIn();
            else
                ZoomOut();
        }

        #endregion

        #region Rotate Processing

        /// <summary>
        /// Flip image
        /// </summary>
        public void RotateFlip()
        {
            // flip image
            this._image.RotateFlip(RotateFlipType.RotateNoneFlipY);

            // update size of window
            UpdateSize();
        }

        /// <summary>
        /// Mirror image
        /// </summary>
        public void RotateMirror()
        {
            // mirror image
            this._image.RotateFlip(RotateFlipType.RotateNoneFlipX);

            // update size of window
            UpdateSize();
        }

        /// <summary>
        /// Rotate image 180 degree
        /// </summary>
        public void Rotate180()
        {
            // 90 degree right rotate
            this._image.RotateFlip(RotateFlipType.Rotate180FlipNone);

            // update size of window
            UpdateSize();
        }

        /// <summary>
        /// Rotate to the right
        /// </summary>
        public void RotateRight()
        {
            // 90 degree right rotate
            this._image.RotateFlip(RotateFlipType.Rotate90FlipNone);

            // update size of window
            UpdateSize();
        }

        /// <summary>
        /// 
        /// </summary>
        public void RotateLeft()
        {
            // 90 degrees left rotate
            this._image.RotateFlip(RotateFlipType.Rotate270FlipNone);

            // update size of window
            UpdateSize();
        }

        /// <summary>
        /// Rotate 
        /// </summary>
        /// <param name="angle"></param>
        public void Rotate(double angle, Color fillColor, RotateType type)
        {
            this._owner.Cursor = Cursors.WaitCursor;
            if (type == RotateType.NearestNeighbor)
                _image = ImageRotate.RotateNearestNeighbor(_image, _rect, angle, fillColor);
            else if (type == RotateType.Bilinear)
                _image = ImageRotate.RotateBilinear(_image, _rect, angle, fillColor);
            else
                _image = ImageRotate.RotateBicubic(_image, _rect, angle, fillColor);

            // update size of window
            UpdateSize();
            this._owner.Cursor = Cursors.Default;
        }

        #endregion

        #region Image Processing

        /// <summary>
        /// Restore to original image
        /// </summary>
        public void ImageRestore()
        {
            this._owner.Cursor = Cursors.WaitCursor;
            _image.Dispose();
            _image = (Bitmap)_bkImg.Clone();
            UpdateSize();
            this._owner.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Adjust brightness and contrast of image
        /// </summary>        
        public void ImageAdjustBrightnessContrast(int brightness, int contrast)
        {
            this._owner.Cursor = Cursors.WaitCursor;            
            Bitmap tmp = ImageAdjustment.AdjustBrightnessContrast(_image, _rect, brightness, contrast);
            if(tmp != null)
            {
                _image.Dispose();
                _image = tmp;
            }
            
            Refresh();
            this._owner.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Adjust hue, saturation and lightness of image
        /// </summary>        
        public void ImageAdjustHSL(int hue, int saturation, int lightness)
        {
            this._owner.Cursor = Cursors.WaitCursor;            
            Bitmap tmp = ImageAdjustment.AdjustHSL(_image, _rect, hue, saturation, lightness);
            if (tmp != null)
            {
                _image.Dispose();
                _image = tmp;
            }
            Refresh();
            this._owner.Cursor = Cursors.Default;
        }

        #endregion Image Processing

        #region Other Functions

        /// <summary>
        /// Copied item count
        /// </summary>
        [DefaultValue(-1)]
        public int CopyCount { get; set; }

        /// <summary>
        /// Cut
        /// </summary>
        public void Cut()
        {
            if (!this.IsActivated) return;

            // copy items
            Copy();

            _isCopy = false;

            // delete cut items            
            CommandDelete command = new CommandDelete(_mokkanList);

            if (_mokkanList.DeleteSelection())
            {
                SetDirty();
                Refresh();
                AddCommandToHistory(command);
            }
        }

        /// <summary>
        /// Copy
        /// </summary>
        public void Copy()
        {
            if (!this.IsActivated) return;

            _isCopy = true;
            _copyItems.Clear();
            _copyItems = _mokkanList.SelectedCloneObjects;
            CopyCount = _copyItems.Count;
            _continousPaste = 1;

            SetDirty();
            Refresh();
        }

        /// <summary>
        /// Paste
        /// </summary>
        public void Paste()
        {
            if (!this.IsActivated) return;

            _mokkanList.UnselectAll();
            List<DrawObject> pasteItems = new List<DrawObject>();
            DrawObject pasteObj;
            foreach (DrawObject obj in _copyItems)
            {
                pasteObj = obj.Clone();
                pasteObj.ID = pasteObj.GetHashCode();
                if (_isCopy)
                    pasteObj.Move(10 * _continousPaste, 10 * _continousPaste);
                pasteObj.MokkanInfo.RBangou = _mokkanList.GetMaxRBangou() + 1;
                pasteObj.Selected = true;
                pasteItems.Add(pasteObj);
                _mokkanList.Add(pasteObj);
            }
            _isCopy = true;

            CommandAdd command = new CommandAdd(pasteItems);
            SetDirty();
            Refresh();
            AddCommandToHistory(command);
            _continousPaste++;
        }

        /// <summary>
        /// Delete
        /// </summary>
        public void Delete()
        {
            if (!this.IsActivated) return;

            CommandDelete command = new CommandDelete(MokkanList);

            if (MokkanList.DeleteSelection())
            {
                SetDirty();
                Refresh();
                AddCommandToHistory(command);
            }
        }

        /// <summary>
        /// Delete all
        /// </summary>
        public void DeleteAll()
        {
            if (!this.IsActivated) return;

            CommandDeleteAll command = new CommandDeleteAll(MokkanList);

            if (MokkanList.Clear())
            {
                SetDirty();
                Refresh();
                AddCommandToHistory(command);
            }
        }

        /// <summary>
        /// Move forward
        /// </summary>
        public void MoveFront()
        {
            if (!this.IsActivated) return;

            if (MokkanList.MoveSelectionToFront())
            {
                SetDirty();
                Refresh();
            }
        }

        /// <summary>
        /// Move backward
        /// </summary>
        public void MoveBack()
        {
            if (!this.IsActivated) return;

            if (MokkanList.MoveSelectionToBack())
            {
                SetDirty();
                Refresh();
            }
        }

        /// <summary>
        /// Select all
        /// </summary>
        public void SelectAll()
        {
            if (!this.IsActivated) return;

            MokkanList.SelectAll();
            Refresh();
        }

        /// <summary>
        /// Unselect 
        /// </summary>
        public void UnSelect()
        {
            if (!this.IsActivated) return;

            MokkanList.UnselectAll();
            Refresh();
        }

        /// <summary>
        /// Show property window
        /// </summary>
        public void ShowPropertyDialog()
        {
            if (MokkanList.SelectionCount == 0)
            {                
                MkaPropertiesGlass frm = new MkaPropertiesGlass(_glassInfo);
                frm.ShowDialog(this);
                if(frm.Changed)
                {
                    SetDirty();
                    if (frm.RFontSizeChanged)
                    {
                        MokkanList.UpdateRFontSize(_glassInfo.RFontSize);
                        Refresh();
                    }
                }
            }
            else if (MokkanList.ShowPropertiesDialog(this))
            {
                SetDirty();
                Refresh();
            }
        }

        /// <summary>
        /// Add command to history.
        /// </summary>
        public void AddCommandToHistory(Command command)
        {
            _undoManager.AddCommandToHistory(command);

            // mokkan data changed, so raise event             
            _owner.DocAreaDataChange();
        }

        /// <summary>
        /// Clear Undo history.
        /// </summary>
        public void ClearHistory()
        {
            _undoManager.ClearHistory();
            this.Dirty = false;
        }

        /// <summary>
        /// Undo
        /// </summary>
        public void Undo()
        {
            if (!_undoManager.CanUndo)
                return;
            this.Dirty = !_undoManager.LastUndo;

            _mokkanList.UnselectAll();
            _undoManager.Undo();
            _mokkanList = _undoManager.MokkanList;
            _mokkanList.Calculate();
            Refresh();
            if (_continousPaste > 1)
                _continousPaste--;

            this.Invalidate();

            // mokkan data changed, so raise event             
            _owner.DocAreaDataChange();
        }

        /// <summary>
        /// Redo
        /// </summary>
        public void Redo()
        {
            if (!_undoManager.CanRedo)
                return;

            _mokkanList.UnselectAll();
            _undoManager.Redo();
            _mokkanList = _undoManager.MokkanList;
            _mokkanList.Calculate();
            Refresh();
            _continousPaste++;

            this.Dirty = true;
            this.Invalidate();

            // mokkan data changed, so raise event             
            _owner.DocAreaDataChange();
        }


        /// <summary>
        /// Set dirty flag (file is changed after last save operation)
        /// </summary>
        public void SetDirty()
        {
            this.Dirty = true;
        }

        /// <summary>
        /// Update document caption
        /// </summary>
        public void UpdateCaption()
        {
            if (_dirty)
                this.Text = this.FileName + "*";
            else
                this.Text = this.FileName;

            _owner.SetCaption(this.Text);
        }

        /// <summary>
        /// Right-click handler
        /// </summary>
        /// <param name="e"></param>
        private void OnContextMenu(MouseEventArgs e)
        {
            // Change current selection if necessary

            Point point = new Point(e.X, e.Y);

            int n = MokkanList.Count;
            DrawObject o = null;

            for (int i = 0; i < n; i++)
            {
                if (MokkanList[i].HitTest(point) == 0)
                {
                    o = MokkanList[i];
                    break;
                }
            }

            if (o != null)
            {
                if (!o.Selected)
                    MokkanList.UnselectAll();

                // Select clicked object
                o.Selected = true;
            }
            else
            {
                MokkanList.UnselectAll();
            }

            Refresh();      // in the case selection was changed

            _owner.MkaContextMenu.Show(this, e.Location);
        }

        /// <summary>
        /// Show remain id or not
        /// </summary>        
        public void RShow(bool showOrNot)
        {
            DrawObject.RShow = showOrNot;

            // redraw
            Invalidate();
        }

        /// <summary>
        /// Create data grid from mokkan list
        /// </summary>
        public DataGridView GetDataGrid()
        {
            DataGridView dtg = new DataGridView();
            dtg.AllowUserToAddRows = false;
            DataGridViewColumn col;
            DataGridViewRow row;

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = MkaDefine.MokkanLblRBangou;
            dtg.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = MkaDefine.MokkanLblKariShakubun;
            dtg.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = MkaDefine.MokkanLblGaihouShoshuuJyouhou;
            dtg.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = MkaDefine.MokkanLblShasinBangouJyouhou;
            dtg.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = MkaDefine.MokkanLblBikou;
            dtg.Columns.Add(col);

            foreach (DrawObject o in MokkanList.MokkanObjectList)
            {
                row = new DataGridViewRow();
                row.CreateCells(dtg);
                row.Cells[0].Value = o.MokkanInfo.RBangou;
                row.Cells[1].Value = o.MokkanInfo.KariShakubun;
                row.Cells[2].Value = o.MokkanInfo.GaihouShoshuuJyouhou;
                row.Cells[3].Value = o.MokkanInfo.ShasinBangouJyouhou;
                row.Cells[4].Value = o.MokkanInfo.Bikou;
                dtg.Rows.Add(row);
            }

            return dtg;
        }

        protected override string GetPersistString()
        {
            // Add extra information into the persist string for this document
            // so that it is available when deserialized.
            return GetType().ToString() + "," + FileName + "," + Text;
        }

        #endregion

        #region IO

        /// <summary>
        /// Get image of selected mokkan to decode
        /// </summary>
        /// <returns></returns>
        public Bitmap GetMokkanToDecode(out int rid)
        {
            rid = 0;
            if (_mokkanList.SelectionCount != 1) return null;

            DrawObject obj = _mokkanList.SelectedObjects[0];
            rid = obj.MokkanInfo.RBangou;
            Bitmap img = GetImage(obj, true);

            return img;
        }

        /// <summary>
        /// Get preview image for image processing
        /// </summary>
        public Bitmap GetPreviewImage()
        {
            if (_image == null) return new Bitmap(200, 200);
            int width = Math.Max(200, _width);
            int height = Math.Max(200, _height);
            int startX = (int)(width - 200) / 2;
            int startY = (int)(height - 200) / 2;
            Rectangle cropRec = new Rectangle(startX, startY, width, height);
            Bitmap img = new Bitmap(cropRec.Width, cropRec.Height);
            Rectangle dstRec = new Rectangle(0, 0, img.Width, img.Height);
            Graphics g = Graphics.FromImage(img);
            g.DrawImage(_image, dstRec, cropRec, GraphicsUnit.Pixel);
            g.Dispose();
            return img;
        }

        /// <summary>
        /// Save mokkan file
        /// </summary>        
        public void Save(bool clear)
        {
            try
            {
                //// auto label before save
                //if (this.MokkanList.Count > 0)
                //    AutoLabel();

                // Serialize object to xml format
                XmlDocument doc = new XmlDocument();
                IXmlFormatter formatter = FepXmlableFactory.CreateXmlFormatter();
                IXmlContext cnt = FepXmlableFactory.CreateXmlContext(formatter, doc);
                doc = cnt.Document;
                XmlDeclaration xmldecl = cnt.Document.CreateXmlDeclaration("1.0", null, null);

                XmlElement _eleBat = cnt.ToXml(this.BatInfo);
                XmlElement _eleGlass = cnt.ToXml(this.GlassInfo);
                this.MokkanList.Sort();
                XmlElement _eleMk = cnt.ToXml(this.MokkanList);
                _eleGlass.AppendChild(_eleMk);
                _eleBat.AppendChild(_eleGlass);
                doc.AppendChild(_eleBat);
                
                // update last r bangou
                MkaMokkanInfo.LastRBangou = MokkanList.GetMaxRBangou() + 1;

                doc.InsertBefore(xmldecl, doc.DocumentElement);
                doc.Save(FileFullName);
                               
                // save mokkan images
                if(MkaDefine.SaveMokkanImageAsSaving)
                    SaveMokkans(FileFullName);

                // register to database
                if(MkaDefine.RegisterToDbAsSaving)
                    RegisterToDb();

                if (clear)
                    ClearHistory();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(_owner, MkaMessage.ErrSaveFile);
                _log.Error(MkaMessage.ErrSaveFile, ex);
            }
        }

        /// <summary>
        /// Save as mokkan file
        /// </summary>
        public void SaveAs()
        {
            // Get the file name
            string newFileName = FileFullName;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = MkaDefine.MkaFilter; ;
            sfd.InitialDirectory = Path.GetDirectoryName(newFileName);
            sfd.FileName = Path.GetFileName(newFileName);

            if (sfd.ShowDialog(_owner) != DialogResult.OK)
                return;

            newFileName = sfd.FileName;
            GlassInfo.GlassFilePath = newFileName;

            // save as glass file
            Save(true);

            // save as image file               
            String newPath = Path.GetDirectoryName(newFileName);
            String newImageName = newPath + "\\" + Path.GetFileNameWithoutExtension(newFileName) + Path.GetExtension(GlassInfo.ImageFilePath);
            File.Copy(GlassInfo.ImageFilePath, newImageName, true);
            GlassImage = newImageName;
        }

        /// <summary>
        /// Save mokkan images
        /// </summary>        
        private void SaveMokkans(String filename)
        {
            // create folder to save mokkans
            String path = Path.GetDirectoryName(filename) + "\\" + Path.GetFileNameWithoutExtension(filename);
                        
            // if folder were existed, delete all
            if (Directory.Exists(path))
                Array.ForEach(Directory.GetFiles(path), delegate(string file) { File.Delete(file); });
            else
                Directory.CreateDirectory(path); 

            Bitmap img;
            String fname;
            //StreamWriter writer;

            foreach(DrawObject obj in MokkanList.MokkanObjectList)
            {
                // save image
                img = GetImage(obj, false);
                fname = String.Format("{0}\\{1}_{2}_{3}_{4:00000}.jpg", path, BatInfo.Grid, BatInfo.BatBangou, GlassInfo.GlassItaBangou, obj.MokkanInfo.RBangou);
                img.Save(fname);
                img.Dispose();   

                //// save boundary coordinates
                //if (MkaDefine.SaveMokkanBoundaryAsSaving)
                //{
                //    fname = String.Format("{0}\\{1}_{2}_{3}_{4:00000}.txt", path, BatInfo.Grid, BatInfo.BatBangou, GlassInfo.GlassItaBangou, obj.MokkanInfo.RBangou);
                //    writer = new StreamWriter(fname);
                //    writer.WriteLine(obj.Points.Count);
                //    foreach (Point p in obj.Points)
                //        writer.WriteLine("{0}\t{1}", p.X - obj.Left, p.Y - obj.Top);
                //    writer.Close();
                //}                                
            }
        }

        /// <summary>
        /// Export data to csv file
        /// </summary>
        public void ExportToCSV()
        {
            // get last accessed path
            RegistryKey key = MkaDefine.RootKey.OpenSubKey(MkaDefine.RegKey);
            String path = (String)key.GetValue(MkaDefine.RecentGlassPath);

            // save to csv file dialog
            SaveFileDialog sfd = new SaveFileDialog();
            if (Directory.Exists(path))
                sfd.InitialDirectory = path;
            sfd.Filter = MkaDefine.IOCSVFilter;
            sfd.FileName = Path.GetFileNameWithoutExtension(GlassInfo.GlassFilePath) + MkaDefine.IOCSVFileExt;

            // get file name
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            String fileName = sfd.FileName;

            // open file to write data
            StreamWriter sw = new StreamWriter(fileName, false, Encoding.GetEncoding("Shift-JIS"));

            // write glass information
            GlassInfo.ExportToCsv(sw);

            // write mokkan information' column name
            sw.WriteLine();
            sw.Write(String.Format("{0},", MkaDefine.MokkanLblRBangou));
            sw.Write(String.Format("{0},", MkaDefine.MokkanLblKariShakubun));
            sw.Write(String.Format("{0},", MkaDefine.MokkanLblGaihouShoshuuJyouhou));
            sw.Write(String.Format("{0},", MkaDefine.MokkanLblShasinBangouJyouhou));
            sw.Write(String.Format("{0}", MkaDefine.MokkanLblBikou));
            sw.WriteLine();

            // write mokkan information
            foreach (DrawObject o in MokkanList.MokkanObjectList)
                o.MokkanInfo.ExportToCsv(sw);

            // close file
            sw.Close();
        }
        /// <summary>
        /// Register annotation data to database
        /// </summary>
        public void RegisterToDb()
        {
            MkaDBConnect dbCon = new MkaDBConnect();            
            int glassId = -1;
            MySqlConnection con;
            MySqlCommand cmd;            

            // open connection
            if (dbCon.OpenConnection() == false)
            {
                MkaMessage.ShowError(MkaMessage.ErrDBConnect);
                return;
            }
            con = dbCon.GetConnection();

            try
            {
                // insert chousa, bat, glass information into database
                cmd = new MySqlCommand("insert_glass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("_chousaJisuu", GlassInfo.ChousaJisuu));
                cmd.Parameters.Add(new MySqlParameter("_ooChikuBangou", GlassInfo.OoChiku));
                cmd.Parameters.Add(new MySqlParameter("_chuushouChikuBangou", GlassInfo.ChuushouChiku));
                cmd.Parameters.Add(new MySqlParameter("_ikouBangou", GlassInfo.Ikoumei));
                cmd.Parameters.Add(new MySqlParameter("_dosouMei", GlassInfo.Dosoumei));
                cmd.Parameters.Add(new MySqlParameter("_grid", GlassInfo.Grid));
                cmd.Parameters.Add(new MySqlParameter("_date", GlassInfo.Date.ToString(MkaDefine.DBDateFormat)));
                cmd.Parameters.Add(new MySqlParameter("_batBangou", GlassInfo.BatBangou));
                cmd.Parameters.Add(new MySqlParameter("_batPath", BatInfo.BatDirectory));
                cmd.Parameters.Add(new MySqlParameter("_glassBangou", GlassInfo.GlassItaBangou));
                cmd.Parameters.Add(new MySqlParameter("_mokkanKazu", GlassInfo.MokkanKazu));
                cmd.Parameters.Add(new MySqlParameter("_kaishiRBangou", GlassInfo.KaishiRBangou));
                cmd.Parameters.Add(new MySqlParameter("_imageFileName", GlassInfo.ImageFileName));
                cmd.Parameters.Add(new MySqlParameter("_imageWidth", GlassInfo.Width));
                cmd.Parameters.Add(new MySqlParameter("_imageHeight", GlassInfo.Height));
                cmd.Parameters.Add(new MySqlParameter("_glassId", glassId));
                cmd.Parameters["_glassId"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                glassId = (int)cmd.Parameters["_glassId"].Value;

                // delete mokkan of the glass if existed
                cmd = new MySqlCommand("delete_glass_mokkan", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("_glassId", glassId));
                cmd.ExecuteNonQuery();

                // insert mokkan, format, shape into database
                foreach (DrawObject mokkan in MokkanList.MokkanObjectList)
                {
                    cmd = new MySqlCommand("insert_mokkan", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new MySqlParameter("_rBangou", mokkan.MokkanInfo.RBangou));
                    cmd.Parameters.Add(new MySqlParameter("_glassId", glassId));
                    cmd.Parameters.Add(new MySqlParameter("_kariShakubun", mokkan.MokkanInfo.KariShakubun));
                    cmd.Parameters.Add(new MySqlParameter("_gaihouShoshuuJyouhou", mokkan.MokkanInfo.GaihouShoshuuJyouhou));
                    cmd.Parameters.Add(new MySqlParameter("_shasinBangouJyouhou", mokkan.MokkanInfo.ShasinBangouJyouhou));
                    cmd.Parameters.Add(new MySqlParameter("_bikou", mokkan.MokkanInfo.Bikou));
                    cmd.Parameters.Add(new MySqlParameter("_borderColor", mokkan.MokkanInfo.Properties.BorderColorHtml));
                    cmd.Parameters.Add(new MySqlParameter("_borderThick", mokkan.MokkanInfo.PenWidth));
                    cmd.Parameters.Add(new MySqlParameter("_fillColor", mokkan.MokkanInfo.Properties.FillColorHtml));
                    cmd.Parameters.Add(new MySqlParameter("_fillTransparent", mokkan.MokkanInfo.FillColorAlpha));
                    cmd.Parameters.Add(new MySqlParameter("_rShowPos", (int)mokkan.MokkanInfo.RShowPosition));
                    cmd.Parameters.Add(new MySqlParameter("_shapeType", mokkan.ShapeType));
                    cmd.Parameters.Add(new MySqlParameter("_startX", mokkan.Left));
                    cmd.Parameters.Add(new MySqlParameter("_startY", mokkan.Top));
                    cmd.Parameters.Add(new MySqlParameter("_width", mokkan.Width));
                    cmd.Parameters.Add(new MySqlParameter("_height", mokkan.Height));
                    if (mokkan.ShapeType == 3)
                    {
                        String pointStr = "";
                        foreach (Point point in mokkan.Points)
                            pointStr += "(" + point.X.ToString() + "," + point.Y.ToString() + ");";
                        pointStr = pointStr.Substring(0, pointStr.Length - 1);
                        cmd.Parameters.Add(new MySqlParameter("_coordinates", pointStr));
                    }
                    else
                        cmd.Parameters.Add(new MySqlParameter("_coordinates", null));
                    cmd.ExecuteNonQuery();
                }            
            }
            catch(Exception ex)
            {
                _log.Error(ex.Message);
            }
            
            // close connection
            dbCon.CloseConnection();
        }

        /// <summary>
        /// Get bitmap from draw object
        /// </summary>        
        public Bitmap GetImage(DrawObject mokkan, bool retRect)
        {
            Rectangle cropRec = Rectangle.FromLTRB(mokkan.Left, mokkan.Top, mokkan.Right, mokkan.Bottom);
            Bitmap boundBmp, cropBmp;

            boundBmp = this.Image.Clone(cropRec, this.Image.PixelFormat);            
            if (retRect || mokkan.ShapeType == 1)   // Rectangle                         
            {
                cropBmp = new Bitmap(boundBmp);;
            }
            else if (mokkan.ShapeType == 2) // Ellipse
            {
                cropBmp = new Bitmap(cropRec.Width, cropRec.Height, PixelFormat.Format32bppArgb);
                TextureBrush tb = new TextureBrush(boundBmp);
                Graphics g = Graphics.FromImage(cropBmp);                
                g.FillEllipse(tb, 0, 0, cropRec.Width, cropRec.Height);
                g.Dispose();
                tb.Dispose();
                cropBmp.MakeTransparent(Color.Black);
            }
            else   // Polygon            
            {
                cropBmp = new Bitmap(cropRec.Width, cropRec.Height, PixelFormat.Format32bppArgb);                
                TextureBrush tb = new TextureBrush(boundBmp);
                Graphics g = Graphics.FromImage(cropBmp);
                g.FillRectangle(Brushes.White, cropRec);
                GraphicsPath gp = new GraphicsPath();
                Point p1, p2;
                p1 = new Point(mokkan.Points[0].X - cropRec.Left, mokkan.Points[0].Y - cropRec.Top);
                for (int i = 1; i < mokkan.Points.Count; i++)
                {
                    p2 = new Point(mokkan.Points[i].X - cropRec.Left, mokkan.Points[i].Y - cropRec.Top);
                    gp.AddLine(p1, p2);
                    p1 = p2;
                }
                g.FillPath(tb, gp);               
                g.Dispose();
                tb.Dispose();
                cropBmp.MakeTransparent(Color.Black);
            }

            boundBmp.Dispose();

            return cropBmp;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Draw graphic objects and 
        /// group selection rectangle (optionally)
        /// </summary>
        private void DocumentArea_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rc = ClientRectangle;

            // specifies nearest-neighbor interpolation
            g.InterpolationMode = InterpolationMode.NearestNeighbor;

            SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255));
            g.FillRectangle(brush, this.ClientRectangle);
            brush.Dispose();

            // zoom image size
            int zoom_width = (int)(this._width * _zoom);
            int zoom_height = (int)(this._height * _zoom);

            // start point
            int x = (rc.Width < zoom_width) ? this.AutoScrollPosition.X : 0;
            int y = (rc.Height < zoom_height) ? this.AutoScrollPosition.Y : 0;

            DrawObject.Origin = new Point(x, y);
            DrawObject.Ratio = _zoom;

            g.TranslateTransform(x, y);

            // draw image
            if (this._image != null && this._activeDisplay != DisplayType.DataOnly)
                g.DrawImage(_image, 0, 0, zoom_width, zoom_height);

            // draw boundaries
            if (this._mokkanList.Count > 0 && this._activeDisplay != DisplayType.ImageOnly)
            {
                _glassInfo.MokkanKazu = _mokkanList.Count;
                
                //update Rfont for each drawing object 
                foreach (DrawObject o in _mokkanList.MokkanObjectList)
                    o.Properties.RFontSize = this.GlassInfo.RFontSize;
                _mokkanList.Draw(g);
            }
            
            // draw meta jouhou 
            // label chiku
            string chiku_Grid = this.GlassInfo.ChuushouChiku + "-" + this.GlassInfo.Grid;
            Font fontChiku = new Font(MkaDefine.RDefaultFontName, this.GlassInfo.RFontSize * 1.5f * _zoom);
            SizeF measureChiku = g.MeasureString(chiku_Grid + "0", fontChiku); //add a end character to string, make it longer

             // check a lable existing or not
            Control[] chikuControls = this.Controls.Find(chiku_Grid, false);
            if (chikuControls.Length == 0)
            {
                
                // shown position
                Point showPntChiku = new Point((int)(this.ImageWidth * _zoom - 2 * measureChiku.Width), (int)(3 * measureChiku.Height));

                MetaJouhouLabel chikuLabel = new MetaJouhouLabel(chiku_Grid);
     

                // properti for chiku label
                chikuLabel.Size = new Size((int)measureChiku.Width, (int)measureChiku.Height);
                chikuLabel.Location = showPntChiku;
                chikuLabel.TextAlign = ContentAlignment.MiddleCenter;
                chikuLabel.Font = fontChiku;

                // store current configuration
                chikuLabel.CurentRatio = _zoom;
                //chikuLabel.Origin = DrawObject.Origin;
                
                // add size of document
                chikuLabel.WidthOwner = this.ImageWidth;
                chikuLabel.HeightOwner = this.ImageHeight;

                // add to document
                this.Controls.Add(chikuLabel);
            }
            else
            {
                MetaJouhouLabel foundControl = (MetaJouhouLabel)chikuControls[0];
                foundControl.Font = fontChiku;
                foundControl.Size = new Size((int)measureChiku.Width, (int)measureChiku.Height);
                float scale = _zoom / foundControl.CurentRatio;
               
                if (_zoom != foundControl.CurentRatio)
                {
                    Point preLocation = new Point(foundControl.Location.X - this.AutoScrollPosition.X, foundControl.Location.Y -this.AutoScrollPosition.Y);
                    PointF changeLocation = new PointF(preLocation.X * scale + this.AutoScrollPosition.X, preLocation.Y * scale + this.AutoScrollPosition.Y);
                    foundControl.CurentRatio = _zoom;
                    foundControl.Location = Point.Round(changeLocation);
                }
                //if (foundControl.Origin != DrawObject.Origin)
                //    foundControl.Origin = DrawObject.Origin;

                //check if change location out of client size
                Point currentLocation = foundControl.Location;
                if (currentLocation.X + foundControl.Width > this.ImageWidth * _zoom + DrawObject.Origin.X)
                    currentLocation.X = (int)(this.ImageWidth * _zoom - foundControl.Width + DrawObject.Origin.X);
                if (currentLocation.Y + foundControl.Height > this.ImageHeight * _zoom + DrawObject.Origin.Y)
                    currentLocation.Y = (int)(this.ImageHeight * _zoom - foundControl.Height + DrawObject.Origin.Y);
                if (foundControl.Location != currentLocation)
                    foundControl.Location = currentLocation;
            }
        }

        /// <summary>
        /// Mouse down.
        /// Left button down event is passed to active tool.
        /// Right button down event is handled in this class.
        /// </summary>
        private void DocumentArea_MouseDown(object sender, MouseEventArgs e)
        {
            ReOpen = false;
            this.Activate();

            if (e.Button == MouseButtons.Left && _activeDisplay != DisplayType.ImageOnly)
                tools[(int)_activeTool].OnMouseDown(this, e);
            else if (e.Button == MouseButtons.Right)
                OnContextMenu(e);
        }

        /// <summary>
        /// Mouse move.
        /// Moving without button pressed or with left button pressed
        /// is passed to active tool.
        /// </summary>
        private void DocumentArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (ReOpen) return;

            if ((e.Button == MouseButtons.Left || e.Button == MouseButtons.None) && _activeDisplay != DisplayType.ImageOnly)
                tools[(int)_activeTool].OnMouseMove(this, e);
            else
                this.Cursor = Cursors.Default;

            ReOpen = false;
        }

        /// <summary>
        /// Mouse up event.
        /// Left button up event is passed to active tool.
        /// </summary>
        private void DocumentArea_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _activeDisplay != DisplayType.ImageOnly)
                tools[(int)_activeTool].OnMouseUp(this, e);

            // raise select change event
            _owner.DocAreaSelectChange();
        }

        #endregion Event Handlers

        #region Image Processing

        /// <summary>
        /// Auto create boundaries
        /// </summary>
        public void AutoSelect(List<DrawObject> _mokkans)
        {
            this._owner.Refresh();
            this.Refresh();
            this._owner.Cursor = Cursors.WaitCursor;
            
            CommandChange command = new CommandChange(MokkanList);
           
            this.MokkanList.MokkanObjectList = _mokkans;

            // rearrange remain id
            MokkanList.ReArrangeId(GlassInfo.KaishiRBangou);

            command.NewState(MokkanList);
            AddCommandToHistory(command);

            // update
            this.Invalidate();
            SetDirty();

            this._owner.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Auto label remain id
        /// </summary>
        public void AutoLabel()
        {
            this._owner.Cursor = Cursors.WaitCursor;

            CommandChange command = new CommandChange(MokkanList);

            // rearrange remain id
            MokkanList.ReArrangeId(GlassInfo.KaishiRBangou);

            command.NewState(MokkanList);
            AddCommandToHistory(command);

            // redraw
            SetDirty();
            this.Invalidate();

            this._owner.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Rearrange id
        /// </summary>
        public void Rearrange(int delta)
        {
            this._owner.Cursor = Cursors.WaitCursor;

            CommandChange command = new CommandChange(MokkanList);

            // rearrange remain id
            foreach (DrawObject o in MokkanList.MokkanObjectList)
                o.MokkanInfo.RBangou += delta;

            command.NewState(MokkanList);
            AddCommandToHistory(command);

            // redraw
            SetDirty();
            this.Invalidate();

            this._owner.Cursor = Cursors.Default;
        }

        #endregion

        /// <summary>
        /// Closing process
        /// </summary>        
        private void MkaDocument_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            if (!_dirty)
            {
                _image.Dispose();
                _bkImg.Dispose();
                return;
            }

            String msg = String.Format(MkaMessage.InfoSaveQuestion, this.FileName);

            switch (MessageBox.Show(_owner, msg, Application.ProductName,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
            {
                case DialogResult.Yes:
                    Save(true);
                    _image.Dispose();
                    _bkImg.Dispose();
                    break;
                case DialogResult.No:
                    _image.Dispose();
                    _bkImg.Dispose();
                    break; ;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break; ;
                default: break;
            }
        }

        /// <summary>
        /// Save document
        /// </summary>        
        private void tsmniSave_Click(object sender, EventArgs e)
        {
            this.Save(true);
        }

        /// <summary>
        /// Close document
        /// </summary>        
        private void tsmniClose_Click(object sender, EventArgs e)
        {
            this.DockHandler.Close();
        }
    }
}
