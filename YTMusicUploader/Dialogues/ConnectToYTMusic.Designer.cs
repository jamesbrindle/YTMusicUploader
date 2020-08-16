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
            this.label1 = new System.Windows.Forms.Label();
            this.pnlBrowser = new System.Windows.Forms.Panel();
            this.browser = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.pnlBrowser.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(479, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sign into YouTube Music to retrieve your authorisation cookie. Once retrieved thi" +
    "s window will close.";
            // 
            // pnlBrowser
            // 
            this.pnlBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBrowser.Controls.Add(this.browser);
            this.pnlBrowser.Location = new System.Drawing.Point(26, 76);
            this.pnlBrowser.Name = "pnlBrowser";
            this.pnlBrowser.Size = new System.Drawing.Size(751, 701);
            this.pnlBrowser.TabIndex = 1;
            // 
            // browser
            // 
            this.browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browser.Location = new System.Drawing.Point(0, 0);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(751, 701);
            this.browser.Source = new System.Uri("https://music.youtube.com/", System.UriKind.Absolute);
            this.browser.TabIndex = 0;
            this.browser.Text = "YouTube Music";
            this.browser.ZoomFactor = 1D;
            this.browser.CoreWebView2Ready += new System.EventHandler<System.EventArgs>(this.Browser_CoreWebView2Ready);
            // 
            // ConnectToYTMusic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 800);
            this.Controls.Add(this.pnlBrowser);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(800, 800);
            this.Name = "ConnectToYTMusic";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConnectToYTMusic_FormClosing);
            this.Load += new System.EventHandler(this.ConnectToYTMusic_Load);
            this.pnlBrowser.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlBrowser;
        private Microsoft.Web.WebView2.WinForms.WebView2 browser;
    }
}