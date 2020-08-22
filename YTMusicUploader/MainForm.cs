using System;
using System.Collections.Generic;
using System.Windows.Forms;
using JBToolkit.WinForms;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.Models;
using YTMusicUploader.Providers.Repos;
using System.Threading;
using YTMusicUploader.Dialogues;
using YTMusicUploader.Business;
using JBToolkit.Culture;
using JBTookit.Network;

namespace YTMusicUploader
{
    public partial class MainForm : OptimisedMetroForm
    {
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
        public ConnectToYTMusic ConnectToYTMusicForm { get; set; }

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

        public bool Aborting { get; set; } = false;
        public bool Queue { get; set; } = false;

        //
        // Threads
        //
        private Thread _scanAndUploadThread;
        private Thread _queueThread;

        public MainForm() : base(formResizable: false)
        {
            CultureHelper.GloballySetCultureToGB();

            InitializeComponent();
            SuspendDrawing(this);

            ConnectToYTMusicForm = new ConnectToYTMusic(this);

            FileScanner = new FileScanner(this);
            Thread.Sleep(2000);
            FileUploader = new FileUploader(this);

            InitialiseTimers();
            InitialiseTooltips();

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

        private void InitialiseTooltips()
        {
            ToolTip tyConnectSuccess = new ToolTip
            {
                ToolTipTitle = "YouTube Music Connection",
                ToolTipIcon = ToolTipIcon.Info,
                IsBalloon = true,
                InitialDelay = 1000,
            };

            ToolTip tyConnectFailure = new ToolTip
            {
                ToolTipTitle = "YouTube Music Connection",
                ToolTipIcon = ToolTipIcon.Warning,
                IsBalloon = true,
                InitialDelay = 1000,
            };

            tyConnectSuccess.SetToolTip(pbConnectedToYoutube,
                "\nYou are connected to YouTube Music and successfully authenticated.");

            tyConnectFailure.SetToolTip(pbNotConnectedToYoutube,
                "\nYou are not connected to YouTube Music.\n\nPress the 'Connect to YouTube Music button and sign into YouTube Music.");
        }

        private void LoadCheckAndProcess()
        {
            DataAccess.CheckAndCopyDatabaseFile();
            _scanAndUploadThread = new Thread((ThreadStart)delegate
            {
                try
                {
                    LoadDb();
                    if (Aborting)
                    {
                        SetStatusMessage("Idle");
                        return;
                    }

                    FileScanner.Process();
                    if (Aborting)
                    {
                        SetStatusMessage("Idle");
                        return;
                    }

                    SetStatusMessage("Connecting to YouTube Music");
                    while (!NetworkHelper.InternetConnectionIsUp())
                        Thread.Sleep(5000);

                    while (!Requests.IsAuthenticated(Settings.AuthenticationCookie))
                    {
                        SetConnectedToYouTubeMusic(false);
                        Thread.Sleep(5000);
                        Settings = SettingsRepo.Load();
                    }

                    SetConnectedToYouTubeMusic(true);
                    SetStatusMessage("Idle");

                    if (Aborting)
                    {
                        SetStatusMessage("Idle");
                        return;
                    }

                    SetStatusMessage("Uploading");
                    FileUploader.Process();
                    SetStatusMessage("Idle");
                }
                catch (Exception e)
                {
#if DEBUG
                    Console.Out.WriteLine("Main Process Thread Error: " + e.Message);
#endif
                }
            });

            _scanAndUploadThread.Start();
        }

        private void LoadDb()
        {
            SetVersion("v" + Global.ApplicationVersion);
            Settings = SettingsRepo.Load();
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
        }

        private void StartQueueCheck()
        {
            _queueThread = new Thread((ThreadStart)delegate
            {
                while (true)
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
            });

            _queueThread.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Aborting = true;
            Thread.Sleep(500);

            try
            {
                ConnectToYTMusicForm.BrowserControl.Dispose();
            }
            catch
            {
                Thread.Sleep(500);
                try
                {
                    ConnectToYTMusicForm.BrowserControl.Dispose();
                }
                catch { }
            }

            try
            {
                _scanAndUploadThread.Interrupt();
            }
            catch { }

            try
            {
                _scanAndUploadThread.Abort();
            }
            catch { }

            try
            {
                Environment.Exit(0);
            }
            catch { }
        }
    }
}
