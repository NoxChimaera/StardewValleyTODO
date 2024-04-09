using System.Collections.Generic;
using SVInventory = StardewValley.Inventories.Inventory;

namespace StardewValleyTodo {
    /// <summary>
    /// Represents players inventory aggregated by item names. Uses display name as item identifiers.
    /// </summary>
    public class Inventory2 {
        private Dictionary<string, int> items;

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="inventory">Native inventory represenation</param>
        public Inventory2(SVInventory inventory) {
            Startup(inventory);
        }

        /// <summary>
        /// Copies items from native inventory into this.
        /// </summary>
        /// <param name="inventory">Native inventory</param>
        private void Startup(SVInventory inventory) {
            items = new Dictionary<string, int>();
            foreach (var item in inventory) {
                if (item == null) {
                    continue;
                }

                var key = item.DisplayName;
                var value = item.Stack;

                if (items.ContainsKey(key)) {
                    items[item.DisplayName] += value;
                } else {
                    items.Add(key, value);
                }
            }
        }

        /// <summary>
        /// Returns count of items of specified id.
        /// </summary>
        /// <param name="item">Item name</param>
        /// <returns></returns>
        public int Get(string item) {
            if (items.ContainsKey(item)) {
                return items[item];
            } else {
                return 0;
            }
        }

        /// <summary>
        /// Adds specified count of items of specified id.
        /// </summary>
        /// <param name="item">Item name</param>
        /// <param name="offset">Amount added</param>
        public void Offset(string item, int offset) {
            if (items.ContainsKey(item)) {
                items[item] += offset;
            } else {
                items[item] = offset;
            }

        }

        /// <summary>
        /// Sets specified count of items of specified id.
        /// </summary>
        /// <param name="item">Item name</param>
        /// <param name="count">Count</param>
        public void Set(string item, int count) {
            items[item] = count;
        }
    }
}
