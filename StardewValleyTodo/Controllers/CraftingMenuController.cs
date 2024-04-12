using System.Collections.Generic;
using StardewValley;
using StardewValley.Menus;
using StardewValleyTodo.Helpers;
using StardewValleyTodo.Tracker;

namespace StardewValleyTodo.Controllers {
    public class CraftingMenuController {
        public void ProcessInput(CraftingPage page, InventoryTracker inventoryTracker) {
            var recipe = page.hoverRecipe;

            if (recipe == null) {
                return;
            }

            var recipeName = recipe.DisplayName;

            if (inventoryTracker.Has(recipeName)) {
                inventoryTracker.Off(recipeName);

                return;
            }

            var rawComponents = recipe.recipeList;
            var components = new List<CountableItem>(rawComponents.Count);

            foreach (var kv in rawComponents) {
                var info = Game1.objectData[kv.Key];
                var name = LocalizedStringLoader.Load(info.DisplayName);
                var count = kv.Value;

                components.Add(new CountableItem(kv.Key, name, count));
            }

            var todoRecipe = new TrackableRecipe(recipeName, components);
            inventoryTracker.Toggle(todoRecipe);
        }
    }
}
