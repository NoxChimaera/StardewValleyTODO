using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace StardewValleyTodo.Models {
    /// <summary>
    /// Todo list.
    /// </summary>
    class TodoList {
        /// <summary>
        /// TODO list items.
        /// </summary>
        public List<TodoItemBase> Items { get; }

        /// <summary>
        /// Create empty todo list.
        /// </summary>
        public TodoList() {
            Items = new List<TodoItemBase>();
        }

        /// <summary>
        /// Returns true if this list contains an item (or recipe) with specified name.
        /// </summary>
        /// <param name="name">Item (or recipe) name</param>
        /// <returns>True if this list contains specified item (or recipe)</returns>
        public bool Has(string name) {
            return Items.Find(x => x.Name == name) != null;
        }

        /// <summary>
        /// Removes item with specified name from the list.
        /// </summary>
        /// <param name="name">Item name</param>
        public void Off(string name) {
            Items.RemoveAll(x => x.Name == name);
        }

        /// <summary>
        /// Adds an item if there is no such item in the list, otherwise removes existed. 
        /// </summary>
        /// <param name="item">Item</param>
        public void Toggle(TodoItemBase item) {
            var found = Items.Find(x => x.Name == item.Name);
            if (found == null) {
                Items.Add(item);
            } else {
                Items.Remove(found);
            }
        }

        /// <summary>
        /// Updates todo list state.
        /// </summary>
        public void Update() {
            // Remove completed bundles.
            var toRemove = Items.OfType<TodoJunimoBundle>().Where(x => x.Bundle.IsComplete).ToArray();
            foreach (var item in toRemove) {
                Items.Remove(item);
            }
        }

        /// <summary>
        /// Draws todo list onto the batch.
        /// </summary>
        /// <param name="batch">Sprite batch</param>
        /// <param name="position">Start position</param>
        /// <param name="inventory">Player inventory</param>
        /// <returns>Drawn area size</returns>
        public Vector2 Draw(SpriteBatch batch, Vector2 position, Inventory inventory) {
            var size = Vector2.Zero;
            foreach (var item in Items) {
                var itemSize = item.Draw(batch, position, inventory);
                position.Y += itemSize.Y + 8;
                size.X = MathHelper.Max(size.X, itemSize.X);
                size.Y += itemSize.Y + 8;
            }
            size.Y -= 8;

            return size;
        }
    }
}
