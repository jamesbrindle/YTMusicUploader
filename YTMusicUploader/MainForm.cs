using JBToolkit.Assemblies;
using JBToolkit.Culture;
using JBToolkit.Network;
using JBToolkit.Threads;
using JBToolkit.WinForms;
using MetroFramework;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YTMusicUploader.AssemblyHelper;
using YTMusicUploader.Business;
using YTMusicUploader.Dialogues;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.DataModels;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader
{
    public partial class MainForm : OptimisedMetroForm
    {
        public bool Paused { get; set; } = false;

        public enum ManagingYTMusicStatusEnum
        {
            Showing,
            CloseNoChange,
            CloseChanges,
            CloseChangesComplete,
            NeverShown
        }

        //
        // Dialogues
        //
        public ConnectToYTMusic ConnectToYTMusicForm { get; set; }
        public IssueLog IssueLogForm { get; set; } = null;
        public UploadLog UploadLogForm { get; set; } = null;
        public ApplicationLog GeneralLogForm { get; set; } = null;
        public ManageYTMusic ManageYTMusic { get; set; } = null;

        //
        // Repos
        //
        public SettingsRepo SettingsRepo { get; set; } = new SettingsRepo();
        public WatchFolderRepo WatchFolderRepo { get; set; } = new WatchFolderRepo();
        public MusicFileRepo MusicFileRepo { get; set; } = new MusicFileRepo();
        public PlaylistFileRepo PlaylistFileRepo { get; set; } = new PlaylistFileRepo();

        //
        // Repo Models
        //
        public Settings Settings { get; set; }
        public List<WatchFolder> WatchFolders { get; set; }

        //
        // Business Classes
        //
        public FileScanner FileScanner { get; set; }
        public FileUploader FileUploader { get; set; }
        public PlaylistProcessor PlaylistProcessor { get; set; }
        public QueueChecker QueueChecker { get; set; }
        public IdleProcessor IdleProcessor { get; set; }

        //
        // Checkers
        //
        public bool ConnectedToYTMusic { get; set; } = false;
        public System.Windows.Forms.Timer ThrottleTextChangedTimer { get; set; }
        public int InitialFilesCount { get; set; } = 0;
        private List<FileSystemWatcher> FileSystemFolderWatchers { get; set; } = new List<FileSystemWatcher>();
        private DateTime? LastFolderChangeTime { get; set; }
        public bool InstallingEdge { get; set; }
        public bool Aborting { get; set; } = false;
        public ManagingYTMusicStatusEnum ManagingYTMusicStatus { get; set; } = ManagingYTMusicStatusEnum.NeverShown;
        public bool DatabaseIntegrityCheckDone { get; set; } = false;

        private bool StartHidden = false;

        //
        // MusicBrainz Access
        //
        public MusicDataFetcher MusicDataFetcher { get; set; }
        public Image ArtworkImage { get; set; }

        //
        // Threads
        //
        private Thread _installingEdgeThread;
        private Thread _connectToYouTubeMusicThread;
        private Thread _scanAndUploadThread;
        private Thread _restartThread;

        //
        // Version
        //
        private string LatestVersionTag { get; set; } = null;
        private string LatestVersionUrl { get; set; } = null;

        //
        // Updater
        //
        private string UpdaterPath { get; set; } = null;

        public MainForm(bool hidden) : base(formResizable: false)
        {
#if RELEASE
            CheckForIllegalCrossThreadCalls = false;
#endif
            CultureHelper.GloballySetCultureToGB();

            MainFormInstance = this;
            InitializeComponent();

            if (hidden)
            {
                ShowInTaskbar = false;
                CenterForm();
                WindowState = FormWindowState.Minimized;
                Opacity = 0;
                StartHidden = true;
            }

            MusicDataFetcher = new MusicDataFetcher();
            SetVersion("v" + Global.ApplicationVersion);

            lblIssues.GotFocus += LinkLabel_GotFocus;
            lblDiscoveredFiles.GotFocus += LinkLabel_GotFocus;

            if (!EdgeDependencyChecker.CheckEdgeCoreFilesArePresentAndCorrect())
            {
                btnConnectToYoutube.Enabled = false;
                InstallEdge();
            }
            else
                ConnectToYTMusicForm = new ConnectToYTMusic(this);

            FileScanner = new FileScanner(this);
            FileUploader = new FileUploader(this);
            PlaylistProcessor = new PlaylistProcessor(this);
            IdleProcessor = new IdleProcessor(this);
            QueueChecker = new QueueChecker(this);

            InitialiseTimers();
            InitialiseTooltips();
            InitialiseSystemTrayIconMenuButtons();
            ConnectToYouTubeMusic();
            StartMainProcess();

            //  Restart everything after 24 hours (is the application is continually run)
            _restartThread = new Thread((ThreadStart)delegate
            {
                while (Settings == null)
                    ThreadHelper.SafeSleep(500);

                while (true)
                {
                    if (Settings.UploadPlaylists)
                    {
                        if (!Settings.LastPlaylistUpload.HasValue)
                            Settings.LastPlaylistUpload = DateTime.Now.AddHours(Global.SessionRestartHours * -1).AddHours(-2);

                        if (!_scanAndUploadThread.IsAlive && DateTime.Now > ((DateTime)Settings.LastPlaylistUpload).AddHours(Global.SessionRestartHours))
                            StartMainProcess();
                    }

                    ThreadHelper.SafeSleep(15000); 
                }
            })
            { IsBackground = true };
            _restartThread.Start();
        }

        private void RunDebugCommands()
        {
            // Debugging code here
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (StartHidden)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    Hide();
                }));
            }

            OnLoad(e);
            ResumeDrawing(this);
        }

        private void InitialiseTimers()
        {
            ThrottleTextChangedTimer = new System.Windows.Forms.Timer
            {
                Interval = 2000,
                Enabled = true

            };
            ThrottleTextChangedTimer.Tick += ThrottleTextChangedTimer_Elapsed;
        }

        private void InitialiseSystemTrayIconMenuButtons()
        {
            tsmShow.Click += new EventHandler(TsmShow_Click);
            tsmQuit.Click += new EventHandler(TsmQuit_Click);
            tsmPauseResume.Click += new EventHandler(TsmPauseResume_Click);
        }

        private async Task InitialiseFolderWatchers()
        {
            FileSystemFolderWatchers.Clear();
            foreach (var watchFolder in WatchFolders)
            {
                FileSystemFolderWatchers.Add(new FileSystemWatcher
                {
                    Path = watchFolder.Path,
                    NotifyFilter = NotifyFilters.CreationTime |
                                   NotifyFilters.DirectoryName |
                                   NotifyFilters.Size |
                                   NotifyFilters.Attributes |
                                   NotifyFilters.FileName |
                                   NotifyFilters.LastWrite |
                                   NotifyFilters.Size,
                    Filter = "*.*",
                    EnableRaisingEvents = true,
                    IncludeSubdirectories = true
                });

                FileSystemFolderWatchers[FileSystemFolderWatchers.Count - 1]
                    .Changed += new FileSystemEventHandler(FolderWatcher_OnChanged);

                FileSystemFolderWatchers[FileSystemFolderWatchers.Count - 1]
                    .Renamed += new RenamedEventHandler(FolderWatcher_OnRenamed);

                FileSystemFolderWatchers[FileSystemFolderWatchers.Count - 1]
                    .Created += new FileSystemEventHandler(FolderWatcher_OnChanged);

                FileSystemFolderWatchers[FileSystemFolderWatchers.Count - 1]
                    .Deleted += new FileSystemEventHandler(FolderWatcher_OnChanged);
            }

            await Task.Run(() => { });
        }

        public void InstallEdge()
        {
            InstallingEdge = true;
            _installingEdgeThread = new Thread((ThreadStart)delegate
            {
                SetConnectToYouTubeButtonEnabled(false);
                SetStatusMessage("Installing Canary Edge Core. This may take a couple of minutes...", "Preparing dependencies");

                try
                {
                    using (var archive = SevenZipArchive.Open(Path.Combine(Global.WorkingDirectory, $"AppData\\{Global.EdgeVersion}.7z")))
                    {
                        using (var reader = archive.ExtractAllEntries())
                        {
                            var options = new ExtractionOptions
                            {
                                ExtractFullPath = true,
                                Overwrite = true
                            };

                            reader.WriteAllToDirectory(Global.EdgeFolder, options);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine(e.Message);
                }

                InstallingEdge = false;
                ConnectToYTMusicForm = new ConnectToYTMusic(this);
                SetConnectToYouTubeButtonEnabled(true);
            })
            {
                IsBackground = true
            };
            _installingEdgeThread.Start();
        }

        private async Task LoadDb()
        {
            Settings = SettingsRepo.Load().Result;

            SetThrottleSpeed(
                Settings.ThrottleSpeed == 0 || Settings.ThrottleSpeed == -1
                    ? "-1"
                    : (Convert.ToDouble(Settings.ThrottleSpeed) / 1000000).ToString());

            await RegistrySettings.SetStartWithWindows(Settings.StartWithWindows);
            SetSendLogsToSource(Settings.SendLogsToSource);
            SetStartWithWindows(Settings.StartWithWindows);
            SetAlsoUploadPlaylists(Settings.UploadPlaylists);

            await BindWatchFoldersList();
            await InitialiseFolderWatchers();

            if (WatchFolders.Count == 0)
                SetAmountLabelsToZero();

            int initialFilesCount = await MusicFileRepo.CountAll();
            int issueCount = await MusicFileRepo.CountIssues();
            int uploadsCount = await MusicFileRepo.CountUploaded();

            InitialFilesCount = Task.FromResult(initialFilesCount).Result;
            SetDiscoveredFilesLabel(InitialFilesCount.ToString());
            SetIssuesLabel(Task.FromResult(issueCount).Result.ToString());
            SetUploadedLabel(Task.FromResult(uploadsCount).Result.ToString());

            Logger.ClearHistoricLogs();

            RunDebugCommands();
            EnableOptionButtons(true);
        }

        private void ConnectToYouTubeMusic()
        {
            _connectToYouTubeMusicThread = new Thread((ThreadStart)delegate
            {
                while (Settings == null)
                    ThreadHelper.SafeSleep(200);

                while (InstallingEdge)
                    ThreadHelper.SafeSleep(200);

                SetStatusMessage("Connecting to YouTube Music", "Connecting to YouTube Music");
                while (!NetworkHelper.InternetConnectionIsUp())
                    ThreadHelper.SafeSleep(5000);

                while (ManagingYTMusicStatus == ManagingYTMusicStatusEnum.Showing)
                    ThreadHelper.SafeSleep(1000);
                if (ManagingYTMusicStatus == ManagingYTMusicStatusEnum.CloseChanges)
                    return;

                while (!Requests.IsAuthenticated(Settings.AuthenticationCookie))
                {
                    SetConnectedToYouTubeMusic(false);
                    ThreadHelper.SafeSleep(1000);
                    Settings = SettingsRepo.Load().Result;
                }

                SetConnectedToYouTubeMusic(true);
            })
            {
                IsBackground = true
            };
            _connectToYouTubeMusicThread.Start();
        }

        public void StartMainProcess(bool restarting = false)
        {
            IdleProcessor.Paused = true;

            // Only perform at start up
            if (!DatabaseIntegrityCheckDone)
            {
                SetStatusMessage("Checking database integrity", "Checking database integrity");
                Database.Maintenance.CheckAndCopyDatabaseFile();
                DatabaseIntegrityCheckDone = true;
            }

            Logger.LogInfo("StartMainProcess", "Main process thread starting");

            _scanAndUploadThread = new Thread((ThreadStart)delegate
            {
                if (restarting)
                {
                    if (WatchFolders.Count == 0)
                    {
                        MusicFileRepo.DeleteAll().Wait();
                        SetDiscoveredFilesLabel("0");
                        SetIssuesLabel("0");
                        SetUploadedLabel("0");
                    }
                }

                MainProcess(restarting);
                int retryIssuesCount = 0;
                while (MusicFileRepo.CountIssues().Result > 0)
                {
                    ThreadHelper.SafeSleep(10000);
                    retryIssuesCount++;
                    if (retryIssuesCount < Global.YTMusicIssuesMainProcessRetry)
                        MainProcess();
                    else
                        break;
                }
            })
            {
                IsBackground = true
            };
            _scanAndUploadThread.Start();
        }

        private void MainProcess(bool restarting = false)
        {
            try
            {
                ThreadHelper.SafeSleep(4000);

                if (!restarting)
                    LoadDb().Wait();

                while (InstallingEdge)
                    ThreadHelper.SafeSleep(200);

                CheckForLatestVersion();

                Logger.LogInfo("MainProcess", "File scanner starting");
                FileScanner.Process();
                Logger.LogInfo("MainProcess", "File scan complete");

                YTMAuthenticationCheckWait();
                RepopulateAmountLables();

                Logger.LogInfo("MainProcess", "Starting upload check and upload process");
                FileUploader.Process().Wait();
                Logger.LogInfo("MainProcess", "Upload check and process upload process complete");

                YTMAuthenticationCheckWait();
                RepopulateAmountLables();

                if (Settings.UploadPlaylists)
                {
                    if (!Settings.LastPlaylistUpload.HasValue)
                        Settings.LastPlaylistUpload = DateTime.Now.AddHours(Global.SessionRestartHours * -1).AddHours(-2);

                    if (DateTime.Now > ((DateTime)Settings.LastPlaylistUpload).AddHours(Global.SessionRestartHours))
                    {
                        Settings.CurrentSessionPlaylistUploadCount = 0;
                        Settings.Save().Wait();

                        Logger.LogInfo("MainProcess", "Starting playlist processing");
                        PlaylistProcessor.Process();
                        Logger.LogInfo("MainProcess", "Playlist processing complete");
                    }
                }

                if (ManagingYTMusicStatus != ManagingYTMusicStatusEnum.Showing)
                {
                    SetStatusMessage("Idle", "Idle");
                    SetUploadingMessage("Idle", "Idle", null, true);
                }

                if (WatchFolders.Count == 0)
                {
                    SetAmountLabelsToZero();
                }
                else
                    RepopulateAmountLables(true);

                ThreadHelper.SafeSleep(10000);
            }
            catch (Exception e)
            {
                string _ = e.Message;
#if DEBUG
                Console.Out.WriteLine("Main Process Thread Error: " + e.Message);
#endif
                if (e.Message.ToLower().Contains("thread was being aborted") ||
                    (e.InnerException != null && e.InnerException.Message.ToLower().Contains("thread was being aborted")))
                {
                    // Non-detrimental - Ignore to not clog up the application log
                    // Logger.Log(e, "Main Process thread error", Log.LogTypeEnum.Warning);
                }
                else
                {
                    Logger.Log(e, "Main Process thread error", Log.LogTypeEnum.Critical);
                }
            }

            IdleProcessor.Paused = false;
        }

        private void YTMAuthenticationCheckWait()
        {
            while (!ConnectedToYTMusic)
            {
                if (Aborting)
                {
                    if (WatchFolders.Count == 0)
                        SetAmountLabelsToZero();

                    SetStatusMessage("Stopping", "Stopping");
                    return;
                }

                ThreadHelper.SafeSleep(1000);
            }

            while (!NetworkHelper.InternetConnectionIsUp())
            {
                SetStatusMessage("No internet connection", "No internet connection");
                ThreadHelper.SafeSleep(5000);
            }

            while (!Requests.IsAuthenticated(Settings.AuthenticationCookie))
            {
                try
                {
                    SetConnectedToYouTubeMusic(false);
                    ThreadHelper.SafeSleep(1000);
                    Settings = SettingsRepo.Load().Result;
                }
                catch { }
            }

            SetConnectedToYouTubeMusic(true);
        }

        public void SetAmountLabelsToZero()
        {
            MusicFileRepo.DeleteAll().Wait();
            SetDiscoveredFilesLabel("0");
            SetIssuesLabel("0");
            SetUploadedLabel("0");
            InitialFilesCount = 0;
        }

        public void RepopulateAmountLables(bool includeDiscoveredFiles = false)
        {
            SetIssuesLabel(MusicFileRepo.CountIssues().Result.ToString());
            SetUploadedLabel(MusicFileRepo.CountUploaded().Result.ToString());

            if (includeDiscoveredFiles)
            {
                InitialFilesCount = MusicFileRepo.CountAll().Result;
                SetDiscoveredFilesLabel(InitialFilesCount.ToString());
            }
        }

        private void CheckForLatestVersion()
        {
            UpdaterPath = EmbeddedResourceHelper.GetEmbeddedResourcePath(
                "YTMusicUploader.Updater.exe",
                "Embedded",
                EmbeddedResourceHelper.TargetAssemblyType.Executing,
                false);

            if (VersionHelper.LatestVersionGreaterThanCurrentVersion(out string htmlUrl, out string latestVersion))
            {
                LatestVersionUrl = htmlUrl;
                LatestVersionTag = latestVersion;
                SetVersionWarningVisible(true);

                NewVersionTooltip.SetToolTip(pbUpdate,
                    "\nVersion " + LatestVersionTag + " available.\nClick for details.");

                Logger.LogInfo("CheckForLatestVersion", "Newer software version detected");
            }
            else
            {
                LatestVersionUrl = null;
                LatestVersionTag = null;
                SetVersionWarningVisible(false);
            }
        }

        public void Restart()
        {
            Aborting = true;

            ThreadPool.QueueUserWorkItem(delegate
            {
                while (
                    !FileScanner.Stopped ||
                    !FileUploader.Stopped ||
                    !PlaylistProcessor.Stopped ||
                    !IdleProcessor.Stopped ||
                    !QueueChecker.Stopped)
                {
                    ThreadHelper.SafeSleep(250);
                }

                FileScanner = new FileScanner(this);
                FileUploader = new FileUploader(this);
                IdleProcessor = new IdleProcessor(this);
                QueueChecker = new QueueChecker(this);
                PlaylistProcessor = new PlaylistProcessor(this);

                FileScanner.Reset();
                Aborting = false;

                ManagingYTMusicStatus = ManagingYTMusicStatusEnum.CloseChangesComplete;
                StartMainProcess(true);
            });
        }

        public void ShowMessageBox(
            string title,
            string message,
            MessageBoxButtons buttons,
            MessageBoxIcon icon,
            int height)
        {
            MetroMessageBox.Show(this, message, title, buttons, icon, height);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Hide();
                e.Cancel = true;
                Opacity = 0;
                ShowInTaskbar = false;
            }
            else
            {
                QuitApplication();
            }
        }

        public void KillApplication()
        {
            QuitApplication(true);
        }

        public void QuitApplication(bool kill = false)
        {
            Logger.LogInfo("MainForm_FormClosing", "Application closing");

            Aborting = true;
            Requests.UploadCheckCache.CleanUp = true;
            IdleProcessor.Stopped = true;
            QueueChecker.Stopped = true;
            FileUploader.Stopped = true;
            PlaylistProcessor.Stopped = true;
            TrayIcon.Visible = false;

            if (!kill)
            {
                try
                {
                    ConnectToYTMusicForm.BrowserControl.Dispose();
                }
                catch
                { }

                try
                {
                    ConnectToYTMusicForm.Dispose();
                }
                catch
                { }
            }

            try
            {
                _installingEdgeThread.Abort();
            }
            catch { }

            try
            {
                Requests.UploadCheckPreloaderThread.Abort();
            }
            catch { }

            try
            {
                Requests.UploadCheckPreloaderSleepThread.Abort();
            }
            catch { }

            try
            {
                IdleProcessor.IdleProcessorThread.Abort();
            }
            catch { }

            try
            {
                _scanAndUploadThread.Abort();
            }
            catch { }

            try
            {
                _connectToYouTubeMusicThread.Abort();
            }
            catch { }

            try
            {
                _restartThread.Abort();
            }
            catch { }

            try
            {
                QueueChecker.QueueCheckerThread.Abort();
            }
            catch { }

            if (!kill)
            {
                try
                {
                    Application.Exit();
                }
                catch { }

                try
                {
                    Environment.Exit(0);
                }
                catch { }
            }

            try
            {
                Process.GetCurrentProcess().Kill();
            }
            catch { }
        }
    }
}
