using System.Diagnostics;
using System;
using System.Linq;
using Jurassic;
using System.Collections.Generic;
using System.IO;

internal class ScriptConvertor
{
    protected ScriptEngine JSEngine;
    protected string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Visual Studio 2010\AddIns\WindAddinFiles";
    public ScriptConvertor()
    {

        FileToExecute = new List<string> { 
                "wind-core.js",
                "wind-builderbase.js",
                "wind-compiler.js",
                "esprima.js",
                "windc.js"
            };
        InitEngine();
    }
    protected List<string> FileToExecute
    {
        get;
        set;
    }
    protected void InitEngine()
    {
        JSEngine = new Jurassic.ScriptEngine();
        FileToExecute = FileToExecute.Select(i => filePath + "\\" + i).ToList();

        FileToExecute.ForEach(file => JSEngine.ExecuteFile(file));
    }

    public bool Convert(string fileFrom,string fileTo)
    {
        try
        {
            var source = File.ReadAllText(fileFrom);
            var result = JSEngine.CallGlobalFunction<string>("compile", source);
            File.WriteAllText(fileTo, result);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}

