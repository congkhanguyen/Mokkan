using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MokkAnnotator.MkaDocToolkit
{
    using StringList = List<String>;
    using StringEnumerator = IEnumerator<String>;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Globalization;
    using Microsoft.Win32;
    using MokkAnnotator.MkaCommon;
    using System.IO;
    using System.Security;

    /// <summary>
    /// MRU manager - manages Most Recently Used Files list
    /// for Windows Form application.
    /// </summary>
    public class MruManager
    {
        #region Members

        // logger
        private static log4net.ILog _log = MkaDefine.Logger;

        // Event raised when user selects file from MRU list
        public event MruFileOpenEventHandler MruOpenEvent;

        private Form ownerForm;                 // owner form

        private ToolStripMenuItem menuItemMRU;          // Recent Files menu item
        private ToolStripMenuItem menuItemParent;       // Recent Files menu item parent        

        private int maxNumberOfFiles = 10;      // maximum number of files in MRU list

        private int maxDisplayLength = 40;      // maximum length of file name for display

        private string currentDirectory;        // current directory

        private StringList mruList;              // MRU list (file names)

        private String regisPath;               // register path

        private const string regEntryName = "file";  // entry name to keep MRU (file0, file1...)

        #endregion

        #region Windows API

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern bool PathCompactPathEx(
            StringBuilder pszOut,
            string pszPath,
            int cchMax,
            int reserved);

        #endregion

        #region Constructor

        public MruManager()
        {
            mruList = new StringList();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Maximum length of displayed file name in menu (default is 40).
        /// 
        /// Set this property to change default value (optional).
        /// </summary>
        public int MaxDisplayNameLength
        {
            set
            {
                maxDisplayLength = value;

                if (maxDisplayLength < 10)
                    maxDisplayLength = 10;
            }

            get
            {
                return maxDisplayLength;
            }
        }

        /// <summary>
        /// Maximum length of MRU list (default is 10).
        /// 
        /// Set this property to change default value (optional).
        /// </summary>
        public int MaxMruLength
        {
            set
            {
                maxNumberOfFiles = value;

                if (maxNumberOfFiles < 1)
                    maxNumberOfFiles = 1;

                if (mruList.Count > maxNumberOfFiles)
                    mruList.RemoveRange(maxNumberOfFiles - 1, mruList.Count - maxNumberOfFiles);
            }

            get
            {
                return maxNumberOfFiles;
            }
        }

        /// <summary>
        /// Set current directory.
        /// 
        /// Default value is program current directory which is set when
        /// Initialize function is called.
        /// 
        /// Set this property to change default value (optional)
        /// after call to Initialize.
        /// </summary>
        public string CurrentDir
        {
            set
            {
                currentDirectory = value;
            }

            get
            {
                return currentDirectory;
            }
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Initialization. Call this function in form Load handler.
        /// </summary>
        /// <param name="owner">Owner form</param>
        /// <param name="mruItem">Recent Files menu item</param>
        /// <param name="regPath">Registry Path to keep MRU list</param>
        public void Initialize(Form owner, ToolStripMenuItem mruItem, ToolStripMenuItem mruItemParent, String regPath)
        {
            // keep reference to owner form
            ownerForm = owner;

            // keep reference to MRU menu item
            menuItemMRU = mruItem;

            // keep reference to MRU menu item parent
            menuItemParent = mruItemParent;

            regisPath = regPath;

            // keep current directory in the time of initialization
            currentDirectory = Directory.GetCurrentDirectory();

            // subscribe to MRU parent Popup event
            menuItemParent.DropDownOpening += new EventHandler(this.OnMRUParentPopup);

            // subscribe to owner form Closing event
            ownerForm.Closing += OnOwnerClosing;

            // load MRU list from Registry
            LoadMRU();
        }

        /// <summary>
        /// Add file name to MRU list.
        /// Call this function when file is opened successfully.
        /// If file already exists in the list, it is moved to the first place.
        /// </summary>
        /// <param name="file">File Name</param>
        public void Add(string file)
        {
            Remove(file);

            // if array has maximum length, remove last element
            if (mruList.Count == maxNumberOfFiles)
                mruList.RemoveAt(maxNumberOfFiles - 1);

            // add new file name to the start of array
            mruList.Insert(0, file);
        }

        /// <summary>
        /// Remove file name from MRU list.
        /// Call this function when File - Open operation failed.
        /// </summary>
        /// <param name="file">File Name</param>
        public void Remove(string file)
        {
            int i = 0;

            StringEnumerator myEnumerator = mruList.GetEnumerator();

            while (myEnumerator.MoveNext())
            {
                if ((string)myEnumerator.Current == file)
                {
                    mruList.RemoveAt(i);
                    return;
                }

                i++;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Update MRU list when MRU menu item parent is opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMRUParentPopup(object sender, EventArgs e)
        {
            // remove all childs
            //if (menuItemMRU.IsParent)
            //{
            menuItemMRU.DropDownItems.Clear();
            //}

            // Disable menu item if MRU list is empty
            if (mruList.Count == 0)
            {
                menuItemMRU.Enabled = false;
                return;
            }

            // enable menu item and add child items
            menuItemMRU.Enabled = true;

            ToolStripMenuItem item;

            StringEnumerator myEnumerator = mruList.GetEnumerator();
            int i = 0;

            while (myEnumerator.MoveNext())
            {
                item = new ToolStripMenuItem();
                item.Text = GetDisplayName((string)myEnumerator.Current);
                item.Tag = i++;

                // subscribe to item's Click event
                item.Click += OnMRUClicked;

                menuItemMRU.DropDownItems.Add(item);
            }
        }

        /// <summary>
        /// MRU menu item is clicked - call owner's OpenMRUFile function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMRUClicked(object sender, EventArgs e)
        {
            string s;

            // cast sender object to MenuItem
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            if (item != null)
            {
                // Get file name from list using item index
                s = (string)mruList[(int)item.Tag];

                // Raise event to owner and pass file name.
                // Owner should handle this event and open file.
                if (s.Length > 0)
                {
                    if (MruOpenEvent != null)
                    {
                        MruOpenEvent(this, new MruFileOpenEventArgs(s));
                    }
                }
            }
        }

        /// <summary>
        /// Save MRU list in Registry when owner form is closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOwnerClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int i, n;

            try
            {
                RegistryKey key = MkaDefine.RootKey.CreateSubKey(regisPath);

                if (key != null)
                {
                    n = mruList.Count;

                    for (i = 0; i < maxNumberOfFiles; i++)
                    {
                        key.DeleteValue(regEntryName +
                            i.ToString(CultureInfo.InvariantCulture), false);
                    }

                    for (i = 0; i < n; i++)
                    {
                        key.SetValue(regEntryName +
                            i.ToString(CultureInfo.InvariantCulture), mruList[i]);
                    }
                }

            }
            catch (Exception ex) 
            {
                MessageBox.Show(ownerForm, MkaMessage.WarnSaveRegistry, Application.ProductName);
                _log.Warn(MkaMessage.WarnSaveRegistry, ex);              
            }
        }


        #endregion

        #region Private Functions

        /// <summary>
        /// Load MRU list from Registry.
        /// Called from Initialize.
        /// </summary>
        private void LoadMRU()
        {
            string sKey, s;

            try
            {
                mruList.Clear();

                RegistryKey key = MkaDefine.RootKey.OpenSubKey(regisPath);

                if (key != null)
                {
                    for (int i = 0; i < maxNumberOfFiles; i++)
                    {
                        sKey = regEntryName + i.ToString(CultureInfo.InvariantCulture);

                        s = (string)key.GetValue(sKey, "");

                        if (s.Length == 0)
                            break;

                        mruList.Add(s);
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ownerForm, MkaMessage.WarnLoadRegistry, Application.ProductName);
                _log.Warn(MkaMessage.WarnLoadRegistry, ex);          
            }          
        }
                      
        /// <summary>
        /// Get display file name from full name.
        /// </summary>
        /// <param name="fullName">Full file name</param>
        /// <returns>Short display name</returns>
        private string GetDisplayName(string fullName)
        {
            // if file is in current directory, show only file name
            FileInfo fileInfo = new FileInfo(fullName);

            if (fileInfo.DirectoryName == currentDirectory)
                return GetShortDisplayName(fileInfo.Name, maxDisplayLength);

            return GetShortDisplayName(fullName, maxDisplayLength);
        }

        /// <summary>
        /// Truncate a path to fit within a certain number of characters 
        /// by replacing path components with ellipses.
        /// 
        /// This solution is provided by CodeProject and GotDotNet C# expert
        /// Richard Deeming.
        /// 
        /// </summary>
        /// <param name="longName">Long file name</param>
        /// <param name="maxLen">Maximum length</param>
        /// <returns>Truncated file name</returns>
        private string GetShortDisplayName(string longName, int maxLen)
        {
            StringBuilder pszOut = new StringBuilder(maxLen + maxLen + 2);  // for safety

            if (PathCompactPathEx(pszOut, longName, maxLen, 0))
            {
                return pszOut.ToString();
            }
            else
            {
                return longName;
            }
        }

        #endregion

    }

    public delegate void MruFileOpenEventHandler(object sender, MruFileOpenEventArgs e);

    public class MruFileOpenEventArgs : System.EventArgs
    {
        private string fileName;

        public MruFileOpenEventArgs(string fileName)
        {
            this.fileName = fileName;
        }

        public string FileName
        {
            get
            {
                return fileName;
            }
        }
    }
}
