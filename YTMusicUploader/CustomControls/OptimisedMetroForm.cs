using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using YTMusicUploader;

namespace JBToolkit.WinForms
{
    /// <summary>
    /// Abstract form to inherit that uses the MetroForms styler, and optimises buffering, drawing, size grips, resizing
    /// shadow, allows custom tooltip for each control by putting a string in the control's 'Tag' property and handles
    /// client screen DPI not set to 100% more effectively.
    /// 
    /// Be sure to call OnLoad(e) / OnLoad(null) in calling Form contructor or 'Load' method.
    /// </summary>
    [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<OptimisedMetroForm, MetroForm>))]
    public abstract class OptimisedMetroForm : MetroForm
    {
        #region System Presentation

        #region Suspend and Resume Layout

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private const int WM_SETREDRAW = 11;

        public MainForm MainFormInstance { get; set; }

        /// <summary>
        /// Suspend the drawing of a control
        /// </summary>
        public static void SuspendDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }

        /// <summary>
        /// Resume the drawing of a control
        /// </summary>
        public static void ResumeDrawing(Control parent)
        {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }

        #endregion

        #region Windows Presentation - Snapping and Grips and anti-flicker

        protected override void WndProc(ref Message message)
        {
            try
            {
                if (message.Msg == WM_SYSCOMMAND && (message.WParam.ToInt32() & 0xfff0) == SC_SIZE)
                {
                    if (FormResizable)
                    {
                        GetSystemParametersInfo(SPI_GETDRAGFULLWINDOWS, 0, out int isDragFullWindow, 0);

                        if (isDragFullWindow != 0)
                        {
                            SetSystemParametersInfo(SPI_SETDRAGFULLWINDOWS, 0, 0, 0);
                        }

                        base.WndProc(ref message);

                        if (isDragFullWindow != 0)
                        {
                            SetSystemParametersInfo(SPI_SETDRAGFULLWINDOWS, 1, 0, 0);
                        }
                    }
                }
                else
                {
                    if (message.Msg == 0x84) // WM_NCHITTEST
                    {
                        if (FormResizable)
                        {
                            // Always add grid styles regardless of border type

                            var cursor = PointToClient(Cursor.Position);

                            if (TopLeft.Contains(cursor))
                            {
                                message.Result = (IntPtr)HTTOPLEFT;
                            }
                            else if (TopRight.Contains(cursor))
                            {
                                message.Result = (IntPtr)HTTOPRIGHT;
                            }
                            else if (BottomLeft.Contains(cursor))
                            {
                                message.Result = (IntPtr)HTBOTTOMLEFT;
                            }
                            else if (BottomRight.Contains(cursor))
                            {
                                message.Result = (IntPtr)HTBOTTOMRIGHT;
                            }
                            else if (Top.Contains(cursor))
                            {
                                message.Result = (IntPtr)HTTOP;
                            }
                            else if (Left.Contains(cursor))
                            {
                                message.Result = (IntPtr)HTLEFT;
                            }
                            else if (Right.Contains(cursor))
                            {
                                message.Result = (IntPtr)HTRIGHT;
                            }
                            else if (Bottom.Contains(cursor))
                            {
                                message.Result = (IntPtr)HTBOTTOM;
                            }
                        }
                        else
                        {
                            Cursor.Current = Cursors.Arrow;
                            message.Result = (IntPtr)1;  // Processed6
                            return;
                        }
                    }

                    base.WndProc(ref message);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "The application encountered a fatal error and must be restarted. Please contact the service desk with this message: " +
                    e.Message, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Variables for borderless form resize

        private const int
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17;

        private const int m_height = 10;

        private new Rectangle Top => new Rectangle(0, 0, ClientSize.Width, m_height);

        private new Rectangle Left => new Rectangle(0, 0, m_height, ClientSize.Height);

        private new Rectangle Bottom => new Rectangle(0, ClientSize.Height - m_height, ClientSize.Width, m_height);

        private new Rectangle Right => new Rectangle(ClientSize.Width - m_height, 0, m_height, ClientSize.Height);

        private Rectangle TopLeft => new Rectangle(0, 0, m_height, m_height);
        private Rectangle TopRight => new Rectangle(ClientSize.Width - m_height, 0, m_height, m_height);
        private Rectangle BottomLeft => new Rectangle(0, ClientSize.Height - m_height, m_height, m_height);

        private Rectangle BottomRight => new Rectangle(ClientSize.Width - m_height, ClientSize.Height - m_height,
            m_height, m_height);

        #endregion

        #region Variables for no content while resizing

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", CharSet = CharSet.Auto)]
        public static extern int GetSystemParametersInfo(int uAction, int uParam, out int lpvParam, int fuWinIni);

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", CharSet = CharSet.Auto)]
        public static extern int SetSystemParametersInfo(int uAction, int uParam, int lpvParam, int fuWinIni);

        private const int SPI_GETDRAGFULLWINDOWS = 38;
        private const int SPI_SETDRAGFULLWINDOWS = 37;
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_SIZE = 0xF000;

        #endregion

        #region Double Buffering and Shadow

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;

                //  cp.ClassStyle |= 0x20000;  // Drop shadow on form
                //  cp.Style |= 0x20000 | 0x80000 | 0x40000; //WS_MINIMIZEBOX | WS_SYSMENU | WS_SIZEBOX;
                //  cp.Style &= ~0x02000000; // Turn off WS_CLIPCHILDREN

                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED    
                cp.Style |= 0x40000; // Form shadow or at least a thin border when no shadow (i.e. remote app) - WS_SIZEBOX; 
                cp.ExStyle |= 0x80; // Prevent show in ALT+Tab

                return cp;
            }
        }

        /// <summary>
        /// Iterates through all controls and ensure they're set to double buffered
        /// </summary>
        public void SetAllControlsDoubleBuffered(Control parentControl)
        {
            var controls = GetAllControls(parentControl);

            foreach (var control in controls)
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, control, new object[] { true });
            }
        }

        /// <summary>
        /// Recursively gets a list of all controls
        /// </summary>
        private List<Control> GetAllControls(Control container)
        {
            return GetAllControls(container, new List<Control>());
        }

        private List<Control> GetAllControls(Control container, List<Control> list)
        {
            foreach (Control c in container.Controls)
            {
                if (c is DataGridView)
                {
                    list.Add(c);
                }

                if (c.Controls.Count > 0)
                {
                    list = GetAllControls(c, list);
                }
            }

            return list;
        }

        #endregion

        #region Text Box Cue Banners

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam,
            [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        /*
            // Example usage to set cue banner: 
            SendMessage(control.Handle, EM_SETCUEBANNER, 1, "This is a cue banner");
        */

        public void SetTextBoxCueBanner(TextBox textbox, string text)
        {
            SendMessage(textbox.Handle, EM_SETCUEBANNER, 1, text);
        }

        public void SetTextBoxCueBanner(RoundTextBox textbox, string text)
        {
            SendMessage(textbox.box.Handle, EM_SETCUEBANNER, 1, text);
        }

        public const int EM_SETCUEBANNER = 0x1501;

        #endregion

        #region FontSizesForDPI

        public int GetScreenDPI()
        {
            int currentDPI = 96;

            using (Graphics g = this.CreateGraphics())
            {
                currentDPI = (int)g.DpiX;
            }

            return currentDPI;
        }

        #endregion

        #endregion

        new public void OnLoad(EventArgs _)
        {
            SetStyle(
               ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
               ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            SetAllControlsDoubleBuffered(this);
            CheckForIllegalCrossThreadCalls = false;

            BorderStyle = MetroFormBorderStyle.None;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            ShadowType = MetroFormShadowType.AeroShadow;

            Resizable = FormResizable;
            MaximizeBox = FormResizable;

            if (FormResizable)
            {
                SizeGripStyle = SizeGripStyle.Show;
            }
            else
            {
                SizeGripStyle = SizeGripStyle.Hide;
            }

            if (HandleCustomClientDPI)
            {
                AutoScaleDimensions = new SizeF(96F, 96F);
                AutoScaleMode = AutoScaleMode.Dpi;
            }

            ResumeDrawing(this);

            Focus();
            Select();
        }

        /// <summary>
        /// A pre-customised and optimised form using the Metro Style.
        /// </summary>
        /// <param name="formResizable">Set's whether or not the form can be resized, and sets up certain properties and border appropriately</param>
        /// <param name="controlTagsAsTooltips">Set's whether or not any text in a control's 'Tag' property is converted to a tooltip - for ease</param>
        /// <param name="handleCustomClientDPI">I.e. 125% or 150% - WinForms isn't that good at dealing with different display DPI settings, however 
        /// setting this to true will 'try' to make the form handle different DPI settings.</param>
        public OptimisedMetroForm(
                bool formResizable = true,
                bool handleCustomClientDPI = false
            )
        {
            SetStyle(
               ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint |
               ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            CheckForIllegalCrossThreadCalls = false;

            FormResizable = formResizable;
            HandleCustomClientDPI = handleCustomClientDPI;

            Resizable = formResizable;
            MaximizeBox = formResizable;

            BorderStyle = MetroFormBorderStyle.None;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            ShadowType = MetroFormShadowType.AeroShadow;

            if (HandleCustomClientDPI)
            {
                AutoScaleDimensions = new SizeF(96F, 96F);
                AutoScaleMode = AutoScaleMode.Dpi;
            }

            if (FormResizable)
            {
                SizeGripStyle = SizeGripStyle.Show;
            }
            else
            {
                SizeGripStyle = SizeGripStyle.Hide;
            }

            ResizeBegin += (s, ex) => { this.SuspendLayout(); };
            ResizeEnd += (s, ex) => { this.ResumeLayout(); };
        }

        private bool FormResizable { get; set; }
        private bool HandleCustomClientDPI { get; set; }
    }

    public class AbstractControlDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
    {
        public AbstractControlDescriptionProvider()
            : base(TypeDescriptor.GetProvider(typeof(TAbstract)))
        {
        }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            if (objectType == typeof(TAbstract))
            {
                return typeof(TBase);
            }

            return base.GetReflectionType(objectType, instance);
        }

        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            if (objectType == typeof(TAbstract))
            {
                objectType = typeof(TBase);
            }

            return base.CreateInstance(provider, objectType, argTypes, args);
        }
    }
}
