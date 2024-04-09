using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValleyTodo.Game;
using StardewValleyTodo.Helpers;

namespace StardewValleyTodo {
    // public class ModEntry : Mod {
    //     public override void Entry(IModHelper helper) {

    //         junimoHelper = new JunimoBundleHelper(Helper);
    //     }

    //     private void InJunimoNoteMenu(JunimoNoteMenu menu) {
    //         // Not a bundle page
    //         if (menu.ingredientSlots.Count == 0) {
    //             return;
    //         }

    //         var bundleName = junimoHelper.GetCurrentBundleName(menu);
    //         if (todolist.Has(bundleName)) {
    //             todolist.Off(bundleName);
    //             return;
    //         }

    //         var bundle = bundles.Find(bundleName);
    //         var todo = new TodoJunimoBundle(bundle);
    //         todolist.Toggle(todo);
    //     }
    // }
}
