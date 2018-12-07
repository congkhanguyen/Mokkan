using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;


namespace MokkAnnotator.MkaDrawTools
{
	/// <summary>
	/// Polygon tool
	/// </summary>
	class ToolPolygon : ToolObject
	{
        private int lastX;
        private int lastY;
        private DrawPolygon newPolygon;        
        private const int minDistance = 15 * 15;

        public ToolPolygon()
		{
            byte[] img = global::MokkAnnotator.Properties.Resources.Polygon;
            MemoryStream stream = new MemoryStream(img);
            Cursor = new Cursor(stream);
        }

        /// <summary>
        /// Left mouse button is pressed
        /// </summary>
        public override void OnMouseDown(MkaDocument docArea, MouseEventArgs e)
        {
            // Create new polygon, add it to the list
            // and keep reference to it
            newPolygon = new DrawPolygon(e.X, e.Y, e.X + 1, e.Y + 1);
            AddNewObject(docArea, newPolygon);
            lastX = e.X;
            lastY = e.Y;
        }

        /// <summary>
        /// Mouse move - resize new polygon
        /// </summary>
        public override void OnMouseMove(MkaDocument docArea, MouseEventArgs e)
        {
            docArea.Cursor = Cursor;

            if ( e.Button != MouseButtons.Left )
                return;

            if ( newPolygon == null )
                return;                 // precaution

            Point point = new Point(e.X, e.Y);
            int distance = (e.X - lastX)*(e.X - lastX) + (e.Y - lastY)*(e.Y - lastY);

            if ( distance < minDistance )
            {
                // Distance between last two points is less than minimum -
                // move last point
                newPolygon.MoveHandleTo(point, newPolygon.HandleCount);
            }
            else
            {
                // Add new point
                newPolygon.AddPoint(point);
                lastX = e.X;
                lastY = e.Y;
            }

            docArea.Refresh();
        }

        /// <summary>
        /// Left mouse button is released
        /// </summary>        
        public override void OnMouseUp(MkaDocument docArea, MouseEventArgs e)
        {
            newPolygon = null;

            base.OnMouseUp (docArea, e);
        }
	}
}
