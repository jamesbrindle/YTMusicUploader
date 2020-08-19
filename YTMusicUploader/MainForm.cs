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


        public bool ConnectedToYTMusic { get; set; } = false;
        public System.Windows.Forms.Timer ThrottleTextChangedTimer { get; set; }
        public int InitialFilesCount { get; set; } = 0;
        public bool StopProcessing { get; set; } = false;


        private Thread _fileDiscoveryThread;

        public MainForm() : base(formResizable: false)
        {
            CultureHelper.GloballySetCultureToGB();
            //CheckForIllegalCrossThreadCalls = false;            

            InitializeComponent();
            SuspendDrawing(this);

            ConnectToYTMusicForm = new ConnectToYTMusic(this);

            FileScanner = new FileScanner(this);
            Thread.Sleep(2000);
            FileUploader = new FileUploader(this);

            InitialiseTimers();
            InitialiseTooltips();

            LoadCheckAndProcess();
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
            _fileDiscoveryThread = new Thread((ThreadStart)delegate
            {
                try
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

                    SetStatusMessage("Connecting to YouTube Music");

                    while (!NetworkHelper.InternetConnectionIsUp())
                        Thread.Sleep(5000);

                    while (!Requests.IsAuthenticated(Settings.AuthenticationCookie)) {
                        SetConnectedToYouTubeMusic(false);
                        Thread.Sleep(5000);
                        Settings = SettingsRepo.Load();
                    }

                    SetConnectedToYouTubeMusic(true);

                    SetStatusMessage("Not running");
                    FileScanner.Process();

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

            _fileDiscoveryThread.Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FileScanner.Abort = true;
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
                _fileDiscoveryThread.Interrupt();
            }
            catch { }

            try
            {
                _fileDiscoveryThread.Abort();
            }
            catch { }
        }
    }
}
