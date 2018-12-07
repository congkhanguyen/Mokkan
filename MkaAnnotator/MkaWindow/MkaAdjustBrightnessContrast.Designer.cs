namespace MokkAnnotator.MkaWindow
{
    partial class MkaAdjustBrightnessContrast
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
            this.trackBarBrightness = new System.Windows.Forms.TrackBar();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblBrightness = new System.Windows.Forms.Label();
            this.txtBrightness = new System.Windows.Forms.TextBox();
            this.lblMinB = new System.Windows.Forms.Label();
            this.lblMaxB = new System.Windows.Forms.Label();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.grbPreview = new System.Windows.Forms.GroupBox();
            this.lblCenterB = new System.Windows.Forms.Label();
            this.lblCenterC = new System.Windows.Forms.Label();
            this.lblMaxC = new System.Windows.Forms.Label();
            this.lblMinC = new System.Windows.Forms.Label();
            this.txtContrast = new System.Windows.Forms.TextBox();
            this.lblContrast = new System.Windows.Forms.Label();
            this.trackBarContrast = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.grbPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarContrast)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarBrightness
            // 
            this.trackBarBrightness.Location = new System.Drawing.Point(12, 32);
            this.trackBarBrightness.Maximum = 100;
            this.trackBarBrightness.Minimum = -100;
            this.trackBarBrightness.Name = "trackBarBrightness";
            this.trackBarBrightness.Size = new System.Drawing.Size(280, 45);
            this.trackBarBrightness.TabIndex = 1;
            this.trackBarBrightness.TickFrequency = 10;
            this.trackBarBrightness.Scroll += new System.EventHandler(this.trackBarBrightness_Scroll);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(12, 193);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(80, 23);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "元に戻す(&Z)";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(142, 193);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(228, 193);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "キャンセル(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblBrightness
            // 
            this.lblBrightness.AutoSize = true;
            this.lblBrightness.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblBrightness.Location = new System.Drawing.Point(17, 8);
            this.lblBrightness.Name = "lblBrightness";
            this.lblBrightness.Size = new System.Drawing.Size(42, 14);
            this.lblBrightness.TabIndex = 7;
            this.lblBrightness.Text = "明るさ";
            // 
            // txtBrightness
            // 
            this.txtBrightness.BackColor = System.Drawing.Color.White;
            this.txtBrightness.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.txtBrightness.Location = new System.Drawing.Point(93, 5);
            this.txtBrightness.MaxLength = 6;
            this.txtBrightness.Name = "txtBrightness";
            this.txtBrightness.Size = new System.Drawing.Size(49, 21);
            this.txtBrightness.TabIndex = 0;
            this.txtBrightness.Text = "0";
            this.txtBrightness.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBrightness.TextChanged += new System.EventHandler(this.txtBrightness_TextChanged);
            this.txtBrightness.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleKeyPress);
            // 
            // lblMinB
            // 
            this.lblMinB.AutoSize = true;
            this.lblMinB.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblMinB.Location = new System.Drawing.Point(12, 63);
            this.lblMinB.Name = "lblMinB";
            this.lblMinB.Size = new System.Drawing.Size(29, 11);
            this.lblMinB.TabIndex = 8;
            this.lblMinB.Text = "-100";
            // 
            // lblMaxB
            // 
            this.lblMaxB.AutoSize = true;
            this.lblMaxB.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblMaxB.Location = new System.Drawing.Point(269, 63);
            this.lblMaxB.Name = "lblMaxB";
            this.lblMaxB.Size = new System.Drawing.Size(23, 11);
            this.lblMaxB.TabIndex = 10;
            this.lblMaxB.Text = "100";
            // 
            // pbPreview
            // 
            this.pbPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbPreview.Location = new System.Drawing.Point(7, 15);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(200, 200);
            this.pbPreview.TabIndex = 17;
            this.pbPreview.TabStop = false;
            // 
            // grbPreview
            // 
            this.grbPreview.Controls.Add(this.pbPreview);
            this.grbPreview.Location = new System.Drawing.Point(314, 3);
            this.grbPreview.Name = "grbPreview";
            this.grbPreview.Size = new System.Drawing.Size(214, 222);
            this.grbPreview.TabIndex = 15;
            this.grbPreview.TabStop = false;
            this.grbPreview.Text = "プレビュー";
            // 
            // lblCenterB
            // 
            this.lblCenterB.AutoSize = true;
            this.lblCenterB.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblCenterB.Location = new System.Drawing.Point(147, 63);
            this.lblCenterB.Name = "lblCenterB";
            this.lblCenterB.Size = new System.Drawing.Size(11, 11);
            this.lblCenterB.TabIndex = 9;
            this.lblCenterB.Text = "0";
            // 
            // lblCenterC
            // 
            this.lblCenterC.AutoSize = true;
            this.lblCenterC.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblCenterC.Location = new System.Drawing.Point(147, 151);
            this.lblCenterC.Name = "lblCenterC";
            this.lblCenterC.Size = new System.Drawing.Size(11, 11);
            this.lblCenterC.TabIndex = 13;
            this.lblCenterC.Text = "0";
            // 
            // lblMaxC
            // 
            this.lblMaxC.AutoSize = true;
            this.lblMaxC.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblMaxC.Location = new System.Drawing.Point(269, 151);
            this.lblMaxC.Name = "lblMaxC";
            this.lblMaxC.Size = new System.Drawing.Size(23, 11);
            this.lblMaxC.TabIndex = 14;
            this.lblMaxC.Text = "100";
            // 
            // lblMinC
            // 
            this.lblMinC.AutoSize = true;
            this.lblMinC.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblMinC.Location = new System.Drawing.Point(12, 151);
            this.lblMinC.Name = "lblMinC";
            this.lblMinC.Size = new System.Drawing.Size(29, 11);
            this.lblMinC.TabIndex = 12;
            this.lblMinC.Text = "-100";
            // 
            // txtContrast
            // 
            this.txtContrast.BackColor = System.Drawing.Color.White;
            this.txtContrast.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.txtContrast.Location = new System.Drawing.Point(93, 93);
            this.txtContrast.MaxLength = 6;
            this.txtContrast.Name = "txtContrast";
            this.txtContrast.Size = new System.Drawing.Size(49, 21);
            this.txtContrast.TabIndex = 2;
            this.txtContrast.Text = "0";
            this.txtContrast.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtContrast.TextChanged += new System.EventHandler(this.txtContrast_TextChanged);
            this.txtContrast.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleKeyPress);
            // 
            // lblContrast
            // 
            this.lblContrast.AutoSize = true;
            this.lblContrast.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblContrast.Location = new System.Drawing.Point(17, 96);
            this.lblContrast.Name = "lblContrast";
            this.lblContrast.Size = new System.Drawing.Size(68, 14);
            this.lblContrast.TabIndex = 11;
            this.lblContrast.Text = "コントラスト";
            // 
            // trackBarContrast
            // 
            this.trackBarContrast.Location = new System.Drawing.Point(12, 120);
            this.trackBarContrast.Maximum = 100;
            this.trackBarContrast.Minimum = -100;
            this.trackBarContrast.Name = "trackBarContrast";
            this.trackBarContrast.Size = new System.Drawing.Size(280, 45);
            this.trackBarContrast.TabIndex = 3;
            this.trackBarContrast.TickFrequency = 10;
            this.trackBarContrast.Scroll += new System.EventHandler(this.trackBarContrast_Scroll);
            // 
            // MkaAdjustBrightnessContrast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 232);
            this.Controls.Add(this.lblCenterC);
            this.Controls.Add(this.lblMaxC);
            this.Controls.Add(this.lblMinC);
            this.Controls.Add(this.txtContrast);
            this.Controls.Add(this.lblContrast);
            this.Controls.Add(this.trackBarContrast);
            this.Controls.Add(this.lblCenterB);
            this.Controls.Add(this.lblMaxB);
            this.Controls.Add(this.lblMinB);
            this.Controls.Add(this.txtBrightness);
            this.Controls.Add(this.lblBrightness);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.trackBarBrightness);
            this.Controls.Add(this.grbPreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MkaAdjustBrightnessContrast";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "明るさ・コントラス調整";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.grbPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarContrast)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBarBrightness;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblBrightness;
        private System.Windows.Forms.TextBox txtBrightness;
        private System.Windows.Forms.Label lblMinB;
        private System.Windows.Forms.Label lblMaxB;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.GroupBox grbPreview;
        private System.Windows.Forms.Label lblCenterB;
        private System.Windows.Forms.Label lblCenterC;
        private System.Windows.Forms.Label lblMaxC;
        private System.Windows.Forms.Label lblMinC;
        private System.Windows.Forms.TextBox txtContrast;
        private System.Windows.Forms.Label lblContrast;
        private System.Windows.Forms.TrackBar trackBarContrast;
    }
}