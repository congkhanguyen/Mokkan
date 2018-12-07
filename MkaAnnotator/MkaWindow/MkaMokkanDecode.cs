using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MokkAnnotator.MkaImageProcess;
using MokkAnnotator.MkaCommon;
using System.IO;

namespace MokkAnnotator.MkaWindow
{
    public partial class MkaMokkanDecode : Form
    {        
        private Bitmap _image;      // image of mokkan
        private String _filename;   // file name
        private String _path;       // document path
        private Rectangle _rect;    // boundary of image
        private List<Image> _hisImg = new List<Image>();    // history list of processed images
        private const int MaxNumOfImages = 7;   // number of images can be stored in history list
        private int _curIndex;                  // current processed image index
        private bool _leave = false;            // focus leave flag
        private bool _changed = true;

        RGB rgb = new RGB();
        List<RGB> rgbs = new List<RGB>();
        HSV hsv = new HSV();
        List<HSV> hsvs = new List<HSV>();
        LAB lab = new LAB();
        List<LAB> labs = new List<LAB>();

        /// <summary>
        /// Constructor
        /// </summary>        
        public MkaMokkanDecode()
        {
            InitializeComponent();

            // initialize controls
            InitControls();
        }

        /// <summary>
        /// Constructor
        /// </summary>        
        public MkaMokkanDecode(Bitmap img, int rid, String path)
        {
            InitializeComponent();

            // initialize controls
            InitControls();

            // initialize image
            InitImage(img, rid.ToString(), path);
        }

        /// <summary>
        /// Initialize control
        /// </summary>
        private void InitControls()
        {
            btnBack.Enabled = false;
            btnForward.Enabled = false;

            rgbs.Clear();
            labs.Clear();
            hsvs.Clear();

            rdbProcNone.Checked = true;
            rdbSimNone.Checked = true; 
            rdbBinNone.Checked = true;

            trackBarBrightness.Value = 0;
            trackBarContrast.Value = 0;
            trackBarHue.Value = 0;
            trackBarSaturation.Value = 100;
            trackBarLightness.Value = 0;
            trackBarSimilarity.Value = 10;
            trackBarThreshold.Value = 150;
        }

        /// <summary>
        /// Initialize image
        /// </summary>        
        private void InitImage(Bitmap img, String fileName, String path)
        {
            _image = img;
            _filename = fileName;
            _path = path;
            _rect = new Rectangle(0, 0, img.Width, img.Height);

            picInput.Image = (Bitmap)_image.Clone();
            picOutput.Image = (Bitmap)_image.Clone();
            
            _hisImg.Clear();
            _hisImg.Add(_image);
            _curIndex = 0;

            btnOriginal.Enabled = true;
            btnMoveToProcess.Enabled = true;
            btnSave.Enabled = true;

            grbProcess.Enabled = true;
            grbBrightContrast.Enabled = true;
            grbHSL.Enabled = true;
            grbSimilarity.Enabled = true;
            grbBinarization.Enabled = true;
        }

        /// <summary>
        /// Image processing
        /// </summary>        
        private void rdbProc_CheckedChanged(object sender, EventArgs e)
        {
            _changed = !_changed;
            if (_changed) return;            

            if (picOutput.Image != null)
                picOutput.Image.Dispose();
            if(rdbProcPseudoColor.Checked)
                picOutput.Image = TakakuraImplement.PseudoColorFilter((Bitmap)picInput.Image, _rect);
            else if(rdbProcHSVStretch.Checked)
                picOutput.Image = TakakuraImplement.HistogramStretchHSV((Bitmap)picInput.Image, _rect);
            else if(rdbProcRGBStretch.Checked)
                picOutput.Image = TakakuraImplement.HistogramStretchRGB((Bitmap)picInput.Image, _rect);
            else if (!_leave && picInput.Image != null)
                picOutput.Image = (Bitmap)picInput.Image.Clone();
        }

        /// <summary>
        /// Brightness and contrast adjustment
        /// </summary>        
        private void trackBarBrightContrast_Scroll(object sender, EventArgs e)
        {           
           TrackBar ctrl = (TrackBar)sender;
           toolTipValue.SetToolTip(ctrl, ctrl.Value.ToString());
           if (picOutput.Image != null)
               picOutput.Image.Dispose();
            picOutput.Image = ImageAdjustment.AdjustBrightnessContrast((Bitmap)picInput.Image, _rect, trackBarBrightness.Value, trackBarContrast.Value);
        }

