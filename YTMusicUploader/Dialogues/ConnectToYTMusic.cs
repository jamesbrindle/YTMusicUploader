using JBToolkit.WinForms;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.RequestModels;

namespace YTMusicUploader.Dialogues
{
    /// </summary>
    public partial class ConnectToYTMusic : OptimisedMetroForm
    {
        private MainForm MainForm { get; set; }

        public ConnectToYTMusic(MainForm mainForm) : base(formResizable: true)
        {
            MainForm = mainForm;
            Font = new Font(Font.Name, 8.25f * 96f / CreateGraphics().DpiX, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
            InitializeComponent();
            SuspendDrawing(this);
        }

        private void ConnectToYTMusic_Load(object sender, EventArgs e)
        {
            OnLoad(e);
            ResumeDrawing(this);

            PreloaderVisible(false);
            StatusVisible(false);

            tbCookieValue.Text = MainForm.Settings.AuthenticationCookie ?? "";
        }

        private void LnkMoreInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://ytmusicapi.readthedocs.io/en/latest/setup.html");
        }

        private void LnkVideo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://youtu.be/ufuckGSathc");
        }

        private void CookieValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbCookieValue.Text))
            {
                StatusVisible(false);
                PreloaderVisible(false);
            }
            else
            {

                string cookieValue = tbCookieValue.Text;
                if (cookieValue.Contains("VISITOR_INFO1_LIVE") &&
                          cookieValue.Contains("SID") &&
                          cookieValue.Contains("__Secure") &&
                          cookieValue.Contains("LOGIN_INFO"))
                {
                    cookieValue = FormatCookieValue(cookieValue.Trim());

                    try
                    {
                        new Thread((ThreadStart)delegate
                        {
                            PreloaderVisible(true);
                            StatusVisible(true);
                            StatusColour(Color.Blue);
                            StatusText("Validating Cookie Value");

                            if (Requests.IsAuthenticated(cookieValue))
                            {
                                MainForm.Settings.AuthenticationCookie = cookieValue;
                                Task.Run(async () => await MainForm.Settings.Save());

                                StatusText("Validation Successful");
                                StatusColour(Color.Green);
                                PreloaderVisible(false);
                                SetConnected(true);
                            }
                            else
                            {
                                StatusText("Not authenticated. Try signing out and back in");
                                StatusColour(Color.Red);
                                PreloaderVisible(false);
                                SetConnected(false);
                            }

                        }).Start();
                    }
                    catch
                    {
                        SetConnected(false);
                    }
                }
                else
                {
                    StatusVisible(true);
                    StatusColour(Color.Red);
                    StatusText("Invalid authentication cookie");
                }
            }
        }

        private string FormatCookieValue(string original)
        {
            try
            {
                if (original.Contains("{"))
                {
                    var cookie = JsonConvert.DeserializeObject<AuthorisationCookie>(original);
                    return cookie.ToString();
                }
            }
            catch { }

            return original;
        }

        delegate void PreloaderVisibleDelegate(bool visible);
        private void PreloaderVisible(bool visible)
        {
            if (pbPreloader.InvokeRequired)
            {
                var d = new PreloaderVisibleDelegate(PreloaderVisible);
                Invoke(d, new object[] { visible });
            }
            else
            {
                pbPreloader.Visible = visible;
            }
        }

        delegate void StatusVisibleDelegate(bool visible);
        private void StatusVisible(bool visible)
        {
            if (lblStatus.InvokeRequired)
            {
                var d = new StatusVisibleDelegate(StatusVisible);
                Invoke(d, new object[] { visible });
            }
            else
            {
                lblStatus.Visible = visible;
            }
        }

        delegate void StatusTextDelegate(string text);
        private void StatusText(string text)
        {
            if (lblStatus.InvokeRequired)
            {
                var d = new StatusTextDelegate(StatusText);
                Invoke(d, new object[] { text });
            }
            else
            {
                lblStatus.Text = text;
            }
        }

        delegate void StatusColourDelegate(Color color);
        private void StatusColour(Color color)
        {
            if (lblStatus.InvokeRequired)
            {
                var d = new StatusColourDelegate(StatusColour);
                Invoke(d, new object[] { color });
            }
            else
            {
                lblStatus.ForeColor = color;
            }
        }

        delegate void SetConnectedDelegate(bool connected);
        private void SetConnected(bool connected)
        {
            if (lblSignInMessage.InvokeRequired ||
                pbConnectSuccess.InvokeRequired ||
                lblConnectSuccess.InvokeRequired)
            {
                var d = new SetConnectedDelegate(SetConnected);
                Invoke(d, new object[] { connected });
            }
            else
            {
                if (connected)
                {
                    lblSignInMessage.Visible = false;
                    pbConnectSuccess.Visible = true;
                    lblConnectSuccess.Visible = true;
                }
                else
                {
                    lblSignInMessage.Visible = true;
                    pbConnectSuccess.Visible = false;
                    lblConnectSuccess.Visible = false;
                }
            }
        }

        public void CloseForm()
        {
            try
            {
                Close();
            }
            catch { }
        }

        private void ConnectToYTMusic_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
            else
            {
                CloseForm();
            }
        }
    }
}
