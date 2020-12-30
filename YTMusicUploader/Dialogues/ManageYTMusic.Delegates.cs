using JBToolkit.Imaging;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Business;
using YTMusicUploader.Providers.RequestModels;

namespace YTMusicUploader.Dialogues
{
    public partial class ManageYTMusic
    {
        delegate void AddPlaylistsNodesToTreeDelegate(List<TreeNode> playlistNodes);
        private void AddPlaylistsNodesToTree(List<TreeNode> playlistNodes)
        {
            if (tvUploads.InvokeRequired)
            {
                AddPlaylistsNodesToTreeDelegate d = new AddPlaylistsNodesToTreeDelegate(AddPlaylistsNodesToTree);
                Invoke(d, new object[] { playlistNodes });
            }
            else
            {
                tvUploads.Nodes.Clear();

                SuspendDrawing(tvUploads);
                tvUploads.Nodes.Add(new TreeNode
                {
                    Name = "playlists",
                    Text = "Playlists",
                    Tag = Tag = new MusicManageTreeNodeModel
                    {
                        NodeType = MusicManageTreeNodeModel.NodeTypeEnum.Root
                    }
                });

                AddChildNodesFromArtistBind(tvUploads.Nodes[0], playlistNodes);

                tvUploads.Nodes[0].Text = tvUploads.Nodes[0].Text + " (" + tvUploads.Nodes[0].Nodes.Count + ")";             
            }
        }

        delegate void AddArtistNodesToTreeDelegate(List<TreeNode> artistNodes);
        private void AddArtistNodesToTree(List<TreeNode> artistNodes)
        {
            if (tvUploads.InvokeRequired)
            {
                AddArtistNodesToTreeDelegate d = new AddArtistNodesToTreeDelegate(AddArtistNodesToTree);
                Invoke(d, new object[] { artistNodes });
            }
            else
            {
                tvUploads.Nodes.Add(new TreeNode
                {
                    Name = "root",
                    Text = "Artists",
                    Tag = Tag = new MusicManageTreeNodeModel
                    {
                        NodeType = MusicManageTreeNodeModel.NodeTypeEnum.Root
                    }
                });

                AddChildNodesFromArtistBind(tvUploads.Nodes[1], artistNodes);

                tvUploads.Nodes[1].Text = tvUploads.Nodes[1].Text + " (" + tvUploads.Nodes[1].Nodes.Count + ")";
                tvUploads.Nodes[1].Expand();
                ResumeDrawing(tvUploads);
            }
        }

        delegate void AddChildNodesDelegate(TreeNode parentNode, List<TreeNode> childNodeList);
        private void AddChildNodes(TreeNode parentNode, List<TreeNode> childNodeList)
        {
            if (tvUploads.InvokeRequired)
            {
                AddChildNodesDelegate d = new AddChildNodesDelegate(AddChildNodes);
                Invoke(d, new object[] { parentNode, childNodeList });
            }
            else
            {
                SuspendDrawing(tvUploads);
                parentNode.Nodes.AddRange(childNodeList.ToArray());
                ResumeDrawing(tvUploads);
            }
        }

        private void AddChildNodesFromArtistBind(TreeNode parentNode, List<TreeNode> childNodeList)
        {
            parentNode.Nodes.AddRange(childNodeList.ToArray());
        }

        delegate void SetStatusDelegate(bool showPreloader);
        private void ShowPreloader(bool showPreloader)
        {
            if (pbPreloader.InvokeRequired)
            {
                SetStatusDelegate d = new SetStatusDelegate(ShowPreloader);
                Invoke(d, new object[] { showPreloader });
            }
            else
            {
                pbPreloader.Visible = showPreloader;
            }
        }

        delegate void SetTreeViewEnabledDelegate(bool enabled);
        private void SetTreeViewEnabled(bool enabled)
        {
            if (tvUploads.InvokeRequired || pbRefresh.InvokeRequired)
            {
                SetTreeViewEnabledDelegate d = new SetTreeViewEnabledDelegate(SetTreeViewEnabled);
                Invoke(d, new object[] { enabled });
            }
            else
            {
                tvUploads.Enabled = enabled;
                if (enabled)
                    pbRefresh.Image = Properties.Resources.refresh;
                else
                    pbRefresh.Image = Properties.Resources.refresh_disabled;

                pbRefresh.Enabled = enabled;
            }
        }

