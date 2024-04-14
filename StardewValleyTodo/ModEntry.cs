using System;
using GenericModConfigMenu;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValleyTodo.Config;
using StardewValleyTodo.Controllers;
using StardewValleyTodo.Game;
using StardewValleyTodo.Tracker;

namespace StardewValleyTodo {
    public class ModEntry : Mod {
        private ModConfig _config;

        private Inventory _inventory;
        private InventoryTracker _inventoryTracker;

        private JunimoBundles _junimoBundles;

        private CraftingMenuController _craftingMenuController;
        private CarpenterMenuController _carpenterMenuController;
        private JunimoBundleController _junimoBundleController;

        public override void Entry(IModHelper helper) {
            _config = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched;

            helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;

            helper.Events.Input.ButtonPressed += Input_ButtonPressed;
            helper.Events.Display.RenderedHud += Display_RenderedHud;

            helper.Events.Player.InventoryChanged += Player_InventoryChanged;
            helper.Events.GameLoop.OneSecondUpdateTicked += GameLoop_OneSecondUpdateTicked;
        }

        private void Shutdown() {
            var helper = Helper;

            helper.Events.GameLoop.GameLaunched -= GameLoop_GameLaunched;

            helper.Events.GameLoop.SaveLoaded -= GameLoop_SaveLoaded;

            helper.Events.Input.ButtonPressed -= Input_ButtonPressed;
            helper.Events.Display.RenderedHud -= Display_RenderedHud;

            helper.Events.Player.InventoryChanged -= Player_InventoryChanged;
            helper.Events.GameLoop.OneSecondUpdateTicked -= GameLoop_OneSecondUpdateTicked;
        }

        private void GameLoop_GameLaunched(object sender, GameLaunchedEventArgs e) {
            var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null) {
                // Generic Mod Config Menu is not installed
                return;
            }

            configMenu.Register(this.ModManifest, () => _config = new ModConfig(), () => Helper.WriteConfig(_config));

            configMenu.AddNumberOption(
                ModManifest,
                () => _config.VerticalOffset,
                value => _config.VerticalOffset = value,
                () => "Vertical Offset"
            );
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e) {
            try {
                _inventory = new Inventory(Game1.player.Items);
                _inventoryTracker = new InventoryTracker();
                _junimoBundles = new JunimoBundles();

                _craftingMenuController = new CraftingMenuController();
                _carpenterMenuController = new CarpenterMenuController();
                _junimoBundleController = new JunimoBundleController();
            } catch (Exception exception) {
                Shutdown();

                throw new Exception("Failed to initialize Recipe Tracker mod", exception);
            }
        }

        private void Player_InventoryChanged(object sender, InventoryChangedEventArgs e) {
            if (!e.IsLocalPlayer) {
                return;
            }

            _inventory.Update();
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
            if (_inventoryTracker.Items.Count == 0 || !_inventoryTracker.IsVisible) {
                return;
            }

            var offset = new Vector2(0, _config.VerticalOffset);
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

            if (e.Button == SButton.Z) {
                var currentMenu = Game1.activeClickableMenu;

                // In menus
                if (currentMenu is GameMenu gameMenu) {
                    if (gameMenu.GetCurrentPage() is CraftingPage page) {
                        _craftingMenuController.ProcessInput(page, _inventoryTracker);
                    }
                    return;
                } else if (currentMenu is JunimoNoteMenu junimoNoteMenu) {
                    _junimoBundleController.ProcessInput(junimoNoteMenu, _inventoryTracker, _junimoBundles);
                    return;
                } else if (currentMenu is CarpenterMenu carpenterMenu) {
                    _carpenterMenuController.ProcessInput(carpenterMenu, _inventoryTracker);
                    return;
                }

                // In game
                if (e.IsDown(SButton.LeftShift) || e.IsDown(SButton.RightShift)) {
                    ResetInventoryTracker();
                    return;
                }

                // Hide or show tracker UI
                _inventoryTracker.IsVisible = !_inventoryTracker.IsVisible;
            }
        }

        private void ResetInventoryTracker() {
            _inventoryTracker.Reset();
        }
    }
}
