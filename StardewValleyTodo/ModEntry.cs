using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValleyTodo.Helpers;
using StardewValleyTodo.Models;

namespace StardewValleyTodo {
    public class ModEntry : Mod {
        private TodoList todolist = new TodoList();
        private Inventory inventory;

        private JunimoBundleHelper junimoHelper;

        public override void Entry(IModHelper helper) {
            helper.Events.Input.ButtonPressed += Input_ButtonPressed;
            helper.Events.Display.RenderedHud += Display_RenderedHud;

            helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
            helper.Events.Player.InventoryChanged += Player_InventoryChanged;


            junimoHelper = new JunimoBundleHelper(Helper);
        }

        /// <summary>
        /// Restores state of player's inventory after game loading.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e) {
            inventory = new Inventory(Game1.player.items);
            todolist = new TodoList();
        }

        /// <summary>
        /// Updates state of player's inventory after native inventory state change.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void Player_InventoryChanged(object sender, InventoryChangedEventArgs e) {
            if (!e.IsLocalPlayer) {
                return;
            }

            foreach (var item in e.Added) {
                inventory.Offset(item.DisplayName, item.Stack);
            }

            foreach (var item in e.Removed) {
                inventory.Offset(item.DisplayName, -item.Stack);
            }

            foreach (var change in e.QuantityChanged) {
                inventory.Set(change.Item.DisplayName, change.NewSize);
            }
        }
        
        private void Display_RenderedHud(object sender, RenderedHudEventArgs e) {
            if (Context.IsWorldReady && Game1.displayHUD) {
                DrawTodo();
            }
        }
    
        /// <summary>
        /// Renders TODO list.
        /// </summary>
        private void DrawTodo() {
            var b = Game1.spriteBatch;
            if (todolist.Items.Count == 0) {
                return;
            }

            // Gap for NPCLocationMap's minimap
            var offset = new Vector2(0, 220);
            var padding = 8;
            var contentOffset = new Vector2(offset.X + padding, offset.Y + padding);

            var size = todolist.Draw(b, contentOffset, inventory);
            b.Draw(
                Game1.menuTexture, 
                new Rectangle((int) offset.X, (int) offset.Y, (int) size.X + padding * 2, (int) size.Y + padding * 2), 
                new Rectangle(8, 256, 3, 4), 
                Color.White);
            todolist.Draw(b, contentOffset, inventory);
        }

        /// <summary>
        /// Handles player input.
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Arguments</param>
        private void Input_ButtonPressed(object sender, ButtonPressedEventArgs e) {
            if (!Context.IsWorldReady) return;
            
            if (e.Button == SButton.Z) {
                if (Game1.activeClickableMenu is GameMenu menu) {
                    if (menu.GetCurrentPage() is CraftingPage page) {
                        var item = Helper.Reflection.GetField<CraftingRecipe>(page, "hoverRecipe").GetValue();
                        if (item == null) {
                            return;
                        }

                        if (todolist.Has(item.DisplayName)) {
                            todolist.Off(item.DisplayName);
                            return;
                        }

                        var rawComponents = Helper.Reflection.GetField<Dictionary<int, int>>(item, "recipeList").GetValue();
                        var components = new List<TodoGameItem>(rawComponents.Count);
                        foreach (var component in rawComponents.Keys) {
                            var info = Game1.objectInformation[component];
                            var name = info.Split('/')[4];
                            var count = rawComponents[component];

                            components.Add(new TodoGameItem(name, count));
                        }

                        var recipe = new TodoRecipe(item.DisplayName, components);
                        todolist.Toggle(recipe);
                    }
                } else if (Game1.activeClickableMenu is JunimoNoteMenu) {
                    InJunimoNoteMenu();
                }
            }
        }

        private void InJunimoNoteMenu() {
            var menu = (JunimoNoteMenu) Game1.activeClickableMenu;
            
            // Not a bundle page
            if (menu.ingredientSlots.Count == 0) {
                return;
            }

            var bundleName = junimoHelper.GetCurrentBundleName(menu);
            if (todolist.Has(bundleName)) {
                todolist.Off(bundleName);
                return;
            }

            var ingredients = junimoHelper
                .GetAvailableIngredients(menu)
                .Select(x => new TodoJunimoBundleItem(x.DisplayName, x.Stack, x.Quality == 2));

            var bundle = new TodoJunimoBundle(bundleName, ingredients, junimoHelper.CountEmptySlots(menu));
            todolist.Toggle(bundle);
        }

    }
}
