using JBToolkit.WinForms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Newtonsoft.Json;
using System;
using System.Windows.Forms;
using YTMusicUploader.Providers;

namespace YTMusicUploader.Dialogues
{
    public partial class ConnectToYTMusic : OptimisedMetroForm
    {
        private MainForm MainForm { get; set; }
        private bool Invisible { get; set; }
        public WebView2 BrowserControl { get { return browser; } }

        public ConnectToYTMusic(MainForm mainForm) : base(formResizable: true,
                                                          controlTagsAsTooltips: false)
        {
            MainForm = mainForm;
            InitializeComponent();
            SuspendDrawing(this);
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
                        Requests.UpdateAuthHeader(cookieValue);
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

        private void ConnectToYTMusic_Shown(object sender, EventArgs e)
        {
         
        }
    }
}
