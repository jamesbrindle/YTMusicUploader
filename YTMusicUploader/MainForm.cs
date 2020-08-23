using JBToolkit.Culture;
using JBToolkit.Network;
using JBToolkit.WinForms;
using MetroFramework;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Common;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Business;
using YTMusicUploader.Dialogues;
using YTMusicUploader.Helpers;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.Models;
using YTMusicUploader.Providers.Repos;

namespace YTMusicUploader
{
    public partial class MainForm : OptimisedMetroForm
    {
        //
        // Dialogues
        //
        public ConnectToYTMusic ConnectToYTMusicForm { get; set; }
        public IssueLog IssueLogForm { get; set; } = null;
        public UploadLog UploadLogForm { get; set; } = null;

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

        //
        // Checkers
        //
        public bool ConnectedToYTMusic { get; set; } = false;
        public System.Windows.Forms.Timer ThrottleTextChangedTimer { get; set; }
        public int InitialFilesCount { get; set; } = 0;
        private List<FileSystemWatcher> FileSystemFolderWatchers { get; set; } = new List<FileSystemWatcher>();
        private DateTime? LastFolderChangeTime { get; set; }
        private bool InstallingEdge { get; set; }
        public bool Aborting { get; set; } = false;
        public bool Queue { get; set; } = false;

        //
        // Threads
        //
        private Thread _installingEdgeThread;
        private Thread _connectToYouTubeMusicThread;
        private Thread _scanAndUploadThread;
        private Thread _queueThread;

