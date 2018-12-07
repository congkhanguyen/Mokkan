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
    /// Brightness adjustment form
    /// </summary>
    public partial class MkaAdjustBrightnessContrast : Form
    {
        private Rectangle _rect;    // preview picture box size
        private Bitmap _previewImage;

        /// <summary>
        /// Brightness
        /// </summary>
        private int _brightness;
        public int Brightness 
        { 
            get { return _brightness; }
            set
            {
                _brightness = value;

                Bitmap tmp = ImageAdjustment.AdjustBrightnessContrast(_previewImage, _rect, _brightness, _contrast);
                if (tmp != null)
                {
                    if (pbPreview.Image != null)
                        pbPreview.Image.Dispose();
                    pbPreview.Image = tmp;
                } 
            }
        }

        /// <summary>
        /// Contrast
        /// </summary>
        private int _contrast;
        public int Contrast
        {
            get { return _contrast; }
            set
            {
                _contrast = value;

                Bitmap tmp = ImageAdjustment.AdjustBrightnessContrast(_previewImage, _rect, _brightness, _contrast);
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
        public MkaAdjustBrightnessContrast(Bitmap previewImg)
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
            Brightness = 0;
            Contrast = 0;

            trackBarBrightness.Value = 0;
            trackBarContrast.Value = 0;

            txtBrightness.Text = "0";
            txtContrast.Text = "0";
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
        /// Change brightness on track bar
        /// </summary>        
        private void trackBarBrightness_Scroll(object sender, EventArgs e)
        {
            Brightness = trackBarBrightness.Value;
            txtBrightness.Text = Brightness.ToString();
        }

        /// <summary>
        /// Change contrast on track bar
        /// </summary>        
        private void trackBarContrast_Scroll(object sender, EventArgs e)
        {
            Contrast = trackBarContrast.Value;
            txtContrast.Text = Contrast.ToString();
        }

        /// <summary>
        /// Input digit, +, -, . only in text box
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
        /// Change brightness in text box
        /// </summary>
        private void txtBrightness_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (!int.TryParse(txtBrightness.Text, out val)) return;

            if (val > 100)
            {
                val = 100;
                txtBrightness.Text = "100";
            }
            else if (val < -100)
            {
                val = -100;
                txtBrightness.Text = "-100";
            }

            Brightness = val;            
            trackBarBrightness.Value = val;
        }

        /// <summary>
        /// Change contrast in text box
        /// </summary>
        private void txtContrast_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (!int.TryParse(txtContrast.Text, out val)) return;

            if (val > 100)
            {
                val = 100;
                txtContrast.Text = "100";
            }
            else if (val < -100)
            {
                val = -100;
                txtContrast.Text = "-100";
            }

            Contrast = val;
            trackBarContrast.Value = val;
        }
     }
}
