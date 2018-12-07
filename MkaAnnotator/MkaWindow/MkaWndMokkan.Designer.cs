namespace MokkAnnotator.MkaWindow
{
    partial class MkaWndMokkan
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MkaWndMokkan));
            this.dgrMokkan = new System.Windows.Forms.DataGridView();
            this.dgcRBangou = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcKariShyakubun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcGaihouShoshuuJyouhou = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcShashinBangouJyouhou = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgcBikou = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgrMokkan)).BeginInit();
            this.SuspendLayout();
            // 
            // dgrMokkan
            // 
            this.dgrMokkan.AllowUserToAddRows = false;
            this.dgrMokkan.AllowUserToDeleteRows = false;
            this.dgrMokkan.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgrMokkan.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgrMokkan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgrMokkan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgcRBangou,
            this.dgcKariShyakubun,
            this.dgcGaihouShoshuuJyouhou,
            this.dgcShashinBangouJyouhou,
            this.dgcBikou});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgrMokkan.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgrMokkan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgrMokkan.Location = new System.Drawing.Point(0, 0);
            this.dgrMokkan.Name = "dgrMokkan";
            this.dgrMokkan.RowHeadersVisible = false;
            this.dgrMokkan.RowTemplate.Height = 21;
            this.dgrMokkan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgrMokkan.Size = new System.Drawing.Size(584, 162);
            this.dgrMokkan.TabIndex = 4;
            this.dgrMokkan.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgrMokkan_CellValueChanged);
            this.dgrMokkan.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgrMokkan_CellValidating);
            this.dgrMokkan.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgrMokkan_EditingControlShowing);
            this.dgrMokkan.SelectionChanged += new System.EventHandler(this.dgrMokkan_SelectionChanged);
            // 
            // dgcRBangou
            // 
            dataGridViewCellStyle2.NullValue = "1";
            this.dgcRBangou.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgcRBangou.HeaderText = "R番号";
            this.dgcRBangou.MaxInputLength = 7;
            this.dgcRBangou.MinimumWidth = 70;
            this.dgcRBangou.Name = "dgcRBangou";
            this.dgcRBangou.Width = 70;
            // 
            // dgcKariShyakubun
            // 
            this.dgcKariShyakubun.HeaderText = "仮釈文";
            this.dgcKariShyakubun.MinimumWidth = 100;
            this.dgcKariShyakubun.Name = "dgcKariShyakubun";
            // 
            // dgcGaihouShoshuuJyouhou
            // 
            this.dgcGaihouShoshuuJyouhou.HeaderText = "概報所収情報";
            this.dgcGaihouShoshuuJyouhou.MinimumWidth = 150;
            this.dgcGaihouShoshuuJyouhou.Name = "dgcGaihouShoshuuJyouhou";
            this.dgcGaihouShoshuuJyouhou.Width = 150;
            // 
            // dgcShashinBangouJyouhou
            // 
            this.dgcShashinBangouJyouhou.HeaderText = "写真番号情報";
            this.dgcShashinBangouJyouhou.MinimumWidth = 150;
            this.dgcShashinBangouJyouhou.Name = "dgcShashinBangouJyouhou";
            this.dgcShashinBangouJyouhou.Width = 150;
            // 
            // dgcBikou
            // 
            this.dgcBikou.HeaderText = "備考";
            this.dgcBikou.MinimumWidth = 70;
            this.dgcBikou.Name = "dgcBikou";
            this.dgcBikou.Width = 70;
            // 
            // MkaWndMokkan
            // 
            this.AutoHidePortion = 0.2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 162);
            this.CloseButtonVisible = false;
            this.Controls.Add(this.dgrMokkan);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MkaWndMokkan";
            this.Text = "木簡のリスト";
            this.SizeChanged += new System.EventHandler(this.MkaWndMokkan_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dgrMokkan)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgrMokkan;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcRBangou;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcKariShyakubun;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcGaihouShoshuuJyouhou;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcShashinBangouJyouhou;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgcBikou;
    }
}