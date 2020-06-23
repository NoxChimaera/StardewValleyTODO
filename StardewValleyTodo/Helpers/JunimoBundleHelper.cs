using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValleyTodo.Helpers {
    class JunimoBundleHelper {
        private IModHelper Helper { get; }

        public JunimoBundleHelper(IModHelper helper) {
            Helper = helper ?? throw new ArgumentNullException(nameof(helper));
        }

        public string GetCurrentBundleName(JunimoNoteMenu menu) {
            var bundle = Helper.Reflection.GetField<Bundle>(menu, "currentPageBundle").GetValue();

            return bundle.label;
        }

        public int CountEmptySlots(JunimoNoteMenu menu) {
            return menu.ingredientSlots.Count(x => x.item == null);
        }

        public IEnumerable<StardewValley.Object> GetAvailableIngredients(JunimoNoteMenu menu) {
            var completeSlots = menu.ingredientSlots.Where(x => x.item != null).ToLookup(x => x.item.DisplayName);
            var items = new List<StardewValley.Object>();

            foreach (var cmpt in menu.ingredientList) {
                var item = (StardewValley.Object) cmpt.item;
                if (completeSlots.Contains(item.DisplayName)) {
                    continue;
                }

                items.Add(item);
            }

            return items;
        }
    }
}
