using System.Diagnostics;
using System;



internal class ScriptConvertor
{

    public static void ConvertWithNode(string fileFrom,string fileTo)
    {
        var rootPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\ZCXP\Wind.js Compiler for VS\";
        var startInfo = new ProcessStartInfo("node", string.Format("node_modules\\windc\\src\\windc.js --input \"{0}\" --output \"{1}\"", fileFrom, fileTo));
        startInfo.WorkingDirectory = rootPath;
        var p = new Process();
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        p.StartInfo = startInfo;
            
        p.Start();
        p.WaitForExit();
    }
}

