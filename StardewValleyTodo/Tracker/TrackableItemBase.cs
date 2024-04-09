using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValleyTodo.Game;

namespace StardewValleyTodo.Tracker {
    /// <summary>
    /// Base trackable item.
    /// </summary>
    public abstract class TrackableItemBase {
        /// <summary>
        /// Item name (identifier).
        /// </summary>
        public string Name { get; }

        protected TrackableItemBase(string name) {
            Name = name;
        }

        /// <summary>
        /// Draws item onto the sprite batch.
        /// </summary>
        /// <param name="sb">Sprite batch</param>
        /// <param name="position">Start position</param>
        /// <param name="inventory">Player's inventory</param>
        /// <returns>Drawn object size</returns>
        public abstract Vector2 Draw(SpriteBatch sb, Vector2 position, Inventory inventory);
    }
}
