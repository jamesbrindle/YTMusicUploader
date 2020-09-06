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
            this.lbWatchFolders = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbStartWithWindows = new MetroFramework.Controls.MetroCheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblVersion = new JBToolkit.WinForms.AntiAliasedLabel();
            this.lblStatus = new JBToolkit.WinForms.AntiAliasedLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblDiscoveredFiles = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlRemoveFromWatchFolder = new System.Windows.Forms.Panel();
            this.btnRemoveWatchFolder = new System.Windows.Forms.PictureBox();
            this.lblUploadingMessage = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.FolderSelector = new Ookii.Dialogs.WinForms.VistaFolderBrowserDialog();
            this.TrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmShow = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.lblIssues = new System.Windows.Forms.LinkLabel();
            this.lblUploaded = new System.Windows.Forms.LinkLabel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbThrottleSpeed = new JBToolkit.WinForms.RoundTextBox();
            this.pbYtMusicManage = new System.Windows.Forms.PictureBox();
            this.pbArtworkIdle = new System.Windows.Forms.PictureBox();
            this.pbArtwork = new System.Windows.Forms.PictureBox();
            this.pbConnectedToYoutube = new System.Windows.Forms.PictureBox();
            this.btnConnectToYoutube = new JBToolkit.WinForms.RoundButton();
            this.btnAddWatchFolder = new System.Windows.Forms.PictureBox();
            this.pbAbout = new System.Windows.Forms.PictureBox();
            this.pbNotConnectedToYoutube = new System.Windows.Forms.PictureBox();
            this.pbPaused = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.pnlRemoveFromWatchFolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnRemoveWatchFolder)).BeginInit();
            this.TrayContextMenuStrip.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbYtMusicManage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbArtworkIdle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbArtwork)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectedToYoutube)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddWatchFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAbout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNotConnectedToYoutube)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPaused)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSub
            // 
            this.lblSub.AutoSize = true;
            this.lblSub.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSub.Location = new System.Drawing.Point(24, 62);
            this.lblSub.Name = "lblSub";
            this.lblSub.Size = new System.Drawing.Size(306, 13);
            this.lblSub.TabIndex = 1;
            this.lblSub.Text = "Automatically upload your music library to YouTube Music.";
            // 
            // lbWatchFolders
            // 
            this.lbWatchFolders.FormattingEnabled = true;
            this.lbWatchFolders.Location = new System.Drawing.Point(27, 95);
            this.lbWatchFolders.Name = "lbWatchFolders";
            this.lbWatchFolders.Size = new System.Drawing.Size(391, 160);
            this.lbWatchFolders.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(442, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Throttle Upload Speed";
            // 
            // cbStartWithWindows
            // 
            this.cbStartWithWindows.AutoSize = true;
            this.cbStartWithWindows.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbStartWithWindows.Location = new System.Drawing.Point(599, 103);
            this.cbStartWithWindows.Name = "cbStartWithWindows";
            this.cbStartWithWindows.Size = new System.Drawing.Size(26, 15);
            this.cbStartWithWindows.TabIndex = 2;
            this.cbStartWithWindows.Text = " ";
            this.cbStartWithWindows.UseSelectable = true;
            this.cbStartWithWindows.CheckedChanged += new System.EventHandler(this.CbStartWithWindows_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(442, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Start with Windows";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(661, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "MB /s";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Controls.Add(this.lblVersion);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Location = new System.Drawing.Point(0, 363);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(735, 29);
            this.panel1.TabIndex = 8;
            // 
            // lblVersion
            // 
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.White;
            this.lblVersion.Location = new System.Drawing.Point(680, 1);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(35, 20);
            this.lblVersion.TabIndex = 14;
            this.lblVersion.Text = "v1.3.4";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(11, 1);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(574, 20);
            this.lblStatus.TabIndex = 13;
            this.lblStatus.Text = "Not running";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(442, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Discovered Files";
            // 
            // lblDiscoveredFiles
            // 
            this.lblDiscoveredFiles.AutoSize = true;
            this.lblDiscoveredFiles.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiscoveredFiles.Location = new System.Drawing.Point(598, 167);
            this.lblDiscoveredFiles.Name = "lblDiscoveredFiles";
            this.lblDiscoveredFiles.Size = new System.Drawing.Size(13, 13);
            this.lblDiscoveredFiles.TabIndex = 10;
            this.lblDiscoveredFiles.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(442, 196);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Issues";
            // 
            // pnlRemoveFromWatchFolder
            // 
            this.pnlRemoveFromWatchFolder.Controls.Add(this.btnRemoveWatchFolder);
            this.pnlRemoveFromWatchFolder.Location = new System.Drawing.Point(397, 263);
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
            this.lblUploadingMessage.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUploadingMessage.Location = new System.Drawing.Point(94, 300);
            this.lblUploadingMessage.Name = "lblUploadingMessage";
            this.lblUploadingMessage.Size = new System.Drawing.Size(608, 40);
            this.lblUploadingMessage.TabIndex = 19;
            this.lblUploadingMessage.Text = "Idle";
            this.lblUploadingMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(442, 224);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Uploaded";
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
            this.tsmQuit});
            this.TrayContextMenuStrip.Name = "TrayContextMenuStrip";
            this.TrayContextMenuStrip.Size = new System.Drawing.Size(104, 48);
            // 
            // tsmShow
            // 
            this.tsmShow.Image = global::YTMusicUploader.Properties.Resources.show;
            this.tsmShow.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmShow.Name = "tsmShow";
            this.tsmShow.Size = new System.Drawing.Size(103, 22);
            this.tsmShow.Text = "Show";
            // 
            // tsmQuit
            // 
            this.tsmQuit.Image = global::YTMusicUploader.Properties.Resources.quit;
            this.tsmQuit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmQuit.Name = "tsmQuit";
            this.tsmQuit.Size = new System.Drawing.Size(103, 22);
            this.tsmQuit.Text = "Quit";
            // 
            // lblIssues
            // 
            this.lblIssues.AutoSize = true;
            this.lblIssues.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lblIssues.Location = new System.Drawing.Point(598, 196);
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
            this.lblUploaded.Location = new System.Drawing.Point(598, 224);
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
            this.pnlHeader.Location = new System.Drawing.Point(24, 21);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(198, 38);
            this.pnlHeader.TabIndex = 23;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::YTMusicUploader.Properties.Resources.Header;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(198, 38);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tbThrottleSpeed
            // 
            this.tbThrottleSpeed.BackColor = System.Drawing.Color.Transparent;
            this.tbThrottleSpeed.Br = System.Drawing.Color.White;
            this.tbThrottleSpeed.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.tbThrottleSpeed.ForeColor = System.Drawing.Color.Black;
            this.tbThrottleSpeed.Location = new System.Drawing.Point(599, 130);
            this.tbThrottleSpeed.Name = "tbThrottleSpeed";
            this.tbThrottleSpeed.PasswordChar = '\0';
            this.tbThrottleSpeed.ReadOnly = false;
            this.tbThrottleSpeed.Size = new System.Drawing.Size(44, 25);
            this.tbThrottleSpeed.TabIndex = 3;
            this.tbThrottleSpeed.Text = "∞";
            this.tbThrottleSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.tbThrottleSpeed.TextChanged += new System.EventHandler(this.TbThrottleSpeed_TextChanged);
            this.tbThrottleSpeed.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbThrottleSpeed_KeyDown);
            this.tbThrottleSpeed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TbThrottleSpeed_KeyPress);
            this.tbThrottleSpeed.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TbThrottleSpeed_KeyDown);
            // 
            // pbYtMusicManage
            // 
            this.pbYtMusicManage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbYtMusicManage.Enabled = false;
            this.pbYtMusicManage.Image = global::YTMusicUploader.Properties.Resources.ytmusic_manage_disabled;
            this.pbYtMusicManage.Location = new System.Drawing.Point(614, 47);
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
            this.pbArtworkIdle.Image = global::YTMusicUploader.Properties.Resources.idle;
            this.pbArtworkIdle.Location = new System.Drawing.Point(27, 296);
            this.pbArtworkIdle.Name = "pbArtworkIdle";
            this.pbArtworkIdle.Size = new System.Drawing.Size(50, 50);
            this.pbArtworkIdle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbArtworkIdle.TabIndex = 22;
            this.pbArtworkIdle.TabStop = false;
            // 
            // pbArtwork
            // 
            this.pbArtwork.Location = new System.Drawing.Point(27, 296);
            this.pbArtwork.Name = "pbArtwork";
            this.pbArtwork.Size = new System.Drawing.Size(50, 50);
            this.pbArtwork.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbArtwork.TabIndex = 21;
            this.pbArtwork.TabStop = false;
            this.pbArtwork.Visible = false;
            // 
            // pbConnectedToYoutube
            // 
            this.pbConnectedToYoutube.Image = global::YTMusicUploader.Properties.Resources.tick;
            this.pbConnectedToYoutube.Location = new System.Drawing.Point(634, 263);
            this.pbConnectedToYoutube.Name = "pbConnectedToYoutube";
            this.pbConnectedToYoutube.Size = new System.Drawing.Size(14, 14);
            this.pbConnectedToYoutube.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbConnectedToYoutube.TabIndex = 16;
            this.pbConnectedToYoutube.TabStop = false;
            this.pbConnectedToYoutube.Visible = false;
            // 
            // btnConnectToYoutube
            // 
            this.btnConnectToYoutube.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.btnConnectToYoutube.Active2 = System.Drawing.Color.LightCoral;
            this.btnConnectToYoutube.BackColor = System.Drawing.Color.Transparent;
            this.btnConnectToYoutube.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConnectToYoutube.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnConnectToYoutube.ForeColor = System.Drawing.Color.White;
            this.btnConnectToYoutube.Image = global::YTMusicUploader.Properties.Resources.ytmusic_aicon;
            this.btnConnectToYoutube.Inactive1 = System.Drawing.Color.Red;
            this.btnConnectToYoutube.Inactive2 = System.Drawing.Color.LightCoral;
            this.btnConnectToYoutube.Location = new System.Drawing.Point(445, 257);
            this.btnConnectToYoutube.Name = "btnConnectToYoutube";
            this.btnConnectToYoutube.Radius = 4;
            this.btnConnectToYoutube.Size = new System.Drawing.Size(180, 26);
            this.btnConnectToYoutube.Stroke = true;
            this.btnConnectToYoutube.StrokeColor = System.Drawing.Color.LightCoral;
            this.btnConnectToYoutube.TabIndex = 6;
            this.btnConnectToYoutube.Text = "Connect to YouTube Music";
            this.btnConnectToYoutube.Transparency = false;
            this.btnConnectToYoutube.Click += new System.EventHandler(this.BtnConnectToYouTube_Click);
            // 
            // btnAddWatchFolder
            // 
            this.btnAddWatchFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddWatchFolder.Image = global::YTMusicUploader.Properties.Resources.plus;
            this.btnAddWatchFolder.Location = new System.Drawing.Point(369, 263);
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
            this.pbAbout.Location = new System.Drawing.Point(660, 39);
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
            this.pbNotConnectedToYoutube.Image = global::YTMusicUploader.Properties.Resources.cross;
            this.pbNotConnectedToYoutube.Location = new System.Drawing.Point(634, 265);
            this.pbNotConnectedToYoutube.Name = "pbNotConnectedToYoutube";
            this.pbNotConnectedToYoutube.Size = new System.Drawing.Size(12, 12);
            this.pbNotConnectedToYoutube.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbNotConnectedToYoutube.TabIndex = 17;
            this.pbNotConnectedToYoutube.TabStop = false;
            this.pbNotConnectedToYoutube.Visible = false;
            // 
            // pbPaused
            // 
            this.pbPaused.Image = global::YTMusicUploader.Properties.Resources.paused;
            this.pbPaused.Location = new System.Drawing.Point(27, 296);
            this.pbPaused.Name = "pbPaused";
            this.pbPaused.Size = new System.Drawing.Size(50, 50);
            this.pbPaused.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbPaused.TabIndex = 25;
            this.pbPaused.TabStop = false;
            this.pbPaused.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 396);
            this.Controls.Add(this.pbPaused);
            this.Controls.Add(this.pbYtMusicManage);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pbArtworkIdle);
            this.Controls.Add(this.pbArtwork);
            this.Controls.Add(this.lblUploaded);
            this.Controls.Add(this.lblIssues);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblUploadingMessage);
            this.Controls.Add(this.pnlRemoveFromWatchFolder);
            this.Controls.Add(this.pbConnectedToYoutube);
            this.Controls.Add(this.btnConnectToYoutube);
            this.Controls.Add(this.btnAddWatchFolder);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblDiscoveredFiles);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbStartWithWindows);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbThrottleSpeed);
            this.Controls.Add(this.lbWatchFolders);
            this.Controls.Add(this.lblSub);
            this.Controls.Add(this.pbAbout);
            this.Controls.Add(this.pbNotConnectedToYoutube);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(736, 396);
            this.MinimumSize = new System.Drawing.Size(736, 396);
            this.Name = "MainForm";
            this.Style = MetroFramework.MetroColorStyle.Red;
            this.Text = "YT Music Uploader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.pnlRemoveFromWatchFolder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnRemoveWatchFolder)).EndInit();
            this.TrayContextMenuStrip.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbYtMusicManage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbArtworkIdle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbArtwork)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectedToYoutube)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddWatchFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAbout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNotConnectedToYoutube)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPaused)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbAbout;
        private System.Windows.Forms.Label lblSub;
        private System.Windows.Forms.ListBox lbWatchFolders;
        private JBToolkit.WinForms.RoundTextBox tbThrottleSpeed;
        private System.Windows.Forms.Label label1;
        private MetroFramework.Controls.MetroCheckBox cbStartWithWindows;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDiscoveredFiles;
        private System.Windows.Forms.Label label7;
        private JBToolkit.WinForms.AntiAliasedLabel lblStatus;
        private System.Windows.Forms.PictureBox btnAddWatchFolder;
        private System.Windows.Forms.PictureBox btnRemoveWatchFolder;
        private JBToolkit.WinForms.RoundButton btnConnectToYoutube;
        private System.Windows.Forms.PictureBox pbConnectedToYoutube;
        private System.Windows.Forms.PictureBox pbNotConnectedToYoutube;
        private System.Windows.Forms.Panel pnlRemoveFromWatchFolder;
        private System.Windows.Forms.Label lblUploadingMessage;
        private System.Windows.Forms.Label label8;
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
    }
}

