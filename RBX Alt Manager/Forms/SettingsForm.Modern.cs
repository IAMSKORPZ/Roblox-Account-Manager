using RBX_Alt_Manager.Classes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RBX_Alt_Manager.Forms
{
    public partial class SettingsForm
    {
        public void InitializeModernLayout()
        {
            ClientSize = new Size(680, 750);
            MinimumSize = new Size(600, 620);
            MaximizeBox = true;
            BackColor = ModernUi.MainBackground;
            ForeColor = ModernUi.TextPrimary;
            Padding = new Padding(16, 12, 16, 16);

            SettingsTC.Dock = DockStyle.Fill;
            SettingsTC.BackColor = ModernUi.MainBackground;
            SettingsTC.Padding = new Point(20, 9);
            SettingsTC.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);

            SetupGeneralTab();
            SetupDeveloperTab();
            SetupMiscellaneousTab();

            ModernUi.Apply(this);
        }

        private void SetupGeneralTab()
        {
            GeneralTab.Controls.Clear();
            GeneralTab.BackColor = ModernUi.MainBackground;

            Panel scrollContainer = CreateScrollContainer();
            TableLayoutPanel cardsLayout = CreateCardsLayout();
            scrollContainer.Controls.Add(cardsLayout);
            GeneralTab.Controls.Add(scrollContainer);

            // Card 1: Launch & Startup
            TableLayoutPanel asyncRow = CreateRowTable(4);
            asyncRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            asyncRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            asyncRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            asyncRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            AsyncJoinCB.Margin = new Padding(0, 4, 12, 4);
            AsyncJoinCB.AutoSize = true;
            AsyncJoinCB.ForeColor = ModernUi.TextPrimary;
            AsyncJoinCB.BackColor = Color.Transparent;

            Panel asyncSpacer = new Panel { BackColor = Color.Transparent };

            DelayLabel.Text = "Launch Delay (s):";
            DelayLabel.ForeColor = ModernUi.TextMuted;
            DelayLabel.BackColor = Color.Transparent;
            DelayLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            DelayLabel.Margin = new Padding(0, 6, 8, 4);
            DelayLabel.AutoSize = true;

            LaunchDelayNumber.Size = new Size(80, 30);
            LaunchDelayNumber.Margin = new Padding(0, 2, 0, 4);
            LaunchDelayNumber.BackColor = ModernUi.InputBackground;
            LaunchDelayNumber.ForeColor = ModernUi.TextPrimary;

            asyncRow.Controls.Add(AsyncJoinCB, 0, 0);
            asyncRow.Controls.Add(asyncSpacer, 1, 0);
            asyncRow.Controls.Add(DelayLabel, 2, 0);
            asyncRow.Controls.Add(LaunchDelayNumber, 3, 0);

            AutoUpdateCB.Margin = new Padding(0, 4, 0, 8);
            AutoUpdateCB.AutoSize = true;
            AutoUpdateCB.ForeColor = ModernUi.TextPrimary;
            AutoUpdateCB.BackColor = Color.Transparent;

            StartOnPCStartup.Margin = new Padding(0, 4, 0, 10);
            StartOnPCStartup.AutoSize = true;
            StartOnPCStartup.ForeColor = ModernUi.TextPrimary;
            StartOnPCStartup.BackColor = Color.Transparent;

            cardsLayout.Controls.Add(CreateCard("Launch & Startup", new Control[]
            {
                AutoUpdateCB,
                StartOnPCStartup,
                asyncRow
            }));

            // Card 2: Account Security
            SavePasswordCB.Margin = new Padding(0, 4, 0, 8);
            SavePasswordCB.AutoSize = true;
            SavePasswordCB.ForeColor = ModernUi.TextPrimary;
            SavePasswordCB.BackColor = Color.Transparent;

            AutoCookieRefreshCB.Margin = new Padding(0, 4, 0, 14);
            AutoCookieRefreshCB.AutoSize = true;
            AutoCookieRefreshCB.ForeColor = ModernUi.TextPrimary;
            AutoCookieRefreshCB.BackColor = Color.Transparent;

            EncryptionSelectionButton.Margin = new Padding(0, 2, 0, 4);
            EncryptionSelectionButton.Size = new Size(230, 36);
            EncryptionSelectionButton.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            ModernUi.StyleNeutralButton(EncryptionSelectionButton);

            cardsLayout.Controls.Add(CreateCard("Account Security", new Control[]
            {
                SavePasswordCB,
                AutoCookieRefreshCB,
                EncryptionSelectionButton
            }));

            // Card 3: Roblox Client
            MultiRobloxCB.Margin = new Padding(0, 4, 0, 8);
            MultiRobloxCB.AutoSize = true;
            MultiRobloxCB.ForeColor = ModernUi.TextPrimary;
            MultiRobloxCB.BackColor = Color.Transparent;

            HideMRobloxCB.Margin = new Padding(0, 4, 0, 8);
            HideMRobloxCB.AutoSize = true;
            HideMRobloxCB.ForeColor = ModernUi.TextPrimary;
            HideMRobloxCB.BackColor = Color.Transparent;

            DisableAgingAlertCB.Margin = new Padding(0, 4, 0, 8);
            DisableAgingAlertCB.AutoSize = true;
            DisableAgingAlertCB.ForeColor = ModernUi.TextPrimary;
            DisableAgingAlertCB.BackColor = Color.Transparent;

            ShuffleLowestServerCB.Margin = new Padding(0, 4, 0, 6);
            ShuffleLowestServerCB.AutoSize = true;
            ShuffleLowestServerCB.ForeColor = ModernUi.TextPrimary;
            ShuffleLowestServerCB.BackColor = Color.Transparent;

            cardsLayout.Controls.Add(CreateCard("Roblox Client", new Control[]
            {
                MultiRobloxCB,
                HideMRobloxCB,
                DisableAgingAlertCB,
                ShuffleLowestServerCB
            }));

            // Card 4: Recent Games & Region
            TableLayoutPanel regionTable = CreateRowTable(2);
            regionTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            regionTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            RegionFormatLabel.Text = "Region Format:";
            RegionFormatLabel.ForeColor = ModernUi.TextMuted;
            RegionFormatLabel.BackColor = Color.Transparent;
            RegionFormatLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            RegionFormatLabel.Margin = new Padding(0, 6, 12, 10);
            RegionFormatLabel.AutoSize = true;

            RegionFormatTB.Dock = DockStyle.Fill;
            RegionFormatTB.Margin = new Padding(0, 3, 0, 10);
            RegionFormatTB.BackColor = ModernUi.InputBackground;
            RegionFormatTB.ForeColor = ModernUi.TextPrimary;

            MRGLabel.Text = "Max Recent Games:";
            MRGLabel.ForeColor = ModernUi.TextMuted;
            MRGLabel.BackColor = Color.Transparent;
            MRGLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            MRGLabel.Margin = new Padding(0, 6, 12, 4);
            MRGLabel.AutoSize = true;

            MaxRecentGamesNumber.Size = new Size(80, 30);
            MaxRecentGamesNumber.Margin = new Padding(0, 2, 0, 4);
            MaxRecentGamesNumber.BackColor = ModernUi.InputBackground;
            MaxRecentGamesNumber.ForeColor = ModernUi.TextPrimary;

            regionTable.Controls.Add(RegionFormatLabel, 0, 0);
            regionTable.Controls.Add(RegionFormatTB, 1, 0);
            regionTable.Controls.Add(MRGLabel, 0, 1);
            regionTable.Controls.Add(MaxRecentGamesNumber, 1, 1);

            cardsLayout.Controls.Add(CreateCard("Recent Games & Region", new Control[]
            {
                regionTable
            }));

            // Card 5: Restart Notice
            Panel noticeCard = CreateNoticeCard();
            RSLabel.Text = "ⓘ  Some settings require the application to restart before they take effect (e.g. Web Server Port, Disable Aging Alert).";
            RSLabel.ForeColor = ModernUi.TextMuted;
            RSLabel.BackColor = Color.Transparent;
            RSLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            RSLabel.Dock = DockStyle.Fill;
            noticeCard.Controls.Add(RSLabel);
            cardsLayout.Controls.Add(noticeCard);
        }

        private void SetupDeveloperTab()
        {
            DeveloperTab.Controls.Clear();
            DeveloperTab.BackColor = ModernUi.MainBackground;

            Panel scrollContainer = CreateScrollContainer();
            TableLayoutPanel cardsLayout = CreateCardsLayout();
            scrollContainer.Controls.Add(cardsLayout);
            DeveloperTab.Controls.Add(scrollContainer);

            // Card 1: Developer Core
            EnableDMCB.Margin = new Padding(0, 4, 0, 8);
            EnableDMCB.AutoSize = true;
            EnableDMCB.ForeColor = ModernUi.TextPrimary;
            EnableDMCB.BackColor = Color.Transparent;

            DisableImagesCB.Margin = new Padding(0, 4, 0, 8);
            DisableImagesCB.AutoSize = true;
            DisableImagesCB.ForeColor = ModernUi.TextPrimary;
            DisableImagesCB.BackColor = Color.Transparent;

            AllowExternalConnectionsCB.Margin = new Padding(0, 4, 0, 6);
            AllowExternalConnectionsCB.AutoSize = true;
            AllowExternalConnectionsCB.ForeColor = ModernUi.TextPrimary;
            AllowExternalConnectionsCB.BackColor = Color.Transparent;

            cardsLayout.Controls.Add(CreateCard("Developer Tools", new Control[]
            {
                EnableDMCB,
                DisableImagesCB,
                AllowExternalConnectionsCB
            }));

            // Card 2: Local Web Server
            EnableWSCB.Margin = new Padding(0, 4, 0, 10);
            EnableWSCB.AutoSize = true;
            EnableWSCB.ForeColor = ModernUi.TextPrimary;
            EnableWSCB.BackColor = Color.Transparent;

            TableLayoutPanel wsTable = CreateRowTable(2);
            wsTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            wsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            PortLabel.ForeColor = ModernUi.TextMuted;
            PortLabel.BackColor = Color.Transparent;
            PortLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            PortLabel.Margin = new Padding(0, 6, 12, 10);
            PortLabel.AutoSize = true;

            PortNumber.Size = new Size(90, 30);
            PortNumber.Margin = new Padding(0, 2, 0, 10);
            PortNumber.BackColor = ModernUi.InputBackground;
            PortNumber.ForeColor = ModernUi.TextPrimary;

            WSPWLabel.Text = "Password:";
            WSPWLabel.ForeColor = ModernUi.TextMuted;
            WSPWLabel.BackColor = Color.Transparent;
            WSPWLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            WSPWLabel.Margin = new Padding(0, 6, 12, 4);
            WSPWLabel.AutoSize = true;

            PasswordTextBox.Dock = DockStyle.Fill;
            PasswordTextBox.Margin = new Padding(0, 3, 0, 4);
            PasswordTextBox.BackColor = ModernUi.InputBackground;
            PasswordTextBox.ForeColor = ModernUi.TextPrimary;

            wsTable.Controls.Add(PortLabel, 0, 0);
            wsTable.Controls.Add(PortNumber, 1, 0);
            wsTable.Controls.Add(WSPWLabel, 0, 1);
            wsTable.Controls.Add(PasswordTextBox, 1, 1);

            ERRPCB.Margin = new Padding(0, 10, 0, 4);
            ERRPCB.AutoSize = true;
            ERRPCB.ForeColor = ModernUi.TextPrimary;
            ERRPCB.BackColor = Color.Transparent;

            cardsLayout.Controls.Add(CreateCard("Local Web Server", new Control[]
            {
                EnableWSCB,
                wsTable,
                ERRPCB
            }));

            // Card 3: API Permissions
            AllowGCCB.Margin = new Padding(0, 4, 0, 8);
            AllowGCCB.AutoSize = true;
            AllowGCCB.ForeColor = ModernUi.TextPrimary;
            AllowGCCB.BackColor = Color.Transparent;

            AllowGACB.Margin = new Padding(0, 4, 0, 8);
            AllowGACB.AutoSize = true;
            AllowGACB.ForeColor = ModernUi.TextPrimary;
            AllowGACB.BackColor = Color.Transparent;

            AllowLACB.Margin = new Padding(0, 4, 0, 8);
            AllowLACB.AutoSize = true;
            AllowLACB.ForeColor = ModernUi.TextPrimary;
            AllowLACB.BackColor = Color.Transparent;

            AllowAECB.Margin = new Padding(0, 4, 0, 6);
            AllowAECB.AutoSize = true;
            AllowAECB.ForeColor = ModernUi.TextPrimary;
            AllowAECB.BackColor = Color.Transparent;

            cardsLayout.Controls.Add(CreateCard("API Permissions", new Control[]
            {
                AllowGCCB,
                AllowGACB,
                AllowLACB,
                AllowAECB
            }));
        }

        private void SetupMiscellaneousTab()
        {
            MiscellaneousTab.Controls.Clear();
            MiscellaneousTab.BackColor = ModernUi.MainBackground;

            Panel scrollContainer = CreateScrollContainer();
            TableLayoutPanel cardsLayout = CreateCardsLayout();
            scrollContainer.Controls.Add(cardsLayout);
            MiscellaneousTab.Controls.Add(scrollContainer);

            // Card 1: Account Presence
            PresenceCB.Margin = new Padding(0, 4, 0, 10);
            PresenceCB.AutoSize = true;
            PresenceCB.ForeColor = ModernUi.TextPrimary;
            PresenceCB.BackColor = Color.Transparent;

            TableLayoutPanel rateTable = CreateRowTable(2);
            rateTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            rateTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            PresenceUpdateLabel.Text = "Refresh Rate (min):";
            PresenceUpdateLabel.ForeColor = ModernUi.TextMuted;
            PresenceUpdateLabel.BackColor = Color.Transparent;
            PresenceUpdateLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            PresenceUpdateLabel.Margin = new Padding(0, 6, 12, 4);
            PresenceUpdateLabel.AutoSize = true;

            PresenceUpdateRateNum.Size = new Size(80, 30);
            PresenceUpdateRateNum.Margin = new Padding(0, 2, 0, 4);
            PresenceUpdateRateNum.BackColor = ModernUi.InputBackground;
            PresenceUpdateRateNum.ForeColor = ModernUi.TextPrimary;

            rateTable.Controls.Add(PresenceUpdateLabel, 0, 0);
            rateTable.Controls.Add(PresenceUpdateRateNum, 1, 0);

            cardsLayout.Controls.Add(CreateCard("Account Presence", new Control[]
            {
                PresenceCB,
                rateTable
            }));

            // Card 2: Performance
            UnlockFPSCB.Margin = new Padding(0, 4, 0, 10);
            UnlockFPSCB.AutoSize = true;
            UnlockFPSCB.ForeColor = ModernUi.TextPrimary;
            UnlockFPSCB.BackColor = Color.Transparent;

            TableLayoutPanel fpsTable = CreateRowTable(2);
            fpsTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            fpsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            FPSCapLabel.Text = "Max FPS Cap:";
            FPSCapLabel.ForeColor = ModernUi.TextMuted;
            FPSCapLabel.BackColor = Color.Transparent;
            FPSCapLabel.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            FPSCapLabel.Margin = new Padding(0, 6, 12, 4);
            FPSCapLabel.AutoSize = true;

            MaxFPSValue.Size = new Size(90, 30);
            MaxFPSValue.Margin = new Padding(0, 2, 0, 4);
            MaxFPSValue.BackColor = ModernUi.InputBackground;
            MaxFPSValue.ForeColor = ModernUi.TextPrimary;

            fpsTable.Controls.Add(FPSCapLabel, 0, 0);
            fpsTable.Controls.Add(MaxFPSValue, 1, 0);

            OverrideWithCustomCB.Margin = new Padding(0, 10, 0, 4);
            OverrideWithCustomCB.AutoSize = true;
            OverrideWithCustomCB.ForeColor = ModernUi.TextPrimary;
            OverrideWithCustomCB.BackColor = Color.Transparent;

            cardsLayout.Controls.Add(CreateCard("Client Performance", new Control[]
            {
                UnlockFPSCB,
                fpsTable,
                OverrideWithCustomCB
            }));

            // Card 3: Updates
            ForceUpdateButton.Size = new Size(220, 36);
            ForceUpdateButton.Margin = new Padding(0, 4, 0, 4);
            ForceUpdateButton.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            ModernUi.StyleNeutralButton(ForceUpdateButton);

            cardsLayout.Controls.Add(CreateCard("Application Updates", new Control[]
            {
                ForceUpdateButton
            }));
        }

        private Panel CreateScrollContainer() => new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            BackColor = ModernUi.MainBackground,
            Padding = new Padding(16, 14, 20, 16)
        };

        private TableLayoutPanel CreateCardsLayout()
        {
            TableLayoutPanel panel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                Padding = new Padding(0, 0, 0, 16),
                BackColor = Color.Transparent
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            return panel;
        }

        private Panel CreateCard(string title, Control[] contentControls)
        {
            TableLayoutPanel card = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = ModernUi.CardBackground,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(20, 18, 20, 18),
                Margin = new Padding(0, 0, 0, 16),
                ColumnCount = 1
            };
            card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            Label titleLabel = ModernUi.CreateSectionTitle(title);
            titleLabel.Margin = new Padding(0, 0, 0, 12);
            titleLabel.BackColor = Color.Transparent;
            card.Controls.Add(titleLabel);

            foreach (Control c in contentControls)
            {
                card.Controls.Add(c);
            }

            return card;
        }

        private Panel CreateNoticeCard()
        {
            return new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = ModernUi.CardBackgroundSecondary,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(18, 14, 18, 14),
                Margin = new Padding(0, 0, 0, 16)
            };
        }

        private TableLayoutPanel CreateRowTable(int columns)
        {
            return new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = columns,
                RowCount = 1,
                Margin = new Padding(0, 4, 0, 4),
                BackColor = Color.Transparent
            };
        }
    }
}
