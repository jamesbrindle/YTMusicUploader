using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTMusicUploader.Helpers
{
    public class PythonHelper
    {
        public static string GetPythonPath()
        {
            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            if (environmentVariables["Path"] is string pathVariable)
            {
                string[] allPaths = pathVariable.Split(';');
                foreach (var path in allPaths)
                {
                    string pythonPathFromEnv = path + @"\python.exe";
                    if (File.Exists(pythonPathFromEnv))
                        return pythonPathFromEnv;
                }
            }

            if (File.Exists(Path.Combine(Path.GetTempPath(), @"TYUploader\Python38\python.exe")))
                return Path.Combine(Path.GetTempPath(), @"TYUploader\Python38\python.exe");

            return null;
        }
    }
}
