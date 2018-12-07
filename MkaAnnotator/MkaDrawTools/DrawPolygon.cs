#region Using directives

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
#endregion

namespace MokkAnnotator.MkaDrawTools
{
    using PointList = List<Point>;
    using PointEnumerator = IEnumerator<Point>;
    using System.IO;
    using System.Xml.Serialization;
    using System.Xml;
    using MokkAnnotator.MkaDocToolkit.Xml;
    using MokkAnnotator.MkaCommon;

    /// <summary>
    /// Polygon graphic object
    /// </summary>
    [Xmlable("mokkan_polygon", IsUnique = false)]
    public class DrawPolygon : DrawLine
    {        
        private static Cursor handleCursor = new Cursor(new MemoryStream(global::MokkAnnotator.Properties.Resources.Polygon));

        private const string entryLength = "Length";
        private const string entryPoint = "Point";

        public DrawPolygon() : base()
        {
            Points = new PointList();
            ShapeType = 3;
            Initialize();
        }

        public DrawPolygon(int x1, int y1, int x2, int y2) : base()
        {
            Points = new PointList();

            x1 = (int)Math.Round((x1 - Origin.X) / Ratio);
            y1 = (int)Math.Round((y1 - Origin.Y) / Ratio);            
            Points.Add(new Point(x1, y1));

            x2 = (int)Math.Round((x2 - Origin.X) / Ratio);
            y2 = (int)Math.Round((y2 - Origin.Y) / Ratio); 
            Points.Add(new Point(x2, y2));
            ShapeType = 3;
            Calculate();
            Initialize();
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawPolygon drawPolygon = new DrawPolygon();

            foreach(Point point in this.Points)
            {
                drawPolygon.Points.Add(point);
            }
            drawPolygon.Properties.RShowPosition = this.Properties.RShowPosition;
            drawPolygon.Properties.RShowTop = this.Properties.RShowTop;
            drawPolygon.Properties.RShowBottom = this.Properties.RShowBottom;

            FillDrawObjectFields(drawPolygon);
            return drawPolygon;
        }

        /// <summary>
        /// Calculate top, bottom, left, right, id show location
        /// </summary>        
        public override void Calculate()
        {
            if (Points == null)
                return;

            Left = Top = int.MaxValue;
            Right = Bottom = int.MinValue;

            foreach (Point pnt in Points)
            {
                if (pnt.X < Left) Left = pnt.X;
                if (pnt.X > Right) Right = pnt.X;
                if (pnt.Y < Top) Top = pnt.Y;
                if (pnt.Y > Bottom) Bottom = pnt.Y;
            }
                        
            List<Point> minRec = new List<Point>();
            minRec.Add(new Point(Left, Top));
            minRec.Add(new Point(Right, Top));
            minRec.Add(new Point(Right, Bottom));
            minRec.Add(new Point(Left, Bottom));
            if (Points.Count >= 5)
            {
                Point centroid = GetCentroid(Points);
                double angle = GetMinimumRect(Points, centroid, ref minRec);
            }

            float x, y;
            x = (float)(minRec[0].X + minRec[1].X) / 2.0f;
            y = (float)(minRec[0].Y + minRec[1].Y) / 2.0f;
            Properties.RShowTop = new PointF(x, y);
            x = (float)(minRec[2].X + minRec[3].X) / 2.0f;
            y = (float)(minRec[2].Y + minRec[3].Y) / 2.0f;
            Properties.RShowBottom = new PointF(x, y);
        }
                
        /// <summary>
        /// Get centroid point
        /// </summary>
        private Point GetCentroid(List<Point> points)
        {
            Point centroid = new Point();
            if (points.Count == 0) return centroid;

            double meanX = 0;
            double meanY = 0;
            foreach (Point p in points)
            {
                meanX += p.X;
                meanY += p.Y;
            }
            centroid.X = (int)(meanX / points.Count);
            centroid.Y = (int)(meanY / points.Count);

            return centroid;
        }

        /// <summary>
        /// Get rotate angle to get minimum bounding rectangle
        /// </summary>
        private double GetMinimumRect(List<Point> points, Point centroid, ref List<Point> minRec)
        {
            double angle, minAngle = 0;
            int minArea = (Right - Left + 1) * (Bottom - Top + 1);
            Rectangle rect, minRect;
            rect = minRect = Rectangle.FromLTRB(Left, Top, Right, Bottom);

            for (int i = -45; i < 46; i += 2)
            {
                angle = i * Math.PI / 180.0;
                int area = GetAreaOfRectangle(points, centroid, angle, out rect);                
                if(area < minArea)
                {
                    minAngle = angle;
                    minArea = area;
                    minRect = rect; 
                }
            }

            if(minAngle != 0.0)
            {
                minRec[0] = rotatePoint(new Point(minRect.Left, minRect.Top), centroid, -minAngle);
                minRec[1] = rotatePoint(new Point(minRect.Right, minRect.Top), centroid, -minAngle);
                minRec[2] = rotatePoint(new Point(minRect.Right, minRect.Bottom), centroid, -minAngle);
                minRec[3] = rotatePoint(new Point(minRect.Left, minRect.Bottom), centroid, -minAngle);
            }            

            return minAngle;
        }

