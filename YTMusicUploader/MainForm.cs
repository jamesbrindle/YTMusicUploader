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
using YTMusicUploader.Business;
using YTMusicUploader.Dialogues;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.DataModels;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader
{
    public partial class MainForm : OptimisedMetroForm
    {
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

        private bool StartHidden = false;

        //
        // MusicBrainz Access
        //
        public MusicDataFetcher MusicDataFetcher { get; set; }
        public Image ArtworkImage { get; set; }

        //
        // Tooltips
        //
        public ToolTip ArtWorkTooltip { get; set; }
        public ToolTip ApplicationLogsTooltip { get; set; }
        public ToolTip ConnectSuccessTooltip { get; set; }
        public ToolTip ConnectFailureTooltip { get; set; }
        public ToolTip YtMusicManageTooltip { get; set; }
        public ToolTip AboutTooltip { get; set; }

        //
        // Threads
        //
        private Thread _installingEdgeThread;
        private Thread _connectToYouTubeMusicThread;
        private Thread _scanAndUploadThread;

        public MainForm(bool hidden) : base(formResizable: false)
        {
#if RELEASE
            CheckForIllegalCrossThreadCalls = false;
#endif
            CultureHelper.GloballySetCultureToGB();

            Logger.LogInfo("MainForm", "Application start begin");

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
            IdleProcessor = new IdleProcessor(this);
            QueueChecker = new QueueChecker(this);

            InitialiseTimers();
            InitialiseTooltips();
            InitialiseSystemTrayIconMenuButtons();

            Logger.LogInfo("MainForm", "Application start success");

            ConnectToYouTubeMusic();
            StartMainProcess();
        }

        private void RunDebugCommands()
        {
            // Placeholder for running any debug tests
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
        }

        private void InitialiseTooltips()
        {
            ConnectSuccessTooltip = new ToolTip
            {
                ToolTipTitle = "YouTube Music Connection",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 750,
            };
            ConnectSuccessTooltip.SetToolTip(pbConnectedToYoutube,
                "\nYou are connected to YouTube Music and successfully authenticated.");

            ConnectFailureTooltip = new ToolTip
            {
                ToolTipTitle = "YouTube Music Connection",
                ToolTipIcon = ToolTipIcon.Warning,
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 750,
            };
            ConnectFailureTooltip.SetToolTip(pbNotConnectedToYoutube,
                "\nYou are not connected to YouTube Music.\n\nPress the 'Connect to YouTube Music button and sign into YouTube Music.");

            ArtWorkTooltip = new ToolTip
            {
                ToolTipTitle = "Music File Meta Data",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 750,
            };
            ArtWorkTooltip.SetToolTip(pbArtwork,
                "\nNothing uploading");

            ApplicationLogsTooltip = new ToolTip
            {
                ToolTipTitle = "Application Logs",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 750,
            };
            ApplicationLogsTooltip.SetToolTip(pbLog,
                $"\nShows all info and error logs over the past {Global.ClearLogsAfterDays} days.");

            YtMusicManageTooltip = new ToolTip
            {
                ToolTipTitle = "YouTube Music Manage",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 750,
            };
            YtMusicManageTooltip.SetToolTip(pbYtMusicManage,
                "\nSee and delete albums and tracks uploaded to YouTube Music.");

            AboutTooltip = new ToolTip
            {
                ToolTipTitle = "About",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 750,
            };
            AboutTooltip.SetToolTip(pbAbout,
                "\nAbout YTMusic Uploader.");
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
                                   NotifyFilters.LastAccess |
                                   NotifyFilters.LastWrite |
                                   NotifyFilters.Size,
                    Filter = "*.*",
                    EnableRaisingEvents = true
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

            var initialFilesCount = await MusicFileRepo.CountAll();
            var issueCount = await MusicFileRepo.CountIssues();
            var uploadsCount = await MusicFileRepo.CountUploaded();

            await RegistrySettings.SetStartWithWindows(Settings.StartWithWindows);
            SetSendLogsToSource(Settings.SendLogsToSource);
            SetStartWithWindows(Settings.StartWithWindows);
            SetDiscoveredFilesLabel(InitialFilesCount.ToString());

            await BindWatchFoldersList();
            await InitialiseFolderWatchers();

            InitialFilesCount = Task.FromResult(initialFilesCount).Result;
            SetIssuesLabel(Task.FromResult(issueCount).Result.ToString());
            SetUploadedLabel(Task.FromResult(uploadsCount).Result.ToString());

            Logger.ClearHistoricLogs();

            RunDebugCommands();
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
                    Thread.Sleep(1000);
                if (ManagingYTMusicStatus == ManagingYTMusicStatusEnum.CloseChanges)
                    return;

                while (!Requests.IsAuthenticated(Settings.AuthenticationCookie))
                {
                    SetConnectedToYouTubeMusic(false);
                    ThreadHelper.SafeSleep(1000);
                    Settings = SettingsRepo.Load().Result;
                }

                Logger.LogInfo("ConnectToYouTubeMusic", "Connection to YouTube Music successful");
                SetConnectedToYouTubeMusic(true);
            })
            {
                IsBackground = true
            };
            _connectToYouTubeMusicThread.Start();
        }

        public void StartMainProcess()
        {
            Logger.LogInfo("StartMainProcess", "Main process thread starting");

            IdleProcessor.Paused = true;

            Logger.LogInfo("StartMainProcess", "DB check starting");
            DataAccess.CheckAndCopyDatabaseFile();
            Logger.LogInfo("StartMainProcess", "DB check complete");

            _scanAndUploadThread = new Thread((ThreadStart)delegate
            {
                MainProcess();
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

        private void MainProcess()
        {
            try
            {
                LoadDb().Wait();

                while (InstallingEdge)
                    ThreadHelper.SafeSleep(200);

                if (Aborting)
                {
                    SetStatusMessage("Idle", "Idle");
                    return;
                }

                Logger.LogInfo("MainProcess", "File scanner starting");
                FileScanner.Process();
                Logger.LogInfo("MainProcess", "File scan complete");

                while (!ConnectedToYTMusic)
                {
                    if (Aborting)
                    {
                        SetStatusMessage("Idle", "Idle");
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
                SetStatusMessage("Uploading", "Uploading");
                RepopulateAmountLables();

                Logger.LogInfo("MainProcess", "Starting upload check and process");
                FileUploader.Process().Wait();
                Logger.LogInfo("MainProcess", "Upload check and process complete");

                SetStatusMessage("Idle", "Idle");
                SetUploadingMessage("Idle", "Idle", null, true);
                RepopulateAmountLables(true);

                Thread.Sleep(10000);
            }
            catch (Exception e)
            {
                string _ = e.Message;
#if DEBUG
                Console.Out.WriteLine("Main Process Thread Error: " + e.Message);
                Logger.Log(e);
#endif
            }

            IdleProcessor.Paused = false;
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

        public void Restart()
        {
            QueueChecker.Queue = false;
            Aborting = false;
            Requests.UploadCheckCache.CleanUp = true;
            FileUploader.Stopped = true;
            FileScanner.Reset();
            StartMainProcess();
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

        public void QuitApplication()
        {
            Logger.LogInfo("MainForm_FormClosing", "Application closing");

            Aborting = true;
            Requests.UploadCheckCache.CleanUp = true;
            IdleProcessor.Stopped = true;
            QueueChecker.Stopped = true;
            FileUploader.Stopped = true;
            TrayIcon.Visible = false;

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
                QueueChecker.QueueCheckerThread.Abort();
            }
            catch { }

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

            try
            {
                Process.GetCurrentProcess().Kill();
            }
            catch { }
        }
    }
}
