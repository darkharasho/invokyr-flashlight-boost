using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace InvokyrFlashlightBoost
{
    /// <summary>
    /// The flashlight is a <c>FlashlightController</c> with a spot <see cref="Light"/> (<c>selfLight</c>). Every client
    /// builds its own, so the boost only changes what you see; nobody else needs the mod. Brightness: <c>SetLight</c>
    /// tweens the light to an intensity it's given each time, so that argument is scaled. Width and range: set every
    /// frame from the light's vanilla values, which are remembered per light, so nothing compounds.
    /// </summary>
    internal static class FlashlightPatches
    {
        private sealed class Vanilla
        {
            public float Angle, Range, Near;
            public float BoostedAngle, BoostedRange, BoostedNear;
        }

        private static readonly Dictionary<IntPtr, Vanilla> Lights = new Dictionary<IntPtr, Vanilla>();
        private static bool _loggedError;

        public static void Apply(Harmony harmony)
        {
            harmony.Patch(AccessTools.Method(typeof(FlashlightController), nameof(FlashlightController.SetLight)),
                prefix: new HarmonyMethod(typeof(FlashlightPatches), nameof(SetLightPrefix)));
            harmony.Patch(AccessTools.Method(typeof(FlashlightController), nameof(FlashlightController.LateUpdate)),
                postfix: new HarmonyMethod(typeof(FlashlightPatches), nameof(LateUpdatePostfix)));
        }

        private static void SetLightPrefix(bool __0, ref float __1)
        {
            if (__0)
                __1 *= Math.Max(0f, Plugin.Brightness.Value);
        }

        private static void LateUpdatePostfix(FlashlightController __instance)
        {
            try
            {
                var light = __instance?.selfLight;
                if (light == null)
                    return;
                var key = light.Pointer;
                if (!Lights.TryGetValue(key, out var v) || Beam.GameChanged(light.spotAngle, v.Angle, v.BoostedAngle) || Beam.GameChanged(light.range, v.Range, v.BoostedRange))
                {
                    if (Lights.Count > 64)
                        Lights.Clear(); // lights from earlier levels
                    v = new Vanilla { Angle = light.spotAngle, Range = light.range, Near = light.shadowNearPlane };
                    Lights[key] = v;
                }

                v.BoostedAngle = light.type == LightType.Spot ? Beam.Widen(v.Angle, Plugin.Diameter.Value) : v.Angle;
                v.BoostedRange = v.Range * Math.Max(0.1f, Plugin.Range.Value);
                v.BoostedNear = Beam.NearPlane(v.Near, Plugin.ShadowNearPlane.Value, v.BoostedRange);
                if (light.type == LightType.Spot)
                    light.spotAngle = v.BoostedAngle;
                light.range = v.BoostedRange;
                light.shadowNearPlane = v.BoostedNear;
            }
            catch (Exception e)
            {
                if (_loggedError)
                    return;
                _loggedError = true;
                Plugin.Log.LogError($"Boosting the flashlight failed: {e}");
            }
        }
    }
}
