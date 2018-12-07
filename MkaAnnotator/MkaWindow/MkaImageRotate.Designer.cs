namespace MokkAnnotator.MkaWindow
{
    partial class MkaImageRotate
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
            this.lbl90 = new System.Windows.Forms.Label();
            this.lbl0 = new System.Windows.Forms.Label();
            this.lbl180 = new System.Windows.Forms.Label();
            this.lbl270 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.txtAngle = new System.Windows.Forms.TextBox();
            this.lblAngle = new System.Windows.Forms.Label();
            this.lblFillColor = new System.Windows.Forms.Label();
            this.lblInterpolation = new System.Windows.Forms.Label();
            this.cmbInterpolation = new System.Windows.Forms.ComboBox();
            this.mkaColorButton = new MokkAnnotator.MkaWindow.MkaColorButton();
            this.mkaAngleSelector = new AngleAltitudeControls.MkaAngleSelector();
            this.SuspendLayout();
            // 
            // lbl90
            // 
            this.lbl90.AutoSize = true;
            this.lbl90.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lbl90.Location = new System.Drawing.Point(72, 9);
            this.lbl90.Name = "lbl90";
            this.lbl90.Size = new System.Drawing.Size(17, 11);
            this.lbl90.TabIndex = 7;
            this.lbl90.Text = "90";
            // 
            // lbl0
            // 
            this.lbl0.AutoSize = true;
            this.lbl0.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lbl0.Location = new System.Drawing.Point(136, 65);
            this.lbl0.Name = "lbl0";
            this.lbl0.Size = new System.Drawing.Size(11, 11);
            this.lbl0.TabIndex = 10;
            this.lbl0.Text = "0";
            // 
            // lbl180
            // 
            this.lbl180.AutoSize = true;
            this.lbl180.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lbl180.Location = new System.Drawing.Point(2, 65);
            this.lbl180.Name = "lbl180";
            this.lbl180.Size = new System.Drawing.Size(23, 11);
            this.lbl180.TabIndex = 8;
            this.lbl180.Text = "180";
            // 
            // lbl270
            // 
            this.lbl270.AutoSize = true;
            this.lbl270.Font = new System.Drawing.Font("MS UI Gothic", 8F);
            this.lbl270.Location = new System.Drawing.Point(69, 123);
            this.lbl270.Name = "lbl270";
            this.lbl270.Size = new System.Drawing.Size(23, 11);
            this.lbl270.TabIndex = 9;
            this.lbl270.Text = "270";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(262, 163);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "キャンセル(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(176, 163);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(12, 163);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(80, 23);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "元に戻す(&Z)";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // txtAngle
            // 
            this.txtAngle.BackColor = System.Drawing.Color.White;
            this.txtAngle.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.txtAngle.Location = new System.Drawing.Point(245, 18);
            this.txtAngle.MaxLength = 6;
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(49, 21);
            this.txtAngle.TabIndex = 1;
            this.txtAngle.Text = "0";
            this.txtAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtAngle.TextChanged += new System.EventHandler(this.txtAngle_TextChanged);
            this.txtAngle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HandleKeyPress);
            // 
            // lblAngle
            // 
            this.lblAngle.AutoSize = true;
            this.lblAngle.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblAngle.Location = new System.Drawing.Point(163, 21);
            this.lblAngle.Name = "lblAngle";
            this.lblAngle.Size = new System.Drawing.Size(46, 14);
            this.lblAngle.TabIndex = 11;
            this.lblAngle.Text = "色合い";
            // 
            // lblFillColor
            // 
            this.lblFillColor.AutoSize = true;
            this.lblFillColor.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblFillColor.Location = new System.Drawing.Point(163, 106);
            this.lblFillColor.Name = "lblFillColor";
            this.lblFillColor.Size = new System.Drawing.Size(76, 14);
            this.lblFillColor.TabIndex = 13;
            this.lblFillColor.Text = "塗りつぶし色";
            // 
            // lblInterpolation
            // 
            this.lblInterpolation.AutoSize = true;
            this.lblInterpolation.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblInterpolation.Location = new System.Drawing.Point(163, 63);
            this.lblInterpolation.Name = "lblInterpolation";
            this.lblInterpolation.Size = new System.Drawing.Size(35, 14);
            this.lblInterpolation.TabIndex = 12;
            this.lblInterpolation.Text = "補間";
            // 
            // cmbInterpolation
            // 
            this.cmbInterpolation.FormattingEnabled = true;
            this.cmbInterpolation.Items.AddRange(new object[] {
            "Nearest neighbour",
            "Bilinear",
            "Bicubic"});
            this.cmbInterpolation.Location = new System.Drawing.Point(245, 61);
            this.cmbInterpolation.Name = "cmbInterpolation";
            this.cmbInterpolation.Size = new System.Drawing.Size(123, 20);
            this.cmbInterpolation.TabIndex = 2;
            // 
            // mkaColorButton
            // 
            this.mkaColorButton.CenterColor = System.Drawing.Color.Empty;
            this.mkaColorButton.Location = new System.Drawing.Point(245, 102);
            this.mkaColorButton.Name = "mkaColorButton";
            this.mkaColorButton.Size = new System.Drawing.Size(35, 23);
            this.mkaColorButton.TabIndex = 3;
            this.mkaColorButton.UseVisualStyleBackColor = true;
            this.mkaColorButton.Click += new System.EventHandler(this.mkaColorButton_Click);
            // 
            // mkaAngleSelector
            // 
            this.mkaAngleSelector.Angle = 0;
            this.mkaAngleSelector.Location = new System.Drawing.Point(31, 21);
            this.mkaAngleSelector.Name = "mkaAngleSelector";
            this.mkaAngleSelector.Size = new System.Drawing.Size(99, 99);
            this.mkaAngleSelector.TabIndex = 0;
            this.mkaAngleSelector.AngleChanged += new AngleAltitudeControls.MkaAngleSelector.AngleChangedDelegate(this.mkaAngleSelector_AngleChanged);
            // 
            // MkaImageRotate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 202);
            this.Controls.Add(this.cmbInterpolation);
            this.Controls.Add(this.lblInterpolation);
            this.Controls.Add(this.lblFillColor);
            this.Controls.Add(this.mkaColorButton);
            this.Controls.Add(this.txtAngle);
            this.Controls.Add(this.lblAngle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.lbl270);
            this.Controls.Add(this.lbl180);
            this.Controls.Add(this.lbl0);
            this.Controls.Add(this.lbl90);
            this.Controls.Add(this.mkaAngleSelector);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MkaImageRotate";
            this.Text = "回転";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AngleAltitudeControls.MkaAngleSelector mkaAngleSelector;
        private System.Windows.Forms.Label lbl90;
        private System.Windows.Forms.Label lbl0;
        private System.Windows.Forms.Label lbl180;
        private System.Windows.Forms.Label lbl270;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TextBox txtAngle;
        private System.Windows.Forms.Label lblAngle;
        private MkaColorButton mkaColorButton;
        private System.Windows.Forms.Label lblFillColor;
        private System.Windows.Forms.Label lblInterpolation;
        private System.Windows.Forms.ComboBox cmbInterpolation;
    }
}