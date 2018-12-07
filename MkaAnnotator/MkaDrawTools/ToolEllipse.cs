using System;
using System.Windows.Forms;
using System.IO;


namespace MokkAnnotator.MkaDrawTools
{
	/// <summary>
	/// Ellipse tool
	/// </summary>
	class ToolEllipse : ToolRectangle
	{
        /// <summary>
        /// Constructor
        /// </summary>
		public ToolEllipse()
		{
            byte[] img = global::MokkAnnotator.Properties.Resources.Ellipse;
            MemoryStream stream = new MemoryStream(img);
            Cursor = new Cursor(stream);
		}

        /// <summary>
        /// Left mouse button is pressed
        /// </summary>       
        public override void OnMouseDown(MkaDocument docArea, MouseEventArgs e)
        {
            AddNewObject(docArea, new DrawEllipse(e.X, e.Y, 1, 1));
        }
	}
}
