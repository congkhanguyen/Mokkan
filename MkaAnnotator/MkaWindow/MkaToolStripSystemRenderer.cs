using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MokkAnnotator.MkaWindow
{
    /// <summary>
    /// System Render for disabling the line under toolstrip 
    /// </summary>
    public class MkaToolStripSystemRenderer : ToolStripSystemRenderer
    {
        public MkaToolStripSystemRenderer() { }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            //base.OnRenderToolStripBorder(e);
        }
    }
}
