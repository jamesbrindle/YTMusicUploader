namespace YTMusicUploader.Dialogues
{
    partial class ConnectToYTMusic
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectToYTMusic));
            this.lblSignInMessage = new System.Windows.Forms.Label();
            this.lblConnectSuccess = new System.Windows.Forms.Label();
            this.pbConnectSuccess = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pbPreloader = new System.Windows.Forms.PictureBox();
            this.lnkMoreInfo = new System.Windows.Forms.LinkLabel();
            this.lnkVideo = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbCookieValue = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectSuccess)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreloader)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSignInMessage
            // 
            this.lblSignInMessage.AutoSize = true;
            this.lblSignInMessage.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblSignInMessage.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSignInMessage.Location = new System.Drawing.Point(27, 34);
            this.lblSignInMessage.Name = "lblSignInMessage";
            this.lblSignInMessage.Size = new System.Drawing.Size(340, 13);
            this.lblSignInMessage.TabIndex = 0;
            this.lblSignInMessage.Text = "Sign into YouTube Music and Retrieve the Authentication Cookie";
            // 
            // lblConnectSuccess
            // 
            this.lblConnectSuccess.AutoSize = true;
            this.lblConnectSuccess.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblConnectSuccess.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectSuccess.Location = new System.Drawing.Point(82, 34);
            this.lblConnectSuccess.Name = "lblConnectSuccess";
            this.lblConnectSuccess.Size = new System.Drawing.Size(233, 13);
            this.lblConnectSuccess.TabIndex = 3;
            this.lblConnectSuccess.Text = "Connected! You can now close this window.";
            this.lblConnectSuccess.Visible = false;
            // 
            // pbConnectSuccess
            // 
            this.pbConnectSuccess.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pbConnectSuccess.Image = global::YTMusicUploader.Properties.Resources.success;
            this.pbConnectSuccess.Location = new System.Drawing.Point(23, 20);
            this.pbConnectSuccess.Name = "pbConnectSuccess";
            this.pbConnectSuccess.Size = new System.Drawing.Size(40, 40);
            this.pbConnectSuccess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbConnectSuccess.TabIndex = 2;
            this.pbConnectSuccess.TabStop = false;
            this.pbConnectSuccess.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.pbPreloader);
            this.panel1.Controls.Add(this.lnkMoreInfo);
            this.panel1.Controls.Add(this.lnkVideo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tbCookieValue);
            this.panel1.Location = new System.Drawing.Point(23, 66);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(715, 527);
            this.panel1.TabIndex = 4;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(362, 144);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(322, 19);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Text = "Validating Cookie Value...";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblStatus.Visible = false;
            // 
            // pbPreloader
            // 
            this.pbPreloader.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbPreloader.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.pbPreloader.Image = global::YTMusicUploader.Properties.Resources.preloader_cricle;
            this.pbPreloader.Location = new System.Drawing.Point(690, 139);
            this.pbPreloader.Name = "pbPreloader";
            this.pbPreloader.Size = new System.Drawing.Size(22, 22);
            this.pbPreloader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbPreloader.TabIndex = 5;
            this.pbPreloader.TabStop = false;
            this.pbPreloader.Visible = false;
            // 
            // lnkMoreInfo
            // 
            this.lnkMoreInfo.AutoSize = true;
            this.lnkMoreInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkMoreInfo.Location = new System.Drawing.Point(37, 98);
            this.lnkMoreInfo.Name = "lnkMoreInfo";
            this.lnkMoreInfo.Size = new System.Drawing.Size(84, 13);
            this.lnkMoreInfo.TabIndex = 4;
            this.lnkMoreInfo.TabStop = true;
            this.lnkMoreInfo.Text = "More info here";
            this.lnkMoreInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkMoreInfo_LinkClicked);
            // 
            // lnkVideo
            // 
            this.lnkVideo.AutoSize = true;
            this.lnkVideo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkVideo.Location = new System.Drawing.Point(37, 67);
            this.lnkVideo.Name = "lnkVideo";
            this.lnkVideo.Size = new System.Drawing.Size(106, 13);
            this.lnkVideo.TabIndex = 3;
            this.lnkVideo.TabStop = true;
            this.lnkVideo.Text = "Click here for video";
            this.lnkVideo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkVideo_LinkClicked);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(705, 41);
            this.label2.TabIndex = 2;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Paste your authentication cookie here:";
            // 
            // tbCookieValue
            // 
            this.tbCookieValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCookieValue.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCookieValue.Location = new System.Drawing.Point(0, 166);
            this.tbCookieValue.Multiline = true;
            this.tbCookieValue.Name = "tbCookieValue";
            this.tbCookieValue.Size = new System.Drawing.Size(715, 350);
            this.tbCookieValue.TabIndex = 0;
            this.tbCookieValue.TextChanged += new System.EventHandler(this.CookieValueTextBox_TextChanged);
            // 
            // ConnectToYTMusic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(776, 618);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblConnectSuccess);
            this.Controls.Add(this.pbConnectSuccess);
            this.Controls.Add(this.lblSignInMessage);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(776, 618);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(776, 618);
            this.Name = "ConnectToYTMusic";
            this.Style = MetroFramework.MetroColorStyle.Red;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConnectToYTMusic_FormClosing);
            this.Load += new System.EventHandler(this.ConnectToYTMusic_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectSuccess)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreloader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSignInMessage;
        private System.Windows.Forms.PictureBox pbConnectSuccess;
        private System.Windows.Forms.Label lblConnectSuccess;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbCookieValue;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.PictureBox pbPreloader;
        private System.Windows.Forms.LinkLabel lnkMoreInfo;
        private System.Windows.Forms.LinkLabel lnkVideo;
        private System.Windows.Forms.Label label2;
    }
}