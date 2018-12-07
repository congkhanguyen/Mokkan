namespace MkaAutoUpdate
{
    partial class MkaFrmUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MkaFrmUpdate));
            this.btnMokkanBrowser = new System.Windows.Forms.Button();
            this.txtMokkanLocation = new System.Windows.Forms.TextBox();
            this.btnInfoBrowser = new System.Windows.Forms.Button();
            this.txtInfoLocation = new System.Windows.Forms.TextBox();
            this.grpbInput = new System.Windows.Forms.GroupBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblInfoLocation = new System.Windows.Forms.Label();
            this.txtKaishiRBangou = new MokkAnnotator.MkaWindow.NumbersOnlyTextBox();
            this.lblKaishiRBangou = new System.Windows.Forms.Label();
            this.ckbFileUpdate = new System.Windows.Forms.CheckBox();
            this.lblMokkanLocation = new System.Windows.Forms.Label();
            this.ckbRegisterDb = new System.Windows.Forms.CheckBox();
            this.btnRun = new System.Windows.Forms.Button();
            this.grpbInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnMokkanBrowser
            // 
            this.btnMokkanBrowser.AutoSize = true;
            this.btnMokkanBrowser.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnMokkanBrowser.Location = new System.Drawing.Point(563, 21);
            this.btnMokkanBrowser.Margin = new System.Windows.Forms.Padding(4);
            this.btnMokkanBrowser.Name = "btnMokkanBrowser";
            this.btnMokkanBrowser.Size = new System.Drawing.Size(68, 26);
            this.btnMokkanBrowser.TabIndex = 2;
            this.btnMokkanBrowser.Text = "参照(&B)...";
            this.btnMokkanBrowser.UseVisualStyleBackColor = true;
            this.btnMokkanBrowser.Click += new System.EventHandler(this.btnMokkanBrowser_Click);
            // 
            // txtMokkanLocation
            // 
            this.txtMokkanLocation.BackColor = System.Drawing.SystemColors.Window;
            this.txtMokkanLocation.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtMokkanLocation.Location = new System.Drawing.Point(217, 25);
            this.txtMokkanLocation.Margin = new System.Windows.Forms.Padding(4);
            this.txtMokkanLocation.Name = "txtMokkanLocation";
            this.txtMokkanLocation.ReadOnly = true;
            this.txtMokkanLocation.Size = new System.Drawing.Size(339, 20);
            this.txtMokkanLocation.TabIndex = 1;
            // 
            // btnInfoBrowser
            // 
            this.btnInfoBrowser.AutoSize = true;
            this.btnInfoBrowser.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnInfoBrowser.Location = new System.Drawing.Point(513, 19);
            this.btnInfoBrowser.Margin = new System.Windows.Forms.Padding(4);
            this.btnInfoBrowser.Name = "btnInfoBrowser";
            this.btnInfoBrowser.Size = new System.Drawing.Size(68, 26);
            this.btnInfoBrowser.TabIndex = 2;
            this.btnInfoBrowser.Text = "参照(&B)...";
            this.btnInfoBrowser.UseVisualStyleBackColor = true;
            this.btnInfoBrowser.Click += new System.EventHandler(this.btnInfoBrowser_Click);
            // 
            // txtInfoLocation
            // 
            this.txtInfoLocation.BackColor = System.Drawing.SystemColors.Window;
            this.txtInfoLocation.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtInfoLocation.Location = new System.Drawing.Point(167, 23);
            this.txtInfoLocation.Margin = new System.Windows.Forms.Padding(4);
            this.txtInfoLocation.Name = "txtInfoLocation";
            this.txtInfoLocation.ReadOnly = true;
            this.txtInfoLocation.Size = new System.Drawing.Size(339, 20);
            this.txtInfoLocation.TabIndex = 1;
            // 
            // grpbInput
            // 
            this.grpbInput.Controls.Add(this.lblDescription);
            this.grpbInput.Controls.Add(this.lblInfoLocation);
            this.grpbInput.Controls.Add(this.txtKaishiRBangou);
            this.grpbInput.Controls.Add(this.lblKaishiRBangou);
            this.grpbInput.Controls.Add(this.btnInfoBrowser);
            this.grpbInput.Controls.Add(this.txtInfoLocation);
            this.grpbInput.Location = new System.Drawing.Point(102, 127);
            this.grpbInput.Name = "grpbInput";
            this.grpbInput.Size = new System.Drawing.Size(594, 84);
            this.grpbInput.TabIndex = 5;
            this.grpbInput.TabStop = false;
            this.grpbInput.Text = "更新情報";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDescription.ForeColor = System.Drawing.Color.Blue;
            this.lblDescription.Location = new System.Drawing.Point(291, 56);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(41, 13);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "(数字)";
            // 
            // lblInfoLocation
            // 
            this.lblInfoLocation.AutoSize = true;
            this.lblInfoLocation.Location = new System.Drawing.Point(7, 23);
            this.lblInfoLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblInfoLocation.Name = "lblInfoLocation";
            this.lblInfoLocation.Size = new System.Drawing.Size(150, 16);
            this.lblInfoLocation.TabIndex = 0;
            this.lblInfoLocation.Text = "調査情報テーブル(&S)：";
            // 
            // txtKaishiRBangou
            // 
            this.txtKaishiRBangou.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.txtKaishiRBangou.Location = new System.Drawing.Point(167, 50);
            this.txtKaishiRBangou.MaxLength = 5;
            this.txtKaishiRBangou.Name = "txtKaishiRBangou";
            this.txtKaishiRBangou.Size = new System.Drawing.Size(86, 23);
            this.txtKaishiRBangou.TabIndex = 4;
            this.txtKaishiRBangou.Text = "10000";
            // 
            // lblKaishiRBangou
            // 
            this.lblKaishiRBangou.AutoSize = true;
            this.lblKaishiRBangou.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblKaishiRBangou.Location = new System.Drawing.Point(7, 53);
            this.lblKaishiRBangou.Name = "lblKaishiRBangou";
            this.lblKaishiRBangou.Size = new System.Drawing.Size(123, 16);
            this.lblKaishiRBangou.TabIndex = 3;
            this.lblKaishiRBangou.Text = "開始のR番号(&R)：";
            // 
            // ckbFileUpdate
            // 
            this.ckbFileUpdate.AutoSize = true;
            this.ckbFileUpdate.Location = new System.Drawing.Point(71, 101);
            this.ckbFileUpdate.Name = "ckbFileUpdate";
            this.ckbFileUpdate.Size = new System.Drawing.Size(161, 20);
            this.ckbFileUpdate.TabIndex = 4;
            this.ckbFileUpdate.Text = "木簡情報を更新する";
            this.ckbFileUpdate.UseVisualStyleBackColor = true;
            this.ckbFileUpdate.CheckedChanged += new System.EventHandler(this.ckbFileUpdate_CheckedChanged);
            // 
            // lblMokkanLocation
            // 
            this.lblMokkanLocation.AutoSize = true;
            this.lblMokkanLocation.Location = new System.Drawing.Point(57, 25);
            this.lblMokkanLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMokkanLocation.Name = "lblMokkanLocation";
            this.lblMokkanLocation.Size = new System.Drawing.Size(131, 16);
            this.lblMokkanLocation.TabIndex = 0;
            this.lblMokkanLocation.Text = "木簡保存場所(&L)：";
            // 
            // ckbRegisterDb
            // 
            this.ckbRegisterDb.AutoSize = true;
            this.ckbRegisterDb.Checked = true;
            this.ckbRegisterDb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbRegisterDb.Location = new System.Drawing.Point(71, 65);
            this.ckbRegisterDb.Name = "ckbRegisterDb";
            this.ckbRegisterDb.Size = new System.Drawing.Size(218, 20);
            this.ckbRegisterDb.TabIndex = 3;
            this.ckbRegisterDb.Text = "データベースへ一括に登録する";
            this.ckbRegisterDb.UseVisualStyleBackColor = true;
            this.ckbRegisterDb.CheckedChanged += new System.EventHandler(this.ckbRegisterDb_CheckedChanged);
            // 
            // btnRun
            // 
            this.btnRun.AutoSize = true;
            this.btnRun.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnRun.Location = new System.Drawing.Point(333, 242);
            this.btnRun.Margin = new System.Windows.Forms.Padding(4);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(82, 26);
            this.btnRun.TabIndex = 6;
            this.btnRun.Text = "実行(&R)";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // MkaFrmUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 281);
            this.Controls.Add(this.ckbRegisterDb);
            this.Controls.Add(this.ckbFileUpdate);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.grpbInput);
            this.Controls.Add(this.lblMokkanLocation);
            this.Controls.Add(this.txtMokkanLocation);
            this.Controls.Add(this.btnMokkanBrowser);
            this.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MkaFrmUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "木簡情報一括更新・登録";
            this.grpbInput.ResumeLayout(false);
            this.grpbInput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMokkanBrowser;
        private System.Windows.Forms.TextBox txtMokkanLocation;
        private System.Windows.Forms.Button btnInfoBrowser;
        private System.Windows.Forms.TextBox txtInfoLocation;
        private System.Windows.Forms.GroupBox grpbInput;
        private System.Windows.Forms.Label lblInfoLocation;
        private System.Windows.Forms.Label lblMokkanLocation;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Label lblDescription;
        private MokkAnnotator.MkaWindow.NumbersOnlyTextBox txtKaishiRBangou;
        private System.Windows.Forms.Label lblKaishiRBangou;
        private System.Windows.Forms.CheckBox ckbFileUpdate;
        private System.Windows.Forms.CheckBox ckbRegisterDb;
    }
}

