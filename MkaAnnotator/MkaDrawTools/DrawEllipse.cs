using System;
using System.Windows.Forms;
using System.Drawing;
using MokkAnnotator.MkaDocToolkit.Xml;
using MokkAnnotator.MkaCommon;

namespace MokkAnnotator.MkaDrawTools
{
	/// <summary>
	/// Ellipse graphic object
	/// </summary>    
    [Xmlable("mokkan_ellipse", IsUnique = false)]
    public class DrawEllipse : DrawRectangle
	{
		public DrawEllipse() : this(0, 0, 1, 1)
		{
            ShapeType = 2;
		}

        public DrawEllipse(int x, int y, int width, int height) : base()
        {
            x = (int)Math.Round((x - Origin.X) / Ratio);
            y = (int)Math.Round((y - Origin.Y) / Ratio);
            width = (int)Math.Round(width / Ratio);
            height = (int)Math.Round(height / Ratio);                       

            Rectangle = new Rectangle(x, y, width, height);
            ShapeType = 2;
            Calculate();            
            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawEllipse drawEllipse = new DrawEllipse();
            drawEllipse.Rectangle = this.Rectangle;
            drawEllipse.Properties.RShowPosition = this.Properties.RShowPosition;
            drawEllipse.Properties.RShowTop = this.Properties.RShowTop;
            drawEllipse.Properties.RShowBottom = this.Properties.RShowBottom;

            FillDrawObjectFields(drawEllipse);
            return drawEllipse;
        }

        /// <summary>
        /// Draw ellipse
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            Pen pen = Properties.GetPen();
            SolidBrush brush = Properties.GetBrush();

            // draw ellipse
            Rectangle rec = DrawRectangle.GetNormalizedRectangle(Rectangle);
            rec.X = (int)Math.Round(rec.X * Ratio);
            rec.Y = (int)Math.Round(rec.Y * Ratio);
            rec.Width = (int)Math.Round(rec.Width * Ratio);
            rec.Height = (int)Math.Round(rec.Height * Ratio);

            g.DrawEllipse(pen,rec);
            g.FillEllipse(brush, rec);

            // draw remain id
            if (RShow)
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                Font font = new Font(MkaDefine.RDefaultFontName, this.Properties.RFontSize * Ratio);
                SizeF measure = g.MeasureString(MokkanInfo.RBangou.ToString(), font); PointF showPnt;
                if (Properties.RShowPosition == ShowPosition.Top)
                    showPnt = new PointF(this.Properties.RShowLocation.X * Ratio, this.Properties.RShowLocation.Y * Ratio - measure.Height / 2.0f);
                else
                    showPnt = new PointF(this.Properties.RShowLocation.X * Ratio, this.Properties.RShowLocation.Y * Ratio + measure.Height / 2.0f);
                 g.DrawString(MokkanInfo.RBangou.ToString(), font, MkaDefine.PrintTextBrush, showPnt, sf);
            }            
            
            pen.Dispose();
            brush.Dispose();
        }

        /// <summary>
        /// Draw ellipse to print
        /// </summary>
        /// <param name="g"></param>
        /// <param name="ratio"></param>
        public override void DrawToPrint(Graphics g, float ratio, Point startPoint)
        {
            Pen pen = Properties.GetPen();
            SolidBrush brush = Properties.GetBrush();

            // draw ellipse
            Rectangle rec = DrawRectangle.GetNormalizedRectangle(Rectangle);
            rec.X = (int)Math.Round(rec.X * ratio) + startPoint.X;
            rec.Y = (int)Math.Round(rec.Y * ratio) + startPoint.Y;
            rec.Width = (int)Math.Round(rec.Width * ratio);
            rec.Height = (int)Math.Round(rec.Height * ratio);

            g.DrawEllipse(pen, rec);
            g.FillEllipse(brush, rec);

            // draw remain id
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            Font font = new Font(MkaDefine.RDefaultFontName, this.Properties.RFontSize * ratio);
            SizeF measure = g.MeasureString(MokkanInfo.RBangou.ToString(), font);
            PointF showPnt;
            if (Properties.RShowPosition == ShowPosition.Top)
                showPnt = new PointF(this.Properties.RShowLocation.X * ratio + startPoint.X, this.Properties.RShowLocation.Y * ratio + startPoint.Y - measure.Height / 2.0f);
            else
                showPnt = new PointF(this.Properties.RShowLocation.X * ratio + startPoint.X, this.Properties.RShowLocation.Y * ratio + startPoint.Y + measure.Height / 2.0f);
            g.DrawString(MokkanInfo.RBangou.ToString(), font, MkaDefine.PrintTextBrush, showPnt, sf);

            pen.Dispose();
            brush.Dispose();
        }
	}
}
