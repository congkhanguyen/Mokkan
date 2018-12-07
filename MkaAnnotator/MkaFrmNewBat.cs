using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MokkAnnotator.MkaToolsData;
using MokkAnnotator.MkaCommon;
using System.IO;
using Microsoft.Win32;

namespace MokkAnnotator
{
    /// <summary>
    /// New Bat Form
    /// </summary>
    public partial class MkaFrmNewBat : Form
    {
        private MkaFrmAnnotator _owner;     // annotation form

        /// <summary>
        /// Bat information
        /// </summary>
        private MkaBatInfo _batInfo;
        public MkaBatInfo BatInfo
        {
            get { return this._batInfo; }
        }

        public MkaBatManager BatManager { get; set; }

        private OpenFileDialog ofd = new OpenFileDialog();
        private FolderBrowserDialog fbd = new FolderBrowserDialog();
        private String batPath;
        private List<String> imgfullnames = new List<String>();
        private List<String> glassnames = new List<String>();

        /// <summary>
        /// Constructor
        /// </summary>
        public MkaFrmNewBat(MkaFrmAnnotator owner)
        {
            InitializeComponent();
            _owner = owner;

            dtpDate.MinDate = MkaDefine.StartDate;
            dtpDate.MaxDate = DateTime.Today;
        }

        /// <summary>
        /// Init form when loading
        /// </summary>   
        private void MkaFrmNewBat_Load(object sender, EventArgs e)
        {
            // load recent registered bat information   
            this.txtChousaJisuu.Text = MkaBatInfo.LastBat.ChousaJisuu.ToString();
            this.txtOoChiku.Text = MkaBatInfo.LastBat.OoChiku;
            this.txtChuushouChiku.Text = MkaBatInfo.LastBat.ChuushouChiku;
            this.txtIkoumei.Text = MkaBatInfo.LastBat.Ikoumei;
            this.txtDosoumei.Text = MkaBatInfo.LastBat.Dosoumei;
            if (MkaBatInfo.LastBat.Grid == 0)
                this.txtGrid.Text = "";
            else
                this.txtGrid.Text = MkaBatInfo.LastBat.Grid.ToString();
            this.dtpDate.Value = MkaBatInfo.LastBat.Date;
        }

        /// <summary>
        /// Select bat's directory
        /// </summary>        
        private void btnBrowserPath_Click(object sender, EventArgs e)
        {
            batPath = txtBatPath.Text;

            // get last accessed path
            if (batPath == "")                
            {                
                RegistryKey key = MkaDefine.RootKey.OpenSubKey(MkaDefine.RegKey);
                batPath = (String)key.GetValue(MkaDefine.RecentBatPath);                
            }

            if (!Directory.Exists(batPath))
                batPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            fbd.SelectedPath = batPath;

            // Display select directory dialog  
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                batPath = fbd.SelectedPath;
                txtBatPath.Text = batPath;
                txtBatBangou.Text = new DirectoryInfo(batPath).Name;
                GetImageFilenamesFromPath(batPath);
                int grid;
                if (Int32.TryParse(Path.GetFileName(Path.GetDirectoryName(batPath)), out grid))
                    txtGrid.Text = grid.ToString();
            }
        }

        /// <summary>
        /// Get all image filenames from the selected path and display on text box
        /// </summary>
        private void GetImageFilenamesFromPath(String path)
        {
            String[] filters = MkaDefine.IOImageFilterExt.Split(';');
            String file;
            imgfullnames.Clear();
            foreach (String filter in filters)            
                imgfullnames.AddRange(Directory.GetFiles(path, filter));                
                
            txtGlassPaths.Text = "";
            glassnames.Clear();
            foreach (String glass in imgfullnames)
            {
                file = Path.GetFileName(glass);                
                txtGlassPaths.Text += "\"" + file + "\" ";
                file = Path.GetDirectoryName(glass) + "\\" + Path.GetFileNameWithoutExtension(glass) + MkaDefine.MkaFileExt; ;                
                glassnames.Add(file);
            }
        }

