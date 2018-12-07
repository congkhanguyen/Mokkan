using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MokkAnnotator.MkaDrawTools;
using MokkAnnotator.MkaCommon;
using System.Configuration;
using MokkAnnotator.MkaToolsData;

namespace MokkAnnotator.MkaWindow
{
    public partial class MkaWndSettings : Form
    {
        private MkaFrmAnnotator _owner;     // parent form
        /// <summary>
        /// Constructor
        /// </summary>
        public MkaWndSettings(MkaFrmAnnotator owner)
        {
            _owner = owner;
            InitializeComponent();
        }

        /// <summary>
        /// Initialize window
        /// </summary>        
        private void MkaWndSettings_Load(object sender, EventArgs e)
        {
            // graphics property
            mkaGraphicsProperties.Properties = new GraphicsProperties();
            cmbFontSize.Text = mkaGraphicsProperties.Properties.RFontSize.ToString();

            // database
            txtServer.Text = MkaDefine.DbServer;
            txtDatabase.Text = MkaDefine.DbDatabase;
            txtUsername.Text = MkaDefine.DbUsername;
            txtPassword.Text = MkaDefine.DbPassword;

            // load last information   
            this.txtChousaJisuu.Text = MkaBatInfo.LastBat.ChousaJisuu.ToString();
            this.txtOoChiku.Text = MkaBatInfo.LastBat.OoChiku;
            this.txtChuushouChiku.Text = MkaBatInfo.LastBat.ChuushouChiku;
            this.txtIkoumei.Text = MkaBatInfo.LastBat.Ikoumei;
            this.txtDosoumei.Text = MkaBatInfo.LastBat.Dosoumei;
            if(MkaBatInfo.LastBat.Grid == 0)
                this.txtGrid.Text = "";
            else
                this.txtGrid.Text = MkaBatInfo.LastBat.Grid.ToString();
            this.dtpDate.Value = MkaBatInfo.LastBat.Date;
            this.txtBatBangou.Text = MkaBatInfo.LastBat.BatBangou;            
            this.txtKaishiRBangou.Text = MkaMokkanInfo.LastRBangou.ToString();

            this.cmbFontSize.Text = MkaDefine.RDefaultFontSize.ToString();
        }

        /// <summary>
        /// Update settings
        /// </summary>
        private void btnOK_Click(object sender, EventArgs e)
        {
            // check input data
            if (!CheckValidInput())
                return;

            // graphics property
            MkaDefine.RDefaultFontSize = float.Parse(cmbFontSize.SelectedItem.ToString());
            MkaDefine.DefaultBorderColor = mkaGraphicsProperties.Properties.BorderColor;
            MkaDefine.DefaultPenWidth = mkaGraphicsProperties.Properties.PenWidth;
            MkaDefine.DefaultFillColor = mkaGraphicsProperties.Properties.FillColor;
            MkaDefine.DefaultColorAlpha = mkaGraphicsProperties.Properties.FillColorAlpha;

            // database
            MkaDefine.DbServer = txtServer.Text.Trim();
            MkaDefine.DbDatabase = txtDatabase.Text.Trim();
            MkaDefine.DbUsername = txtUsername.Text.Trim();
            MkaDefine.DbPassword = txtPassword.Text.Trim();

            // last default value
            MkaBatInfo.LastBat.ChousaJisuu = Int32.Parse(txtChousaJisuu.Text.Trim());
            MkaBatInfo.LastBat.OoChiku = txtOoChiku.Text.Trim();
            MkaBatInfo.LastBat.ChuushouChiku = txtChuushouChiku.Text.Trim();
            MkaBatInfo.LastBat.Ikoumei = txtIkoumei.Text.Trim();
            MkaBatInfo.LastBat.Dosoumei = txtDosoumei.Text.Trim();
            if (txtGrid.Text.Trim() == "")
                MkaBatInfo.LastBat.Grid = 0;
            else
                MkaBatInfo.LastBat.Grid = Int32.Parse(txtGrid.Text.Trim());
            MkaBatInfo.LastBat.Date = dtpDate.Value;
            MkaBatInfo.LastBat.BatBangou = txtBatBangou.Text.Trim();
            MkaMokkanInfo.LastRBangou = Int32.Parse(txtKaishiRBangou.Text.Trim());
        
            //Save all settings
            MkaDefine.SaveAppSettings();

            //Transfer setting to manual panel
            _owner.WndToolBar.uctrlGraphics.Properties = mkaGraphicsProperties.Properties;

            // close form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Check input data
        /// </summary>
        /// <returns>true if all input data were valid</returns>
        private bool CheckValidInput()
        {
            String error = "";
            int number;
            
            // check username
            if (txtUsername.Text.Trim() == "")
            {
                error = String.Format(MkaMessage.ErrInputRequest, GetLblText(lblUsername)) + "\n" + error;
                tbSettings.SelectedIndex = 1;
                txtUsername.Focus();                
            }

            // check database
            if (txtDatabase.Text.Trim() == "")
            {
                error = String.Format(MkaMessage.ErrInputRequest, GetLblText(lblDatabase)) + "\n" + error;
                tbSettings.SelectedIndex = 1;
                txtDatabase.Focus();
            }

            // check server
            if (txtServer.Text.Trim() == "")
            {
                error = String.Format(MkaMessage.ErrInputRequest, GetLblText(lblServer)) + "\n" + error;
                tbSettings.SelectedIndex = 1;
                txtServer.Focus();
            }

            // check start remain id
            if (!Int32.TryParse(txtKaishiRBangou.Text, out number))
            {
                error = String.Format(MkaMessage.ErrNumberRequest, GetLblText(lblKaishiRBangou)) + "\n" + error;
                txtKaishiRBangou.Focus();
            }

            // check bat number
            if (txtBatBangou.Text.Trim() == "")
            {
                error = String.Format(MkaMessage.ErrInputRequest, GetLblText(lblBatBangou)) + "\n" + error;
                txtBatBangou.Focus();
            }

            // check grid
            if (!Int32.TryParse(txtGrid.Text, out number))
            {
                error = String.Format(MkaMessage.ErrNumberRequest, GetLblText(lblGrid)) + "\n" + error;
                txtGrid.Focus();
            }

            // check layer name
            if (txtDosoumei.Text.Trim() == "")
            {
                error = String.Format(MkaMessage.ErrInputRequest, GetLblText(lblDosoumei)) + "\n" + error;
                txtDosoumei.Focus();
            }

            // check remain name
            if (txtIkoumei.Text.Trim() == "")
            {
                error = String.Format(MkaMessage.ErrInputRequest, GetLblText(lblIkoumei)) + "\n" + error;
                txtIkoumei.Focus();
            }

            // check narrow area
            if (txtChuushouChiku.Text.Trim() == "")
            {
                error = String.Format(MkaMessage.ErrInputRequest, GetLblText(lblChuusouChiku)) + "\n" + error;
                txtChuushouChiku.Focus();
            }

            // check large area
            if (txtOoChiku.Text.Trim() == "")
            {
                error = String.Format(MkaMessage.ErrInputRequest, GetLblText(lblOoChiku)) + "\n" + error;
                txtOoChiku.Focus();
            }

            // check dig order
            if (!Int32.TryParse(txtChousaJisuu.Text, out number))
            {
                error = String.Format(MkaMessage.ErrNumberRequest, GetLblText(lblChousaJisuu)) + "\n" + error;
                txtChousaJisuu.Focus();
            }
                                   
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
