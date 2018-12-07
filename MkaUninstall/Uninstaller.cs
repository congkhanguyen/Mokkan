using System;
using System.Linq;
using System.Diagnostics;


namespace MkaUninstall
{
    public partial class Uninstaller 
    {
        static void Main(string[] args)
        {
            ExecuteCommand(@"C:\WINDOWS\system32\msiexec.exe /x {EABD9B36-6B0B-4288-AFEA-E215F7F21982}");
        }

        static void ExecuteCommand(string Command)
        {
            ProcessStartInfo ProcessInfo = new ProcessStartInfo("cmd.exe", "/C " + Command);
            Process Process = new Process();
            ProcessInfo.CreateNoWindow = true;
            ProcessInfo.UseShellExecute = false;
            Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process = Process.Start(ProcessInfo);            
            Process.Close();
        }        
    }
}
