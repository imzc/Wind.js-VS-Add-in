using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace InstallNode
{
    class Program
    {
        static void Main(string[] args)
        {
            var path  = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\ZCXP\Wind.js Compiler for VS\";
            var startInfo = new ProcessStartInfo("msiexec", "/i node-v0.8.7-x86.msi /quiet");
            startInfo.WorkingDirectory = path;

            Process.Start(startInfo);
        }
    }
}
