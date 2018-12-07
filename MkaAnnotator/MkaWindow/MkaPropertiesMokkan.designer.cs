namespace MokkAnnotator.MkaWindow
{
    partial class MkaPropertiesMokkan
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbCtrlProperty = new System.Windows.Forms.TabControl();
            this.tbpMokkanInfo = new System.Windows.Forms.TabPage();
            this.lblDescription1 = new System.Windows.Forms.Label();
            this.txtBikou = new System.Windows.Forms.TextBox();
            this.txtShashinBangouJyouhou = new System.Windows.Forms.TextBox();
            this.txtGaihouShoshuuJyouhou = new System.Windows.Forms.TextBox();
            this.txtKariShakubun = new System.Windows.Forms.TextBox();
            this.lblBikou = new System.Windows.Forms.Label();
            this.lblShashinBangouJyouhou = new System.Windows.Forms.Label();
            this.lblGaihouShoshuuJyouhou = new System.Windows.Forms.Label();
            this.lblKariShakubun = new System.Windows.Forms.Label();
            this.lblRBangou = new System.Windows.Forms.Label();
            this.tbpGraphics = new System.Windows.Forms.TabPage();
            this.grbRPosition = new System.Windows.Forms.GroupBox();
            this.cmbRShowPosition = new System.Windows.Forms.ComboBox();
            this.txtRBangou = new MokkAnnotator.MkaWindow.NumbersOnlyTextBox();
            this.mkaGraphicsProperties = new MokkAnnotator.MkaWindow.MkaGraphicsProperties();
            this.tbCtrlProperty.SuspendLayout();
            this.tbpMokkanInfo.SuspendLayout();
            this.tbpGraphics.SuspendLayout();
            this.grbRPosition.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(176, 240);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(78, 25);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(267, 240);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(78, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "キャンセル(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tbCtrlProperty
            // 
            this.tbCtrlProperty.Controls.Add(this.tbpMokkanInfo);
            this.tbCtrlProperty.Controls.Add(this.tbpGraphics);
            this.tbCtrlProperty.Location = new System.Drawing.Point(3, 4);
            this.tbCtrlProperty.Name = "tbCtrlProperty";
            this.tbCtrlProperty.SelectedIndex = 0;
            this.tbCtrlProperty.Size = new System.Drawing.Size(515, 222);
            this.tbCtrlProperty.TabIndex = 1;
            // 
            // tbpMokkanInfo
            // 
            this.tbpMokkanInfo.Controls.Add(this.lblDescription1);
            this.tbpMokkanInfo.Controls.Add(this.txtRBangou);
            this.tbpMokkanInfo.Controls.Add(this.txtBikou);
            this.tbpMokkanInfo.Controls.Add(this.txtShashinBangouJyouhou);
            this.tbpMokkanInfo.Controls.Add(this.txtGaihouShoshuuJyouhou);
            this.tbpMokkanInfo.Controls.Add(this.txtKariShakubun);
            this.tbpMokkanInfo.Controls.Add(this.lblBikou);
            this.tbpMokkanInfo.Controls.Add(this.lblShashinBangouJyouhou);
            this.tbpMokkanInfo.Controls.Add(this.lblGaihouShoshuuJyouhou);
            this.tbpMokkanInfo.Controls.Add(this.lblKariShakubun);
            this.tbpMokkanInfo.Controls.Add(this.lblRBangou);
            this.tbpMokkanInfo.Location = new System.Drawing.Point(4, 22);
            this.tbpMokkanInfo.Name = "tbpMokkanInfo";
            this.tbpMokkanInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tbpMokkanInfo.Size = new System.Drawing.Size(507, 196);
            this.tbpMokkanInfo.TabIndex = 0;
            this.tbpMokkanInfo.Text = "木簡情報";
            this.tbpMokkanInfo.UseVisualStyleBackColor = true;
            // 
            // lblDescription1
            // 
            this.lblDescription1.AutoSize = true;
            this.lblDescription1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDescription1.Location = new System.Drawing.Point(200, 29);
            this.lblDescription1.Name = "lblDescription1";
            this.lblDescription1.Size = new System.Drawing.Size(41, 13);
            this.lblDescription1.TabIndex = 2;
            this.lblDescription1.Text = "(数字)";
            // 
            // txtBikou
            // 
            this.txtBikou.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtBikou.Location = new System.Drawing.Point(132, 134);
            this.txtBikou.MaxLength = 1000;
            this.txtBikou.Name = "txtBikou";
            this.txtBikou.Size = new System.Drawing.Size(369, 20);
            this.txtBikou.TabIndex = 10;
            // 
            // txtShashinBangouJyouhou
            // 
            this.txtShashinBangouJyouhou.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtShashinBangouJyouhou.Location = new System.Drawing.Point(132, 107);
            this.txtShashinBangouJyouhou.MaxLength = 1000;
            this.txtShashinBangouJyouhou.Name = "txtShashinBangouJyouhou";
            this.txtShashinBangouJyouhou.Size = new System.Drawing.Size(369, 20);
            this.txtShashinBangouJyouhou.TabIndex = 8;
            // 
            // txtGaihouShoshuuJyouhou
            // 
            this.txtGaihouShoshuuJyouhou.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtGaihouShoshuuJyouhou.Location = new System.Drawing.Point(132, 80);
            this.txtGaihouShoshuuJyouhou.MaxLength = 1000;
            this.txtGaihouShoshuuJyouhou.Name = "txtGaihouShoshuuJyouhou";
            this.txtGaihouShoshuuJyouhou.Size = new System.Drawing.Size(369, 20);
            this.txtGaihouShoshuuJyouhou.TabIndex = 6;
            // 
            // txtKariShakubun
            // 
            this.txtKariShakubun.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtKariShakubun.Location = new System.Drawing.Point(132, 53);
            this.txtKariShakubun.MaxLength = 1000;
            this.txtKariShakubun.Name = "txtKariShakubun";
            this.txtKariShakubun.Size = new System.Drawing.Size(369, 20);
            this.txtKariShakubun.TabIndex = 4;
            // 
            // lblBikou
            // 
            this.lblBikou.AutoSize = true;
            this.lblBikou.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblBikou.Location = new System.Drawing.Point(17, 137);
            this.lblBikou.Name = "lblBikou";
            this.lblBikou.Size = new System.Drawing.Size(57, 13);
            this.lblBikou.TabIndex = 9;
            this.lblBikou.Text = "備考(&C)：";
            // 
            // lblShashinBangouJyouhou
            // 
            this.lblShashinBangouJyouhou.AutoSize = true;
            this.lblShashinBangouJyouhou.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblShashinBangouJyouhou.Location = new System.Drawing.Point(17, 110);
            this.lblShashinBangouJyouhou.Name = "lblShashinBangouJyouhou";
            this.lblShashinBangouJyouhou.Size = new System.Drawing.Size(108, 13);
            this.lblShashinBangouJyouhou.TabIndex = 7;
            this.lblShashinBangouJyouhou.Text = "写真番号情報(&S)：";
            // 
            // lblGaihouShoshuuJyouhou
            // 
            this.lblGaihouShoshuuJyouhou.AutoSize = true;
            this.lblGaihouShoshuuJyouhou.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblGaihouShoshuuJyouhou.Location = new System.Drawing.Point(17, 83);
            this.lblGaihouShoshuuJyouhou.Name = "lblGaihouShoshuuJyouhou";
            this.lblGaihouShoshuuJyouhou.Size = new System.Drawing.Size(109, 13);
            this.lblGaihouShoshuuJyouhou.TabIndex = 5;
            this.lblGaihouShoshuuJyouhou.Text = "概報所収情報(&G)：";
            // 
            // lblKariShakubun
            // 
            this.lblKariShakubun.AutoSize = true;
            this.lblKariShakubun.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblKariShakubun.Location = new System.Drawing.Point(17, 56);
            this.lblKariShakubun.Name = "lblKariShakubun";
            this.lblKariShakubun.Size = new System.Drawing.Size(69, 13);
            this.lblKariShakubun.TabIndex = 3;
            this.lblKariShakubun.Text = "仮釈文(&K)：";
            // 
            // lblRBangou
            // 
            this.lblRBangou.AutoSize = true;
            this.lblRBangou.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblRBangou.Location = new System.Drawing.Point(17, 29);
            this.lblRBangou.Name = "lblRBangou";
            this.lblRBangou.Size = new System.Drawing.Size(64, 13);
            this.lblRBangou.TabIndex = 0;
            this.lblRBangou.Text = "R番号(&R)：";
            // 
            // tbpGraphics
            // 
            this.tbpGraphics.Controls.Add(this.grbRPosition);
            this.tbpGraphics.Controls.Add(this.mkaGraphicsProperties);
            this.tbpGraphics.Location = new System.Drawing.Point(4, 22);
            this.tbpGraphics.Name = "tbpGraphics";
            this.tbpGraphics.Padding = new System.Windows.Forms.Padding(3);
            this.tbpGraphics.Size = new System.Drawing.Size(507, 196);
            this.tbpGraphics.TabIndex = 1;
            this.tbpGraphics.Text = "書式";
            this.tbpGraphics.UseVisualStyleBackColor = true;
            // 
            // grbRPosition
            // 
            this.grbRPosition.Controls.Add(this.cmbRShowPosition);
            this.grbRPosition.Location = new System.Drawing.Point(244, 9);
            this.grbRPosition.Name = "grbRPosition";
            this.grbRPosition.Size = new System.Drawing.Size(142, 63);
            this.grbRPosition.TabIndex = 1;
            this.grbRPosition.TabStop = false;
            this.grbRPosition.Text = "R番号表示位置";
            // 
            // cmbRShowPosition
            // 
            this.cmbRShowPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRShowPosition.FormattingEnabled = true;
            this.cmbRShowPosition.Location = new System.Drawing.Point(31, 21);
            this.cmbRShowPosition.Name = "cmbRShowPosition";
            this.cmbRShowPosition.Size = new System.Drawing.Size(81, 20);
            this.cmbRShowPosition.TabIndex = 0;
            // 
            // txtRBangou
            // 
            this.txtRBangou.Location = new System.Drawing.Point(132, 27);
            this.txtRBangou.MaxLength = 7;
            this.txtRBangou.Name = "txtRBangou";
            this.txtRBangou.Size = new System.Drawing.Size(62, 19);
            this.txtRBangou.TabIndex = 1;
            // 
            // mkaGraphicsProperties
            // 
            this.mkaGraphicsProperties.AutoSize = true;
            this.mkaGraphicsProperties.ChangeLast = false;
            this.mkaGraphicsProperties.Location = new System.Drawing.Point(24, 6);
            this.mkaGraphicsProperties.Name = "mkaGraphicsProperties";
            graphicsProperties1.BorderColor = System.Drawing.Color.DodgerBlue;
            graphicsProperties1.BorderColorHtml = "DodgerBlue";
            graphicsProperties1.FillColor = System.Drawing.Color.DeepSkyBlue;
            graphicsProperties1.FillColorAlpha = 50;
            graphicsProperties1.FillColorHtml = "DeepSkyBlue";
            graphicsProperties1.PenWidth = 1F;
            graphicsProperties1.RShowPosition = MokkAnnotator.MkaCommon.ShowPosition.Top;
            this.mkaGraphicsProperties.Properties = graphicsProperties1;
            this.mkaGraphicsProperties.Size = new System.Drawing.Size(185, 189);
            this.mkaGraphicsProperties.TabIndex = 0;
            // 
            // MkaPropertiesMokkan
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(520, 277);
            this.Controls.Add(this.tbCtrlProperty);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MkaPropertiesMokkan";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "木簡のプロパティ";
            this.Load += new System.EventHandler(this.MkaPropertiesDialog_Load);
            this.tbCtrlProperty.ResumeLayout(false);
            this.tbpMokkanInfo.ResumeLayout(false);
            this.tbpMokkanInfo.PerformLayout();
            this.tbpGraphics.ResumeLayout(false);
            this.tbpGraphics.PerformLayout();
            this.grbRPosition.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private MkaGraphicsProperties mkaGraphicsProperties;
        private System.Windows.Forms.TabControl tbCtrlProperty;
        private System.Windows.Forms.TabPage tbpMokkanInfo;
        private System.Windows.Forms.TabPage tbpGraphics;
        private System.Windows.Forms.Label lblDescription1;
        private NumbersOnlyTextBox txtRBangou;
        private System.Windows.Forms.TextBox txtBikou;
        private System.Windows.Forms.TextBox txtShashinBangouJyouhou;
        private System.Windows.Forms.TextBox txtGaihouShoshuuJyouhou;
        private System.Windows.Forms.TextBox txtKariShakubun;
        private System.Windows.Forms.Label lblBikou;
        private System.Windows.Forms.Label lblShashinBangouJyouhou;
        private System.Windows.Forms.Label lblGaihouShoshuuJyouhou;
        private System.Windows.Forms.Label lblKariShakubun;
        private System.Windows.Forms.Label lblRBangou;
        private System.Windows.Forms.GroupBox grbRPosition;
        private System.Windows.Forms.ComboBox cmbRShowPosition;
    }
}