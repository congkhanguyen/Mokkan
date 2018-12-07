namespace MokkAnnotator.MkaWindow
{
    partial class MkaWndToolbar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MkaWndToolbar));
            this.lblTool = new System.Windows.Forms.Label();
            this.tsSelect = new System.Windows.Forms.ToolStrip();
            this.btnPointer = new System.Windows.Forms.ToolStripButton();
            this.btnRectangle = new System.Windows.Forms.ToolStripButton();
            this.btnEllipse = new System.Windows.Forms.ToolStripButton();
            this.btnPolygon = new System.Windows.Forms.ToolStripButton();
            this.lblFormat = new System.Windows.Forms.Label();
            this.uctrlGraphics = new MokkAnnotator.MkaWindow.MkaGraphicsProperties();
            this.tsSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTool
            // 
            this.lblTool.AutoSize = true;
            this.lblTool.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblTool.Location = new System.Drawing.Point(4, 9);
            this.lblTool.Name = "lblTool";
            this.lblTool.Size = new System.Drawing.Size(39, 14);
            this.lblTool.TabIndex = 34;
            this.lblTool.Text = "ツール";
            // 
            // tsSelect
            // 
            this.tsSelect.AutoSize = false;
            this.tsSelect.BackColor = System.Drawing.SystemColors.Control;
            this.tsSelect.CanOverflow = false;
            this.tsSelect.Dock = System.Windows.Forms.DockStyle.None;
            this.tsSelect.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsSelect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPointer,
            this.btnRectangle,
            this.btnEllipse,
            this.btnPolygon});
            this.tsSelect.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.tsSelect.Location = new System.Drawing.Point(44, 28);
            this.tsSelect.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.tsSelect.Name = "tsSelect";
            this.tsSelect.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.tsSelect.Size = new System.Drawing.Size(96, 135);
            this.tsSelect.TabIndex = 33;
            // 
            // btnPointer
            // 
            this.btnPointer.AutoSize = false;
            this.btnPointer.CheckOnClick = true;
            this.btnPointer.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.btnPointer.Image = global::MokkAnnotator.Properties.Resources.draw_cursor;
            this.btnPointer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPointer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPointer.Margin = new System.Windows.Forms.Padding(90, 5, 0, 0);
            this.btnPointer.Name = "btnPointer";
            this.btnPointer.Size = new System.Drawing.Size(90, 22);
            this.btnPointer.Text = "ポインタ";
            this.btnPointer.Click += new System.EventHandler(this.m_btnSelection_Click);
            // 
            // btnRectangle
            // 
            this.btnRectangle.AutoSize = false;
            this.btnRectangle.CheckOnClick = true;
            this.btnRectangle.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.btnRectangle.Image = global::MokkAnnotator.Properties.Resources.draw_rectangle;
            this.btnRectangle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRectangle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRectangle.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.btnRectangle.Name = "btnRectangle";
            this.btnRectangle.Size = new System.Drawing.Size(90, 22);
            this.btnRectangle.Text = "四角形";
            this.btnRectangle.Click += new System.EventHandler(this.m_btnSelection_Click);
            // 
            // btnEllipse
            // 
            this.btnEllipse.AutoSize = false;
            this.btnEllipse.CheckOnClick = true;
            this.btnEllipse.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.btnEllipse.Image = global::MokkAnnotator.Properties.Resources.draw_ellipse;
            this.btnEllipse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEllipse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEllipse.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.btnEllipse.Name = "btnEllipse";
            this.btnEllipse.Size = new System.Drawing.Size(90, 22);
            this.btnEllipse.Text = "楕円";
            this.btnEllipse.Click += new System.EventHandler(this.m_btnSelection_Click);
            // 
            // btnPolygon
            // 
            this.btnPolygon.AutoSize = false;
            this.btnPolygon.CheckOnClick = true;
            this.btnPolygon.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.btnPolygon.Image = global::MokkAnnotator.Properties.Resources.draw_polygon;
            this.btnPolygon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPolygon.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPolygon.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.btnPolygon.Name = "btnPolygon";
            this.btnPolygon.Size = new System.Drawing.Size(90, 22);
            this.btnPolygon.Text = "なげなわ";
            this.btnPolygon.Click += new System.EventHandler(this.m_btnSelection_Click);
            // 
            // lblFormat
            // 
            this.lblFormat.AutoSize = true;
            this.lblFormat.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.lblFormat.Location = new System.Drawing.Point(4, 179);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(63, 14);
            this.lblFormat.TabIndex = 30;
            this.lblFormat.Text = "書式設定";
            // 
            // uctrlGraphics
            // 
            this.uctrlGraphics.AutoSize = true;
            this.uctrlGraphics.ChangeLast = false;
            this.uctrlGraphics.Location = new System.Drawing.Point(-3, 196);
            this.uctrlGraphics.Name = "uctrlGraphics";
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
            this.uctrlGraphics.Properties = graphicsProperties1;
            this.uctrlGraphics.Size = new System.Drawing.Size(185, 189);
            this.uctrlGraphics.TabIndex = 35;
            // 
            // MkaWndToolbar
            // 
            this.AutoHidePortion = 0.15D;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 393);
            this.Controls.Add(this.uctrlGraphics);
            this.Controls.Add(this.lblTool);
            this.Controls.Add(this.tsSelect);
            this.Controls.Add(this.lblFormat);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MkaWndToolbar";
            this.Text = "注釈ツールバー";
            this.tsSelect.ResumeLayout(false);
            this.tsSelect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTool;
        private System.Windows.Forms.ToolStrip tsSelect;
        private System.Windows.Forms.ToolStripButton btnPointer;
        private System.Windows.Forms.ToolStripButton btnRectangle;
        private System.Windows.Forms.ToolStripButton btnEllipse;
        private System.Windows.Forms.ToolStripButton btnPolygon;
        private System.Windows.Forms.Label lblFormat;
        public MkaGraphicsProperties uctrlGraphics;

    }
}