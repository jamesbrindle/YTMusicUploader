using System;
using System.Threading;
using System.Windows.Forms;

namespace JBToolkit.WinForms
{
    /// <summary>
    /// Simple, nice general popup with a delay before dissappearing
    /// </summary>
    public partial class Popup : Form
    {
        public Popup(string message, int durationInSeconds)
        {
            Message = message;
            Duration = durationInSeconds * 1000;

            InitializeComponent();
        }

        public Popup(string message)
        {
            Message = message;
            Duration = 5 * 1000; // default 5 seconds

            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;

                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED     
                return cp;
            }
        }

        private string Message { get; }

        private int Duration { get; }

        private void Popup_Load(object sender, EventArgs e)
        {
            lblMessage.Text = Message;
            FadeInOut();
        }

        private void FadeInOut()
        {
            //Object is fully visible. Fade it out

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                var opacity = Opacity;

                while (Opacity < 0.9)
                {
                    Thread.Sleep(5);
                    opacity += 0.04;

                    SetOpacity(opacity);
                }

                SetOpacity(0.9);

                Thread.Sleep(Duration / 2);

                while (Opacity > 0.0)
                {
                    Thread.Sleep(5);
                    opacity -= 0.005;

                    SetOpacity(opacity);
                }

                SetOpacity(0);

                ClosePopup();

            }).Start();
        }

        private delegate void SetOpacity_Callback(double opacity);

        private void SetOpacity(double opacity)
        {
            if (InvokeRequired)
            {
                SetOpacity_Callback d = SetOpacity;
                Invoke(d, opacity);
            }
            else
            {
                Opacity = opacity;
                this.Refresh();
            }
        }

        private delegate void ClosePopup_Callback();
        private void ClosePopup()
        {
            if (InvokeRequired)
            {
                ClosePopup_Callback d = ClosePopup;
                Invoke(d);
            }
            else
            {
                try
                {
                    Close();
                }
                catch
                {
                    // Ignore
                }
            }
        }

        private void Popup_Shown(object sender, EventArgs e)
        {
            Focus();
            BringToFront();
        }
    }
}