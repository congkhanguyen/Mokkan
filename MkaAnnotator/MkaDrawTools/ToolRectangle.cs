using System;
using System.Windows.Forms;
using System.Drawing;
using System.IO;


namespace MokkAnnotator.MkaDrawTools
{
	/// <summary>
	/// Rectangle tool
	/// </summary>
	class ToolRectangle : ToolObject
	{
        /// <summary>
        /// Constructor
        /// </summary>
		public ToolRectangle()
		{
            byte[] img = global::MokkAnnotator.Properties.Resources.Rectangle;
            MemoryStream stream = new MemoryStream(img);
            Cursor = new Cursor(stream);
		}

        /// <summary>
        /// Left mouse button is pressed
        /// </summary> 
        public override void OnMouseDown(MkaDocument docArea, MouseEventArgs e)
        {
            AddNewObject(docArea, new DrawRectangle(e.X, e.Y, 1, 1));
        }

        /// <summary>
        /// Mouse is moved, left mouse button is pressed or none button is pressed
        /// </summary>
        public override void OnMouseMove(MkaDocument docArea, MouseEventArgs e)
        {
            docArea.Cursor = Cursor;

            if ( e.Button == MouseButtons.Left )
            {
                Point point = new Point(e.X, e.Y);
                docArea.MokkanList[0].MoveHandleTo(point, 5);
                docArea.Refresh();
            }
        }
	}
}
