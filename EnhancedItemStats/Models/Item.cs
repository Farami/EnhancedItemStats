namespace EnhancedItemStats.Models {
    using System.Collections.Generic;
    using System.Linq;

    public class Item {
        public string Name { get; set; }

        public Rarity Rarity { get; set; }

        public Type Type { get; set; }

        public Quality Quality { get; set; }

        public Damage PhysicalDamage { get; set; }

        public List<Damage> ElementalDamage { get; set; } = new List<Damage>();

        public Damage ChaosDamage { get; set; }

        public double CriticalStrikeChance { get; set; }

        public double AttacksPerSecond { get; set; }

        public bool Identified { get; set; } = true;

        public double PhysicalDps => (this.PhysicalDamage.Min + this.PhysicalDamage.Max) / 2 * this.AttacksPerSecond;

        public double ElementalDps => (this.ElementalDamage.Sum(ed => ed.Min) + this.ElementalDamage.Sum(ed => ed.Max)) / 2 * this.AttacksPerSecond;

        public double ChaosDps => (this.ChaosDamage.Min + this.ChaosDamage.Max) / 2 * this.AttacksPerSecond;

        public double Dps => this.PhysicalDps + this.ElementalDps + this.ChaosDps;
    }

    public struct Quality {
        public Quality(int quantityValue, bool augmented) {
            this.Value = quantityValue;
            this.Augmented = augmented;
        }

        public int Value { get; set; }

        public bool Augmented { get; set; }
    }

    public struct Damage {
        public int Min { get; set; }

        public int Max { get; set; }

        public bool Augmented { get; set; }
    }

    public struct Type {
        public string MainType { get; set; }

        public string SubType { get; set; }
    }

    public enum Rarity {
        Currency,
        Normal,
        Magic,
        Rare,
        Unique
    }
}
