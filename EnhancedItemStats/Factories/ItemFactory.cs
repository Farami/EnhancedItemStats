namespace EnhancedItemStats.Factories {
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Helper;
    using Models;

    public static class ItemFactory {
        private static string[] SplitString { get; } = { "--------" };

        public static Item ParseItem(string itemText) {
            var item = new Item();

            // items are split by a series of dashes (--------)
            var itemParts = itemText.Split(SplitString, StringSplitOptions.RemoveEmptyEntries);

            // some preliminary scans to try to detect the item type
            item.Type = ItemTypeHelper.GetType(itemParts[0]);

            item.Identified = !itemText.Contains("Unidentified");

            item.ParseMainProperties(itemParts[0]); // every item has these
            item.ParseSecondaryProperties(itemParts[1]);

            return item;
        }

        private static void ParseMainProperties(this Item item, string itemPart) {
            var parts = itemPart.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            item.Rarity = (Rarity)Enum.Parse(typeof(Rarity), parts[0].Split(':')[1]);

            item.Name = parts[1];
        }

        private static void ParseSecondaryProperties(this Item item, string itemPart) {
            var parts = itemPart.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var partParts in parts.Select(part => part.Split(':'))) {
                switch (partParts[0]) {
                    case "Quality":
                        var qualityPart = int.Parse(Regex.Matches(partParts[1], "[0-2]?[0-9]")[0].Value);
                        item.Quality = new Quality(qualityPart, partParts[1].Contains("(augmented)"));
                        break;
                    case "Physical Damage":
                    case "Elemental Damage":
                    case "Chaos Damage":
                        var damage = new Damage();

                        // can be 1-2 (augmented) or without (augmented), can be multiple separated by comma
                        var multipleParts = partParts[1].Split(',');

                        foreach (var subParts in multipleParts.Select(damagePart => damagePart.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))) {
                            if (subParts.Length > 1) {
                                damage.Augmented = true;
                            }

                            damage.Min = int.Parse(subParts[0].Split('-')[0]);
                            damage.Max = int.Parse(subParts[0].Split('-')[1]);

                            switch (partParts[0]) {
                                case "Physical Damage":
                                    item.PhysicalDamage = damage;
                                    break;
                                case "Elemental Damage":
                                    item.ElementalDamage.Add(damage);
                                    break;
                                case "Chaos Damage":
                                    item.ChaosDamage = damage;
                                    break;
                            }
                        }
                        break;
                    case "Critical Strike Chance":
                        item.CriticalStrikeChance = double.Parse(Regex.Matches(partParts[1], @"\d+.\d+")[0].Value, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { NumberDecimalSeparator = "." });
                        break;
                    case "Attacks per Second":
                        item.AttacksPerSecond = double.Parse(Regex.Matches(partParts[1], @"\d+.\d+")[0].Value, NumberStyles.AllowDecimalPoint, new NumberFormatInfo { NumberDecimalSeparator = "." });
                        break;
                }
            }
        }
    }
}
