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
using MokkAnnotator.MkaDrawTools;

namespace MokkAnnotator.MkaWindow
{
    public partial class MkaPropertiesGlass : Form
    {
        private MkaGlassInfo _glassInfo;
        private MkaGlassInfo _beforeGlassInfo;
        private float _beforeSize;
        public bool Changed;
        public bool RFontSizeChanged;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public MkaPropertiesGlass(MkaGlassInfo glass)
        {
            InitializeComponent();
            _glassInfo = glass;            

            dtpDate.MinDate = MkaDefine.StartDate;
            dtpDate.MaxDate = DateTime.Today;

            Changed = false;
            RFontSizeChanged = false;

            // initialize
            InitProperty();
        }

        private void InitProperty()
        {
            // load bat information   
            this.txtChousaJisuu.Text = _glassInfo.ChousaJisuu.ToString();
            this.txtOoChiku.Text = _glassInfo.OoChiku;
            this.txtChuushouChiku.Text = _glassInfo.ChuushouChiku;
            this.txtIkoumei.Text = _glassInfo.Ikoumei;
            this.txtDosoumei.Text = _glassInfo.Dosoumei;
            if(_glassInfo.Grid == 0)
                this.txtGrid.Text = "";
            else
                this.txtGrid.Text = _glassInfo.Grid.ToString();
            this.dtpDate.Value = _glassInfo.Date;
            this.txtBatBangou.Text = _glassInfo.BatBangou;

            this.txtGlassBangou.Text = _glassInfo.GlassItaBangou.ToString();
            this.txtKaishiRBangou.Text = _glassInfo.KaishiRBangou.ToString();

            this.cmbFontSize.Text = _glassInfo.RFontSize.ToString();

            _beforeSize = _glassInfo.RFontSize;
            _beforeGlassInfo = _glassInfo.Clone();
        }

        /// <summary>
        /// Check input data
        /// </summary>
        /// <returns>true if all input data were valid</returns>
        private bool CheckValidInput()
        {
            int number;
            String error = "";
            
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

        /// <summary>
        /// Submit bat property
        /// </summary>        
        private void btnOK_Click(object sender, EventArgs e)
        {
            // check input data
            if (!CheckValidInput())
                return;

            // bat information
            _glassInfo.ChousaJisuu = Int32.Parse(txtChousaJisuu.Text.Trim());
            _glassInfo.OoChiku = txtOoChiku.Text.Trim();
            _glassInfo.ChuushouChiku = txtChuushouChiku.Text.Trim();
            _glassInfo.Ikoumei = txtIkoumei.Text.Trim();
            _glassInfo.Dosoumei = txtDosoumei.Text.Trim();
            if (txtGrid.Text.Trim() == "")
                _glassInfo.Grid = 0;
            else
                _glassInfo.Grid = Int32.Parse(txtGrid.Text.Trim());
            _glassInfo.Date = dtpDate.Value;
            _glassInfo.BatBangou = txtBatBangou.Text.Trim();

            // glass information
            _glassInfo.GlassItaBangou = txtGlassBangou.Text.Trim();
            _glassInfo.KaishiRBangou = Int32.Parse(txtKaishiRBangou.Text.Trim());

            _glassInfo.RFontSize = float.Parse(cmbFontSize.SelectedItem.ToString());

            Changed = (_glassInfo != _beforeGlassInfo);
            RFontSizeChanged = (_glassInfo.RFontSize != _beforeSize);

            // Close form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