        /// <summary>
        /// Hue, saturation and lightness adjustment
        /// </summary>        
        private void trackBarHSL_Scroll(object sender, EventArgs e)
        {
            TrackBar ctrl = (TrackBar)sender;
            toolTipValue.SetToolTip(ctrl, ctrl.Value.ToString());

            int _saturation = trackBarSaturation.Value;
            if (_saturation > 100)
                _saturation = ((_saturation - 100) * 3) + 100;
            _saturation = (_saturation * 1024) / 100;

            if (picOutput.Image != null)
                picOutput.Image.Dispose();
            picOutput.Image = ImageAdjustment.AdjustHSL((Bitmap)picInput.Image, _rect, trackBarHue.Value, _saturation, trackBarLightness.Value);
        }

        /// <summary>
        /// Similarity selection
        /// </summary>        
        private void rdbSimilarity_CheckedChanged(object sender, EventArgs e)
        {
            _changed = !_changed;
            if (_changed) return;            

            if (picOutput.Image != null)
                picOutput.Image.Dispose();
            if (rdbSimRGB.Checked)
                picOutput.Image = TakakuraImplement.Similiar_RGB((Bitmap)picInput.Image, _rect, rgbs, trackBarSimilarity.Value);
            else if (rdbSimRGBAver.Checked)
                picOutput.Image = TakakuraImplement.Similiar_RGBAver((Bitmap)picInput.Image, _rect, rgbs, trackBarSimilarity.Value);
            else if (rdbSimLAB.Checked)
                picOutput.Image = TakakuraImplement.Similiar_LAB((Bitmap)picInput.Image, _rect, labs, trackBarSimilarity.Value);
            else if (rdbSimLABAver.Checked)
                picOutput.Image = TakakuraImplement.Similiar_LABAver((Bitmap)picInput.Image, _rect, labs, trackBarSimilarity.Value);
            else if (rdbSimHSV.Checked)
                picOutput.Image = TakakuraImplement.Similiar_HSV((Bitmap)picInput.Image, _rect, hsvs, trackBarSimilarity.Value);
            else if (!_leave && picInput.Image != null)
                picOutput.Image = (Bitmap)picInput.Image.Clone();
        }

        /// <summary>
        /// Similarity value adjustment
        /// </summary>        
        private void trackBarSimilarity_Scroll(object sender, EventArgs e)
        {
            TrackBar ctrl = (TrackBar)sender;
            toolTipValue.SetToolTip(ctrl, ctrl.Value.ToString());

            if (picOutput.Image != null)
                picOutput.Image.Dispose();
            if (rdbSimRGB.Checked)
                picOutput.Image = TakakuraImplement.Similiar_RGB((Bitmap)picInput.Image, _rect, rgbs, trackBarSimilarity.Value);
            else if (rdbSimRGBAver.Checked)
                picOutput.Image = TakakuraImplement.Similiar_RGBAver((Bitmap)picInput.Image, _rect, rgbs, trackBarSimilarity.Value);
            else if (rdbSimLAB.Checked)
                picOutput.Image = TakakuraImplement.Similiar_LAB((Bitmap)picInput.Image, _rect, labs, trackBarSimilarity.Value);
            else if (rdbSimLABAver.Checked)
                picOutput.Image = TakakuraImplement.Similiar_LABAver((Bitmap)picInput.Image, _rect, labs, trackBarSimilarity.Value);
            else if (rdbSimHSV.Checked)
                picOutput.Image = TakakuraImplement.Similiar_HSV((Bitmap)picInput.Image, _rect, hsvs, trackBarSimilarity.Value);
            else if(picInput.Image != null)
                picOutput.Image = (Bitmap)picInput.Image.Clone();
        }

        /// <summary>
        /// Get pixel
        /// </summary>        
        private void picInput_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = picInput.CreateGraphics();
            Bitmap img = (Bitmap)picInput.Image;
            Point pix = new Point();
            if (e.Button == MouseButtons.Left && !rdbSimNone.Checked)
            {
                g.DrawEllipse(Pens.Red, e.X, e.Y, 1, 1);
                pix = TransformPosition(img, e.Location);
                rgb = new RGB(img.GetPixel(pix.X, pix.Y));
                lab = LAB.FromRGB(rgb);
                hsv = HSV.FromRGB(rgb);
                rgbs.Add(rgb);
                labs.Add(lab);
                hsvs.Add(hsv);
                
                if (picOutput.Image != null)
                    picOutput.Image.Dispose();

                if (rdbSimRGB.Checked)
                    picOutput.Image = TakakuraImplement.Similiar_RGB((Bitmap)picInput.Image, _rect, rgbs, trackBarSimilarity.Value);
                else if (rdbSimRGBAver.Checked)
                    picOutput.Image = TakakuraImplement.Similiar_RGBAver((Bitmap)picInput.Image, _rect, rgbs, trackBarSimilarity.Value);
                else if (rdbSimLAB.Checked)
                    picOutput.Image = TakakuraImplement.Similiar_LAB((Bitmap)picInput.Image, _rect, labs, trackBarSimilarity.Value);
                else if (rdbSimLABAver.Checked)
                    picOutput.Image = TakakuraImplement.Similiar_LABAver((Bitmap)picInput.Image, _rect, labs, trackBarSimilarity.Value);
                else
                    picOutput.Image = TakakuraImplement.Similiar_HSV((Bitmap)picInput.Image, _rect, hsvs, trackBarSimilarity.Value);
            }
            g.Dispose();            
        }

