using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Html;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace JBToolkit.WinForms
{
    /// <summary>
    /// Nicer Custom Tooltip for controls
    /// </summary>
    [ToolboxBitmap(typeof(ToolTip))]
    public class CustomToolTip : ToolTip
    {
        private const float _fSz = 9F;

        private static string _tdsc = string.Empty;
        private static Font _dscF = new Font("Segoe UI", _fSz, FontStyle.Regular);
        private static Font _uiF = new Font("Segoe UI", _fSz, FontStyle.Bold);

        private InitialHtmlContainer container;

        private readonly List<ToolTipControl> ToolTipControls = new List<ToolTipControl>();

        private string ToolTipTitleText = string.Empty, ToolTipDecription = string.Empty;

        public CustomToolTip()
        {
            base.OwnerDraw = true;
            Popup += OnPopup;
            Draw += OnDraw;
            AutoPopDelay = 6000;
            InitialDelay = 1000;
            ReshowDelay = 10;
            ShowAlways = true;
        }

        // For customizing a ToolTip there are two main values that must be set as follows:
        // 'OwnerDraw = true' & 'IsBalloon = false'
        // So our ToolTip is going to be customized, then let's hide those two properties
        // from 'Editor' & 'IntelliSense' for avoiding any possible styling errors!
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool OwnerDraw { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool IsBalloon { get; set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Color ForeColor { get; set; }

        /// <summary>
        ///     Gets or sets the background color for the ToolTip.
        ///     No background color is taken into account when a background image is set!
        /// </summary>
        [Description(
            "Gets or sets the background color for the ToolTip.\n\nNote: No background color will be taken into account if a background image is set!")]
        public new Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        /// <summary>
        ///     Specifies a title for the ToolTip to use when it is about to be desplayed
        ///     but only when no title was set for a control in the ToolTip!
        /// </summary>
        [DefaultValue("ToolTip title")]
        public new string ToolTipTitle { get; set; } = string.Empty;

        /// <summary>
        ///     In order to set this you must set the AutoSize value to false.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Size Size { get; set; } = new Size(100, 70);

        /// <summary>
        ///     Specifies whether the ToolTip will calculate its size automatically, or
        ///     will use a specified size.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool AutoSize { get; set; } = true;

        /// <summary>
        ///     Gets or sets the ToolTip description content.
        /// </summary>
#pragma warning disable IDE0051 // Remove unused private members
        private string ToolTipDescription
#pragma warning restore IDE0051 // Remove unused private members
        {
            get { return _tdsc; }
            set
            {
                var _tmp = value;
                while (_tmp.Contains("  "))
                {
                    _tmp = _tmp.Replace("  ", " ");
                }

                while (_tmp.Contains("\n\n"))
                {
                    _tmp = _tmp.Replace("\n\n", "\n");
                }

                _tdsc = _tmp;
            }
        }

        /// <summary>
        ///     Specifies a Font value for the title on the ToolTip.
        /// </summary>
        public Font TitleFont
        {
            get { return _uiF; }
            set { _uiF = value; }
        }

        /// <summary>
        ///     Specifies a Font value for the description on the ToolTip.
        /// </summary>
        public Font DescriptionFont
        {
            get { return _dscF; }
            set { _dscF = value; }
        }

        // Summary:
        //      This sets a maximum value for the width of the ToolTip.
        //      For desabling MaxWidth reset this to a 0 value.
        //      If this value is smaller than the calculated width of the title text the width of the title will be used as a minimum width for the ToolTip. 
        // Note: 
        //      by assigning a value to this please avoid using 'new line' characters, sucj as '\n' character.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int MaxWidth { get; set; } = 0;


        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool StripAmpersands { get; set; }

        /// <summary>
        ///     Associates ToolTip title and description with the specified control.
        /// </summary>
        /// <param name="control">The System.Windows.Forms.Control to associate the ToolTip text with.</param>
        /// <param name="description">The ToolTip description to display when the the ToolTip is shown over the specified control.</param>
        /// <param name="title">The ToolTip title to display when the ToolTip is shown.</param>
        /// <param name="duration">Time in ms into tooltips faddes</param>
        public void SetToolTip(Control control, string description, string title, int duration = 6000)
        {
            AutoPopDelay = duration;

            if (title == null)
            {
                title = string.Empty;
            }

            var _found = false;
            foreach (ToolTipControl ttControl in ToolTipControls)
            {
                var ctrl = ttControl;
                if (ctrl.Control == control)
                {
                    _found = true;
                    ctrl.Title = title;
                    ctrl.Description = description;
                }
            }

            if (!_found)
            {
                ToolTipControls.Add(new ToolTipControl
                {
                    Control = control,
                    Title = title,
                    Description = description
                });
            }

            base.SetToolTip(control, description);
        }

        /// <summary>
        ///     Associates ToolTip title and description with the specified control.
        /// </summary>
        /// <param name="control">The System.Windows.Forms.Control to associate the ToolTip text with.</param>
        /// <param name="description">The ToolTip description to display when the the ToolTip is shown over the specified control.</param>
        public new void SetToolTip(Control control, string description)
        {
            var _found = false;

            foreach (ToolTipControl ttControl in ToolTipControls)
            {
                var ctrl = ttControl;
                if (ctrl.Control == control)
                {
                    _found = true;
                    ctrl.Description = description;
                }
            }

            if (!_found)
            {
                var tt = new ToolTipControl
                {
                    Control = control,
                    Title = string.Empty,
                    Description = description
                };


                // ££ - Allows us to use whatever is in the 'tag' part of a control but still allow things like 'title' and 'duration'

                if (description.Contains("-££-"))
                {
                    var parts = Regex.Split(description, "-££-");

                    tt.Title = parts[0];
                    tt.Description = parts[1];

                    if (parts.Length > 3)
                    {
                        try
                        {
                            AutomaticDelay = Convert.ToInt32(parts[3]);
                        }
                        catch
                        {
                            AutoPopDelay = 5000;
                        }
                    }
                }

                ToolTipControls.Add(tt);
            }

            base.SetToolTip(control, description);
        }

        /// <summary>
        ///     Removes the associated ToolTip from the specified control.
        /// </summary>
        /// <param name="control">The System.Windows.Forms.Control to associate the ToolTip text with.</param>
        public void RemoveToolTip(Control control)
        {
            foreach (var x in ToolTipControls)
            {
                if (x.Control.Equals(control))
                {
                    ToolTipControls.Remove(x);
                }
            }
        }

        /// <summary>
        ///     Gets the associated ToolTip with the specified control.
        /// </summary>
        public new ToolTipControl GetToolTip(Control control)
        {
            var n = new ToolTipControl();
            foreach (var x in ToolTipControls)
            {
                if (x.Control == control)
                {
                    n = x;
                }
            }

            return n;
        }

        private void OnPopup(object sender, PopupEventArgs e)
        {
            foreach (var __tCtrl in ToolTipControls)
            {
                var ctrl = __tCtrl.Control;
                if (ctrl == e.AssociatedControl)
                {
                    // Set the appropriate title
                    ToolTipTitleText = __tCtrl.Title.Trim() == string.Empty ? ToolTipTitle : __tCtrl.Title;

                    // Set the description
                    ToolTipDecription = __tCtrl.Description;
                }
            }

            var text = ToolTipDecription;
            var font = string.Format(NumberFormatInfo.InvariantInfo, "font: {0}pt {1}", _fSz,
                DescriptionFont.FontFamily.Name);

            if (!string.IsNullOrEmpty(ToolTipTitleText))
            {
                text = "<b>" + ToolTipTitleText + "</b>" + @"<hr><p style=""margin-top: 10px; margin-bottom: 2px;"">" + text;
            }

            //Create fragment container
            container = new InitialHtmlContainer(
                "<table class=htmltooltipbackground cellspacing=5 cellpadding=5 style=\"" + font +
                "\"><tr><td style=border:0px>" + text.Replace("<br />", "<p style=\"margin-top: -1; margin-bottom:-2\"></p>").
                        Replace("<br/>", "<p style=\"margin-top: -1; margin-bottom:-2\"></p>") + "</td></tr></table>")
            { AvoidGeometryAntialias = true };

            //Measure bounds of the container
            using (var g = e.AssociatedControl.CreateGraphics())
            {
                container.MeasureBounds(g);
            }

            //Set the size of the tooltip
            e.ToolTipSize = Size.Round(container.MaximumSize);
        }

        private void OnDraw(object sender, DrawToolTipEventArgs e) // use this event to customise the tool tip
        {
            var g = e.Graphics;
            g.Clear(BackColor);

            if (container != null)
            {
                container.Paint(e.Graphics, false);
            }
        }

        /// <summary>
        ///     Provides information about ToolTips that are associated with their controls.
        ///     This structure is used specifically with the 'GetToolTip(control)' method.
        /// </summary>
        public struct ToolTipControl
        {
            public Control Control;
            public string Title;
            public string Description;
        }
    }
}