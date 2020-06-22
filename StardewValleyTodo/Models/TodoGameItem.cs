using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace StardewValleyTodo.Models {
    /// <summary>
    /// Game item.
    /// </summary>
    class TodoGameItem : TodoItemBase {
        /// <summary>
        /// Count to craft.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="name">Item name</param>
        /// <param name="count">Count to craft</param>
        public TodoGameItem(string name, int count): base(name) {
            Count = count;
        }

        /// <inheritdoc />
        public override Vector2 Draw(SpriteBatch batch, Vector2 position, Inventory inventory) {
            var has = inventory.Get(Name);
            var display = $"{Name} ({has}/{Count})";
            var color = has >= Count ? Color.LightGreen : Color.White;
            batch.DrawString(Game1.smallFont, display, position, color);

            var size = Game1.smallFont.MeasureString(display);
            return size;
        }
    }
}
