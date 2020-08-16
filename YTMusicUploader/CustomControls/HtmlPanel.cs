using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Html;
using System.Drawing.Text;
using System.Reflection;
using System.Windows.Forms;

namespace JBToolkit.WinForms
{
    /// <summary>
    /// Provides HTML rendering on a panel
    /// </summary>
    public class HtmlPanel
        : ScrollableControl
    {
        #region Fields

        public InitialHtmlContainer _htmlContainer;

        #endregion

        #region Ctor

        /// <summary>
        ///     Creates a new HtmlPanel
        /// </summary>
        protected HtmlPanel()
        {
            _htmlContainer = new InitialHtmlContainer();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque, true);
            //SetStyle(ControlStyles.Selectable, true);

            DoubleBuffered = true;

            BackColor = SystemColors.Window;
            AutoScroll = true;

            HtmlRenderer.AddReference(Assembly.GetCallingAssembly());
        }

        #endregion

        #region Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set { base.AutoSize = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoScroll
        {
            get { return base.AutoScroll; }
            set { base.AutoScroll = value; }
        }

        /// <summary>
        ///     Gets the Initial HtmlContainer of this HtmlPanel
        /// </summary>
        public InitialHtmlContainer HtmlContainer => _htmlContainer;

        /// <summary>
        ///     Gets or sets the text of this panel
        /// </summary>
        [Editor(
            "System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",
            typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Localizable(true)]
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;

                CreateFragment();
                MeasureBounds();
                Invalidate();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Creates the fragment of HTML that is rendered
        /// </summary>
        private void CreateFragment()
        {
            _htmlContainer = new InitialHtmlContainer(Text);
        }

        /// <summary>
        ///     Measures the bounds of the container
        /// </summary>
        public virtual void MeasureBounds()
        {
            _htmlContainer.SetBounds(ClientRectangle);

            using (var g = CreateGraphics())
            {
                _htmlContainer.MeasureBounds(g);
            }

            AutoScrollMinSize = Size.Round(_htmlContainer.MaximumSize);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            Focus();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            MeasureBounds();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(SystemColors.Window);

            _htmlContainer.ScrollOffset = AutoScrollPosition;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            _htmlContainer.Paint(e.Graphics, true);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            foreach (var box in _htmlContainer.LinkRegions.Keys)
            {
                var rect = _htmlContainer.LinkRegions[box];
                if (Rectangle.Round(rect).Contains(e.X, e.Y))
                {
                    Cursor = Cursors.Hand;
                    return;
                }
            }

            Cursor = Cursors.Default;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            foreach (var box in _htmlContainer.LinkRegions.Keys)
            {
                var rect = _htmlContainer.LinkRegions[box];
                if (Rectangle.Round(rect).Contains(e.X, e.Y))
                {
                    CssValue.GoLink(box.GetAttribute("href", string.Empty));
                    return;
                }
            }
        }

        #endregion
    }
}