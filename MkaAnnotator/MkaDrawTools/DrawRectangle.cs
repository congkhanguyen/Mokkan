using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;
using MokkAnnotator.MkaDocToolkit.Xml;
using MokkAnnotator.MkaCommon;

namespace MokkAnnotator.MkaDrawTools
{
	/// <summary>
	/// Rectangle graphic object
	/// </summary>
    [Xmlable("mokkan_rectangle", IsUnique = false)]
    public class DrawRectangle : DrawObject
	{
        private Rectangle rectangle;

        private const string entryRectangle = "Rect";
        
        protected Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }
        
		public DrawRectangle() : this(0, 0, 1, 1)
		{
            ShapeType = 1;
		}
                
        public DrawRectangle(int x, int y, int width, int height) : base()
        {
            rectangle.X = (int)Math.Round((x - Origin.X) / Ratio);
            rectangle.Y = (int)Math.Round((y - Origin.Y) / Ratio);
            rectangle.Width = (int)Math.Round(width / Ratio);
            rectangle.Height = (int)Math.Round(height / Ratio);
            ShapeType = 1;
            Calculate();
            Initialize();
        }

        /// <summary>
        /// Calculate top, bottom, left, right, angle of object
        /// </summary>
        public override void Calculate()
        {
            Top = rectangle.Top;
            Bottom = rectangle.Bottom;
            Left = rectangle.Left;
            Right = rectangle.Right;

            float x, y;
            x = (float)(Left + (float)Width / 2.0);
            y = (float)Top;
            Properties.RShowTop = new PointF(x, y);
            x = (float)(Left + (float)Width / 2.0);
            y = (float)Bottom;
            Properties.RShowBottom = new PointF(x, y);            
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawRectangle drawRectangle = new DrawRectangle();
            drawRectangle.rectangle = this.rectangle;
            drawRectangle.Properties.RShowPosition = this.Properties.RShowPosition;
            drawRectangle.Properties.RShowTop = this.Properties.RShowTop;
            drawRectangle.Properties.RShowBottom = this.Properties.RShowBottom;

            FillDrawObjectFields(drawRectangle);
            return drawRectangle;
        }


        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="g"></param>
        public override void Draw(Graphics g)
        {
            Pen pen = Properties.GetPen();
            SolidBrush brush = Properties.GetBrush();

            // draw rectangle
            Rectangle rec = DrawRectangle.GetNormalizedRectangle(Rectangle);
            rec.X = (int)Math.Round(rec.X * Ratio);
            rec.Y = (int)Math.Round(rec.Y * Ratio);
            rec.Width = (int)Math.Round(rec.Width * Ratio);
            rec.Height = (int)Math.Round(rec.Height * Ratio);

            g.DrawRectangle(pen, rec);
            g.FillRectangle(brush, rec);

            // draw remain id
            if (RShow)
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                Font font = new Font(MkaDefine.RDefaultFontName, this.Properties.RFontSize * Ratio);
                SizeF measure = g.MeasureString(MokkanInfo.RBangou.ToString(), font);
                PointF showPnt;
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
        /// Draw rectangle to print
        /// </summary>
        /// <param name="g"></param>
        /// <param name="ratio"></param>
        public override void DrawToPrint(Graphics g, float ratio, Point startPoint)
        {
            Pen pen = Properties.GetPen();
            SolidBrush brush = Properties.GetBrush();

            // draw rectangle
            Rectangle rec = DrawRectangle.GetNormalizedRectangle(Rectangle);
            rec.X = (int)Math.Round(rec.X * ratio) + startPoint.X;
            rec.Y = (int)Math.Round(rec.Y * ratio) + startPoint.Y;
            rec.Width = (int)Math.Round(rec.Width * ratio);
            rec.Height = (int)Math.Round(rec.Height * ratio);

            g.DrawRectangle(pen, rec);
            g.FillRectangle(brush, rec);

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

        protected void SetRectangle(int x, int y, int width, int height)
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;

            Calculate();
        }

        /// <summary>
        /// Get number of handles
        /// </summary>
        public override int HandleCount
        {
            get
            {
                return 8;
            }
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Point GetHandle(int handleNumber)
        {
            int x, y, xCenter, yCenter;
            
            Rectangle rectangle = DrawRectangle.GetNormalizedRectangle(Rectangle);            
            rectangle.X = (int)Math.Round(rectangle.X * Ratio);
            rectangle.Y = (int)Math.Round(rectangle.Y * Ratio);
            rectangle.Width = (int)Math.Round(rectangle.Width * Ratio);
            rectangle.Height = (int)Math.Round(rectangle.Height * Ratio);

            xCenter = rectangle.X + rectangle.Width / 2;
            yCenter = rectangle.Y + rectangle.Height / 2;
            x = rectangle.X;
            y = rectangle.Y;
 
            switch ( handleNumber )
            {
                case 1:
                    x = rectangle.X;
                    y = rectangle.Y;
                    break;
                case 2:
                    x = xCenter;
                    y = rectangle.Y;
                    break;
                case 3:
                    x = rectangle.Right;
                    y = rectangle.Y;
                    break;
                case 4:
                    x = rectangle.Right;
                    y = yCenter;
                    break;
                case 5:
                    x = rectangle.Right;
                    y = rectangle.Bottom;
                    break;
                case 6:
                    x = xCenter;
                    y = rectangle.Bottom;
                    break;
                case 7:
                    x = rectangle.X;
                    y = rectangle.Bottom;
                    break;
                case 8:
                    x = rectangle.X;
                    y = yCenter;
                    break;
            }

            return new Point(x, y);

        }

        /// <summary>
        /// Hit test.
        /// Return value: -1 - no hit
        ///                0 - hit anywhere
        ///                > 1 - handle number
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override int HitTest(Point point)
        {
            point.X -= Origin.X;
            point.Y -= Origin.Y;

            if ( Selected )
            {
                for ( int i = 1; i <= HandleCount; i++ )
                {
                    if (GetHandleRectangle(i).Contains(point))
                        return i;
               }
            }

            if (PointInObject(point))
                return 0;

            return -1;
        }

        
        protected override bool PointInObject(Point point)
        {
            point.X = (int)Math.Round(point.X / Ratio);
            point.Y = (int)Math.Round(point.Y / Ratio);
            
            return rectangle.Contains(point);
        }

        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch ( handleNumber )
            {
                case 1:
                    return Cursors.SizeNWSE;
                case 2:
                    return Cursors.SizeNS;
                case 3:
                    return Cursors.SizeNESW;
                case 4:
                    return Cursors.SizeWE;
                case 5:
                    return Cursors.SizeNWSE;
                case 6:
                    return Cursors.SizeNS;
                case 7:
                    return Cursors.SizeNESW;
                case 8:
                    return Cursors.SizeWE;
                default:
                    return Cursors.Default;
            }
        }

        /// <summary>
        /// Move handle to new point (resizing)
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber"></param>
        public override void MoveHandleTo(Point point, int handleNumber)
        {
            int left = Rectangle.Left;
            int top = Rectangle.Top;
            int right = Rectangle.Right;
            int bottom = Rectangle.Bottom;

            point.X = (int)Math.Round((point.X - Origin.X) / Ratio);
            point.Y = (int)Math.Round((point.Y - Origin.Y) / Ratio);
            
            switch ( handleNumber )
            {
                case 1:
                    left = point.X;
                    top = point.Y;
                    break;
                case 2:
                    top = point.Y;
                    break;
                case 3:
                    right = point.X;
                    top = point.Y;
                    break;
                case 4:
                    right = point.X;
                    break;
                case 5:
                    right = point.X;
                    bottom = point.Y;
                    break;
                case 6:
                    bottom = point.Y;
                    break;
                case 7:
                    left = point.X;
                    bottom = point.Y;
                    break;
                case 8:
                    left = point.X;
                    break;
            }

            SetRectangle(left, top, right - left, bottom - top);
        }


        public override bool IntersectsWith(Rectangle rectangle)
        {
            rectangle.X = (int)Math.Round((rectangle.X - Origin.X) / Ratio);
            rectangle.Y = (int)Math.Round((rectangle.Y - Origin.Y) / Ratio);
            rectangle.Width = (int)Math.Round(rectangle.Width / Ratio);
            rectangle.Height = (int)Math.Round(rectangle.Height / Ratio);

            return Rectangle.IntersectsWith(rectangle);
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public override void Move(int deltaX, int deltaY)
        {
            deltaX = (int)Math.Round(deltaX / Ratio);
            deltaY = (int)Math.Round(deltaY / Ratio);
            rectangle.X += deltaX;
            rectangle.Y += deltaY;

            Properties.RShowTop = new PointF(Properties.RShowTop.X + deltaX, Properties.RShowTop.Y + deltaY);
            Properties.RShowBottom = new PointF(Properties.RShowBottom.X + deltaX, Properties.RShowBottom.Y + deltaY);
        }

        /// <summary>
        /// Normalize rectangle
        /// </summary>
        public override void Normalize()
        {
            rectangle = DrawRectangle.GetNormalizedRectangle(rectangle);
        }

        #region Helper Functions

        public static Rectangle GetNormalizedRectangle(int x1, int y1, int x2, int y2)
        {
            if (x2 < x1)
            {
                int tmp = x2;
                x2 = x1;
                x1 = tmp;
            }

            if (y2 < y1)
            {
                int tmp = y2;
                y2 = y1;
                y1 = tmp;
            }

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        public static Rectangle GetNormalizedRectangle(Point p1, Point p2)
        {
            return GetNormalizedRectangle(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static Rectangle GetNormalizedRectangle(Rectangle r)
        {
            return GetNormalizedRectangle(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
        }

        #endregion

        #region IXmlable Members

        /// <summary>
        /// Load object from xml file
        /// </summary>
        public override void FromXml(XmlElement xmlEle, IXmlContext cnt)
        {
            base.FromXml(xmlEle, cnt);

            XmlElement shapeEle = xmlEle["shape"];
            rectangle.X = Int32.Parse(shapeEle.GetAttribute("x"));
            rectangle.Y = Int32.Parse(shapeEle.GetAttribute("y"));
            rectangle.Width = Int32.Parse(shapeEle.GetAttribute("width"));
            rectangle.Height = Int32.Parse(shapeEle.GetAttribute("height"));

            PointF temp = Properties.RShowLocation;
            Calculate();
            if (Properties.RShowPosition == ShowPosition.Top)
                Properties.RShowTop = temp;
            else
                Properties.RShowBottom = temp;
        }

        /// <summary>
        /// Save object to xml file
        /// </summary> 
        public override void ToXml(XmlElement xmlEle, IXmlContext cnt)
        {
            base.ToXml(xmlEle, cnt);

            XmlElement shapeEle = cnt.Document.CreateElement("shape");
            XmlToolkit.SetAttribute(shapeEle, "x", rectangle.X.ToString());
            XmlToolkit.SetAttribute(shapeEle, "y", rectangle.Y.ToString());
            XmlToolkit.SetAttribute(shapeEle, "width", rectangle.Width.ToString());
            XmlToolkit.SetAttribute(shapeEle, "height", rectangle.Height.ToString());
            xmlEle.AppendChild(shapeEle);
        }

        #endregion
    }
}
