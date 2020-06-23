using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using System.Collections.Generic;
using System.Linq;

namespace StardewValleyTodo.Models {
    class TodoJunimoBundle : TodoItemBase {
        /// <summary>
        /// Bundle components.
        /// </summary>
        public List<TodoJunimoBundleItem> Items { get; }

        public int EmptySlots { get; }

        public TodoJunimoBundle(string name, IEnumerable<TodoJunimoBundleItem> items, int emptySlots) : base(name) {
            Items = items.ToList();
            EmptySlots = emptySlots;
        }

        public override Vector2 Draw(SpriteBatch batch, Vector2 position, Inventory inventory) {
            var display = $"{Name} ({EmptySlots} slots):";
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
