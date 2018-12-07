namespace MokkAnnotator.MkaWindow
{
    partial class MkaAutoExtractMokkan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnImageView = new System.Windows.Forms.Panel();
            this.picImageView = new System.Windows.Forms.PictureBox();
            this.grbBoundaryTracking = new System.Windows.Forms.GroupBox();
            this.rdbThresholding = new System.Windows.Forms.RadioButton();
            this.grbThresholding = new System.Windows.Forms.GroupBox();
            this.lblThresholding = new System.Windows.Forms.Label();
            this.grbThreshold = new System.Windows.Forms.GroupBox();
            this.nupThreshold = new System.Windows.Forms.NumericUpDown();
            this.lbl255 = new System.Windows.Forms.Label();
            this.lbl0 = new System.Windows.Forms.Label();
            this.tbThreshold = new System.Windows.Forms.TrackBar();
            this.cmbThresholding = new System.Windows.Forms.ComboBox();
            this.rdbYCbCr = new System.Windows.Forms.RadioButton();
            this.rdbHSL = new System.Windows.Forms.RadioButton();
            this.grbYCbCr = new System.Windows.Forms.GroupBox();
            this.lblCrRange = new System.Windows.Forms.Label();
            this.lblCr = new System.Windows.Forms.Label();
            this.lblCbRange = new System.Windows.Forms.Label();
            this.slideCr = new MokkAnnotator.MkaWindow.MkaColorSlider();
            this.lblYRange = new System.Windows.Forms.Label();
            this.lblCb = new System.Windows.Forms.Label();
            this.slideCb = new MokkAnnotator.MkaWindow.MkaColorSlider();
            this.lblY = new System.Windows.Forms.Label();
            this.slideY = new MokkAnnotator.MkaWindow.MkaColorSlider();
            this.grbHSL = new System.Windows.Forms.GroupBox();
            this.lblLRange = new System.Windows.Forms.Label();
            this.lblSRange = new System.Windows.Forms.Label();
            this.lblHRange = new System.Windows.Forms.Label();
            this.lblL = new System.Windows.Forms.Label();
            this.slideL = new MokkAnnotator.MkaWindow.MkaColorSlider();
            this.lblS = new System.Windows.Forms.Label();
            this.slideS = new MokkAnnotator.MkaWindow.MkaColorSlider();
            this.lblH = new System.Windows.Forms.Label();
            this.slideH = new MokkAnnotator.MkaWindow.MkaColorSlider();
            this.nupLeft = new System.Windows.Forms.NumericUpDown();
            this.grbMokkanArea = new System.Windows.Forms.GroupBox();
            this.lblBottom = new System.Windows.Forms.Label();
            this.nupBottom = new System.Windows.Forms.NumericUpDown();
            this.lblTop = new System.Windows.Forms.Label();
            this.nupTop = new System.Windows.Forms.NumericUpDown();
            this.lblRight = new System.Windows.Forms.Label();
            this.nupRight = new System.Windows.Forms.NumericUpDown();
            this.lblLeft = new System.Windows.Forms.Label();
            this.nupMokkanNum = new System.Windows.Forms.NumericUpDown();
            this.lblMokkanNum = new System.Windows.Forms.Label();
            this.lblMokkanMinArea = new System.Windows.Forms.Label();
            this.nupMokkanMinArea = new System.Windows.Forms.NumericUpDown();
            this.lblWidHeiRatio = new System.Windows.Forms.Label();
            this.nupWidHeiRatio = new System.Windows.Forms.NumericUpDown();
            this.lblTolerance = new System.Windows.Forms.Label();
            this.nupTolerance = new System.Windows.Forms.NumericUpDown();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnImageView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImageView)).BeginInit();
            this.grbBoundaryTracking.SuspendLayout();
            this.grbThresholding.SuspendLayout();
            this.grbThreshold.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbThreshold)).BeginInit();
            this.grbYCbCr.SuspendLayout();
            this.grbHSL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupLeft)).BeginInit();
            this.grbMokkanArea.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupMokkanNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupMokkanMinArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupWidHeiRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupTolerance)).BeginInit();
            this.SuspendLayout();
            // 
            // pnImageView
            // 
            this.pnImageView.AutoScroll = true;
            this.pnImageView.BackColor = System.Drawing.Color.White;
            this.pnImageView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnImageView.Controls.Add(this.picImageView);
            this.pnImageView.Location = new System.Drawing.Point(388, 4);
            this.pnImageView.Name = "pnImageView";
            this.pnImageView.Size = new System.Drawing.Size(606, 434);
            this.pnImageView.TabIndex = 13;
            // 
            // picImageView
            // 
            this.picImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picImageView.Location = new System.Drawing.Point(0, 0);
            this.picImageView.Name = "picImageView";
            this.picImageView.Size = new System.Drawing.Size(602, 430);
            this.picImageView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImageView.TabIndex = 0;
            this.picImageView.TabStop = false;
            // 
            // grbBoundaryTracking
            // 
            this.grbBoundaryTracking.Controls.Add(this.rdbThresholding);
            this.grbBoundaryTracking.Controls.Add(this.grbThresholding);
            this.grbBoundaryTracking.Controls.Add(this.rdbYCbCr);
            this.grbBoundaryTracking.Controls.Add(this.rdbHSL);
            this.grbBoundaryTracking.Controls.Add(this.grbYCbCr);
            this.grbBoundaryTracking.Controls.Add(this.grbHSL);
            this.grbBoundaryTracking.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.grbBoundaryTracking.ForeColor = System.Drawing.Color.Blue;
            this.grbBoundaryTracking.Location = new System.Drawing.Point(1, 110);
            this.grbBoundaryTracking.Name = "grbBoundaryTracking";
            this.grbBoundaryTracking.Size = new System.Drawing.Size(383, 414);
            this.grbBoundaryTracking.TabIndex = 3;
            this.grbBoundaryTracking.TabStop = false;
            this.grbBoundaryTracking.Text = "木簡境界抽出";
            // 
            // rdbThresholding
            // 
            this.rdbThresholding.AutoSize = true;
            this.rdbThresholding.Location = new System.Drawing.Point(6, 307);
            this.rdbThresholding.Name = "rdbThresholding";
            this.rdbThresholding.Size = new System.Drawing.Size(60, 18);
            this.rdbThresholding.TabIndex = 4;
            this.rdbThresholding.Text = "2値化";
            this.rdbThresholding.UseVisualStyleBackColor = true;
            this.rdbThresholding.CheckedChanged += new System.EventHandler(this.rdbThresholding_CheckedChanged);
            // 
            // grbThresholding
            // 
            this.grbThresholding.Controls.Add(this.lblThresholding);
            this.grbThresholding.Controls.Add(this.grbThreshold);
            this.grbThresholding.Controls.Add(this.cmbThresholding);
            this.grbThresholding.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.grbThresholding.ForeColor = System.Drawing.Color.Black;
            this.grbThresholding.Location = new System.Drawing.Point(6, 323);
            this.grbThresholding.Name = "grbThresholding";
            this.grbThresholding.Size = new System.Drawing.Size(372, 86);
            this.grbThresholding.TabIndex = 5;
            this.grbThresholding.TabStop = false;
            // 
            // lblThresholding
            // 
            this.lblThresholding.AutoSize = true;
            this.lblThresholding.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblThresholding.Location = new System.Drawing.Point(8, 18);
            this.lblThresholding.Name = "lblThresholding";
            this.lblThresholding.Size = new System.Drawing.Size(59, 12);
            this.lblThresholding.TabIndex = 0;
            this.lblThresholding.Text = "2値化手法";
            // 
            // grbThreshold
            // 
            this.grbThreshold.Controls.Add(this.nupThreshold);
            this.grbThreshold.Controls.Add(this.lbl255);
            this.grbThreshold.Controls.Add(this.lbl0);
            this.grbThreshold.Controls.Add(this.tbThreshold);
            this.grbThreshold.Location = new System.Drawing.Point(132, 18);
            this.grbThreshold.Name = "grbThreshold";
            this.grbThreshold.Size = new System.Drawing.Size(234, 63);
            this.grbThreshold.TabIndex = 2;
            this.grbThreshold.TabStop = false;
            this.grbThreshold.Text = "閾値設定";
            // 
            // nupThreshold
            // 
            this.nupThreshold.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nupThreshold.Location = new System.Drawing.Point(164, 18);
            this.nupThreshold.Name = "nupThreshold";
            this.nupThreshold.Size = new System.Drawing.Size(64, 28);
            this.nupThreshold.TabIndex = 1;
            this.nupThreshold.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nupThreshold.ValueChanged += new System.EventHandler(this.nupThreshold_ValueChanged);
            // 
            // lbl255
            // 
            this.lbl255.AutoSize = true;
            this.lbl255.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lbl255.Location = new System.Drawing.Point(144, 47);
            this.lbl255.Name = "lbl255";
            this.lbl255.Size = new System.Drawing.Size(23, 12);
            this.lbl255.TabIndex = 3;
            this.lbl255.Text = "255";
            // 
            // lbl0
            // 
            this.lbl0.AutoSize = true;
            this.lbl0.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lbl0.Location = new System.Drawing.Point(30, 47);
            this.lbl0.Name = "lbl0";
            this.lbl0.Size = new System.Drawing.Size(11, 12);
            this.lbl0.TabIndex = 2;
            this.lbl0.Text = "0";
            // 
            // tbThreshold
            // 
            this.tbThreshold.Location = new System.Drawing.Point(22, 20);
            this.tbThreshold.Maximum = 255;
            this.tbThreshold.Name = "tbThreshold";
            this.tbThreshold.Size = new System.Drawing.Size(144, 45);
            this.tbThreshold.TabIndex = 0;
            this.tbThreshold.TickFrequency = 16;
            this.tbThreshold.Scroll += new System.EventHandler(this.tbThreshold_Scroll);
            // 
            // cmbThresholding
            // 
            this.cmbThresholding.FormattingEnabled = true;
            this.cmbThresholding.Items.AddRange(new object[] {
            "MaxEntropy法",
            "Otsu法",
            "SIS法",
            "Huang法",
            "Yen法",
            "Li法",
            "Mean法",
            "Moment法",
            "Iterative法",
            "IsoData法",
            "Percentile法",
            "閾値設定"});
            this.cmbThresholding.Location = new System.Drawing.Point(10, 36);
            this.cmbThresholding.Name = "cmbThresholding";
            this.cmbThresholding.Size = new System.Drawing.Size(116, 20);
            this.cmbThresholding.TabIndex = 1;
            this.cmbThresholding.Text = "MaxEntropy法";
            this.cmbThresholding.SelectedIndexChanged += new System.EventHandler(this.cmbThresholding_SelectedIndexChanged);
            // 
            // rdbYCbCr
            // 
            this.rdbYCbCr.AutoSize = true;
            this.rdbYCbCr.Location = new System.Drawing.Point(5, 165);
            this.rdbYCbCr.Name = "rdbYCbCr";
            this.rdbYCbCr.Size = new System.Drawing.Size(112, 18);
            this.rdbYCbCr.TabIndex = 2;
            this.rdbYCbCr.Text = "YCbCrフィルター";
            this.rdbYCbCr.UseVisualStyleBackColor = true;
            this.rdbYCbCr.CheckedChanged += new System.EventHandler(this.rdbYCbCr_CheckedChanged);
            // 
            // rdbHSL
            // 
            this.rdbHSL.AutoSize = true;
            this.rdbHSL.Checked = true;
            this.rdbHSL.Location = new System.Drawing.Point(5, 20);
            this.rdbHSL.Name = "rdbHSL";
            this.rdbHSL.Size = new System.Drawing.Size(99, 18);
            this.rdbHSL.TabIndex = 0;
            this.rdbHSL.TabStop = true;
            this.rdbHSL.Text = "HSLフィルター";
            this.rdbHSL.UseVisualStyleBackColor = true;
            this.rdbHSL.CheckedChanged += new System.EventHandler(this.rdbHSL_CheckedChanged);
            // 
            // grbYCbCr
            // 
            this.grbYCbCr.Controls.Add(this.lblCrRange);
            this.grbYCbCr.Controls.Add(this.lblCr);
            this.grbYCbCr.Controls.Add(this.lblCbRange);
            this.grbYCbCr.Controls.Add(this.slideCr);
            this.grbYCbCr.Controls.Add(this.lblYRange);
            this.grbYCbCr.Controls.Add(this.lblCb);
            this.grbYCbCr.Controls.Add(this.slideCb);
            this.grbYCbCr.Controls.Add(this.lblY);
            this.grbYCbCr.Controls.Add(this.slideY);
            this.grbYCbCr.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.grbYCbCr.ForeColor = System.Drawing.Color.Black;
            this.grbYCbCr.Location = new System.Drawing.Point(5, 182);
            this.grbYCbCr.Name = "grbYCbCr";
            this.grbYCbCr.Size = new System.Drawing.Size(372, 120);
            this.grbYCbCr.TabIndex = 3;
            this.grbYCbCr.TabStop = false;
            // 
            // lblCrRange
            // 
            this.lblCrRange.AutoSize = true;
            this.lblCrRange.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblCrRange.Location = new System.Drawing.Point(295, 85);
            this.lblCrRange.Name = "lblCrRange";
            this.lblCrRange.Size = new System.Drawing.Size(59, 12);
            this.lblCrRange.TabIndex = 8;
            this.lblCrRange.Text = "(-0.5～0.5)";
            // 
            // lblCr
            // 
            this.lblCr.AutoSize = true;
            this.lblCr.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblCr.Location = new System.Drawing.Point(9, 88);
            this.lblCr.Name = "lblCr";
            this.lblCr.Size = new System.Drawing.Size(17, 12);
            this.lblCr.TabIndex = 6;
            this.lblCr.Text = "Cr";
            // 
            // lblCbRange
            // 
            this.lblCbRange.AutoSize = true;
            this.lblCbRange.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblCbRange.Location = new System.Drawing.Point(295, 56);
            this.lblCbRange.Name = "lblCbRange";
            this.lblCbRange.Size = new System.Drawing.Size(59, 12);
            this.lblCbRange.TabIndex = 5;
            this.lblCbRange.Text = "(-0.5～0.5)";
            // 
            // slideCr
            // 
            this.slideCr.EndColor = System.Drawing.Color.Black;
            this.slideCr.FillColor = System.Drawing.Color.LightSkyBlue;
            this.slideCr.Location = new System.Drawing.Point(22, 84);
            this.slideCr.Max = 100;
            this.slideCr.Name = "slideCr";
            this.slideCr.Size = new System.Drawing.Size(277, 23);
            this.slideCr.StartColor = System.Drawing.Color.White;
            this.slideCr.TabIndex = 7;
            this.slideCr.Type = MokkAnnotator.MkaWindow.MkaColorSlider.ColorSliderType.OuterGradient;
            this.slideCr.ValuesChanged += new System.EventHandler(this.slideCr_ValuesChanged);
            // 
            // lblYRange
            // 
            this.lblYRange.AutoSize = true;
            this.lblYRange.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblYRange.Location = new System.Drawing.Point(295, 26);
            this.lblYRange.Name = "lblYRange";
            this.lblYRange.Size = new System.Drawing.Size(37, 12);
            this.lblYRange.TabIndex = 2;
            this.lblYRange.Text = "(0～1)";
            // 
            // lblCb
            // 
            this.lblCb.AutoSize = true;
            this.lblCb.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblCb.Location = new System.Drawing.Point(9, 59);
            this.lblCb.Name = "lblCb";
            this.lblCb.Size = new System.Drawing.Size(19, 12);
            this.lblCb.TabIndex = 3;
            this.lblCb.Text = "Cb";
            // 
            // slideCb
            // 
            this.slideCb.EndColor = System.Drawing.Color.Black;
            this.slideCb.FillColor = System.Drawing.Color.LightSkyBlue;
            this.slideCb.Location = new System.Drawing.Point(22, 55);
            this.slideCb.Max = 100;
            this.slideCb.Name = "slideCb";
            this.slideCb.Size = new System.Drawing.Size(277, 23);
            this.slideCb.StartColor = System.Drawing.Color.White;
            this.slideCb.TabIndex = 4;
            this.slideCb.Type = MokkAnnotator.MkaWindow.MkaColorSlider.ColorSliderType.OuterGradient;
            this.slideCb.ValuesChanged += new System.EventHandler(this.slideCb_ValuesChanged);
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblY.Location = new System.Drawing.Point(9, 30);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(12, 12);
            this.lblY.TabIndex = 0;
            this.lblY.Text = "Y";
            // 
            // slideY
            // 
            this.slideY.EndColor = System.Drawing.Color.Black;
            this.slideY.FillColor = System.Drawing.Color.LightSkyBlue;
            this.slideY.Location = new System.Drawing.Point(22, 26);
            this.slideY.Max = 100;
            this.slideY.Name = "slideY";
            this.slideY.Size = new System.Drawing.Size(277, 23);
            this.slideY.StartColor = System.Drawing.Color.White;
            this.slideY.TabIndex = 1;
            this.slideY.Type = MokkAnnotator.MkaWindow.MkaColorSlider.ColorSliderType.OuterGradient;
            this.slideY.ValuesChanged += new System.EventHandler(this.slideY_ValuesChanged);
            // 
            // grbHSL
            // 
            this.grbHSL.Controls.Add(this.lblLRange);
            this.grbHSL.Controls.Add(this.lblSRange);
            this.grbHSL.Controls.Add(this.lblHRange);
            this.grbHSL.Controls.Add(this.lblL);
            this.grbHSL.Controls.Add(this.slideL);
            this.grbHSL.Controls.Add(this.lblS);
            this.grbHSL.Controls.Add(this.slideS);
            this.grbHSL.Controls.Add(this.lblH);
            this.grbHSL.Controls.Add(this.slideH);
            this.grbHSL.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.grbHSL.ForeColor = System.Drawing.Color.Black;
            this.grbHSL.Location = new System.Drawing.Point(5, 37);
            this.grbHSL.Name = "grbHSL";
            this.grbHSL.Size = new System.Drawing.Size(373, 120);
            this.grbHSL.TabIndex = 1;
            this.grbHSL.TabStop = false;
            // 
            // lblLRange
            // 
            this.lblLRange.AutoSize = true;
            this.lblLRange.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblLRange.Location = new System.Drawing.Point(295, 84);
            this.lblLRange.Name = "lblLRange";
            this.lblLRange.Size = new System.Drawing.Size(37, 12);
            this.lblLRange.TabIndex = 8;
            this.lblLRange.Text = "(0～1)";
            // 
            // lblSRange
            // 
            this.lblSRange.AutoSize = true;
            this.lblSRange.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblSRange.Location = new System.Drawing.Point(295, 55);
            this.lblSRange.Name = "lblSRange";
            this.lblSRange.Size = new System.Drawing.Size(37, 12);
            this.lblSRange.TabIndex = 5;
            this.lblSRange.Text = "(0～1)";
            // 
            // lblHRange
            // 
            this.lblHRange.AutoSize = true;
            this.lblHRange.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblHRange.Location = new System.Drawing.Point(295, 25);
            this.lblHRange.Name = "lblHRange";
            this.lblHRange.Size = new System.Drawing.Size(49, 12);
            this.lblHRange.TabIndex = 2;
            this.lblHRange.Text = "(0～359)";
            // 
            // lblL
            // 
            this.lblL.AutoSize = true;
            this.lblL.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblL.Location = new System.Drawing.Point(9, 88);
            this.lblL.Name = "lblL";
            this.lblL.Size = new System.Drawing.Size(11, 12);
            this.lblL.TabIndex = 6;
            this.lblL.Text = "L";
            // 
            // slideL
            // 
            this.slideL.EndColor = System.Drawing.Color.Black;
            this.slideL.FillColor = System.Drawing.Color.LightSkyBlue;
            this.slideL.Location = new System.Drawing.Point(22, 84);
            this.slideL.Max = 100;
            this.slideL.Name = "slideL";
            this.slideL.Size = new System.Drawing.Size(277, 23);
            this.slideL.StartColor = System.Drawing.Color.White;
            this.slideL.TabIndex = 7;
            this.slideL.Type = MokkAnnotator.MkaWindow.MkaColorSlider.ColorSliderType.OuterGradient;
            this.slideL.ValuesChanged += new System.EventHandler(this.slideL_ValuesChanged);
            // 
            // lblS
            // 
            this.lblS.AutoSize = true;
            this.lblS.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblS.Location = new System.Drawing.Point(9, 59);
            this.lblS.Name = "lblS";
            this.lblS.Size = new System.Drawing.Size(12, 12);
            this.lblS.TabIndex = 3;
            this.lblS.Text = "S";
            // 
            // slideS
            // 
            this.slideS.EndColor = System.Drawing.Color.Black;
            this.slideS.FillColor = System.Drawing.Color.LightSkyBlue;
            this.slideS.Location = new System.Drawing.Point(22, 55);
            this.slideS.Max = 100;
            this.slideS.Name = "slideS";
            this.slideS.Size = new System.Drawing.Size(277, 23);
            this.slideS.StartColor = System.Drawing.Color.White;
            this.slideS.TabIndex = 4;
            this.slideS.Type = MokkAnnotator.MkaWindow.MkaColorSlider.ColorSliderType.OuterGradient;
            this.slideS.ValuesChanged += new System.EventHandler(this.slideS_ValuesChanged);
            // 
            // lblH
            // 
            this.lblH.AutoSize = true;
            this.lblH.Font = new System.Drawing.Font("MS UI Gothic", 9F);
            this.lblH.Location = new System.Drawing.Point(9, 30);
            this.lblH.Name = "lblH";
            this.lblH.Size = new System.Drawing.Size(13, 12);
            this.lblH.TabIndex = 0;
            this.lblH.Text = "H";
            // 
            // slideH
            // 
            this.slideH.EndColor = System.Drawing.Color.Black;
            this.slideH.FillColor = System.Drawing.Color.LightSkyBlue;
            this.slideH.Location = new System.Drawing.Point(22, 26);
            this.slideH.Max = 180;
            this.slideH.Name = "slideH";
            this.slideH.Size = new System.Drawing.Size(277, 23);
            this.slideH.StartColor = System.Drawing.Color.White;
            this.slideH.TabIndex = 1;
            this.slideH.Type = MokkAnnotator.MkaWindow.MkaColorSlider.ColorSliderType.OuterGradient;
            this.slideH.ValuesChanged += new System.EventHandler(this.slideH_ValuesChanged);
            // 
            // nupLeft
            // 
            this.nupLeft.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nupLeft.ForeColor = System.Drawing.Color.Black;
            this.nupLeft.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nupLeft.Location = new System.Drawing.Point(62, 43);
            this.nupLeft.Name = "nupLeft";
            this.nupLeft.Size = new System.Drawing.Size(70, 28);
            this.nupLeft.TabIndex = 1;
            this.nupLeft.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nupLeft.ValueChanged += new System.EventHandler(this.boundSize_ValueChanged);
            // 
            // grbMokkanArea
            // 
            this.grbMokkanArea.Controls.Add(this.lblBottom);
            this.grbMokkanArea.Controls.Add(this.nupBottom);
            this.grbMokkanArea.Controls.Add(this.lblTop);
            this.grbMokkanArea.Controls.Add(this.nupTop);
            this.grbMokkanArea.Controls.Add(this.lblRight);
            this.grbMokkanArea.Controls.Add(this.nupRight);
            this.grbMokkanArea.Controls.Add(this.lblLeft);
            this.grbMokkanArea.Controls.Add(this.nupLeft);
            this.grbMokkanArea.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.grbMokkanArea.ForeColor = System.Drawing.Color.Blue;
            this.grbMokkanArea.Location = new System.Drawing.Point(1, 9);
            this.grbMokkanArea.Name = "grbMokkanArea";
            this.grbMokkanArea.Size = new System.Drawing.Size(383, 89);
            this.grbMokkanArea.TabIndex = 2;
            this.grbMokkanArea.TabStop = false;
            this.grbMokkanArea.Text = "木簡配置領域設定";
            // 
            // lblBottom
            // 
            this.lblBottom.AutoSize = true;
            this.lblBottom.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblBottom.ForeColor = System.Drawing.Color.Black;
            this.lblBottom.Location = new System.Drawing.Point(147, 64);
            this.lblBottom.Name = "lblBottom";
            this.lblBottom.Size = new System.Drawing.Size(17, 12);
            this.lblBottom.TabIndex = 6;
            this.lblBottom.Text = "下";
            // 
            // nupBottom
            // 
            this.nupBottom.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nupBottom.ForeColor = System.Drawing.Color.Black;
            this.nupBottom.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nupBottom.Location = new System.Drawing.Point(178, 59);
            this.nupBottom.Name = "nupBottom";
            this.nupBottom.Size = new System.Drawing.Size(70, 28);
            this.nupBottom.TabIndex = 7;
            this.nupBottom.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nupBottom.ValueChanged += new System.EventHandler(this.boundSize_ValueChanged);
            // 
            // lblTop
            // 
            this.lblTop.AutoSize = true;
            this.lblTop.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTop.ForeColor = System.Drawing.Color.Black;
            this.lblTop.Location = new System.Drawing.Point(147, 26);
            this.lblTop.Name = "lblTop";
            this.lblTop.Size = new System.Drawing.Size(17, 12);
            this.lblTop.TabIndex = 4;
            this.lblTop.Text = "上";
            // 
            // nupTop
            // 
            this.nupTop.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nupTop.ForeColor = System.Drawing.Color.Black;
            this.nupTop.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nupTop.Location = new System.Drawing.Point(178, 23);
            this.nupTop.Name = "nupTop";
            this.nupTop.Size = new System.Drawing.Size(70, 28);
            this.nupTop.TabIndex = 5;
            this.nupTop.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nupTop.ValueChanged += new System.EventHandler(this.boundSize_ValueChanged);
            // 
            // lblRight
            // 
            this.lblRight.AutoSize = true;
            this.lblRight.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblRight.ForeColor = System.Drawing.Color.Black;
            this.lblRight.Location = new System.Drawing.Point(265, 46);
            this.lblRight.Name = "lblRight";
            this.lblRight.Size = new System.Drawing.Size(17, 12);
            this.lblRight.TabIndex = 2;
            this.lblRight.Text = "右";
            // 
            // nupRight
            // 
            this.nupRight.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nupRight.ForeColor = System.Drawing.Color.Black;
            this.nupRight.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nupRight.Location = new System.Drawing.Point(296, 43);
            this.nupRight.Name = "nupRight";
            this.nupRight.Size = new System.Drawing.Size(63, 28);
            this.nupRight.TabIndex = 3;
            this.nupRight.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nupRight.ValueChanged += new System.EventHandler(this.boundSize_ValueChanged);
            // 
            // lblLeft
            // 
            this.lblLeft.AutoSize = true;
            this.lblLeft.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblLeft.ForeColor = System.Drawing.Color.Black;
            this.lblLeft.Location = new System.Drawing.Point(31, 46);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(17, 12);
            this.lblLeft.TabIndex = 0;
            this.lblLeft.Text = "左";
            // 
            // nupMokkanNum
            // 
            this.nupMokkanNum.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nupMokkanNum.Location = new System.Drawing.Point(598, 460);
            this.nupMokkanNum.Name = "nupMokkanNum";
            this.nupMokkanNum.Size = new System.Drawing.Size(56, 28);
            this.nupMokkanNum.TabIndex = 5;
            this.nupMokkanNum.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nupMokkanNum.ValueChanged += new System.EventHandler(this.parameters_ValueChanged);
            // 
            // lblMokkanNum
            // 
            this.lblMokkanNum.AutoSize = true;
            this.lblMokkanNum.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblMokkanNum.Location = new System.Drawing.Point(476, 461);
            this.lblMokkanNum.Name = "lblMokkanNum";
            this.lblMokkanNum.Size = new System.Drawing.Size(49, 14);
            this.lblMokkanNum.TabIndex = 4;
            this.lblMokkanNum.Text = "木簡数";
            // 
            // lblMokkanMinArea
            // 
            this.lblMokkanMinArea.AutoSize = true;
            this.lblMokkanMinArea.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblMokkanMinArea.Location = new System.Drawing.Point(476, 496);
            this.lblMokkanMinArea.Name = "lblMokkanMinArea";
            this.lblMokkanMinArea.Size = new System.Drawing.Size(102, 14);
            this.lblMokkanMinArea.TabIndex = 8;
            this.lblMokkanMinArea.Text = "木簡の最少面積";
            // 
            // nupMokkanMinArea
            // 
            this.nupMokkanMinArea.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nupMokkanMinArea.Location = new System.Drawing.Point(598, 493);
            this.nupMokkanMinArea.Name = "nupMokkanMinArea";
            this.nupMokkanMinArea.Size = new System.Drawing.Size(56, 28);
            this.nupMokkanMinArea.TabIndex = 9;
            this.nupMokkanMinArea.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nupMokkanMinArea.ValueChanged += new System.EventHandler(this.parameters_ValueChanged);
            // 
            // lblWidHeiRatio
            // 
            this.lblWidHeiRatio.AutoSize = true;
            this.lblWidHeiRatio.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblWidHeiRatio.Location = new System.Drawing.Point(681, 461);
            this.lblWidHeiRatio.Name = "lblWidHeiRatio";
            this.lblWidHeiRatio.Size = new System.Drawing.Size(129, 14);
            this.lblWidHeiRatio.TabIndex = 6;
            this.lblWidHeiRatio.Text = "長さ・広さの最大比率";
            // 
            // nupWidHeiRatio
            // 
            this.nupWidHeiRatio.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nupWidHeiRatio.Location = new System.Drawing.Point(820, 458);
            this.nupWidHeiRatio.Name = "nupWidHeiRatio";
            this.nupWidHeiRatio.Size = new System.Drawing.Size(56, 28);
            this.nupWidHeiRatio.TabIndex = 7;
            this.nupWidHeiRatio.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nupWidHeiRatio.ValueChanged += new System.EventHandler(this.parameters_ValueChanged);
            // 
            // lblTolerance
            // 
            this.lblTolerance.AutoSize = true;
            this.lblTolerance.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblTolerance.Location = new System.Drawing.Point(681, 496);
            this.lblTolerance.Name = "lblTolerance";
            this.lblTolerance.Size = new System.Drawing.Size(116, 14);
            this.lblTolerance.TabIndex = 10;
            this.lblTolerance.Text = "境界点削減の公差";
            // 
            // nupTolerance
            // 
            this.nupTolerance.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.nupTolerance.Location = new System.Drawing.Point(820, 491);
            this.nupTolerance.Name = "nupTolerance";
            this.nupTolerance.Size = new System.Drawing.Size(56, 28);
            this.nupTolerance.TabIndex = 11;
            this.nupTolerance.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nupTolerance.ValueChanged += new System.EventHandler(this.parameters_ValueChanged);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(150, 542);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(80, 23);
            this.btnReset.TabIndex = 12;
            this.btnReset.Text = "初期値(&D)";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(587, 542);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(78, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(715, 542);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "キャンセル(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // MkaAutoExtractMokkan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(997, 582);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblTolerance);
            this.Controls.Add(this.nupTolerance);
            this.Controls.Add(this.lblWidHeiRatio);
            this.Controls.Add(this.nupWidHeiRatio);
            this.Controls.Add(this.lblMokkanMinArea);
            this.Controls.Add(this.nupMokkanMinArea);
            this.Controls.Add(this.lblMokkanNum);
            this.Controls.Add(this.nupMokkanNum);
            this.Controls.Add(this.grbBoundaryTracking);
            this.Controls.Add(this.grbMokkanArea);
            this.Controls.Add(this.pnImageView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MkaAutoExtractMokkan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "木簡境界自動指定";
            this.Load += new System.EventHandler(this.MkaAutoExtractMokkan_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MkaAutoExtractMokkan_FormClosing);
            this.pnImageView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picImageView)).EndInit();
            this.grbBoundaryTracking.ResumeLayout(false);
            this.grbBoundaryTracking.PerformLayout();
            this.grbThresholding.ResumeLayout(false);
            this.grbThresholding.PerformLayout();
            this.grbThreshold.ResumeLayout(false);
            this.grbThreshold.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbThreshold)).EndInit();
            this.grbYCbCr.ResumeLayout(false);
            this.grbYCbCr.PerformLayout();
            this.grbHSL.ResumeLayout(false);
            this.grbHSL.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupLeft)).EndInit();
            this.grbMokkanArea.ResumeLayout(false);
            this.grbMokkanArea.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nupBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupMokkanNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupMokkanMinArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupWidHeiRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nupTolerance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnImageView;
        private System.Windows.Forms.GroupBox grbBoundaryTracking;
        private System.Windows.Forms.GroupBox grbHSL;
        private System.Windows.Forms.NumericUpDown nupLeft;
        private System.Windows.Forms.GroupBox grbMokkanArea;
        private System.Windows.Forms.Label lblBottom;
        private System.Windows.Forms.NumericUpDown nupBottom;
        private System.Windows.Forms.Label lblTop;
        private System.Windows.Forms.NumericUpDown nupTop;
        private System.Windows.Forms.Label lblRight;
        private System.Windows.Forms.NumericUpDown nupRight;
        private System.Windows.Forms.Label lblLeft;
        private System.Windows.Forms.Label lblL;
        private MkaColorSlider slideL;
        private System.Windows.Forms.Label lblS;
        private MkaColorSlider slideS;
        private System.Windows.Forms.Label lblH;
        private MkaColorSlider slideH;
        private System.Windows.Forms.RadioButton rdbYCbCr;
        private System.Windows.Forms.RadioButton rdbHSL;
        private System.Windows.Forms.GroupBox grbYCbCr;
        private System.Windows.Forms.RadioButton rdbThresholding;
        private System.Windows.Forms.GroupBox grbThresholding;
        private System.Windows.Forms.NumericUpDown nupMokkanNum;
        private System.Windows.Forms.Label lblMokkanNum;
        private System.Windows.Forms.Label lblMokkanMinArea;
        private System.Windows.Forms.NumericUpDown nupMokkanMinArea;
        private System.Windows.Forms.Label lblWidHeiRatio;
        private System.Windows.Forms.NumericUpDown nupWidHeiRatio;
        private System.Windows.Forms.Label lblTolerance;
        private System.Windows.Forms.NumericUpDown nupTolerance;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblLRange;
        private System.Windows.Forms.Label lblSRange;
        private System.Windows.Forms.Label lblHRange;
        private System.Windows.Forms.Label lblCrRange;
        private System.Windows.Forms.Label lblCr;
        private System.Windows.Forms.Label lblCbRange;
        private MkaColorSlider slideCr;
        private System.Windows.Forms.Label lblYRange;
        private System.Windows.Forms.Label lblCb;
        private MkaColorSlider slideCb;
        private System.Windows.Forms.Label lblY;
        private MkaColorSlider slideY;
        private System.Windows.Forms.NumericUpDown nupThreshold;
        private System.Windows.Forms.Label lbl255;
        private System.Windows.Forms.Label lbl0;
        private System.Windows.Forms.TrackBar tbThreshold;
        private System.Windows.Forms.ComboBox cmbThresholding;
        private System.Windows.Forms.GroupBox grbThreshold;
        private System.Windows.Forms.PictureBox picImageView;
        private System.Windows.Forms.Label lblThresholding;
    }
}