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
using MokkAnnotator.MkaDrawTools;
using MokkAnnotator.MkaToolsData;
using System.Drawing.Drawing2D;

namespace MokkAnnotator.MkaWindow
{
    /// <summary>
    /// Image zoom type
    /// </summary>
    public enum ChangeType
    {
        BoundaryChanged,
        ThresholdChanged,
        ExtractionChanged
    }

    public partial class MkaAutoExtractMokkan : Form
    {
        private Bitmap _oriBmp;
        private Bitmap _bndBmp;
        private Bitmap _binBmp;
        private int _wid;
        private int _hei;
        private Rectangle _imgRec; 
        private Rectangle _bndRec;        

        private IntRange _hue;
        private DoubleRange _sat;
        private DoubleRange _lum;
        private DoubleRange _y;
        private DoubleRange _cb;
        private DoubleRange _cr;

        private int _mokkanNum;
        private int _minArea;
        private int _sizeRatio;
        private int _tolerance;

        private bool _init;
        private Pen _pen;
        private Brush _brush;

        private List<DrawObject> _list;

        private float _oldRatio;

        /// <summary>
        /// Constructor
        /// </summary>        
        public MkaAutoExtractMokkan(Bitmap image)
        {
            InitializeComponent();

            _oldRatio = DrawObject.Ratio;
            DrawObject.Origin = new Point(0, 0);
            DrawObject.Ratio = 1;

            _oriBmp = image;            
            _wid = image.Width;
            _hei = image.Height;
            _imgRec = new Rectangle(0, 0, _wid, _hei);
            _bndRec = _imgRec;
            _list = new List<DrawObject>();

            _pen = new Pen(Color.Red, 2.0f);
            _brush = new SolidBrush(Color.FromArgb(90, Color.DeepSkyBlue));

            Init();            
        }

        /// <summary>
        /// Extracted Mokkan list
        /// </summary>
        public List<DrawObject> MokkanList { get { return _list; } }

        /// <summary>
        /// Load form
        /// </summary>        
        private void MkaAutoExtractMokkan_Load(object sender, EventArgs e)
        {            
            Reset();
        }
        
        /// <summary>
        /// Release memory
        /// </summary>
        private void MkaAutoExtractMokkan_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_bndBmp != null)
                _bndBmp.Dispose();
            if (_binBmp != null)
                _binBmp.Dispose();
            if (picImageView.Image != null)
                picImageView.Image.Dispose();

