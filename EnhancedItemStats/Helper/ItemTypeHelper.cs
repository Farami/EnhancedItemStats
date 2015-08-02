namespace EnhancedItemStats.Helper {
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Models;

    using Newtonsoft.Json;

    public static class ItemTypeHelper {
        private static readonly ParsedBaseTypes BaseTypes;

        static ItemTypeHelper() {
            var unparsedItems = File.ReadAllText(@"Data\BaseItems.json");

            BaseTypes = JsonConvert.DeserializeObject<ParsedBaseTypes>(unparsedItems);
        }

        public static Type GetType(string item) {
            var type = new Type();

            var subTypeNames = BaseTypes.MainTypes.SelectMany(bt => bt.SpecificTypes).Select(st => st.Name);
            foreach (var subType in subTypeNames.Where(item.Contains)) {
                type.MainType = BaseTypes.MainTypes.First(mt => mt.SpecificTypes.Any(st => st.Name == subType)).Name;
                type.SubType = subType;
            }

            return type;
        }

        private class ParsedBaseType {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
        }

        private class GearBaseType {
            [JsonProperty(PropertyName = "type")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "Item")]
            public List<ParsedBaseType> SpecificTypes { get; set; }
        }

        private class ParsedBaseTypes {
            [JsonProperty(PropertyName = "GearBaseTypes")]
            public List<GearBaseType> MainTypes { get; set; }
        }
    }
}
