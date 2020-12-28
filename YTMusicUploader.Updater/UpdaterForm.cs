using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Updater.Business.Pipes;

namespace YTMusicUploader.Updater
{
    /// <summary>
    /// Standalone application's form used to perform a YT Music Upgrade by downloaod the new version from GitHub
    /// and then installing it.
    /// </summary>
    public partial class UpdaterForm : Form
    {
        private Thread ProcessThread { get; set; }
        private string DownloadUrl { get; set; }
        private string DownloadPath { get; set; }
        private string Version { get; set; }
        private string InstallerPath { get; set; }
        private string InstalledLocation { get; set; }
        private string Platform { get; set; }


        private bool _downloadComplete = false;

        /// <summary>
        /// Standalone application's form used to perform a YT Music Upgrade by downloaod the new version from GitHub
        /// and then installing it.
        /// </summary>
        public UpdaterForm(
            string downloadUrl, 
            string downloadPath,
            string version, 
            string installedLocation)
        {
            DownloadUrl = downloadUrl;
            DownloadPath = downloadPath;
            Version = version;
            InstalledLocation = installedLocation;

            InitializeComponent();
            AssemblyHelper.PreloadAssemblies();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolve);

            ThreadStart starter = MainProcess;
            starter += () =>
            {
                Application.Exit();
            };
            ProcessThread = new Thread(starter) { IsBackground = true };
            ProcessThread.Start();
        }

        private void MainProcess()
        {
            CloseYTMusicUploader();
            WaitForYTMusicUploaderToExit();
            GetPlatform();
            DownloadNewVersion();
            InstallNewVersion();
            OpenYTMusicUploader();
        }

        private void CloseYTMusicUploader()
        {
            SetStatus($"Closing YT Music Uploader");
            var wcfClient = new WcfClient("JBS_YTMusicUploader");
            int sleepCount = 0;

            while (true)
            {
                try
                {
                    wcfClient.Send("Close");
                    break;
                }
                catch
                {
                    if (sleepCount > 7000)
                        break;

                    ThreadHelper.SafeSleep(50);
                    sleepCount += 50;

                    wcfClient = new WcfClient("JBS_YTMusicUploader");
                }
            }
        }

        private void WaitForYTMusicUploaderToExit()
        {
            ThreadHelper.SafeSleep(500);
            int sleepCounter = 0;
            try
            {
                var ytMusicUploaderProcess = Process.GetProcessesByName("YTMusicUploader");
                while (ytMusicUploaderProcess.Length > 0)
                {
                    ThreadHelper.SafeSleep(500);
                    ytMusicUploaderProcess = Process.GetProcessesByName("YTMusicUploader");
                    sleepCounter++;

                    if (sleepCounter > 20) // 10 seconds
                    {
                        foreach (var p in ytMusicUploaderProcess)
                        {
                            try
                            {
                                p.Kill();
                            }
                            catch { }
                        }
                    }
                }
            }
            catch { }
        }

        private void GetPlatform()
        {
            try
            {
                var installedApps = InstalledApplicationHelper.GetInstalledPrograms();
                Platform = installedApps.Where(a => a.DisplayName.ToLower().Contains("yt music uploader")).FirstOrDefault().Plaform.ToString().ToLower();

                if (string.IsNullOrEmpty(Platform))
                    Platform = "x64";
            }
            catch
            {
                Platform = "x64";
            }
        }

        private void DownloadNewVersion()
        {
            TaskbarHelper.SetTaskbarProgress(0, 100);
            SetStatus($"Downloading version {Version}");
            InstallerPath = Path.Combine(DownloadPath, $"YT.Music.Uploader.v{Version}.Installer-{Platform}.msi");

            if (File.Exists(InstallerPath))
            {
                if (new FileInfo(InstallerPath).Length > 70000000)
                {
                    TaskbarHelper.SetTaskbarProgress(100, 100);
                    return;
                }
                else
                    File.Delete(InstallerPath);
            }

            if (!Directory.Exists(DownloadPath))
                Directory.CreateDirectory(DownloadPath);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += DownloadProgress;
                wc.DownloadFileCompleted += DownloadComplete;
                wc.DownloadFileAsync(
                    new Uri(DownloadUrl),
                    InstallerPath
                );
            }

            while (!_downloadComplete)
                Thread.Sleep(200);
        }

        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            TaskbarHelper.SetTaskbarProgress(e.ProgressPercentage, 100);
            SetStatus($"Downloading version {Version} ({e.ProgressPercentage}%)");
        }

        private void DownloadComplete(object sender, EventArgs e)
        {
            _downloadComplete = true;
        }

        private void InstallNewVersion()
        {
            SetStatus($"Installing version: {Version}");
            var process = new Process();
            process.StartInfo.FileName = "msiexec";
            process.StartInfo.Arguments = $"/i \"{InstallerPath}\" /passive";
            process.Start();
            process.WaitForExit();
        }

        private void OpenYTMusicUploader()
        {
            var process = new Process();
            process.StartInfo.FileName = InstalledLocation;
            process.Start();
        }       

        delegate void SetStatusDelegate(string statusText);
        private void SetStatus(string statusText)
        {
            if (lblStatus.InvokeRequired)
            {
                SetStatusDelegate d = new SetStatusDelegate(SetStatus);
                Invoke(d, new object[] { statusText });
            }
            else
                lblStatus.Text = statusText;
        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return AssemblyHelper.LoadAssembly(args.Name);
        }
    }
}
