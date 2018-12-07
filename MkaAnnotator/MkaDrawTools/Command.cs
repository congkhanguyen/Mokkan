using System;
using System.Collections.Generic;
using System.Text;

namespace MokkAnnotator.MkaDrawTools
{
    /// <summary>
    /// Base class for commands used for Undo - Redo
    /// </summary>
    public abstract class Command
    {
        // This function is used to make Undo operation.
        // It makes action opposite to the original command.
        public abstract void Undo(ref MokkanList list);

        // This command is used to make Redo operation.
        // It makes original command again.
        public abstract void Redo(ref MokkanList list);

        // Derived classes have members which contain enough information
        // to make Undo and Redo operations for every specific command.
    }
}
