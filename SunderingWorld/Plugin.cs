using System.Linq;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud;
using Lumina.Excel.GeneratedSheets;
using Lumina.Text;
using System.Collections.Generic;
using Dalamud.Logging;

namespace SunderingWorld
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "SunderingWorld";

        //private const string commandName = "/pmycommand";
        internal bool NewerGameVersion = false;

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<Service>();

            Service.Configuration = Service.Interface.GetPluginConfig() as Configuration ?? new Configuration();
            Service.Configuration.Initialize();

            //Service.PluginUI = new PluginUI();

            // Service.Interface.UiBuilder.Draw += DrawUI;
            //Service.Interface.UiBuilder.OpenConfigUi += DrawConfigUI;
            var gameVersiontext = Service.DataManager.GameData.Repositories.First(repo => repo.Key == "ffxiv").Value.Version;
            if (Service.DataManager.Language == ClientLanguage.ChineseSimplified)
            {
                if (new Dalamud.Game.GameVersion(gameVersiontext) <= new Dalamud.Game.GameVersion("2022.12.07.0000.0000"))
                {
                    NewerGameVersion = false;
                    this.ReplaceDcAndWorlds();
                }
                else
                {
                    NewerGameVersion = true;
                }
            }
            if (NewerGameVersion)
            {
#if DEBUG
                PluginLog.Information($"当前游戏版本: {gameVersiontext}");
#endif
                Service.ChatGui.Print($"[{Name}] 发现新的游戏版本版本，请等待更新\n若已开启跨大区，理论上可以永久关闭本插件");
            }
        }

        public void Dispose()
        {
            //Service.PluginUI.Dispose();
            Service.DataManager.Excel.RemoveSheetFromCache<WorldDCGroupType>();
            Service.DataManager.Excel.RemoveSheetFromCache<World>();
        }

        private void DrawUI()
        {
            Service.PluginUI.Draw();
        }

        private void DrawConfigUI()
        {
            Service.PluginUI.SettingsVisible = true;
        }
        public void ReplaceDcAndWorlds()
        {
            var gameDCs = Service.DataManager.GetExcelSheet<WorldDCGroupType>()!;
            var gameWorlds = Service.DataManager.GetExcelSheet<World>()!;
            // close international servers
            foreach (var item in gameWorlds.Where(w => w.RowId < 1000 && w.IsPublic && w.DataCenter.Value?.RowId is >= 1 and <= 4).ToArray())
            {
                item.IsPublic = false;
            }

            foreach (var mydc in ChineseServers.DataCenterMap.Values)
            {
                var dc = gameDCs.GetRow(mydc.Id);

                if (dc != null)
                {
                    dc.Name = new SeString(mydc.Name);
                    dc.Region = (byte)7;
                }

                foreach (var wid in mydc.WorldIds)
                {
                    var myWorld = ChineseServers.WorldMap[wid];
                    var world = gameWorlds.GetRow(wid)!;
                    world.Name = new SeString(myWorld.Name);
                    world.IsPublic = true;
                    world.DataCenter = new Lumina.Excel.LazyRow<WorldDCGroupType>(Service.DataManager.GameData, mydc.Id);
                }
            }
        }
    }
}
