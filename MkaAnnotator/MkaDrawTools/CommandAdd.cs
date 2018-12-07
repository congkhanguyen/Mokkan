using System;
using System.Collections.Generic;
using System.Text;

namespace MokkAnnotator.MkaDrawTools
{
    /// <summary>
    /// Add new object command
    /// </summary>
    class CommandAdd : Command
    {        
        List<DrawObject> cloneObjects;
        int addedCount;

        // Create this command with DrawObject instance added to the list
        public CommandAdd(List<DrawObject> addDrawObjects) : base()
        {
            cloneObjects = new List<DrawObject>();            

            // Keep copy of added objects
            foreach(DrawObject o in addDrawObjects)
                cloneObjects.Add(o.Clone());
            addedCount = cloneObjects.Count;
        }

        public override void Undo(ref MokkanList list)
        {
            list.DeleteLastAddedObjects(addedCount);
        }

        public override void Redo(ref MokkanList list)
        {
            list.UnselectAll();
            foreach(DrawObject o in cloneObjects)
                list.Add(o);
            addedCount = cloneObjects.Count;
        }
    }
}