            DrawObject.Ratio = _oldRatio;
        }

        /// <summary>
        /// Save settings for the next time
        /// </summary>        
        private void btnOK_Click(object sender, EventArgs e)
        {
            MokkanExtraction.Hue = _hue;
            MokkanExtraction.Saturation = _sat;
            MokkanExtraction.Luminance = _lum;
            MokkanExtraction.Y = _y;
            MokkanExtraction.Cb = _cb;
            MokkanExtraction.Cr = _cr;

            MkaDefine.MOKKAN_NUM = _mokkanNum;
            MkaDefine.MIN_AREA = _minArea;
            MkaDefine.SIZE_RATIO = _sizeRatio;
            MkaDefine.TOLERANCE = _tolerance;
        }

        /// <summary>
        /// Reset form to default values
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        /// <summary>
        /// Initialize interface
        /// </summary>
        private void Init()
        {
            _init = true;

            // initialize boundary of mokkan area            
            int half = (int)(_wid / 2);
            nupLeft.Minimum = 0;
            nupLeft.Maximum = half;            
            nupRight.Minimum = half + 1;
            nupRight.Maximum = _wid;           
            
            half = (int)(_hei / 2);
            nupTop.Minimum = 0;
            nupTop.Maximum = half;
            nupBottom.Minimum = half + 1;
            nupBottom.Maximum = _hei;                 
       
            // initialize threshold
            nupThreshold.Minimum = 0;
            nupThreshold.Maximum = 255;            

            // initialize auto zoning settings
            nupMokkanNum.Minimum = 1;
            nupMokkanNum.Maximum = 20;
            nupMokkanMinArea.Minimum = 10;
            nupMokkanMinArea.Maximum = 200; 
            nupWidHeiRatio.Minimum = 5;
            nupWidHeiRatio.Maximum = 20;  
            nupTolerance.Minimum = 0;
            nupTolerance.Maximum = 20;

            _init = false;
        }

        /// <summary>
        /// Reset to default values
        /// </summary>
        private void Reset()
        {
            _init = true;

            // boundary settings
            _bndRec = MokkanExtraction.SearchMokkanRegion(_oriBmp, _imgRec);
            nupLeft.Value = _bndRec.Left;
            nupRight.Value = _bndRec.Right;
            nupTop.Value = _bndRec.Top;
            nupBottom.Value = _bndRec.Bottom;
            
            // filter settings
            rdbHSL.Checked = true;
            grbHSL.Enabled = true;
            rdbYCbCr.Checked = false;
            grbYCbCr.Enabled = false;
            rdbThresholding.Checked = false;
            grbThresholding.Enabled = false;
            _hue = (IntRange)MokkanExtraction.Hue.Clone();
            _sat = (DoubleRange)MokkanExtraction.Saturation.Clone();
            _lum = (DoubleRange)MokkanExtraction.Luminance.Clone();
            _y = (DoubleRange)MokkanExtraction.Y.Clone();
            _cb = (DoubleRange)MokkanExtraction.Cb.Clone();
            _cr = (DoubleRange)MokkanExtraction.Cr.Clone();
            slideH.Min = (int)(_hue.Min * 255 / 359);
            slideH.Max = (int)(_hue.Max * 255 / 359);
            slideS.Min = (int)(_sat.Min * 255);
            slideS.Max = (int)(_sat.Max * 255);
            slideL.Min = (int)(_lum.Min * 255);
            slideL.Max = (int)(_lum.Max * 255);
            slideY.Min = (int)(_y.Min * 255);
            slideY.Max = (int)(_y.Max * 255);
            slideCb.Min = (int)((_cb.Min + 0.5) * 255);
            slideCb.Max = (int)((_cb.Max + 0.5) * 255);
            slideCr.Min = (int)((_cr.Min + 0.5) * 255);
            slideCr.Max = (int)((_cr.Max + 0.5) * 255);
            lblHRange.Text = String.Format("({0}～{1})", _hue.Min, _hue.Max);
            lblSRange.Text = String.Format("({0:0.00}～{1:0.00})", _sat.Min, _sat.Max);
            lblLRange.Text = String.Format("({0:0.00}～{1:0.00})", _lum.Min, _lum.Max);
            lblYRange.Text = String.Format("({0:0.00}～{1:0.00})", _y.Min, _y.Max);
            lblCbRange.Text = String.Format("({0:0.00}～{1:0.00})", _cb.Min, _cb.Max);
            lblCrRange.Text = String.Format("({0:0.00}～{1:0.00})", _cr.Min, _cr.Max);
            cmbThresholding.SelectedIndex = 0;
            grbThreshold.Enabled = false;

            // threshold setting
            tbThreshold.Value = MkaDefine.THRESHOLD;
            nupThreshold.Value = MkaDefine.THRESHOLD;

            // auto zoning settings            
            nupMokkanNum.Text = MkaDefine.MOKKAN_NUM.ToString();
            nupMokkanMinArea.Text = MkaDefine.MIN_AREA.ToString();
            nupWidHeiRatio.Text = MkaDefine.SIZE_RATIO.ToString();
            nupTolerance.Text = MkaDefine.TOLERANCE.ToString();

            _init = false;

            ChangeBoundary();

        }

        private void boundSize_ValueChanged(object sender, EventArgs e)
        {
            ChangeBoundary();
        }

        /// <summary>
        /// Change to HSL filter mode
        /// </summary>        
        private void rdbHSL_CheckedChanged(object sender, EventArgs e)
        {
            grbHSL.Enabled = rdbHSL.Checked;             
            if (!rdbHSL.Checked) return;

            ChangeThreshold();
        }

        /// <summary>
        /// Change to YCbCr filter mode
        /// </summary>
        private void rdbYCbCr_CheckedChanged(object sender, EventArgs e)
        {
            grbYCbCr.Enabled = rdbYCbCr.Checked;            
            if (!rdbYCbCr.Checked) return;

            ChangeThreshold();
        }

        /// <summary>
        /// Change to Thresholding mode
        /// </summary>
        private void rdbThresholding_CheckedChanged(object sender, EventArgs e)
        {
            grbThresholding.Enabled = rdbThresholding.Checked;
            if (!rdbThresholding.Checked) return;

            ChangeThreshold();
        }

        #region HSL

        /// <summary>
        /// Change Hue value
        /// </summary>
        private void slideH_ValuesChanged(object sender, EventArgs e)
        {
            _hue.Min = (int)(slideH.Min * 359 / 255);
            _hue.Max = (int)(slideH.Max * 359 / 255);
            lblHRange.Text = String.Format("({0}～{1})", _hue.Min, _hue.Max);

            ChangeThreshold();
        }

        /// <summary>
        /// Change Saturation value
        /// </summary>
        private void slideS_ValuesChanged(object sender, EventArgs e)
        {
            _sat.Min = (double)slideS.Min / 255;
            _sat.Max = (double)slideS.Max / 255;
            lblSRange.Text = String.Format("({0:0.00}～{1:0.00})", _sat.Min, _sat.Max);

            ChangeThreshold();
        }

        /// <summary>
        /// Change Luminance value
        /// </summary>
        private void slideL_ValuesChanged(object sender, EventArgs e)
        {
            _lum.Min = (double)slideL.Min / 255;
            _lum.Max = (double)slideL.Max / 255;
            lblLRange.Text = String.Format("({0:0.00}～{1:0.00})", _lum.Min, _lum.Max);

            ChangeThreshold();
        }        

        #endregion HSL

        #region YCbCr

        /// <summary>
        /// Change Y value
        /// </summary>
        private void slideY_ValuesChanged(object sender, EventArgs e)
        {
            _y.Min = (double)slideY.Min / 255;
            _y.Max = (double)slideY.Max / 255;
            lblYRange.Text = String.Format("({0:0.00}～{1:0.00})", _y.Min, _y.Max);

            ChangeThreshold();
        }

        /// <summary>
        /// Change Cb value
        /// </summary>
        private void slideCb_ValuesChanged(object sender, EventArgs e)
        {
            _cb.Min = (double)slideCb.Min / 255 - 0.5;
            _cb.Max = (double)slideCb.Max / 255 - 0.5;
            lblCbRange.Text = String.Format("({0:0.00}～{1:0.00})", _cb.Min, _cb.Max);

            ChangeThreshold();
        }

        /// <summary>
        /// Change Cr value
        /// </summary>        
        private void slideCr_ValuesChanged(object sender, EventArgs e)
        {
            _cr.Min = (double)slideCr.Min / 255 - 0.5;
            _cr.Max = (double)slideCr.Max / 255 - 0.5;
            lblCrRange.Text = String.Format("({0:0.00}～{1:0.00})", _cr.Min, _cr.Max);

            ChangeThreshold();
        }       
        
        #endregion YCbCr

        #region Thresholding
        
        /// <summary>
        /// Change Thresholding method
        /// </summary>
        private void cmbThresholding_SelectedIndexChanged(object sender, EventArgs e)
        {
            grbThreshold.Enabled = false;  
            ChangeThreshold();
        }

        /// <summary>
        /// Change threshold value
        /// </summary>
        private void tbThreshold_Scroll(object sender, EventArgs e)
        {
            nupThreshold.Text = tbThreshold.Value.ToString();
        }

        /// <summary>
        /// Change threshold value
        /// </summary>        
        private void nupThreshold_ValueChanged(object sender, EventArgs e)
        {
            tbThreshold.Value = (int)nupThreshold.Value;
            ChangeThreshold();
        } 

        #endregion Thresholding 

        /// <summary>
        /// Parameters for auto extraction changed
        /// </summary>        
        private void parameters_ValueChanged(object sender, EventArgs e)
        {
            _mokkanNum = (int)nupMokkanNum.Value;
            _minArea = (int)nupMokkanMinArea.Value;
            _sizeRatio = (int)nupWidHeiRatio.Value;
            _tolerance = (int)nupTolerance.Value;

            ChangeExtraction();
        }

        /// <summary>
        /// Change boundary of processing area
        /// </summary>
        private void ChangeBoundary()
        {
            if (_init) return;

            this.Cursor = Cursors.WaitCursor;

            _bndRec = Rectangle.FromLTRB((int)nupLeft.Value, (int)nupTop.Value, (int)nupRight.Value, (int)nupBottom.Value);
            if (_bndBmp != null)
                _bndBmp.Dispose();
            _bndBmp = MokkanExtraction.ExtractMokkanRegion(_oriBmp, _bndRec);
            
            ChangeThreshold();

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Change thresholding method
        /// </summary>
        private void ChangeThreshold()
        {
            if (_init) return;

            this.Cursor = Cursors.WaitCursor;

            if (_binBmp != null)
                _binBmp.Dispose();

            if(rdbHSL.Checked)
            {
                _binBmp = MokkanExtraction.HSLFilter(_oriBmp, _bndRec, _hue, _sat, _lum);
                
            }
            else if(rdbYCbCr.Checked)
            {
                _binBmp = MokkanExtraction.YCbCrFilter(_oriBmp, _bndRec, _y, _cb, _cr);
            }
            else
            {
                Binarization.ExtractBackground = true;
                switch (cmbThresholding.SelectedItem.ToString())
                {
                    case "MaxEntropy法":
                        _binBmp = Binarization.MaxEntropyThreshold(_bndBmp, _bndRec);
                        break;
                    case "Otsu法":
                        _binBmp = Binarization.OtsuThreshold(_bndBmp, _bndRec);
                        break;
                    case "SIS法":
                        _binBmp = Binarization.SISThreshold(_bndBmp, _bndRec);
                        break;
                    case "Huang法":
                        _binBmp = Binarization.HuangThreshold(_bndBmp, _bndRec);
                        break;
                    case "Yen法":
                        _binBmp = Binarization.YenThreshold(_bndBmp, _bndRec);
                        break;
                    case "Li法":
                        _binBmp = Binarization.LiThreshold(_bndBmp, _bndRec);
                        break;
                    case "Mean法":
                        _binBmp = Binarization.MeanThreshold(_bndBmp, _bndRec);
                        break;
                    case "Moment法":
                        _binBmp = Binarization.MomentsThreshold(_bndBmp, _bndRec);
                        break;
                    case "Iterative法":
                        _binBmp = Binarization.IteractiveThreshold(_bndBmp, _bndRec, 0, 128);
                        break;
                    case "IsoData法":
                        _binBmp = Binarization.IsoDataThreshold(_bndBmp, _bndRec);
                        break;
                    case "Percentile法":
                        _binBmp = Binarization.PercentileThreshold(_bndBmp, _bndRec);
                        break;
                    case "閾値設定":
                        grbThreshold.Enabled = true;
                        _binBmp = Binarization.Threshold(_bndBmp, _bndRec, (int)nupThreshold.Value);
                        break;
                }
                Binarization.ExtractBackground = false;
            }

            ChangeExtraction();

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Change mokkan extraction parameters
        /// </summary>
        private void ChangeExtraction()
        {
            if (_init) return;

            this.Cursor = Cursors.WaitCursor;

            List<Boundary> bounds = MokkanExtraction.BoundaryTracking(_binBmp, _bndRec);

            // get top regions
            List<int> tops = new List<int>();
            GetTopAreas(bounds, _mokkanNum, tops);

            // number of regions
            int n = Math.Min(_mokkanNum, tops.Count - 1);

            DrawPolygon reducePolygon;
            MkaMokkanInfo.LastRBangou -= _list.Count;
            _list.Clear();
            Bitmap ret = (Bitmap)_oriBmp.Clone();
            Graphics g = Graphics.FromImage(ret);
            g.DrawRectangle(Pens.Red, _bndRec);

            for (int i = 0; i < n; i++)
            {     
                if(_tolerance == 0)
                {
                    reducePolygon = new DrawPolygon();                    
                    foreach (PointF p in bounds[tops[i]].Points)
                        reducePolygon.AddPoint(new Point((int)p.X, (int)p.Y));
                }
                else
                {
                    // get reduce polygon
                    reducePolygon = PolygonApproximation.ReductionPolygon(bounds[tops[i]].Points, _tolerance);
                }                

                _list.Add(reducePolygon);
                MkaMokkanInfo.LastRBangou++;

                g.DrawPolygon(_pen, bounds[tops[i]].Points.ToArray());
                g.FillPolygon(_brush, bounds[tops[i]].Points.ToArray());
            }

            g.Dispose();
            bounds.Clear();
            tops.Clear();

            if (picImageView.Image != null)
                picImageView.Image.Dispose();
            picImageView.Image = ret;

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Get top area of regions
        /// </summary>        
        private void GetTopAreas(List<Boundary> bounds, int topCount, List<int> topIndex)
        {
            int i, j, area;
            List<int> topArea = new List<int>();
            topArea.Add(-1);
            topIndex.Add(-1);
            Boundary bound;

            for (i = 0; i < bounds.Count; i++)
            {
                bound = bounds[i];

                if (bound.WidHeiRatio > _sizeRatio)
                    continue;

                area = bound.BoundArea;
                if (area < _minArea || bound.BoundPointCount < _minArea)
                    continue;

                for (j = 0; j < topCount; j++)
                {
                    if (area > topArea[j])
                    {
                        topIndex.Insert(j, i);
                        topArea.Insert(j, area);
                        break;
                    }
                }
            }
        }
    }
}
