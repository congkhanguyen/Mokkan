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

namespace MokkAnnotator.MkaWindow
{
    public partial class MkaPropertiesBat : Form
    {
        private MkaBatManager _batManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public MkaPropertiesBat(MkaBatManager bat)
        {
            InitializeComponent();
            _batManager = bat;

            dtpDate.MinDate = MkaDefine.StartDate;
            dtpDate.MaxDate = DateTime.Today;

            // initialize
            InitProperty();
        }

        private void InitProperty()
        {
            // load bat information   
            this.txtChousaJisuu.Text = _batManager.BatInfo.ChousaJisuu.ToString();
            this.txtOoChiku.Text = _batManager.BatInfo.OoChiku;
            this.txtChuushouChiku.Text = _batManager.BatInfo.ChuushouChiku;
            this.txtIkoumei.Text = _batManager.BatInfo.Ikoumei;
            this.txtDosoumei.Text = _batManager.BatInfo.Dosoumei;
            if (_batManager.BatInfo.Grid == 0)
                this.txtGrid.Text = "";
            else
                this.txtGrid.Text = _batManager.BatInfo.Grid.ToString();
            this.dtpDate.Value = _batManager.BatInfo.Date;
            this.txtBatBangou.Text = _batManager.BatInfo.BatBangou;
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

            // create bat information
            MkaBatInfo _batInfo = new MkaBatInfo();
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
            _batInfo.BatDirectory = _batManager.BatDirectory;

            MkaBatInfo.LastBat = _batInfo;

            _batManager.BatInfo = _batInfo;

            // Close form
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
