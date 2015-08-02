namespace EnhancedItemStats {
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Windows;

    using Factories;
    using Helper;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        ItemPreviewWindow previewWindow;

        public MainWindow() {
            InitializeComponent();

            // subscribe to clipboard changes
            ClipboardNotification.ClipboardUpdate += OnClipboardUpdate;

            this.Hide();
        }

        private void OnClipboardUpdate(object sender, EventArgs eventArgs) {
            var text = Clipboard.GetText();

            // check if its a valid item before doing anything with it
            if (!text.Contains("Rarity:")) {
                return;
            }

            // only show one item window at a time
            if (previewWindow != null && previewWindow.IsLoaded) {
                previewWindow.Close();
            }

            var item = ItemFactory.ParseItem(text);

            var helper = new PoePriceHelper();
            item.ChaosValue = helper.GetItemValue(text);

            this.previewWindow = new ItemPreviewWindow(item, GetItemPrice(text));
        }

        private static string GetItemPrice(string item) {
            try {
                // very hackish
                using (var client = new HttpClient()) {
                    var values = new Dictionary<string, string> {
                            { "v", "5" },
                            { "itemData", item },
                            { "league", "warbands" },
                            { "showDays", "10" }
                        };

                    var content = new FormUrlEncodedContent(values);
                    var response = client.PostAsync("http://api.exiletools.com/item-report-text", content).Result;
                    return response.Content.ReadAsStringAsync().Result;
                }
            } catch (Exception) {
                return null;
            }
        }
    }
}
