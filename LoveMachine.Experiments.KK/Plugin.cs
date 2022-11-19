using BepInEx;
using KKAPI.MainGame;
using System.IO;

namespace LoveMachine.Experiments
{
    [BepInProcess("Koikatu")]
    [BepInProcess("KoikatuVR")]
    [BepInProcess("Koikatsu Party")]
    [BepInProcess("Koikatsu Party VR")]
    [BepInProcess("KoikatsuSunshine")]
    [BepInProcess("KoikatsuSunshine_VR")]
    [BepInPlugin(guid, pluginName, VersionInfo.Version)]
    internal class Plugin : BaseUnityPlugin
    {
        private const string guid = "Sauceke.LoveMachine.Experiments";
        private const string pluginName = "LoveMachine Experiments";

        private void Start()
        {
            GameAPI.RegisterExtraBehaviour<KoikatsuCalorDepthController>(guid);
            GameAPI.RegisterExtraBehaviour<KoikatsuHotdogDepthController>(guid);
            Experiments.Config.Initialize(this);
            Experiments.Config.Logger = Logger;
            Experiments.Config.PluginDirectoryPath = Path.GetDirectoryName(Info.Location)
                .TrimEnd(Path.DirectorySeparatorChar)
                + Path.DirectorySeparatorChar;
        }
    }
}