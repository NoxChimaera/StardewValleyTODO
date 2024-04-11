using System;
using System.Collections.Generic;
using System.Linq;
using StardewValley;
using StardewValley.Locations;
using StardewValleyTodo.Helpers;

namespace StardewValleyTodo.Game {
    public class JunimoBundles {
        private CommunityCenter _communityCenter;
        private List<JunimoBundle> _junimoBundles;

        /// <summary>
        /// Invokes when an item was donated into bundle.
        /// </summary>
        public event EventHandler<JunimoBundleIngredient> ItemDonated;

        /// <summary>
        /// Invokes when a bundle was completed.
        /// </summary>
        public event EventHandler<JunimoBundle> BundleCompleted;

        public JunimoBundles() {
            Startup();
        }

        private void Startup() {
            _communityCenter = Game1.getLocationFromName("CommunityCenter") as CommunityCenter;
            _junimoBundles = new List<JunimoBundle>();

            var raw = Game1.content.Load<Dictionary<string, string>>("Data\\Bundles");

            foreach (var rawBundle in raw) {
                // Bulletin Board/35
                var (_, roomId) = BundleStringParser.parseKey(rawBundle.Key);

                // Fodder/BO 104 1/262 10 0 178 10 0 613 3 0/3///Кормовой
                var (_, _, ingredientsString, slots, localeName) = BundleStringParser.parseValue(rawBundle.Value);

                var bundle = new JunimoBundle(roomId, localeName, slots);
                _junimoBundles.Add(bundle);

                var netbundle = _communityCenter.bundles[roomId];
                var rawIngredients = ingredientsString.Split(' ');
                for (var i = 0; i < rawIngredients.Length; i += 3) {
                    var objectId = rawIngredients[i];
                    var objectCount = int.Parse(rawIngredients[i + 1]);
                    var objectQuality = int.Parse(rawIngredients[i + 2]);

                    // Skip Money bundles
                    if (objectId == "-1") {
                        continue;
                    }

                    var displayNameRaw = Game1.objectData[objectId].DisplayName;
                    var displayName = LocalizedStringLoader.Load(displayNameRaw);

                    var isDonated = netbundle[i / 3];
                    var ingredient = new JunimoBundleIngredient(objectId, i / 3, displayName, objectCount, objectQuality, isDonated);
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

        public void Update() {
            var net = _communityCenter.bundles;

            foreach (var bundle in _junimoBundles.Where(x => !x.IsComplete)) {
                var netbundle = net[bundle.RoomId];
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
        public JunimoBundle Find(string displayName) {
            return _junimoBundles.Find(x => x.Name == displayName);
        }
    }
}