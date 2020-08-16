using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JBToolkit.WinForms
{
    /// <summary>
    /// Event handling for 'scoll' of listbox - Can come in handy, say if you want to scroll two list boxes simultaneously
    /// </summary>
    public class ScrollingListBox : ListBox
    {
        [Category("Action")]
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        public event ScrollEventHandler OnHorizontalScroll;
        public event ScrollEventHandler OnVerticalScroll;

        private const int SB_ENDSCROLL = 8;
        private const int SIF_TRACKPOS = 0x10;
        private const int SIF_RANGE = 0x1;
        private const int SIF_POS = 0x4;
        private const int SIF_PAGE = 0x2;
        private const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetScrollInfo(
        IntPtr hWnd, int n, ref ScrollInfoStruct lpScrollInfo);

        private struct ScrollInfoStruct
        {
            public int cbSize;
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }
        protected override void WndProc(ref System.Windows.Forms.Message msg)
        {
            if (msg.Msg == WM_HSCROLL)
            {
                if (OnHorizontalScroll != null)
                {
                    ScrollInfoStruct si = new ScrollInfoStruct
                    {
                        fMask = SIF_ALL
                    };
                    si.cbSize = Marshal.SizeOf(si);
                    GetScrollInfo(msg.HWnd, 0, ref si);
                    if (msg.WParam.ToInt32() == SB_ENDSCROLL)
                    {
                        ScrollEventArgs sargs = new ScrollEventArgs(
                        ScrollEventType.EndScroll,
                        si.nPos);
                        OnHorizontalScroll(this, sargs);
                    }
                }
            }
            if (msg.Msg == WM_VSCROLL)
            {
                if (OnVerticalScroll != null)
                {
                    ScrollInfoStruct si = new ScrollInfoStruct
                    {
                        fMask = SIF_ALL
                    };
                    si.cbSize = Marshal.SizeOf(si);
                    GetScrollInfo(msg.HWnd, 0, ref si);
                    if (msg.WParam.ToInt32() == SB_ENDSCROLL)
                    {
                        ScrollEventArgs sargs = new ScrollEventArgs(
                        ScrollEventType.EndScroll,
                        si.nPos);
                        OnVerticalScroll(this, sargs);
                    }
                }
            }
            base.WndProc(ref msg);
        }

#pragma warning disable IDE0051 // Remove unused private members
        private void InitializeComponent()
#pragma warning restore IDE0051 // Remove unused private members
        {
            this.SuspendLayout();
            // 
            // scrolled
            // 
            this.Size = new System.Drawing.Size(120, 95);
            this.ResumeLayout(false);
        }
    }
}