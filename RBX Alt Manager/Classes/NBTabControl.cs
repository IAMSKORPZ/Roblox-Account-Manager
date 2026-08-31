using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RBX_Alt_Manager.Classes
{
    internal class NBTabControl : TabControl
    {
        // https://dotnetrix.co.uk/tabcontrol.htm

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        public NBTabControl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                if (components != null)
                    components.Dispose();

            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() =>
            components = new System.ComponentModel.Container();
        #endregion

        #region Interop

        [StructLayout(LayoutKind.Sequential)]
        private struct NMHDR
        {
            public IntPtr HWND;
            public uint idFrom;
            public int code;
            public override String ToString()
            {
                return String.Format("Hwnd: {0}, ControlID: {1}, Code: {2}", HWND, idFrom, code);
            }
        }

        private const int TCN_FIRST = 0 - 550;
        private const int TCN_SELCHANGING = (TCN_FIRST - 2);

        private const int WM_USER = 0x400;
        private const int WM_NOTIFY = 0x4E;
        private const int WM_REFLECT = WM_USER + 0x1C00;

        #endregion

        #region BackColor Manipulation

        private Color m_Backcolor = Color.Empty;
        [Browsable(true), Description("The background color used to display text and graphics in a control.")]
        public override Color BackColor
        {
            get
            {
                if (m_Backcolor.Equals(Color.Empty))
                {
                    if (Parent == null)
                        return Control.DefaultBackColor;
                    else
                        return Parent.BackColor;
                }
                return m_Backcolor;
            }
            set
            {
                if (m_Backcolor.Equals(value)) return;
                m_Backcolor = value;
                Invalidate();

                base.OnBackColorChanged(EventArgs.Empty);
            }
        }

        public bool ShouldSerializeBackColor() => !m_Backcolor.Equals(Color.Empty);

        public override void ResetBackColor()
        {
            m_Backcolor = Color.Empty;
            Invalidate();
        }

        #endregion

        private int HoveredIndex = -1;

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            Invalidate();
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            int newHover = -1;
            for (int i = 0; i < TabCount; i++)
            {
                if (GetTabRect(i).Contains(e.Location))
                {
                    newHover = i;
                    break;
                }
            }
            if (newHover != HoveredIndex)
            {
                HoveredIndex = newHover;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (HoveredIndex != -1)
            {
                HoveredIndex = -1;
                Invalidate();
            }
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                int top = 36;
                if (TabCount > 0)
                {
                    Rectangle lastTab = GetTabRect(TabCount - 1);
                    top = Math.Max(top, lastTab.Bottom + 4);
                }
                return new Rectangle(0, top, Width, Math.Max(0, Height - top));
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            pevent.Graphics.Clear(BackColor);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            e.Graphics.Clear(BackColor);

            if (TabCount <= 0) return;

            // Draw tab header pill buttons
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                for (int index = 0; index < TabCount; index++)
                {
                    PaintTabButton(index, e, sf);
                }
            }
        }

        private void PaintTabButton(int index, PaintEventArgs e, StringFormat sf)
        {
            TabPage tp = TabPages[index];
            Rectangle r = GetTabRect(index);
            r.Inflate(-2, -3);
            if (r.Width <= 4 || r.Height <= 4) return;

            bool isSelected = index == SelectedIndex;
            bool isHovered = index == HoveredIndex;

            Color bg;
            Color fg;
            Font tabFont;

            if (isSelected)
            {
                bg = ModernUi.AccentPurple;
                fg = Color.White;
                tabFont = new Font(Font.FontFamily, 9.5F, FontStyle.Bold);
            }
            else if (isHovered)
            {
                bg = Color.FromArgb(32, 40, 60);
                fg = ModernUi.TextPrimary;
                tabFont = new Font(Font.FontFamily, 9.5F, FontStyle.Regular);
            }
            else
            {
                bg = ModernUi.CardBackgroundSecondary;
                fg = ModernUi.TextMuted;
                tabFont = new Font(Font.FontFamily, 9.5F, FontStyle.Regular);
            }

            using (System.Drawing.Drawing2D.GraphicsPath path = CreateRoundedRect(r, 6))
            using (SolidBrush brush = new SolidBrush(bg))
            using (SolidBrush textBrush = new SolidBrush(tp.Enabled ? fg : Color.FromArgb(100, 110, 125)))
            {
                e.Graphics.FillPath(brush, path);

                if (Alignment == TabAlignment.Left || Alignment == TabAlignment.Right)
                {
                    float RotateAngle = Alignment == TabAlignment.Left ? 270 : 90;
                    PointF cp = new PointF(r.Left + (r.Width >> 1), r.Top + (r.Height >> 1));
                    e.Graphics.TranslateTransform(cp.X, cp.Y);
                    e.Graphics.RotateTransform(RotateAngle);
                    Rectangle rotR = new Rectangle(-(r.Height >> 1), -(r.Width >> 1), r.Height, r.Width);
                    e.Graphics.DrawString(tp.Text, tabFont, textBrush, rotR, sf);
                    e.Graphics.ResetTransform();
                }
                else
                {
                    e.Graphics.DrawString(tp.Text, tabFont, textBrush, r, sf);
                }
            }

            tabFont.Dispose();
        }

        private static System.Drawing.Drawing2D.GraphicsPath CreateRoundedRect(Rectangle bounds, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            int diameter = radius * 2;
            if (bounds.Width < diameter) diameter = bounds.Width;
            if (bounds.Height < diameter) diameter = bounds.Height;
            if (diameter <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
            path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
            path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }

        [Description("Occurs as a tab is being changed.")]
        public event SelectedTabPageChangeEventHandler SelectedIndexChanging;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (WM_REFLECT + WM_NOTIFY))
            {
                NMHDR hdr = (NMHDR)(Marshal.PtrToStructure(m.LParam, typeof(NMHDR)));
                if (hdr.code == TCN_SELCHANGING)
                {
                    TabPage tp = TestTab(PointToClient(Cursor.Position));
                    if (tp != null)
                    {
                        TabPageChangeEventArgs e = new TabPageChangeEventArgs(SelectedTab, tp);
                        if (SelectedIndexChanging != null)
                            SelectedIndexChanging(this, e);
                        if (e.Cancel || tp.Enabled == false)
                        {
                            m.Result = new IntPtr(1);
                            return;
                        }
                    }
                }
            }
            base.WndProc(ref m);
        }

        private TabPage TestTab(Point pt)
        {
            for (int index = 0; index <= TabCount - 1; index++)
                if (GetTabRect(index).Contains(pt.X, pt.Y))
                    return TabPages[index];

            return null;
        }
    }

    public class TabPageChangeEventArgs : EventArgs
    {
        private TabPage _Selected = null;
        private TabPage _PreSelected = null;
        public bool Cancel = false;

        public TabPage CurrentTab => _Selected;
        public TabPage NextTab => _PreSelected;

        public TabPageChangeEventArgs(TabPage CurrentTab, TabPage NextTab)
        {
            _Selected = CurrentTab;
            _PreSelected = NextTab;
        }
    }

    public delegate void SelectedTabPageChangeEventHandler(Object sender, TabPageChangeEventArgs e);
}