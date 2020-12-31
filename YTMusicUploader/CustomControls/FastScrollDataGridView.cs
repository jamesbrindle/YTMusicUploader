using System;
using System.Reflection;
using System.Windows.Forms;

namespace JBToolkit.WinForms
{
    /// <summary>
    /// Must smoother scrolling DataGridView wrapper control
    /// </summary>
    public class FastScrollDataGridView : DataGridView
    {
        private readonly int numberOfRowsPerScroll = 20;
        private int zDelta = 0;

        private int int32 = 0;
        private int shifted = 0;

        public bool DisableScroll { get; set; } = false;

        short GET_WHEEL_DELTA_WPARAM(IntPtr wParam)
        {
            int32 = (int)wParam.ToInt64();
            shifted = int32 >> 16;

            return (short)shifted;
        }

        protected override void WndProc(ref Message m)
        {

            if (m.Msg == 0x20a) //WM_MOUSEWHEEL = 0x20a
            {
                if (!DisableScroll)
                {
                    zDelta = GET_WHEEL_DELTA_WPARAM(m.WParam) > 0 ? numberOfRowsPerScroll : -numberOfRowsPerScroll;

                    if (zDelta < 0) // mouse wheel down
                    {
                        try
                        {
                            this.FirstDisplayedScrollingRowIndex += 1;
                        }
                        catch { }
                    }
                    else
                    {
                        // mouse wheel up
                        try
                        {
                            var verticalOffset = this.GetType().GetProperty("VerticalOffset", BindingFlags.NonPublic | BindingFlags.Instance);
                            verticalOffset.SetValue(this, this.VerticalScrollingOffset - 2, null);
                        }
                        catch { }
                    }


                }

                return;
            }

            base.WndProc(ref m);
        }
    }
}
