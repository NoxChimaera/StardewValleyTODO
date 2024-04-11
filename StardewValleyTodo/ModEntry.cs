using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValleyTodo.Controllers;
using StardewValleyTodo.Game;
using StardewValleyTodo.Tracker;

namespace StardewValleyTodo {
    public class ModEntry : Mod {
        private Inventory _inventory;
        private InventoryTracker _inventoryTracker;

        private JunimoBundles _junimoBundles;

        private CraftingMenuController _craftingMenuController;
        private CarpenterMenuController _carpenterMenuController;
        private JunimoBundleController _junimoBundleController;

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

            _craftingMenuController = new CraftingMenuController();
            _carpenterMenuController = new CarpenterMenuController();
            _junimoBundleController = new JunimoBundleController();
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
                    if (gameMenu.GetCurrentPage() is CraftingPage page) {
                        _craftingMenuController.ProcessInput(page, _inventoryTracker);
                    }
                } else if (currentMenu is JunimoNoteMenu junimoNoteMenu) {
                    _junimoBundleController.ProcessInput(junimoNoteMenu, _inventoryTracker, _junimoBundles);
                } else if (currentMenu is CarpenterMenu carpenterMenu) {
                    _carpenterMenuController.ProcessInput(carpenterMenu, _inventoryTracker);
                }
            }
        }

        private void ResetInventoryTracker() {
            _inventoryTracker.Reset();
        }
    }
}
