using System;
using System.Windows.Forms;
using System.Drawing;
using MokkAnnotator;
using MokkAnnotator.MkaCommon;
using System.Collections.Generic;
using MokkAnnotator.MkaToolsData;

namespace MokkAnnotator.MkaDrawTools
{
	/// <summary>
	/// Base class for all tools which create new graphic object
	/// </summary>
	abstract class ToolObject : Tool
	{
        private Cursor cursor;

        /// <summary>
        /// Tool cursor.
        /// </summary>
        protected Cursor Cursor
        {
            get
            {
                return cursor;
            }
            set
            {
                cursor = value;
            }
        }


        /// <summary>
        /// Left mouse is released.
        /// New object is created and resized.
        /// </summary>
        public override void OnMouseUp(MkaDocument docArea, MouseEventArgs e)
        {
            docArea.MokkanList[0].Normalize();
            List<DrawObject> list = new List<DrawObject>();
            list.Add(docArea.MokkanList[0]);
            docArea.AddCommandToHistory(new CommandAdd(list));
            docArea.ActiveTool = ToolType.Pointer;

            docArea.Capture = false;
            docArea.Refresh();
        }

        /// <summary>
        /// Add new object to draw area.
        /// Function is called when user left-clicks draw area,
        /// and one of ToolObject-derived tools is active.
        /// </summary>
        protected void AddNewObject(MkaDocument docArea, DrawObject o)
        {
            docArea.MokkanList.UnselectAll();

            o.Selected = true;
            docArea.MokkanList.Add(o);
            docArea.MokkanList[0].MokkanInfo.RBangou = docArea.MokkanList.GetNextRBangou(docArea.MokkanList[0].MokkanInfo.RBangou, 1);
            MkaMokkanInfo.LastRBangou = docArea.MokkanList.GetNextRBangou(MkaMokkanInfo.LastRBangou + 1, 0);
            docArea.Capture = true;
            docArea.Refresh();

            docArea.SetDirty();
        }
	}
}
