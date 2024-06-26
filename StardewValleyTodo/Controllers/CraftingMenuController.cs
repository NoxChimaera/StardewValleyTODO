using System.Collections.Generic;
using StardewValley;
using StardewValley.Menus;
using StardewValleyTodo.Helpers;
using StardewValleyTodo.Tracker;

namespace StardewValleyTodo.Controllers {
    public class CraftingMenuController {
        private readonly InventoryTracker _inventoryTracker;

        public CraftingMenuController(InventoryTracker inventoryTracker) {
            _inventoryTracker = inventoryTracker;
        }

        public void ProcessInput(CraftingPage page) {
            var recipe = page.hoverRecipe;

            if (recipe == null) {
                return;
            }

            var recipeName = recipe.DisplayName;

            if (_inventoryTracker.Has(recipeName)) {
                _inventoryTracker.Off(recipeName);

                return;
            }

            var rawComponents = recipe.recipeList;
            var components = new List<TrackableItemBase>(rawComponents.Count);

            foreach (var kv in rawComponents) {
                var key = ObjectKey.Parse(kv.Key);
                var count = kv.Value;

                if (key.Contains("-")) {
                    var name = recipe.getNameFromIndex(key);

                    components.Add(new CountableItemCategory(key, name, count));
                } else {
                    var info = Game1.objectData[key];
                    var name = LocalizedStringLoader.Load(info.DisplayName);

                    components.Add(new CountableItem(key, name, count));
                }
            }

            var todoRecipe = new TrackableRecipe(recipeName, components);
            _inventoryTracker.Toggle(todoRecipe);
        }
    }
}