        /// <summary>
        /// Gets the mouse position over the image when the PictureBox's SizeMode is set to Zoom
        /// </summary>
        private Point TransformPosition(Bitmap image, Point coordinates)
        {
            // test to make sure our image is not null
            if (image == null) return coordinates;

            float imageAspect = (float)image.Width / image.Height;
            float controlAspect = (float)picInput.Width / picInput.Height;
            float newX = coordinates.X;
            float newY = coordinates.Y;
            if (imageAspect > controlAspect)
            {
                // This means that we are limited by width, 
                // meaning the image fills up the entire control from left to right
                float ratioWidth = (float)image.Width / picInput.Width;
                newX *= ratioWidth;
                float scale = (float)picInput.Width / image.Width;
                float displayHeight = scale * image.Height;
                float diffHeight = picInput.Height - displayHeight;
                diffHeight /= 2;
                newY -= diffHeight;
                newY /= scale;
            }
            else
            {
                // This means that we are limited by height, 
                // meaning the image fills up the entire control from top to bottom
                float ratioHeight = (float)image.Height / picInput.Height;
                newY *= ratioHeight;
                float scale = (float)picInput.Height / image.Height;
                float displayWidth = scale * image.Width;
                float diffWidth = picInput.Width - displayWidth;
                diffWidth /= 2;
                newX -= diffWidth;
                newX /= scale;
            }
            return new Point((int)newX, (int)newY);
        }

        /// <summary>
        /// Binarization method selection
        /// </summary>        
        private void rdbBinarization_CheckedChanged(object sender, EventArgs e)
        {
            _changed = !_changed; 
            if (_changed) return;            

            lblThreshold.Enabled = false;
            trackBarThreshold.Enabled = false;
            
            if (picOutput.Image != null)
                picOutput.Image.Dispose();

            if (rdbBinDA.Checked)
                picOutput.Image = TakakuraImplement.DiscriminantAnalysis((Bitmap)picInput.Image, _rect);
            else if (rdbBinSIS.Checked)
                picOutput.Image = Binarization.SISThreshold((Bitmap)picInput.Image, _rect);
            else if (rdbBinOtsu.Checked)
                picOutput.Image = Binarization.OtsuThreshold((Bitmap)picInput.Image, _rect);
            else if (rdbBinLi.Checked)
                picOutput.Image = Binarization.LiThreshold((Bitmap)picInput.Image, _rect);
            else if (rdbBinHuang.Checked)
                picOutput.Image = Binarization.HuangThreshold((Bitmap)picInput.Image, _rect);
            else if (rdbBinYen.Checked)
                picOutput.Image = Binarization.YenThreshold((Bitmap)picInput.Image, _rect);            
            else if (rdbBinMean.Checked)
                picOutput.Image = Binarization.MeanThreshold((Bitmap)picInput.Image, _rect);
            else if (rdbBinPercentile.Checked)
                picOutput.Image = Binarization.PercentileThreshold((Bitmap)picInput.Image, _rect);
            else if (rdbBinIterative.Checked)
                picOutput.Image = Binarization.IteractiveThreshold((Bitmap)picInput.Image, _rect, 0, 180);
            else if (rdbBinMoments.Checked)
                picOutput.Image = Binarization.MomentsThreshold((Bitmap)picInput.Image, _rect);
            else if (rdbBinIsoData.Checked)
                picOutput.Image = Binarization.IsoDataThreshold((Bitmap)picInput.Image, _rect);
            else if (rdbBinAdjust.Checked)
            {
                lblThreshold.Enabled = true;
                trackBarThreshold.Enabled = true;
                picOutput.Image = Binarization.Threshold((Bitmap)picInput.Image, _rect, trackBarThreshold.Value);
            }
            else if (!_leave && picInput.Image != null)
                picOutput.Image = (Bitmap)picInput.Image.Clone();
        }

        /// <summary>
        /// Threshold adjustment
        /// </summary>        
        private void trackBarThreshold_Scroll(object sender, EventArgs e)
        {
            TrackBar ctrl = (TrackBar)sender;
            toolTipValue.SetToolTip(ctrl, ctrl.Value.ToString());

            if (picOutput.Image != null)
                picOutput.Image.Dispose();
            picOutput.Image = Binarization.Threshold((Bitmap)picInput.Image, _rect, trackBarThreshold.Value);
        }

