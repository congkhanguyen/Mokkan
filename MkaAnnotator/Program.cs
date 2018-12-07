using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using MokkAnnotator.MkaCommon;
using System.IO;

namespace MokkAnnotator
{
    static class Program
    {
        static MkaFrmLoading loadFrm;  

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // idle start
            Application.Idle += new EventHandler(Application_Idle);

            loadFrm = new MkaFrmLoading();
            loadFrm.Show();
            Application.DoEvents();

            // register file type
            Register(); 

            // Check command line
            if (args.Length > 1)
            {
                MessageBox.Show("Incorrect number of arguments. Usage: MokkAnnotator.exe [filenames]", "MokkAnnotator");
            }

            MkaFrmAnnotator form = new MkaFrmAnnotator();
            if (args.Length == 1)
                form.ArgumentFile = args[0];

            Application.Run(form);

            // idle end
            Application.Idle -= new EventHandler(Application_Idle);
        }

        static void Application_Idle(object sender, EventArgs e)
        {
            if (loadFrm != null)
            {
                loadFrm.Close();
                loadFrm.Dispose();
                loadFrm = null;
            }
        }
        
        /// <summary>
        /// Register file type
        /// </summary>
        static void Register()
        {
            // register glass file
            RegisterFileType(MkaDefine.MkaFileExt, MkaDefine.MkaFileProgId, MkaDefine.MkaFileDescription, 1);

            // register bat file
            RegisterFileType(MkaDefine.BmkFileExt, MkaDefine.BmkFileProgId, MkaDefine.BmkFileDescription, 3);
        }

        /// <summary>
        /// Associate file type with this program in the Registry
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true - OK, false - failed</returns>
        static bool RegisterFileType(
            string extension,
            string progId,
            string description,
            int fileId)
        {
            try
            {
                String path = "Software\\Classes\\";
                // Register custom extension with the shell                
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(path + extension))
                {
                    // Map custom  extension to a ProgID
                    key.SetValue("", progId);
                }

                // create ProgID key with display name                
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(path + progId))
                {
                    key.SetValue(null, description);
                }

                // register icon                                
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(path + progId + @"\DefaultIcon"))
                {
                    key.SetValue(null, Application.ExecutablePath + "," + fileId.ToString());
                }

                // Register open command with the shell
                string cmdkey = progId + @"\shell\open\command";
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(path + cmdkey))
                {
                    // Map ProgID to an Open action for the shell
                    key.SetValue(null, Application.ExecutablePath + " \"%1\"");
                }

                // Register application for "Open With" dialog
                string appkey = "Applications\\" + new FileInfo(Application.ExecutablePath).Name + "\\shell";
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(path + appkey))
                {
                    key.SetValue("FriendlyCache", MkaDefine.ProductName);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
