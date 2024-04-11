using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValleyTodo.Game;
using StardewValleyTodo.Tracker;

namespace StardewValleyTodo {
    public class ModEntry : Mod {
        private Inventory _inventory;
        private InventoryTracker _inventoryTracker;

        private JunimoBundles _junimoBundles;

        public override void Entry(IModHelper helper) {
            helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;

            helper.Events.Input.ButtonPressed += Input_ButtonPressed;
            helper.Events.Display.RenderedHud += Display_RenderedHud;

            helper.Events.Player.InventoryChanged += Player_InventoryChanged;
            helper.Events.GameLoop.OneSecondUpdateTicked += GameLoop_OneSecondUpdateTicked;
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e) {
            _inventory = new Inventory(Game1.player.Items);
            _inventoryTracker = new InventoryTracker();
            _junimoBundles = new JunimoBundles();
        }

        private void Player_InventoryChanged(object sender, InventoryChangedEventArgs e) {
            if (!e.IsLocalPlayer) {
                return;
            }

            foreach (var item in e.Added) {
                _inventory.Offset(item.DisplayName, item.Stack);
            }

            foreach (var item in e.Removed) {
                _inventory.Offset(item.DisplayName, -item.Stack);
            }

            foreach (var change in e.QuantityChanged) {
                _inventory.Set(change.Item.DisplayName, change.NewSize);
            }
        }

        private void GameLoop_OneSecondUpdateTicked(object sender, OneSecondUpdateTickedEventArgs e) {
            if (!Context.IsWorldReady) {
                return;
            }

            _junimoBundles.Update();
            _inventoryTracker.Update();
        }

        private void Display_RenderedHud(object sender, RenderedHudEventArgs e) {
            if (Context.IsWorldReady && Game1.displayHUD) {
                DrawTodo();
            }
        }

        private void DrawTodo() {
            var sb = Game1.spriteBatch;
            if (_inventoryTracker.Items.Count == 0) {
                return;
            }

            // TODO: Add settings
            var offset = new Vector2(0, 220);
            var padding = 8;
            var contentOffset = new Vector2(offset.X + padding, offset.Y + padding);

            var size = _inventoryTracker.Draw(sb, contentOffset, _inventory);
            sb.Draw(
                Game1.menuTexture,
                new Rectangle((int) offset.X, (int) offset.Y, (int) size.X + padding * 2, (int) size.Y + padding * 2),
                new Rectangle(8, 256, 3, 4),
                Color.White);
            _inventoryTracker.Draw(sb, contentOffset, _inventory);
        }

        private void Input_ButtonPressed(object sender, ButtonPressedEventArgs e) {
            if (!Context.IsWorldReady) {
                return;
            }

            // TODO: For debug
            if (e.Button == SButton.Q) {
                if (e.IsDown(SButton.LeftShift)) {
                    Game1.timeOfDay -= 30;
                } else {
                    Game1.timeOfDay += 30;
                }
            }

            if (e.Button == SButton.Z) {
                if (e.IsDown(SButton.LeftShift) || e.IsDown(SButton.RightShift)) {
                    ResetInventoryTracker();

                    return;
                }

                var currentMenu = Game1.activeClickableMenu;

                if (currentMenu is GameMenu gameMenu) {
                    ProcessInputInGameMenu(gameMenu);
                } else if (currentMenu is JunimoNoteMenu junimoNoteMenu) {
                    ProcessInputInJunimoMenu(junimoNoteMenu);
                } else if (currentMenu is CarpenterMenu carpenterMenu) {
                    ProcessInputInCarpenterMenu(carpenterMenu);
                }
            }
        }

        private void ResetInventoryTracker() {
            _inventoryTracker.Reset();
        }

        private void ProcessInputInGameMenu(GameMenu menu) {
            var currentPage = menu.GetCurrentPage();

            if (currentPage is CraftingPage page) {
                var recipe = page.hoverRecipe;

                if (recipe == null) {
                    return;
                }

                if (_inventoryTracker.Has(recipe.DisplayName)) {
                    _inventoryTracker.Off(recipe.DisplayName);

                    return;
                }

                var rawComponents = recipe.recipeList;
                var components = new List<CountableItem>(rawComponents.Count);

                foreach (var pair in rawComponents.ToList()) {
                    var info = Game1.objectData[pair.Key];
                    var name = info.Name;
                    var count = pair.Value;

                    components.Add(new CountableItem(name, count));
                }

                var todoRecipe = new TrackableRecipe(recipe.DisplayName, components);
                _inventoryTracker.Toggle(todoRecipe);
            }
        }

        private void ProcessInputInJunimoMenu(JunimoNoteMenu menu) {
            // Not a bundle page
            if (menu.ingredientSlots.Count == 0) {
                return;
            }

            var currentPage = menu.currentPageBundle;
            if (currentPage.complete) {
                return;
            }

            var name = currentPage.label;
            if (_inventoryTracker.Has(name)) {
                _inventoryTracker.Off(name);
            }

            var bundle = _junimoBundles.Find(name);
            var todo = new TrackableJunimoBundle(bundle);
            _inventoryTracker.Toggle(todo);
        }

        // Robin's building menu
        private void ProcessInputInCarpenterMenu(CarpenterMenu menu) {
            var blueprint = menu.Blueprint;
            var name = blueprint.DisplayName;
            var cost = blueprint.BuildCost;
            var displayName = $"{name} ({cost} g.)";

            if (_inventoryTracker.Has(displayName)) {
                _inventoryTracker.Off(displayName);

                return;
            }

            var materials = blueprint.BuildMaterials;
            var components = new List<CountableItem>(materials.Count);

            foreach (var material in materials) {
                // Converts "(O)388" to "388"
                var id = material.Id.Split(')').Last();
                var info = Game1.objectData[id];
                var materialName = info.Name;

                components.Add(new CountableItem(materialName, material.Amount));
            }

            var recipe = new TrackableRecipe(displayName, components);
            _inventoryTracker.Toggle(recipe);
        }
    }
}
