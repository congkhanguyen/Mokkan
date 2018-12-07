using System;
using System.Collections.Generic;
using System.Text;

namespace MokkAnnotator.MkaDrawTools
{
    /// <summary>
    /// Changing state of existing objects:
    /// move, resize, change properties.
    /// </summary>
    class CommandChangeState : Command
    {
        // Selected object(s) before operation
        List<DrawObject> listBefore;

        // Selected object(s) after operation
        List<DrawObject> listAfter;
        

        // Create this command BEFORE operation.
        public CommandChangeState(MokkanList mokkanList)
        {
            // Keep objects state before operation.
            FillList(mokkanList, ref listBefore);
        }

        // Call this function AFTER operation.
        public void NewState(MokkanList mokkanList)
        {
            // Keep objects state after operation.
            FillList(mokkanList, ref listAfter);
        }

        public override void Undo(ref MokkanList list)
        {
            // Replace all objects in the list with objects from listBefore
            ReplaceObjects(list, listBefore);
        }

        public override void Redo(ref MokkanList list)
        {
            // Replace all objects in the list with objects from listAfter
            ReplaceObjects(list, listAfter);
        }

        // Replace objects in mokkanList with objects from list
        private void ReplaceObjects(MokkanList mokkanList, List<DrawObject> list)
        {
            for ( int i = 0; i < mokkanList.Count; i++ )
            {
                DrawObject replacement = null;

                foreach(DrawObject o in list)
                {
                    if ( o.ID == mokkanList[i].ID )
                    {
                        replacement = o;
                        replacement.Selected = true;
                        break;
                    }
                }

                if ( replacement != null )
                {
                    mokkanList.Replace(i, replacement);
                }
            }
        }

        // Fill list from selection
        private void FillList(MokkanList mokkanList, ref List<DrawObject> listToFill)
        {
            listToFill = new List<DrawObject>();
            DrawObject changedObj;
            foreach (DrawObject o in mokkanList.Selection)
            {
                changedObj = o.Clone();
                changedObj.Selected = false;
                listToFill.Add(changedObj);
            }
        }
    }
}
