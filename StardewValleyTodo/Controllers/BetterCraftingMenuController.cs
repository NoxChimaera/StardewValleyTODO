using System.Collections.Generic;
using System.Reflection;
using StardewValley;
using StardewValley.Menus;
using StardewValleyTodo.Helpers;
using StardewValleyTodo.Tracker;

namespace StardewValleyTodo.Controllers {
    public class BetterCraftingMenuController {
        public void ProcessInput(IClickableMenu page, InventoryTracker inventoryTracker) {
            var pageType = page.GetType();
            var hoverRecipeGetter = pageType.GetField("hoverRecipe",  BindingFlags.Instance | BindingFlags.NonPublic);
            var hoverRecipe = hoverRecipeGetter.GetValue(page);

            if (hoverRecipe == null) {
                return;
            }

            var recipeName = ((dynamic) hoverRecipe).DisplayName;

            if (inventoryTracker.Has(recipeName)) {
                inventoryTracker.Off(recipeName);

                return;
            }

            var recipeGetter = hoverRecipe.GetType().GetField("Recipe");
            var recipeProp = recipeGetter.GetValue(hoverRecipe);

            var recipeListGetter = recipeProp.GetType().GetField("recipeList");
            var rawComponents = (Dictionary<string, int>) recipeListGetter.GetValue(recipeProp);
            var components = new List<CountableItem>(rawComponents.Count);

            foreach (var kv in rawComponents) {
                var key = ObjectKey.Parse(kv.Key);
                var info = Game1.objectData[key];
                var name = LocalizedStringLoader.Load(info.DisplayName);
                var count = kv.Value;

                components.Add(new CountableItem(key, name, count));
            }

            var todoRecipe = new TrackableRecipe(recipeName, components);
            inventoryTracker.Toggle(todoRecipe);
        }
    }
}
