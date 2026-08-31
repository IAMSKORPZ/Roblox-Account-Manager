using System;
using System.Drawing;
using System.Windows.Forms;

namespace Roblox_Account_Manager_Updater
{
    public class UpdaterForm : Form
    {
        private Label titleLabel;
        private Label statusLabel;
        private Label detailLabel;
        private ProgressBar progressBar;
        private Panel cardPanel;

        public UpdaterForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(460, 165);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.Text = "Roblox Account Manager Updater";
            this.BackColor = Color.FromArgb(11, 15, 26); // #0B0F1A
            this.ForeColor = Color.FromArgb(244, 246, 251); // #F4F6FB
            this.Padding = new Padding(12);

            cardPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(17, 23, 37), // #111725
                Padding = new Padding(16, 12, 16, 12)
            };

            titleLabel = new Label
            {
                Text = "Updating Roblox Account Manager",
                Font = new Font("Segoe UI", 11.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(139, 92, 246), // #8B5CF6
                Dock = DockStyle.Top,
                Height = 26
            };

            statusLabel = new Label
            {
                Text = "Waiting for Roblox Account Manager to close...",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(244, 246, 251),
                Dock = DockStyle.Top,
                Height = 22
            };

            detailLabel = new Label
            {
                Text = "User settings and accounts will be preserved.",
                Font = new Font("Segoe UI", 8.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(168, 176, 194), // #A8B0C2
                Dock = DockStyle.Top,
                Height = 20
            };

            progressBar = new ProgressBar
            {
                Dock = DockStyle.Bottom,
                Height = 18,
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30
            };

            cardPanel.Controls.Add(detailLabel);
            cardPanel.Controls.Add(statusLabel);
            cardPanel.Controls.Add(titleLabel);
            cardPanel.Controls.Add(progressBar);

            this.Controls.Add(cardPanel);
        }

        public void SetStatus(string status, string detail = null, int? percent = null)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => SetStatus(status, detail, percent)));
                return;
            }

            if (!string.IsNullOrEmpty(status)) statusLabel.Text = status;
            if (!string.IsNullOrEmpty(detail)) detailLabel.Text = detail;

            if (percent.HasValue)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
                progressBar.Value = Math.Max(0, Math.Min(100, percent.Value));
            }
        }
    }
}
