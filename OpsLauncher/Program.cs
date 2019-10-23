using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpsLauncher
{
   class Program
   {
      private static string FileName = "";
      [STAThread]
      static void Main(string[] args)
      {
         Console.ForegroundColor = ConsoleColor.Cyan;
         Console.WriteLine();
         Console.WriteLine("  ************************************** OpsLauncher **************************************");
         Console.WriteLine("  Free Program by CIVIL SOFT SCIENCE Group: www.civilSoftScience.com (www.OmranElmAfzar.ir)");
         Console.WriteLine("  *****************************************************************************************");
         if (args.Length > 0)
         {
            FileName = args[0];
         }
         else
         {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            dlg.DefaultExt = ".tcl";
            dlg.FileOk += OnOk;
            dlg.Title = "OpsLauncher: Select a \"tcl\" file to run with OpenSees";
            dlg.ShowHelp = true;
            dlg.HelpRequest += new EventHandler(OnHelpRequest);
            dlg.ShowDialog();
         }
         if (FileName.Length == 0)
         {
            Console.WriteLine("Empty file name encountered. Press any key to exit");
            Console.ReadKey();
            return;
         }
         var workingDir = Directory.GetParent(FileName).FullName;
         var ind = FileName.LastIndexOf("\\", StringComparison.Ordinal);
         if (ind != -1)
            FileName = FileName.Remove(0, ind + 1);
         FileName = string.Format("\"{0}\\{1}\"", workingDir, FileName);
         Console.Title = FileName + " www.civilsoftscience.com (www.OmranElmAfzar.ir)";
         RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\CivilSoftScience\Settings");
         if (key == null)
         {
            Console.WriteLine("ERROR Reading registry data. You should Install OpenSees-CSS first (see www.civilsoftscience.com)");
            Console.ReadKey();
            return;
         }
         string opsPath = "";
         opsPath = (string)key.GetValue("InstallPath");

         if (opsPath == "")
         {
            Console.WriteLine("ERROR Reading registry data. You should Install OpenSees-CSS first (see www.civilsoftscience.com)");
            Console.ReadKey();
            return;
         }
         string opsname = "OpenSees.exe";

         var opsFile = $@"{opsPath}\{opsname}";
         var info = new ProcessStartInfo(opsFile, FileName)
         {
            WorkingDirectory = workingDir,
            RedirectStandardOutput = true,
            UseShellExecute = false
         };
         Process theProcess = null;
         try
         {
            theProcess = new Process { StartInfo = info };
         }
         catch
         {
            Console.WriteLine($"ERROR: The OpenSees.exe file was not found in {opsPath}");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(false);
            return;
         }
         theProcess.OutputDataReceived += NetStndrdDataHandler;
         var watch = Stopwatch.StartNew();
         var strt = DateTime.Now;
         Console.ForegroundColor = ConsoleColor.White;
         try
         {
            theProcess.Start();
         }
         catch
         {
            Console.WriteLine(@"ERROR: The OpenSees.exe file was not found in C:\Program Files\Omran Elm Afzar");
            Console.WriteLine("Press any key to continue");
            Console.ReadKey(false);
            return;
         }
         theProcess.BeginOutputReadLine();
         theProcess.WaitForExit();
         watch.Stop();
         var processTime = watch.Elapsed;
         Console.WriteLine("-------------------------------------------------------------------------------------------------------");
         Console.WriteLine("End of script {0} reached.", FileName);
         Console.ForegroundColor = ConsoleColor.Cyan;
         Console.WriteLine("Analysis started at: {0}", strt);
         Console.WriteLine("Analysis ended at: {0}", DateTime.Now);
         Console.WriteLine("Total process time = {0}", watch.Elapsed);
         Console.WriteLine("Press any key to continue");
         Console.ReadKey(false);

      }

      private static void OnOk(object sender, EventArgs e)
      {
         var dlg = (OpenFileDialog)sender;
         FileName = dlg.FileName;
      }

      private static void OnHelpRequest(object sender, System.EventArgs e)
      {

         //MessageBox.Show("OpsLauncher developed by Civil Soft Science Group.\nwebsite: www.civilsoftscience.com");
         MessageBox.Show("OpsLauncher developed by Civil-Soft-Science (Omran Elm Afzar) Group.\nwebsite:www.civilsoftscience.com (www.OmranElmAfzar.ir)");
      }

      private static void NetErrorDataHandler(object sendingProcess,
                      DataReceivedEventArgs errLine)
      {
         Console.WriteLine(errLine.Data);
      }
      private static void NetStndrdDataHandler(object sendingProcess,
                      DataReceivedEventArgs errLine)
      {
         Console.WriteLine(errLine.Data);
      }
   }
}
