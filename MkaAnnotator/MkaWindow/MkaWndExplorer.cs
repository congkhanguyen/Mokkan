using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using MokkAnnotator.MkaToolsData;
using System.IO;
using System.Collections;
using MokkAnnotator.MkaCommon;

namespace MokkAnnotator.MkaWindow
{
    public partial class MkaWndExplorer : DockContent
    {
        private MkaFrmAnnotator _owner;         // parent form
        private String _batPath;                // bat directory path
        private String _oldName;
        private MkaBatManager _batFile;
        private List<Bitmap> _thumbBmps;

        /// <summary>
        /// Constructor
        /// </summary>
        public MkaWndExplorer(MkaFrmAnnotator owner)
        {
            InitializeComponent();
            _owner = owner;

            BatSelected = false;

            tsList.Checked = true;
            treeExplorer.Visible = true;
            tsThumbnail.Checked = false;
            thumbExplorer.Visible = false;
            _thumbBmps = new List<Bitmap>();
        }
        
        /// <summary>
        /// Bat is selected or not
        /// </summary>
        public bool BatSelected { get; set; }

        /// <summary>
        /// Explore the bat file and glass file names to explorer windows
        /// </summary>
        /// <param name="bat">target bat information</param>
        public void Explore(MkaBatManager batFile)
        {
            _batFile = batFile;
            _batPath = batFile.BatDirectory;
                        
            // set bat name
            treeExplorer.Nodes.Clear();
            treeExplorer.Nodes.Add(batFile.BatInfo.BatBangou);

            String glass;
            FileStream fs;
            Bitmap bmp;
            ListViewItem lvi;
            int width = Math.Max(50, Math.Min((int)(0.5 * thumbExplorer.Width), 300));
            int height = (int)(0.75 * width);
            viewImgList.Images.Clear();
            thumbExplorer.Items.Clear();
            _thumbBmps.Clear();
            viewImgList.ImageSize = new Size(width, height);

            // set view and outline of glass files
            for (int i = 0; i < batFile.GlassFilePaths.Count; i++)
            {
                glass = Path.GetFileNameWithoutExtension(batFile.GlassFilePaths[i]);
                treeExplorer.Nodes[0].Nodes.Add(new TreeNode(glass, 3, 3));

                fs = new FileStream(_batFile.ImageFilePaths[i], FileMode.Open);
                bmp = (Bitmap)Bitmap.FromStream(fs);
                bmp = (Bitmap)bmp.GetThumbnailImage(200, 200, null, IntPtr.Zero);
                _thumbBmps.Add(bmp);
                fs.Close();                
                
                viewImgList.Images.Add(bmp);
                lvi = new ListViewItem(glass);
                lvi.ImageIndex = i;
                thumbExplorer.Items.Add(lvi);
            }

            treeExplorer.Sort();
            treeExplorer.ExpandAll();
        }  
      
        /// <summary>
        /// Clear tree explorer
        /// </summary>
        public void Clear()
        {
            treeExplorer.Nodes.Clear();
        }

        /// <summary>
        /// Open glass file when double click or enter specified filename
        /// </summary>        
        private void OpenGlassFile(object sender, EventArgs e)
        {
            TreeNode node = ((TreeView)sender).SelectedNode;
            if (node != null && node.Level == 1)
            {
                // open and display glass
                _owner.FileOpenGlass(_batPath + "\\" + node.Text + MkaDefine.MkaFileExt);
                // glass property
                _owner.ExplorerShowPropertyGrid(_batPath + "\\" + node.Text + MkaDefine.MkaFileExt, true, false); 
            }
        }

        #region TreeView Event

