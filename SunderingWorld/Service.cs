using Dalamud.Data;
using Dalamud.Game;
using Dalamud.Game.ClientState;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.Game.Gui.FlyText;
using Dalamud.IoC;
using Dalamud.Plugin;

namespace SunderingWorld
{
    internal class Service
    {
        internal static Configuration Configuration { get; set; } = null!;
        internal static PluginUI PluginUI { get; set; } = null!;

        [PluginService]
        internal static DalamudPluginInterface Interface { get; private set; } = null!;
        [PluginService]
        internal static DataManager DataManager { get; private set; } = null!;
        [PluginService]
        internal static ChatGui ChatGui { get; private set; } = null!;
    }
}
