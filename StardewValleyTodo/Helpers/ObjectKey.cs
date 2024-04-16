using System.Text.RegularExpressions;

namespace StardewValleyTodo.Helpers {
    public static class ObjectKey {
        private static Regex _regex = new Regex(@"(\(.+\)\s?)?(\d+)");

        public static string Parse(string rawKey) {
            var id = _regex.Match(rawKey).Groups[2];

            return id.Success ? id.Value : rawKey;
        }
    }
}
