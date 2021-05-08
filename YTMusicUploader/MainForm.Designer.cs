namespace YTMusicUploader
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lblSub = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbUpdate = new System.Windows.Forms.PictureBox();
            this.lblVersion = new JBToolkit.WinForms.AntiAliasedLabel();
            this.lblStatus = new JBToolkit.WinForms.AntiAliasedLabel();
            this.lblDiscoveredFilesLabel = new System.Windows.Forms.Label();
            this.lblDiscoveredFiles = new System.Windows.Forms.Label();
            this.lblIssuesLabel = new System.Windows.Forms.Label();
            this.pnlRemoveFromWatchFolder = new System.Windows.Forms.Panel();
            this.btnRemoveWatchFolder = new System.Windows.Forms.PictureBox();
            this.lblUploadingMessage = new System.Windows.Forms.Label();
            this.lblUploadedLabel = new System.Windows.Forms.Label();
            this.FolderSelector = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmShow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmPauseResume = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.lblIssues = new System.Windows.Forms.LinkLabel();
            this.lblUploaded = new System.Windows.Forms.LinkLabel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblArtistMeta = new System.Windows.Forms.Label();
            this.lblAlbumMeta = new System.Windows.Forms.Label();
            this.lblTrackMeta = new System.Windows.Forms.Label();
            this.pbPaused = new System.Windows.Forms.PictureBox();
            this.pbLog = new System.Windows.Forms.PictureBox();
            this.pbYtMusicManage = new System.Windows.Forms.PictureBox();
            this.pbArtworkIdle = new System.Windows.Forms.PictureBox();
            this.pbArtwork = new System.Windows.Forms.PictureBox();
            this.pbConnectedToYoutube = new System.Windows.Forms.PictureBox();
            this.btnAddWatchFolder = new System.Windows.Forms.PictureBox();
            this.pbAbout = new System.Windows.Forms.PictureBox();
            this.pbNotConnectedToYoutube = new System.Windows.Forms.PictureBox();
            this.roundGroupBox2 = new JBToolkit.WinForms.RoundGroupBox();
            this.lbWatchFolders = new System.Windows.Forms.ListBox();
            this.roundGroupBox1 = new JBToolkit.WinForms.RoundGroupBox();
            this.pbUploadPlaylistsInfo = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbAlsoUploadPlaylists = new MetroFramework.Controls.MetroCheckBox();
            this.lblStartWithWindows = new System.Windows.Forms.Label();
            this.tbThrottleSpeed = new JBToolkit.WinForms.RoundTextBox();
            this.lblSendDiagnosticData = new System.Windows.Forms.Label();
            this.lblThrottleUploadSpeed = new System.Windows.Forms.Label();
            this.cbSendErrorLogsToSource = new MetroFramework.Controls.MetroCheckBox();
            this.cbStartWithWindows = new MetroFramework.Controls.MetroCheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnConnectToYoutube = new JBToolkit.WinForms.RoundButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbUpdate)).BeginInit();
            this.pnlRemoveFromWatchFolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnRemoveWatchFolder)).BeginInit();
            this.TrayContextMenuStrip.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPaused)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbYtMusicManage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbArtworkIdle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbArtwork)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectedToYoutube)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddWatchFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAbout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNotConnectedToYoutube)).BeginInit();
            this.roundGroupBox2.SuspendLayout();
            this.roundGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbUploadPlaylistsInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSub
            // 
            this.lblSub.AutoSize = true;
            this.lblSub.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblSub.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSub.Location = new System.Drawing.Point(24, 62);
            this.lblSub.Name = "lblSub";
            this.lblSub.Size = new System.Drawing.Size(306, 13);
            this.lblSub.TabIndex = 1;
            this.lblSub.Text = "Automatically upload your music library to YouTube Music.";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Controls.Add(this.pbUpdate);
            this.panel1.Controls.Add(this.lblVersion);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.panel1.Location = new System.Drawing.Point(0, 419);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(735, 31);
            this.panel1.TabIndex = 8;
            // 
            // pbUpdate
            // 
            this.pbUpdate.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pbUpdate.Image = global::YTMusicUploader.Properties.Resources.update_up;
            this.pbUpdate.Location = new System.Drawing.Point(657, 2);
            this.pbUpdate.Name = "pbUpdate";
            this.pbUpdate.Size = new System.Drawing.Size(16, 14);
            this.pbUpdate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbUpdate.TabIndex = 15;
            this.pbUpdate.TabStop = false;
            this.pbUpdate.Visible = false;
            this.pbUpdate.Click += new System.EventHandler(this.PbUpdate_Click);
            this.pbUpdate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbUpdate_MouseDown);
            this.pbUpdate.MouseEnter += new System.EventHandler(this.PbUpdate_MouseEnter);
            this.pbUpdate.MouseLeave += new System.EventHandler(this.PbUpdate_MouseLeave);
            this.pbUpdate.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PbUpdate_MouseUp);
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(680, 1);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(35, 20);
            this.lblVersion.TabIndex = 14;
            this.lblVersion.Text = "v1.7.0";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblStatus
            // 
            this.lblStatus.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(11, 1);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(574, 20);
            this.lblStatus.TabIndex = 13;
            this.lblStatus.Text = "Not running";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblDiscoveredFilesLabel
            // 
            this.lblDiscoveredFilesLabel.AutoSize = true;
            this.lblDiscoveredFilesLabel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblDiscoveredFilesLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscoveredFilesLabel.Location = new System.Drawing.Point(444, 257);
            this.lblDiscoveredFilesLabel.Name = "lblDiscoveredFilesLabel";
            this.lblDiscoveredFilesLabel.Size = new System.Drawing.Size(89, 13);
            this.lblDiscoveredFilesLabel.TabIndex = 9;
            this.lblDiscoveredFilesLabel.Text = "Discovered Files";
            // 
            // lblDiscoveredFiles
            // 
            this.lblDiscoveredFiles.AutoSize = true;
            this.lblDiscoveredFiles.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblDiscoveredFiles.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscoveredFiles.Location = new System.Drawing.Point(600, 257);
            this.lblDiscoveredFiles.Name = "lblDiscoveredFiles";
            this.lblDiscoveredFiles.Size = new System.Drawing.Size(13, 13);
            this.lblDiscoveredFiles.TabIndex = 10;
            this.lblDiscoveredFiles.Text = "0";
            // 
            // lblIssuesLabel
            // 
            this.lblIssuesLabel.AutoSize = true;
            this.lblIssuesLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblIssuesLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIssuesLabel.Location = new System.Drawing.Point(444, 277);
            this.lblIssuesLabel.Name = "lblIssuesLabel";
            this.lblIssuesLabel.Size = new System.Drawing.Size(38, 13);
            this.lblIssuesLabel.TabIndex = 11;
            this.lblIssuesLabel.Text = "Issues";
            // 
            // pnlRemoveFromWatchFolder
            // 
            this.pnlRemoveFromWatchFolder.Controls.Add(this.btnRemoveWatchFolder);
            this.pnlRemoveFromWatchFolder.Location = new System.Drawing.Point(381, 277);
            this.pnlRemoveFromWatchFolder.Name = "pnlRemoveFromWatchFolder";
            this.pnlRemoveFromWatchFolder.Size = new System.Drawing.Size(21, 17);
            this.pnlRemoveFromWatchFolder.TabIndex = 18;
            this.pnlRemoveFromWatchFolder.Click += new System.EventHandler(this.BtnRemoveWatchFolder_Click);
            this.pnlRemoveFromWatchFolder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnRemoveWatchFolder_MouseDown);
            this.pnlRemoveFromWatchFolder.MouseEnter += new System.EventHandler(this.BtnRemoveWatchFolder_MouseEnter);
            this.pnlRemoveFromWatchFolder.MouseLeave += new System.EventHandler(this.BtnRemoveWatchFolder_MouseLeave);
            // 
            // btnRemoveWatchFolder
            // 
            this.btnRemoveWatchFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemoveWatchFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemoveWatchFolder.Enabled = false;
            this.btnRemoveWatchFolder.Image = global::YTMusicUploader.Properties.Resources.minus;
            this.btnRemoveWatchFolder.Location = new System.Drawing.Point(0, 0);
            this.btnRemoveWatchFolder.Name = "btnRemoveWatchFolder";
            this.btnRemoveWatchFolder.Size = new System.Drawing.Size(21, 17);
            this.btnRemoveWatchFolder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnRemoveWatchFolder.TabIndex = 14;
            this.btnRemoveWatchFolder.TabStop = false;
            this.btnRemoveWatchFolder.Click += new System.EventHandler(this.BtnRemoveWatchFolder_Click);
            this.btnRemoveWatchFolder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnRemoveWatchFolder_MouseDown);
            this.btnRemoveWatchFolder.MouseLeave += new System.EventHandler(this.BtnRemoveWatchFolder_MouseLeave);
            this.btnRemoveWatchFolder.MouseHover += new System.EventHandler(this.BtnRemoveWatchFolder_MouseEnter);
            // 
            // lblUploadingMessage
            // 
            this.lblUploadingMessage.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblUploadingMessage.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUploadingMessage.Location = new System.Drawing.Point(27, 365);
            this.lblUploadingMessage.Margin = new System.Windows.Forms.Padding(0);
            this.lblUploadingMessage.Name = "lblUploadingMessage";
            this.lblUploadingMessage.Size = new System.Drawing.Size(668, 40);
            this.lblUploadingMessage.TabIndex = 19;
            this.lblUploadingMessage.Text = "Idle";
            this.lblUploadingMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblUploadedLabel
            // 
            this.lblUploadedLabel.AutoSize = true;
            this.lblUploadedLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblUploadedLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUploadedLabel.Location = new System.Drawing.Point(444, 297);
            this.lblUploadedLabel.Name = "lblUploadedLabel";
            this.lblUploadedLabel.Size = new System.Drawing.Size(58, 13);
            this.lblUploadedLabel.TabIndex = 20;
            this.lblUploadedLabel.Text = "Uploaded";
            // 
            // TrayIcon
            // 
            this.TrayIcon.BalloonTipText = "Upload your music library to YouTube Music";
            this.TrayIcon.BalloonTipTitle = "YT Music Uploader";
            this.TrayIcon.ContextMenuStrip = this.TrayContextMenuStrip;
            this.TrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("TrayIcon.Icon")));
            this.TrayIcon.Text = "YT Music Uploader";
            this.TrayIcon.Visible = true;
            this.TrayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseClick);
            this.TrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TrayIcon_MouseClick);
            // 
            // TrayContextMenuStrip
            // 
            this.TrayContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmShow,
            this.tsmPauseResume,
            this.tsmQuit});
            this.TrayContextMenuStrip.Name = "TrayContextMenuStrip";
            this.TrayContextMenuStrip.Size = new System.Drawing.Size(106, 70);
            // 
            // tsmShow
            // 
            this.tsmShow.Image = global::YTMusicUploader.Properties.Resources.show;
            this.tsmShow.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmShow.Name = "tsmShow";
            this.tsmShow.Size = new System.Drawing.Size(105, 22);
            this.tsmShow.Text = "Show";
            // 
            // tsmPauseResume
            // 
            this.tsmPauseResume.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.tsmPauseResume.Image = global::YTMusicUploader.Properties.Resources.pause_disabled;
            this.tsmPauseResume.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmPauseResume.Name = "tsmPauseResume";
            this.tsmPauseResume.Size = new System.Drawing.Size(105, 22);
            this.tsmPauseResume.Text = "Pause";
            // 
            // tsmQuit
            // 
            this.tsmQuit.Image = global::YTMusicUploader.Properties.Resources.quit;
            this.tsmQuit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmQuit.Name = "tsmQuit";
            this.tsmQuit.Size = new System.Drawing.Size(105, 22);
            this.tsmQuit.Text = "Quit";
            // 
            // lblIssues
            // 
            this.lblIssues.AutoSize = true;
            this.lblIssues.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblIssues.Location = new System.Drawing.Point(600, 277);
            this.lblIssues.Name = "lblIssues";
            this.lblIssues.Size = new System.Drawing.Size(13, 13);
            this.lblIssues.TabIndex = 4;
            this.lblIssues.TabStop = true;
            this.lblIssues.Text = "0";
            this.lblIssues.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LblIssues_LinkClicked);
            // 
            // lblUploaded
            // 
            this.lblUploaded.AutoSize = true;
            this.lblUploaded.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblUploaded.Location = new System.Drawing.Point(600, 297);
            this.lblUploaded.Name = "lblUploaded";
            this.lblUploaded.Size = new System.Drawing.Size(13, 13);
            this.lblUploaded.TabIndex = 5;
            this.lblUploaded.TabStop = true;
            this.lblUploaded.Text = "0";
            this.lblUploaded.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LblUploaded_LinkClicked);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.pictureBox1);
            this.pnlHeader.Location = new System.Drawing.Point(13, 16);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(214, 46);
            this.pnlHeader.TabIndex = 23;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::YTMusicUploader.Properties.Resources.Header;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(214, 46);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblArtistMeta
            // 
            this.lblArtistMeta.AutoSize = true;
            this.lblArtistMeta.Location = new System.Drawing.Point(98, 302);
            this.lblArtistMeta.Name = "lblArtistMeta";
            this.lblArtistMeta.Size = new System.Drawing.Size(0, 13);
            this.lblArtistMeta.TabIndex = 31;
            // 
            // lblAlbumMeta
            // 
            this.lblAlbumMeta.AutoSize = true;
            this.lblAlbumMeta.Location = new System.Drawing.Point(98, 320);
            this.lblAlbumMeta.Name = "lblAlbumMeta";
            this.lblAlbumMeta.Size = new System.Drawing.Size(0, 13);
            this.lblAlbumMeta.TabIndex = 32;
            // 
            // lblTrackMeta
            // 
            this.lblTrackMeta.AutoSize = true;
            this.lblTrackMeta.Location = new System.Drawing.Point(98, 338);
            this.lblTrackMeta.Name = "lblTrackMeta";
            this.lblTrackMeta.Size = new System.Drawing.Size(0, 13);
            this.lblTrackMeta.TabIndex = 33;
            // 
            // pbPaused
            // 
            this.pbPaused.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pbPaused.Image = global::YTMusicUploader.Properties.Resources.paused;
            this.pbPaused.Location = new System.Drawing.Point(30, 302);
            this.pbPaused.Name = "pbPaused";
            this.pbPaused.Padding = new System.Windows.Forms.Padding(2, 9, 0, 0);
            this.pbPaused.Size = new System.Drawing.Size(50, 50);
            this.pbPaused.TabIndex = 25;
            this.pbPaused.TabStop = false;
            this.pbPaused.Visible = false;
            // 
            // pbLog
            // 
            this.pbLog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbLog.Image = global::YTMusicUploader.Properties.Resources.log_up;
            this.pbLog.Location = new System.Drawing.Point(566, 44);
            this.pbLog.Name = "pbLog";
            this.pbLog.Size = new System.Drawing.Size(28, 30);
            this.pbLog.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbLog.TabIndex = 30;
            this.pbLog.TabStop = false;
            this.pbLog.Click += new System.EventHandler(this.PbLog_Click);
            this.pbLog.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbLog_MouseDown);
            this.pbLog.MouseEnter += new System.EventHandler(this.PbLog_MouseEnter);
            this.pbLog.MouseLeave += new System.EventHandler(this.PbLog_MouseLeave);
            this.pbLog.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PLog_MouseUp);
            // 
            // pbYtMusicManage
            // 
            this.pbYtMusicManage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbYtMusicManage.Enabled = false;
            this.pbYtMusicManage.Image = global::YTMusicUploader.Properties.Resources.ytmusic_manage_disabled;
            this.pbYtMusicManage.Location = new System.Drawing.Point(612, 44);
            this.pbYtMusicManage.Name = "pbYtMusicManage";
            this.pbYtMusicManage.Size = new System.Drawing.Size(30, 31);
            this.pbYtMusicManage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbYtMusicManage.TabIndex = 24;
            this.pbYtMusicManage.TabStop = false;
            this.pbYtMusicManage.Click += new System.EventHandler(this.PbYtMusicManage_Click);
            this.pbYtMusicManage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbYtMusicManage_MouseDown);
            this.pbYtMusicManage.MouseEnter += new System.EventHandler(this.PbYtMusicManage_MouseEnter);
            this.pbYtMusicManage.MouseLeave += new System.EventHandler(this.PbYtMusicManage_MouseLeave);
            this.pbYtMusicManage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PbYtMusicManage_MouseUp);
            // 
            // pbArtworkIdle
            // 
            this.pbArtworkIdle.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pbArtworkIdle.Image = global::YTMusicUploader.Properties.Resources.idle_square;
            this.pbArtworkIdle.Location = new System.Drawing.Point(30, 302);
            this.pbArtworkIdle.Name = "pbArtworkIdle";
            this.pbArtworkIdle.Size = new System.Drawing.Size(50, 50);
            this.pbArtworkIdle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbArtworkIdle.TabIndex = 22;
            this.pbArtworkIdle.TabStop = false;
            // 
            // pbArtwork
            // 
            this.pbArtwork.Location = new System.Drawing.Point(30, 302);
            this.pbArtwork.Name = "pbArtwork";
            this.pbArtwork.Size = new System.Drawing.Size(50, 50);
            this.pbArtwork.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbArtwork.TabIndex = 21;
            this.pbArtwork.TabStop = false;
            this.pbArtwork.Visible = false;
            // 
            // pbConnectedToYoutube
            // 
            this.pbConnectedToYoutube.Cursor = System.Windows.Forms.Cursors.Default;
            this.pbConnectedToYoutube.Image = global::YTMusicUploader.Properties.Resources.tick;
            this.pbConnectedToYoutube.Location = new System.Drawing.Point(645, 331);
            this.pbConnectedToYoutube.Name = "pbConnectedToYoutube";
            this.pbConnectedToYoutube.Size = new System.Drawing.Size(14, 14);
            this.pbConnectedToYoutube.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbConnectedToYoutube.TabIndex = 16;
            this.pbConnectedToYoutube.TabStop = false;
            this.pbConnectedToYoutube.Visible = false;
            // 
            // btnAddWatchFolder
            // 
            this.btnAddWatchFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddWatchFolder.Enabled = false;
            this.btnAddWatchFolder.Image = global::YTMusicUploader.Properties.Resources.plus;
            this.btnAddWatchFolder.Location = new System.Drawing.Point(353, 277);
            this.btnAddWatchFolder.Name = "btnAddWatchFolder";
            this.btnAddWatchFolder.Size = new System.Drawing.Size(16, 16);
            this.btnAddWatchFolder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnAddWatchFolder.TabIndex = 13;
            this.btnAddWatchFolder.TabStop = false;
            this.btnAddWatchFolder.Click += new System.EventHandler(this.BtnWatchFolder_Click);
            this.btnAddWatchFolder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnAddWatchFolder_MouseDown);
            this.btnAddWatchFolder.MouseEnter += new System.EventHandler(this.BtnAddWatchFolder_MouseEnter);
            this.btnAddWatchFolder.MouseLeave += new System.EventHandler(this.BtnAddWatchFolder_MouseLeave);
            // 
            // pbAbout
            // 
            this.pbAbout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbAbout.Image = ((System.Drawing.Image)(resources.GetObject("pbAbout.Image")));
            this.pbAbout.Location = new System.Drawing.Point(658, 39);
            this.pbAbout.Name = "pbAbout";
            this.pbAbout.Size = new System.Drawing.Size(40, 40);
            this.pbAbout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAbout.TabIndex = 0;
            this.pbAbout.TabStop = false;
            this.pbAbout.Click += new System.EventHandler(this.PbAbout_Click);
            this.pbAbout.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbAbout_MouseDown);
            this.pbAbout.MouseEnter += new System.EventHandler(this.PbAbout_MouseEnter);
            this.pbAbout.MouseLeave += new System.EventHandler(this.PbAbout_MouseLeave);
            this.pbAbout.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PbAbout_MouseUp);
            // 
            // pbNotConnectedToYoutube
            // 
            this.pbNotConnectedToYoutube.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pbNotConnectedToYoutube.Image = global::YTMusicUploader.Properties.Resources.cross;
            this.pbNotConnectedToYoutube.Location = new System.Drawing.Point(645, 331);
            this.pbNotConnectedToYoutube.Name = "pbNotConnectedToYoutube";
            this.pbNotConnectedToYoutube.Size = new System.Drawing.Size(12, 12);
            this.pbNotConnectedToYoutube.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbNotConnectedToYoutube.TabIndex = 17;
            this.pbNotConnectedToYoutube.TabStop = false;
            this.pbNotConnectedToYoutube.Visible = false;
            // 
            // roundGroupBox2
            // 
            this.roundGroupBox2.BackColor = System.Drawing.Color.Transparent;
            this.roundGroupBox2.Controls.Add(this.lbWatchFolders);
            this.roundGroupBox2.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.roundGroupBox2.Location = new System.Drawing.Point(27, 97);
            this.roundGroupBox2.Name = "roundGroupBox2";
            this.roundGroupBox2.Radious = 25;
            this.roundGroupBox2.Size = new System.Drawing.Size(380, 175);
            this.roundGroupBox2.TabIndex = 29;
            this.roundGroupBox2.TabStop = false;
            this.roundGroupBox2.Text = "Watch Folders";
            this.roundGroupBox2.TitleBackColor = System.Drawing.Color.Red;
            this.roundGroupBox2.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roundGroupBox2.TitleForeColor = System.Drawing.Color.White;
            this.roundGroupBox2.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Percent60;
            // 
            // lbWatchFolders
            // 
            this.lbWatchFolders.BackColor = System.Drawing.Color.White;
            this.lbWatchFolders.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbWatchFolders.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lbWatchFolders.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbWatchFolders.FormattingEnabled = true;
            this.lbWatchFolders.Location = new System.Drawing.Point(11, 36);
            this.lbWatchFolders.Name = "lbWatchFolders";
            this.lbWatchFolders.Size = new System.Drawing.Size(359, 104);
            this.lbWatchFolders.TabIndex = 1;
            this.lbWatchFolders.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.LbWatchFolders_DrawItem);
            this.lbWatchFolders.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LbWatchFolders_MouseDown);
            // 
            // roundGroupBox1
            // 
            this.roundGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.roundGroupBox1.Controls.Add(this.pbUploadPlaylistsInfo);
            this.roundGroupBox1.Controls.Add(this.label1);
            this.roundGroupBox1.Controls.Add(this.cbAlsoUploadPlaylists);
            this.roundGroupBox1.Controls.Add(this.lblStartWithWindows);
            this.roundGroupBox1.Controls.Add(this.tbThrottleSpeed);
            this.roundGroupBox1.Controls.Add(this.lblSendDiagnosticData);
            this.roundGroupBox1.Controls.Add(this.lblThrottleUploadSpeed);
            this.roundGroupBox1.Controls.Add(this.cbSendErrorLogsToSource);
            this.roundGroupBox1.Controls.Add(this.cbStartWithWindows);
            this.roundGroupBox1.Controls.Add(this.label3);
            this.roundGroupBox1.Location = new System.Drawing.Point(432, 97);
            this.roundGroupBox1.Name = "roundGroupBox1";
            this.roundGroupBox1.Radious = 25;
            this.roundGroupBox1.Size = new System.Drawing.Size(263, 147);
            this.roundGroupBox1.TabIndex = 28;
            this.roundGroupBox1.TabStop = false;
            this.roundGroupBox1.Text = "Options";
            this.roundGroupBox1.TitleBackColor = System.Drawing.Color.Red;
            this.roundGroupBox1.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.roundGroupBox1.TitleForeColor = System.Drawing.Color.White;
            this.roundGroupBox1.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Percent60;
            // 
            // pbUploadPlaylistsInfo
            // 
            this.pbUploadPlaylistsInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbUploadPlaylistsInfo.Image = global::YTMusicUploader.Properties.Resources.info_up;
            this.pbUploadPlaylistsInfo.Location = new System.Drawing.Point(193, 61);
            this.pbUploadPlaylistsInfo.Name = "pbUploadPlaylistsInfo";
            this.pbUploadPlaylistsInfo.Size = new System.Drawing.Size(15, 15);
            this.pbUploadPlaylistsInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbUploadPlaylistsInfo.TabIndex = 30;
            this.pbUploadPlaylistsInfo.TabStop = false;
            this.pbUploadPlaylistsInfo.Click += new System.EventHandler(this.PbUploadPlaylistsInfo_Click);
            this.pbUploadPlaylistsInfo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbUploadPlaylistsInfo_MouseDown);
            this.pbUploadPlaylistsInfo.MouseEnter += new System.EventHandler(this.PbUploadPlaylistsInfo_MouseEnter);
            this.pbUploadPlaylistsInfo.MouseLeave += new System.EventHandler(this.PbUploadPlaylistsInfo_MouseLeave);
            this.pbUploadPlaylistsInfo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PbUploadPlaylistsInfo_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Include Playlists Upload";
            // 
            // cbAlsoUploadPlaylists
            // 
            this.cbAlsoUploadPlaylists.AutoSize = true;
            this.cbAlsoUploadPlaylists.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbAlsoUploadPlaylists.Enabled = false;
            this.cbAlsoUploadPlaylists.Location = new System.Drawing.Point(163, 61);
            this.cbAlsoUploadPlaylists.Name = "cbAlsoUploadPlaylists";
            this.cbAlsoUploadPlaylists.Size = new System.Drawing.Size(26, 15);
            this.cbAlsoUploadPlaylists.Style = MetroFramework.MetroColorStyle.Red;
            this.cbAlsoUploadPlaylists.TabIndex = 28;
            this.cbAlsoUploadPlaylists.Text = " ";
            this.cbAlsoUploadPlaylists.UseSelectable = true;
            this.cbAlsoUploadPlaylists.UseStyleColors = true;
            this.cbAlsoUploadPlaylists.CheckedChanged += new System.EventHandler(this.CbAlsoUploadPlaylists_CheckedChanged);
            // 
            // lblStartWithWindows
            // 
            this.lblStartWithWindows.AutoSize = true;
            this.lblStartWithWindows.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblStartWithWindows.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartWithWindows.Location = new System.Drawing.Point(11, 35);
            this.lblStartWithWindows.Name = "lblStartWithWindows";
            this.lblStartWithWindows.Size = new System.Drawing.Size(109, 13);
            this.lblStartWithWindows.TabIndex = 6;
            this.lblStartWithWindows.Text = "Start with Windows";
            // 
            // tbThrottleSpeed
            // 
            this.tbThrottleSpeed.BackColor = System.Drawing.Color.Transparent;
            this.tbThrottleSpeed.Br = System.Drawing.Color.White;
            this.tbThrottleSpeed.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tbThrottleSpeed.Enabled = false;
            this.tbThrottleSpeed.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.tbThrottleSpeed.ForeColor = System.Drawing.Color.Black;
            this.tbThrottleSpeed.Location = new System.Drawing.Point(163, 112);
            this.tbThrottleSpeed.Name = "tbThrottleSpeed";
            this.tbThrottleSpeed.PasswordChar = '\0';
            this.tbThrottleSpeed.ReadOnly = false;
            this.tbThrottleSpeed.Size = new System.Drawing.Size(44, 25);
            this.tbThrottleSpeed.TabIndex = 3;
            this.tbThrottleSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbThrottleSpeed.TextChanged += new System.EventHandler(this.TbThrottleSpeed_TextChanged);
            this.tbThrottleSpeed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbThrottleSpeed_KeyDown);
            this.tbThrottleSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbThrottleSpeed_KeyPress);
            this.tbThrottleSpeed.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TbThrottleSpeed_KeyDown);
            // 
            // lblSendDiagnosticData
            // 
            this.lblSendDiagnosticData.AutoSize = true;
            this.lblSendDiagnosticData.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblSendDiagnosticData.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSendDiagnosticData.Location = new System.Drawing.Point(11, 90);
            this.lblSendDiagnosticData.Name = "lblSendDiagnosticData";
            this.lblSendDiagnosticData.Size = new System.Drawing.Size(118, 13);
            this.lblSendDiagnosticData.TabIndex = 27;
            this.lblSendDiagnosticData.Text = "Send Diagnostic Data";
            // 
            // lblThrottleUploadSpeed
            // 
            this.lblThrottleUploadSpeed.AutoSize = true;
            this.lblThrottleUploadSpeed.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblThrottleUploadSpeed.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThrottleUploadSpeed.Location = new System.Drawing.Point(11, 118);
            this.lblThrottleUploadSpeed.Name = "lblThrottleUploadSpeed";
            this.lblThrottleUploadSpeed.Size = new System.Drawing.Size(124, 13);
            this.lblThrottleUploadSpeed.TabIndex = 4;
            this.lblThrottleUploadSpeed.Text = "Throttle Upload Speed";
            // 
            // cbSendErrorLogsToSource
            // 
            this.cbSendErrorLogsToSource.AutoSize = true;
            this.cbSendErrorLogsToSource.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbSendErrorLogsToSource.Enabled = false;
            this.cbSendErrorLogsToSource.Location = new System.Drawing.Point(163, 89);
            this.cbSendErrorLogsToSource.Name = "cbSendErrorLogsToSource";
            this.cbSendErrorLogsToSource.Size = new System.Drawing.Size(26, 15);
            this.cbSendErrorLogsToSource.Style = MetroFramework.MetroColorStyle.Red;
            this.cbSendErrorLogsToSource.TabIndex = 26;
            this.cbSendErrorLogsToSource.Text = " ";
            this.cbSendErrorLogsToSource.UseSelectable = true;
            this.cbSendErrorLogsToSource.UseStyleColors = true;
            this.cbSendErrorLogsToSource.CheckedChanged += new System.EventHandler(this.CbSendErrorLogsToSource_CheckedChanged);
            // 
            // cbStartWithWindows
            // 
            this.cbStartWithWindows.AutoSize = true;
            this.cbStartWithWindows.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbStartWithWindows.Enabled = false;
            this.cbStartWithWindows.Location = new System.Drawing.Point(163, 34);
            this.cbStartWithWindows.Name = "cbStartWithWindows";
            this.cbStartWithWindows.Size = new System.Drawing.Size(26, 15);
            this.cbStartWithWindows.Style = MetroFramework.MetroColorStyle.Red;
            this.cbStartWithWindows.TabIndex = 2;
            this.cbStartWithWindows.Text = " ";
            this.cbStartWithWindows.UseSelectable = true;
            this.cbStartWithWindows.CheckedChanged += new System.EventHandler(this.CbStartWithWindows_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(214, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "MB /s";
            // 
            // btnConnectToYoutube
            // 
            this.btnConnectToYoutube.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.btnConnectToYoutube.Active2 = System.Drawing.Color.LightCoral;
            this.btnConnectToYoutube.BackColor = System.Drawing.Color.Transparent;
            this.btnConnectToYoutube.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnectToYoutube.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConnectToYoutube.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnConnectToYoutube.ForeColor = System.Drawing.Color.White;
            this.btnConnectToYoutube.Image = global::YTMusicUploader.Properties.Resources.ytmusic_aicon;
            this.btnConnectToYoutube.Inactive1 = System.Drawing.Color.Red;
            this.btnConnectToYoutube.Inactive2 = System.Drawing.Color.LightCoral;
            this.btnConnectToYoutube.Location = new System.Drawing.Point(442, 324);
            this.btnConnectToYoutube.Name = "btnConnectToYoutube";
            this.btnConnectToYoutube.Radius = 4;
            this.btnConnectToYoutube.Size = new System.Drawing.Size(193, 26);
            this.btnConnectToYoutube.Stroke = true;
            this.btnConnectToYoutube.StrokeColor = System.Drawing.Color.LightCoral;
            this.btnConnectToYoutube.TabIndex = 6;
            this.btnConnectToYoutube.Text = "Connect to YouTube Music";
            this.btnConnectToYoutube.Transparency = false;
            this.btnConnectToYoutube.Click += new System.EventHandler(this.BtnConnectToYouTube_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(736, 453);
            this.Controls.Add(this.pbArtworkIdle);
            this.Controls.Add(this.pbPaused);
            this.Controls.Add(this.lblTrackMeta);
            this.Controls.Add(this.lblAlbumMeta);
            this.Controls.Add(this.lblArtistMeta);
            this.Controls.Add(this.pbLog);
            this.Controls.Add(this.roundGroupBox2);
            this.Controls.Add(this.roundGroupBox1);
            this.Controls.Add(this.pbYtMusicManage);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pbArtwork);
            this.Controls.Add(this.lblUploaded);
            this.Controls.Add(this.lblIssues);
            this.Controls.Add(this.lblUploadedLabel);
            this.Controls.Add(this.lblUploadingMessage);
            this.Controls.Add(this.pnlRemoveFromWatchFolder);
            this.Controls.Add(this.pbConnectedToYoutube);
            this.Controls.Add(this.btnConnectToYoutube);
            this.Controls.Add(this.btnAddWatchFolder);
            this.Controls.Add(this.lblIssuesLabel);
            this.Controls.Add(this.lblDiscoveredFiles);
            this.Controls.Add(this.lblDiscoveredFilesLabel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblSub);
            this.Controls.Add(this.pbAbout);
            this.Controls.Add(this.pbNotConnectedToYoutube);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(736, 453);
            this.MinimumSize = new System.Drawing.Size(736, 453);
            this.Name = "MainForm";
            this.Style = MetroFramework.MetroColorStyle.Red;
            this.Text = "YT Music Uploader";
            this.Theme = MetroFramework.MetroThemeStyle.Default;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbUpdate)).EndInit();
            this.pnlRemoveFromWatchFolder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnRemoveWatchFolder)).EndInit();
            this.TrayContextMenuStrip.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPaused)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbYtMusicManage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbArtworkIdle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbArtwork)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectedToYoutube)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddWatchFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAbout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNotConnectedToYoutube)).EndInit();
            this.roundGroupBox2.ResumeLayout(false);
            this.roundGroupBox1.ResumeLayout(false);
            this.roundGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbUploadPlaylistsInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbAbout;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.ListBox lbWatchFolders;
        private JBToolkit.WinForms.RoundTextBox tbThrottleSpeed;
        private System.Windows.Forms.Label lblThrottleUploadSpeed;
        private MetroFramework.Controls.MetroCheckBox cbStartWithWindows;
        private System.Windows.Forms.Label lblStartWithWindows;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblDiscoveredFilesLabel;
        private System.Windows.Forms.Label lblDiscoveredFiles;
        private System.Windows.Forms.Label lblIssuesLabel;
        private JBToolkit.WinForms.AntiAliasedLabel lblStatus;
        private System.Windows.Forms.PictureBox btnAddWatchFolder;
        private System.Windows.Forms.PictureBox btnRemoveWatchFolder;
        private JBToolkit.WinForms.RoundButton btnConnectToYoutube;
        private System.Windows.Forms.PictureBox pbConnectedToYoutube;
        private System.Windows.Forms.PictureBox pbNotConnectedToYoutube;
        private System.Windows.Forms.Panel pnlRemoveFromWatchFolder;
        private System.Windows.Forms.Label lblUploadingMessage;
        private System.Windows.Forms.Label lblUploadedLabel;
        private Ookii.Dialogs.WinForms.VistaFolderBrowserDialog FolderSelector;
        private JBToolkit.WinForms.AntiAliasedLabel lblVersion;
        private System.Windows.Forms.NotifyIcon TrayIcon;
        private System.Windows.Forms.ContextMenuStrip TrayContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tsmShow;
        private System.Windows.Forms.ToolStripMenuItem tsmQuit;
        private System.Windows.Forms.LinkLabel lblIssues;
        private System.Windows.Forms.LinkLabel lblUploaded;
        private System.Windows.Forms.PictureBox pbArtwork;
        private System.Windows.Forms.PictureBox pbArtworkIdle;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pbYtMusicManage;
        private System.Windows.Forms.PictureBox pbPaused;
        private System.Windows.Forms.Label lblSendDiagnosticData;
        private MetroFramework.Controls.MetroCheckBox cbSendErrorLogsToSource;
        private JBToolkit.WinForms.RoundGroupBox roundGroupBox1;
        private JBToolkit.WinForms.RoundGroupBox roundGroupBox2;
        private System.Windows.Forms.PictureBox pbLog;
        private System.Windows.Forms.PictureBox pbUpdate;
        private System.Windows.Forms.ToolStripMenuItem tsmPauseResume;
        private System.Windows.Forms.Label label1;
        private MetroFramework.Controls.MetroCheckBox cbAlsoUploadPlaylists;
        private System.Windows.Forms.PictureBox pbUploadPlaylistsInfo;
        private System.Windows.Forms.Label lblArtistMeta;
        private System.Windows.Forms.Label lblAlbumMeta;
        private System.Windows.Forms.Label lblTrackMeta;
    }
}

