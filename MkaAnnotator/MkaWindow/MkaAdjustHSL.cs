using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MokkAnnotator.MkaImageProcess;

namespace MokkAnnotator.MkaWindow
{
    /// <summary>
    /// Hue saturation lightness adjustment form
    /// </summary>
    public partial class MkaAdjustHSL : Form
    {
        private Rectangle _rect;    // preview picture box size
        private Bitmap _previewImage;

        /// <summary>
        /// Hue
        /// </summary>
        private int _hue;
        public int Hue 
        { 
            get { return _hue; }
            set
            {
                _hue = value;
                Bitmap tmp = ImageAdjustment.AdjustHSL(_previewImage, _rect, _hue, _saturation, _lightness);
                if(tmp != null)
                {
                    if (pbPreview.Image != null)
                        pbPreview.Image.Dispose();
                    pbPreview.Image = tmp;
                }                
            }
        }

        /// <summary>
        /// Saturation
        /// </summary>
        private int _saturation;
        public int Saturation
        {
            get { return _saturation; }
            set
            {
                _saturation = value;

                // map the range [0,100] -> [0,100] and the range [101,200] -> [103,400]
                if (_saturation > 100)                
                    _saturation = ((_saturation - 100) * 3) + 100;

                _saturation = (_saturation * 1024) / 100;

                Bitmap tmp = ImageAdjustment.AdjustHSL(_previewImage, _rect, _hue, _saturation, _lightness);
                if (tmp != null)
                {
                    if (pbPreview.Image != null)
                        pbPreview.Image.Dispose();
                    pbPreview.Image = tmp;
                } 
            }
        }

        /// <summary>
        /// Lightness
        /// </summary>
        private int _lightness;
        public int Lightness
        {
            get { return _lightness; }
            set
            {
                _lightness = value;

                Bitmap tmp = ImageAdjustment.AdjustHSL(_previewImage, _rect, _hue, _saturation, _lightness);
                if (tmp != null)
                {
                    if (pbPreview.Image != null)
                        pbPreview.Image.Dispose();
                    pbPreview.Image = tmp;
                } 
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MkaAdjustHSL(Bitmap previewImg)
        {
            InitializeComponent();

            _previewImage = previewImg;            
            _rect = new Rectangle(0, 0, 200, 200);

            // initialize
            Init();            
        }

        /// <summary>
        /// Initialize
        /// </summary>
        private void Init()
        {            
            Hue = 0;
            Saturation = 100;
            Lightness = 0;

            trackBarHue.Value = 0;
            trackBarSaturation.Value = 100;
            trackBarLightness.Value = 0;

            txtHue.Text = "0";
            txtSaturation.Text = "100";
            txtLightness.Text = "0";
        }

        /// <summary>
        /// Reset value 
        /// </summary>        
        private void btnReset_Click(object sender, EventArgs e)
        {
            // initialize
            Init();
        }

        /// <summary>
        /// Change hue on track bar
        /// </summary>        
        private void trackBarBrightness_Scroll(object sender, EventArgs e)
        {
            Hue = trackBarHue.Value;
            txtHue.Text = trackBarHue.Value.ToString();
        }

        /// <summary>
        /// Change saturation on track bar
        /// </summary>        
        private void trackBarSaturation_Scroll(object sender, EventArgs e)
        {
            Saturation = trackBarSaturation.Value;
            txtSaturation.Text = trackBarSaturation.Value.ToString();
        }

        /// <summary>
        /// Change lightness on track bar
        /// </summary>        
        private void trackBarLightness_Scroll(object sender, EventArgs e)
        {
            Lightness = trackBarLightness.Value;
            txtLightness.Text = trackBarLightness.Value.ToString();
        }

        /// <summary>
        /// Input digit, - only in text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && 
                e.KeyChar != 45 && 
                e.KeyChar != 8)
                e.Handled = true;
        }

        /// <summary>
        /// Change hue in text box
        /// </summary>
        private void txtHue_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (!int.TryParse(txtHue.Text, out val)) return;

            if (val > 180)
            {
                val = 180;
                txtHue.Text = "180";
            }
            else if (val < -180)
            {
                val = -180;
                txtHue.Text = "-180";
            }

            Hue = val;            
            trackBarHue.Value = val;
        }

        /// <summary>
        /// Change saturation in text box
        /// </summary>
        private void txtSaturation_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (!int.TryParse(txtSaturation.Text, out val)) return;

            if (val > 200)
            {
                val = 200;
                txtSaturation.Text = "200";
            }
            else if (val < 0)
            {
                val = 0;
                txtSaturation.Text = "0";
            }

            Saturation = val;
            trackBarSaturation.Value = val;
        }

        /// <summary>
        /// Change lightness in text box
        /// </summary>
        private void txtLightness_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (!int.TryParse(txtLightness.Text, out val)) return;

            if (val > 100)
            {
                val = 100;
                txtLightness.Text = "100";
            }
            else if (val < -100)
            {
                val = -100;
                txtLightness.Text = "-100";
            }

            Lightness = val;
            trackBarLightness.Value = val;
        }
     }
}
