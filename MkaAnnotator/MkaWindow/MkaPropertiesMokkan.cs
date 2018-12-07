using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using MokkAnnotator.MkaDrawTools;
using MokkAnnotator.MkaToolsData;
using MokkAnnotator.MkaCommon;


namespace MokkAnnotator.MkaWindow
{
    partial class MkaPropertiesMokkan : Form
    {
        /// <summary>
        /// List of mokkans
        /// </summary>
        public MokkanList MokkanList { get; set; }

        /// <summary>
        /// Mokkan information
        /// </summary>
        public MkaMokkanInfo MokkanProperty { get; set; }

        /// <summary>
        /// Graphical Properties
        /// </summary>
        public GraphicsProperties Properties { get; set; }

        /// <summary>
        /// Multiple object selected
        /// </summary>
        private bool _multi = false;


        /// <summary>
        /// Constructor
        /// </summary>        
        public MkaPropertiesMokkan(MokkanList mokkans, MkaMokkanInfo info, GraphicsProperties properties, bool multi)
        {
            InitializeComponent();

            MokkanList = mokkans;
            MokkanProperty = info;
            Properties = properties;
            mkaGraphicsProperties.Properties = Properties;
            _multi = multi;
        }

        /// <summary>
        /// Initialize
        /// </summary>        
        private void MkaPropertiesDialog_Load(object sender, EventArgs e)
        {
            if (_multi)
                txtRBangou.Enabled = false;
            else
               txtRBangou.Text = MokkanProperty.RBangou.ToString();
            txtKariShakubun.Text = MokkanProperty.KariShakubun;
            txtGaihouShoshuuJyouhou.Text = MokkanProperty.GaihouShoshuuJyouhou;
            txtShashinBangouJyouhou.Text = MokkanProperty.ShasinBangouJyouhou;
            txtBikou.Text = MokkanProperty.Bikou;
            cmbRShowPosition.DataSource = Enum.GetValues(typeof(ShowPosition));
        }

        /// <summary>
        /// Submit changed data
        /// </summary>        
        private void btnOK_Click(object sender, EventArgs e)
        {
            // information
            if (!_multi)
            {
                // check remain id
                int rid;
                String error = "";

                // check valid input
                if (!Int32.TryParse(txtRBangou.Text, out rid))
                {
                    error = String.Format(MkaMessage.ErrNumberRequest, GetLblText(lblRBangou));
                    txtRBangou.Focus();
                    MkaMessage.ShowError(error);
                    return;                 
                }
                
                // check duplication
                if (rid != MokkanProperty.RBangou && MokkanList.CheckDuplexRID(rid))
                {
                    error = String.Format(MkaMessage.ErrDublication, GetLblText(lblRBangou));
                    txtRBangou.Focus();
                    MkaMessage.ShowError(error);
                    return;
                }

                MokkanProperty.RBangou = rid;
            }
            MokkanProperty.KariShakubun = txtKariShakubun.Text;
            MokkanProperty.GaihouShoshuuJyouhou = txtGaihouShoshuuJyouhou.Text;
            MokkanProperty.ShasinBangouJyouhou = txtShashinBangouJyouhou.Text;
            MokkanProperty.Bikou = txtBikou.Text;
            
            // graphics
            Properties = mkaGraphicsProperties.Properties;
            Properties.RShowPosition = (ShowPosition)cmbRShowPosition.SelectedItem;

            DialogResult = DialogResult.OK;
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