using JBToolkit;
using MetroFramework;
using System;
using System.Threading;
using System.Windows.Forms;

namespace YTMusicUploader.Dialogues
{
    public partial class ManageYTMusic
    {
        //
        // MBID Hyperlink
        //
        private void LblMbId_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(lblMbId.Tag.ToString()) && lblMbId.Tag.ToString() != "-")
                System.Diagnostics.Process.Start(lblMbId.Tag.ToString());
        }

        //
        // Refresh
        //

        private void PbRefresh_Click(object sender, EventArgs e)
        {
            DisableAllActionButtons(true);
            ThreadPool.QueueUserWorkItem(delegate
            {
                GetArtistsAndPlaylists();
            });
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

        //
        // Reset Upload States
        //

        private void PbResetUploadStates_Click(object sender, EventArgs e)
        {
            if (MetroMessageBox.Show(this,
                                "\r\nThis will clear the 'uploaded date' of all music file entries in the database, " +
                                    "causing a recheck of all files. \r\n\r\nAre you sure you want to contine?", "Confirm Action",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Error,
                                150) == DialogResult.Yes)
            {
                ChangesMade = true;

                ShowPreloader(true);
                AppendUpdatesText("Reseting uploaded music file entry states...",
                                  ColourHelper.HexStringToColor("#e20000"));

                DeselectAllActionButtons();
                ResetMusicFileAndPlaylistEntryStates();
            }
        }

        private void PbResetUploadStates_MouseDown(object sender, MouseEventArgs e)
        {
            PbResetUploadStates.Image = Properties.Resources.reset_uploaded_down;
            lblSelectedButton.Text = "Reset uploaded music states (causes recheck of all music files).";
            lblSelectedButton.Visible = true;
        }

        private void PbResetUploadStates_MouseEnter(object sender, EventArgs e)
        {
            PbResetUploadStates.Image = Properties.Resources.reset_uploaded_hover;
            lblSelectedButton.Text = "Reset uploaded music states (causes recheck of all music files).";
            lblSelectedButton.Visible = true;
        }

        private void PbResetUploadStates_MouseLeave(object sender, EventArgs e)
        {
            PbResetUploadStates.Image = Properties.Resources.reset_uploaded;
            lblSelectedButton.Visible = false;
        }

        private void PbResetUploadStates_MouseUp(object sender, MouseEventArgs e)
        {
            PbResetUploadStates.Image = Properties.Resources.reset_uploaded_hover;
            lblSelectedButton.Text = "Reset uploaded music states (will recheck all music files).";
            lblSelectedButton.Visible = true;
        }

        //
        // Reset Database
        //

        private void PbResetDatabase_Click(object sender, EventArgs e)
        {
            if (MetroMessageBox.Show(this,
                                "\r\nThis will completely reset the database which includes all settings and all music file " +
                                    "entries and their states. The application will need to close for changes to take effect." +
                                    "\r\n\r\nAre you sure you want to contine?",
                                "Confirm Action",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Error,
                                170) == DialogResult.Yes)
            {
                ChangesMade = true;

                ShowPreloader(true);
                AppendUpdatesText("Resetting the database...",
                                  ColourHelper.HexStringToColor("#e20000"));

                DeselectAllActionButtons();
                ResetUserDatabase();
                MainForm.QuitApplication();
            }
        }

        private void PbResetDatabase_MouseDown(object sender, MouseEventArgs e)
        {
            PbResetDatabase.Image = Properties.Resources.reset_database_down;
            lblSelectedButton.Text = "Reset entire database (start from scratch).";
            lblSelectedButton.Visible = true;
        }

        private void PbResetDatabase_MouseEnter(object sender, EventArgs e)
        {
            PbResetDatabase.Image = Properties.Resources.reset_database_hover;
            lblSelectedButton.Text = "Reset entire database (start from scratch).";
            lblSelectedButton.Visible = true;
        }

        private void PbResetDatabase_MouseLeave(object sender, EventArgs e)
        {
            PbResetDatabase.Image = Properties.Resources.reset_database;
            lblSelectedButton.Visible = false;
        }

        private void PbResetDatabase_MouseUp(object sender, MouseEventArgs e)
        {
            PbResetDatabase.Image = Properties.Resources.reset_database_hover;
            lblSelectedButton.Text = "Reset entire database (start from scratch).";
            lblSelectedButton.Visible = true;
        }

        //
        // Delete from YT Music
        //

        private void PbDeleteYTUploaded_Click(object sender, EventArgs e)
        {
            if (MetroMessageBox.Show(this,
                    "\r\nThis will delete the selected tracks from YouTube Music or your playlist.\r\n\r\n" +
                        "Are you sure you want to do this?",
                    "Confirm Action",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error,
                    150) == DialogResult.Yes)
            {
                ChangesMade = true;

                ShowPreloader(true);
                AppendUpdatesText("Deleting from YouTube Music...",
                                   ColourHelper.HexStringToColor("#e20000"));

                DeselectAllActionButtons();
                try
                {
                    DeleteTracksFromYouTubeMusic();
                }
                catch
                {
                    DisableAllActionButtons(false);
                }
            }
        }

        private void PbDeleteYTUploaded_MouseDown(object sender, MouseEventArgs e)
        {
            PbDeleteYTUploaded.Image = Properties.Resources.delete_from_youtube_down;
            lblSelectedButton.Text = "Delete selected tracks from YouTube Music.";
            lblSelectedButton.Visible = true;
        }

        private void PbDeleteYTUploaded_MouseEnter(object sender, EventArgs e)
        {
            PbDeleteYTUploaded.Image = Properties.Resources.delete_from_youtube_hover;
            lblSelectedButton.Text = "Delete selected tracks from YouTube Music.";
            lblSelectedButton.Visible = true;
        }

        private void PbDeleteYTUploaded_MouseLeave(object sender, EventArgs e)
        {
            PbDeleteYTUploaded.Image = Properties.Resources.delete_from_youtube;
            lblSelectedButton.Visible = false;
        }

        private void PbDeleteYTUploaded_MouseUp(object sender, MouseEventArgs e)
        {
            PbDeleteYTUploaded.Image = Properties.Resources.delete_from_youtube_hover;
            lblSelectedButton.Text = "Delete selected tracks from YouTube Music.";
            lblSelectedButton.Visible = true;
        }
    }
}
