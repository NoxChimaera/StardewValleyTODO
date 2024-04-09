using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValleyTodo.Game;
using System.Linq;

namespace StardewValleyTodo.Tracker {
    /// <summary>
    /// Junimo bundle todo item.
    /// </summary>
    class TrackableJunimoBundle : TrackableItemBase {
        /// <summary>
        /// Junimo bundle.
        /// </summary>
        public Bundle Bundle { get; }

        /// <summary>
        /// Creates new todo item.
        /// </summary>
        /// <param name="bundle">Junimo bundle</param>
        public TrackableJunimoBundle(Bundle bundle)
        : base(bundle.DisplayName) {
            Bundle = bundle;
        }

        /// <inheritdoc />
        public override Vector2 Draw(SpriteBatch batch, Vector2 position, Inventory inventory) {
            var caption = $"{Bundle.DisplayName} (нужно {Bundle.CountEmptySlots()}):";
            var size = Game1.smallFont.MeasureString(caption);
            batch.DrawString(Game1.smallFont, caption, position, Color.Yellow);
            position.Y += size.Y;

            foreach (var item in Bundle.Ingredients.Where(x => !x.IsDonated)) {
                var itemSize = DrawItem(batch, position, inventory, item);
                position.Y += itemSize.Y;

                size.X = MathHelper.Max(size.X, itemSize.X);
                size.Y += itemSize.Y;
            }

            return size;
        }

        /// <summary>
        /// Draws bundle ingredient.
        /// </summary>
        /// <param name="batch">Sprite batch</param>
        /// <param name="position">Position</param>
        /// <param name="inventory">Inventory</param>
        /// <param name="ingredient">Ingredient</param>
        /// <returns>Drawn area size</returns>
        private Vector2 DrawItem(SpriteBatch batch, Vector2 position, Inventory inventory, BundleIngredient ingredient) {
            // TODO: check item quality
            var has = inventory.Get(ingredient.DisplayName);
            var display = $"{ingredient.DisplayName} {(ingredient.Quality == 2 ? "*" : "")} ({has}/{ingredient.Count})";
            var color = has >= ingredient.Count ? Color.LightGreen : Color.White;
            batch.DrawString(Game1.smallFont, display, position, color);

            var size = Game1.smallFont.MeasureString(display);
            return size;
        }
    }
}
