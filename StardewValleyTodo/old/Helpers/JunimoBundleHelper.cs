using StardewModdingAPI;
using StardewValley.Menus;
using System;

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
    }
}
