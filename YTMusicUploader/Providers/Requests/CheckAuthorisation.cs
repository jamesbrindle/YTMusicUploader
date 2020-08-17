using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YTMusicUploader.Providers
{
    public partial class Requests
    {
        public static bool IsAuthenticated()
        {
            StringBuilder outputStringBuilder = new StringBuilder();
            Process process = new Process();
            string error = string.Empty;
            bool isError = false;

            try
            {
                process = new Process();

                process.StartInfo.FileName = PythonPath;
                process.StartInfo.WorkingDirectory = RequestsLocation;
                process.StartInfo.Arguments = "GetIsAuthenticated.py";

                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.EnableRaisingEvents = false;
                process.OutputDataReceived += Process_OutputDataReceived;
                process.ErrorDataReceived += Process_ErrorDataReceived;
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                var processExited = process.WaitForExit(30000);

                if (processExited == false) // we timed out...
                {
                    process.Kill();
                    throw new Exception("ERROR: ytmusicapi Process took too long to finish");
                }
                else if (process.ExitCode != 0)
                {
                    var output = outputStringBuilder.ToString();

                    throw new Exception("ytmusicapi process exited with non-zero exit code of: " + process.ExitCode + Environment.NewLine +
                    "Output from process: " + outputStringBuilder.ToString());
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                process.Close();

                try
                {
                    process.Kill();
                }
                catch { }

                if (!string.IsNullOrEmpty(error))
                    isError = true;
            }

#if DEBUG
            Console.Out.WriteLine(outputStringBuilder.ToString());
#endif
            if (isError)
                return false;

            return true;

            void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
            {
                outputStringBuilder.Append(e.Data);
            }

            void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
            {
                if (!string.IsNullOrWhiteSpace(e.Data))
                    error = "ytmusicapi Error: " + e.Data;
            }
        }
    }
}