        delegate void SetMusicDetailsDelegate(MusicManageTreeNodeModel nodeTag);
        private void SetMusicDetails(MusicManageTreeNodeModel nodeTag)
        {
            if (lblArtistTitle.InvokeRequired ||
                lblAlbumTitle.InvokeRequired ||
                lblSongTitle.InvokeRequired ||
                lblDuration.InvokeRequired ||
                lblDatabaseExistence.InvokeRequired ||
                lblMbId.InvokeRequired ||
                lblUploaded.InvokeRequired ||
                pbCoverArt.InvokeRequired)
            {
                SetMusicDetailsDelegate d = new SetMusicDetailsDelegate(SetMusicDetails);
                Invoke(d, new object[] { nodeTag });
            }
            else
            {
                if (nodeTag.NodeType == MusicManageTreeNodeModel.NodeTypeEnum.Playlist)
                {
                    lblAlbumTitle.Text = nodeTag.PlaylistTitle;
                    lblArtistTitle.Text = "Playlist";
                }
                else
                {
                    lblArtistTitle.Text = nodeTag.ArtistTitle;
                    lblAlbumTitle.Text = nodeTag.AlbumTitle;                    
                }

                lblSongTitle.Text = nodeTag.SongTitleOrDescription;
                lblDuration.Text = nodeTag.Duration;
                lblDatabaseExistence.Text = nodeTag.DatabaseExistence;
                lblMbId.Text = nodeTag.MbId;

                if (nodeTag.NodeType == MusicManageTreeNodeModel.NodeTypeEnum.Album)
                    lblMbId.Tag = "https://musicbrainz.org/release/" + nodeTag.MbId;
                else if (nodeTag.NodeType == MusicManageTreeNodeModel.NodeTypeEnum.Song)
                    lblMbId.Tag = "https://musicbrainz.org/recording/" + nodeTag.MbId;
                else
                    lblMbId.Tag = string.Empty;

                lblUploaded.Text = nodeTag.Uploaded;

                if (nodeTag.NodeType == MusicManageTreeNodeModel.NodeTypeEnum.Artist ||
                    nodeTag.NodeType == MusicManageTreeNodeModel.NodeTypeEnum.Root)
                {
                    pbCoverArt.Image = Properties.Resources.default_artwork_60;
                }
                else
                {
                    if (string.IsNullOrEmpty(nodeTag.CovertArtUrl))
                        pbCoverArt.Image = Properties.Resources.default_artwork_60;
                    else
                    {
                        ThreadPool.QueueUserWorkItem(delegate
                        {
                            byte[] imageBytes = MusicDataFetcher.GetImageBytesFromUrl(nodeTag.CovertArtUrl);
                            if (imageBytes == null)
                                pbCoverArt.Image = Properties.Resources.default_artwork_60;
                            else
                            {
                                using (var ms = new MemoryStream(imageBytes))
                                {
                                    var image = Image.FromStream(ms);
                                    if (image.Width != 60)
                                        image = ImageHelper.ResizeBitmap((Bitmap)image, 60, 60);

                                    SetCovertArtImage(image);
                                }
                            }
                        });
                    }
                }
            }
        }

        delegate void SetCovertArtImageDelegate(Image image);
        private void SetCovertArtImage(Image image)
        {
            if (pbCoverArt.InvokeRequired)
            {
                SetCovertArtImageDelegate d = new SetCovertArtImageDelegate(SetCovertArtImage);
                Invoke(d, new object[] { image });
            }
            else
                pbCoverArt.Image = image;
        }

        delegate void DeselectAllActionButtonsDelegate();
        private void DeselectAllActionButtons()
        {
            if (PbResetUploadStates.InvokeRequired ||
                PbResetDatabase.InvokeRequired ||
                PbDeleteYTUploaded.InvokeRequired)
            {
                DeselectAllActionButtonsDelegate d = new DeselectAllActionButtonsDelegate(DeselectAllActionButtons);
                Invoke(d, new object[] { });
            }
            else
            {
                PbResetUploadStates.Image = Properties.Resources.reset_uploaded;
                lblSelectedButton.Visible = false;

                PbResetDatabase.Image = Properties.Resources.reset_database;
                lblSelectedButton.Visible = false;

                PbDeleteYTUploaded.Image = Properties.Resources.delete_from_youtube;
                lblSelectedButton.Visible = false;
            }
        }

