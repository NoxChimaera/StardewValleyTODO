using System.Collections.Generic;
using NativeInventory = StardewValley.Inventories.Inventory;

namespace StardewValleyTodo.Game {
    public class Inventory {
        private readonly NativeInventory _nativeInventory;
        private Dictionary<string, int> _items;

        /// <summary>
        /// Represents player's inventory.
        /// </summary>
        /// <param name="inventory">Native inventory structure</param>
        public Inventory(NativeInventory inventory) {
            _nativeInventory = inventory;
            _items = new Dictionary<string, int>();

            Startup(inventory);
        }

        private void Startup(NativeInventory inventory) {
            foreach (var item in inventory) {
                if (item == null) {
                    // Skip empty slots
                    continue;
                }

                var itemName = item.DisplayName;
                var count = item.Stack;

                if (_items.ContainsKey(itemName)) {
                    _items[itemName] += count;
                } else {
                    _items.Add(itemName, count);
                }
            }
        }

        /// <summary>
        /// Returns amount of items of specified id.
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public int Get(string itemName) {
            if (_items.ContainsKey(itemName)) {
                return _items[itemName];
            } else {
                return 0;
            }
        }

        /// <summary>
        /// Sets count of items of specified id.
        /// </summary>
        /// <param name="item">Item name</param>
        /// <param name="count">Count</param>
        public void Set(string item, int count) {
            _items[item] = count;
        }

        /// <summary>
        /// Adds count of items of specified id.
        /// </summary>
        /// <param name="item">Item name</param>
        /// <param name="offset">Amount added</param>
        public void Offset(string item, int offset) {
            if (_items.ContainsKey(item)) {
                _items[item] += offset;
            } else {
                _items[item] = offset;
            }
        }
    }
}
