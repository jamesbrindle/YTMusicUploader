using System.ComponentModel;
using System.Drawing;
using System.Drawing.Html;
using System.Windows.Forms;

namespace JBToolkit.WinForms
{
    /// <summary>
    /// Provides HTML rendering on the text of the label
    /// </summary>
    public sealed class HtmlLabel
        : HtmlPanel
    {
        #region Ctor

        /// <summary>
        ///     Creates a new HTML Label
        /// </summary>
        public HtmlLabel()
        {
            SetStyle(ControlStyles.Opaque, false);

            AutoScroll = false;
        }

        #endregion

        #region Properties

        [DefaultValue(true)]
        [Description("Automatically sets the size of the label by measuring the content")]
        [Browsable(true)]
        public override bool AutoSize
        {
            get { return base.AutoSize; }
            set
            {
                base.AutoSize = value;

                if (value)
                {
                    MeasureBounds();
                }
            }
        }

        #endregion

        #region Fields

        #endregion

        #region Methods

#pragma warning disable IDE0051 // Remove unused private members
        private void CreateFragment()
#pragma warning restore IDE0051 // Remove unused private members
        {
            var text = Text;
            var font = $"font: {Font.Size}pt {Font.FontFamily.Name}";

            //Create fragment container
            _htmlContainer = new InitialHtmlContainer("<table border=0 cellspacing=5 cellpadding=0 style=\"" + font +
                                                      "\"><tr><td>" + text + "</td></tr></table>");
            //_htmlContainer.SetBounds(new Rectangle(0, 0, 10, 10));
        }

        public override void MeasureBounds()
        {
            base.MeasureBounds();

            if (_htmlContainer != null && AutoSize)
            {
                Size = Size.Round(_htmlContainer.MaximumSize);
            }
        }

        #endregion
    }
}