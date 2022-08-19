using System.Linq;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud;
using Lumina.Excel.GeneratedSheets;
using Lumina.Text;
using System.Collections.Generic;

namespace SunderingWorld
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Sundering World";

        //private const string commandName = "/pmycommand";

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface)
        {
            pluginInterface.Create<Service>();

            //Service.Configuration = Service.Interface.GetPluginConfig() as Configuration ?? new Configuration();
            //Service.Configuration.Initialize();

            //Service.PluginUI = new PluginUI();

            // Service.Interface.UiBuilder.Draw += DrawUI;
            //Service.Interface.UiBuilder.OpenConfigUi += DrawConfigUI;
            
            if (Service.DataManager.Language == ClientLanguage.ChineseSimplified)
            {
                this.ReplaceDcAndWorlds();
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
