using System;
using System.Collections.Generic;
using System.Text;

namespace MokkAnnotator.MkaDrawTools
{
    /// <summary>
    /// Change object command
    /// </summary>
    class CommandChange : Command
    {
        // Selected object(s) before operation
        MokkanList listBefore;

        // Selected object(s) after operation
        MokkanList listAfter;

        // Create this command BEFORE operation.
        public CommandChange(MokkanList mokkanList)
        {
            // Keep objects state before operation.
            listBefore = mokkanList.Clone();
        }

        // Call this function AFTER operation.
        public void NewState(MokkanList mokkanList)
        {
            // Keep objects state after operation.
            listAfter = mokkanList.Clone();
        }

        public override void Undo(ref MokkanList list)
        {
            list = listBefore.Clone();
        }

        public override void Redo(ref MokkanList list)
        {
            list = listAfter.Clone();
        }
    }
}
