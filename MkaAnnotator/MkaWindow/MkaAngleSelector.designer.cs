namespace AngleAltitudeControls
{
    partial class MkaAngleSelector
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
            this.SuspendLayout();
            // 
            // MkaAngleSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "MkaAngleSelector";
            this.Size = new System.Drawing.Size(191, 164);
            this.Load += new System.EventHandler(this.AngleSelector_Load);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AngleSelector_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AngleSelector_MouseDown);
            this.SizeChanged += new System.EventHandler(this.AngleSelector_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
