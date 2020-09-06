using System.Threading;
using System.Windows.Forms;
using YTMusicUploader.Providers.RequestModels;

namespace YTMusicUploader.Dialogues
{
    public partial class ManageYTMusic
    {
        private void TvUploads_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedNode = e.Node;
            if (selectedNode != null)
            {
                if (((MusicManageTreeNodeModel)selectedNode.Tag).NodeType == MusicManageTreeNodeModel.NodeTypeEnum.Artist)
                {
                    if (selectedNode.Nodes == null || selectedNode.Nodes.Count == 0)
                    {
                        new Thread((ThreadStart)delegate
                        {
                            GetAlbums(selectedNode.Name, ((MusicManageTreeNodeModel)selectedNode.Tag).ArtistTitle);
                        }).Start();
                    }
                }

                SetMusicDetails((MusicManageTreeNodeModel)selectedNode.Tag);
            }
        }

        private void TvUploads_AfterCheck(object sender, TreeViewEventArgs e)
        {
            CheckAllChildNodes(e.Node, e.Node.Checked);
            if (!e.Node.Checked)
            {
                tvUploads.AfterCheck -= TvUploads_AfterCheck;
                UncheckParentNodes(e.Node);
                tvUploads.AfterCheck += TvUploads_AfterCheck;
            }
            else
            {
                // If all child nodes checked, then check parent node
                tvUploads.AfterCheck -= TvUploads_AfterCheck;
                CheckParentNodes(e.Node);
                tvUploads.AfterCheck += TvUploads_AfterCheck;
            }
        }
    }
}
