using Ionic.Zip;
using IronPython.Hosting;
using JBToolkit.SafeStream;
using Microsoft.Scripting.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using YTMusicUploader.Helpers;
using YTMusicUploader.Properties;

namespace YTMusicUploader.Providers
{
    public partial class Requests
    {
        public static string PythonPath = null;
        public static ScriptEngine PythonEngine = Python.CreateEngine();

        public static string ApiLocation
        {
            get
            {
                return Path.Combine(Global.AppDataLocation, @"ytmusicapi");
            }
        }

        public static string RequestsLocation
        {
            get
            {
                return Path.Combine(Global.AppDataLocation, @"ytmusicapi\requests");
            }
        }

        public static string AuthHeaderLocation
        {
            get
            {
                return Path.Combine(Global.AppDataLocation, @"ytmusicapi\requests\headers_auth.json");
            }
        }

        public static void UpdateAuthHeader(string authCoookieValue)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(SafeFileStream.ReadAllText(AuthHeaderLocation));
            jsonObj.Cookie = authCoookieValue;

            File.WriteAllText(AuthHeaderLocation, JsonConvert.SerializeObject(jsonObj, Formatting.Indented));
        }

        public static bool CheckAndCopyApiFiles(MainForm mainForm)
        {
            if (!Directory.Exists(ApiLocation))
                Directory.CreateDirectory(ApiLocation);

            var zip = ZipFile.Read(Path.Combine(Global.WorkingDirectory, @"AppData\ytmusicapi.zip"));
            zip.ExtractAll(ApiLocation, ExtractExistingFileAction.OverwriteSilently);

            PythonPath = PythonHelper.GetPythonPath();

            if (string.IsNullOrEmpty(PythonPath))
            {
                mainForm.SetStatusMessage("Installing Python");

                zip = ZipFile.Read(Path.Combine(Global.WorkingDirectory, @"AppData\Python38.zip"));
                zip.ExtractAll(Path.Combine(Global.AppDataLocation, "Python38"), ExtractExistingFileAction.OverwriteSilently);

                var name = "PATH";
                var scope = EnvironmentVariableTarget.User; 
                var oldValue = Environment.GetEnvironmentVariable(name, scope);                
                var newValue = oldValue + @";" + Path.Combine(ApiLocation, "Python38") + @"\";
                Environment.SetEnvironmentVariable(name, newValue, scope);

                PythonPath = Path.Combine(Global.AppDataLocation, @"Python38\python.exe");

                mainForm.SetStatusMessage("Not running");
            }

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = PythonPath,
                    Arguments = Path.Combine(ApiLocation, "setup.py install"),
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process.Start(startInfo);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
