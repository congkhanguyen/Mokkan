using System;
using System.Windows.Forms;
using System.Drawing;


namespace MokkAnnotator.MkaDrawTools
{
	/// <summary>
	/// Pointer tool
	/// </summary>
	class ToolPointer : Tool
	{
        private enum SelectionMode
        {
            None,
            NetSelection,   // group selection is active
            Move,           // object(s) are moves
            Size            // object is resized
        }

        private SelectionMode selectMode = SelectionMode.None;

        // Object which is currently resized:
        private DrawObject resizedObject;
        private int resizedObjectHandle;

        // Keep state about last and current point (used to move and resize objects)
        private Point lastPoint = new Point(0,0);
        private Point startPoint = new Point(0, 0);

        private CommandChangeState commandChangeState;
        bool wasMove;

		public ToolPointer()
		{
		}

        /// <summary>
        /// Left mouse button is pressed
        /// </summary>
        public override void OnMouseDown(MkaDocument docArea, MouseEventArgs e)
        {
            commandChangeState = null;
            wasMove = false;

            selectMode = SelectionMode.None;
            Point point = new Point(e.X, e.Y);

            // Test for resizing (only if control is selected, cursor is on the handle)
            foreach (DrawObject o in docArea.MokkanList.Selection)
            {
                int handleNumber = o.HitTest(point);

                if (handleNumber > 0)
                {
                    selectMode = SelectionMode.Size;

                    // keep resized object in class member
                    resizedObject = o;
                    resizedObjectHandle = handleNumber;

                    // Since we want to resize only one object, unselect all other objects
                    docArea.MokkanList.UnselectAll();
                    o.Selected = true;

                    commandChangeState = new CommandChangeState(docArea.MokkanList);

                    break;
                }
            }

            // Test for move (cursor is on the object)
            if ( selectMode == SelectionMode.None )
            {
                int n1 = docArea.MokkanList.Count;
                DrawObject o = null;

                for ( int i = 0; i < n1; i++ )
                {
                    if ( docArea.MokkanList[i].HitTest(point) == 0 )
                    {
                        o = docArea.MokkanList[i];
                        break;
                    }
                }

                if ( o != null )
                {
                    selectMode = SelectionMode.Move;

                    // Unselect all if Ctrl is not pressed and clicked object is not selected yet
                    if ( ( Control.ModifierKeys & Keys.Control ) == 0  && !o.Selected )
                        docArea.MokkanList.UnselectAll();

                    // Select clicked object
                    o.Selected = true;

                    commandChangeState = new CommandChangeState(docArea.MokkanList);

                    docArea.Cursor = Cursors.SizeAll;
                }
            }

            // Net selection
            if ( selectMode == SelectionMode.None )
            {
                // click on background
                if ( ( Control.ModifierKeys & Keys.Control ) == 0 )
                    docArea.MokkanList.UnselectAll();

                selectMode = SelectionMode.NetSelection;

            }

            lastPoint.X = e.X;
            lastPoint.Y = e.Y;
            startPoint.X = e.X;
            startPoint.Y = e.Y;

            docArea.Capture = true;

            docArea.Refresh();

            if ( selectMode == SelectionMode.NetSelection )
            {
                // Draw selection rectangle in initial position
                ControlPaint.DrawReversibleFrame(
                    docArea.RectangleToScreen(DrawRectangle.GetNormalizedRectangle(startPoint, lastPoint)),
                    Color.Black,
                    FrameStyle.Dashed);
            }
        }


        /// <summary>
        /// Mouse is moved.
        /// None button is pressed, or left button is pressed.
        /// </summary>
        public override void OnMouseMove(MkaDocument docArea, MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            Point oldPoint = lastPoint;
                        
            wasMove = true;

            // set cursor when mouse button is not pressed
            if (e.Button == MouseButtons.None)
            {
                Cursor cursor = null;

                for (int i = 0; i < docArea.MokkanList.Count; i++)
                {
                    int n = docArea.MokkanList[i].HitTest(point);

                    if (n > 0)
                    {
                        cursor = docArea.MokkanList[i].GetHandleCursor(n);
                        break;
                    }
                }

                if (cursor == null)
                    cursor = Cursors.Default;

                docArea.Cursor = cursor;

                return;
            }

            if (e.Button != MouseButtons.Left)
                return;

            /// Left button is pressed

            // Find difference between previous and current position
            int dx = e.X - lastPoint.X;
            int dy = e.Y - lastPoint.Y;

            lastPoint.X = e.X;
            lastPoint.Y = e.Y;

            // resize
            if (selectMode == SelectionMode.Size)
            {
                if (resizedObject != null)
                {
                    resizedObject.MoveHandleTo(point, resizedObjectHandle);
                    docArea.SetDirty();
                    docArea.Refresh();
                }
            }

            // move
            if (selectMode == SelectionMode.Move)
            {
                foreach (DrawObject o in docArea.MokkanList.Selection)
                {
                    o.Move(dx, dy);
                }

                docArea.Cursor = Cursors.SizeAll;
                docArea.SetDirty();
                docArea.Refresh();
            }

            if (selectMode == SelectionMode.NetSelection)
            {
                // Remove old selection rectangle
                ControlPaint.DrawReversibleFrame(
                    docArea.RectangleToScreen(DrawRectangle.GetNormalizedRectangle(startPoint, oldPoint)),
                    Color.Black,
                    FrameStyle.Dashed);

                // Draw new selection rectangle
                ControlPaint.DrawReversibleFrame(
                    docArea.RectangleToScreen(DrawRectangle.GetNormalizedRectangle(startPoint, point)),
                    Color.Black,
                    FrameStyle.Dashed);

                return;
            }           

        }

        /// <summary>
        /// Right mouse button is released
        /// </summary>
        public override void OnMouseUp(MkaDocument docArea, MouseEventArgs e)
        {
            if ( selectMode == SelectionMode.NetSelection )
            {
                // Remove old selection rectangle
                ControlPaint.DrawReversibleFrame(
                    docArea.RectangleToScreen(DrawRectangle.GetNormalizedRectangle(startPoint, lastPoint)),
                    Color.Black,
                    FrameStyle.Dashed);

                // Make group selection
                docArea.MokkanList.SelectInRectangle(
                    DrawRectangle.GetNormalizedRectangle(startPoint, lastPoint));

                selectMode = SelectionMode.None;
            }

            if ( resizedObject != null )
            {
                // after resizing
                resizedObject.Normalize();
                resizedObject = null;
            }

            docArea.Capture = false;
            docArea.Refresh();

            if ( commandChangeState != null  && wasMove )
            {
                // Keep state after moving/resizing and add command to history
                commandChangeState.NewState(docArea.MokkanList);
                docArea.AddCommandToHistory(commandChangeState);
                commandChangeState = null;
            }
        }
	}
}
