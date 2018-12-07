using System;
using System.Collections.Generic;
using System.Text;

namespace MokkAnnotator.MkaDrawTools
{
    /// <summary>
    /// Class is responsible for executing Undo - Redo operations
    /// </summary>
    class UndoManager
    {
        #region Class Members

        private MokkanList _mokkanList;

        public MokkanList MokkanList 
        {
            get { return _mokkanList; }
            set { _mokkanList = value; } 
        }

        List<Command> historyList;
        int nextUndo;

        #endregion  Class Members

        #region Constructor

        public UndoManager(MokkanList mokkanList)
        {
            this._mokkanList = mokkanList;

            ClearHistory();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Return true if Undo operation is available
        /// </summary>
        public bool CanUndo
        {
            get
            {
                // If the NextUndo pointer is -1, no commands to undo
                if (nextUndo < 0 ||
                    nextUndo > historyList.Count - 1)   // precaution
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Last Undo
        /// </summary>
        /// <returns>return true if this is the last undo</returns>
        public bool LastUndo
        {
            get { return (nextUndo == 0); }
        }

        /// <summary>
        /// Return true if Redo operation is available
        /// </summary>
        public bool CanRedo
        {
            get
            {
                // If the NextUndo pointer points to the last item, no commands to redo
                if (nextUndo == historyList.Count - 1)
                {
                    return false;
                }

                return true;
            }
        }

        #endregion Properties

        #region Public Functions

        /// <summary>
        /// Clear History
        /// </summary>
        public void ClearHistory()
        {
            historyList = new List<Command>();
            nextUndo = -1;
        }

        /// <summary>
        /// Add new command to history.
        /// Called by client after executing some action.
        /// </summary>
        /// <param name="command"></param>
        public void AddCommandToHistory(Command command)
        {
            // Purge history list
            this.TrimHistoryList();

            // Add command and increment undo counter
            historyList.Add(command);

            nextUndo++;
        }

        /// <summary>
        /// Undo
        /// </summary>
        public void Undo()
        {
            if ( ! CanUndo )
            {
                return;
            }

            // Get the Command object to be undone
            Command command = historyList[nextUndo];

            // Execute the Command object's undo method
            command.Undo(ref _mokkanList);

            // Move the pointer up one item
            nextUndo--;
        }
               
        /// <summary>
        /// Redo
        /// </summary>
        public void Redo()
        {
            if ( ! CanRedo )
            {
                return;
            }

            // Get the Command object to redo
            int itemToRedo = nextUndo + 1;
            Command command = historyList[itemToRedo];

            // Execute the Command object
            command.Redo(ref _mokkanList);

            // Move the undo pointer down one item
            nextUndo++;
        }

        #endregion Public Functions

        #region Private Functions

        private void TrimHistoryList()
        {
            // We can redo any undone command until we execute a new 
            // command. The new command takes us off in a new direction,
            // which means we can no longer redo previously undone actions. 
            // So, we purge all undone commands from the history list.*/

            // Exit if no items in History list
            if (historyList.Count == 0)
            {
                return;
            }

            // Exit if NextUndo points to last item on the list
            if (nextUndo == historyList.Count - 1)
            {
                return;
            }

            // Purge all items below the NextUndo pointer
            for (int i = historyList.Count - 1; i > nextUndo; i--)
            {
                historyList.RemoveAt(i);
            }
        }

        #endregion
    }
}
