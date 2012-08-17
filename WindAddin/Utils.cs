using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;



public static class Utils
{
    public static string FileName(this ProjectItem item)
    {
        try
        {
            //regular project
            return item.get_FileNames(1);
        }
        catch (Exception)
        {
            //VS.Php
            return item.get_FileNames(0);
        }
    }
}

