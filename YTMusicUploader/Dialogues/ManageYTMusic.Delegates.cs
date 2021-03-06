﻿using JBToolkit;
using JBToolkit.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Business;
using YTMusicUploader.Providers.RequestModels;
using static YTMusicUploader.Providers.RequestModels.ArtistCache;

namespace YTMusicUploader.Dialogues
{
    public partial class ManageYTMusic
    {
        delegate void AddPlaylistsNodesToTreeDelegate(List<TreeNode> playlistNodes);
        private void AddPlaylistsNodesToTree(List<TreeNode> playlistNodes)
        {
            if (tvUploads.InvokeRequired)
            {
                var d = new AddPlaylistsNodesToTreeDelegate(AddPlaylistsNodesToTree);
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
                var d = new AddArtistNodesToTreeDelegate(AddArtistNodesToTree);
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
                var d = new AddChildNodesDelegate(AddChildNodes);
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
                var d = new SetStatusDelegate(ShowPreloader);
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
                var d = new SetTreeViewEnabledDelegate(SetTreeViewEnabled);
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
                var d = new SetMusicDetailsDelegate(SetMusicDetails);
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
                var d = new SetCovertArtImageDelegate(SetCovertArtImage);
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
                var d = new DeselectAllActionButtonsDelegate(DeselectAllActionButtons);
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
                var d = new DisableAllActionButtonsDelegate(DisableAllActionButtons);
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
                var d = new DisableDeleteFromYTMusicButtonDelegate(DisableDeleteFromYTMusicButton);
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
                var d = new AppendUpdatesTextDelegate(AppendUpdatesText);
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
                var d = new SetCheckedLabelDelegate(SetCheckedLabel);
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
                var d = new ChangeChildCountDelegate(ChangeChildCount);
                Invoke(d, new object[] { parentNode });
            }
            else
            {
                int end = parentNode.Text.LastIndexOf("(");
                parentNode.Text = parentNode.Text.Substring(0, end).Trim() + " (" + parentNode.Nodes.Count + ")";
            }
        }


        delegate void BindPlaylistNodesFromSelectDelegate(
            TreeNode playlistNode,
            OnlinePlaylist playlist,
            bool expand = true,
            bool showFetchedMessage = true,
            bool isDeleting = false);
        private void BindPlaylistNodesFromSelect(
            TreeNode playlistNode,
            OnlinePlaylist playlist,
            bool expand = true,
            bool showFetchedMessage = true,
            bool isDeleting = false)
        {
            if (tvUploads.InvokeRequired)
            {
                var d = new BindPlaylistNodesFromSelectDelegate(BindPlaylistNodesFromSelect);
                Invoke(d, new object[] { playlistNode, playlist, expand, showFetchedMessage, isDeleting });
            }
            else
            {
                SetTreeViewEnabled(false);
                if (playlistNode != null)
                {
                    var dbPlaylistEntry = MainForm.PlaylistFileRepo.LoadFromPlayListId(playlist.BrowseId).Result;
                    playlistNode.Tag = new MusicManageTreeNodeModel
                    {
                        NodeType = MusicManageTreeNodeModel.NodeTypeEnum.Playlist,
                        PlaylistTitle = playlist.Title,
                        Duration = playlist.Duration,
                        EntityOrBrowseId = playlist.BrowseId,
                        CovertArtUrl = playlist.CoverArtUrl,
                        SongTitleOrDescription = playlist.Description,
                        DatabaseExistence = dbPlaylistEntry != null
                                                ? $"Exists ({dbPlaylistEntry.Id})"
                                                : "Not found or not mapped"
                    };

                    var playlistItems = new List<TreeNode>();
                    foreach (var song in playlist.Songs)
                    {
                        var playlistItem = new TreeNode
                        {
                            Name = Guid.NewGuid().ToString(),
                            Text = song.Title,
                            Tag = Tag = new MusicManageTreeNodeModel
                            {
                                NodeType = MusicManageTreeNodeModel.NodeTypeEnum.PlaylistItem,
                                ArtistTitle = song.ArtistTitle,
                                AlbumTitle = song.AlbumTitle,
                                CovertArtUrl = song.CoverArtUrl,
                                Duration = song.Duration,
                                SongTitleOrDescription = song.Title,
                                MbId = "-",
                                EntityOrBrowseId = song.VideoId,
                                DatabaseExistence = "-",
                                AltEntityId = song.SetVideoId,
                                Uploaded = "-"
                            }
                        };

                        playlistItems.Add(playlistItem);
                    }

                    playlistNode.Nodes.AddRange(playlistItems.ToArray());
                    playlistNode.Text = playlistNode.Text + " (" + playlistNode.Nodes.Count + ")";

                    if (showFetchedMessage)
                        AppendUpdatesText($"Fetched {playlistItems.Count} playlist items.",
                                          ColourHelper.HexStringToColor("#0d5601"));
                    if (expand)
                        playlistNode.Expand();

                    if (playlistNode.Checked)
                        CheckAllChildNodes(playlistNode, true);

                    if (!isDeleting)
                        SetMusicDetails((MusicManageTreeNodeModel)playlistNode.Tag);
                }

                if (!isDeleting)
                {
                    ShowPreloader(false);
                    SetTreeViewEnabled(true);
                    DisableAllActionButtons(false);
                }
            }
        }

