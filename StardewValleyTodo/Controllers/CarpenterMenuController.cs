using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Menus;
using StardewValleyTodo.Helpers;
using StardewValleyTodo.Tracker;

namespace StardewValleyTodo.Controllers {
    public class CarpenterMenuController {
        public void ProcessInput(CarpenterMenu menu, InventoryTracker inventoryTracker) {
            var blueprint = menu.Blueprint;
            var name = blueprint.DisplayName;
            var cost = blueprint.BuildCost;
            var displayName = $"{name} ({cost} g.)";

            if (inventoryTracker.Has(displayName)) {
                inventoryTracker.Off(displayName);

                return;
            }

            var materials = blueprint.BuildMaterials;
            var components = new List<CountableItem>(materials.Count);

            foreach (var material in materials) {
                // Converts "(O)388" to "388"
                var id = material.Id.Split(')').Last();
                var info = Game1.objectData[id];
                var materialName = LocalizedStringLoader.Load(info.DisplayName);

                components.Add(new CountableItem(materialName, material.Amount));
            }

            var recipe = new TrackableRecipe(displayName, components);
            inventoryTracker.Toggle(recipe);
        }
    }
}
