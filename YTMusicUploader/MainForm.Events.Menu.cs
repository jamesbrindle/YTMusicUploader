using JBToolkit.Threads;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace YTMusicUploader
{
    public partial class MainForm
    {
        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.Button == MouseButtons.Left)
                    TrayContextMenuStrip.Show();
            }
            else if (e.Button == MouseButtons.Left)
            {
                ShowForm();
            }
        }

        private void TsmShow_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        private void TsmQuit_Click(object sender, EventArgs e)
        {
            QuitApplication();
        }

        private void CenterForm()
        {
            Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2,
                                 (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
        }

        public void QuitApplication()
        {
            Aborting = true;
            FileUploader.Stopped = true;
            TrayIcon.Visible = false;

            try
            {
                ConnectToYTMusicForm.BrowserControl.Dispose();
            }
            catch
            { }

            ThreadHelper.SafeSleep(500);

            try
            {
                _installingEdgeThread.Abort();
            }
            catch { }

            try
            {
                _scanAndUploadThread.Abort();
            }
            catch { }

            try
            {
                _connectToYouTubeMusicThread.Abort();
            }
            catch { }

            try
            {
                _queueThread.Abort();
            }
            catch { }

            try
            {
                Environment.Exit(0);
            }
            catch { }
        }
    }
}
