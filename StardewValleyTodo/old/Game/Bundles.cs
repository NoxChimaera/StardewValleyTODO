using StardewValley;
using StardewValley.Locations;
using StardewValleyTodo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StardewValleyTodo.Game {
    /// <summary>
    /// Junimo bundles.
    /// </summary>
    class Bundles {
        /// <summary>
        /// Invokes when a bundle was completed.
        /// </summary>
        public event EventHandler<Bundle> BundleCompleted;

        /// <summary>
        /// Invokes when an item was donated into bundle.
        /// </summary>
        public event EventHandler<BundleIngredient> ItemDonated;

        private CommunityCenter communityCenter;

        private List<Bundle> bundles;

        /// <summary>
        /// Creates new instance.
        /// </summary>
        public Bundles() {
            bundles = new List<Bundle>();
        }

        /// <summary>
        /// Loads bundle information.
        /// </summary>
        public void Startup() {
            communityCenter = Game1.getLocationFromName("CommunityCenter") as CommunityCenter;

            var raw = Game1.content.Load<Dictionary<string, string>>("Data\\Bundles" + Locale.GetCurrentLocaleName());

            foreach (var pair in raw) {
                var key = pair.Key.Split('/');
                var id = int.Parse(key[1]);

                var value = pair.Value.Split('/');
                var english = value[0];
                var locale = value.Last();
                var slots = int.Parse(value[value.Length - 2]);
                if (value.Length == 5) {
                    slots = 0;
                }

                var bundle = new Bundle(id, english, locale, slots);
                bundles.Add(bundle);

                var netbundle = communityCenter.bundles[id];

                var rawIngredients = value[2].Split(' ');
                for (var i = 0; i < rawIngredients.Length; i += 3) {
                    var objectId = int.Parse(rawIngredients[i]);
                    var objectCount = int.Parse(rawIngredients[i + 1]);
                    var objectQuality = int.Parse(rawIngredients[i + 2]);

                    if (!IsObjectExists(objectId)) {
                        continue;
                    }

                    var objectName = LoadObjectName(objectId);
                    var donated = netbundle[i / 3];

                    var ingredient = new BundleIngredient(objectId, i / 3, objectName, objectCount, objectQuality, donated);
                    bundle.AddIngredient(ingredient);
                }

                if (bundle.Slots == 0) {
                    bundle.Slots = bundle.Ingredients.Count;
                }

                if (netbundle.All(x => x == true)) {
                    bundle.IsComplete = true;
                }
            }
        }

        /// <summary>
        /// Updates bundles state. Invokes events.
        /// </summary>
        public void Update() {
            var net = communityCenter.bundles;
            foreach (var bundle in bundles.Where(x => !x.IsComplete)) {
                var netbundle = net[bundle.Id];
                foreach (var ingredient in bundle.Ingredients) {
                    if (netbundle[ingredient.Index] && !ingredient.IsDonated) {
                        ingredient.IsDonated = true;
                        ItemDonated?.Invoke(this, ingredient);
                    }
                }

                if (bundle.Ingredients.Where(x => x.IsDonated).Count() >= bundle.Slots) {
                    bundle.IsComplete = true;
                    BundleCompleted?.Invoke(this, bundle);
                }
            }
        }

        /// <summary>
        /// Returns a bundle with specified name.
        /// </summary>
        /// <param name="displayName">Localized name</param>
        /// <returns>Bundle</returns>
        public Bundle Find(string displayName) {
            return bundles.Find(x => x.DisplayName == displayName);
        }

        private bool IsObjectExists(int id) {
            return false;
            // return Game1.objectInformation.ContainsKey(id);
        }

        private string LoadObjectName(int id) {
            return "";
            // return Game1.objectInformation[id].Split('/')[4];
        }
    }
}
