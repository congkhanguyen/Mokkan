using System;
using System.Collections.Generic;
using System.Text;

namespace MokkAnnotator.MkaDrawTools
{
    /// <summary>
    /// Delete All command
    /// </summary>
    class CommandDeleteAll : Command
    {
        List<DrawObject> cloneList;

        // Create this command BEFORE applying Delete All function.
        public CommandDeleteAll(MokkanList mokkanList)
        {
            cloneList = new List<DrawObject>();

            // Make clone of the whole list.
            // Add objects in reverse order because mokkanList.Add
            // insert every object to the beginning.
            int n = mokkanList.Count;

            for ( int i = n - 1; i >= 0; i-- )
            {
                cloneList.Add(mokkanList[i].Clone());
            }
        }

        public override void Undo(ref MokkanList list)
        {
            // Add all objects from clone list to list -
            // opposite to DeleteAll
            foreach (DrawObject o in cloneList)
            {
                list.Add(o);
            }
        }

        public override void Redo(ref MokkanList list)
        {
            // Clear list - make DeleteAll again
            list.Clear();
        }
    }
}