        public MainForm(bool hidden) : base(formResizable: false)
        {
            CultureHelper.GloballySetCultureToGB();

            if (hidden)
            {
                ShowInTaskbar = false;
                WindowState = FormWindowState.Minimized;
            }

            InitializeComponent();
            SuspendDrawing(this);

            lblIssues.GotFocus += LinkLabel_GotFocus;
            lblDiscoveredFiles.GotFocus += LinkLabel_GotFocus;

            if (!EdgeCoreHelper.CheckEdgeCoreFilesArePresentAndCorrect())
            {
                btnConnectToYoutube.Enabled = false;
                InstallEdge();
            }
            else
                ConnectToYTMusicForm = new ConnectToYTMusic(this);

            FileScanner = new FileScanner(this);
            FileUploader = new FileUploader(this);

            InitialiseTimers();
            InitialiseTooltips();
            InitialiseSystemTryIconMenuButtons();

            ConnectToYouTube();
            LoadCheckAndProcess();
            StartQueueCheck();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
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

        private void InitialiseSystemTryIconMenuButtons()
        {
            tsmShow.Click += new EventHandler(TsmShow_Click);
            tsmQuit.Click += new EventHandler(TsmQuit_Click);
        }

        private void InitialiseTooltips()
        {
            ToolTip tyConnectSuccess = new ToolTip
            {
                ToolTipTitle = "YouTube Music Connection",
                ToolTipIcon = ToolTipIcon.Info,
                IsBalloon = true,
                InitialDelay = 750,
            };

            ToolTip tyConnectFailure = new ToolTip
            {
                ToolTipTitle = "YouTube Music Connection",
                ToolTipIcon = ToolTipIcon.Warning,
                IsBalloon = true,
                InitialDelay = 750,
            };

            tyConnectSuccess.SetToolTip(pbConnectedToYoutube,
                "\nYou are connected to YouTube Music and successfully authenticated.");

            tyConnectFailure.SetToolTip(pbNotConnectedToYoutube,
                "\nYou are not connected to YouTube Music.\n\nPress the 'Connect to YouTube Music button and sign into YouTube Music.");
        }

        private void InitialiseFolderWatchers()
        {
            FileSystemFolderWatchers.Clear();
            foreach (var watchFolder in WatchFolders)
            {
                FileSystemFolderWatchers.Add(new FileSystemWatcher
                {
                    Path = watchFolder.Path,
                    NotifyFilter = NotifyFilters.CreationTime |
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
                    .Created += new FileSystemEventHandler(FolderWatcher_OnChanged);

                FileSystemFolderWatchers[FileSystemFolderWatchers.Count - 1]
                    .Deleted += new FileSystemEventHandler(FolderWatcher_OnChanged);
            }
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
                    using (var archive = SevenZipArchive.Open(Path.Combine(Global.WorkingDirectory, @"AppData\84.0.522.63.7z")))
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
            });

            _installingEdgeThread.Start();
        }

        private void ConnectToYouTube()
        {
            _connectToYouTubeMusicThread = new Thread((ThreadStart)delegate
            {
                while (Settings == null)
                {
                    try
                    {
                        Thread.Sleep(200);
                    }
                    catch { }
                }

                while (InstallingEdge)
                {
                    try
                    {
                        Thread.Sleep(200);
                    }
                    catch { }
                }

                SetStatusMessage("Connecting to YouTube Music", "Connecting to YouTube Music");
                while (!NetworkHelper.InternetConnectionIsUp())
                {
                    try
                    {
                        Thread.Sleep(5000);
                    }
                    catch { }
                }

                while (!Requests.IsAuthenticated(Settings.AuthenticationCookie))
                {
                    try
                    {
                        SetConnectedToYouTubeMusic(false);
                        Thread.Sleep(500);
                        Settings = SettingsRepo.Load();
                    }
                    catch { }
                }

                SetConnectedToYouTubeMusic(true);
            });

            _connectToYouTubeMusicThread.Start();
        }

        private void LoadCheckAndProcess()
        {
            DataAccess.CheckAndCopyDatabaseFile();
            _scanAndUploadThread = new Thread((ThreadStart)delegate
            {
                try
                {
                    LoadDb();

                    while (InstallingEdge)
                    {
                        try
                        {
                            Thread.Sleep(200);
                        }
                        catch { }
                    }

                    if (Aborting)
                    {
                        SetStatusMessage("Idle", "Idle");
                        return;
                    }

                    FileScanner.Process();
                    while (!ConnectedToYTMusic)
                    {
                        if (Aborting)
                        {
                            SetStatusMessage("Idle", "Idle");
                            return;
                        }

                        try
                        {
                            Thread.Sleep(5000);
                        }
                        catch { }
                    }

                    while (!NetworkHelper.InternetConnectionIsUp())
                    {
                        SetStatusMessage("No internet connection", "No internet connection");

                        try
                        {
                            Thread.Sleep(5000);
                        }
                        catch { }
                    }

                    while (!Requests.IsAuthenticated(Settings.AuthenticationCookie))
                    {
                        try
                        {
                            SetConnectedToYouTubeMusic(false);
                            Thread.Sleep(500);
                            Settings = SettingsRepo.Load();
                        }
                        catch { }
                    }

                    SetConnectedToYouTubeMusic(true);
                    SetStatusMessage("Uploading", "Uploading");
                    RepopulateAmountLables();
                    FileUploader.Process();
                    SetStatusMessage("Idle", "Idle");
                }
                catch (Exception e)
                {
                    string _ = e.Message;
#if DEBUG
                    Console.Out.WriteLine("Main Process Thread Error: " + e.Message);
#endif
                }
            });

            _scanAndUploadThread.Start();
        }

        private void StartQueueCheck()
        {
            _queueThread = new Thread((ThreadStart)delegate
            {
                while (InstallingEdge)
                {
                    try
                    {
                        Thread.Sleep(200);
                    }
                    catch { }
                }

                while (true)
                {
                    try
                    {
                        Thread.Sleep(1000);
                        if (Queue)
                        {
                            while (!FileUploader.Stopped)
                                Thread.Sleep(1000);

                            Queue = false;
                            Aborting = false;
                            FileUploader.Stopped = true;
                            FileScanner.Reset();
                            LoadCheckAndProcess();
                        }
                    }
                    catch { }
                }
            });

            _queueThread.Start();
        }

        private void LoadDb()
        {
            SetVersion("v" + Global.ApplicationVersion);
            Settings = SettingsRepo.Load();
            RegistryHelper.SetStartWithWindows(Settings.StartWithWindows);
            SetThrottleSpeed(
                Settings.ThrottleSpeed == 0 ||
                Settings.ThrottleSpeed == -1
                    ? "-1"
                    : (Convert.ToDouble(Settings.ThrottleSpeed) / 1000000).ToString());

            SetStartWithWindows(Settings.StartWithWindows);
            InitialFilesCount = MusicFileRepo.CountAll();
            SetDiscoveredFilesLabel(InitialFilesCount.ToString());
            SetIssuesLabel(MusicFileRepo.CountIssues().ToString());
            SetUploadedLabel(MusicFileRepo.CountUploaded().ToString());

            BindWatchFoldersList();
            InitialiseFolderWatchers();
        }

        public void RepopulateAmountLables()
        {
            SetIssuesLabel(MusicFileRepo.CountIssues().ToString());
            SetUploadedLabel(MusicFileRepo.CountUploaded().ToString());
        }

        public void ShowMessageBox(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon, int height)
        {
            MetroMessageBox.Show(this, message, title, buttons, icon, height);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            WindowState = FormWindowState.Minimized;
            ShowInTaskbar = false;
        }
    }
}