        delegate void DisableAllActionButtonsDelegate(bool disabled);
        private void DisableAllActionButtons(bool disabled)
        {
            if (PbResetUploadStates.InvokeRequired ||
                PbResetDatabase.InvokeRequired ||
                PbDeleteYTUploaded.InvokeRequired ||
                pbRefresh.InvokeRequired ||
                tvUploads.InvokeRequired ||
                tbSearchArtists.InvokeRequired)
            {
                DisableAllActionButtonsDelegate d = new DisableAllActionButtonsDelegate(DisableAllActionButtons);
                Invoke(d, new object[] { disabled });
            }
            else
            {
                lblSelectedButton.Visible = false;
                lblSelectedButton.Visible = false;
                lblSelectedButton.Visible = false;

                tvUploads.AfterSelect -= TvUploads_AfterSelect;
                tbSearchArtists.TextChanged -= TbSearchArtists_TextChanged;
                tbSearchArtists.KeyDown -= TbSearchArtists_KeyDown;

                if (disabled)
                {
                    PbResetUploadStates.Image = Properties.Resources.reset_uploaded_disabled;
                    PbResetUploadStates.Enabled = false;

                    PbResetDatabase.Image = Properties.Resources.reset_database_disabled;
                    PbResetDatabase.Enabled = false;

                    PbDeleteYTUploaded.Image = Properties.Resources.delete_from_youtube_disabled;
                    PbDeleteYTUploaded.Enabled = false;

                    pbRefresh.Image = Properties.Resources.refresh_disabled;
                    pbRefresh.Enabled = false;

                    tbSearchArtists.Enabled = false;
                }
                else
                {
                    PbResetUploadStates.Image = Properties.Resources.reset_uploaded;
                    PbResetUploadStates.Enabled = true;

                    PbResetDatabase.Image = Properties.Resources.reset_database;
                    PbResetDatabase.Enabled = true;

                    if (lblCheckedCount.Text == "0 tracks checked")
                    {
                        PbDeleteYTUploaded.Image = Properties.Resources.delete_from_youtube_disabled;
                        PbDeleteYTUploaded.Enabled = false;
                    }
                    else
                    {
                        PbDeleteYTUploaded.Image = Properties.Resources.delete_from_youtube;
                        PbDeleteYTUploaded.Enabled = true;
                    }

                    pbRefresh.Image = Properties.Resources.refresh;
                    pbRefresh.Enabled = true;

                    tbSearchArtists.Enabled = true;
                }


                tvUploads.AfterSelect += TvUploads_AfterSelect;
                tbSearchArtists.TextChanged += TbSearchArtists_TextChanged;
                tbSearchArtists.KeyDown += TbSearchArtists_KeyDown;
            }
        }

        delegate void DisableDeleteFromYTMusicButtonDelegate();
        private void DisableDeleteFromYTMusicButton()
        {
            if (PbDeleteYTUploaded.InvokeRequired)
            {
                DisableDeleteFromYTMusicButtonDelegate d = new DisableDeleteFromYTMusicButtonDelegate(DisableDeleteFromYTMusicButton);
                Invoke(d, new object[] { });
            }
            else
            {
                PbDeleteYTUploaded.Image = Properties.Resources.delete_from_youtube_disabled;
                PbDeleteYTUploaded.Enabled = false;
                lblSelectedButton.Visible = false;
            }
        }

        delegate void AppendUpdatesTextDelegate(string text, Color color);
        private void AppendUpdatesText(string text, Color color)
        {
            if (tbUpdates.InvokeRequired)
            {
                AppendUpdatesTextDelegate d = new AppendUpdatesTextDelegate(AppendUpdatesText);
                Invoke(d, new object[] { text, color });
            }
            else
            {
                Logger.LogInfo("ManageYTMusic", text);

                tbUpdates.SelectionColor = color;
                tbUpdates.AppendText("- " + text + "\r\n");
                tbUpdates.SelectionStart = tbUpdates.Text.Length;
                tbUpdates.ScrollToCaret();
            }
        }

        delegate void SetCheckedLabelDelegate(string text);
        private void SetCheckedLabel(string text)
        {
            if (lblCheckedCount.InvokeRequired)
            {
                SetCheckedLabelDelegate d = new SetCheckedLabelDelegate(SetCheckedLabel);
                Invoke(d, new object[] { text });
            }
            else
            {
                lblCheckedCount.Text = text;
            }
        }

        delegate void ChangeChildCountDelegate(TreeNode parentNode);
        private void ChangeChildCount(TreeNode parentNode)
        {
            if (tvUploads.InvokeRequired)
            {
                ChangeChildCountDelegate d = new ChangeChildCountDelegate(ChangeChildCount);
                Invoke(d, new object[] { parentNode });
            }
            else
            {
                int end = parentNode.Text.LastIndexOf("(");
                parentNode.Text = parentNode.Text.Substring(0, end).Trim() + " (" + parentNode.Nodes.Count + ")";
            }
        }
    }
}
