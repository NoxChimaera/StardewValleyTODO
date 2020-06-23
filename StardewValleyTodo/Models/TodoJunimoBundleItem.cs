using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace StardewValleyTodo.Models {
    class TodoJunimoBundleItem : TodoItemBase {
        /// <summary>
        /// Count to craft.
        /// </summary>
        public int Count { get; }

        public bool NeedsGoldQuality { get; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        /// <param name="name">Item name</param>
        /// <param name="count">Count to craft</param>
        public TodoJunimoBundleItem(string name, int count, bool needsGoldQuality) : base(name) {
            Count = count;
            NeedsGoldQuality = needsGoldQuality;
        }

        /// <inheritdoc />
        public override Vector2 Draw(SpriteBatch batch, Vector2 position, Inventory inventory) {
            // TODO: check item quality
            var has = inventory.Get(Name);
            var display = $"{Name} {(NeedsGoldQuality ? "*" : "")} ({has}/{Count})";
            var color = has >= Count ? Color.LightGreen : Color.White;
            batch.DrawString(Game1.smallFont, display, position, color);

            var size = Game1.smallFont.MeasureString(display);
            return size;
        }
    }
}
