using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using MokkAnnotator.MkaToolsData;
using MokkAnnotator.MkaCommon;
using Microsoft.Win32;
using MokkAnnotator.MkaDrawTools;

namespace MokkAnnotator
{
    /// <summary>
    /// New Glass Form
    /// </summary>
    public partial class MkaFrmNewGlass : Form
    {
        private MkaFrmAnnotator _owner;     // annotation form
        private MkaBatInfo _batInfo;        // bat information
        private MkaGlassInfo _glassInfo;    // glass information

        /// <summary>
        /// New glass document
        /// </summary>
        private MkaDocument _glassDoc;
        public MkaDocument NewGlassDoc
        {
            get { return this._glassDoc; }
        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public MkaFrmNewGlass(MkaFrmAnnotator owner)
        {
            InitializeComponent();
            _owner = owner;

            dtpDate.MinDate = MkaDefine.StartDate;
            dtpDate.MaxDate = DateTime.Today;
        }       

        /// <summary>
        /// Init form when loading
        /// </summary>        
        private void MkaFrmNewGlass_Load(object sender, EventArgs e)
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
            this.txtBatBangou.Text = MkaBatInfo.LastBat.BatBangou;

            this.txtGlassBangou.Text = String.Empty;
            this.txtKaishiRBangou.Text = MkaMokkanInfo.LastRBangou.ToString();

            this.cmbFontSize.Text = MkaDefine.RDefaultFontSize.ToString();
        }

        /// <summary>
        /// Select bat's directory
        /// </summary>        
        private void btnBrowser_Click(object sender, EventArgs e)
        {
            // get last accessed path
            RegistryKey key = MkaDefine.RootKey.OpenSubKey(MkaDefine.RegKey);
            String path = (String)key.GetValue(MkaDefine.RecentBatPath);

            // Display select directory dialog
            OpenFileDialog ofd = new OpenFileDialog();
            if(Directory.Exists(path))
                ofd.InitialDirectory = path;
            ofd.Multiselect = false;
            ofd.Filter = MkaDefine.IOImageFilterAll + "|" + MkaDefine.IOImageFilter;            

            // Get bat's directory
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.txtImagePath.Text = ofd.FileName;
                txtBatBangou.Text = new DirectoryInfo(Path.GetDirectoryName(ofd.FileName)).Name;
                txtGlassBangou.Text = Path.GetFileNameWithoutExtension(ofd.FileName);
                int grid;
                if (Int32.TryParse(Path.GetFileName(Path.GetDirectoryName(Path.GetDirectoryName(ofd.FileName))), out grid))
                    txtGrid.Text = grid.ToString();
            }
        }

        /// <summary>
        /// OK button click event
        /// </summary>        
        private void btnOK_Click(object sender, EventArgs e)
        {
            // check input
            if (!CheckValidInput()) return;

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
            _batInfo.BatDirectory = Path.GetDirectoryName(txtImagePath.Text);            

            // create glass information
            _glassInfo = new MkaGlassInfo();
            _glassInfo.GlassItaBangou = txtGlassBangou.Text.Trim();            
            _glassInfo.ImageFilePath = txtImagePath.Text;
            _glassInfo.ImageFileName = Path.GetFileName(txtImagePath.Text);
            _glassInfo.GlassFilePath = Path.GetDirectoryName(txtImagePath.Text) + "\\" + Path.GetFileNameWithoutExtension(txtImagePath.Text) + MkaDefine.MkaFileExt;
            _glassInfo.KaishiRBangou = Int32.Parse(txtKaishiRBangou.Text.Trim());

            _glassInfo.RFontSize = float.Parse(cmbFontSize.SelectedItem.ToString());

            //update working directory
            DirectoryInfo wFolder = new DirectoryInfo(_glassInfo.GlassFilePath);
            MkaDefine.MokkanPath = wFolder.Parent.Parent.Parent.FullName;

            // check if existed
            if(File.Exists(_glassInfo.GlassFilePath))
            {
                switch (MessageBox.Show(this, String.Format(MkaMessage.WarnExistedFile, _glassInfo.GlassFileName), 
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
            MkaMokkanInfo.LastRBangou = _glassInfo.KaishiRBangou;

            // store accessed path in registry
            RegistryKey key = MkaDefine.RootKey.CreateSubKey(MkaDefine.RegKey);
            key.SetValue(MkaDefine.RecentGlassPath, Path.GetDirectoryName(txtImagePath.Text));

            // create new glass document
            _glassDoc = new MkaDocument(_owner);
            _glassDoc.Text = _glassInfo.GlassFileName;             
            _glassDoc.GlassInfo = _glassInfo;            
            _glassDoc.GlassImage = _glassInfo.ImageFilePath;            
            _glassDoc.BatInfo = _batInfo;
            _glassDoc.Save(true);

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

            // check image path
            path = this.txtImagePath.Text;
            if (path == String.Empty)
            {
                error = String.Format(MkaMessage.ErrEmptyPath, GetLblText(lblImagePath)) + "\n" + error;
                this.btnBrowser.Focus();
            }
            else if (!File.Exists(path))
            {
                error = String.Format(MkaMessage.ErrInvalid, GetLblText(lblImagePath)) + "\n" + error;
                this.btnBrowser.Focus();
            }

            // check start remain id
            if (!Int32.TryParse(txtKaishiRBangou.Text, out number))
            {
                error = String.Format(MkaMessage.ErrNumberRequest, GetLblText(lblKaishiRBangou)) + "\n" + error;
                txtKaishiRBangou.Focus();
            }

            // check glass id            
            if (txtGlassBangou.Text.Trim() == "")
            {
                error = String.Format(MkaMessage.ErrInputRequest, GetLblText(lblGlassBangou)) + "\n" + error;
                txtGlassBangou.Focus();
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
            //if (glass != this.txtGlassBangou.Text)
            //{
            //    error = String.Format(MkaMessage.ErrIncorrectFilePath, GetLblText(lblGlassBangou)) + "\n" + error;
            //    txtGlassBangou.Focus();
            //}

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
