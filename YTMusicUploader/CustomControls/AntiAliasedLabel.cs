using System.Drawing;
using System.Windows.Forms;

namespace JBToolkit.WinForms
{
    /// <summary>
    /// Adds better anti-aliasing to a label for better visuals
    /// Warning: Can look fuzzy when used in certain places
    /// </summary>
    public class AntiAliasedLabel : Label
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), 0, 0);
        }
    }
}
