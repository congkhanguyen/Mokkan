using System;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace MokkAnnotator.MkaDrawTools
{
	/// <summary>
	/// Line graphic object
	/// </summary>
	public class DrawLine : DrawObject
	{
        protected Point startPoint;
        protected Point endPoint;

        private const string entryStart = "Start";
        private const string entryEnd = "End";

        /// <summary>
        ///  Graphic objects for hit test
        /// </summary>
        private GraphicsPath areaPath = null;
        private Pen areaPen = null;
        private Region areaRegion = null;

        protected GraphicsPath AreaPath
        {
            get
            {
                return areaPath;
            }
            set
            {
                areaPath = value;
            }
        }

        protected Pen AreaPen
        {
            get
            {
                return areaPen;
            }
            set
            {
                areaPen = value;
            }
        }

        protected Region AreaRegion
        {
            get
            {
                return areaRegion;
            }
            set
            {
                areaRegion = value;
            }
        }

		public DrawLine() : this(0, 0, 1, 0)
		{
		}

        public DrawLine(int x1, int y1, int x2, int y2) : base()
        {
            startPoint.X = (int)Math.Round((x1 - Origin.X) / Ratio);
            startPoint.Y = (int)Math.Round((y1 - Origin.Y) / Ratio);
            endPoint.X = (int)Math.Round((x2 - Origin.X) / Ratio);
            endPoint.Y = (int)Math.Round((y2 - Origin.Y) / Ratio);

            Calculate();
            Initialize();
        }

        /// <summary>
        /// Calculate top, bottom, left, right, angle of object
        /// </summary>
        public override void Calculate()
        {
            if (startPoint.X < endPoint.X)
            {
                Left = startPoint.X;
                Right = endPoint.X;
            }
            else
            {
                Right = startPoint.X;
                Left = endPoint.X;
            }

            if (startPoint.Y < endPoint.Y)
            {
                Top = startPoint.Y;
                Bottom = endPoint.Y;
            }
            else
            {
                Bottom = startPoint.Y;
                Top = endPoint.Y;
            }

            // get distance
            Distance = 0;
        }

        /// <summary>
        /// Clone this instance
        /// </summary>
        public override DrawObject Clone()
        {
            DrawLine drawLine = new DrawLine();
            drawLine.startPoint = this.startPoint;
            drawLine.endPoint = this.endPoint;
            drawLine.Properties.RShowPosition = this.Properties.RShowPosition;
            drawLine.Properties.RShowTop = this.Properties.RShowTop;
            drawLine.Properties.RShowBottom = this.Properties.RShowBottom;

            FillDrawObjectFields(drawLine);
            return drawLine;
        }


        public override void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Pen pen = Properties.GetPen();

            int x1 = (int)Math.Round(startPoint.X * Ratio);
            int y1 = (int)Math.Round(startPoint.Y * Ratio);
            int x2 = (int)Math.Round(endPoint.X * Ratio);
            int y2 = (int)Math.Round(endPoint.Y * Ratio);
            g.DrawLine(pen, x1, y1, x2, y2);

            pen.Dispose();
        }

        public override int HandleCount
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Point GetHandle(int handleNumber)
        {
            Point point = new Point();
            if (handleNumber == 1)
            {
                point.X = (int)Math.Round(startPoint.X * Ratio);
                point.Y = (int)Math.Round(startPoint.Y * Ratio);
            }
            else
            {
                point.X = (int)Math.Round(endPoint.X * Ratio);
                point.Y = (int)Math.Round(endPoint.Y * Ratio);
            }

            return point;           
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
                    if ( GetHandleRectangle(i).Contains(point) )
                        return i;
                }
            }

            if ( PointInObject(point) )
                return 0;

            return -1;
        }

        protected override bool PointInObject(Point point)
        {
            CreateObjects();

            point.X = (int)Math.Round(point.X / Ratio);
            point.Y = (int)Math.Round(point.Y / Ratio);

            return AreaRegion.IsVisible(point);
        }

        public override bool IntersectsWith(Rectangle rectangle)
        {
            CreateObjects();

            rectangle.X = (int)Math.Round((rectangle.X - Origin.X) / Ratio);
            rectangle.Y = (int)Math.Round((rectangle.Y - Origin.Y) / Ratio);
            rectangle.Width = (int)Math.Round(rectangle.Width / Ratio);
            rectangle.Height = (int)Math.Round(rectangle.Height / Ratio);

            return AreaRegion.IsVisible(rectangle);
        }

        public override Cursor GetHandleCursor(int handleNumber)
        {
            switch ( handleNumber )
            {
                case 1:
                case 2:
                    return Cursors.SizeAll;
                default:
                    return Cursors.Default;
            }
        }

        public override void MoveHandleTo(Point point, int handleNumber)
        {
            point.X = (int)Math.Round((point.X - Origin.X) / Ratio);
            point.Y = (int)Math.Round((point.Y - Origin.Y) / Ratio);

            if ( handleNumber == 1 )
                startPoint = point;
            else
                endPoint = point;

            Calculate();
            Invalidate();
        }

        public override void Move(int deltaX, int deltaY)
        {
            deltaX = (int)Math.Round(deltaX / Ratio);
            deltaY = (int)Math.Round(deltaY / Ratio);

            startPoint.X += deltaX;
            startPoint.Y += deltaY;

            endPoint.X += deltaX;
            endPoint.Y += deltaY;

            Calculate();
            Invalidate();
        }

        /// <summary>
        /// Invalidate object.
        /// When object is invalidated, path used for hit test
        /// is released and should be created again.
        /// </summary>
        protected void Invalidate()
        {
            if ( AreaPath != null )
            {
                AreaPath.Dispose();
                AreaPath = null;
            }

            if ( AreaPen != null )
            {
                AreaPen.Dispose();
                AreaPen = null;
            }

            if ( AreaRegion != null )
            {
                AreaRegion.Dispose();
                AreaRegion = null;
            }
        }

        /// <summary>
        /// Create graphic objects used from hit test.
        /// </summary>
        protected virtual void CreateObjects()
        {
            if ( AreaPath != null )
                return;

            // Create path which contains wide line
            // for easy mouse selection
            AreaPath = new GraphicsPath();
            AreaPen = new Pen(Color.Black, 7);
            
            AreaPath.AddLine(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y);          
            
            AreaPath.Widen(AreaPen);

            // Create region from the path
            AreaRegion = new Region(AreaPath);
        }      
	}
}
