using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace LoveMachine.Experiments
{
    public static class Config
    {
        public static ManualLogSource Logger { get; set; }
        public static string PluginDirectoryPath { get; set; }

        public static ConfigEntry<bool> EnableCalorDepthControl { get; private set; }
        public static ConfigEntry<bool> EnableHotdogDepthControl { get; private set; }
        public static ConfigEntry<string> HotdogServerAddress { get; private set; }

        public static void Initialize(BaseUnityPlugin plugin)
        {
            string experimentalTitle = "Experimental Features";
            EnableCalorDepthControl = plugin.Config.Bind(
                section: experimentalTitle,
                key: "Enable Lovense Calor depth control",
                defaultValue: true,
                "Use a Lovense Calor device for depth control");
            EnableHotdogDepthControl = plugin.Config.Bind(
                section: experimentalTitle,
                key: "Enable Hotdog depth control",
                defaultValue: true,
                "Use a Hotdog device for depth control");
            HotdogServerAddress = plugin.Config.Bind(
                section: experimentalTitle,
                key: "Hotdog server address",
                defaultValue: "ws://localhost:5365",
                "The address of the Hotdog server");
        }
    }
}