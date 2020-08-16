using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YTMusicUploader.Dialogues;

namespace YTMusicUploader
{
    public partial class MainForm 
    {
        // Connect to TY Music

        private void BtnConnectToYoutube_Click(object sender, EventArgs e)
        {
            new ConnectToYTMusic().ShowDialog();
        }


        // Add To Wath Folder

        private void BtnWatchFolder_Click(object sender, EventArgs e)
        {

        }

        private void BtnAddWatchFolder_MouseEnter(object sender, EventArgs e)
        {
            btnAddWatchFolder.Image = Properties.Resources.plus_hover;
        }

        private void BtnAddWatchFolder_MouseLeave(object sender, EventArgs e)
        {
            btnAddWatchFolder.Image = Properties.Resources.plus;
        }

        private void BtnAddWatchFolder_MouseDown(object sender, MouseEventArgs e)
        {
            btnAddWatchFolder.Image = Properties.Resources.plus_down;
        }

        // Remove from Watch Folder

        private void BtnRemoveWatchFolder_Click(object sender, EventArgs e)
        {

        }

        private void BtnRemoveWatchFolder_MouseEnter(object sender, EventArgs e)
        {
            btnRemoveWatchFolder.Image = Properties.Resources.minus_hover;
        }

        private void BtnRemoveWatchFolder_MouseLeave(object sender, EventArgs e)
        {
            btnRemoveWatchFolder.Image = Properties.Resources.minus;
        }

        private void BtnRemoveWatchFolder_MouseDown(object sender, MouseEventArgs e)
        {
            btnRemoveWatchFolder.Image = Properties.Resources.minus_down;
        }       
    }
}
