namespace MokkAnnotator.MkaDrawTools
{
    public partial class MkaDocument
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MkaDocument));
            this.conMnStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmniSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmniClose = new System.Windows.Forms.ToolStripMenuItem();
            this.conMnStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // conMnStrip
            // 
            this.conMnStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmniSave,
            this.tsmniClose});
            this.conMnStrip.Name = "conMnStrip";
            this.conMnStrip.Size = new System.Drawing.Size(153, 70);
            // 
            // tsmniSave
            // 
            this.tsmniSave.Image = global::MokkAnnotator.Properties.Resources.file_save;
            this.tsmniSave.Name = "tsmniSave";
            this.tsmniSave.Size = new System.Drawing.Size(152, 22);
            this.tsmniSave.Text = "•Û‘¶(&S)";
            this.tsmniSave.Click += new System.EventHandler(this.tsmniSave_Click);
            // 
            // tsmniClose
            // 
            this.tsmniClose.Image = global::MokkAnnotator.Properties.Resources.file_close;
            this.tsmniClose.Name = "tsmniClose";
            this.tsmniClose.Size = new System.Drawing.Size(152, 22);
            this.tsmniClose.Text = "•Â‚¶‚é(&C)";
            this.tsmniClose.Click += new System.EventHandler(this.tsmniClose_Click);
            // 
            // MkaDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(510, 392);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MkaDocument";
            this.TabPageContextMenuStrip = this.conMnStrip;
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DocumentArea_MouseUp);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DocumentArea_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DocumentArea_MouseDown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MkaDocument_FormClosing);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DocumentArea_MouseMove);
            this.conMnStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip conMnStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmniSave;
        private System.Windows.Forms.ToolStripMenuItem tsmniClose;
    }
}
