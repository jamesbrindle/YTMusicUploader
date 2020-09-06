using JBToolkit;
using JBToolkit.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Business;
using YTMusicUploader.Providers;
using YTMusicUploader.Providers.RequestModels;
using static YTMusicUploader.Providers.RequestModels.ArtistCache;

namespace YTMusicUploader.Dialogues
{
    public partial class ManageYTMusic
    {
        delegate void BindArtistsDelegate(bool showFetchedMessage = true);
        private void BindArtists(bool showFetchedMessage = true)
        {
            if (tvUploads.InvokeRequired)
            {
                BindArtistsDelegate d = new BindArtistsDelegate(BindArtists);
                Invoke(d, new object[] { showFetchedMessage });
            }
            else
            {
                SetTreeViewEnabled(false);
                tvUploads.Nodes.Clear();

                tvUploads.Nodes.Add(new TreeNode
                {
                    Name = "root",
                    Text = "Artists",
                    Tag = Tag = new MusicManageTreeNodeModel
                    {
                        NodeType = MusicManageTreeNodeModel.NodeTypeEnum.Root
                    }
                });

                foreach (var artist in Requests.ArtistCache.Artists)
                {
                    tvUploads.Nodes[0].Nodes.Add(new TreeNode
                    {
                        Name = artist.BrowseId,
                        Text = artist.ArtistName,
                        Tag = Tag = new MusicManageTreeNodeModel
                        {
                            NodeType = MusicManageTreeNodeModel.NodeTypeEnum.Artist,
                            ArtistTitle = artist.ArtistName
                        }
                    });

                    if (artist.AlbumSongCollection != null &&
                        artist.AlbumSongCollection.Albums != null &&
                        artist.AlbumSongCollection.Albums.Count > 0)
                    {
                        BindAlbumNodes(artist.BrowseId, artist.AlbumSongCollection, false, showFetchedMessage);
                    }
                }

                string artistText = "artists";
                int artistCount = Requests.ArtistCache.Artists.Count;

                if (artistCount == 1)
                    artistText = "artist";

                if (showFetchedMessage)
                    AppendUpdatesText($"Fetched {artistCount} {artistText}.",
                                      ColourHelper.HexStringToColor("#0d5601"));

                tvUploads.Nodes[0].Text = tvUploads.Nodes[0].Text + " (" + tvUploads.Nodes[0].Nodes.Count + ")";
                tvUploads.Nodes[0].Expand();
                ShowPreloader(false);
                SetTreeViewEnabled(true);
            }
        }

        delegate void BindAlbumNodesDelegate(
            string artistNodeName,
            AlbumSongCollection albumSongCollection,
            bool expand = true,
            bool showFetchedMessage = true);
        private void BindAlbumNodes(
            string artistNodeName,
            AlbumSongCollection albumSongCollection,
            bool expand = true,
            bool showFetchedMessage = true)
        {
            if (tvUploads.InvokeRequired)
            {
                BindAlbumNodesDelegate d = new BindAlbumNodesDelegate(BindAlbumNodes);
                Invoke(d, new object[] { artistNodeName, albumSongCollection, expand, showFetchedMessage });
            }
            else
            {
                SetTreeViewEnabled(false);
                TreeNode artistNode = null;
                for (int i = 0; i < tvUploads.Nodes[0].Nodes.Count; i++)
                {
                    if (tvUploads.Nodes[0].Nodes[i].Name == artistNodeName)
                    {
                        artistNode = tvUploads.Nodes[0].Nodes[i];
                        break;
                    }
                }

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
                                SongTitle = song.Title,
                                Duration = song.Duration,
                                CovertArtUrl = song.CoverArtUrl,
                                DatabaseExistence = databaseExistenceText,
                                MbId = musicFile == null || string.IsNullOrEmpty(musicFile.MbId) ? "-" : musicFile.MbId,
                                EntityId = musicFile == null || string.IsNullOrEmpty(musicFile.EntityId) ? "-" : musicFile.EntityId,
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
                            Uploaded = "-"
                        }
                    };

                    albumNode.Nodes.AddRange(songNodes.ToArray());
                    albumNode.Text = albumNode.Text + " (" + songNodes.Count + ")";
                    artistNode.Nodes.Add(albumNode);
                }

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

                ShowPreloader(false);
                SetTreeViewEnabled(true);
            }
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
                lblArtistTitle.Text = nodeTag.ArtistTitle;
                lblAlbumTitle.Text = nodeTag.AlbumTitle;
                lblSongTitle.Text = nodeTag.SongTitle;
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
                        new Thread((ThreadStart)delegate
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
                        }).Start();
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
                tbUpdates.SelectionColor = color;
                tbUpdates.AppendText("- " + text + "\r\n");
                tbUpdates.SelectionStart = tbUpdates.Text.Length;
                tbUpdates.ScrollToCaret();
            }
        }
    }
}
