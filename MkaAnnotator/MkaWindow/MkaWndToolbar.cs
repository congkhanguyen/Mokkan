using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;
using MokkAnnotator;
using MokkAnnotator.MkaCommon;
using MokkAnnotator.MkaDrawTools;

namespace MokkAnnotator.MkaWindow
{
    public partial class MkaWndToolbar : DockContent
    {
        private MkaFrmAnnotator _owner;     // parent form
        private ToolType _selectAction;     // select tool type
          
        /// <summary>
        /// Constructor
        /// </summary>
        public MkaWndToolbar(MkaFrmAnnotator owner)
        {
            InitializeComponent();
            _owner = owner;
            this.uctrlGraphics.ChangeLast = true;
            tsSelect.Renderer = new MkaToolStripSystemRenderer();
        }

        public GraphicsProperties Properties
        {
            get { return uctrlGraphics.Properties; }
            set { uctrlGraphics.Properties = value; }

        }

        public void Select(ToolType type)
        {
            // Select active tool
            btnPointer.Checked = (type == ToolType.Pointer);
            btnRectangle.Checked = (type == ToolType.Rectangle);
            btnEllipse.Checked = (type == ToolType.Ellipse);
            btnPolygon.Checked = (type == ToolType.Polygon);
        }

        /// <summary>
        /// Tool type selection
        /// </summary>        
        private void m_btnSelection_Click(object sender, EventArgs e)
        {
            btnPointer.Checked = false;
            btnRectangle.Checked = false;
            btnEllipse.Checked = false;
            btnPolygon.Checked = false;

            ToolStripButton tsBtn = (ToolStripButton)sender;
            tsBtn.Checked = true;
            switch (tsBtn.Name)
            {
                case "btnCursor":
                    this._selectAction = ToolType.Pointer;
                    break;
                case "btnRectangle":
                    this._selectAction = ToolType.Rectangle;
                    break;
                case "btnEllipse":
                    this._selectAction = ToolType.Ellipse;
                    break;
                case "btnPolygon":
                    this._selectAction = ToolType.Polygon;
                    break;
                default:
                    this._selectAction = ToolType.Pointer;
                    break;
            }

            _owner.ToolbarCommandChanged(this._selectAction);
        }
    }
}
