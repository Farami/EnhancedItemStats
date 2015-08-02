namespace EnhancedItemStats {
    using System;
    using System.Windows;
    using System.Windows.Threading;

    using Models;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ItemPreviewWindow : Window {
        public ItemPreviewWindow(Item item, string itemPrice) {
            InitializeComponent();

            ItemStats.Items.Clear();

            ItemStats.Items.Add($"Name: {item.Name}");
            ItemStats.Items.Add($"Rarity: {item.Rarity}");
            ItemStats.Items.Add($"Main Type: {item.Type.MainType}");
            ItemStats.Items.Add($"Sub Type: {item.Type.SubType}");

            if (item.PhysicalDps > 0) {
                ItemStats.Items.Add($"Physical Dps: {item.PhysicalDps}");
            }

            if (item.ElementalDps > 0) {
                ItemStats.Items.Add($"Elemental Dps: {item.ElementalDps}");
            }

            if (item.ChaosDps > 0) {
                ItemStats.Items.Add($"Chaos Dps: {item.ChaosDps}");
            }

            if (item.Dps > 0) {
                ItemStats.Items.Add($"Total Dps: {item.Dps}");
            }

            if (!item.Identified) {
                ItemStats.Items.Add("Unidentified");
            }

            var splitValue = itemPrice?.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (splitValue?.Length > 6) {
                this.ItemStats.Items.Add(string.Empty);
                this.ItemStats.Items.Add($"Item Value: {splitValue[2]} - {splitValue[3]}");
                this.ItemStats.Items.Add($"Item Value: {splitValue[4]}");
                this.ItemStats.Items.Add($"Item Value: {splitValue[5]} - {splitValue[6]}");
            }

            var location = System.Windows.Forms.Cursor.Position;
            this.Left = location.X;
            this.Top = location.Y - this.ActualHeight + 30;

            this.Show();
            this.StartCloseTimer();
        }

        private void StartCloseTimer() {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3d) };
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e) {
            var timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= TimerTick;
            Close();
        }
    }
}