        /// <summary>
        /// Select glass images' path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowserGlass_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(batPath))            
            {
                // get last accessed path
                RegistryKey key = MkaDefine.RootKey.OpenSubKey(MkaDefine.RegKey);
                batPath = (String)key.GetValue(MkaDefine.RecentBatPath);
                if (!Directory.Exists(batPath))
                    batPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
            
            // Display select file dialog              
            ofd.InitialDirectory = batPath;
            ofd.Multiselect = true;
            ofd.Filter = MkaDefine.IOImageFilterAll + "|" + MkaDefine.IOImageFilter;

            // Get glass images' filenames
            String glassname;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                imgfullnames = ofd.FileNames.ToList<String>();
                glassnames.Clear();

                txtGlassPaths.Text = "";
                int i;
                for (i = 0; i < ofd.FileNames.Length; i++)
                {
                    txtGlassPaths.Text += "\"" + Path.GetFileName(ofd.FileNames[i]) + "\" ";
                    glassname = Path.GetDirectoryName(ofd.FileNames[i]) + "\\" + Path.GetFileNameWithoutExtension(ofd.FileNames[i]) + MkaDefine.MkaFileExt;
                    glassnames.Add(glassname);
                }

                if (i > 0)
                {
                    batPath = Path.GetDirectoryName(ofd.FileName);
                    int grid;
                    if (Int32.TryParse(Path.GetFileName(Path.GetDirectoryName(batPath)), out grid))
                        txtGrid.Text = grid.ToString();
                }
                txtBatPath.Text = batPath;
                txtBatBangou.Text = new DirectoryInfo(batPath).Name;
            }
        }

        /// <summary>
        /// OK button -> new bat file is created
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            // check input data
            if (!CheckValidInput())
                return;

            //update working directory
            DirectoryInfo wFolder = new DirectoryInfo(txtBatPath.Text);
            MkaDefine.MokkanPath = wFolder.Parent.Parent.FullName;
            
            // create bat information
            _batInfo = new MkaBatInfo();
            _batInfo.ChousaJisuu = Int32.Parse(txtChousaJisuu.Text.Trim());
            _batInfo.OoChiku = txtOoChiku.Text.Trim();
            _batInfo.ChuushouChiku = txtChuushouChiku.Text.Trim();
            _batInfo.Ikoumei = txtIkoumei.Text.Trim();
            _batInfo.Dosoumei = txtDosoumei.Text.Trim();
            if (txtGrid.Text.Trim() == "")
                _batInfo.Grid = 0;
            else
                _batInfo.Grid = Int32.Parse(txtGrid.Text.Trim());
            _batInfo.Date = dtpDate.Value;
            _batInfo.BatBangou = txtBatBangou.Text.Trim();
            _batInfo.BatDirectory = txtBatPath.Text;
            
            // check if existed
            String batFile = _batInfo.BatDirectory + "\\" + _batInfo.BatBangou + MkaDefine.BmkFileExt;

            if (File.Exists(batFile))
            {
                switch (MessageBox.Show(this, String.Format(MkaMessage.WarnExistedFile, _batInfo.BatBangou + MkaDefine.BmkFileExt),
                                        MkaMessage.AppCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:
                        break;
                    case DialogResult.No:
                        return;
                    case DialogResult.Cancel:
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                        return;
                }
            }

            MkaBatInfo.LastBat = _batInfo;

            BatManager = new MkaBatManager(_owner);
            BatManager.BatInfo = _batInfo;
            BatManager.ImageFilePaths = imgfullnames;
            BatManager.GlassFilePaths = glassnames;
            for(int i = 0; i < imgfullnames.Count; i++)
            {
                BatManager.ImageFileNames.Add(Path.GetFileName(imgfullnames[i]));
                BatManager.GlassFileNames.Add(Path.GetFileName(glassnames[i]));
            }

            // save bat path to registry
            RegistryKey key = MkaDefine.RootKey.CreateSubKey(MkaDefine.RegKey);
            key.SetValue(MkaDefine.RecentBatPath, batPath);  

            // Close form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Check input data
        /// </summary>
        /// <returns>true if all input data were valid</returns>
        private bool CheckValidInput()
        {
            int number;
            String path;
            String error = "";

            // check glass filenames
            path = this.txtGlassPaths.Text;
            if(path == String.Empty)
            {
                error = String.Format(MkaMessage.ErrEmptyPath, GetLblText(lblGlassPaths)) + "\n" + error;
                this.btnBrowserGlass.Focus();
            }

            // check path
            path = this.txtBatPath.Text;
            if (path == String.Empty)
            {
                error = String.Format(MkaMessage.ErrEmptyPath, GetLblText(lblBatPath)) + "\n" + error;
                this.btnBrowserPath.Focus();
            }
            else if (!Directory.Exists(path))
            {
                error = String.Format(MkaMessage.ErrInvalid, GetLblText(lblBatPath)) + "\n" + error;
                this.btnBrowserPath.Focus();
            }

            // check bat number
            if (txtBatBangou.Text.Trim() == "")
            {
                error = String.Format(MkaMessage.ErrInputRequest, GetLblText(lblBatBangou)) + "\n" + error;
                txtBatBangou.Focus();
            }

            //// check grid
            //if (!Int32.TryParse(txtGrid.Text, out number))
            //{
            //    error = String.Format(MkaMessage.ErrNumberRequest, GetLblText(lblGrid)) + "\n" + error;
            //    txtGrid.Focus();
            //}

            //// check layer name
            //if (txtDosoumei.Text.Trim() == "")
            //{
            //    error = String.Format(MkaMessage.ErrInputRequest, GetLblText(lblDosoumei)) + "\n" + error;
            //    txtDosoumei.Focus();
            //}

            //// check remain name
            //if (txtIkoumei.Text.Trim() == "")
            //{
            //    error = String.Format(MkaMessage.ErrInputRequest, GetLblText(lblIkoumei)) + "\n" + error;
            //    txtIkoumei.Focus();
            //}

            //// check narrow area
            //if (txtChuushouChiku.Text.Trim() == "")
            //{
            //    error = String.Format(MkaMessage.ErrInputRequest, GetLblText(lblChuusouChiku)) + "\n" + error;
            //    txtChuushouChiku.Focus();
            //}

            //// check large area
            //if (txtOoChiku.Text.Trim() == "")
            //{
            //    error = String.Format(MkaMessage.ErrInputRequest, GetLblText(lblOoChiku)) + "\n" + error;
            //    txtOoChiku.Focus();
            //}

            // check dig order
            if (!Int32.TryParse(txtChousaJisuu.Text, out number))
            {
                error = String.Format(MkaMessage.ErrNumberRequest, GetLblText(lblChousaJisuu)) + "\n" + error;
                txtChousaJisuu.Focus();
            }

            //// check match directory
            //String glass = Path.GetFileNameWithoutExtension(path);

            //DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(path));
            //String bat = dir.Parent.ToString();
            //String grid = dir.Parent.Parent.ToString();

            //foreach (string pathGlass in this.txtGlassPaths.Lines)
            //{

            //}
            ////if (glass != this.txtGlassBangou.Text)
            ////{
            ////    error = String.Format(MkaMessage.ErrIncorrectFilePath, GetLblText(lblGlassBangou)) + "\n" + error;
            ////    txtGlassBangou.Focus();
            ////}

            //if (bat != this.txtBatBangou.Text)
            //{
            //    error = String.Format(MkaMessage.ErrIncorrectFilePath, GetLblText(lblBatBangou)) + "\n" + error;
            //    txtBatBangou.Focus();
            //}
            //if (grid != this.txtGrid.Text)
            //{
            //    error = String.Format(MkaMessage.ErrIncorrectFilePath, GetLblText(lblGrid)) + "\n" + error;
            //    txtGrid.Focus();
            //}
            // show error message
            if (error != "")
            {
                MkaMessage.ShowError(error);
                return false;
            }

            return true;
        }


        /// <summary>
        /// Get label text 
        /// </summary>        
        private String GetLblText(Label label)
        {
            String lblText = label.Text;
            lblText = lblText.Substring(0, lblText.IndexOf('('));
            return lblText;
        }
    }
}
