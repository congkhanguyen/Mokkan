using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using MokkAnnotator.MkaToolsData;
using System.Xml.Serialization;
using System.Xml.Schema;
using MokkAnnotator.MkaDocToolkit.Xml;
using System.Xml;
using MokkAnnotator.MkaCommon;

namespace MokkAnnotator.MkaDrawTools
{
    using PointList = List<Point>;

    /// <summary>
    /// Base class for all draw objects
    /// </summary>  
    [Xmlable("mokkan", IsUnique = true)]
    public abstract class DrawObject : IXmlable
    {
        #region Members

        // mokkan information
        private MkaMokkanInfo _mkaInfo;     

        // selected flag
        private bool _selected;

        // active flag
        private bool _active;

        // id index used in undo-redo function
        int _id;

        // ratio of drawing object
        public static float Ratio = 1.0f;

        // remainder id show flag        
        public static bool RShow = false;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public DrawObject()
        {
            _id = this.GetHashCode();
                        
            _mkaInfo = new MkaMokkanInfo();
            _mkaInfo.Properties = GraphicsProperties.LastProperties.Clone();
                       
            _selected = false;
        }

        #region Properties

        public int ShapeType;
        public PointList Points;

        /// <summary>
        /// Mokkan Information
        /// </summary>
        public MkaMokkanInfo MokkanInfo
        {
            get { return this._mkaInfo; }
            set { this._mkaInfo = value; }
        }

        /// <summary>
        /// Graphical properties
        /// </summary>
        public GraphicsProperties Properties
        {
            get { return _mkaInfo.Properties; }
            set { _mkaInfo.Properties = value; }
        }

        /// <summary>
        /// Selection flag
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        /// <summary>
        /// Selection flag
        /// </summary>
        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        /// <summary>
        /// Number of handles
        /// </summary>        
        public virtual int HandleCount
        {
            get { return 0; }
        }

        /// <summary>
        /// Object ID
        /// </summary>
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Top of object
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Bottom of object
        /// </summary>
        public int Bottom { get; set; }

        /// <summary>
        /// Left of object
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Right of object
        /// </summary>
        public int Right { get; set; }

        public int Width { get { return Right - Left + 1; } }
        public int Height { get { return Bottom - Top + 1; } }

        /// <summary>
        /// Show R distance
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Drawing start point
        /// </summary>
        public static Point Origin { get; set;}

        #endregion

        #region Virtual Functions

        /// <summary>
        /// Clone this instance.
        /// </summary>
        public abstract DrawObject Clone();

        /// <summary>
        /// Draw object
        /// </summary>
        /// <param name="g"></param>
        public virtual void Draw(Graphics g)
        {                       
        }

        /// <summary>
        /// Calculate top, bottom, left, right, angle of object
        /// </summary>
        public virtual void Calculate()
        {
        }

        /// <summary>
        /// Draw object to print
        /// </summary>
        /// <param name="g"></param>
        /// <param name="ratio"></param>
        public virtual void DrawToPrint(Graphics g, float ratio, Point startPoint)
        {
        }

        /// <summary>
        /// Zoom in/out object
        /// </summary>
        public virtual void Zoom()
        { 
        }
        
        /// <summary>
        /// Get handle point by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual Point GetHandle(int handleNumber)
        {
            return new Point(0, 0);
        }

        /// <summary>
        /// Get handle rectangle by 1-based number
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual Rectangle GetHandleRectangle(int handleNumber)
        {
            Point point = GetHandle(handleNumber);

            return new Rectangle(point.X - 3, point.Y - 3, 7, 7);
        }

        /// <summary>
        /// Draw tracker for selected object
        /// </summary>
        /// <param name="g"></param>
        public virtual void DrawTracker(Graphics g)
        {
            if (!Selected)
                return;

            SolidBrush brush = new SolidBrush(Color.Black);
            Rectangle rec;

            for (int i = 1; i <= HandleCount; i++)
            {
                rec = GetHandleRectangle(i);                
                g.FillRectangle(brush, rec);
            }

            brush.Dispose();
        }

        /// <summary>
        /// Hit test.
        /// Return value: -1 - no hit
        ///                0 - hit anywhere
        ///                > 1 - handle number
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual int HitTest(Point point)
        {
            return -1;
        }


        /// <summary>
        /// Test whether point is inside of the object
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected virtual bool PointInObject(Point point)
        {
            return false;
        }


        /// <summary>
        /// Get cursor for the handle
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public virtual Cursor GetHandleCursor(int handleNumber)
        {
            return Cursors.Default;
        }

        /// <summary>
        /// Test whether object intersects with rectangle
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public virtual bool IntersectsWith(Rectangle rectangle)
        {
            return false;
        }

        /// <summary>
        /// Move object
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public virtual void Move(int deltaX, int deltaY)
        {
        }

        /// <summary>
        /// Move handle to the point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleNumber"></param>
        public virtual void MoveHandleTo(Point point, int handleNumber)
        {
        }

        /// <summary>
        /// Normalize object.
        /// Call this function in the end of object resizing.
        /// </summary>
        public virtual void Normalize()
        {
        }

        #endregion

        #region Other functions

        /// <summary>
        /// Initialization
        /// </summary>
        protected void Initialize()
        {
            _mkaInfo.Properties = GraphicsProperties.LastProperties.Clone();
        }

        /// <summary>
        /// Copy fields from this instance to cloned instance drawObject.
        /// Called from Clone functions of derived classes.
        /// </summary>
        protected void FillDrawObjectFields(DrawObject drawObject)
        {            
            drawObject._mkaInfo = this._mkaInfo.Clone();
            drawObject._selected = this._selected;
            drawObject.Properties = this.Properties.Clone();
            drawObject.ID = this.ID;
        }

        #endregion

        #region IXmlable Members

        public virtual void FromXml(XmlElement xmlEle, IXmlContext cnt)
        {
            XmlElement _infoEle = xmlEle["info"];
            if (null != _infoEle)            
                _mkaInfo = cnt.FromXml(_infoEle as XmlElement) as MkaMokkanInfo;
            
            XmlElement _formatEle = xmlEle["format"];
            if (null != _formatEle)
                _mkaInfo.Properties = cnt.FromXml(_formatEle as XmlElement) as GraphicsProperties;

        }

        public virtual void ToXml(XmlElement xmlEle, IXmlContext cnt)
        {
            XmlElement _infoEle = cnt.ToXml(_mkaInfo);
            XmlElement _formatEle = cnt.ToXml(_mkaInfo.Properties);
            xmlEle.AppendChild(_infoEle);
            xmlEle.AppendChild(_formatEle);
        }

        #endregion
    }
}