        /// <summary>
        /// Show property on PropertyGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeExplorer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                _owner.ExplorerShowPropertyGrid(null, false, false);       // bat property
                BatSelected = true;
            }
            else
            {
                _owner.ExplorerShowPropertyGrid(_batPath + "\\" + e.Node.Text + MkaDefine.MkaFileExt, true, false); // glass property
                BatSelected = false;
            }
        }

        /// <summary>
        /// Show context menu strip
        /// </summary>        
        private void treeExplorer_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Select the clicked node
                treeExplorer.SelectedNode = treeExplorer.GetNodeAt(e.X, e.Y);

                if (treeExplorer.SelectedNode != null)
                {
                    if (treeExplorer.SelectedNode.Level == 0)
                        batMenu.Show(treeExplorer, e.Location);     // bat context menu
                    else
                        glassMenu.Show(treeExplorer, e.Location);   // glass context menu
                }
            }
        }

        /// <summary>
        /// When press enter key, open the glass file
        /// </summary>        
        private void treeExplorer_KeyPress(object sender, KeyPressEventArgs e)
        {
            // if there is a currently selected node
            if (null != treeExplorer.SelectedNode)
            {
                // if the Enter key was pressed
                if ((char)Keys.Return == e.KeyChar)
                    OpenGlassFile(sender, e);
            }
        }

        #endregion TreeView Event
        
        #region ContextMenuStrip

        /// <summary>
        /// Rename bat, glass name from menu
        /// </summary>        
        private void mniRename_Click(object sender, EventArgs e)
        {
            BeginRename();
        }

        /// <summary>
        /// Rename when press F2 key
        /// </summary>        
        private void treeExplorer_KeyDown(object sender, KeyEventArgs e)
        {
            // if there is a currently selected node
            if (null != treeExplorer.SelectedNode)
            {
                // if the Enter key was pressed
                if (e.KeyData == Keys.F2)
                    BeginRename();
            }
        }

        /// <summary>
        /// Rename in case of pressing F2 or selecting from menu
        /// </summary>  
        private void BeginRename()
        {
            // allow to edit node label
            _oldName = treeExplorer.SelectedNode.Text;
            treeExplorer.SelectedNode.BeginEdit();
        }
        
        /// <summary>
        /// Rename in case of slow double click
        /// </summary>        
        private void treeExplorer_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            _oldName = e.Node.Text;
        }

        /// <summary>
        /// Submit renamed bat, glass name
        /// </summary>        
        private void treeExplorer_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label == null) return;

            if (e.Label.Trim() == "")       // empty string
            {
                MkaMessage.ShowError(MkaMessage.ErrEmptyName);
                e.CancelEdit = true;
                return;
            }
            else if (e.Label.IndexOfAny(new char[] { '\\', '/', ':', '*', '?', '"', '<', '>', '|' }) != -1)    // invalid characters
            {
                MkaMessage.ShowError(MkaMessage.ErrInvalidName);
                e.CancelEdit = true;
                return;
            }

            String newName = e.Label;

            // change name
            if (newName != _oldName)
            {
                if (e.Node.Level == 0)
                    _owner.FileChangeBatName(newName);
                else
                    _owner.FileChangeGlassName(_oldName, newName);
            }
        }

        /// <summary>
        /// Open glass file
        /// </summary>        
        private void mniGlassOpen_Click(object sender, EventArgs e)
        {
            _owner.FileOpenGlass(_batPath + "\\" + treeExplorer.SelectedNode.Text + MkaDefine.MkaFileExt);             
        }

        /// <summary>
        /// Remove glass from bat
        /// </summary>        
        private void mniGlassRemove_Click(object sender, EventArgs e)
        {
            _owner.FileRemoveGlass(_batPath + "\\" + treeExplorer.SelectedNode.Text + MkaDefine.MkaFileExt);
            treeExplorer.SelectedNode.Remove();
        }

        /// <summary>
        /// Show glass's property
        /// </summary>        
        private void mniGlassProperty_Click(object sender, EventArgs e)
        {
            _owner.ExplorerShowPropertyGrid(_batPath + "\\" + treeExplorer.SelectedNode.Text + MkaDefine.MkaFileExt, true, true); 
        }

        /// <summary>
        /// Add new glass to bat
        /// </summary>        
        private void mniBatAddNewGlass_Click(object sender, EventArgs e)
        {
            _owner.FileNewGlass();
        }

        /// <summary>
        /// Add existing glass to bat
        /// </summary>        
        private void mniBatAddExistingGlass_Click(object sender, EventArgs e)
        {
            _owner.FileAddGlass();
        }

        /// <summary>
        /// Add existing glass image to bat
        /// </summary>        
        private void mniBatAddNewGlassImage_Click(object sender, EventArgs e)
        {
            _owner.FileAddImage();
        }

        /// <summary>
        /// Close bat
        /// </summary>        
        private void mniBatClose_Click(object sender, EventArgs e)
        {
            _owner.FileCloseBat();        
        }

        /// <summary>
        /// Show bat's property
        /// </summary>        
        private void mniBatProperty_Click(object sender, EventArgs e)
        {
            _owner.ExplorerShowPropertyGrid(null, false, true);
        }

        #endregion ContextMenuStrip

        /// <summary>
        /// Change view mode of explorer
        /// </summary>        
        private void tsViewModeChange_Click(object sender, EventArgs e)
        {
            // selected item
            ToolStripButton item = (ToolStripButton)sender;
            String fName = item.Name.Substring(2);

            if (fName == "List")
            {
                treeExplorer.Visible = true;
                tsThumbnail.Checked = false;
                thumbExplorer.Visible = false;
            }
            else
            {
                thumbExplorer.Visible = true;
                tsList.Checked = false;
                treeExplorer.Visible = false;
            }
        }

        private void thumbExplorer_KeyPress(object sender, KeyPressEventArgs e)
        {
            // if there is a currently selected node
            if (null != thumbExplorer.SelectedItems)
            {
                // if the Enter key was pressed
                if ((char)Keys.Return == e.KeyChar)
                {
                    // open and display glass
                    _owner.FileOpenGlass(_batPath + "\\" + thumbExplorer.SelectedItems[0].Text + MkaDefine.MkaFileExt);
                    // glass property
                    _owner.ExplorerShowPropertyGrid(_batPath + "\\" + thumbExplorer.SelectedItems[0].Text + MkaDefine.MkaFileExt, true, false); 
                }                    
            }
        }

        private void thumbExplorer_DoubleClick(object sender, EventArgs e)
        {
            // open and display glass
            _owner.FileOpenGlass(_batPath + "\\" + thumbExplorer.SelectedItems[0].Text + MkaDefine.MkaFileExt);
            // glass property
            _owner.ExplorerShowPropertyGrid(_batPath + "\\" + thumbExplorer.SelectedItems[0].Text + MkaDefine.MkaFileExt, true, false); 
        }     
    }
}
