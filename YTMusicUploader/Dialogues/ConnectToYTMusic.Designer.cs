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
            this.pnlBrowser = new System.Windows.Forms.Panel();
            this.browser = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.lblConnectSuccess = new System.Windows.Forms.Label();
            this.pbConnectSuccess = new System.Windows.Forms.PictureBox();
            this.pnlBrowser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectSuccess)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSignInMessage
            // 
            this.lblSignInMessage.AutoSize = true;
            this.lblSignInMessage.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSignInMessage.Location = new System.Drawing.Point(23, 34);
            this.lblSignInMessage.Name = "lblSignInMessage";
            this.lblSignInMessage.Size = new System.Drawing.Size(328, 13);
            this.lblSignInMessage.TabIndex = 0;
            this.lblSignInMessage.Text = "Sign into YouTube Music to retrieve your authorisation cookie.";
            // 
            // pnlBrowser
            // 
            this.pnlBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBrowser.Controls.Add(this.browser);
            this.pnlBrowser.Location = new System.Drawing.Point(26, 76);
            this.pnlBrowser.MinimumSize = new System.Drawing.Size(600, 400);
            this.pnlBrowser.Name = "pnlBrowser";
            this.pnlBrowser.Size = new System.Drawing.Size(988, 633);
            this.pnlBrowser.TabIndex = 1;
            // 
            // browser
            // 
            this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.MinimumSize = new System.Drawing.Size(600, 400);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(988, 633);
            this.browser.TabIndex = 0;
            this.browser.Text = "YouTube Music";
            this.browser.ZoomFactor = 1D;
            this.browser.CoreWebView2Ready += new System.EventHandler<System.EventArgs>(this.Browser_CoreWebView2Ready);
            // 
            // lblConnectSuccess
            // 
            this.lblConnectSuccess.AutoSize = true;
            this.lblConnectSuccess.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectSuccess.Location = new System.Drawing.Point(75, 34);
            this.lblConnectSuccess.Name = "lblConnectSuccess";
            this.lblConnectSuccess.Size = new System.Drawing.Size(233, 13);
            this.lblConnectSuccess.TabIndex = 3;
            this.lblConnectSuccess.Text = "Connected! You can now close this window.";
            this.lblConnectSuccess.Visible = false;
            // 
            // pbConnectSuccess
            // 
            this.pbConnectSuccess.Image = global::YTMusicUploader.Properties.Resources.success;
            this.pbConnectSuccess.Location = new System.Drawing.Point(23, 20);
            this.pbConnectSuccess.Name = "pbConnectSuccess";
            this.pbConnectSuccess.Size = new System.Drawing.Size(40, 40);
            this.pbConnectSuccess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbConnectSuccess.TabIndex = 2;
            this.pbConnectSuccess.TabStop = false;
            this.pbConnectSuccess.Visible = false;
            // 
            // ConnectToYTMusic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 732);
            this.Controls.Add(this.lblConnectSuccess);
            this.Controls.Add(this.pbConnectSuccess);
            this.Controls.Add(this.pnlBrowser);
            this.Controls.Add(this.lblSignInMessage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "ConnectToYTMusic";
            this.Style = MetroFramework.MetroColorStyle.Red;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConnectToYTMusic_FormClosing);
            this.Load += new System.EventHandler(this.ConnectToYTMusic_Load);
            this.pnlBrowser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbConnectSuccess)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSignInMessage;
        private System.Windows.Forms.Panel pnlBrowser;
        private Microsoft.Web.WebView2.WinForms.WebView2 browser;
        private System.Windows.Forms.PictureBox pbConnectSuccess;
        private System.Windows.Forms.Label lblConnectSuccess;
    }
}