namespace MokkAnnotator.MkaWindow
{
    partial class MkaAdjustHSL
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
            this.trackBarHue = new System.Windows.Forms.TrackBar();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblHue = new System.Windows.Forms.Label();
            this.txtHue = new System.Windows.Forms.TextBox();
            this.lblMinH = new System.Windows.Forms.Label();
            this.lblMaxH = new System.Windows.Forms.Label();
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.grbPreview = new System.Windows.Forms.GroupBox();
            this.lblCenterH = new System.Windows.Forms.Label();
            this.lblCenterS = new System.Windows.Forms.Label();
            this.lblMaxS = new System.Windows.Forms.Label();
            this.lblMinS = new System.Windows.Forms.Label();
            this.txtSaturation = new System.Windows.Forms.TextBox();
            this.lblSaturation = new System.Windows.Forms.Label();
            this.trackBarSaturation = new System.Windows.Forms.TrackBar();
            this.lblCenterL = new System.Windows.Forms.Label();
            this.lblMaxL = new System.Windows.Forms.Label();
            this.lblMinL = new System.Windows.Forms.Label();
            this.txtLightness = new System.Windows.Forms.TextBox();
            this.lblLightness = new System.Windows.Forms.Label();
            this.trackBarLightness = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            this.grbPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSaturation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightness)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBarHue
            // 
            this.trackBarHue.Location = new System.Drawing.Point(12, 32);
            this.trackBarHue.Maximum = 180;
            this.trackBarHue.Minimum = -180;
            this.trackBarHue.Name = "trackBarHue";
            this.trackBarHue.Size = new System.Drawing.Size(280, 45);
            this.trackBarHue.TabIndex = 1;
            this.trackBarHue.TickFrequency = 10;
            this.trackBarHue.Scroll += new System.EventHandler(this.trackBarBrightness_Scroll);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(12, 262);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(80, 23);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "元に戻す(&Z)";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(321, 262);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(407, 262);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "キャンセル(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblHue
            // 
            this.lblHue.AutoSize = true;
            this.lblHue.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblHue.Location = new System.Drawing.Point(17, 8);
            this.lblHue.Name = "lblHue";
            this.lblHue.Size = new System.Drawing.Size(46, 14);
            this.lblHue.TabIndex = 9;
            this.lblHue.Text = "色合い";
            // 
            // txtHue
            // 
            this.txtHue.BackColor = System.Drawing.Color.White;
            this.txtHue.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.txtHue.Location = new System.Drawing.Point(93, 5);
            this.txtHue.MaxLength = 6;
            this.txtHue.Name = "txtHue";
            this.txtHue.Size = new System.Drawing.Size(49, 21);
            this.txtHue.TabIndex = 0;
            this.txtHue.Text = "0";
            this.txtHue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtHue.TextChanged += new System.EventHandler(this.txtHue_TextChanged);
            this.txtHue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleKeyPress);
            // 
            // lblMinH
            // 
            this.lblMinH.AutoSize = true;
            this.lblMinH.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblMinH.Location = new System.Drawing.Point(12, 63);
            this.lblMinH.Name = "lblMinH";
            this.lblMinH.Size = new System.Drawing.Size(29, 11);
            this.lblMinH.TabIndex = 10;
            this.lblMinH.Text = "-180";
            // 
            // lblMaxH
            // 
            this.lblMaxH.AutoSize = true;
            this.lblMaxH.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblMaxH.Location = new System.Drawing.Point(269, 63);
            this.lblMaxH.Name = "lblMaxH";
            this.lblMaxH.Size = new System.Drawing.Size(23, 11);
            this.lblMaxH.TabIndex = 12;
            this.lblMaxH.Text = "180";
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
            this.grbPreview.TabIndex = 21;
            this.grbPreview.TabStop = false;
            this.grbPreview.Text = "プレビュー";
            // 
            // lblCenterH
            // 
            this.lblCenterH.AutoSize = true;
            this.lblCenterH.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblCenterH.Location = new System.Drawing.Point(147, 63);
            this.lblCenterH.Name = "lblCenterH";
            this.lblCenterH.Size = new System.Drawing.Size(11, 11);
            this.lblCenterH.TabIndex = 11;
            this.lblCenterH.Text = "0";
            // 
            // lblCenterS
            // 
            this.lblCenterS.AutoSize = true;
            this.lblCenterS.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblCenterS.Location = new System.Drawing.Point(147, 151);
            this.lblCenterS.Name = "lblCenterS";
            this.lblCenterS.Size = new System.Drawing.Size(23, 11);
            this.lblCenterS.TabIndex = 15;
            this.lblCenterS.Text = "100";
            // 
            // lblMaxS
            // 
            this.lblMaxS.AutoSize = true;
            this.lblMaxS.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblMaxS.Location = new System.Drawing.Point(269, 151);
            this.lblMaxS.Name = "lblMaxS";
            this.lblMaxS.Size = new System.Drawing.Size(23, 11);
            this.lblMaxS.TabIndex = 16;
            this.lblMaxS.Text = "200";
            // 
            // lblMinS
            // 
            this.lblMinS.AutoSize = true;
            this.lblMinS.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblMinS.Location = new System.Drawing.Point(12, 151);
            this.lblMinS.Name = "lblMinS";
            this.lblMinS.Size = new System.Drawing.Size(11, 11);
            this.lblMinS.TabIndex = 14;
            this.lblMinS.Text = "0";
            // 
            // txtSaturation
            // 
            this.txtSaturation.BackColor = System.Drawing.Color.White;
            this.txtSaturation.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.txtSaturation.Location = new System.Drawing.Point(93, 93);
            this.txtSaturation.MaxLength = 6;
            this.txtSaturation.Name = "txtSaturation";
            this.txtSaturation.Size = new System.Drawing.Size(49, 21);
            this.txtSaturation.TabIndex = 2;
            this.txtSaturation.Text = "0";
            this.txtSaturation.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSaturation.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleKeyPress);
            // 
            // lblSaturation
            // 
            this.lblSaturation.AutoSize = true;
            this.lblSaturation.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblSaturation.Location = new System.Drawing.Point(17, 96);
            this.lblSaturation.Name = "lblSaturation";
            this.lblSaturation.Size = new System.Drawing.Size(54, 14);
            this.lblSaturation.TabIndex = 13;
            this.lblSaturation.Text = "鮮やかさ";
            // 
            // trackBarSaturation
            // 
            this.trackBarSaturation.Location = new System.Drawing.Point(12, 120);
            this.trackBarSaturation.Maximum = 200;
            this.trackBarSaturation.Name = "trackBarSaturation";
            this.trackBarSaturation.Size = new System.Drawing.Size(280, 45);
            this.trackBarSaturation.TabIndex = 3;
            this.trackBarSaturation.TickFrequency = 10;
            this.trackBarSaturation.Scroll += new System.EventHandler(this.trackBarSaturation_Scroll);
            // 
            // lblCenterL
            // 
            this.lblCenterL.AutoSize = true;
            this.lblCenterL.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblCenterL.Location = new System.Drawing.Point(147, 239);
            this.lblCenterL.Name = "lblCenterL";
            this.lblCenterL.Size = new System.Drawing.Size(11, 11);
            this.lblCenterL.TabIndex = 19;
            this.lblCenterL.Text = "0";
            // 
            // lblMaxL
            // 
            this.lblMaxL.AutoSize = true;
            this.lblMaxL.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblMaxL.Location = new System.Drawing.Point(269, 239);
            this.lblMaxL.Name = "lblMaxL";
            this.lblMaxL.Size = new System.Drawing.Size(23, 11);
            this.lblMaxL.TabIndex = 20;
            this.lblMaxL.Text = "100";
            // 
            // lblMinL
            // 
            this.lblMinL.AutoSize = true;
            this.lblMinL.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lblMinL.Location = new System.Drawing.Point(12, 239);
            this.lblMinL.Name = "lblMinL";
            this.lblMinL.Size = new System.Drawing.Size(29, 11);
            this.lblMinL.TabIndex = 18;
            this.lblMinL.Text = "-100";
            // 
            // txtLightness
            // 
            this.txtLightness.BackColor = System.Drawing.Color.White;
            this.txtLightness.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.txtLightness.Location = new System.Drawing.Point(93, 181);
            this.txtLightness.MaxLength = 6;
            this.txtLightness.Name = "txtLightness";
            this.txtLightness.Size = new System.Drawing.Size(49, 21);
            this.txtLightness.TabIndex = 4;
            this.txtLightness.Text = "0";
            this.txtLightness.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtLightness.TextChanged += new System.EventHandler(this.txtLightness_TextChanged);
            this.txtLightness.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleKeyPress);
            // 
            // lblLightness
            // 
            this.lblLightness.AutoSize = true;
            this.lblLightness.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblLightness.Location = new System.Drawing.Point(17, 184);
            this.lblLightness.Name = "lblLightness";
            this.lblLightness.Size = new System.Drawing.Size(42, 14);
            this.lblLightness.TabIndex = 17;
            this.lblLightness.Text = "明るさ";
            // 
            // trackBarLightness
            // 
            this.trackBarLightness.Location = new System.Drawing.Point(12, 208);
            this.trackBarLightness.Maximum = 100;
            this.trackBarLightness.Minimum = -100;
            this.trackBarLightness.Name = "trackBarLightness";
            this.trackBarLightness.Size = new System.Drawing.Size(280, 45);
            this.trackBarLightness.TabIndex = 5;
            this.trackBarLightness.TickFrequency = 10;
            this.trackBarLightness.Scroll += new System.EventHandler(this.trackBarLightness_Scroll);
            // 
            // MkaAdjustHSL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 297);
            this.Controls.Add(this.lblCenterL);
            this.Controls.Add(this.lblMaxL);
            this.Controls.Add(this.lblMinL);
            this.Controls.Add(this.txtLightness);
            this.Controls.Add(this.lblLightness);
            this.Controls.Add(this.trackBarLightness);
            this.Controls.Add(this.lblCenterS);
            this.Controls.Add(this.lblMaxS);
            this.Controls.Add(this.lblMinS);
            this.Controls.Add(this.txtSaturation);
            this.Controls.Add(this.lblSaturation);
            this.Controls.Add(this.trackBarSaturation);
            this.Controls.Add(this.lblCenterH);
            this.Controls.Add(this.lblMaxH);
            this.Controls.Add(this.lblMinH);
            this.Controls.Add(this.txtHue);
            this.Controls.Add(this.lblHue);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.trackBarHue);
            this.Controls.Add(this.grbPreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MkaAdjustHSL";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "色合い・鮮やかさ調整";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            this.grbPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSaturation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBarHue;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblHue;
        private System.Windows.Forms.TextBox txtHue;
        private System.Windows.Forms.Label lblMinH;
        private System.Windows.Forms.Label lblMaxH;
        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.GroupBox grbPreview;
        private System.Windows.Forms.Label lblCenterH;
        private System.Windows.Forms.Label lblCenterS;
        private System.Windows.Forms.Label lblMaxS;
        private System.Windows.Forms.Label lblMinS;
        private System.Windows.Forms.TextBox txtSaturation;
        private System.Windows.Forms.Label lblSaturation;
        private System.Windows.Forms.TrackBar trackBarSaturation;
        private System.Windows.Forms.Label lblCenterL;
        private System.Windows.Forms.Label lblMaxL;
        private System.Windows.Forms.Label lblMinL;
        private System.Windows.Forms.TextBox txtLightness;
        private System.Windows.Forms.Label lblLightness;
        private System.Windows.Forms.TrackBar trackBarLightness;
    }
}