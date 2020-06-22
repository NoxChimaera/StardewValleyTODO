using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System.Collections.Generic;
using System.Linq;

namespace StardewValleyTodo.Models {
    /// <summary>
    /// Crafting recipe.
    /// </summary>
    class TodoRecipe : TodoItemBase {
        /// <summary>
        /// Recipe components.
        /// </summary>
        public List<TodoGameItem> Items { get; }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="name">Recipe name</param>
        /// <param name="items">Recipe components</param>
        public TodoRecipe(string name, IEnumerable<TodoGameItem> items): base(name) {
            Items = items.ToList();
        }

        /// <inheritdoc />
        public override Vector2 Draw(SpriteBatch batch, Vector2 position, Inventory inventory) {
            var display = $"{Name}:";
            var size = Game1.smallFont.MeasureString(display);
            batch.DrawString(Game1.smallFont, display, position, Color.Yellow);
            position.Y += size.Y;
            
            foreach (var item in Items) {
                var itemSize = item.Draw(batch, position, inventory);
                position.Y += itemSize.Y;

                size.X = MathHelper.Max(size.X, itemSize.X);
                size.Y += itemSize.Y;
            }

            return size;
        }
    }
}
