using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace SunderingWorld
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;


        public void Initialize()
        {

        }

        public void Save()
        {
            Service.Interface.SavePluginConfig(this);
        }
    }
}
