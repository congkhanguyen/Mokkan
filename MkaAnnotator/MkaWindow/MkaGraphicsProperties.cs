using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;
using MokkAnnotator.MkaCommon;
using MokkAnnotator.MkaDrawTools;

namespace MokkAnnotator.MkaWindow
{
    public partial class MkaGraphicsProperties : UserControl
    {   
        /// <summary>
        /// Graphical Properties
        /// </summary>        
        private GraphicsProperties _properties;
        public GraphicsProperties Properties
        {
            get
            {
                return this._properties;
            }
            set
            {
                this._properties = value;
                btnFillColor.CenterColor = value.FillColor;
                cmbFillTransparent.Text = value.FillColorAlpha.ToString();
                btnLineColor.CenterColor = value.BorderColor;
                cmbLineWeight.Text = value.PenWidth.ToString() + " pt"; 
            }
        }
                
        /// <summary>
        /// Change last used properties flag
        /// </summary>
        [DefaultValue(true)]
        public bool ChangeLast { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        public MkaGraphicsProperties()
        {
            InitializeComponent();            
        }

        /// <summary>
        /// Initialize graphical control
        /// </summary>        
        private void uctrlGraphics_Load(object sender, EventArgs e)
        {
            foreach (float weight in MkaDefine.LineWeight)
                cmbLineWeight.Items.Add(weight.ToString()+ " pt");

            foreach (int transparent in MkaDefine.Transparency)
                cmbFillTransparent.Items.Add(transparent.ToString());

            //if (_properties == null)
            //    _properties = new GraphicsProperties();

            _properties = GraphicsProperties.LastProperties;

            btnFillColor.CenterColor = _properties.FillColor;
            cmbFillTransparent.Text = _properties.FillColorAlpha.ToString();
            btnLineColor.CenterColor = _properties.BorderColor;
            cmbLineWeight.Text = _properties.PenWidth.ToString() + " pt";
        }

        /// <summary>
        /// Select color for fill area
        /// </summary>        
        private void btnFillColor_Click(object sender, EventArgs e)
        {
            SelectColor(sender, e);
            _properties.FillColor = btnFillColor.CenterColor;
            if (ChangeLast)
                GraphicsProperties.LastProperties.FillColor = btnFillColor.CenterColor;
        }

        /// <summary>
        /// Select color for border
        /// </summary>        
        private void btnLineColor_Click(object sender, EventArgs e)
        {
            SelectColor(sender, e);
            _properties.BorderColor = btnLineColor.CenterColor;
            if (ChangeLast)
                GraphicsProperties.LastProperties.BorderColor = btnLineColor.CenterColor;
        }

        /// <summary>
        /// Open color dialog to select color
        /// </summary>        
        private void SelectColor(object sender, EventArgs e)
        {
            MkaColorButton btn = (MkaColorButton)sender;
            Point p = new Point(btn.Left, btn.Top + btn.Height);
            p = PointToScreen(p);

            MkaColorDialog clDlg = new MkaColorDialog(p.X, p.Y);

            clDlg.ShowDialog();

            if (clDlg.DialogResult == DialogResult.OK)
                btn.CenterColor = clDlg.Color;                         
        }

        /// <summary>
        /// Change fill area transparent
        /// </summary>        
        private void cmbFillTransparent_SelectedIndexChanged(object sender, EventArgs e)
        {
            int alpha = Convert.ToInt16(cmbFillTransparent.Text);
            _properties.FillColorAlpha = alpha;
            if (ChangeLast)
                GraphicsProperties.LastProperties.FillColorAlpha = alpha;
        }

        /// <summary>
        /// Change pen line weight
        /// </summary>
        private void cmbLineWeight_SelectedIndexChanged(object sender, EventArgs e)
        {
            float weight = (float)Convert.ToDouble(cmbLineWeight.Text.Substring(0, cmbLineWeight.Text.IndexOf(" ")));
            _properties.PenWidth = weight;
            if (ChangeLast)
                GraphicsProperties.LastProperties.PenWidth = weight;
        }

        /// <summary>
        /// Set to default values
        /// </summary>        
        private void btnDefault_Click(object sender, EventArgs e)
        {
            _properties = new GraphicsProperties();
            if(ChangeLast)
                GraphicsProperties.LastProperties = Properties;
            btnFillColor.CenterColor = _properties.FillColor;
            cmbFillTransparent.Text = _properties.FillColorAlpha.ToString();
            btnLineColor.CenterColor = _properties.BorderColor;
            cmbLineWeight.Text = _properties.PenWidth.ToString() + " pt";

            this.Refresh();
        }
    }
}
