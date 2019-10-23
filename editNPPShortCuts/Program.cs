using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace editNPPShortCuts
{
   class Program
   {
      static void Main(string[] args)
      {
          var opspath = args[0];
            //var opspath = @"C:\Program Files\OpenSees-CSS\OpsLauncher.exe";
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
         var fileName = $@"{path}\Notepad++\shortcuts.xml";
         //Console.WriteLine($"opspath= {opspath}, fileName= {fileName}");
         //Console.ReadKey();
         if (!File.Exists(fileName))
            return;
         var lines = File.ReadAllLines(fileName);
         var file = File.CreateText(fileName);
         foreach (var line in lines)
         {
            if (line.Contains("</UserDefinedCommands>"))
            {
               file.WriteLine($@"        <Command name=""OpsLaunch"" Ctrl=""yes"" Alt=""no"" Shift=""yes"" Key=""13"">&quot;{opspath}&quot; &quot;$(FULL_CURRENT_PATH)&quot;</Command>");
            }
            file.WriteLine(line);
         }
         file.Close();
      }
   }
}
