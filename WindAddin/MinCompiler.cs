using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic;
using System.IO;

namespace WindAddin
{
    internal class MinCompiler:ScriptConvertor
    {
        public MinCompiler()
        {
            var files = new List<string> { 
                    "parse-js.js",
                    "process.js",
                    "uglify-js.js"
                };
            FileToExecute = files;
            InitEngine();
        }
    }
}
