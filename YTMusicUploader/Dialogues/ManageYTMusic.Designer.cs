namespace YTMusicUploader.Dialogues
{
    partial class ManageYTMusic
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageYTMusic));
            this.lblTitle = new System.Windows.Forms.Label();
            this.tblManageYTMusicUploads = new System.Windows.Forms.TableLayoutPanel();
            this.tvUploads = new System.Windows.Forms.TreeView();
            this.pnlMusicDetailsPanel = new System.Windows.Forms.Panel();
            this.lblArtistTitle = new System.Windows.Forms.Label();
            this.lblUploaded = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblMbId = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblDatabaseExistence = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lblSongTitle = new System.Windows.Forms.Label();
            this.lblAlbumTitle = new System.Windows.Forms.Label();
            this.pbCoverArt = new System.Windows.Forms.PictureBox();
            this.pnlUpdatesPanel = new System.Windows.Forms.Panel();
            this.tbUpdates = new System.Windows.Forms.RichTextBox();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.lblCheckedCount = new System.Windows.Forms.Label();
            this.pbPreloader = new System.Windows.Forms.PictureBox();
            this.pbRefresh = new System.Windows.Forms.PictureBox();
            this.pbIssueLogIcon = new System.Windows.Forms.PictureBox();
            this.tblManageYTMusicUploads.SuspendLayout();
            this.pnlMusicDetailsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCoverArt)).BeginInit();
            this.pnlUpdatesPanel.SuspendLayout();
            this.pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreloader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIssueLogIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(73, 35);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(372, 13);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "See and delete YouTube Music uploads and manage your uploads database.";
            // 
            // tblManageYTMusicUploads
            // 
            this.tblManageYTMusicUploads.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblManageYTMusicUploads.ColumnCount = 2;
            this.tblManageYTMusicUploads.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.0222F));
            this.tblManageYTMusicUploads.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.9778F));
            this.tblManageYTMusicUploads.Controls.Add(this.tvUploads, 0, 0);
            this.tblManageYTMusicUploads.Controls.Add(this.pnlMusicDetailsPanel, 1, 0);
            this.tblManageYTMusicUploads.Controls.Add(this.pnlUpdatesPanel, 1, 2);
            this.tblManageYTMusicUploads.Controls.Add(this.pnlActions, 1, 1);
            this.tblManageYTMusicUploads.Location = new System.Drawing.Point(23, 76);
            this.tblManageYTMusicUploads.Name = "tblManageYTMusicUploads";
            this.tblManageYTMusicUploads.RowCount = 3;
            this.tblManageYTMusicUploads.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 137F));
            this.tblManageYTMusicUploads.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tblManageYTMusicUploads.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblManageYTMusicUploads.Size = new System.Drawing.Size(991, 633);
            this.tblManageYTMusicUploads.TabIndex = 5;
            // 
            // tvUploads
            // 
            this.tvUploads.CheckBoxes = true;
            this.tvUploads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvUploads.Location = new System.Drawing.Point(3, 0);
            this.tvUploads.Margin = new System.Windows.Forms.Padding(3, 0, 20, 0);
            this.tvUploads.Name = "tvUploads";
            this.tblManageYTMusicUploads.SetRowSpan(this.tvUploads, 3);
            this.tvUploads.Size = new System.Drawing.Size(552, 633);
            this.tvUploads.TabIndex = 1;
            this.tvUploads.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.TvUploads_AfterCheck);
            this.tvUploads.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TvUploads_AfterSelect);
            // 
            // pnlMusicDetailsPanel
            // 
            this.pnlMusicDetailsPanel.Controls.Add(this.lblArtistTitle);
            this.pnlMusicDetailsPanel.Controls.Add(this.lblUploaded);
            this.pnlMusicDetailsPanel.Controls.Add(this.label4);
            this.pnlMusicDetailsPanel.Controls.Add(this.lblMbId);
            this.pnlMusicDetailsPanel.Controls.Add(this.label2);
            this.pnlMusicDetailsPanel.Controls.Add(this.lblDatabaseExistence);
            this.pnlMusicDetailsPanel.Controls.Add(this.label1);
            this.pnlMusicDetailsPanel.Controls.Add(this.lblDuration);
            this.pnlMusicDetailsPanel.Controls.Add(this.lblSongTitle);
            this.pnlMusicDetailsPanel.Controls.Add(this.lblAlbumTitle);
            this.pnlMusicDetailsPanel.Controls.Add(this.pbCoverArt);
            this.pnlMusicDetailsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMusicDetailsPanel.Location = new System.Drawing.Point(578, 0);
            this.pnlMusicDetailsPanel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.pnlMusicDetailsPanel.Name = "pnlMusicDetailsPanel";
            this.pnlMusicDetailsPanel.Size = new System.Drawing.Size(410, 134);
            this.pnlMusicDetailsPanel.TabIndex = 2;
            // 
            // lblArtistTitle
            // 
            this.lblArtistTitle.AutoSize = true;
            this.lblArtistTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArtistTitle.Location = new System.Drawing.Point(73, 0);
            this.lblArtistTitle.Name = "lblArtistTitle";
            this.lblArtistTitle.Size = new System.Drawing.Size(103, 13);
            this.lblArtistTitle.TabIndex = 10;
            this.lblArtistTitle.Text = "Nothing selected";
            // 
            // lblUploaded
            // 
            this.lblUploaded.AutoSize = true;
            this.lblUploaded.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUploaded.Location = new System.Drawing.Point(127, 108);
            this.lblUploaded.Name = "lblUploaded";
            this.lblUploaded.Size = new System.Drawing.Size(95, 13);
            this.lblUploaded.TabIndex = 9;
            this.lblUploaded.Text = "23/01/2011 15:33";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(-3, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Uploaded:";
            // 
            // lblMbId
            // 
            this.lblMbId.AutoSize = true;
            this.lblMbId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMbId.Location = new System.Drawing.Point(127, 92);
            this.lblMbId.Name = "lblMbId";
            this.lblMbId.Size = new System.Drawing.Size(156, 13);
            this.lblMbId.TabIndex = 7;
            this.lblMbId.Text = "sdf34-34cfr324rc234-d3cr34r34";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(-3, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "MBID:";
            // 
            // lblDatabaseExistence
            // 
            this.lblDatabaseExistence.AutoSize = true;
            this.lblDatabaseExistence.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatabaseExistence.Location = new System.Drawing.Point(127, 75);
            this.lblDatabaseExistence.Name = "lblDatabaseExistence";
            this.lblDatabaseExistence.Size = new System.Drawing.Size(102, 13);
            this.lblDatabaseExistence.TabIndex = 5;
            this.lblDatabaseExistence.Text = "Exists (ID: 2345234)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-3, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Database Existence:";
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDuration.Location = new System.Drawing.Point(73, 48);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(10, 13);
            this.lblDuration.TabIndex = 3;
            this.lblDuration.Text = "-";
            // 
            // lblSongTitle
            // 
            this.lblSongTitle.AutoSize = true;
            this.lblSongTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSongTitle.Location = new System.Drawing.Point(73, 31);
            this.lblSongTitle.Name = "lblSongTitle";
            this.lblSongTitle.Size = new System.Drawing.Size(10, 13);
            this.lblSongTitle.TabIndex = 2;
            this.lblSongTitle.Text = "-";
            // 
            // lblAlbumTitle
            // 
            this.lblAlbumTitle.AutoSize = true;
            this.lblAlbumTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlbumTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(2)))), ((int)(((byte)(68)))));
            this.lblAlbumTitle.Location = new System.Drawing.Point(73, 15);
            this.lblAlbumTitle.Name = "lblAlbumTitle";
            this.lblAlbumTitle.Size = new System.Drawing.Size(11, 13);
            this.lblAlbumTitle.TabIndex = 1;
            this.lblAlbumTitle.Text = "-";
            // 
            // pbCoverArt
            // 
            this.pbCoverArt.Image = global::YTMusicUploader.Properties.Resources.default_artwork_60;
            this.pbCoverArt.Location = new System.Drawing.Point(0, 0);
            this.pbCoverArt.Name = "pbCoverArt";
            this.pbCoverArt.Size = new System.Drawing.Size(60, 60);
            this.pbCoverArt.TabIndex = 0;
            this.pbCoverArt.TabStop = false;
            // 
            // pnlUpdatesPanel
            // 
            this.pnlUpdatesPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.pnlUpdatesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUpdatesPanel.Controls.Add(this.tbUpdates);
            this.pnlUpdatesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUpdatesPanel.Location = new System.Drawing.Point(578, 194);
            this.pnlUpdatesPanel.Margin = new System.Windows.Forms.Padding(3, 3, 2, 0);
            this.pnlUpdatesPanel.Name = "pnlUpdatesPanel";
            this.pnlUpdatesPanel.Padding = new System.Windows.Forms.Padding(4);
            this.pnlUpdatesPanel.Size = new System.Drawing.Size(411, 439);
            this.pnlUpdatesPanel.TabIndex = 3;
            // 
            // tbUpdates
            // 
            this.tbUpdates.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.tbUpdates.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbUpdates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbUpdates.ForeColor = System.Drawing.Color.White;
            this.tbUpdates.Location = new System.Drawing.Point(4, 4);
            this.tbUpdates.Margin = new System.Windows.Forms.Padding(8);
            this.tbUpdates.Name = "tbUpdates";
            this.tbUpdates.Size = new System.Drawing.Size(401, 429);
            this.tbUpdates.TabIndex = 0;
            this.tbUpdates.Text = "";
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.lblCheckedCount);
            this.pnlActions.Controls.Add(this.pbPreloader);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlActions.Location = new System.Drawing.Point(575, 137);
            this.pnlActions.Margin = new System.Windows.Forms.Padding(0);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(416, 54);
            this.pnlActions.TabIndex = 4;
            // 
            // lblCheckedCount
            // 
            this.lblCheckedCount.AutoSize = true;
            this.lblCheckedCount.Location = new System.Drawing.Point(0, 40);
            this.lblCheckedCount.Name = "lblCheckedCount";
            this.lblCheckedCount.Size = new System.Drawing.Size(94, 13);
            this.lblCheckedCount.TabIndex = 1;
            this.lblCheckedCount.Text = "0 Tracks checked";
            // 
            // pbPreloader
            // 
            this.pbPreloader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pbPreloader.Image = global::YTMusicUploader.Properties.Resources.preloader_cricle;
            this.pbPreloader.Location = new System.Drawing.Point(392, 30);
            this.pbPreloader.Name = "pbPreloader";
            this.pbPreloader.Size = new System.Drawing.Size(22, 22);
            this.pbPreloader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbPreloader.TabIndex = 0;
            this.pbPreloader.TabStop = false;
            // 
            // pbRefresh
            // 
            this.pbRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbRefresh.Image = global::YTMusicUploader.Properties.Resources.refresh;
            this.pbRefresh.Location = new System.Drawing.Point(989, 38);
            this.pbRefresh.Name = "pbRefresh";
            this.pbRefresh.Size = new System.Drawing.Size(25, 25);
            this.pbRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbRefresh.TabIndex = 4;
            this.pbRefresh.TabStop = false;
            this.pbRefresh.Click += new System.EventHandler(this.PbRefresh_Click);
            this.pbRefresh.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbRefresh_MouseDown);
            this.pbRefresh.MouseEnter += new System.EventHandler(this.PbRefresh_MouseEnter);
            this.pbRefresh.MouseLeave += new System.EventHandler(this.PbRefresh_MouseLeave);
            this.pbRefresh.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PbRefresh_MouseUp);
            // 
            // pbIssueLogIcon
            // 
            this.pbIssueLogIcon.Image = global::YTMusicUploader.Properties.Resources.ytmusic_manage_40px;
            this.pbIssueLogIcon.Location = new System.Drawing.Point(23, 20);
            this.pbIssueLogIcon.Name = "pbIssueLogIcon";
            this.pbIssueLogIcon.Size = new System.Drawing.Size(38, 40);
            this.pbIssueLogIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbIssueLogIcon.TabIndex = 2;
            this.pbIssueLogIcon.TabStop = false;
            // 
            // ManageYTMusic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 732);
            this.Controls.Add(this.tblManageYTMusicUploads);
            this.Controls.Add(this.pbRefresh);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pbIssueLogIcon);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "ManageYTMusic";
            this.Style = MetroFramework.MetroColorStyle.Red;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ManageYTMusic_FormClosing);
            this.Load += new System.EventHandler(this.ManageYTMusic_Load);
            this.tblManageYTMusicUploads.ResumeLayout(false);
            this.pnlMusicDetailsPanel.ResumeLayout(false);
            this.pnlMusicDetailsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCoverArt)).EndInit();
            this.pnlUpdatesPanel.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreloader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbIssueLogIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbIssueLogIcon;
        private System.Windows.Forms.PictureBox pbRefresh;
        private System.Windows.Forms.TableLayoutPanel tblManageYTMusicUploads;
        private System.Windows.Forms.TreeView tvUploads;
        private System.Windows.Forms.PictureBox pbPreloader;
        private System.Windows.Forms.Panel pnlMusicDetailsPanel;
        private System.Windows.Forms.Label lblUploaded;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblMbId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDatabaseExistence;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label lblSongTitle;
        private System.Windows.Forms.Label lblAlbumTitle;
        private System.Windows.Forms.PictureBox pbCoverArt;
        private System.Windows.Forms.Panel pnlUpdatesPanel;
        private System.Windows.Forms.RichTextBox tbUpdates;
        private System.Windows.Forms.Label lblArtistTitle;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Label lblCheckedCount;
    }
}