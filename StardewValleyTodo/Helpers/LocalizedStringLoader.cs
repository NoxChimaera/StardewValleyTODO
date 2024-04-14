using System;
using System.Text.RegularExpressions;
using StardewValley;

namespace StardewValleyTodo.Helpers {
    public static class LocalizedStringLoader {
        public static string Load(string path) {
            if (!path.Contains("LocalizedText")) {
                return path;
            }

            var re = new Regex(@"\[LocalizedText (.*)\]");
            var match = re.Match(path);
            var id = match.Groups[1].Value;

            return Game1.content.LoadString(id);
        }
    }
}
