using System.Windows.Forms;

namespace YTMusicUploader
{
    public partial class MainForm
    {
        //
        // Tooltips
        //
        public ToolTip ArtWorkTooltip { get; set; }
        public ToolTip ApplicationLogsTooltip { get; set; }
        public ToolTip ConnectSuccessTooltip { get; set; }
        public ToolTip ConnectFailureTooltip { get; set; }
        public ToolTip YtMusicManageTooltip { get; set; }
        public ToolTip NewVersionTooltip { get; set; }
        public ToolTip AboutTooltip { get; set; }
        public ToolTip ConnectToYTMusicBtnTooltip { get; set; }
        public ToolTip StartWithWindowsTooltip { get; set; }
        public ToolTip SendDiagnosticDataTooltip { get; set; }
        public ToolTip ThrottleUploadSpeedTooltip { get; set; }
        public ToolTip AddWatchFolderTooltip { get; set; }
        public ToolTip RemoveWatchFolderTooltip { get; set; }
        public ToolTip DiscoveredFilesTooltip { get; set; }
        public ToolTip UploadIssuesTooltip { get; set; }
        public ToolTip UploadSuccessTooltip { get; set; }
        public ToolTip DiscoveredFilesLabelTooltip { get; set; }
        public ToolTip UploadIssuesLabelTooltip { get; set; }
        public ToolTip UploadSuccessLabelTooltip { get; set; }

        private void InitialiseTooltips()
        {
            //
            // Standard delay
            //

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
                "\nView and delete albums, tracks and playlists uploaded to YouTube Music.");

            NewVersionTooltip = new ToolTip
            {
                ToolTipTitle = "New Version Available",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 500,
            };
            NewVersionTooltip.SetToolTip(pbUpdate,
                "\nVersion " + LatestVersionTag + " available.\nClick for details.");

            AboutTooltip = new ToolTip
            {
                ToolTipTitle = "About",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 750,
            };
            AboutTooltip.SetToolTip(pbAbout,
                "\nAbout YTMusic Uploader.");

            //
            // More delayed
            //

            ConnectToYTMusicBtnTooltip = new ToolTip
            {
                ToolTipTitle = "YouTube Music Connection",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 1500,
            };
            ConnectToYTMusicBtnTooltip.SetToolTip(btnConnectToYoutube,
                "\nClick here to login to YouTube Music.");

            StartWithWindowsTooltip = new ToolTip
            {
                ToolTipTitle = "Start Application With Windows",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 1500,
            };
            StartWithWindowsTooltip.SetToolTip(lblStartWithWindows,
                "\nIf checked, YT Music Uploader will automatically start (hidden to the system tray)\n" +
                "when Windows starts up.");

            StartWithWindowsTooltip = new ToolTip
            {
                ToolTipTitle = "Send Diagnostic Data",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 1500,
            };
            StartWithWindowsTooltip.SetToolTip(lblSendDiagnosticData,
                "\nIf checked, this application will send error logs to the source for the sole\n" +
                "purpose of troubleshooting bugs.");

            ThrottleUploadSpeedTooltip = new ToolTip
            {
                ToolTipTitle = "Throttle Upload Speed",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 1500,
            };
            ThrottleUploadSpeedTooltip.SetToolTip(lblThrottleUploadSpeed,
                "\nIf you want to conserve upload bandwidth, specify the maximum upload\n" +
                "speed in MB (mega bytes) here.");

            AddWatchFolderTooltip = new ToolTip
            {
                ToolTipTitle = "Add Watch Folder",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 1500,
            };
            AddWatchFolderTooltip.SetToolTip(btnAddWatchFolder,
                "\nSpecify a folder to monitor and upload it's contents to YouTube Music.");

            RemoveWatchFolderTooltip = new ToolTip
            {
                ToolTipTitle = "Remove Watch Folder",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 1500,
            };
            RemoveWatchFolderTooltip.SetToolTip(btnRemoveWatchFolder,
                "\nRemove a watch folder form this list and stop monitoring that folder.");

            DiscoveredFilesTooltip = new ToolTip
            {
                ToolTipTitle = "Discovered Files",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 1500,
            };
            DiscoveredFilesTooltip.SetToolTip(lblDiscoveredFiles,
                "\nShows the total amount of 'valid' discovered files found in the\nwatch folders.");

            DiscoveredFilesLabelTooltip = new ToolTip
            {
                ToolTipTitle = "Discovered Files Amount",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 1500,
            };
            DiscoveredFilesLabelTooltip.SetToolTip(lblDiscoveredFilesLabel,
                "\nShows the total amount of 'valid' discovered files found in the\nwatch folders.");

            UploadIssuesTooltip = new ToolTip
            {
                ToolTipTitle = "Upload Issues Amount",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 1500,
            };
            UploadIssuesTooltip.SetToolTip(lblIssues,
                "\nShows the number of files that could not be uploaded to YouTube Music.\n" +
                "Click for more details.");

            UploadIssuesLabelTooltip = new ToolTip
            {
                ToolTipTitle = "Upload Issues Amount",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 1500,
            };
            UploadIssuesLabelTooltip.SetToolTip(lblIssuesLabel,
                "\nShows the number of files that could not be uploaded to YouTube Music.\n" +
                "Click the quanity hyperlink for more details.");

            UploadSuccessTooltip = new ToolTip
            {
                ToolTipTitle = "Upload Success Amount",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 1500,
            };
            UploadSuccessTooltip.SetToolTip(lblUploaded,
                "\nShows the number of files that have been successfully uploaded to YouTube Music.\n" +
                "Click for more details.");

            UploadSuccessLabelTooltip = new ToolTip
            {
                ToolTipTitle = "Upload Success Amount",
                UseFading = true,
                IsBalloon = true,
                InitialDelay = 1500,
            };
            UploadSuccessLabelTooltip.SetToolTip(lblUploadedLabel,
                "\nShows the number of files that have been successfully uploaded to YouTube Music.\n" +
                "Click the quanity hyperlink for more details.");
        }
    }
}
