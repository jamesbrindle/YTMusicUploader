using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YTMusicUploader.Dialogues
{
    public partial class ManageYTMusic
    {
        private void PbRefresh_Click(object sender, EventArgs e)
        {
            new Thread((ThreadStart)delegate { GetArtists(); }).Start();
        }

        private void PbRefresh_MouseDown(object sender, MouseEventArgs e)
        {
            pbRefresh.Image = Properties.Resources.refresh_down;
        }

        private void PbRefresh_MouseEnter(object sender, EventArgs e)
        {
            pbRefresh.Image = Properties.Resources.refresh_hover;
        }

        private void PbRefresh_MouseLeave(object sender, EventArgs e)
        {
            pbRefresh.Image = Properties.Resources.refresh;
        }

        private void PbRefresh_MouseUp(object sender, MouseEventArgs e)
        {
            pbRefresh.Image = Properties.Resources.refresh_hover;
        }
    }
}
