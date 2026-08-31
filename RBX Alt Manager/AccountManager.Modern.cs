using RBX_Alt_Manager.Classes;
using RBX_Alt_Manager.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RBX_Alt_Manager
{
    public partial class AccountManager
    {
        private TableLayoutPanel rootLayout;
        private TableLayoutPanel contentLayout;
        private TableLayoutPanel leftLayout;
        private TableLayoutPanel rightLayout;
        private TableLayoutPanel footerLayout;
        private FlowLayoutPanel leftFooter;
        private FlowLayoutPanel rightFooter;

        private Label modernAccountsTitle;
        private Label modernLaunchTitle;
        private Label modernAccountTitle;
        private Label aliasLabel;
        private Label descLabel;

        private Panel modernLaunchCard;
        private Panel modernAccountCard;
        private TableLayoutPanel launchTable;
        private TableLayoutPanel accountTable;
        private TableLayoutPanel accountActions;

        private void InitializeModernLayout()
        {
            MinimumSize = new Size(1040, 650);
            ClientSize = new Size(1120, 680);
            MaximizeBox = true;
            BackColor = ModernUi.MainBackground;
            ForeColor = ModernUi.TextPrimary;
            Padding = new Padding(18, 14, 18, 14);

            DonateButton.Image = null;
            DonateButton.Text = "PayPal";
            DonateButton.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            DonateButton.Size = new Size(76, 28);
            ModernUi.StyleNeutralButton(DonateButton);
            SaveTooltip.SetToolTip(DonateButton, "PayPal donations — coming soon");

            BuildModernLayoutHierarchy();

            ModernUi.Apply(this);
            RefreshModernCardStyling();
        }

        public void RefreshModernCardStyling()
        {
            if (modernAccountsTitle != null) modernAccountsTitle.BackColor = Color.Transparent;
            if (modernLaunchTitle != null) modernLaunchTitle.BackColor = Color.Transparent;
            if (modernAccountTitle != null) modernAccountTitle.BackColor = Color.Transparent;
            if (CurrentPlace != null) CurrentPlace.BackColor = Color.Transparent;
            if (LabelPlaceID != null) LabelPlaceID.BackColor = Color.Transparent;
            if (LabelJobID != null) LabelJobID.BackColor = Color.Transparent;
            if (LabelUserID != null) LabelUserID.BackColor = Color.Transparent;
            if (aliasLabel != null) aliasLabel.BackColor = Color.Transparent;
            if (descLabel != null) descLabel.BackColor = Color.Transparent;
            if (DLChromiumLabel != null) DLChromiumLabel.BackColor = Color.Transparent;

            if (modernLaunchCard != null)
            {
                modernLaunchCard.BackColor = ModernUi.CardBackground;
                if (launchTable != null) launchTable.BackColor = ModernUi.CardBackground;

                ModernUi.StyleCardIcon(HistoryIcon, 30);
                ModernUi.StyleCardIcon(ShuffleIcon, 30);

                ModernUi.StyleSmallIconButton(ArgumentsB, 30);
                ModernUi.StyleSmallIconButton(ConfigButton, 30);
                ModernUi.StyleSmallIconButton(SaveToAccount, 30);

                ModernUi.StylePrimaryButton(JoinServer);
                ModernUi.StyleNeutralButton(ServerList);
            }

            if (modernAccountCard != null)
            {
                modernAccountCard.BackColor = ModernUi.CardBackground;
                if (accountTable != null) accountTable.BackColor = ModernUi.CardBackground;
                if (accountActions != null) accountActions.BackColor = ModernUi.CardBackground;

                Follow.Size = new Size(96, 30);
                Follow.Anchor = AnchorStyles.None;
                Follow.Margin = Padding.Empty;
                ModernUi.StyleNeutralButton(Follow);

                SetAlias.Size = new Size(96, 30);
                SetAlias.Anchor = AnchorStyles.None;
                SetAlias.Margin = Padding.Empty;
                ModernUi.StyleNeutralButton(SetAlias);

                ModernUi.StylePrimaryButton(SetDescription);
                ModernUi.StyleNeutralButton(BrowserButton);
            }

            if (DonateButton != null)
            {
                ModernUi.StyleNeutralButton(DonateButton);
            }

            if (leftFooter != null)
            {
                ModernUi.StyleNeutralButton(Add);
                ModernUi.StyleNeutralButton(Remove);
                ModernUi.StyleNeutralButton(OpenBrowser);
                ModernUi.StyleSmallIconButton(JoinDiscord, 36);
                if (HideUsernamesCheckbox != null)
                {
                    HideUsernamesCheckbox.Anchor = AnchorStyles.None;
                    HideUsernamesCheckbox.Margin = new Padding(0, 0, 16, 0);
                }
            }

            if (rightFooter != null)
            {
                ModernUi.StyleNeutralButton(EditTheme);
                ModernUi.StyleNeutralButton(LaunchNexus);
            }
        }

        private void BuildModernLayoutHierarchy()
        {
            Controls.Clear();

            // Root layout: Content (Fill) on top, Footer (58px) on bottom
            rootLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));

            // Content layout: Left (Accounts list) + Right (Launch & Selected Account)
            contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Padding = Padding.Empty,
                Margin = new Padding(0, 0, 0, 8)
            };
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 420F));
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            SetupLeftLayout();
            SetupRightLayout();
            SetupFooterLayout();

            contentLayout.Controls.Add(leftLayout, 0, 0);
            contentLayout.Controls.Add(rightLayout, 1, 0);

            rootLayout.Controls.Add(contentLayout, 0, 0);
            rootLayout.Controls.Add(footerLayout, 0, 1);

            Controls.Add(rootLayout);
            if (PasswordPanel != null)
            {
                Controls.Add(PasswordPanel);
                PasswordPanel.BringToFront();
            }
        }

        private void SetupLeftLayout()
        {
            leftLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 16, 0),
                Padding = Padding.Empty
            };
            leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            leftLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            modernAccountsTitle = ModernUi.CreateSectionTitle("ACCOUNTS");
            modernAccountsTitle.Margin = new Padding(0, 2, 0, 6);
            modernAccountsTitle.BackColor = Color.Transparent;

            AccountsView.Dock = DockStyle.Fill;
            AccountsView.Margin = Padding.Empty;
            ModernUi.StyleObjectListView(AccountsView);

            FlowLayoutPanel dlPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0, 6, 0, 0),
                BackColor = Color.Transparent
            };
            DLChromiumLabel.AutoSize = true;
            DLChromiumLabel.ForeColor = ModernUi.TextMuted;
            DLChromiumLabel.BackColor = Color.Transparent;
            DLChromiumLabel.Margin = new Padding(0, 2, 8, 0);
            DownloadProgressBar.Size = new Size(200, 12);
            DownloadProgressBar.Margin = new Padding(0, 4, 0, 0);
            dlPanel.Controls.Add(DLChromiumLabel);
            dlPanel.Controls.Add(DownloadProgressBar);

            leftLayout.Controls.Add(modernAccountsTitle, 0, 0);
            leftLayout.Controls.Add(AccountsView, 0, 1);
            leftLayout.Controls.Add(dlPanel, 0, 2);
        }

        private void SetupRightLayout()
        {
            rightLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = Color.Transparent,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));  // Launch Title Row
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 195F)); // Launch Card Row
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));  // Selected Account Title Row
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));  // Selected Account Card Row

            TableLayoutPanel launchHeader = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 2),
                Padding = Padding.Empty
            };
            launchHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            launchHeader.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            modernLaunchTitle = ModernUi.CreateSectionTitle("LAUNCH");
            modernLaunchTitle.Margin = new Padding(0, 2, 0, 0);
            modernLaunchTitle.BackColor = Color.Transparent;
            DonateButton.Size = new Size(76, 28);
            DonateButton.Margin = Padding.Empty;

            launchHeader.Controls.Add(modernLaunchTitle, 0, 0);
            launchHeader.Controls.Add(DonateButton, 1, 0);

            modernLaunchCard = ModernUi.CreateCardPanel(new Padding(18, 16, 18, 16));
            modernLaunchCard.Dock = DockStyle.Fill;
            modernLaunchCard.Margin = new Padding(0, 0, 0, 8);
            SetupLaunchCardContent();

            modernAccountTitle = ModernUi.CreateSectionTitle("SELECTED ACCOUNT");
            modernAccountTitle.Margin = new Padding(0, 4, 0, 0);
            modernAccountTitle.BackColor = Color.Transparent;

            modernAccountCard = ModernUi.CreateCardPanel(new Padding(18, 16, 18, 16));
            modernAccountCard.Dock = DockStyle.Fill;
            modernAccountCard.Margin = Padding.Empty;
            SetupAccountCardContent();

            rightLayout.Controls.Add(launchHeader, 0, 0);
            rightLayout.Controls.Add(modernLaunchCard, 0, 1);
            rightLayout.Controls.Add(modernAccountTitle, 0, 2);
            rightLayout.Controls.Add(modernAccountCard, 0, 3);
        }

        private void SetupLaunchCardContent()
        {
            modernLaunchCard.Controls.Clear();

            launchTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 4,
                ColumnCount = 1,
                BackColor = ModernUi.CardBackground,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            launchTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            launchTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F)); // Row 0: Header (Current Place & [A] [Gear])
            launchTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F)); // Row 1: Labels (Place ID & Job ID)
            launchTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F)); // Row 2: Inputs Row (PlaceID, Shuffle, Clock | JobID, Save)
            launchTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F)); // Row 3: Action buttons (Join Server & Utilities)

            // Row 0: Header Row
            TableLayoutPanel headerRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 2,
                BackColor = ModernUi.CardBackground,
                Padding = Padding.Empty,
                Margin = new Padding(0, 0, 0, 6)
            };
            headerRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            headerRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            CurrentPlace.Text = "Current Place";
            CurrentPlace.Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
            CurrentPlace.ForeColor = ModernUi.TextPrimary;
            CurrentPlace.BackColor = Color.Transparent;
            CurrentPlace.Dock = DockStyle.Fill;
            CurrentPlace.TextAlign = ContentAlignment.MiddleLeft;
            CurrentPlace.Margin = Padding.Empty;

            TableLayoutPanel headerButtons = new TableLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                Height = 30,
                RowCount = 1,
                ColumnCount = 2,
                BackColor = ModernUi.CardBackground,
                Margin = Padding.Empty,
                Padding = Padding.Empty
            };
            headerButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 34F));
            headerButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 34F));
            headerButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            ArgumentsB.Tag = "SmallIcon";
            ArgumentsB.Size = new Size(30, 30);
            ArgumentsB.Dock = DockStyle.Fill;
            ArgumentsB.Margin = new Padding(0, 0, 4, 0);
            ModernUi.StyleSmallIconButton(ArgumentsB, 30);

            ConfigButton.Tag = "SmallIcon";
            ConfigButton.Size = new Size(30, 30);
            ConfigButton.Dock = DockStyle.Fill;
            ConfigButton.Margin = new Padding(0);
            ModernUi.StyleSmallIconButton(ConfigButton, 30);

            headerButtons.Controls.Add(ArgumentsB, 0, 0);
            headerButtons.Controls.Add(ConfigButton, 1, 0);

            headerRow.Controls.Add(CurrentPlace, 0, 0);
            headerRow.Controls.Add(headerButtons, 1, 0);
            launchTable.Controls.Add(headerRow, 0, 0);

            // Row 1: Labels Row
            TableLayoutPanel labelsRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 3,
                BackColor = ModernUi.CardBackground,
                Padding = Padding.Empty,
                Margin = new Padding(0, 0, 0, 4)
            };
            labelsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            labelsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F));
            labelsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            labelsRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            LabelPlaceID.Text = "Place ID";
            LabelPlaceID.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            LabelPlaceID.ForeColor = ModernUi.TextMuted;
            LabelPlaceID.BackColor = Color.Transparent;
            LabelPlaceID.Dock = DockStyle.Fill;
            LabelPlaceID.TextAlign = ContentAlignment.BottomLeft;
            LabelPlaceID.Margin = Padding.Empty;

            LabelJobID.Text = "Job ID";
            LabelJobID.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            LabelJobID.ForeColor = ModernUi.TextMuted;
            LabelJobID.BackColor = Color.Transparent;
            LabelJobID.Dock = DockStyle.Fill;
            LabelJobID.TextAlign = ContentAlignment.BottomLeft;
            LabelJobID.Margin = Padding.Empty;

            labelsRow.Controls.Add(LabelPlaceID, 0, 0);
            labelsRow.Controls.Add(new Panel { BackColor = Color.Transparent }, 1, 0);
            labelsRow.Controls.Add(LabelJobID, 2, 0);
            launchTable.Controls.Add(labelsRow, 0, 1);

            // Row 2: Inputs Row (One single clean horizontal row!)
            TableLayoutPanel inputsRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 3,
                BackColor = ModernUi.CardBackground,
                Padding = Padding.Empty,
                Margin = new Padding(0, 0, 0, 10)
            };
            inputsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            inputsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F));
            inputsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            inputsRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Left input side: PlaceID textbox + ShuffleIcon + HistoryIcon
            TableLayoutPanel placeInputPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 3,
                BackColor = ModernUi.CardBackground,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            placeInputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            placeInputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 34F));
            placeInputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 34F));
            placeInputPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            PlaceID.Dock = DockStyle.Fill;
            PlaceID.Margin = new Padding(0, 2, 4, 2);

            ShuffleIcon.Tag = "CardIcon";
            ShuffleIcon.Size = new Size(30, 30);
            ShuffleIcon.SizeMode = PictureBoxSizeMode.Zoom;
            ShuffleIcon.Padding = new Padding(5);
            ShuffleIcon.Anchor = AnchorStyles.None;
            ShuffleIcon.Cursor = Cursors.Hand;
            ShuffleIcon.BackColor = ModernUi.InputBackground;
            ShuffleIcon.Margin = new Padding(0, 2, 4, 2);
            ModernUi.StyleCardIcon(ShuffleIcon, 30);

            HistoryIcon.Tag = "CardIcon";
            HistoryIcon.Size = new Size(30, 30);
            HistoryIcon.SizeMode = PictureBoxSizeMode.Zoom;
            HistoryIcon.Padding = new Padding(5);
            HistoryIcon.Anchor = AnchorStyles.None;
            HistoryIcon.Cursor = Cursors.Hand;
            HistoryIcon.BackColor = ModernUi.InputBackground;
            HistoryIcon.Margin = new Padding(0, 2, 0, 2);
            ModernUi.StyleCardIcon(HistoryIcon, 30);

            placeInputPanel.Controls.Add(PlaceID, 0, 0);
            placeInputPanel.Controls.Add(ShuffleIcon, 1, 0);
            placeInputPanel.Controls.Add(HistoryIcon, 2, 0);

            // Right input side: JobID textbox + SaveToAccount
            TableLayoutPanel jobInputPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 2,
                BackColor = ModernUi.CardBackground,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            jobInputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            jobInputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 34F));
            jobInputPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            JobID.Dock = DockStyle.Fill;
            JobID.Margin = new Padding(0, 2, 4, 2);

            SaveToAccount.Tag = "SmallIcon";
            SaveToAccount.Size = new Size(30, 30);
            SaveToAccount.Anchor = AnchorStyles.None;
            SaveToAccount.Margin = new Padding(0, 2, 0, 2);
            ModernUi.StyleSmallIconButton(SaveToAccount, 30);

            jobInputPanel.Controls.Add(JobID, 0, 0);
            jobInputPanel.Controls.Add(SaveToAccount, 1, 0);

            inputsRow.Controls.Add(placeInputPanel, 0, 0);
            inputsRow.Controls.Add(new Panel { BackColor = Color.Transparent }, 1, 0);
            inputsRow.Controls.Add(jobInputPanel, 2, 0);
            launchTable.Controls.Add(inputsRow, 0, 2);

            // Row 3: Action Buttons Row (Join Server & Utilities)
            TableLayoutPanel actionsRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 2,
                BackColor = ModernUi.CardBackground,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            actionsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            actionsRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            actionsRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            JoinServer.Dock = DockStyle.Fill;
            JoinServer.Height = 42;
            JoinServer.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            JoinServer.Margin = new Padding(0, 2, 4, 0);
            ModernUi.StylePrimaryButton(JoinServer);

            ServerList.Dock = DockStyle.Fill;
            ServerList.Height = 42;
            ServerList.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            ServerList.Margin = new Padding(4, 2, 0, 0);
            ModernUi.StyleNeutralButton(ServerList);

            actionsRow.Controls.Add(JoinServer, 0, 0);
            actionsRow.Controls.Add(ServerList, 1, 0);
            launchTable.Controls.Add(actionsRow, 0, 3);

            modernLaunchCard.Controls.Add(launchTable);
        }

        private void SetupAccountCardContent()
        {
            modernAccountCard.Controls.Clear();

            accountTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 7,
                ColumnCount = 1,
                BackColor = ModernUi.CardBackground,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            accountTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            accountTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F)); // Row 0: Username Label
            accountTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F)); // Row 1: Username + Follow
            accountTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F)); // Row 2: Alias Label
            accountTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F)); // Row 3: Alias + Set Alias
            accountTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 18F)); // Row 4: Description Label
            accountTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Row 5: Description Box (absorbs vertical space!)
            accountTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F)); // Row 6: Set Description + Account Utilities

            // Row 0: Username Label
            LabelUserID.Text = "Username";
            LabelUserID.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            LabelUserID.ForeColor = ModernUi.TextMuted;
            LabelUserID.BackColor = Color.Transparent;
            LabelUserID.Dock = DockStyle.Fill;
            LabelUserID.TextAlign = ContentAlignment.BottomLeft;
            LabelUserID.Margin = new Padding(0, 0, 0, 4);
            LabelUserID.Visible = true;
            accountTable.Controls.Add(LabelUserID, 0, 0);

            // Row 1: Username Row (UserID + Follow)
            TableLayoutPanel userRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 2,
                BackColor = ModernUi.CardBackground,
                Padding = Padding.Empty,
                Margin = new Padding(0, 0, 0, 10)
            };
            userRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            userRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
            userRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            UserID.Dock = DockStyle.None;
            UserID.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            UserID.Margin = new Padding(0, 0, 8, 0);

            Follow.Dock = DockStyle.None;
            Follow.Anchor = AnchorStyles.None;
            Follow.Size = new Size(96, 30);
            Follow.Margin = Padding.Empty;
            Follow.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            ModernUi.StyleNeutralButton(Follow);

            userRow.Controls.Add(UserID, 0, 0);
            userRow.Controls.Add(Follow, 1, 0);
            accountTable.Controls.Add(userRow, 0, 1);

            // Row 2: Alias Label
            aliasLabel = new Label
            {
                Text = "Alias",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = ModernUi.TextMuted,
                BackColor = Color.Transparent,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft,
                Margin = new Padding(0, 0, 0, 4)
            };
            accountTable.Controls.Add(aliasLabel, 0, 2);

            // Row 3: Alias Row (Alias + Set Alias)
            TableLayoutPanel aliasRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 2,
                BackColor = ModernUi.CardBackground,
                Padding = Padding.Empty,
                Margin = new Padding(0, 0, 0, 10)
            };
            aliasRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            aliasRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96F));
            aliasRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            Alias.Dock = DockStyle.None;
            Alias.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            Alias.Margin = new Padding(0, 0, 8, 0);

            SetAlias.Dock = DockStyle.None;
            SetAlias.Anchor = AnchorStyles.None;
            SetAlias.Size = new Size(96, 30);
            SetAlias.Margin = Padding.Empty;
            SetAlias.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            ModernUi.StyleNeutralButton(SetAlias);

            aliasRow.Controls.Add(Alias, 0, 0);
            aliasRow.Controls.Add(SetAlias, 1, 0);
            accountTable.Controls.Add(aliasRow, 0, 3);

            // Row 4: Description Label
            descLabel = new Label
            {
                Text = "Description",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = ModernUi.TextMuted,
                BackColor = Color.Transparent,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.BottomLeft,
                Margin = new Padding(0, 0, 0, 4)
            };
            accountTable.Controls.Add(descLabel, 0, 4);

            // Row 5: Description Box (absorbs 100% remaining vertical height!)
            DescriptionBox.Dock = DockStyle.Fill;
            DescriptionBox.Margin = new Padding(0, 0, 0, 10);
            accountTable.Controls.Add(DescriptionBox, 0, 5);

            // Row 6: Bottom Action Buttons Row (Set Description + Account Utilities)
            accountActions = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 2,
                BackColor = ModernUi.CardBackground,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            accountActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            accountActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            accountActions.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            SetDescription.Dock = DockStyle.Fill;
            SetDescription.Height = 38;
            SetDescription.Margin = new Padding(0, 2, 4, 0);
            SetDescription.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            ModernUi.StylePrimaryButton(SetDescription);

            BrowserButton.Text = "Account Utilities";
            BrowserButton.Dock = DockStyle.Fill;
            BrowserButton.Height = 38;
            BrowserButton.Margin = new Padding(4, 2, 0, 0);
            BrowserButton.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            ModernUi.StyleNeutralButton(BrowserButton);

            accountActions.Controls.Add(SetDescription, 0, 0);
            accountActions.Controls.Add(BrowserButton, 1, 0);
            accountTable.Controls.Add(accountActions, 0, 6);

            modernAccountCard.Controls.Add(accountTable);
        }

        private void SetupFooterLayout()
        {
            footerLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62F));
            footerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
            footerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            leftFooter = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };

            Add.Size = new Size(130, 36);
            Add.Margin = new Padding(0, 8, 8, 8);
            ModernUi.StyleNeutralButton(Add);

            Remove.Size = new Size(95, 36);
            Remove.Margin = new Padding(0, 8, 14, 8);
            ModernUi.StyleNeutralButton(Remove);

            HideUsernamesCheckbox.AutoSize = true;
            HideUsernamesCheckbox.ForeColor = ModernUi.TextPrimary;
            HideUsernamesCheckbox.BackColor = Color.Transparent;
            HideUsernamesCheckbox.Anchor = AnchorStyles.None;
            HideUsernamesCheckbox.Margin = new Padding(0, 0, 16, 0);

            OpenBrowser.Size = new Size(145, 36);
            OpenBrowser.Margin = new Padding(0, 8, 12, 8);
            ModernUi.StyleNeutralButton(OpenBrowser);

            JoinDiscord.Tag = "SmallIcon";
            JoinDiscord.Size = new Size(36, 36);
            JoinDiscord.Margin = new Padding(0, 8, 0, 8);
            ModernUi.StyleSmallIconButton(JoinDiscord, 36);

            leftFooter.Controls.Add(Add);
            leftFooter.Controls.Add(Remove);
            leftFooter.Controls.Add(HideUsernamesCheckbox);
            leftFooter.Controls.Add(OpenBrowser);
            leftFooter.Controls.Add(JoinDiscord);

            rightFooter = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = false,
                BackColor = Color.Transparent,
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };

            LaunchNexus.Size = new Size(140, 36);
            LaunchNexus.Margin = new Padding(0, 8, 0, 8);
            ModernUi.StyleNeutralButton(LaunchNexus);

            EditTheme.Size = new Size(115, 36);
            EditTheme.Margin = new Padding(0, 8, 12, 8);
            ModernUi.StyleNeutralButton(EditTheme);

            rightFooter.Controls.Add(LaunchNexus);
            rightFooter.Controls.Add(EditTheme);

            footerLayout.Controls.Add(leftFooter, 0, 0);
            footerLayout.Controls.Add(rightFooter, 1, 0);
        }
    }
}
