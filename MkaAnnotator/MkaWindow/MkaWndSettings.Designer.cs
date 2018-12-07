namespace MokkAnnotator.MkaWindow
{
    partial class MkaWndSettings
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
            MokkAnnotator.MkaDrawTools.GraphicsProperties graphicsProperties1 = new MokkAnnotator.MkaDrawTools.GraphicsProperties();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MkaWndSettings));
            this.btnOK = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.tbpGraphics = new System.Windows.Forms.TabPage();
            this.cmbFontSize = new System.Windows.Forms.ComboBox();
            this.lblRegionFormat = new System.Windows.Forms.Label();
            this.lblFontSize = new System.Windows.Forms.Label();
            this.mkaGraphicsProperties = new MokkAnnotator.MkaWindow.MkaGraphicsProperties();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.tbpDatabase = new System.Windows.Forms.TabPage();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbSettings = new System.Windows.Forms.TabControl();
            this.tbpDefaultValues = new System.Windows.Forms.TabPage();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.txtBatBangou = new System.Windows.Forms.TextBox();
            this.lblDescription2 = new System.Windows.Forms.Label();
            this.lblBatBangou = new System.Windows.Forms.Label();
            this.lblDescription1 = new System.Windows.Forms.Label();
            this.lblDescription4 = new System.Windows.Forms.Label();
            this.txtGrid = new MokkAnnotator.MkaWindow.NumbersOnlyTextBox();
            this.txtKaishiRBangou = new MokkAnnotator.MkaWindow.NumbersOnlyTextBox();
            this.txtChousaJisuu = new MokkAnnotator.MkaWindow.NumbersOnlyTextBox();
            this.lblKaishiRBangou = new System.Windows.Forms.Label();
            this.txtDosoumei = new System.Windows.Forms.TextBox();
            this.txtIkoumei = new System.Windows.Forms.TextBox();
            this.txtChuushouChiku = new System.Windows.Forms.TextBox();
            this.txtOoChiku = new System.Windows.Forms.TextBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblGrid = new System.Windows.Forms.Label();
            this.lblDosoumei = new System.Windows.Forms.Label();
            this.lblIkoumei = new System.Windows.Forms.Label();
            this.lblChuusouChiku = new System.Windows.Forms.Label();
            this.lblOoChiku = new System.Windows.Forms.Label();
            this.lblChousaJisuu = new System.Windows.Forms.Label();
            this.tbpGraphics.SuspendLayout();
            this.tbpDatabase.SuspendLayout();
            this.tbSettings.SuspendLayout();
            this.tbpDefaultValues.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(241, 369);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(78, 25);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.txtPassword.Location = new System.Drawing.Point(182, 173);
            this.txtPassword.MaxLength = 1000;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(168, 23);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.txtUsername.Location = new System.Drawing.Point(182, 131);
            this.txtUsername.MaxLength = 1000;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(168, 23);
            this.txtUsername.TabIndex = 5;
            // 
            // tbpGraphics
            // 
            this.tbpGraphics.Controls.Add(this.cmbFontSize);
            this.tbpGraphics.Controls.Add(this.lblRegionFormat);
            this.tbpGraphics.Controls.Add(this.lblFontSize);
            this.tbpGraphics.Controls.Add(this.mkaGraphicsProperties);
            this.tbpGraphics.Location = new System.Drawing.Point(4, 25);
            this.tbpGraphics.Name = "tbpGraphics";
            this.tbpGraphics.Padding = new System.Windows.Forms.Padding(3);
            this.tbpGraphics.Size = new System.Drawing.Size(418, 329);
            this.tbpGraphics.TabIndex = 1;
            this.tbpGraphics.Text = "書式";
            this.tbpGraphics.UseVisualStyleBackColor = true;
            // 
            // cmbFontSize
            // 
            this.cmbFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFontSize.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbFontSize.FormattingEnabled = true;
            this.cmbFontSize.Items.AddRange(new object[] {
            "12",
            "14",
            "16",
            "18",
            "20",
            "24",
            "28",
            "32",
            "36",
            "40",
            "44",
            "48",
            "54",
            "60",
            "66",
            "72"});
            this.cmbFontSize.Location = new System.Drawing.Point(178, 23);
            this.cmbFontSize.Name = "cmbFontSize";
            this.cmbFontSize.Size = new System.Drawing.Size(82, 24);
            this.cmbFontSize.TabIndex = 1;
            // 
            // lblRegionFormat
            // 
            this.lblRegionFormat.AutoSize = true;
            this.lblRegionFormat.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblRegionFormat.Location = new System.Drawing.Point(18, 58);
            this.lblRegionFormat.Name = "lblRegionFormat";
            this.lblRegionFormat.Size = new System.Drawing.Size(145, 16);
            this.lblRegionFormat.TabIndex = 2;
            this.lblRegionFormat.Text = "木簡境界の形式(&R)：";
            // 
            // lblFontSize
            // 
            this.lblFontSize.AutoSize = true;
            this.lblFontSize.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblFontSize.Location = new System.Drawing.Point(18, 26);
            this.lblFontSize.Name = "lblFontSize";
            this.lblFontSize.Size = new System.Drawing.Size(154, 16);
            this.lblFontSize.TabIndex = 0;
            this.lblFontSize.Text = "R番号表示フォント(&F)：";
            // 
            // mkaGraphicsProperties
            // 
            this.mkaGraphicsProperties.AutoSize = true;
            this.mkaGraphicsProperties.ChangeLast = false;
            this.mkaGraphicsProperties.Location = new System.Drawing.Point(85, 83);
            this.mkaGraphicsProperties.Name = "mkaGraphicsProperties";
            graphicsProperties1.BorderColor = System.Drawing.Color.DodgerBlue;
            graphicsProperties1.BorderColorHtml = "DodgerBlue";
            graphicsProperties1.FillColor = System.Drawing.Color.DeepSkyBlue;
            graphicsProperties1.FillColorAlpha = 50;
            graphicsProperties1.FillColorHtml = "DeepSkyBlue";
            graphicsProperties1.PenWidth = 1F;
            graphicsProperties1.RFontSize = 24F;
            graphicsProperties1.RShowBottom = ((System.Drawing.PointF)(resources.GetObject("graphicsProperties1.RShowBottom")));
            graphicsProperties1.RShowPosition = MokkAnnotator.MkaCommon.ShowPosition.Top;
            graphicsProperties1.RShowTop = ((System.Drawing.PointF)(resources.GetObject("graphicsProperties1.RShowTop")));
            this.mkaGraphicsProperties.Properties = graphicsProperties1;
            this.mkaGraphicsProperties.Size = new System.Drawing.Size(277, 237);
            this.mkaGraphicsProperties.TabIndex = 3;
            // 
            // txtDatabase
            // 
            this.txtDatabase.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.txtDatabase.Location = new System.Drawing.Point(182, 84);
            this.txtDatabase.MaxLength = 1000;
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(168, 23);
            this.txtDatabase.TabIndex = 3;
            // 
            // tbpDatabase
            // 
            this.tbpDatabase.Controls.Add(this.txtServer);
            this.tbpDatabase.Controls.Add(this.txtPassword);
            this.tbpDatabase.Controls.Add(this.txtUsername);
            this.tbpDatabase.Controls.Add(this.txtDatabase);
            this.tbpDatabase.Controls.Add(this.lblPassword);
            this.tbpDatabase.Controls.Add(this.lblUsername);
            this.tbpDatabase.Controls.Add(this.lblDatabase);
            this.tbpDatabase.Controls.Add(this.lblServer);
            this.tbpDatabase.Location = new System.Drawing.Point(4, 25);
            this.tbpDatabase.Name = "tbpDatabase";
            this.tbpDatabase.Padding = new System.Windows.Forms.Padding(3);
            this.tbpDatabase.Size = new System.Drawing.Size(418, 329);
            this.tbpDatabase.TabIndex = 0;
            this.tbpDatabase.Text = "データベース";
            this.tbpDatabase.UseVisualStyleBackColor = true;
            // 
            // txtServer
            // 
            this.txtServer.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.txtServer.Location = new System.Drawing.Point(182, 38);
            this.txtServer.MaxLength = 1000;
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(168, 23);
            this.txtServer.TabIndex = 1;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.lblPassword.Location = new System.Drawing.Point(48, 176);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(111, 16);
            this.lblPassword.TabIndex = 6;
            this.lblPassword.Text = "パースワード(&P)：";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.lblUsername.Location = new System.Drawing.Point(48, 134);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(91, 16);
            this.lblUsername.TabIndex = 4;
            this.lblUsername.Text = "ユーザ名(&U)：";
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.lblDatabase.Location = new System.Drawing.Point(48, 87);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(127, 16);
            this.lblDatabase.TabIndex = 2;
            this.lblDatabase.Text = "データベース名(&D)：";
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.lblServer.Location = new System.Drawing.Point(48, 41);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(105, 16);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "サーバー名(&S)：";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(337, 369);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "キャンセル(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tbSettings
            // 
            this.tbSettings.Controls.Add(this.tbpDefaultValues);
            this.tbSettings.Controls.Add(this.tbpGraphics);
            this.tbSettings.Controls.Add(this.tbpDatabase);
            this.tbSettings.Font = new System.Drawing.Font("MS UI Gothic", 11F);
            this.tbSettings.Location = new System.Drawing.Point(1, 5);
            this.tbSettings.Name = "tbSettings";
            this.tbSettings.SelectedIndex = 0;
            this.tbSettings.Size = new System.Drawing.Size(426, 358);
            this.tbSettings.TabIndex = 0;
            // 
            // tbpDefaultValues
            // 
            this.tbpDefaultValues.Controls.Add(this.dtpDate);
            this.tbpDefaultValues.Controls.Add(this.txtBatBangou);
            this.tbpDefaultValues.Controls.Add(this.lblDescription2);
            this.tbpDefaultValues.Controls.Add(this.lblBatBangou);
            this.tbpDefaultValues.Controls.Add(this.lblDescription1);
            this.tbpDefaultValues.Controls.Add(this.lblDescription4);
            this.tbpDefaultValues.Controls.Add(this.txtGrid);
            this.tbpDefaultValues.Controls.Add(this.txtKaishiRBangou);
            this.tbpDefaultValues.Controls.Add(this.txtChousaJisuu);
            this.tbpDefaultValues.Controls.Add(this.lblKaishiRBangou);
            this.tbpDefaultValues.Controls.Add(this.txtDosoumei);
            this.tbpDefaultValues.Controls.Add(this.txtIkoumei);
            this.tbpDefaultValues.Controls.Add(this.txtChuushouChiku);
            this.tbpDefaultValues.Controls.Add(this.txtOoChiku);
            this.tbpDefaultValues.Controls.Add(this.lblDate);
            this.tbpDefaultValues.Controls.Add(this.lblGrid);
            this.tbpDefaultValues.Controls.Add(this.lblDosoumei);
            this.tbpDefaultValues.Controls.Add(this.lblIkoumei);
            this.tbpDefaultValues.Controls.Add(this.lblChuusouChiku);
            this.tbpDefaultValues.Controls.Add(this.lblOoChiku);
            this.tbpDefaultValues.Controls.Add(this.lblChousaJisuu);
            this.tbpDefaultValues.Location = new System.Drawing.Point(4, 25);
            this.tbpDefaultValues.Name = "tbpDefaultValues";
            this.tbpDefaultValues.Padding = new System.Windows.Forms.Padding(3);
            this.tbpDefaultValues.Size = new System.Drawing.Size(418, 329);
            this.tbpDefaultValues.TabIndex = 2;
            this.tbpDefaultValues.Text = "初期値";
            this.tbpDefaultValues.UseVisualStyleBackColor = true;
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "";
            this.dtpDate.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dtpDate.Location = new System.Drawing.Point(232, 185);
            this.dtpDate.MaxDate = new System.DateTime(2012, 4, 24, 0, 0, 0, 0);
            this.dtpDate.MinDate = new System.DateTime(1990, 1, 1, 0, 0, 0, 0);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(133, 23);
            this.dtpDate.TabIndex = 31;
            this.dtpDate.Value = new System.DateTime(1990, 1, 1, 0, 0, 0, 0);
            // 
            // txtBatBangou
            // 
            this.txtBatBangou.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBatBangou.Location = new System.Drawing.Point(232, 214);
            this.txtBatBangou.MaxLength = 7;
            this.txtBatBangou.Name = "txtBatBangou";
            this.txtBatBangou.Size = new System.Drawing.Size(133, 23);
            this.txtBatBangou.TabIndex = 31;
            // 
            // lblDescription2
            // 
            this.lblDescription2.AutoSize = true;
            this.lblDescription2.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDescription2.ForeColor = System.Drawing.Color.Blue;
            this.lblDescription2.Location = new System.Drawing.Point(324, 162);
            this.lblDescription2.Name = "lblDescription2";
            this.lblDescription2.Size = new System.Drawing.Size(41, 13);
            this.lblDescription2.TabIndex = 29;
            this.lblDescription2.Text = "(数字)";
            // 
            // lblBatBangou
            // 
            this.lblBatBangou.AutoSize = true;
            this.lblBatBangou.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblBatBangou.Location = new System.Drawing.Point(27, 217);
            this.lblBatBangou.Name = "lblBatBangou";
            this.lblBatBangou.Size = new System.Drawing.Size(102, 16);
            this.lblBatBangou.TabIndex = 30;
            this.lblBatBangou.Text = "バット番号(&B)：";
            // 
            // lblDescription1
            // 
            this.lblDescription1.AutoSize = true;
            this.lblDescription1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDescription1.ForeColor = System.Drawing.Color.Blue;
            this.lblDescription1.Location = new System.Drawing.Point(324, 27);
            this.lblDescription1.Name = "lblDescription1";
            this.lblDescription1.Size = new System.Drawing.Size(41, 13);
            this.lblDescription1.TabIndex = 18;
            this.lblDescription1.Text = "(数字)";
            // 
            // lblDescription4
            // 
            this.lblDescription4.AutoSize = true;
            this.lblDescription4.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDescription4.ForeColor = System.Drawing.Color.Blue;
            this.lblDescription4.Location = new System.Drawing.Point(324, 249);
            this.lblDescription4.Name = "lblDescription4";
            this.lblDescription4.Size = new System.Drawing.Size(41, 13);
            this.lblDescription4.TabIndex = 35;
            this.lblDescription4.Text = "(数字)";
            // 
            // txtGrid
            // 
            this.txtGrid.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtGrid.Location = new System.Drawing.Point(232, 156);
            this.txtGrid.MaxLength = 7;
            this.txtGrid.Name = "txtGrid";
            this.txtGrid.Size = new System.Drawing.Size(86, 23);
            this.txtGrid.TabIndex = 28;
            // 
            // txtKaishiRBangou
            // 
            this.txtKaishiRBangou.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtKaishiRBangou.Location = new System.Drawing.Point(232, 243);
            this.txtKaishiRBangou.MaxLength = 4;
            this.txtKaishiRBangou.Name = "txtKaishiRBangou";
            this.txtKaishiRBangou.Size = new System.Drawing.Size(86, 23);
            this.txtKaishiRBangou.TabIndex = 34;
            // 
            // txtChousaJisuu
            // 
            this.txtChousaJisuu.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtChousaJisuu.Location = new System.Drawing.Point(232, 21);
            this.txtChousaJisuu.MaxLength = 7;
            this.txtChousaJisuu.Name = "txtChousaJisuu";
            this.txtChousaJisuu.Size = new System.Drawing.Size(86, 23);
            this.txtChousaJisuu.TabIndex = 17;
            // 
            // lblKaishiRBangou
            // 
            this.lblKaishiRBangou.AutoSize = true;
            this.lblKaishiRBangou.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblKaishiRBangou.Location = new System.Drawing.Point(27, 246);
            this.lblKaishiRBangou.Name = "lblKaishiRBangou";
            this.lblKaishiRBangou.Size = new System.Drawing.Size(123, 16);
            this.lblKaishiRBangou.TabIndex = 33;
            this.lblKaishiRBangou.Text = "開始のR番号(&R)：";
            // 
            // txtDosoumei
            // 
            this.txtDosoumei.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtDosoumei.Location = new System.Drawing.Point(232, 129);
            this.txtDosoumei.MaxLength = 20;
            this.txtDosoumei.Name = "txtDosoumei";
            this.txtDosoumei.Size = new System.Drawing.Size(133, 23);
            this.txtDosoumei.TabIndex = 26;
            // 
            // txtIkoumei
            // 
            this.txtIkoumei.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtIkoumei.Location = new System.Drawing.Point(232, 102);
            this.txtIkoumei.MaxLength = 10;
            this.txtIkoumei.Name = "txtIkoumei";
            this.txtIkoumei.Size = new System.Drawing.Size(133, 23);
            this.txtIkoumei.TabIndex = 24;
            // 
            // txtChuushouChiku
            // 
            this.txtChuushouChiku.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtChuushouChiku.Location = new System.Drawing.Point(232, 75);
            this.txtChuushouChiku.MaxLength = 7;
            this.txtChuushouChiku.Name = "txtChuushouChiku";
            this.txtChuushouChiku.Size = new System.Drawing.Size(86, 23);
            this.txtChuushouChiku.TabIndex = 22;
            // 
            // txtOoChiku
            // 
            this.txtOoChiku.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtOoChiku.Location = new System.Drawing.Point(232, 48);
            this.txtOoChiku.MaxLength = 7;
            this.txtOoChiku.Name = "txtOoChiku";
            this.txtOoChiku.Size = new System.Drawing.Size(86, 23);
            this.txtOoChiku.TabIndex = 20;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDate.Location = new System.Drawing.Point(27, 186);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(100, 16);
            this.lblDate.TabIndex = 30;
            this.lblDate.Text = "出土日付(&D)：";
            // 
            // lblGrid
            // 
            this.lblGrid.AutoSize = true;
            this.lblGrid.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblGrid.Location = new System.Drawing.Point(27, 159);
            this.lblGrid.Name = "lblGrid";
            this.lblGrid.Size = new System.Drawing.Size(79, 16);
            this.lblGrid.TabIndex = 27;
            this.lblGrid.Text = "グリッド(&G)：";
            // 
            // lblDosoumei
            // 
            this.lblDosoumei.AutoSize = true;
            this.lblDosoumei.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDosoumei.Location = new System.Drawing.Point(27, 132);
            this.lblDosoumei.Name = "lblDosoumei";
            this.lblDosoumei.Size = new System.Drawing.Size(83, 16);
            this.lblDosoumei.TabIndex = 25;
            this.lblDosoumei.Text = "土層名(&L)：";
            // 
            // lblIkoumei
            // 
            this.lblIkoumei.AutoSize = true;
            this.lblIkoumei.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblIkoumei.Location = new System.Drawing.Point(27, 105);
            this.lblIkoumei.Name = "lblIkoumei";
            this.lblIkoumei.Size = new System.Drawing.Size(84, 16);
            this.lblIkoumei.TabIndex = 23;
            this.lblIkoumei.Text = "遺構名(&R)：";
            // 
            // lblChuusouChiku
            // 
            this.lblChuusouChiku.AutoSize = true;
            this.lblChuusouChiku.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblChuusouChiku.Location = new System.Drawing.Point(27, 78);
            this.lblChuusouChiku.Name = "lblChuusouChiku";
            this.lblChuusouChiku.Size = new System.Drawing.Size(181, 16);
            this.lblChuusouChiku.TabIndex = 21;
            this.lblChuusouChiku.Text = "調査地区－中小地区(&C)：";
            // 
            // lblOoChiku
            // 
            this.lblOoChiku.AutoSize = true;
            this.lblOoChiku.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblOoChiku.Location = new System.Drawing.Point(27, 51);
            this.lblOoChiku.Name = "lblOoChiku";
            this.lblOoChiku.Size = new System.Drawing.Size(164, 16);
            this.lblOoChiku.TabIndex = 19;
            this.lblOoChiku.Text = "調査地区－大地区(&A)：";
            // 
            // lblChousaJisuu
            // 
            this.lblChousaJisuu.AutoSize = true;
            this.lblChousaJisuu.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblChousaJisuu.Location = new System.Drawing.Point(27, 24);
            this.lblChousaJisuu.Name = "lblChousaJisuu";
            this.lblChousaJisuu.Size = new System.Drawing.Size(133, 16);
            this.lblChousaJisuu.TabIndex = 16;
            this.lblChousaJisuu.Text = "発掘調査次数(&O)：";
            // 
            // MkaWndSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(427, 406);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MkaWndSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "設定";
            this.Load += new System.EventHandler(this.MkaWndSettings_Load);
            this.tbpGraphics.ResumeLayout(false);
            this.tbpGraphics.PerformLayout();
            this.tbpDatabase.ResumeLayout(false);
            this.tbpDatabase.PerformLayout();
            this.tbSettings.ResumeLayout(false);
            this.tbpDefaultValues.ResumeLayout(false);
            this.tbpDefaultValues.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TabPage tbpGraphics;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.TabPage tbpDatabase;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tbSettings;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.TabPage tbpDefaultValues;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label lblDescription2;
        private System.Windows.Forms.Label lblDescription1;
        private NumbersOnlyTextBox txtGrid;
        private NumbersOnlyTextBox txtChousaJisuu;
        private System.Windows.Forms.TextBox txtDosoumei;
        private System.Windows.Forms.TextBox txtIkoumei;
        private System.Windows.Forms.TextBox txtChuushouChiku;
        private System.Windows.Forms.TextBox txtOoChiku;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblGrid;
        private System.Windows.Forms.Label lblDosoumei;
        private System.Windows.Forms.Label lblIkoumei;
        private System.Windows.Forms.Label lblChuusouChiku;
        private System.Windows.Forms.Label lblOoChiku;
        private System.Windows.Forms.Label lblChousaJisuu;
        private System.Windows.Forms.TextBox txtBatBangou;
        private System.Windows.Forms.Label lblBatBangou;
        private System.Windows.Forms.Label lblDescription4;
        private NumbersOnlyTextBox txtKaishiRBangou;
        private System.Windows.Forms.Label lblKaishiRBangou;
        private System.Windows.Forms.Label lblFontSize;
        private System.Windows.Forms.ComboBox cmbFontSize;
        private System.Windows.Forms.Label lblRegionFormat;
        internal MkaGraphicsProperties mkaGraphicsProperties;
    }
}