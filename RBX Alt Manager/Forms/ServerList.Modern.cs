using BrightIdeasSoftware;
using RBX_Alt_Manager.Classes;
using RBX_Alt_Manager.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RBX_Alt_Manager
{
    public partial class ServerList
    {
        public void InitializeModernLayout()
        {
            ClientSize = new Size(960, 640);
            MinimumSize = new Size(840, 540);
            MaximizeBox = true;
            Padding = new Padding(12);
            BackColor = ModernUi.MainBackground;
            ForeColor = ModernUi.TextPrimary;

            Tabs.Padding = new Point(16, 8);
            Tabs.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            Tabs.BackColor = ModernUi.MainBackground;
            Tabs.ForeColor = ModernUi.TextPrimary;

            foreach (TabPage page in Tabs.TabPages)
            {
                page.UseVisualStyleBackColor = false;
                page.BackColor = ModernUi.MainBackground;
                page.Padding = Padding.Empty;
                page.Margin = Padding.Empty;
            }

            SetupServersTab();
            SetupGamesTab();
            SetupFavoritesTab();
            SetupUniverseTab();
            SetupOutfitsTab();
            SetupWatcherTab();

            ModernUi.Apply(this);
        }

        private void SetupServersTab()
        {
            ServersTab.Controls.Clear();
            ServersTab.BackColor = ModernUi.MainBackground;

            TableLayoutPanel toolbar = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(12, 9, 12, 9),
                BackColor = ModernUi.CardBackground,
                ColumnCount = 5,
                RowCount = 1
            };
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            RefreshServers.Size = new Size(85, 32);
            RefreshServers.Margin = new Padding(0, 0, 10, 0);

            OtherPlaceId.Dock = DockStyle.Fill;
            OtherPlaceId.Margin = new Padding(0, 3, 12, 0);

            UsernameLabel.AutoSize = true;
            UsernameLabel.Text = "Username:";
            UsernameLabel.Margin = new Padding(0, 6, 8, 0);

            Username.Dock = DockStyle.Fill;
            Username.Margin = new Padding(0, 3, 12, 0);

            SearchPlayer.Size = new Size(85, 32);
            SearchPlayer.Margin = new Padding(0);
            ModernUi.StylePrimaryButton(SearchPlayer);

            toolbar.Controls.Add(RefreshServers, 0, 0);
            toolbar.Controls.Add(OtherPlaceId, 1, 0);
            toolbar.Controls.Add(UsernameLabel, 2, 0);
            toolbar.Controls.Add(Username, 3, 0);
            toolbar.Controls.Add(SearchPlayer, 4, 0);

            ServerListView.Dock = DockStyle.Fill;
            ServerListView.Margin = new Padding(0);
            ServerListView.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            ModernUi.StyleObjectListView(ServerListView);

            ServerListView.Resize += (s, e) => UpdateServerListColumns();
            UpdateServerListColumns();

            ServersTab.Controls.Add(ServerListView);
            ServersTab.Controls.Add(toolbar);
        }

        private void UpdateServerListColumns()
        {
            int totalW = ServerListView.ClientSize.Width > 0 ? ServerListView.ClientSize.Width : ServerListView.Width;
            if (totalW < 200) return;

            int playingW = Math.Max(70, (int)(totalW * 0.12));
            int pingW = Math.Max(60, (int)(totalW * 0.10));
            int regionW = Math.Max(130, (int)(totalW * 0.22));
            int jobW = Math.Max(200, totalW - playingW - pingW - regionW - 4);

            JobId.Width = jobW;
            Playing.Width = playingW;
            PingColumn.Width = pingW;
            RegionColumn.Width = regionW;
        }

        private void SetupGamesTab()
        {
            GamesPage.Controls.Clear();
            GamesPage.BackColor = ModernUi.MainBackground;

            TableLayoutPanel toolbar = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(12, 9, 12, 9),
                BackColor = ModernUi.CardBackground,
                ColumnCount = 5,
                RowCount = 1
            };
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            Term.Dock = DockStyle.Fill;
            Term.Margin = new Padding(0, 3, 12, 0);

            label1.AutoSize = true;
            label1.Text = "Page:";
            label1.Margin = new Padding(0, 6, 8, 0);

            PageNum.Dock = DockStyle.Fill;
            PageNum.Margin = new Padding(0, 3, 12, 0);

            Search.Size = new Size(85, 32);
            Search.Margin = new Padding(0, 0, 12, 0);
            ModernUi.StylePrimaryButton(Search);

            ListViewCB.AutoSize = true;
            ListViewCB.Text = "List View";
            ListViewCB.Margin = new Padding(0, 6, 0, 0);

            toolbar.Controls.Add(Term, 0, 0);
            toolbar.Controls.Add(label1, 1, 0);
            toolbar.Controls.Add(PageNum, 2, 0);
            toolbar.Controls.Add(Search, 3, 0);
            toolbar.Controls.Add(ListViewCB, 4, 0);

            Panel contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ModernUi.MainBackground
            };

            GameListPanel.Dock = DockStyle.Fill;
            GameListPanel.BackColor = ModernUi.MainBackground;
            GameListPanel.AutoScroll = true;

            GamesListView.Dock = DockStyle.Fill;
            GamesListView.Visible = false;
            ModernUi.StyleObjectListView(GamesListView);
            name.Width = 360;
            playerCount.Width = 90;
            likeRatio.Width = 90;

            contentPanel.Controls.Add(GameListPanel);
            contentPanel.Controls.Add(GamesListView);

            GamesPage.Controls.Add(contentPanel);
            GamesPage.Controls.Add(toolbar);
        }

        private void SetupFavoritesTab()
        {
            FavoritesPage.Controls.Clear();
            FavoritesPage.BackColor = ModernUi.MainBackground;

            TableLayoutPanel toolbar = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(12, 9, 12, 9),
                BackColor = ModernUi.CardBackground,
                ColumnCount = 3,
                RowCount = 1
            };
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            Favorite.Size = new Size(180, 32);
            Favorite.Margin = new Padding(0, 0, 14, 0);

            FavoriteListViewCB.AutoSize = true;
            FavoriteListViewCB.Text = "List View";
            FavoriteListViewCB.Margin = new Padding(0, 6, 0, 0);

            toolbar.Controls.Add(Favorite, 0, 0);
            toolbar.Controls.Add(FavoriteListViewCB, 1, 0);

            Panel contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ModernUi.MainBackground
            };

            FavoriteGamesPanel.Dock = DockStyle.Fill;
            FavoriteGamesPanel.BackColor = ModernUi.MainBackground;
            FavoriteGamesPanel.AutoScroll = true;

            FavoritesListView.Dock = DockStyle.Fill;
            FavoritesListView.Visible = false;
            ModernUi.StyleObjectListView(FavoritesListView);
            GameName.Width = 400;

            contentPanel.Controls.Add(FavoriteGamesPanel);
            contentPanel.Controls.Add(FavoritesListView);

            FavoritesPage.Controls.Add(contentPanel);
            FavoritesPage.Controls.Add(toolbar);
        }

        private void SetupUniverseTab()
        {
            UniversePage.Controls.Clear();
            UniversePage.BackColor = ModernUi.MainBackground;

            TableLayoutPanel toolbar = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(12, 9, 12, 9),
                BackColor = ModernUi.CardBackground,
                ColumnCount = 6,
                RowCount = 1
            };
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            uPlaceIDLabel.AutoSize = true;
            uPlaceIDLabel.Text = "Place ID:";
            uPlaceIDLabel.Margin = new Padding(0, 6, 8, 0);

            PlaceIDUniTB.Dock = DockStyle.Fill;
            PlaceIDUniTB.Margin = new Padding(0, 3, 10, 0);

            GetUniverseID.Size = new Size(130, 32);
            GetUniverseID.Margin = new Padding(0, 0, 14, 0);

            uUniverseIdLabel.AutoSize = true;
            uUniverseIdLabel.Text = "Universe ID:";
            uUniverseIdLabel.Margin = new Padding(0, 6, 8, 0);

            UniverseIDTB.Dock = DockStyle.Fill;
            UniverseIDTB.Margin = new Padding(0, 3, 12, 0);

            ViewUniverse.Size = new Size(110, 32);
            ViewUniverse.Margin = new Padding(0);
            ModernUi.StylePrimaryButton(ViewUniverse);

            toolbar.Controls.Add(uPlaceIDLabel, 0, 0);
            toolbar.Controls.Add(PlaceIDUniTB, 1, 0);
            toolbar.Controls.Add(GetUniverseID, 2, 0);
            toolbar.Controls.Add(uUniverseIdLabel, 3, 0);
            toolbar.Controls.Add(UniverseIDTB, 4, 0);
            toolbar.Controls.Add(ViewUniverse, 5, 0);

            UniverseGamesPanel.Dock = DockStyle.Fill;
            UniverseGamesPanel.BackColor = ModernUi.MainBackground;
            UniverseGamesPanel.AutoScroll = true;

            UniversePage.Controls.Add(UniverseGamesPanel);
            UniversePage.Controls.Add(toolbar);
        }

        private void SetupOutfitsTab()
        {
            OutfitsPage.Controls.Clear();
            OutfitsPage.BackColor = ModernUi.MainBackground;

            TableLayoutPanel toolbar = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                Padding = new Padding(12, 9, 12, 9),
                BackColor = ModernUi.CardBackground,
                ColumnCount = 4,
                RowCount = 1
            };
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            toolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            OutfitUsernameLabel.AutoSize = true;
            OutfitUsernameLabel.Text = "Username:";
            OutfitUsernameLabel.Margin = new Padding(0, 6, 8, 0);

            OutfitUsernameTB.Dock = DockStyle.Fill;
            OutfitUsernameTB.Margin = new Padding(0, 3, 12, 0);

            ViewOutfits.Size = new Size(110, 32);
            ViewOutfits.Margin = new Padding(0, 0, 10, 0);
            ModernUi.StylePrimaryButton(ViewOutfits);

            WearCustomButton.Size = new Size(120, 32);
            WearCustomButton.Margin = new Padding(0);

            toolbar.Controls.Add(OutfitUsernameLabel, 0, 0);
            toolbar.Controls.Add(OutfitUsernameTB, 1, 0);
            toolbar.Controls.Add(ViewOutfits, 2, 0);
            toolbar.Controls.Add(WearCustomButton, 3, 0);

            OutfitsPanel.Dock = DockStyle.Fill;
            OutfitsPanel.BackColor = ModernUi.MainBackground;
            OutfitsPanel.AutoScroll = true;

            OutfitsPage.Controls.Add(OutfitsPanel);
            OutfitsPage.Controls.Add(toolbar);
        }

        private void SetupWatcherTab()
        {
            RobloxScan.Controls.Clear();
            RobloxScan.BackColor = ModernUi.MainBackground;

            Panel scrollContainer = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ModernUi.MainBackground,
                Padding = new Padding(16, 16, 16, 16)
            };

            TableLayoutPanel cardsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 1,
                Padding = new Padding(0, 0, 0, 16),
                BackColor = Color.Transparent
            };
            cardsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            scrollContainer.Controls.Add(cardsLayout);
            RobloxScan.Controls.Add(scrollContainer);

            // Card 1: Core Watcher Settings
            RobloxScannerCB.Margin = new Padding(0, 6, 0, 10);
            RobloxScannerCB.AutoSize = true;

            TableLayoutPanel scanIntervalTable = CreateWatcherRowTable(2);
            scanIntervalTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            scanIntervalTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            ScanESLabel.Text = "Scan Every (s):";
            ScanESLabel.Margin = new Padding(0, 6, 12, 8);
            ScanESLabel.AutoSize = true;
            ScanIntervalN.Size = new Size(65, 26);
            ScanIntervalN.Margin = new Padding(0, 2, 0, 8);

            label2.Text = "Read Every (ms):";
            label2.Margin = new Padding(0, 6, 12, 4);
            label2.AutoSize = true;
            ReadIntervalN.Size = new Size(75, 26);
            ReadIntervalN.Margin = new Padding(0, 2, 0, 4);

            scanIntervalTable.Controls.Add(ScanESLabel, 0, 0);
            scanIntervalTable.Controls.Add(ScanIntervalN, 1, 0);
            scanIntervalTable.Controls.Add(label2, 0, 1);
            scanIntervalTable.Controls.Add(ReadIntervalN, 1, 1);

            cardsLayout.Controls.Add(CreateWatcherCard("ROBLOX WATCHER CORE", new Control[]
            {
                RobloxScannerCB,
                scanIntervalTable
            }));

            // Card 2: Rules & Automation
            ExitIfBetaDetectedCB.Margin = new Padding(0, 6, 0, 8);
            ExitIfBetaDetectedCB.AutoSize = true;

            TableLayoutPanel connRow = CreateWatcherRowTable(4);
            connRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            connRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            connRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            connRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            ExitIfNoConnectionCB.Margin = new Padding(0, 4, 12, 0);
            ExitIfNoConnectionCB.AutoSize = true;
            Panel connSpacer = new Panel { BackColor = Color.Transparent };
            ConnectionSecondsLabel.Text = "Timeout (s):";
            ConnectionSecondsLabel.Margin = new Padding(0, 6, 8, 0);
            ConnectionSecondsLabel.AutoSize = true;
            TimeoutNum.Size = new Size(65, 26);
            TimeoutNum.Margin = new Padding(0, 2, 0, 0);

            connRow.Controls.Add(ExitIfNoConnectionCB, 0, 0);
            connRow.Controls.Add(connSpacer, 1, 0);
            connRow.Controls.Add(ConnectionSecondsLabel, 2, 0);
            connRow.Controls.Add(TimeoutNum, 3, 0);

            VerifyDataModelCB.Margin = new Padding(0, 8, 0, 6);
            VerifyDataModelCB.AutoSize = true;
            IgnoreExistingProcesses.Margin = new Padding(0, 6, 0, 6);
            IgnoreExistingProcesses.AutoSize = true;
            SaveWindowPositionsCB.Margin = new Padding(0, 6, 0, 6);
            SaveWindowPositionsCB.AutoSize = true;

            cardsLayout.Controls.Add(CreateWatcherCard("AUTOMATION & CONNECTION RULES", new Control[]
            {
                ExitIfBetaDetectedCB,
                connRow,
                VerifyDataModelCB,
                IgnoreExistingProcesses,
                SaveWindowPositionsCB
            }));

            // Card 3: Memory & Window Monitoring
            TableLayoutPanel memRow = CreateWatcherRowTable(3);
            memRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            memRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            memRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            RbxMemoryCB.Margin = new Padding(0, 4, 10, 8);
            RbxMemoryCB.AutoSize = true;
            RbxMemoryLTNum.Size = new Size(75, 26);
            RbxMemoryLTNum.Margin = new Padding(0, 2, 8, 8);
            MBLabel.Text = "MB";
            MBLabel.Margin = new Padding(0, 6, 0, 8);
            MBLabel.AutoSize = true;

            memRow.Controls.Add(RbxMemoryCB, 0, 0);
            memRow.Controls.Add(RbxMemoryLTNum, 1, 0);
            memRow.Controls.Add(MBLabel, 2, 0);

            TableLayoutPanel titleRow = CreateWatcherRowTable(2);
            titleRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            titleRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            CloseRbxWindowTitleCB.Margin = new Padding(0, 4, 10, 4);
            CloseRbxWindowTitleCB.AutoSize = true;
            RbxWindowNameTB.Dock = DockStyle.Fill;
            RbxWindowNameTB.Margin = new Padding(0, 2, 0, 4);

            titleRow.Controls.Add(CloseRbxWindowTitleCB, 0, 0);
            titleRow.Controls.Add(RbxWindowNameTB, 1, 0);

            cardsLayout.Controls.Add(CreateWatcherCard("WINDOW & MEMORY MONITORING", new Control[]
            {
                memRow,
                titleRow
            }));
        }

        private Panel CreateWatcherCard(string title, Control[] contentControls)
        {
            TableLayoutPanel card = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = ModernUi.CardBackground,
                BorderStyle = BorderStyle.None,
                Padding = new Padding(20, 16, 20, 18),
                Margin = new Padding(0, 0, 0, 16),
                ColumnCount = 1
            };
            card.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            Label titleLabel = ModernUi.CreateSectionTitle(title);
            card.Controls.Add(titleLabel);

            foreach (Control c in contentControls)
            {
                card.Controls.Add(c);
            }

            return card;
        }

        private TableLayoutPanel CreateWatcherRowTable(int columns)
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
