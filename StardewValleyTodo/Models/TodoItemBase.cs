using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StardewValleyTodo.Models {
    /// <summary>
    /// Base item of TODO list.
    /// </summary>
    abstract class TodoItemBase {
        /// <summary>
        /// Item name (identifier).
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates new instance.
        /// </summary>
        /// <param name="name">Item name (identifier)</param>
        protected TodoItemBase(string name) {
            Name = name;
        }

        /// <summary>
        /// Draws item onto the sprite batch.
        /// </summary>
        /// <param name="batch">Sprite batch</param>
        /// <param name="position">Start position</param>
        /// <param name="inventory">Player inventory</param>
        /// <returns>Drawn object size</returns>
        public abstract Vector2 Draw(SpriteBatch batch, Vector2 position, Inventory inventory);
    }
}
