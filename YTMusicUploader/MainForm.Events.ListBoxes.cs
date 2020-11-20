using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using YTMusicUploader.Providers.DataModels;

namespace YTMusicUploader
{
    public partial class MainForm
    {
        private void LbWatchFolders_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          Color.FromArgb(255, 147, 147));

            e.DrawBackground();

            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            e.Graphics.DrawString(
                ((WatchFolder)((ListBox)sender).Items[e.Index]).Path,
                e.Font,
                Brushes.Black,
                e.Bounds.X,
                e.Bounds.Y - 1,
                StringFormat.GenericDefault);

            e.DrawFocusRectangle();
        }

        private void LbWatchFolders_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = new Point(e.X, e.Y);
            int index = ((ListBox)sender).IndexFromPoint(pt);

            if (index <= -1)
            {
                ((ListBox)sender).SelectedItems.Clear();
                ((ListBox)sender).SelectedIndex = -1;
            }
        }
    }
}
