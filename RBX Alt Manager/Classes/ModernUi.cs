using BrightIdeasSoftware;
using RBX_Alt_Manager.Forms;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace RBX_Alt_Manager.Classes
{
    internal static class ModernUi
    {
        public static readonly Color MainBackground = Color.FromArgb(11, 15, 26);
        public static readonly Color CardBackground = Color.FromArgb(17, 23, 37);
        public static readonly Color CardBackgroundSecondary = Color.FromArgb(20, 27, 42);
        public static readonly Color InputBackground = Color.FromArgb(21, 28, 43);
        public static readonly Color BorderSubtle = Color.FromArgb(42, 50, 68);
        public static readonly Color TextPrimary = Color.FromArgb(244, 246, 251);
        public static readonly Color TextMuted = Color.FromArgb(168, 176, 194);
        public static readonly Color AccentPurple = Color.FromArgb(139, 92, 246);
        public static readonly Color AccentPurpleHover = Color.FromArgb(157, 108, 255);
        public static readonly Color AccentPurpleDown = Color.FromArgb(117, 71, 232);

        public static readonly Font UiFont = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font UiFontBold = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold, GraphicsUnit.Point);
        public static readonly Font SectionTitleFont = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point);

        private static readonly ConditionalWeakTable<Button, object> StyledButtons = new ConditionalWeakTable<Button, object>();
        private static readonly ConditionalWeakTable<CheckBox, object> StyledCheckBoxes = new ConditionalWeakTable<CheckBox, object>();

        public static void Apply(Form form)
        {
            form.Font = UiFont;
            form.BackColor = MainBackground;
            form.ForeColor = TextPrimary;
            Apply(form.Controls);
        }

        public static void Apply(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (!(control.Tag is string tag && tag == "UseControlFont")) control.Font = UiFont;

                if (control is Button button)
                {
                    if (button.Tag is string btag && btag == "Primary" || button.BackColor == AccentPurple)
                    {
                        StylePrimaryButton(button);
                    }
                    else if (button.Tag is string stag && stag == "SmallIcon")
                    {
                        StyleSmallIconButton(button, button.Width > 0 ? button.Width : 30);
                    }
                    else
                    {
                        StyleNeutralButton(button);
                    }
                }
                else if (control is PictureBox pb)
                {
                    if (pb.Tag is string ptag && ptag == "CardIcon" || pb.Name == "HistoryIcon" || pb.Name == "ShuffleIcon")
                    {
                        StyleCardIcon(pb, 30);
                    }
                }
                else if (control is TextBoxBase textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.BackColor = InputBackground;
                    textBox.ForeColor = TextPrimary;
                    textBox.Margin = new Padding(0, 4, 0, 4);
                }
                else if (control is NumericUpDown numeric)
                {
                    numeric.BorderStyle = BorderStyle.FixedSingle;
                    numeric.BackColor = InputBackground;
                    numeric.ForeColor = TextPrimary;
                }
                else if (control is CheckBox checkBox)
                {
                    checkBox.FlatStyle = FlatStyle.Flat;
                    checkBox.FlatAppearance.BorderSize = 1;
                    checkBox.FlatAppearance.BorderColor = BorderSubtle;
                    checkBox.FlatAppearance.CheckedBackColor = AccentPurple;
                    checkBox.FlatAppearance.MouseOverBackColor = CardBackgroundSecondary;
                    checkBox.UseVisualStyleBackColor = false;
                    ApplyCheckBoxState(checkBox);
                    if (!StyledCheckBoxes.TryGetValue(checkBox, out _))
                    {
                        StyledCheckBoxes.Add(checkBox, new object());
                        checkBox.CheckedChanged += (sender, args) => ApplyCheckBoxState(checkBox);
                    }
                }
                else if (control is ComboBox comboBox)
                {
                    comboBox.FlatStyle = FlatStyle.Flat;
                    comboBox.BackColor = InputBackground;
                    comboBox.ForeColor = TextPrimary;
                }
                else if (control is ObjectListView olv)
                {
                    StyleObjectListView(olv);
                }
                else if (control is ListView listView)
                {
                    listView.BorderStyle = BorderStyle.None;
                    listView.BackColor = CardBackground;
                    listView.ForeColor = TextPrimary;
                    listView.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
                }
                else if (control is TabControl tabControl)
                {
                    tabControl.BackColor = MainBackground;
                    tabControl.ForeColor = TextPrimary;
                    foreach (TabPage page in tabControl.TabPages)
                    {
                        page.UseVisualStyleBackColor = false;
                        page.BackColor = MainBackground;
                        page.Padding = Padding.Empty;
                        page.Margin = Padding.Empty;
                    }
                }
                else if (control is TabPage tabPage)
                {
                    tabPage.UseVisualStyleBackColor = false;
                    tabPage.BackColor = MainBackground;
                    tabPage.Padding = Padding.Empty;
                    tabPage.Margin = Padding.Empty;
                }
                else if (control is Label label)
                {
                    label.BackColor = Color.Transparent;
                }
                else if (control is ContextMenuStrip menu)
                {
                    menu.BackColor = InputBackground;
                    menu.ForeColor = TextPrimary;
                    menu.Renderer = new ToolStripProfessionalRenderer(new ModernColorTable());
                    menu.Padding = new Padding(4);
                }

                if (control.HasChildren) Apply(control.Controls);
            }
        }

        private static void ApplyCheckBoxState(CheckBox checkBox)
        {
            checkBox.BackColor = checkBox.Checked ? AccentPurpleDown : Color.Transparent;
            checkBox.ForeColor = checkBox.Checked ? Color.White : TextPrimary;
        }

        public static Panel CreateCardPanel(Padding? padding = null)
        {
            return new Panel
            {
                BackColor = CardBackground,
                BorderStyle = BorderStyle.None,
                Padding = padding ?? new Padding(18, 14, 18, 16),
                Margin = new Padding(0, 0, 0, 16)
            };
        }

        public static Label CreateSectionTitle(string text) => new Label
        {
            AutoSize = true,
            Text = text.ToUpperInvariant(),
            Font = SectionTitleFont,
            ForeColor = AccentPurple,
            BackColor = Color.Transparent,
            Margin = new Padding(0, 0, 0, 10)
        };

        public static Label CreateFieldLabel(string text) => new Label
        {
            AutoSize = true,
            Text = text,
            Font = UiFont,
            ForeColor = TextPrimary,
            BackColor = Color.Transparent,
            Margin = new Padding(0, 5, 8, 0)
        };

        public static void StylePrimaryButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = AccentPurple;
            button.ForeColor = Color.White;
            button.FlatAppearance.MouseOverBackColor = AccentPurpleHover;
            button.FlatAppearance.MouseDownBackColor = AccentPurpleDown;
            button.Cursor = Cursors.Hand;
            button.Font = UiFontBold;
            if (button.Height < 32) button.Height = 32;
            Round(button, 8);

            if (!StyledButtons.TryGetValue(button, out _))
            {
                StyledButtons.Add(button, new object());
                button.Resize += (sender, args) => Round((Button)sender, 8);
            }
        }

        public static void StyleNeutralButton(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = BorderSubtle;
            button.BackColor = InputBackground;
            button.ForeColor = TextPrimary;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(32, 42, 64);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(17, 23, 37);
            button.Cursor = Cursors.Hand;
            if (button.Height < 32) button.Height = 32;
            Round(button, 8);

            if (!StyledButtons.TryGetValue(button, out _))
            {
                StyledButtons.Add(button, new object());
                button.Resize += (sender, args) => Round((Button)sender, 8);
            }
        }

        public static void StyleSmallIconButton(Button button, int size = 30)
        {
            button.Size = new Size(size, size);
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = BorderSubtle;
            button.BackColor = InputBackground;
            button.ForeColor = TextPrimary;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(32, 42, 64);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(17, 23, 37);
            button.Cursor = Cursors.Hand;
            button.Anchor = AnchorStyles.None;
            button.Margin = Padding.Empty;
            Round(button, 6);

            if (!StyledButtons.TryGetValue(button, out _))
            {
                StyledButtons.Add(button, new object());
                button.Resize += (sender, args) => Round((Button)sender, 6);
            }
        }

        public static void StyleCardIcon(PictureBox pb, int size = 30)
        {
            pb.Size = new Size(size, size);
            pb.SizeMode = PictureBoxSizeMode.Zoom;
            pb.Padding = new Padding(5);
            pb.BackColor = InputBackground;
            pb.Anchor = AnchorStyles.None;
            pb.Cursor = Cursors.Hand;
            pb.Margin = Padding.Empty;
            Round(pb, 6);
            pb.Paint -= CardIcon_Paint;
            pb.Paint += CardIcon_Paint;
        }

        private static void CardIcon_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Control control)
            {
                using (Pen p = new Pen(BorderSubtle, 1f))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    e.Graphics.DrawRectangle(p, 0, 0, control.Width - 1, control.Height - 1);
                }
            }
        }

        public static void StyleObjectListView(ObjectListView olv)
        {
            olv.BackColor = CardBackground;
            olv.ForeColor = TextPrimary;
            olv.BorderStyle = BorderStyle.None;
            olv.GridLines = false;
            olv.FullRowSelect = true;
            olv.RowHeight = 32;
            olv.UseAlternatingBackColors = true;
            olv.AlternateRowBackColor = CardBackgroundSecondary;
            olv.UseCustomSelectionColors = true;
            olv.SelectedBackColor = AccentPurple;
            olv.SelectedForeColor = Color.White;
            olv.UnfocusedSelectedBackColor = BorderSubtle;
            olv.UnfocusedSelectedForeColor = TextPrimary;
            olv.HeaderStyle = ThemeEditor.ShowHeaders ? ColumnHeaderStyle.Clickable : ColumnHeaderStyle.None;
        }

        public static void Round(Control control, int radius)
        {
            if (control.Width <= radius || control.Height <= radius) return;
            using (GraphicsPath path = new GraphicsPath())
            {
                int diameter = radius * 2;
                Rectangle bounds = new Rectangle(0, 0, control.Width - 1, control.Height - 1);
                path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);
                path.AddArc(bounds.Right - diameter, bounds.Top, diameter, diameter, 270, 90);
                path.AddArc(bounds.Right - diameter, bounds.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(bounds.Left, bounds.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();
                control.Region?.Dispose();
                control.Region = new Region(path);
            }
        }

        private sealed class ModernColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected => ControlPaint.Light(ThemeEditor.ButtonsBackground, 0.1F);
            public override Color MenuItemBorder => ThemeEditor.ButtonsBorder;
            public override Color ToolStripDropDownBackground => ThemeEditor.ButtonsBackground;
            public override Color ImageMarginGradientBegin => ThemeEditor.ButtonsBackground;
            public override Color ImageMarginGradientMiddle => ThemeEditor.ButtonsBackground;
            public override Color ImageMarginGradientEnd => ThemeEditor.ButtonsBackground;
        }
    }
}
