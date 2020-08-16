﻿namespace YTMusicUploader
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pbAbout = new System.Windows.Forms.PictureBox();
            this.lblSub = new System.Windows.Forms.Label();
            this.lbWatchFolders = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbStartWithWindows = new MetroFramework.Controls.MetroCheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStatus = new JBToolkit.WinForms.AntiAliasedLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblDiscoveredFiles = new System.Windows.Forms.Label();
            this.lblIssues = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnAddWatchFolder = new System.Windows.Forms.PictureBox();
            this.btnRemoveWatchFolder = new System.Windows.Forms.PictureBox();
            this.pbConnectedToYoutube = new System.Windows.Forms.PictureBox();
            this.pbNotConnectedToYoutube = new System.Windows.Forms.PictureBox();
            this.pnlRemoveFromWatchFolder = new System.Windows.Forms.Panel();
            this.lblUploadingMessage = new System.Windows.Forms.Label();
            this.lblUploaded = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnConnectToYoutube = new JBToolkit.WinForms.RoundButton();
            this.tbThrottleSpeed = new JBToolkit.WinForms.RoundTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbAbout)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnAddWatchFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRemoveWatchFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectedToYoutube)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNotConnectedToYoutube)).BeginInit();
            this.pnlRemoveFromWatchFolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbAbout
            // 
            this.pbAbout.Image = global::YTMusicUploader.Properties.Resources.yt_logo;
            this.pbAbout.Location = new System.Drawing.Point(662, 39);
            this.pbAbout.Name = "pbAbout";
            this.pbAbout.Size = new System.Drawing.Size(40, 40);
            this.pbAbout.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbAbout.TabIndex = 0;
            this.pbAbout.TabStop = false;
            // 
            // lblSub
            // 
            this.lblSub.AutoSize = true;
            this.lblSub.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSub.Location = new System.Drawing.Point(23, 62);
            this.lblSub.Name = "lblSub";
            this.lblSub.Size = new System.Drawing.Size(302, 13);
            this.lblSub.TabIndex = 1;
            this.lblSub.Text = "Automatically upload your music libary to YouTube Music.";
            // 
            // lbWatchFolders
            // 
            this.lbWatchFolders.FormattingEnabled = true;
            this.lbWatchFolders.Location = new System.Drawing.Point(27, 95);
            this.lbWatchFolders.Name = "lbWatchFolders";
            this.lbWatchFolders.Size = new System.Drawing.Size(391, 160);
            this.lbWatchFolders.TabIndex = 2;
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
            this.cbStartWithWindows.Location = new System.Drawing.Point(599, 103);
            this.cbStartWithWindows.Name = "cbStartWithWindows";
            this.cbStartWithWindows.Size = new System.Drawing.Size(26, 15);
            this.cbStartWithWindows.TabIndex = 5;
            this.cbStartWithWindows.Text = " ";
            this.cbStartWithWindows.UseSelectable = true;
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
            this.label3.Text = "mb /s";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Location = new System.Drawing.Point(0, 363);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(735, 29);
            this.panel1.TabIndex = 8;
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(11, 1);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(702, 20);
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
            // lblIssues
            // 
            this.lblIssues.AutoSize = true;
            this.lblIssues.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIssues.Location = new System.Drawing.Point(598, 196);
            this.lblIssues.Name = "lblIssues";
            this.lblIssues.Size = new System.Drawing.Size(13, 13);
            this.lblIssues.TabIndex = 12;
            this.lblIssues.Text = "0";
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
            // btnAddWatchFolder
            // 
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
            // btnRemoveWatchFolder
            // 
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
            // pbConnectedToYoutube
            // 
            this.pbConnectedToYoutube.Image = global::YTMusicUploader.Properties.Resources.tick;
            this.pbConnectedToYoutube.Location = new System.Drawing.Point(620, 261);
            this.pbConnectedToYoutube.Name = "pbConnectedToYoutube";
            this.pbConnectedToYoutube.Size = new System.Drawing.Size(16, 16);
            this.pbConnectedToYoutube.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbConnectedToYoutube.TabIndex = 16;
            this.pbConnectedToYoutube.TabStop = false;
            this.pbConnectedToYoutube.Visible = false;
            // 
            // pbNotConnectedToYoutube
            // 
            this.pbNotConnectedToYoutube.Image = global::YTMusicUploader.Properties.Resources.cross;
            this.pbNotConnectedToYoutube.Location = new System.Drawing.Point(623, 263);
            this.pbNotConnectedToYoutube.Name = "pbNotConnectedToYoutube";
            this.pbNotConnectedToYoutube.Size = new System.Drawing.Size(12, 12);
            this.pbNotConnectedToYoutube.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbNotConnectedToYoutube.TabIndex = 17;
            this.pbNotConnectedToYoutube.TabStop = false;
            this.pbNotConnectedToYoutube.Visible = false;
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
            // lblUploadingMessage
            // 
            this.lblUploadingMessage.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUploadingMessage.Location = new System.Drawing.Point(24, 302);
            this.lblUploadingMessage.Name = "lblUploadingMessage";
            this.lblUploadingMessage.Size = new System.Drawing.Size(689, 40);
            this.lblUploadingMessage.TabIndex = 19;
            this.lblUploadingMessage.Text = "Uploading: N/A";
            // 
            // lblUploaded
            // 
            this.lblUploaded.AutoSize = true;
            this.lblUploaded.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUploaded.Location = new System.Drawing.Point(598, 224);
            this.lblUploaded.Name = "lblUploaded";
            this.lblUploaded.Size = new System.Drawing.Size(13, 13);
            this.lblUploaded.TabIndex = 21;
            this.lblUploaded.Text = "0";
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
            // btnConnectToYoutube
            // 
            this.btnConnectToYoutube.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.btnConnectToYoutube.Active2 = System.Drawing.Color.LightCoral;
            this.btnConnectToYoutube.BackColor = System.Drawing.Color.Transparent;
            this.btnConnectToYoutube.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnConnectToYoutube.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.btnConnectToYoutube.ForeColor = System.Drawing.Color.White;
            this.btnConnectToYoutube.Image = null;
            this.btnConnectToYoutube.Inactive1 = System.Drawing.Color.Red;
            this.btnConnectToYoutube.Inactive2 = System.Drawing.Color.LightCoral;
            this.btnConnectToYoutube.Location = new System.Drawing.Point(445, 257);
            this.btnConnectToYoutube.Name = "btnConnectToYoutube";
            this.btnConnectToYoutube.Radius = 4;
            this.btnConnectToYoutube.Size = new System.Drawing.Size(166, 24);
            this.btnConnectToYoutube.Stroke = true;
            this.btnConnectToYoutube.StrokeColor = System.Drawing.Color.LightCoral;
            this.btnConnectToYoutube.TabIndex = 15;
            this.btnConnectToYoutube.Text = "Connect to YouTube Music";
            this.btnConnectToYoutube.Transparency = false;
            this.btnConnectToYoutube.Click += new System.EventHandler(this.BtnConnectToYoutube_Click);
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
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 396);
            this.Controls.Add(this.lblUploaded);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblUploadingMessage);
            this.Controls.Add(this.pnlRemoveFromWatchFolder);
            this.Controls.Add(this.pbConnectedToYoutube);
            this.Controls.Add(this.btnConnectToYoutube);
            this.Controls.Add(this.btnAddWatchFolder);
            this.Controls.Add(this.lblIssues);
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
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbAbout)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnAddWatchFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnRemoveWatchFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectedToYoutube)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNotConnectedToYoutube)).EndInit();
            this.pnlRemoveFromWatchFolder.ResumeLayout(false);
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
        private System.Windows.Forms.Label lblIssues;
        private System.Windows.Forms.Label label7;
        private JBToolkit.WinForms.AntiAliasedLabel lblStatus;
        private System.Windows.Forms.PictureBox btnAddWatchFolder;
        private System.Windows.Forms.PictureBox btnRemoveWatchFolder;
        private JBToolkit.WinForms.RoundButton btnConnectToYoutube;
        private System.Windows.Forms.PictureBox pbConnectedToYoutube;
        private System.Windows.Forms.PictureBox pbNotConnectedToYoutube;
        private System.Windows.Forms.Panel pnlRemoveFromWatchFolder;
        private System.Windows.Forms.Label lblUploadingMessage;
        private System.Windows.Forms.Label lblUploaded;
        private System.Windows.Forms.Label label8;
    }
}
