namespace StardewValleyTodo.Helpers {
    public static class BundleStringParser {
        public static (string RoomId, int SpriteIndex) parseKey(string raw) {
            var parts = raw.Split('/');

            return (
                RoomId: parts[0],
                SpriteIndex: int.Parse(parts[1])
            );
        }

        public static (string BundleName, string Reward, string Ingredients, int NumberOfItems, string DisplayName) parseValue(string raw) {
            // Animal/BO 16 1/186 1 0 182 1 0 174 1 0 438 1 0 440 1 0 442 1 0/4/5//Животный
            var parts = raw.Split('/');

            return (
                // Animal
                BundleName: parts[0],
                // BO 16 1
                Reward: parts[1],
                // 186 1 0 182 1 0 174 1 0 438 1 0 440 1 0 442 1 0
                Ingredients: parts[2],
                NumberOfItems: parts[4] == "" ? 0 : int.Parse(parts[4]),
                DisplayName: parts[6]
            );
        }
    }
}
