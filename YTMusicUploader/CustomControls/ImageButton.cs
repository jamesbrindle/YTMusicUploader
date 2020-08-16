using System;
using System.Windows.Forms;

namespace JBToolkit.WinForms
{
    /// <summary>
    /// Image as button (with Cursor swap)
    /// </summary>
    public class ImageButton : Button
    {
        protected override bool ShowFocusCues => false;

        protected override void OnMouseEnter(EventArgs e)
        {
            Cursor = Cursors.Hand;

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Cursor = Cursors.Default;

            base.OnMouseLeave(e);
        }
    }
}