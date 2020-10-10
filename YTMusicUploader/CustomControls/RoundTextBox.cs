using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace JBToolkit.WinForms
{
    /// <summary>
    /// Nicer looking 'round' edge text box
    /// </summary>
    public sealed class RoundTextBox : Control
    {
        private readonly int radius = 5;
        public TextBox box = new TextBox();
        private Color br;
        private GraphicsPath innerRect;
        private GraphicsPath shape;

        public RoundTextBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            box.Parent = this;
            Controls.Add(box);

            //    box.BorderStyle = BorderStyle.None;
            box.TextAlign = HorizontalAlignment.Left;
            box.Font = Font;
            box.PasswordChar = '*';
            box.ReadOnly = false;

            BackColor = Color.Transparent;
            ForeColor = Color.Black;
            br = Color.White;
            box.BackColor = br;
            box.BorderStyle = BorderStyle.None;
            Text = null;
            Font = new Font("Segoe UI", 8);
            Size = new Size(135, 25);
            DoubleBuffered = true;
            box.KeyDown += Box_KeyDown;
            box.TextChanged += Box_TextChanged;
            box.MouseDoubleClick += Box_MouseDoubleClick;
        }

        public Color Br
        {
            get { return br; }
            set
            {
                br = value;
                if (br != Color.Transparent)
                {
                    box.BackColor = br;
                }

                Invalidate();
            }
        }

        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                br = value;
                base.BackColor = Color.Transparent;
            }
        }

        public HorizontalAlignment TextAlign
        {

            get { return box.TextAlign; }
            set
            {
                box.TextAlign = value;
            }
        }

        public bool ReadOnly
        {
            get { return box.ReadOnly; }
            set
            {
                box.ReadOnly = value;
            }
        }

        public char PasswordChar
        {
            get { return box.PasswordChar; }
            set
            {
                box.PasswordChar = value;
            }
        }

        private void Box_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            box.SelectAll();
        }

        private void Box_TextChanged(object sender, EventArgs e)
        {
            Text = box.Text;
        }

        public void SelectAll()
        {
            box.SelectAll();
        }

        private void Box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                box.SelectionStart = 0;
                box.SelectionLength = Text.Length;
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            box.Text = Text;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            box.Text = Text;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            box.Font = Font;
            Invalidate();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            box.ForeColor = ForeColor;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            shape = new RoundedRectangleF(Width, Height, radius).Path;
            innerRect = new RoundedRectangleF(Width - 0.5f, Height - 0.5f, radius, 0.5f, 0.5f).Path;
            if (box.Height >= Height - 4)
            {
                Height = box.Height + 4;
            }

            box.Location = new Point(radius, Height / 2 - box.Font.Height / 2);
            box.Width = Width - (int)(radius * 1.5);

            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.DrawPath(Pens.Gray, shape);
            using (var brush = new SolidBrush(br))
            {
                e.Graphics.FillPath(brush, innerRect);
            }

            ColourHelper.MakeTransparent(this, e.Graphics);

            base.OnPaint(e);
        }
    }
}