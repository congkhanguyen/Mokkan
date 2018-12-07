using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using WeifenLuo.WinFormsUI.Docking;
using MokkAnnotator.MkaDockToolkit;
using MokkAnnotator.MkaDrawTools;
using MokkAnnotator.MkaWindow;
using MokkAnnotator.MkaCommon;
using MokkAnnotator.MkaDocToolkit.Xml;
using MokkAnnotator.MkaDocToolkit;
using MokkAnnotator.MkaToolsData;
using System.Collections;
using Microsoft.Win32;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace MokkAnnotator
{
    public partial class MkaFrmAnnotator : Form
    {
        #region Members

        private static log4net.ILog _log = MkaDefine.Logger;    // log manager       
        private string argumentFile = "";               // file name from command line        

        private DeserializeDockContent _deserializeDockContent;
        private DragDropManager _dragDropBatManager;    // permit drag&drop to open bat file
        private DragDropManager _dragDropGlassManager;  // permit drag&drop to open glass file        
        private MruManager _recentBmkManager;           // keep recent bat files 
        private MruManager _recentMkgManager;           // keep recent glass files 
        private PersistWindowState _persistState;       // keep last window state        

        private MkaPrint _print;                        // print manager       
        private MkaFrmSearch _frmSearch;                // mokkan search form

        private MkaDocument _activeDoc;         // active glass file document
        private MkaBatManager _batManager;      // glass file manager

        private MkaWndExplorer _wndExplorer;    // file explorer window
        private MkaWndMokkan _wndMokkan;        // mokkan list view window       
        private MkaWndProperty _wndProperty;    // property view window        
        private MkaWndToolbar _wndToolbar;      // toolbar for annotation window               

        private int _zoomRatio;                 // zoom ratio       
        private DisplayType _displayType;       // data display type
        private ToolType _activeTool;           // tool type

        #endregion Members

        #region Properties

        /// <summary>
        /// MkaWndToolBar object
        /// </summary>
        public MkaWndToolbar WndToolBar
        {
            get { return _wndToolbar; }
            set { _wndToolbar = value; }
        }
        /// <summary>        
        /// Context menu in the mokkan file view area
        /// </summary>        
        public ContextMenuStrip MkaContextMenu
        {
            get { return mkaContextMenu; }
        }

        /// <summary>
        /// File name from the command line
        /// </summary>
        public string ArgumentFile
        {
            get { return argumentFile; }
            set { argumentFile = value; }
        }

        #endregion Properties

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public MkaFrmAnnotator()
        {
            InitializeComponent();

            // construct windows
            _wndExplorer = new MkaWndExplorer(this);
            _wndMokkan = new MkaWndMokkan(this);
            _wndToolbar = new MkaWndToolbar(this);
            _wndProperty = new MkaWndProperty(this);

            // initialize manager objects
            InitPrintManager();
            InitBatManager();
            InitGlassManager();

            // initialize window components
            InitWindow();
        }

        /// <summary>
        /// Initialize form
        /// </summary>        
        private void frmMokkAnnotator_Load(object sender, EventArgs e)
        {
            // initialize view
            InitView();

            // initialize control components
            InitControls();

            // Open file passed in the command line            
            if (File.Exists(argumentFile))
            {
                String ext = Path.GetExtension(argumentFile);
                if (ext == MkaDefine.MkaFileExt)
                {
                    _activeDoc = FileLoadGlass(argumentFile);
                    if (_activeDoc != null)
                    {
                        // display on tab panel
                        _activeDoc.Show(mkaDockPanel);
                    }
                }
                else if (ext == MkaDefine.BmkFileExt)
                    FileOpenBat(argumentFile);
            }
        }

        /// <summary>
        /// After loading form
        /// </summary>        
        private void frmMokkAnnotator_Shown(object sender, EventArgs e)
        {
            // load application settings
            MkaDefine.LoadAppSettings();

            // load settings
            LoadSettingsFromRegistry();

            // property grid                        
            _wndToolbar.Properties = GraphicsProperties.LastProperties;
        }

        /// <summary>
        /// Initialize print manger
        /// </summary>
        private void InitPrintManager()
        {
            _print = new MkaPrint();
            mniFilePrintImage.Checked = false;
            mniFilePrintAnnotation.Checked = false;
            mniFilePrintAll.Checked = true;
            _print.PrintGlassImage = false;
            _print.PrintMokkanInfo = false;
            _print.PrintGlassInfo = true;
        }

        /// <summary>
        /// Initialize bat file manager
        /// </summary>
        private void InitBatManager()
        {
            // drag drop manager
            _dragDropBatManager = new DragDropManager(this);
            _dragDropBatManager.FileDroppedEvent += delegate(object sender, FileDroppedEventArgs e)
            {
                String[] fileArr = (String[])e.FileArray;
                List<String> bats = new List<String>();
                foreach (String file in fileArr)
                    if (Path.GetExtension(file) == MkaDefine.BmkFileExt)
                        bats.Add(file);

                if (bats.Count > 0)
                {
                    OpenBatHandler d = new OpenBatHandler(FileOpenBat);
                    this.Invoke(d, new object[] { bats[bats.Count - 1] });
                }
            };

            // recent files manager
            _recentBmkManager = new MruManager();
            _recentBmkManager.Initialize(this, this.mniFileRecentBats, this.mniFile, MkaDefine.RecentBats);
            _recentBmkManager.MruOpenEvent += delegate(object sender, MruFileOpenEventArgs e)
            {
                FileOpenBat(e.FileName);
            };
        }

        /// <summary>
        /// Initialize glass file manager
        /// </summary>
        private void InitGlassManager()
        {
            // drag drop manager
            _dragDropGlassManager = new DragDropManager(this);
            _dragDropGlassManager.FileDroppedEvent += delegate(object sender, FileDroppedEventArgs e)
            {
                String[] fileArr = (String[])e.FileArray;
                List<String> glasses = new List<String>();
                foreach (String file in fileArr)
                    if (Path.GetExtension(file) == MkaDefine.MkaFileExt)
                        glasses.Add(file);

                if (glasses.Count > 0)
                {
                    OpenGlassHandler d = new OpenGlassHandler(FileOpenGlass);
                    this.Invoke(d, new object[] { glasses.ToArray() });
                }
            };

            // recent files manager
            _recentMkgManager = new MruManager();
            _recentMkgManager.Initialize(this, this.mniFileRecentGlasses, this.mniFile, MkaDefine.RecentGlasses);
            _recentMkgManager.MruOpenEvent += delegate(object sender, MruFileOpenEventArgs e)
            {
                FileOpenGlass(e.FileName);
            };
        }

        internal delegate void OpenBatHandler(String bat);
        internal delegate void OpenGlassHandler(String[] glasses);

        /// <summary>
        /// Initialize window components
        /// </summary>
        private void InitWindow()
        {
            mkaNaviBar.Renderer = new MkaToolStripSystemRenderer();

            _deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);

            // keep last window state
            _persistState = new PersistWindowState(this);

            // initialize display status            
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (File.Exists(configFile))
                mkaDockPanel.LoadFromXml(configFile, _deserializeDockContent);
            else
            {
                mkaDockPanel.SuspendLayout(true);
                _wndExplorer.Show(mkaDockPanel, DockState.DockLeftAutoHide);
                _wndToolbar.Show(mkaDockPanel, DockState.DockLeftAutoHide);
                _wndProperty.Show(mkaDockPanel, DockState.DockRightAutoHide);
                _wndMokkan.Show(mkaDockPanel, DockState.DockBottomAutoHide);
                mkaDockPanel.ResumeLayout(true);
            }
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(MkaWndExplorer).ToString())
                return _wndExplorer;
            else if (persistString == typeof(MkaWndMokkan).ToString())
                return _wndMokkan;
            else if (persistString == typeof(MkaWndProperty).ToString())
                return _wndProperty;
            else if (persistString == typeof(MkaWndToolbar).ToString())
                return _wndToolbar;
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Initialize view
        /// </summary>
        private void InitView()
        {
            // initialize zoom
            _zoomRatio = 100;
            tscmbZoom.SelectedText = "100%";

            // initialize display
            mniViewAll.Checked = true;
            tsbtnViewAll.Checked = true;
            _displayType = DisplayType.All;
        }

        /// <summary>
        /// Initialize control components
        /// </summary>
        private void InitControls()
        {
            // Submit to Idle event to set controls state at idle time
            Application.Idle += delegate(object o, EventArgs a)
            {
                SetStateOfFile();
                SetStateOfViewEdit();
            };

            // Subscribe to DropDownOpened event for each popup menu            
            foreach (ToolStripItem item in this.mkaMenuBar.Items)
            {
                if (item.GetType() == typeof(ToolStripMenuItem))
                {
                    ((ToolStripMenuItem)item).DropDownOpened += frmMokkAnnotator_DropDownOpened;
                }
            }
        }

        #region Relationships between windows

        /// <summary>
        /// Draw Toolbar Command
        /// </summary>        
        public void ToolbarCommandChanged(ToolType type)
        {
            // get command type
            switch (type)
            {
                case ToolType.Pointer:      // pointer
                    _activeTool = ToolType.Pointer;
                    break;
                case ToolType.Rectangle:    // rectangle
                    _activeTool = ToolType.Rectangle;
                    break;
                case ToolType.Ellipse:      // ellipse
                    _activeTool = ToolType.Ellipse;
                    break;
                case ToolType.Polygon:      // polygon
                    _activeTool = ToolType.Polygon;
                    break;
            }

            if (_activeDoc != null)
                _activeDoc.ActiveTool = _activeTool;
        }

        public void ToolbarChangeCommand(ToolType type)
        {
            _wndToolbar.Select(type);
        }

        #region Called from Explorer window

        public void ExplorerShowPropertyGrid(string glassName, bool isGlass, bool isShow)
        {
            if (isGlass)
            {
                MkaDocument doc = FileGetDoc(glassName);
                if (doc != null)
                {
                    _wndProperty.SelectDoc(_activeDoc);
                    if (isShow)
                        _activeDoc.ShowPropertyDialog();
                }
                else
                    _wndProperty.Clear();
            }
            else
            {
                _wndProperty.SelectBat(_batManager);
                if (isShow)
                    _batManager.ShowPropertyDialog();
            }
        }

        #endregion Called from Explorer window

        #region Called from Document window

        public void DocAreaDataChange()
        {
            // change data grid
            _wndMokkan.ChangeGridData(_activeDoc.MokkanList);

            // change property grid
            _wndProperty.SelectDoc(_activeDoc);
        }

        public void DocAreaSelectChange()
        {
            // change property grid
            _wndProperty.SelectDoc(_activeDoc);

            // change data grid
            _wndMokkan.ChangeSelect(_activeDoc.MokkanList);
        }

        public void DocAreaZoomChange()
        {
            // update zoom ratio
            this.tscmbZoom.Text = this._activeDoc.Zoom + "%";
        }

        #endregion Called from Document window

        #region Called from Mokkan window

        public void MokkanDataChange()
        {
            // change document
            CommandChange command = new CommandChange(_activeDoc.MokkanList);
            command.NewState(_wndMokkan.MokkanList);
            _activeDoc.AddCommandToHistory(command);
            _activeDoc.SetDirty();
            _activeDoc.Refresh();

            // change property grid
            _wndProperty.SelectDoc(_activeDoc);
        }

        public void MokkanSelectChange()
        {
            // change document            
            _activeDoc.Refresh();

            // change property grid                
            _wndProperty.SelectDoc(_activeDoc);
        }

        #endregion Called from Mokkan window

        #region Called from Property window

        public void PropertyDataChange()
        {
            // change mokkan information
            if (_wndProperty.SelectedType == SelectedObjectType.Mokkan)
            {
                // change data grid                
                _wndMokkan.ChangeGridData(_wndProperty.MokkanList);

                // change document                      
                _activeDoc.Refresh();
            }
            else if (_wndProperty.SelectedType == SelectedObjectType.Glass)   // change glass information           
                _activeDoc = _wndProperty.DocumentArea;
            else if (_wndProperty.SelectedType == SelectedObjectType.Bat)
                FileChangeBat();
        }

        #endregion Called from Property window

        #endregion Relationships between windows

        /// <summary>
        /// Form Closing handle
        /// </summary>        
        private void frmMokkAnnotator_FormClosing(object sender, FormClosingEventArgs e)
        {
            // save application settings
            MkaDefine.SaveAppSettings();

            // save settings
            SaveSettingsToRegistry();

            // save state of dock panel 
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            mkaDockPanel.SaveAsXml(configFile);
        }

        /// <summary>
        /// Popup menu item (File, Edit ...) is opened.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMokkAnnotator_DropDownOpened(object sender, EventArgs e)
        {
            // Reset active tool to pointer.
            // This prevents bug in rare case when non-pointer tool is active, user opens
            // main main menu and after this clicks in the m_docArea. MouseDown event is not
            // raised in this case (why ??), and MouseMove event works incorrectly.
            _activeTool = ToolType.Pointer;
            if (_activeDoc != null)
                _activeDoc.ActiveTool = _activeTool;
            _wndToolbar.Select(_activeTool);
        }

        private void mkaNaviBar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            _activeTool = ToolType.Pointer;
            if (_activeDoc != null)
                _activeDoc.ActiveTool = _activeTool;
            _wndToolbar.Select(_activeTool);
        }

        private void mkaContextMenu_Opened(object sender, EventArgs e)
        {
            _activeTool = ToolType.Pointer;
            if (_activeDoc != null)
                _activeDoc.ActiveTool = _activeTool;
            _wndToolbar.Select(_activeTool);
        }

        #endregion Initialize

        #region File

        #region Bat file

        /// <summary>
        /// Menu File New Bat
        /// </summary>        
        private void mniFileNewBat_Click(object sender, EventArgs e)
        {
            MkaFrmNewBat frm = new MkaFrmNewBat(this);
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;

            MkaBatInfo.LastBat = frm.BatInfo;
            _batManager = frm.BatManager;
            _wndExplorer.Explore(_batManager);

            _batManager.Save();
        }

        /// <summary>
        /// Menu File Open Bat
        /// </summary>        
        private void mniFileOpenBat_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = MkaDefine.BmkFilter;
            ofd.Multiselect = false;

            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;

            FileOpenBat(ofd.FileName);
            // update working directory
            DirectoryInfo wFolder = new DirectoryInfo(ofd.FileName);
            MkaDefine.MokkanPath = wFolder.Parent.Parent.Parent.FullName;
        }

        /// <summary>
        /// Open bat file
        /// </summary>        
        private void FileOpenBat(String filename)
        {
            try
            {
                if (_batManager != null)
                {
                    if (_batManager.BatFilePath == filename)
                        return;
                    else
                        FileCloseBat();
                }

                this.Cursor = Cursors.WaitCursor;

                XmlDocument doc = new XmlDocument();
                IXmlFormatter formatter = FepXmlableFactory.CreateXmlFormatter();
                IXmlContext cnt;
                XmlElement _eleBat;

                // Deserialize object from xml format        
                doc.Load(filename);
                cnt = FepXmlableFactory.CreateXmlContext(formatter, doc);
                doc = cnt.Document;
                _eleBat = doc.DocumentElement;
                _batManager = cnt.FromXml(_eleBat) as MkaBatManager;
                _batManager.Owner = this;
                _batManager.BatInfo.BatDirectory = Path.GetDirectoryName(filename);
                for (int i = 0; i < _batManager.ImageFileNames.Count; i++)
                {
                    _batManager.ImageFilePaths.Add(_batManager.BatInfo.BatDirectory + "\\" + _batManager.ImageFileNames[i]);
                    _batManager.GlassFilePaths.Add(_batManager.BatInfo.BatDirectory + "\\" + Path.GetFileNameWithoutExtension(_batManager.ImageFileNames[i]) + MkaDefine.MkaFileExt);
                }
                MkaBatInfo.LastBat = _batManager.BatInfo;

                _wndExplorer.Explore(_batManager);

                _recentBmkManager.Add(filename);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                MkaMessage.ShowError(MkaMessage.ErrOpenFile);
                _log.Error(MkaMessage.ErrOpenFile, ex);
            }
        }

        #endregion Bat file

        #region Glass file

        /// <summary>
        /// Menu File New Glass
        /// </summary>
        private void mniFileNewGlass_Click(object sender, EventArgs e)
        {
            FileNewGlass();
        }

        /// <summary>
        /// Create new glass document
        /// </summary>
        public void FileNewGlass()
        {
            MkaFrmNewGlass frm = new MkaFrmNewGlass(this);
            if (frm.ShowDialog(this) != DialogResult.OK)
                return;

            MkaDocument doc = frm.NewGlassDoc;

            if (_batManager != null && !_batManager.GlassFilePaths.Contains(doc.GlassInfo.GlassFilePath))
            {
                _batManager.GlassFilePaths.Add(doc.FileFullName);
                _batManager.ImageFilePaths.Add(doc.GlassInfo.ImageFilePath);
                _batManager.Save();
                _wndExplorer.Explore(_batManager);
            }

            _activeDoc = FileGetDoc(doc.FileFullName);
            if (_activeDoc == null)    // being not opened now
            {
                // open document
                doc.Show(mkaDockPanel);
                doc.ZoomToWindow();
            }
            else  // overwrite
            {
                _activeDoc.DockHandler.Activate();
                _activeDoc.ReNew(doc.GlassInfo);
            }

            _wndMokkan.MokkanList = _activeDoc.MokkanList;
        }

        /// <summary>
        /// Menu File Open Glass
        /// </summary>
        private void mniFileOpenGlass_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = MkaDefine.MkaFilter;
            ofd.Multiselect = true;

            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;

            FileOpenGlass(ofd.FileNames);
            // update working directory
            DirectoryInfo wFolder = new DirectoryInfo(ofd.FileName);
            MkaDefine.MokkanPath = wFolder.Parent.Parent.Parent.FullName;      
        }

        /// <summary>
        /// Menu File Add Glass
        /// </summary>        
        private void mniFileAddGlass_Click(object sender, EventArgs e)
        {
            FileAddGlass();
        }

        /// <summary>
        /// Add glass documents into bat
        /// </summary>
        public void FileAddGlass()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = MkaDefine.MkaFilter;
            ofd.Multiselect = true;

            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;

            foreach (String filename in ofd.FileNames)
            {
                if (!_batManager.GlassFilePaths.Contains(filename))
                {
                    MkaDocument doc = FileLoadGlass(filename);
                    if (doc != null)
                    {
                        _batManager.GlassFilePaths.Add(filename);
                        _batManager.ImageFilePaths.Add(doc.GlassInfo.ImageFilePath);
                    }
                }
            }

            _batManager.Save();
            _wndExplorer.Explore(_batManager);
        }

        /// <summary>
        /// Menu File Add Image
        /// </summary> 
        private void mniFileAddImage_Click(object sender, EventArgs e)
        {
            FileAddImage();
        }

        /// <summary>
        /// Add glass image into bat
        /// </summary>
        public void FileAddImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = MkaDefine.IOImageFilterAll + "|" + MkaDefine.IOImageFilter;
            ofd.Multiselect = true;

            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;
            foreach (String filename in ofd.FileNames)
            {
                if (!_batManager.ImageFilePaths.Contains(filename))
                {
                    _batManager.ImageFilePaths.Add(filename);
                    String glassFile = Path.GetDirectoryName(filename) + "\\" + Path.GetFileNameWithoutExtension(filename) + MkaDefine.MkaFileExt;
                    MkaDocument doc = FileLoadGlass(glassFile);
                    if (doc == null)
                        FileNewGlassInBat(glassFile);
                    _batManager.GlassFilePaths.Add(glassFile);
                    _batManager.ImageFilePaths.Add(filename);
                }
            }

            _batManager.Save();
            _wndExplorer.Explore(_batManager);
        }

        /// <summary>
        /// File - Open a glass file
        /// </summary>  
        /// <returns>true if load existed file</returns>
        public void FileOpenGlass(String filename)
        {
            // show glass file already opened            
            if (FileShowGlass(filename))
                return;
            // load file
            else if (File.Exists(filename))
            {
                _activeDoc = FileLoadGlass(filename);
                if (_activeDoc != null)
                {
                    // display on tab panel
                    _activeDoc.Show(mkaDockPanel);
                    _activeDoc.ZoomToWindow();
                }
                return;
            }

            _activeDoc = FileNewGlassInBat(filename);
            _activeDoc.Show(mkaDockPanel);
            _activeDoc.ZoomToWindow();
        }

        /// <summary>
        /// Create new glass file in bat folder
        /// </summary>        
        private MkaDocument FileNewGlassInBat(String filename)
        {
            // create new glass document            
            MkaDocument doc = new MkaDocument(this);
            doc.GlassInfo = new MkaGlassInfo();
            doc.GlassInfo.BatInfo = _batManager.BatInfo;
            doc.GlassInfo.GlassFilePath = filename;
            doc.GlassInfo.GlassItaBangou = Path.GetFileNameWithoutExtension(filename);
            doc.GlassInfo.KaishiRBangou = MkaMokkanInfo.LastRBangou;
            doc.GlassImage = _batManager.GetImagePath(filename);
            doc.Text = Path.GetFileName(filename);
            doc.Save(true);

            return doc;
        }

        /// <summary>
        /// Show existed glass file on tab panel
        /// </summary>        
        private bool FileShowGlass(String filename)
        {
            MkaDocument doc = FileGetDoc(filename);
            if (doc == null) return false;

            doc.ReOpen = true;
            doc.DockHandler.Activate();

            _recentMkgManager.Add(filename);

            return true;
        }

        /// <summary>
        /// Load glass information from xml file
        /// </summary>
        /// <param name="filename"></param>
        private MkaDocument FileLoadGlass(String filename)
        {
            if (!File.Exists(filename)) return null;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);

                // Deserialize object from xml format                    
                IXmlFormatter formatter = FepXmlableFactory.CreateXmlFormatter();
                IXmlContext cnt = FepXmlableFactory.CreateXmlContext(formatter, doc);

                XmlElement _eleBat = cnt.Document.DocumentElement;
                XmlNode _eleGlass = _eleBat.FirstChild;
                XmlNode _eleMk = _eleGlass.FirstChild;
                MkaDocument mkaDoc = new MkaDocument(this);
                mkaDoc.Text = Path.GetFileName(filename);
                mkaDoc.GlassInfo = cnt.FromXml(_eleGlass as XmlElement) as MkaGlassInfo;
                mkaDoc.BatInfo = cnt.FromXml(_eleBat as XmlElement) as MkaBatInfo;
                mkaDoc.MokkanList = cnt.FromXml(_eleMk as XmlElement) as MokkanList;
                mkaDoc.BatInfo.BatDirectory = Path.GetDirectoryName(filename);
                mkaDoc.GlassInfo.GlassFilePath = filename;
                mkaDoc.GlassInfo.MokkanKazu = mkaDoc.MokkanList.Count;
                mkaDoc.GlassImage = Path.GetDirectoryName(filename) + "\\" + mkaDoc.GlassInfo.ImageFileName;

                int maxRid = mkaDoc.MokkanList.GetMaxRBangou() + 1;
                if (maxRid == 1)
                    MkaMokkanInfo.LastRBangou = mkaDoc.GlassInfo.KaishiRBangou;
                else
                    MkaMokkanInfo.LastRBangou = maxRid;
                mkaDoc.MokkanList.KaishiRBangou = MkaMokkanInfo.LastRBangou;

                _recentMkgManager.Add(filename);

                return mkaDoc;
            }
            catch (Exception ex)
            {
                MkaMessage.ShowError(MkaMessage.ErrOpenFile);
                _log.Error(MkaMessage.ErrOpenFile, ex);

                return null;
            }
        }

        /// <summary>
        /// File - Open multiple glass files
        /// </summary>        
        private void FileOpenGlass(String[] files)
        {
            foreach (String file in files)
                FileOpenGlass(file);
        }

        /// <summary>
        /// Get glass document from document list
        /// </summary>
        private MkaDocument FileGetDoc(String filename)
        {
            IEnumerator docEnum = mkaDockPanel.Documents.GetEnumerator();
            MkaDocument doc;
            while (docEnum.MoveNext())
            {
                doc = (MkaDocument)docEnum.Current;
                if (doc.GlassInfo.GlassFilePath == filename)
                    return doc;
            }

            return null;
        }
        #endregion Glass file

        #region Save

        /// <summary>
        /// Menu File Save
        /// </summary>
        private void mniFileSave_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            _activeDoc.Save(true);

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Menu File Save As
        /// </summary>
        private void mniFileSaveAs_Click(object sender, EventArgs e)
        {
            _activeDoc.SaveAs();
        }

        /// <summary>
        /// Menu File Save All
        /// </summary>        
        private void mniFileSaveAll_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            IEnumerator docEnum = mkaDockPanel.Documents.GetEnumerator();
            MkaDocument doc;
            while (docEnum.MoveNext())
            {
                doc = (MkaDocument)docEnum.Current;
                doc.Save(true);
            }

            this.Cursor = Cursors.Default;
        }

        #endregion Save

        #region Export

        /// <summary>
        /// Menu File Export To CSV
        /// </summary>
        private void mniFileExportCsv_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            _activeDoc.ExportToCSV();

            this.Cursor = Cursors.Default;
        }

        #endregion Export

        #region Print

        /// <summary>
        /// Print image only
        /// </summary>        
        private void mniFilePrintImage_Click(object sender, EventArgs e)
        {
            mniFilePrintImage.Checked = true;
            mniFilePrintAnnotation.Checked = false;
            mniFilePrintAll.Checked = false;

            _print.PrintGlassImage = true;
            _print.PrintMokkanInfo = false;
            _print.PrintGlassInfo = false;
        }

        /// <summary>
        /// Print image with annotation
        /// </summary>        
        private void mniFilePrintAnnotation_Click(object sender, EventArgs e)
        {
            mniFilePrintImage.Checked = false;
            mniFilePrintAnnotation.Checked = true;
            mniFilePrintAll.Checked = false;

            _print.PrintGlassImage = false;
            _print.PrintMokkanInfo = true;
            _print.PrintGlassInfo = false;
        }

        /// <summary>
        /// Print all
        /// </summary>        
        private void mniFilePrintAll_Click(object sender, EventArgs e)
        {
            mniFilePrintImage.Checked = false;
            mniFilePrintAnnotation.Checked = false;
            mniFilePrintAll.Checked = true;

            _print.PrintGlassImage = false;
            _print.PrintMokkanInfo = false;
            _print.PrintGlassInfo = true;
        }

        /// <summary>
        /// Menu File Config Page
        /// </summary>
        private void mniFileConfigPage_Click(object sender, EventArgs e)
        {
            if (_print.ShowPageSetupDialog(this) == DialogResult.OK)
            {
            }
        }

        /// <summary>
        /// Menu File Print Preview
        /// </summary>
        private void mniFilePrintPreview_Click(object sender, EventArgs e)
        {
            _print.DocumentArea = _activeDoc;
            if (_print.ShowPreviewDialog(this) == DialogResult.OK)
            {
            }
        }

        /// <summary>
        /// Menu File Print
        /// </summary>
        private void mniFilePrint_Click(object sender, EventArgs e)
        {
            _print.DocumentArea = _activeDoc;
            if (_print.Print(this) == DialogResult.OK)
            {
            }
        }

        #endregion Print

        #region Close

        /// <summary>
        /// Menu File Close Bat
        /// </summary>        
        private void mniFileCloseBat_Click(object sender, EventArgs e)
        {
            FileCloseBat();
        }

        /// <summary>
        /// Close active bat
        /// </summary>
        public void FileCloseBat()
        {
            if (_batManager != null)
            {
                while (mkaDockPanel.ActiveDocument != null)
                    mkaDockPanel.ActiveDocument.DockHandler.Close();
                _batManager = null;
                _wndExplorer.Clear();
                _wndMokkan.Clear();
                _wndProperty.Clear();
                SetCaption(String.Empty);
            }
        }

        /// <summary>
        /// Menu File Close Glass
        /// </summary>
        private void mniFileCloseGlass_Click(object sender, EventArgs e)
        {
            if (mkaDockPanel.ActiveDocument != null)
                mkaDockPanel.ActiveDocument.DockHandler.Close();
        }

        /// <summary>
        /// Remove glass document from bat
        /// </summary>        
        public void FileRemoveGlass(String glassName)
        {
            _batManager.GlassFilePaths.Remove(glassName);
            _batManager.ImageFilePaths.RemoveAll(delegate(String s) { return s.Contains(Path.GetFileNameWithoutExtension(glassName) + "."); });
            _batManager.Save();

            MkaDocument doc = FileGetDoc(glassName);
            if (doc != null)
                doc.DockHandler.Close();
        }

        /// <summary>
        /// Menu File Close Application
        /// </summary>
        private void mniFileCloseApp_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion Close

        /// <summary>
        /// Change bat information
        /// </summary>        
        public void FileChangeBat()
        {
            foreach (String glassName in _batManager.GlassFilePaths)
            {
                MkaDocument doc = FileGetDoc(glassName);
                if (doc == null)
                    doc = FileLoadGlass(glassName);

                if (doc != null)
                {
                    doc.BatInfo = _batManager.BatInfo;
                    doc.Save(false);
                }
            }
            _batManager.Save();
        }

        /// <summary>
        /// Change bat name
        /// </summary>        
        public void FileChangeBatName(String newName)
        {
            File.Delete(_batManager.BatFilePath);

            _batManager.BatInfo.BatBangou = newName;
            foreach (String glassName in _batManager.GlassFilePaths)
            {
                MkaDocument doc = FileGetDoc(glassName);
                if (doc == null)
                    doc = FileLoadGlass(glassName);

                if (doc != null)
                {
                    doc.BatInfo.BatBangou = newName;
                    doc.Save(false);
                }
            }

            _batManager.Save();
        }

        /// <summary>
        /// Change glass file name
        /// </summary>
        public void FileChangeGlassName(String oldName, String newName)
        {
            String glassOldName = _batManager.BatDirectory + "\\" + oldName + MkaDefine.MkaFileExt;
            String glassNewName = _batManager.BatDirectory + "\\" + newName + MkaDefine.MkaFileExt;
            MkaDocument mkaDoc = FileGetDoc(glassOldName);
            if (mkaDoc == null)
                mkaDoc = FileLoadGlass(glassOldName);
            if (mkaDoc == null)
            {
                String oldImageName = _batManager.ImageFilePaths.Find(delegate(String s) { return s.Contains(oldName + "."); });
                String newImageName = Path.GetDirectoryName(oldImageName) + "\\" + newName + Path.GetExtension(oldImageName);
                FileChangeImageName(oldImageName, newImageName);
                return;
            }
            mkaDoc.GlassInfo.ChangeGlassName(glassNewName);
            mkaDoc.GlassImage = mkaDoc.GlassInfo.ImageFilePath;
            mkaDoc.UpdateCaption();
            mkaDoc.Save(true);

            // remove old file
            _batManager.GlassFilePaths.Remove(glassOldName);
            _batManager.ImageFilePaths.RemoveAll(delegate(String s) { return s.Contains(Path.GetFileNameWithoutExtension(glassOldName) + "."); });

            // add new file                        
            _batManager.GlassFilePaths.Add(glassNewName);
            _batManager.ImageFilePaths.Add(mkaDoc.GlassInfo.ImageFilePath);

            _batManager.Save();
        }

        /// <summary>
        /// Change image filename
        /// </summary>        
        public void FileChangeImageName(String oldName, String newName)
        {
            File.Move(oldName, newName);

            // remove old file
            _batManager.GlassFilePaths.RemoveAll(delegate(String s) { return s.Contains(Path.GetFileNameWithoutExtension(oldName) + MkaDefine.MkaFileExt); });
            _batManager.ImageFilePaths.Remove(oldName);

            // add new file                        
            _batManager.GlassFilePaths.Add(Path.GetDirectoryName(newName) + "\\" + Path.GetFileNameWithoutExtension(newName) + MkaDefine.MkaFileExt);
            _batManager.ImageFilePaths.Add(newName);

            _batManager.Save();
        }

        #endregion File

        #region Edit

        /// <summary>
        /// Menu Edit Undo
        /// </summary>
        private void mniEditUndo_Click(object sender, EventArgs e)
        {
            _activeDoc.Undo();
        }

        /// <summary>
        /// Menu Edit Redo
        /// </summary>        
        private void mniEditRedo_Click(object sender, EventArgs e)
        {
            _activeDoc.Redo();
        }

        /// <summary>
        /// Menu Edit Cut
        /// </summary>        
        private void mniEditCut_Click(object sender, EventArgs e)
        {
            _activeDoc.Cut();
        }

        /// <summary>
        /// Menu Edit Copy
        /// </summary>        
        private void mniEditCopy_Click(object sender, EventArgs e)
        {
            _activeDoc.Copy();
        }

        /// <summary>
        /// Menu Edit Paste
        /// </summary>        
        private void mniEditPaste_Click(object sender, EventArgs e)
        {
            _activeDoc.Paste();
        }

        /// <summary>
        /// Menu Edit Delete
        /// </summary>        
        private void mniEditDelete_Click(object sender, EventArgs e)
        {
            _activeDoc.Delete();
        }

        /// <summary>
        /// Menu Edit Delete All
        /// </summary>        
        private void mniEditDeleteAll_Click(object sender, EventArgs e)
        {
            _activeDoc.DeleteAll();
        }

        /// <summary>
        /// Menu Edit Move Front
        /// </summary>        
        private void mniEditMoveFront_Click(object sender, EventArgs e)
        {
            _activeDoc.MoveFront();
        }

        /// <summary>
        /// Menu Edit Move Back
        /// </summary>        
        private void mniEditMoveBack_Click(object sender, EventArgs e)
        {
            _activeDoc.MoveBack();
        }

        /// <summary>
        /// Menu Edit Select All
        /// </summary>        
        private void mniEditSelectAll_Click(object sender, EventArgs e)
        {
            _activeDoc.SelectAll();
        }

        /// <summary>
        /// Menu Edit Unselect
        /// </summary>        
        private void mniEditUnselect_Click(object sender, EventArgs e)
        {
            _activeDoc.UnSelect();
        }

        /// <summary>
        /// Menu Edit Property
        /// </summary>        
        private void mniEditProperty_Click(object sender, EventArgs e)
        {
            ShowPropertyDialog();
        }

        /// <summary>
        /// Show property dialog
        /// </summary>
        public void ShowPropertyDialog()
        {
            if (_activeDoc != null)
            {
                _wndProperty.SelectDoc(_activeDoc);
                _activeDoc.ShowPropertyDialog();
                _wndProperty.SelectDoc(_activeDoc);
            }
            else if (_batManager != null)
            {
                _wndProperty.SelectBat(_batManager);
                _batManager.ShowPropertyDialog();
                _wndProperty.SelectBat(_batManager);
            }
        }

        #endregion Edit

        #region View

        #region Zoom

        /// <summary>
        /// Menu View Zoom
        /// </summary>     
        private void mniViewZoom_Click(object sender, EventArgs e)
        {
            // selected item
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            String fName = item.Name.Substring(3);

            ViewZoom(fName);
        }

        private void tsbtnViewZoom_Click(object sender, EventArgs e)
        {
            // selected item
            ToolStripButton item = (ToolStripButton)sender;
            String fName = item.Name.Substring(5);

            ViewZoom(fName);
        }

        private void ViewZoom(String fName)
        {
            switch (fName)
            {
                case "ViewZoomIn":
                    this._activeDoc.ZoomIn();   // Zoom in
                    break;
                case "ViewZoomOut":
                    this._activeDoc.ZoomOut();  // Zoom out
                    break;
                case "ViewZoomWindow":
                    this._activeDoc.ZoomToWindow();   // Fix to window size
                    break;
                case "ViewZoomActual":
                    this._activeDoc.ZoomToActual();   // Display in original size
                    break;
            }
        }

        /// <summary>
        /// Zoom by select specified ratio
        /// </summary>        
        private void tscmbZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get zoom ratio from selection
            _zoomRatio = Convert.ToInt16(tscmbZoom.Text.Replace("%", ""));

            // execute zoom
            this._activeDoc.ZoomImage((float)_zoomRatio / 100);
        }

        /// <summary>
        /// Digit and % only are possible
        /// </summary>
        private void tscmbZoom_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ("0123456789\b".IndexOf(e.KeyChar) == -1)
                e.Handled = true;
        }

        /// <summary>
        /// Zoom by input ratio
        /// </summary>
        private void tscmbZoom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            //　get zoom ration from input area
            tscmbZoom.Text = tscmbZoom.Text.Replace("%", "");
            if (tscmbZoom.Text == "")
            {
                tscmbZoom.Text = _zoomRatio.ToString() + "%";
                MessageBox.Show("Zoom " + tscmbZoom.Text);
                return;
            }

            _zoomRatio = Convert.ToInt16(tscmbZoom.Text);

            // maximum 1000%
            if (_zoomRatio > 1000)
            {
                tscmbZoom.Text = "1000%";
                _zoomRatio = 1000;
            }
            else
            {
                tscmbZoom.Text = _zoomRatio.ToString() + "%";
            }
            tscmbZoom.Focus();

            // execute zoom
            this._activeDoc.ZoomImage((float)_zoomRatio / 100);
        }
        #endregion Zoom

        #region Display

        /// <summary>
        /// Display Menu
        /// </summary>        
        private void mniViewDisplay_Click(object sender, EventArgs e)
        {
            // selected item
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            String fName = item.Name.Substring(3);

            if (fName == "ViewRId")
                tsbtnViewRId.Checked = mniViewRId.Checked;

            ViewDisplay(fName);
        }

        private void tsbtnViewDisplay_Click(object sender, EventArgs e)
        {
            // selected item
            ToolStripButton item = (ToolStripButton)sender;
            String fName = item.Name.Substring(5);

            if (fName == "ViewRId")
                mniViewRId.Checked = tsbtnViewRId.Checked;

            ViewDisplay(fName);
        }

        private void ViewDisplay(String fName)
        {
            // uncheck all item
            mniViewAll.Checked = false;
            tsbtnViewAll.Checked = false;
            mniViewImageOnly.Checked = false;
            tsbtnViewImageOnly.Checked = false;
            mniViewDataOnly.Checked = false;
            tsbtnViewDataOnly.Checked = false;

            switch (fName)
            {
                case "ViewAll":
                    mniViewAll.Checked = true;
                    tsbtnViewAll.Checked = true;
                    _displayType = DisplayType.All;
                    break;
                case "ViewImageOnly":
                    mniViewImageOnly.Checked = true;
                    tsbtnViewImageOnly.Checked = true;
                    _displayType = DisplayType.ImageOnly;
                    break;
                case "ViewDataOnly":
                    mniViewDataOnly.Checked = true;
                    tsbtnViewDataOnly.Checked = true;
                    _displayType = DisplayType.DataOnly;
                    break;
                case "ViewRId":
                    _activeDoc.RShow(mniViewRId.Checked);
                    break;
            }

            // update display type
            _activeDoc.ActiveDisplay = _displayType;
            _activeDoc.Invalidate();
        }

        #endregion Display

        #endregion View

        #region Image

        private void mniImageRestore_Click(object sender, EventArgs e)
        {
            _activeDoc.ImageRestore();
        }

        private void mniImageBrightnessContrast_Click(object sender, EventArgs e)
        {
            Bitmap previewImg = _activeDoc.GetPreviewImage();
            MkaAdjustBrightnessContrast frm = new MkaAdjustBrightnessContrast(previewImg);
            if (frm.ShowDialog(this) != DialogResult.OK)
            {
                previewImg.Dispose();
                return;
            }

            if (frm.Brightness != 0 || frm.Contrast != 0)
                _activeDoc.ImageAdjustBrightnessContrast(frm.Brightness, frm.Contrast);
            previewImg.Dispose();
        }

        private void mniImageAdjustHSL_Click(object sender, EventArgs e)
        {
            Bitmap previewImg = _activeDoc.GetPreviewImage();
            MkaAdjustHSL frm = new MkaAdjustHSL(previewImg);
            if (frm.ShowDialog(this) != DialogResult.OK)
            {
                previewImg.Dispose();
                return;
            }

            if (frm.Hue != 0 || frm.Saturation != 100 || frm.Lightness != 0)
                _activeDoc.ImageAdjustHSL(frm.Hue, frm.Saturation, frm.Lightness);
            previewImg.Dispose();
        }

        private void mniImageMirror_Click(object sender, EventArgs e)
        {
            _activeDoc.RotateMirror();
        }

        private void mniImageFlip_Click(object sender, EventArgs e)
        {
            _activeDoc.RotateFlip();
        }

        private void mniImageRotateRight_Click(object sender, EventArgs e)
        {
            _activeDoc.RotateRight();
        }

        private void mniImageRotateLeft_Click(object sender, EventArgs e)
        {
            _activeDoc.RotateLeft();
        }

        private void mniImageRotate180_Click(object sender, EventArgs e)
        {
            _activeDoc.Rotate180();
        }

        private void mniImageRotate_Click(object sender, EventArgs e)
        {
            MkaImageRotate frm = new MkaImageRotate();
            if (frm.ShowDialog(this) != DialogResult.OK) return;

            if (frm.Angle > 0)
                _activeDoc.Rotate(frm.Angle, frm.FillColor, frm.RotateType);
        }

        #endregion Image

        #region Annotation

        /// <summary>
        /// Auto Select Mokkan
        /// </summary>        
        private void mniMokkanAutoSelect_Click(object sender, EventArgs e)
        {
            MkaAutoExtractMokkan extractMok = new MkaAutoExtractMokkan(_activeDoc.Image);
            if (extractMok.ShowDialog(this) != DialogResult.OK) return;

            _activeDoc.AutoSelect(extractMok.MokkanList);
        }

        /// <summary>
        /// Auto Label Mokkan
        /// </summary>        
        private void mniMokkanAutoLabel_Click(object sender, EventArgs e)
        {
            _activeDoc.AutoLabel();
        }

        /// <summary>
        /// Decode Mokkan
        /// </summary>        
        private void mniMokkanDecode_Click(object sender, EventArgs e)
        {
            MkaMokkanDecode frm;
            if (_activeDoc == null || _activeDoc.MokkanList.SelectionCount != 1)
            {
                frm = new MkaMokkanDecode();
                frm.ShowDialog(this);
                return;
            }

            int rid;
            String path = _activeDoc.GlassInfo.GlassFilePath;
            Bitmap image = _activeDoc.GetMokkanToDecode(out rid);
            frm = new MkaMokkanDecode(image, rid, path);
            frm.ShowDialog(this);
            image.Dispose();
        }

        /// <summary>
        /// Save last state of parent form to be restored from Search form
        /// </summary>
        private FormWindowState _lastState;

        /// <summary>
        /// Search Mokkan
        /// </summary>            
        private void mniMokkanSearch_Click(object sender, EventArgs e)
        {
            _frmSearch = new MkaFrmSearch();
            _frmSearch.OpenSelectedEvent += new OpenSelectedEventHandler(this.SearchSelectChange);
            _frmSearch.WindowState = FormWindowState.Normal;
            _lastState = this.WindowState;
            _frmSearch.Show();
        }

        /// <summary>
        /// Display glass file and select specified mokkan
        /// </summary>
        /// <param name="e"></param>
        private void SearchSelectChange(OpenSelectedEventArgs e)
        {
            this.WindowState = _lastState;
            FileOpenGlass(e.FileName);
            _activeDoc.MokkanList.UnselectAll();
            _activeDoc.MokkanList[e.RId].Selected = true;
            _activeDoc.Invalidate();
        }

        /// <summary>
        /// Customize Settings
        /// </summary>        
        private void mniMokkanSettings_Click(object sender, EventArgs e)
        {
            MkaWndSettings _wndSettings = new MkaWndSettings(this);

            // Get current property
            _wndSettings.mkaGraphicsProperties.Properties = GraphicsProperties.LastProperties;
           
            if (_wndSettings.ShowDialog(this) == DialogResult.OK)
                SaveSettingsToRegistry();
        }

        #endregion Annotation

        #region Windows

        /// <summary>
        /// Hide/Show Explorer Window
        /// </summary>        
        private void mniWindowExplorer_Click(object sender, EventArgs e)
        {
            _wndExplorer.Show(mkaDockPanel, DockState.DockLeftAutoHide);
        }

        /// <summary>
        /// Hide/Show Annotator Window
        /// </summary>        
        private void mniWindowToolbar_Click(object sender, EventArgs e)
        {

            _wndToolbar.Show(mkaDockPanel, DockState.DockLeftAutoHide);
        }

        /// <summary>
        /// Hide/Show Property Window
        /// </summary>        
        private void mniWindowProperty_Click(object sender, EventArgs e)
        {
            _wndProperty.Show(mkaDockPanel, DockState.DockRightAutoHide);
        }

        /// <summary>
        /// Hide/Show Mokkan List Window
        /// </summary>        
        private void mniWindowMokkan_Click(object sender, EventArgs e)
        {
            this._wndMokkan.Show(mkaDockPanel, DockState.DockBottomAutoHide);
        }

        #endregion Windows

        #region Help

        /// <summary>
        /// Help Content
        /// </summary>        
        private void mniHelpContent_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Show Shortcut List
        /// </summary>        
        private void mniHelpShortcut_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// About application
        /// </summary>        
        private void mniHelpVersion_Click(object sender, EventArgs e)
        {
            // Show About dialog
            mkaFrmSelect about = new mkaFrmSelect();
            about.ShowDialog(this);
        }

        #endregion Help

        #region Other Functions

        /// <summary>
        /// Set form caption
        /// </summary>
        public void SetCaption(String filename)
        {
            if (filename == "")
                this.Text = Application.ProductName;
            else
                this.Text = Application.ProductName + " - " + filename;
        }

        /// <summary>
        /// Active document change
        /// </summary>        
        private void mkaDockPanel_ActiveDocumentChanged(object sender, EventArgs e)
        {
            _activeDoc = (MkaDocument)mkaDockPanel.ActiveDocument;
            if (_activeDoc != null)
            {
                _activeDoc.UpdateZoom();
                _activeDoc.UpdateCaption();

                // update display type
                _activeDoc.ActiveDisplay = _displayType;
                _activeDoc.Invalidate();

                DocAreaDataChange();
            }
            else
            {
                if (_batManager == null)
                {
                    _wndMokkan.Clear();
                    if (_wndProperty != null)
                        _wndProperty.Clear();
                }
                SetCaption(String.Empty);
            }
        }

        /// <summary>
        /// Set state of file controls
        /// </summary>
        public void SetStateOfFile()
        {
            bool active = (_activeDoc != null);
            bool dirty = active && _activeDoc.Dirty;

            this.mniFileAdd.Enabled = (_batManager != null);
            this.mniFileExport.Enabled = active;
            this.mniFilePrintPreview.Enabled = active;
            this.tsbtnPreview.Enabled = active;
            this.mniFilePrint.Enabled = active;
            this.tsbtnPrint.Enabled = active;
            this.mniFileSave.Enabled = dirty;
            this.tsbtnSave.Enabled = dirty;
            this.mniFileSaveAs.Enabled = active;
        }

        /// <summary>
        /// Set state of view and edit controls.
        /// Function is called at idle time.
        /// </summary>
        public void SetStateOfViewEdit()
        {
            bool active, imageObject, objects, selectedObjects, copiedObjects;

            active = (_activeDoc != null);
            imageObject = active && _activeDoc.Image != null && _activeDoc.ActiveDisplay != DisplayType.DataOnly;
            objects = active && _activeDoc.MokkanList.Count > 0 && _activeDoc.ActiveDisplay != DisplayType.ImageOnly;
            selectedObjects = active && _activeDoc.MokkanList.SelectionCount > 0 && _activeDoc.ActiveDisplay != DisplayType.ImageOnly;
            copiedObjects = active && _activeDoc.CopyCount > 0 && _activeDoc.ActiveDisplay != DisplayType.ImageOnly;

            // Edit operations
            this.mniEditCut.Enabled = selectedObjects;
            this.tsbtnCut.Enabled = selectedObjects;
            this.cmniEditCut.Enabled = selectedObjects;

            this.mniEditCopy.Enabled = selectedObjects;
            this.tsbtnCopy.Enabled = selectedObjects;
            this.cmniEditCopy.Enabled = selectedObjects;

            this.mniEditPaste.Enabled = copiedObjects;
            this.tsbtnPaste.Enabled = copiedObjects;
            this.cmniEditPaste.Enabled = copiedObjects;

            this.mniEditDelete.Enabled = selectedObjects;
            this.cmniEditDelete.Enabled = selectedObjects;

            this.mniEditDeleteAll.Enabled = objects;
            this.cmniEditDeleteAll.Enabled = objects;

            this.mniEditSelectAll.Enabled = objects;
            this.cmniEditSelectAll.Enabled = objects;

            this.mniEditUnselect.Enabled = objects;
            this.cmniEditUnselect.Enabled = objects;

            this.mniEditMoveFront.Enabled = selectedObjects;
            this.cmniEditMoveFront.Enabled = selectedObjects;

            this.mniEditMoveBack.Enabled = selectedObjects;
            this.cmniEditMoveBack.Enabled = selectedObjects;

            this.mniEditProperty.Enabled = selectedObjects || active || _batManager != null;
            this.cmniEditProperty.Enabled = selectedObjects || active || _batManager != null;

            // Undo, Redo
            this.mniEditUndo.Enabled = active && _activeDoc.CanUndo;
            this.tssbtnUndo.Enabled = active && _activeDoc.CanUndo;

            this.mniEditRedo.Enabled = active && _activeDoc.CanRedo;
            this.tssbtnRedo.Enabled = active && _activeDoc.CanRedo;

            // View operations
            foreach (ToolStripItem item in mniView.DropDownItems)
                item.Enabled = active;
            this.tsbtnViewAll.Enabled = active;
            this.tsbtnViewDataOnly.Enabled = active;
            this.tsbtnViewImageOnly.Enabled = active;
            this.tsbtnViewRId.Enabled = objects;
            this.tsbtnViewZoomActual.Enabled = active;
            this.tsbtnViewZoomIn.Enabled = active;
            this.tsbtnViewZoomOut.Enabled = active;
            this.tsbtnViewZoomWindow.Enabled = active;
            this.tscmbZoom.Enabled = active;

            // Image operations
            foreach (ToolStripItem item in mniImage.DropDownItems)
                item.Enabled = imageObject;

            // Annotation operations
            this.mniMokkanAutoLabel.Enabled = objects;
            this.mniMokkanAutoSelect.Enabled = imageObject;
            this.cmniMokkanDecode.Enabled = active && _activeDoc.MokkanList.SelectionCount == 1 && _activeDoc.ActiveDisplay == DisplayType.All;
        }

        /// <summary>
        /// Load application settings from the Registry
        /// </summary>
        private void LoadSettingsFromRegistry()
        {
            GraphicsProperties.LoadFromRegistry();
            MkaBatInfo.LoadFromRegistry();
            MkaMokkanInfo.LoadFromRegistry();
        }

        /// <summary>
        /// Save application settings to the Registry
        /// </summary>
        private void SaveSettingsToRegistry()
        {
            GraphicsProperties.SaveToRegistry();
            MkaBatInfo.SaveToRegistry();
            MkaMokkanInfo.SaveToRegistry();
        }

        #endregion

        /// <summary>
        /// Save all glasses to excel file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mniFileExportExcel_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            IEnumerator docEnum = mkaDockPanel.Documents.GetEnumerator();
            //convert from IEnumerator to list and reverse
            List<MkaDocument> docs = new List<MkaDocument>();
            while (docEnum.MoveNext())
            {
                docs.Add((MkaDocument)docEnum.Current);
            }

            //no document is opened
            if (docs.Count() == 0)
            {
                MessageBox.Show("ガラスが開かれていない！", "エラー",MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                // get last accessed path
                RegistryKey key = MkaDefine.RootKey.OpenSubKey(MkaDefine.RegKey);
                String path = (String)key.GetValue(MkaDefine.RecentBatPath);

                // save to excel file dialog
                SaveFileDialog sfd = new SaveFileDialog();
                if (Directory.Exists(path))
                    sfd.InitialDirectory = path;
                sfd.Filter = MkaDefine.IOEXCELFilter;
                sfd.FileName = _batManager.BatInfo.BatBangou + MkaDefine.IOEXCELFileExt;

                // get file name
                if (sfd.ShowDialog() != DialogResult.OK)
                    return;
                String fileName = sfd.FileName;

                Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

                if (excel == null)
                {
                    MessageBox.Show("エクセルが正しくインストールされていない！", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                // excel.DisplayAlerts = false;
                excel.Visible = false;
                excel.ScreenUpdating = false;

                // workbook and worksheet for excel file
                object misValue = System.Reflection.Missing.Value;
                Excel.Workbooks workbooks = excel.Workbooks;
                Excel.Workbook workbook = workbooks.Add(misValue);


                //Assign first file for 
                int sheetIndex = 1;

                //active sheet index
                int activeSheet = 0;

                foreach (MkaDocument doc in docs)
                {
                    int numMokkan = doc.MokkanList.Count;

                    if (sheetIndex != 1)
                    {
                        workbook.Sheets.Add(After:workbook.Sheets[workbook.Sheets.Count]);
                    }
                    Excel.Sheets worksheets = workbook.Sheets;

                    Excel.Worksheet worksheet = (Excel.Worksheet)worksheets[sheetIndex];
                    sheetIndex++;

                    worksheet.Name = doc.GlassInfo.GlassFileName;

                    if (doc == _activeDoc)
                    {
                        activeSheet = sheetIndex - 1;
                    }

                    if (numMokkan != 0)
                    {
                        var data = new object[6, numMokkan];

                        //merger first cell for information of batch

                        //first line
                        data[0, 0] = doc.BatInfo.BatBangou;

                        for (int i = 0; i < numMokkan; i++)
                        {
                            // first row
                            data[1, i] = doc.MokkanList[i].MokkanInfo.KariShakubun;

                            // second line
                            data[2, i] = doc.GlassInfo.ChuushouChiku + "|" + doc.GlassInfo.Grid + doc.GlassInfo.Dosoumei;

                            // third line
                            data[3, i] = doc.GlassInfo.BatBangou + "|" + doc.GlassInfo.GlassItaBangou;
                            //xlWorkSheet.Hyperlinks.Add(xlWorkSheet.Cells[5, i + 2], doc.GlassInfo.GlassFilePath);

                            //fourth line
                            data[4, i] = "R" + doc.MokkanList[i].MokkanInfo.RBangou;
                        }

                        // Write this data to the excel worksheet.
                        Excel.Range sheetData = worksheet.Range[worksheet.Cells[2, 2], worksheet.Cells[6, numMokkan + 1]];
                        sheetData.Value2 = data;
                        Marshal.ReleaseComObject(sheetData);

                        //merge first cell 
                        Excel.Range mergeCell = worksheet.Range[worksheet.Cells[2, 2], worksheet.Cells[2, numMokkan + 1]];
                        mergeCell.Merge();
                        worksheet.Hyperlinks.Add(mergeCell, _batManager.BatFilePath);
                        Marshal.ReleaseComObject(mergeCell);

                        //add link and comment for cell
                        for (int i = 0; i < numMokkan; i++)
                        {
                            //comment
                            DrawObject obj = doc.MokkanList.MokkanObjectList[i];
                            Excel.Range cellRange = (Excel.Range)worksheet.Cells[3, i + 2];

                            String directoryImage = Path.GetDirectoryName(doc.FileFullName) + "\\" + Path.GetFileNameWithoutExtension(doc.FileFullName);
                            String pathImage = String.Format("{0}\\{1}_{2}_{3}_{4:00000}.jpg", directoryImage, doc.BatInfo.Grid, doc.BatInfo.BatBangou, doc.GlassInfo.GlassItaBangou, obj.MokkanInfo.RBangou);
                            cellRange.AddComment(" ");
                            if (!File.Exists(pathImage))
                            {
                                Bitmap img = doc.GetImage(obj, false);
                                img.Save(pathImage);
                            }
                            cellRange.Comment.Shape.Fill.UserPicture(pathImage);
                            cellRange.Comment.Shape.Line.Weight = 0;
                            cellRange.Comment.Shape.Fill.BackColor.RGB = ColorTranslator.ToOle(Color.White);
                            cellRange.Comment.Shape.Width = obj.Width;
                            cellRange.Comment.Shape.Height = obj.Height;
                            Marshal.ReleaseComObject(cellRange);
                            // link 
                            worksheet.Hyperlinks.Add(worksheet.Cells[5, i + 2], doc.GlassInfo.GlassFilePath);
                        }

                        Excel.Range verticalFormat = worksheet.get_Range(worksheet.Cells[3, 2], worksheet.Cells[6, numMokkan + 1]);
                        verticalFormat.Orientation = Microsoft.Office.Interop.Excel.XlOrientation.xlVertical;
                        Marshal.ReleaseComObject(verticalFormat);

                        //set column width 
                        Excel.Range rangeFomat = worksheet.get_Range(worksheet.Cells[2, 2], worksheet.Cells[6, numMokkan + 1]);
                        rangeFomat.ColumnWidth = 5;
                        rangeFomat.Interior.Color = ColorTranslator.ToOle(Color.LightGoldenrodYellow);


                        //border
                        Excel.Borders borders = rangeFomat.Borders;
                        borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                        borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                        borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                        borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                        borders.Weight = 3;
                        borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
                        borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
                        borders[Excel.XlBordersIndex.xlInsideVertical].Weight = 1;
                        borders[Excel.XlBordersIndex.xlInsideHorizontal].Weight = 1;
                        borders[Excel.XlBordersIndex.xlDiagonalUp].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                        borders[Excel.XlBordersIndex.xlDiagonalDown].LineStyle = Excel.XlLineStyle.xlLineStyleNone;
                        Marshal.ReleaseComObject(borders);
                        //set no under line 
                        rangeFomat.Font.Underline = false;
                        Marshal.ReleaseComObject(rangeFomat);
                  }
                    Marshal.ReleaseComObject(worksheet);
                    Marshal.ReleaseComObject(worksheets);
                }

                // Turn off alerts to prevent asking for 'overwrite existing' and 'save changes' messages.
                excel.DisplayAlerts = false;

                //set active sheet
                ((Excel.Worksheet)excel.ActiveWorkbook.Sheets[activeSheet]).Select();

                workbook.SaveAs(fileName, Excel.XlFileFormat.xlOpenXMLWorkbook, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                workbook.Close(true, misValue, misValue);
                excel.Quit();

                // Release our resources.
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(workbooks);
                Marshal.ReleaseComObject(excel);
                Marshal.FinalReleaseComObject(excel);
                MessageBox.Show("エクセルファイルは作成しました！", "メッセージ");
            }
            catch (Exception ex)
            {
                MessageBox.Show("エクセルファイルを作成できません。もし今のファイルは開いてあったら、閉めてください" + Environment.NewLine + "エラー："  + Environment.NewLine + ex.ToString(),  "エラー",
                 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            this.Cursor = Cursors.Default;
        }
    }
}
