using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace JBToolkit.WinForms
{
    public class RoundPictureBox : PictureBox
    {
        public RoundPictureBox()
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Brush brushImage;
            try
            {
                var Imagem = new Bitmap(this.Image);
                Imagem = new Bitmap(Imagem, new Size(this.Width - 1, this.Height - 1));
                brushImage = new TextureBrush(Imagem);
            }
            catch
            {
                var Imagem = new Bitmap(this.Width - 1, this.Height - 1, PixelFormat.Format24bppRgb);
                using (var grp = Graphics.FromImage(Imagem))
                {
                    grp.FillRectangle(
                        Brushes.White, 0, 0, this.Width - 1, this.Height - 1);
                    Imagem = new Bitmap(this.Width - 1, this.Height - 1, grp);
                }
                brushImage = new TextureBrush(Imagem);
            }
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            var path = new GraphicsPath();
            path.AddEllipse(0, 0, this.Width - 1, this.Height - 1);
            e.Graphics.FillPath(brushImage, path);
        }
    }
}