        /// <summary>
        /// Reset to original image
        /// </summary>        
        private void btnOriginal_Click(object sender, EventArgs e)
        {
            picInput.Image = (Bitmap)_image.Clone();
            picOutput.Image = (Bitmap)_image.Clone();

            _hisImg.Clear();
            _hisImg.Add(_image);
            _curIndex = 0;

            btnOriginal.Enabled = true;
            btnMoveToProcess.Enabled = true;
            btnSave.Enabled = true;
        }

        /// <summary>
        /// Move processed image to process image
        /// </summary>
        private void btnMoveToProcess_Click(object sender, EventArgs e)        
        {
            if (picInput.Image != null)
                picInput.Image.Dispose();
            picInput.Image = (Bitmap)picOutput.Image.Clone();

            _hisImg.RemoveRange(_curIndex + 1, _hisImg.Count - _curIndex - 1);
            if (_hisImg.Count == MaxNumOfImages)            
                _hisImg.RemoveAt(0);
            _hisImg.Insert(_hisImg.Count, picOutput.Image);

            _curIndex = _hisImg.Count - 1;            
            btnBack.Enabled = true;
            btnForward.Enabled = false;
        }

        /// <summary>
        /// Backward button
        /// </summary>        
        private void btnBack_Click(object sender, EventArgs e)
        {
            _curIndex--;
            if (picInput.Image != null)
                picInput.Image.Dispose();
            picInput.Image = (Bitmap)_hisImg[_curIndex].Clone();
            if (_curIndex == 0) btnBack.Enabled = false;
            else btnBack.Enabled = true;
            if (_curIndex == _hisImg.Count - 1) btnForward.Enabled = false;
            else btnForward.Enabled = true;
        }

        /// <summary>
        /// Forward button
        /// </summary>        
        private void btnForward_Click(object sender, EventArgs e)
        {            
            _curIndex++;
            if (picInput.Image != null)
                picInput.Image.Dispose();
            picInput.Image = (Bitmap)_hisImg[_curIndex].Clone();
            if (_curIndex == 0) btnBack.Enabled = false;
            else btnBack.Enabled = true;
            if (_curIndex == _hisImg.Count - 1) btnForward.Enabled = false;
            else btnForward.Enabled = true;            
        }

        /// <summary>
        /// Reset controls when leave Image Process group processing
        /// </summary>
        private void grbProcess_Leave(object sender, EventArgs e)
        {
            _leave = true;

            rdbProcNone.Checked = true;

            _leave = false;
        }

        /// <summary>
        /// Reset controls when leave Brightness/Contrast group processing
        /// </summary>
        private void grbBrightContrast_Leave(object sender, EventArgs e)
        {
            trackBarBrightness.Value = 0;
            trackBarContrast.Value = 0;
        }

        /// <summary>
        /// Reset controls when leave Hue/Saturation group processing
        /// </summary>
        private void grbHSL_Leave(object sender, EventArgs e)
        {
            _leave = true;

            trackBarHue.Value = 0;
            trackBarSaturation.Value = 100;
            trackBarLightness.Value = 0;
        }

        /// <summary>
        /// Reset controls when leave Similarity group processing
        /// </summary>
        private void grbSimilarity_Leave(object sender, EventArgs e)
        {
            _leave = true;

            rdbSimNone.Checked = true;
            trackBarSimilarity.Value = 10;
            picInput.Refresh();

            rgbs.Clear();
            labs.Clear();
            hsvs.Clear();

            _leave = false;
        }

        /// <summary>
        /// Reset controls when leave Binarization group processing
        /// </summary>        
        private void grbBinarization_Leave(object sender, EventArgs e)
        {
            _leave = true;

            rdbBinNone.Checked = true;

            _leave = false;
        }

        /// <summary>
        /// Save processed image to file
        /// </summary>        
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = MkaDefine.IOImageFilter; ;
            sfd.InitialDirectory = Path.GetDirectoryName(_path);            
            sfd.FileName = _filename;

            if (sfd.ShowDialog(this) != DialogResult.OK)
                return;
                        
            picOutput.Image.Save(sfd.FileName);
        }

        /// <summary>
        /// Open image file
        /// </summary>        
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = MkaDefine.IOImageFilterAll + "|" + MkaDefine.IOImageFilter;
            ofd.Multiselect = false;

            if (ofd.ShowDialog(this) != DialogResult.OK)
                return;

            // read image file            
            Bitmap img = new Bitmap(Bitmap.FromFile(ofd.FileName));                        

            // initialize control
            InitControls();

            // initialize image
            InitImage(img, ofd.SafeFileName, ofd.FileName);            
        }
    }
}
