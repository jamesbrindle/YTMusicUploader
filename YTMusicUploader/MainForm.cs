using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JBToolkit.WinForms;
using MetroFramework;
using MetroFramework.Forms;
using JBToolkit.Helpers;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.Models;
using YTMusicUploader.Providers.Repos;
using System.Threading;
using YTMusicUploader.Dialogues;

namespace YTMusicUploader
{
    public partial class MainForm : OptimisedMetroForm
    {
        public SettingsRepo SettingsRepo { get; set; } = new SettingsRepo();
        public WatchFolderRepo WatchFolderRepo { get; set; } = new WatchFolderRepo();
        public MusicFileRepo MusicFileRepo { get; set; } = new MusicFileRepo();
        public Settings Settings { get; set; }
        public List<WatchFolder> WatchFolders { get; set; }
        public ConnectToYTMusic ConnectToYTMusicForm { get; set; }

        public MainForm() : base(formResizable: false,
                                 controlTagsAsTooltips: false)
        {
            CulterHelper.GloballySetCultureToGB();
            //CheckForIllegalCrossThreadCalls = false;            

            InitializeComponent();
            SuspendDrawing(this);

            ConnectToYTMusicForm = new ConnectToYTMusic(this);
            InitialiseTooltips();
            PerformPrechecks();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            OnLoad(e);
            ResumeDrawing(this);
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

            tyConnectSuccess.SetToolTip(pbConnectedToYoutube, "\nYou are connected to YouTube Music and successfully authenticated.");
            tyConnectFailure.SetToolTip(pbNotConnectedToYoutube, "\nYou are not connected to YouTube Music.\n\nPress the 'Connect to YouTube Music button and sign into YouTube Music.");
        }

        private void PerformPrechecks()
        {
            // TODO: Try catch and handle here

            new Thread((ThreadStart)delegate
            {
                DataAccess.CheckAndCopyDatabaseFile();
                WatchFolders = WatchFolderRepo.Load();
                Settings = SettingsRepo.Load();

                SetStartWithWindows(Settings.StartWithWindows);
                BindWatchFoldersList();

                if (!Requests.CheckAndCopyApiFiles(this))
                {
                    MetroMessageBox.Show(
                       this,
                       @"Python (minimum version: 3.5) is required for this tool to work. You can download from here: https://www.python.org/downloads/",
                       "Dependency Required",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Asterisk,
                       120);
                }
                else
                {
                    SetStatusMessage("Connecting to YouTube Music");

                    if (!string.IsNullOrEmpty(Settings.AuthenticationCookie))
                        SetConnectedToYouTubeMusic(Requests.IsAuthenticated());
                    else
                        SetConnectedToYouTubeMusic(false);

                    SetStatusMessage("Not running");
                }

            }).Start();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ConnectToYTMusicForm.BrowserControl.Dispose();
        }
    }
}