        delegate void BindAlbumNodesFromSelectDelegate(
            TreeNode artistNode,
            AlbumSongCollection albumSongCollection,
            bool expand = true,
            bool showFetchedMessage = true,
            bool isDeleting = false);
        private void BindAlbumNodesFromSelect(
            TreeNode artistNode,
            AlbumSongCollection albumSongCollection,
            bool expand = true,
            bool showFetchedMessage = true,
            bool isDeleting = false)
        {
            if (tvUploads.InvokeRequired)
            {
                var d = new BindAlbumNodesFromSelectDelegate(BindAlbumNodesFromSelect);
                Invoke(d, new object[] { artistNode, albumSongCollection, expand, showFetchedMessage, isDeleting });
            }
            else
            {
                SetTreeViewEnabled(false);
                if (artistNode != null)
                {
                    var albumNodes = new List<TreeNode>();
                    foreach (var album in albumSongCollection.Albums)
                    {
                        var songNodes = new List<TreeNode>();
                        string releaseMbId = string.Empty;

                        foreach (var song in album.Songs)
                        {
                            var musicFile = MainForm.MusicFileRepo.LoadFromEntityId(song.EntityId).Result;
                            string databaseExistenceText = "Not found or not mapped";

                            if (musicFile != null && musicFile.Id != 0 && musicFile.Id != -1)
                            {
                                databaseExistenceText = $"Exists ({musicFile.Id})";
                                releaseMbId = string.IsNullOrEmpty(musicFile.ReleaseMbId) ? releaseMbId : musicFile.ReleaseMbId;
                            }

                            songNodes.Add(new TreeNode
                            {
                                Name = song.EntityId,
                                Text = song.Title,
                                Tag = Tag = new MusicManageTreeNodeModel
                                {
                                    NodeType = MusicManageTreeNodeModel.NodeTypeEnum.Song,
                                    ArtistTitle = ((MusicManageTreeNodeModel)artistNode.Tag).ArtistTitle,
                                    AlbumTitle = album.Title,
                                    SongTitleOrDescription = song.Title,
                                    Duration = song.Duration,
                                    CovertArtUrl = song.CoverArtUrl,
                                    DatabaseExistence = databaseExistenceText,
                                    MbId = musicFile == null || string.IsNullOrEmpty(musicFile.MbId) ? "-" : musicFile.MbId,
                                    EntityOrBrowseId = song.EntityId,
                                    Uploaded = musicFile == null ? "-" : musicFile.LastUpload.ToString("dd/MM/yyyy HH:mm")
                                }
                            });
                        }

                        var albumNode = new TreeNode
                        {
                            Name = Guid.NewGuid().ToString(),
                            Text = album.Title,
                            Tag = Tag = new MusicManageTreeNodeModel
                            {
                                NodeType = MusicManageTreeNodeModel.NodeTypeEnum.Album,
                                ArtistTitle = ((MusicManageTreeNodeModel)artistNode.Tag).ArtistTitle,
                                AlbumTitle = album.Title,
                                CovertArtUrl = album.CoverArtUrl,
                                DatabaseExistence = string.IsNullOrEmpty(releaseMbId) ? "Not found or not mapped" : "Tracks exists for this album",
                                MbId = string.IsNullOrEmpty(releaseMbId) ? "-" : releaseMbId,
                                EntityOrBrowseId = album.EntityId,
                                Uploaded = "-"
                            }
                        };

                        albumNode.Nodes.AddRange(songNodes.ToArray());
                        albumNode.Text = albumNode.Text + " (" + songNodes.Count + ")";
                        albumNodes.Add(albumNode);
                    };

                    AddChildNodes(artistNode, albumNodes);

                    int albumCount = albumSongCollection.Albums.Count;
                    int songCount = albumSongCollection.Songs.Count;

                    string albumText = "albums";
                    string songText = "tracks";

                    if (albumCount == 1)
                        albumText = "album";

                    if (songCount == 1)
                        songText = "track";

                    if (showFetchedMessage)
                        AppendUpdatesText($"Fetched {albumCount} {albumText}, {songCount} {songText}.",
                                          ColourHelper.HexStringToColor("#0d5601"));

                    artistNode.Text = artistNode.Text + " (" + artistNode.Nodes.Count + ")";
                    if (expand)
                        artistNode.Expand();

                    if (artistNode.Checked)
                        CheckAllChildNodes(artistNode, true);

                    if (!isDeleting)
                        SetMusicDetails((MusicManageTreeNodeModel)artistNode.Tag);
                }

                if (!isDeleting)
                {
                    ShowPreloader(false);
                    SetTreeViewEnabled(true);
                    DisableAllActionButtons(false);
                }
            }
        }
    }
}
