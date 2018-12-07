using System;
using System.Windows.Forms;
using System.Drawing;


namespace MokkAnnotator.MkaDrawTools
{
	/// <summary>
	/// Base class for all drawing tools
	/// </summary>
	abstract class Tool
	{ 
        /// <summary>
        /// Left mouse button is pressed
        /// </summary>     
        public virtual void OnMouseDown(MkaDocument docArea, MouseEventArgs e)
        {
        }


        /// <summary>
        /// Mouse is moved, left mouse button is pressed or none button is pressed
        /// </summary>
        public virtual void OnMouseMove(MkaDocument docArea, MouseEventArgs e)
        {
        }


        /// <summary>
        /// Left mouse button is released
        /// </summary>        
        public virtual void OnMouseUp(MkaDocument docArea, MouseEventArgs e)
        {
        }
    }
}
