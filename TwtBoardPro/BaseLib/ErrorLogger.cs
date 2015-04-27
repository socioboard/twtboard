using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Globussoft;

namespace BaseLib
{
    public class ErrorLogger
    {
        public static string path_ErrorTextFile = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\twtboardpro\\" + "Error.txt";

        public static void AddToErrorLogText(string message)
        {
            try
            {
                GlobusFileHelper.AppendStringToTextfileNewLine(message, path_ErrorTextFile);
            }
            catch { }
        }
    }
}