        /// <summary>
        /// Get area of bounding rectangle when rotating object by a specified angle
        /// </summary>
        private int GetAreaOfRectangle(List<Point> points, Point centroid, double angle, out Rectangle boundRec)
        {
            int left, top, right, bottom;
            left = top = int.MaxValue;
            right = bottom = int.MinValue;

            foreach(Point p in points)
            {
                Point newP = rotatePoint(p, centroid, angle);
                if (newP.X < left) left = newP.X;
                if (newP.X > right) right = newP.X;
                if (newP.Y < top) top = newP.Y;
                if (newP.Y > bottom) bottom = newP.Y;
            }

            boundRec = Rectangle.FromLTRB(left, top, right, bottom);
            return (right - left + 1) * (bottom - top + 1);
        }

        /// <summary>
        /// Rotate point around centroid by angle
        /// </summary>
        private Point rotatePoint(Point point, Point centroid, double angle)
        {
            int x = centroid.X + (int)((point.X - centroid.X) * Math.Cos(angle) - (point.Y - centroid.Y) * Math.Sin(angle));
            int y = centroid.Y + (int)((point.X - centroid.X) * Math.Sin(angle) + (point.Y - centroid.Y) * Math.Cos(angle));
            return new Point(x, y);
        }

        /// <summary>
        /// Draw polygon
        /// </summary>
        public override void Draw(Graphics g)
        {
            int firstX = 0, firstY = 0;
            int x1 = 0, y1 = 0;     // previous point
            int x2 = 0, y2 = 0;     // current point           

            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen pen = Properties.GetPen();
            SolidBrush brush = Properties.GetBrush();

            // draw polygon
            Point[] drawPoints = Points.ToArray();
            drawPoints[0].X = (int)Math.Round(drawPoints[0].X * Ratio);
            drawPoints[0].Y = (int)Math.Round(drawPoints[0].Y * Ratio);            
            x1 = drawPoints[0].X;
            y1 = drawPoints[0].Y;
            firstX = x1;
            firstY = y1;

            for (int i = 1; i < drawPoints.Length; i++)
            {
                drawPoints[i].X = (int)Math.Round(drawPoints[i].X * Ratio);
                drawPoints[i].Y = (int)Math.Round(drawPoints[i].Y * Ratio);
                x2 = drawPoints[i].X;
                y2 = drawPoints[i].Y;

                g.DrawLine(pen, x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }

            g.DrawLine(pen, x2, y2, firstX, firstY);
            g.FillPolygon(brush, drawPoints);

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
        /// Draw polygon to print
        /// </summary>
        /// <param name="g"></param>
        /// <param name="ratio"></param>
        public override void DrawToPrint(Graphics g, float ratio, Point startPoint)
        {
            int firstX = 0, firstY = 0;
            int x1 = 0, y1 = 0;     // previous point
            int x2 = 0, y2 = 0;     // current point           

            g.SmoothingMode = SmoothingMode.AntiAlias;
            Pen pen = Properties.GetPen();
            SolidBrush brush = Properties.GetBrush();

            // draw polygon
            Point[] drawPoints = Points.ToArray();
            drawPoints[0].X = (int)Math.Round(drawPoints[0].X * ratio) + startPoint.X;
            drawPoints[0].Y = (int)Math.Round(drawPoints[0].Y * ratio) + startPoint.Y;
            x1 = drawPoints[0].X;
            y1 = drawPoints[0].Y;
            firstX = x1;
            firstY = y1;

            for (int i = 1; i < drawPoints.Length; i++)
            {
                drawPoints[i].X = (int)Math.Round(drawPoints[i].X * ratio) + startPoint.X;
                drawPoints[i].Y = (int)Math.Round(drawPoints[i].Y * ratio) + startPoint.Y;
                x2 = drawPoints[i].X;
                y2 = drawPoints[i].Y;

                g.DrawLine(pen, x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }

            g.DrawLine(pen, x2, y2, firstX, firstY);
            g.FillPolygon(brush, drawPoints);

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

        public void AddPoint(Point point)
        {
            point.X = (int)Math.Round((point.X - Origin.X) / Ratio);
            point.Y = (int)Math.Round((point.Y - Origin.Y) / Ratio);

            Points.Add(point);

            Calculate();
        }

        public void LoadPoints(List<Point> points)
        {
            Point pnt = new Point();
            foreach (Point p in points)
            {                
                pnt.X = (int)Math.Round((p.X - Origin.X) / Ratio);
                pnt.Y = (int)Math.Round((p.Y - Origin.Y) / Ratio);
                Points.Add(pnt);
            }
        }

        public override int HandleCount
        {
            get
            {
                return Points.Count;
            }
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Point GetHandle(int handleNumber)
        {
            if (handleNumber < 1)
                handleNumber = 1;

            if (handleNumber > Points.Count)
                handleNumber = Points.Count;

            Point point = (Point)Points[handleNumber - 1];
            point.X = (int)Math.Round(point.X * Ratio);
            point.Y = (int)Math.Round(point.Y * Ratio);

            return point;
        }

        public override Cursor GetHandleCursor(int handleNumber)
        {
            return handleCursor;
        }

        public override void MoveHandleTo(Point point, int handleNumber)
        {
            if (handleNumber < 1)
                handleNumber = 1;

            if (handleNumber > Points.Count)
                handleNumber = Points.Count;

            point.X = (int)Math.Round((point.X - Origin.X) / Ratio);
            point.Y = (int)Math.Round((point.Y - Origin.Y) / Ratio);
            Points[handleNumber - 1] = point;

            Calculate();
            Invalidate();
        }

        public override void Move(int deltaX, int deltaY)
        {
            int n = Points.Count;
            Point point;

            deltaX = (int)Math.Round(deltaX / Ratio);
            deltaY = (int)Math.Round(deltaY / Ratio);

            for (int i = 0; i < n; i++)
            {
                point = new Point(((Point)Points[i]).X + deltaX, ((Point)Points[i]).Y + deltaY);

                Points[i] = point;
            }

            Properties.RShowTop = new PointF(Properties.RShowTop.X + deltaX, Properties.RShowTop.Y + deltaY);
            Properties.RShowBottom = new PointF(Properties.RShowBottom.X + deltaX, Properties.RShowBottom.Y + deltaY);

            Invalidate();
        }  

        /// <summary>
        /// Create graphic object used for hit test
        /// </summary>
        protected override void CreateObjects()
        {
            if (AreaPath != null)
                return;

            // Create closed path which contains all polygon vertexes
            AreaPath = new GraphicsPath();

            int x1 = 0, y1 = 0;     // previous point
            int x2, y2;             // current point

            PointEnumerator enumerator = Points.GetEnumerator();

            if (enumerator.MoveNext())
            {
                x1 = ((Point)enumerator.Current).X;
                y1 = ((Point)enumerator.Current).Y;
            }

            while (enumerator.MoveNext())
            {
                x2 = ((Point)enumerator.Current).X;
                y2 = ((Point)enumerator.Current).Y;

                AreaPath.AddLine(x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;
            }

            AreaPath.CloseFigure();

            // Create region from the path
            AreaRegion = new Region(AreaPath);
        }

        #region IXmlable Members

        /// <summary>
        /// Load object from xml file
        /// </summary>
        public override void FromXml(XmlElement xmlEle, IXmlContext cnt)
        {
            base.FromXml(xmlEle, cnt);

            char[] separators = { ',', ';', '(', ')', '{', '}' };
            String pointStr;
            Point point;
            List<Point> points = new List<Point>();
            int count;

            XmlElement shapeEle = xmlEle["shape"];
            count = Int32.Parse(shapeEle.GetAttribute("count"));
            pointStr = shapeEle.GetAttribute("coordinates");

            String[] coords = pointStr.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < count; i++)
            {
                point = new Point();
                point.X = Int32.Parse(coords[2*i]);
                point.Y = Int32.Parse(coords[2*i+1]);
                points.Add(point);                
            }
            this.LoadPoints(points);

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

            String points = "{";
            foreach (Point point in Points)
                points += "(" + point.X.ToString() + "," + point.Y.ToString() + ");";
            points = points.Substring(0, points.Length - 1) + "}";

            XmlElement shapeEle = cnt.Document.CreateElement("shape");
            XmlToolkit.SetAttribute(shapeEle, "count", Points.Count.ToString());
            XmlToolkit.SetAttribute(shapeEle, "coordinates", points);
            xmlEle.AppendChild(shapeEle);
        }

        #endregion
    }
}

