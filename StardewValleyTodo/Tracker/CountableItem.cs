using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValleyTodo.Game;

namespace StardewValleyTodo.Tracker {
    /// <summary>
    /// Game item.
    /// </summary>
    class CountableItem : TrackableItemBase {
        /// <summary>
        /// Count to craft.
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="name">Item name</param>
        /// <param name="count">Count to craft</param>
        public CountableItem(string name, int count): base(name) {
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
