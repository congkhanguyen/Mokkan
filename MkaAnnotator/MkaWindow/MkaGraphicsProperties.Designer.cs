namespace MokkAnnotator.MkaWindow
{
    partial class MkaGraphicsProperties
    {
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.grbLine = new System.Windows.Forms.GroupBox();
            this.btnLineColor = new MokkAnnotator.MkaWindow.MkaColorButton();
            this.cmbLineWeight = new System.Windows.Forms.ComboBox();
            this.lblLineColor = new System.Windows.Forms.Label();
            this.lblLineWeight = new System.Windows.Forms.Label();
            this.grbFill = new System.Windows.Forms.GroupBox();
            this.btnFillColor = new MokkAnnotator.MkaWindow.MkaColorButton();
            this.cmbFillTransparent = new System.Windows.Forms.ComboBox();
            this.lblFillTransparent = new System.Windows.Forms.Label();
            this.lblFillColor = new System.Windows.Forms.Label();
            this.btnDefault = new System.Windows.Forms.Button();
            this.grbLine.SuspendLayout();
            this.grbFill.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbLine
            // 
            this.grbLine.Controls.Add(this.btnLineColor);
            this.grbLine.Controls.Add(this.cmbLineWeight);
            this.grbLine.Controls.Add(this.lblLineColor);
            this.grbLine.Controls.Add(this.lblLineWeight);
            this.grbLine.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.grbLine.Location = new System.Drawing.Point(2, 82);
            this.grbLine.Name = "grbLine";
            this.grbLine.Size = new System.Drawing.Size(180, 75);
            this.grbLine.TabIndex = 34;
            this.grbLine.TabStop = false;
            this.grbLine.Text = "枠線";
            // 
            // btnLineColor
            // 
            this.btnLineColor.CenterColor = System.Drawing.Color.DodgerBlue;
            this.btnLineColor.Location = new System.Drawing.Point(91, 15);
            this.btnLineColor.Name = "btnLineColor";
            this.btnLineColor.Size = new System.Drawing.Size(35, 23);
            this.btnLineColor.TabIndex = 33;
            this.btnLineColor.UseVisualStyleBackColor = true;
            this.btnLineColor.Click += new System.EventHandler(this.btnLineColor_Click);
            // 
            // cmbLineWeight
            // 
            this.cmbLineWeight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLineWeight.FormatString = "     ";
            this.cmbLineWeight.Location = new System.Drawing.Point(91, 44);
            this.cmbLineWeight.Name = "cmbLineWeight";
            this.cmbLineWeight.Size = new System.Drawing.Size(68, 21);
            this.cmbLineWeight.TabIndex = 34;
            this.cmbLineWeight.SelectedIndexChanged += new System.EventHandler(this.cmbLineWeight_SelectedIndexChanged);
            // 
            // lblLineColor
            // 
            this.lblLineColor.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblLineColor.Image = global::MokkAnnotator.Properties.Resources.linecolor;
            this.lblLineColor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLineColor.Location = new System.Drawing.Point(8, 17);
            this.lblLineColor.Name = "lblLineColor";
            this.lblLineColor.Size = new System.Drawing.Size(55, 21);
            this.lblLineColor.TabIndex = 32;
            this.lblLineColor.Text = "色";
            this.lblLineColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLineWeight
            // 
            this.lblLineWeight.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblLineWeight.Image = global::MokkAnnotator.Properties.Resources.lineweight;
            this.lblLineWeight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLineWeight.Location = new System.Drawing.Point(8, 42);
            this.lblLineWeight.Name = "lblLineWeight";
            this.lblLineWeight.Size = new System.Drawing.Size(65, 22);
            this.lblLineWeight.TabIndex = 33;
            this.lblLineWeight.Text = "太さ";
            this.lblLineWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grbFill
            // 
            this.grbFill.Controls.Add(this.btnFillColor);
            this.grbFill.Controls.Add(this.cmbFillTransparent);
            this.grbFill.Controls.Add(this.lblFillTransparent);
            this.grbFill.Controls.Add(this.lblFillColor);
            this.grbFill.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.grbFill.Location = new System.Drawing.Point(2, 2);
            this.grbFill.Name = "grbFill";
            this.grbFill.Size = new System.Drawing.Size(180, 74);
            this.grbFill.TabIndex = 33;
            this.grbFill.TabStop = false;
            this.grbFill.Text = "塗りつぶし";
            // 
            // btnFillColor
            // 
            this.btnFillColor.CenterColor = System.Drawing.Color.Empty;
            this.btnFillColor.Location = new System.Drawing.Point(91, 16);
            this.btnFillColor.Name = "btnFillColor";
            this.btnFillColor.Size = new System.Drawing.Size(35, 23);
            this.btnFillColor.TabIndex = 32;
            this.btnFillColor.UseVisualStyleBackColor = true;
            this.btnFillColor.Click += new System.EventHandler(this.btnFillColor_Click);
            // 
            // cmbFillTransparent
            // 
            this.cmbFillTransparent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFillTransparent.FormattingEnabled = true;
            this.cmbFillTransparent.Location = new System.Drawing.Point(91, 45);
            this.cmbFillTransparent.Name = "cmbFillTransparent";
            this.cmbFillTransparent.Size = new System.Drawing.Size(51, 21);
            this.cmbFillTransparent.TabIndex = 31;
            this.cmbFillTransparent.SelectedIndexChanged += new System.EventHandler(this.cmbFillTransparent_SelectedIndexChanged);
            // 
            // lblFillTransparent
            // 
            this.lblFillTransparent.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblFillTransparent.Image = global::MokkAnnotator.Properties.Resources.filltransparent;
            this.lblFillTransparent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFillTransparent.Location = new System.Drawing.Point(8, 43);
            this.lblFillTransparent.Name = "lblFillTransparent";
            this.lblFillTransparent.Size = new System.Drawing.Size(84, 22);
            this.lblFillTransparent.TabIndex = 29;
            this.lblFillTransparent.Text = "透明性";
            this.lblFillTransparent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblFillColor
            // 
            this.lblFillColor.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblFillColor.Image = global::MokkAnnotator.Properties.Resources.fillcolor;
            this.lblFillColor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblFillColor.Location = new System.Drawing.Point(8, 18);
            this.lblFillColor.Name = "lblFillColor";
            this.lblFillColor.Size = new System.Drawing.Size(56, 21);
            this.lblFillColor.TabIndex = 26;
            this.lblFillColor.Text = "色";
            this.lblFillColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(0, 163);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(75, 23);
            this.btnDefault.TabIndex = 35;
            this.btnDefault.Text = "初期値(&D)";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // MkaGraphicsProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.grbLine);
            this.Controls.Add(this.grbFill);
            this.Name = "MkaGraphicsProperties";
            this.Size = new System.Drawing.Size(185, 189);
            this.Load += new System.EventHandler(this.uctrlGraphics_Load);
            this.grbLine.ResumeLayout(false);
            this.grbFill.ResumeLayout(false);
            this.ResumeLayout(false);

        }

       #endregion

        private System.Windows.Forms.GroupBox grbLine;
        private System.Windows.Forms.Label lblLineColor;
        private System.Windows.Forms.Label lblLineWeight;
        private System.Windows.Forms.GroupBox grbFill;
        private System.Windows.Forms.Label lblFillTransparent;
        private System.Windows.Forms.Label lblFillColor;
        private System.Windows.Forms.ComboBox cmbLineWeight;
        private System.Windows.Forms.ComboBox cmbFillTransparent;
        private MkaColorButton btnLineColor;
        private System.Windows.Forms.Button btnDefault;
        internal MkaColorButton btnFillColor;
   }
}
