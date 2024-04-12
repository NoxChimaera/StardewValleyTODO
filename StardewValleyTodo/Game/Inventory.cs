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

            Startup(inventory);
        }

        private void Startup(NativeInventory inventory) {
            _items = new Dictionary<string, int>();

            foreach (var item in inventory) {
                if (item == null) {
                    // Skip empty slots
                    continue;
                }

                var itemId = item.ItemId;
                var count = item.Stack;

                if (_items.ContainsKey(itemId)) {
                    _items[itemId] += count;
                } else {
                    _items.Add(itemId, count);
                }
            }
        }

        public void Update() {
            Startup(_nativeInventory);
        }

        /// <summary>
        /// Returns amount of items of specified id.
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public int Get(string itemId) {
            if (_items.ContainsKey(itemId)) {
                return _items[itemId];
            } else {
                return 0;
            }
        }
    }
}
