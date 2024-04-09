using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValleyTodo.Game {
    /// <summary>
    /// Junimo bundle.
    /// </summary>
    class Bundle {
        /// <summary>
        /// Bundle identifier.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// English name.
        /// </summary>
        public string EnglishName { get; }

        /// <summary>
        /// Localized name.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Bundle ingredients.
        /// </summary>
        public List<BundleIngredient> Ingredients { get; }

        /// <summary>
        /// Bundle slots.
        /// </summary>
        public int Slots { get; set; }

        /// <summary>
        /// Is complete.
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Creates new junimo bundle.
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="englishName">English name</param>
        /// <param name="displayName">Localized name</param>
        /// <param name="slots">Slots</param>
        public Bundle(int id, string englishName, string displayName, int slots) {
            Id = id;
            EnglishName = englishName ?? throw new ArgumentNullException(nameof(englishName));
            DisplayName = displayName ?? throw new ArgumentNullException(nameof(displayName));
            Ingredients = new List<BundleIngredient>();
            Slots = slots;
        }

        /// <summary>
        /// Adds an ingredient into bundle description.
        /// </summary>
        /// <param name="ingredient">Ingredient</param>
        public void AddIngredient(BundleIngredient ingredient) {
            Ingredients.Add(ingredient);
        }

        public int CountEmptySlots() {
            return Math.Max(0, Slots - Ingredients.Where(x => x.IsDonated).Count());
        }
    }
}
