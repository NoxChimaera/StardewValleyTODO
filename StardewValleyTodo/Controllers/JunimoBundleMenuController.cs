using StardewValley.Menus;
using StardewValleyTodo.Game;
using StardewValleyTodo.Tracker;

namespace StardewValleyTodo.Controllers {
    public class JunimoBundleController {
        public void ProcessInput(JunimoNoteMenu menu, InventoryTracker inventoryTracker, JunimoBundles bundles) {
            // Not a bundle page
            if (menu.ingredientSlots.Count == 0) {
                return;
            }

            var currentPage = menu.currentPageBundle;
            if (currentPage.complete) {
                return;
            }

            var name = currentPage.label;
            if (inventoryTracker.Has(name)) {
                inventoryTracker.Off(name);
            }

            var bundle = bundles.Find(name);
            var todo = new TrackableJunimoBundle(bundle);
            inventoryTracker.Toggle(todo);
        }
    }
}
