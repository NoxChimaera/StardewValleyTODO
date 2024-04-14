using StardewModdingAPI;
using StardewModdingAPI.Utilities;

namespace StardewValleyTodo.Config {
    public sealed class ModConfig {
        public int VerticalOffset { get; set; } = 0;

        public KeybindList ToggleVisibility { get; set; } = KeybindList.ForSingle(SButton.Z);
        public KeybindList ClearTracker { get; set; } = KeybindList.ForSingle(SButton.Z, SButton.LeftShift);
        public KeybindList TrackItem { get; set; } = KeybindList.ForSingle(SButton.Z);
    }
}
