using JBToolkit;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace YTMusicUploader.Dialogues
{
    public partial class ManageYTMusic
    {
        private List<TreeNode> _tnMatchList = new List<TreeNode>();
        private int _currentMatchNodeIndex = 0;

        private void TbSearchArtists_TextChanged(object sender, EventArgs e)
        {
            tvUploads.AfterSelect -= TvUploads_AfterSelect;
            if (tbSearchArtists.Text == "")
            {
                foreach (TreeNode artistNode in tvUploads.Nodes[1].Nodes)
                    artistNode.BackColor = Color.White;

                tvUploads.AfterSelect += TvUploads_AfterSelect;
                return;
            }

            TreeNode heightlightedNode = null;
            _tnMatchList = new List<TreeNode>();

            if (tvUploads.Nodes != null && tvUploads.Nodes.Count > 0)
            {
                foreach (TreeNode artistNode in tvUploads.Nodes[1].Nodes)
                {
                    artistNode.BackColor = Color.White;
                    if (artistNode.Text.ToLower().Contains(tbSearchArtists.Text))
                    {
                        tvUploads.SelectedNode = artistNode;
                        artistNode.BackColor = ColourHelper.HexStringToColor("#8eea88");
                        heightlightedNode = artistNode;
                        ScrollUploadsArtistNodeToCentre(heightlightedNode);
                        tvUploads.SelectedNode = null;
                        _currentMatchNodeIndex = 0;
                        break;
                    }
                }

                if (heightlightedNode != null)
                {
                    foreach (TreeNode artistNode in tvUploads.Nodes[1].Nodes)
                    {
                        if (artistNode.Text.ToLower().Contains(tbSearchArtists.Text))
                            _tnMatchList.Add(artistNode);

                        if (artistNode != heightlightedNode)
                            artistNode.BackColor = Color.White;
                    }
                }
            }

            tvUploads.AfterSelect += TvUploads_AfterSelect;
        }

        private void TbSearchArtists_KeyDown(object sender, KeyEventArgs e)
        {
            tvUploads.AfterSelect -= TvUploads_AfterSelect;
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                _currentMatchNodeIndex++;
                if (_tnMatchList.Count > 1)
                {
                    if (_currentMatchNodeIndex == _tnMatchList.Count)
                        _currentMatchNodeIndex = 0;

                    tvUploads.SelectedNode = _tnMatchList[_currentMatchNodeIndex];
                    _tnMatchList[_currentMatchNodeIndex].BackColor = ColourHelper.HexStringToColor("#8eea88");
                    ScrollUploadsArtistNodeToCentre(_tnMatchList[_currentMatchNodeIndex]);
                    tvUploads.SelectedNode = null;

                    foreach (TreeNode artistNode in tvUploads.Nodes[1].Nodes)
                    {
                        if (artistNode != _tnMatchList[_currentMatchNodeIndex])
                            artistNode.BackColor = Color.White;
                    }
                }
            }

            tvUploads.AfterSelect += TvUploads_AfterSelect;
        }
    }
}
