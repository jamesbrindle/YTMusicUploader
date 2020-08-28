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
    }
}
