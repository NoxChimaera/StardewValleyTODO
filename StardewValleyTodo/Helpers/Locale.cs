using StardewValley;
using System;

namespace StardewValleyTodo.Helpers {
    static class Locale {
        public static string GetCurrentLocaleName() {
            switch (LocalizedContentManager.CurrentLanguageCode) {
                case LocalizedContentManager.LanguageCode.en:
                    return "";
                case LocalizedContentManager.LanguageCode.ru:
                    return ".ru-RU";
                default:
                    throw new NotImplementedException();
            }
        }

    }
}
