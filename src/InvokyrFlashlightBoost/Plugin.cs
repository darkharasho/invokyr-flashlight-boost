using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace InvokyrFlashlightBoost
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BasePlugin
    {
        public const string PluginGuid = "com.darkharasho.invokyr.flashlightboost";
        public const string PluginName = "InvokyrFlashlightBoost";
        public const string PluginVersion = "1.0.1";

        internal static new ManualLogSource Log;
        internal static ConfigEntry<float> Diameter;
        internal static ConfigEntry<float> Brightness;
        internal static ConfigEntry<float> Range;
        internal static ConfigEntry<float> ShadowNearPlane;

        public override void Load()
        {
            Log = base.Log;
            Diameter = Config.Bind("Beam", "DiameterMultiplier", 2f, new ConfigDescription(
                "How much wider the lit circle is (1 = vanilla). 2 makes the 75° cone about 113°.",
                new AcceptableValueRange<float>(0.5f, 4f), new ConfigurationManagerAttributes { DispName = "Width", Order = 3 }));
            Brightness = Config.Bind("Beam", "BrightnessMultiplier", 2f, new ConfigDescription(
                "How much brighter the flashlight is (1 = vanilla). Applies the next time it's switched on.",
                new AcceptableValueRange<float>(0.25f, 6f), new ConfigurationManagerAttributes { DispName = "Brightness", Order = 2 }));
            Range = Config.Bind("Beam", "RangeMultiplier", 1f, new ConfigDescription(
                "How much farther it reaches (1 = vanilla, about 12 m).",
                new AcceptableValueRange<float>(0.5f, 4f), new ConfigurationManagerAttributes { DispName = "Range", Order = 1 }));
            ShadowNearPlane = Config.Bind("Beam", "ShadowNearPlane", 1.5f, new ConfigDescription(
                "Metres in front of the light before anything casts a shadow. Keeps the item in your hands from throwing a dark shape across the beam. 0 = vanilla.",
                new AcceptableValueRange<float>(0f, 4f), new ConfigurationManagerAttributes { DispName = "Shadow near plane", Order = 0 }));

            FlashlightPatches.Apply(new Harmony(PluginGuid));
            Log.LogInfo($"{PluginName} {PluginVersion} loaded");
        }
    }
}
