using JBToolkit.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YTMusicUploader
{
    public static class Global
    {
        public static string AppDataLocation
        {
            get
            {
                return Path.Combine(DirectoryHelper.GetPath(KnownFolder.LocalAppData), @"TYUploader");
            }
        }

        public static string WorkingDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }
    }
}
