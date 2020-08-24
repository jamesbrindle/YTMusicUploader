using JBToolkit.WinForms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace YTMusicUploader.Dialogues
{
    /*## WARNING ####################################################################################
    
     If  you change anything in the ConnectYTMusic layout, the .Desinger.cs will put the '.Source'
     property back into the WebView2 control. You need to make sure you remove the Source line
     so that we can set the CoreWebView2Environment 'before' setting the source (known bug).

     If you don't then once deployed, the WebView2 control will try and use the exe folder location
     for cached files, which it doesn't have access to and so will fail to load.

    ################################################################################################*/

    /// <summary>
    /// Shows a WebView2 control
    /// https://docs.microsoft.com/en-us/microsoft-edge/webview2/
    /// 
    /// Seems to be the only browser control I've found that will actually display YouTube music.
    /// This control is used purely so users can sign into YouTube and automatically grab the authentication
    /// cookie that lets you upload music to.
    /// 
    /// DEPENDENCY: Microsoft Edge (Canary Channel) required. It's been packed with the installer for convienience.
    /// </summary>
    public partial class ConnectToYTMusic : OptimisedMetroForm
    {
        private MainForm MainForm { get; set; }
        private bool Invisible { get; set; }
        public WebView2 BrowserControl { get { return browser; } }

        /// <summary>
        /// Shows a WebView2 control
        /// https://docs.microsoft.com/en-us/microsoft-edge/webview2/
        /// 
        /// Seems to be the only browser control I've found that will actually display YouTube music.
        /// This control is used purely so users can sign into YouTube and automatically grab the authentication
        /// cookie that lets you upload music to.
        /// 
        /// DEPENDENCY: Microsoft Edge (Canary Channel) required. It's been packed with the installer for convienience.
        /// </summary>
        public ConnectToYTMusic(MainForm mainForm) : base(formResizable: true)
        {
            MainForm = mainForm;
            InitializeComponent();
            InitializeBrowser();
            SuspendDrawing(this);
        }

        private async void InitializeBrowser()
        {
            // Must create the environment and set the application data to somewhere writable. I.e. user's
            // local AppData directory. And you must do this before attempting to navigate the browser to an
            // address.

            try
            {
                var env = await CoreWebView2Environment.CreateAsync(Global.EdgeFolder, Global.AppDataLocation);
                await browser.EnsureCoreWebView2Async(env);
                browser.Source = new Uri("https://music.youtube.com/", UriKind.Absolute);
            }
            catch (Exception e)
            {
                MainForm.SetConnectToYouTubeButtonEnabled(false);
                MainForm.ShowMessageBox(
                            "WebView2 Error",
                            $"{Environment.NewLine}It looks like Canary Edge Core never finished installing. {Environment.NewLine}{Environment.NewLine}" +
                            $"Please delete the folder: {Global.EdgeFolder} and then restart this application. If the error persists, then this application" +
                            $"has been built wrong. Please see the 'WARNING' in file: 'Dialogues/ConnectToYTMusics." +
                            $"{Environment.NewLine}{Environment.NewLine}The application will reinstall automatically once restarted." +
                            $"{Environment.NewLine}{Environment.NewLine}Exception: {e.Message}" +
                            $"{Environment.NewLine}{Environment.NewLine}YT Music Uploader will now exit.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            320);

                Process.GetCurrentProcess().Kill();
            }
        }

        private void ConnectToYTMusic_Load(object sender, EventArgs e)
        {
            OnLoad(e);
            ResumeDrawing(this);
        }

        private void ConnectToYTMusic_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void CoreWebView2_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            Uri myUri = new Uri("https://music.youtube.com/");
            if (myUri.IsBaseOf(e.Request.RequestUri))
            {
                var json = JsonConvert.SerializeObject(e.Request.Headers);
                dynamic obj = JsonConvert.DeserializeObject(json);

                try
                {
                    string cookieValue = obj[1].Value[0].ToString();

                    if (cookieValue.Contains("VISITOR_INFO1_LIVE") &&
                        cookieValue.Contains("SID") &&
                        cookieValue.Contains("__Secure") &&
                        cookieValue.Contains("LOGIN_INFO"))
                    {
                        MainForm.Settings.AuthenticationCookie = cookieValue;
                        MainForm.Settings.Save();

                        lblSignInMessage.Visible = false;
                        pbConnectSuccess.Visible = true;
                        lblConnectSuccess.Visible = true;
                    }
                }
                catch { }
            }
        }

        private void Browser_CoreWebView2Ready(object sender, EventArgs e)
        {
            browser.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
            browser.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
        }
    }
}
