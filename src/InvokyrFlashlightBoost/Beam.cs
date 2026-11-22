using System;

namespace InvokyrFlashlightBoost
{
    /// <summary>Beam arithmetic, kept apart from the game so it can be tested.</summary>
    internal static class Beam
    {
        /// <summary>
        /// The spot angle whose lit circle is <paramref name="diameter"/> times as wide as <paramref name="angle"/>'s at
        /// any distance: 2·atan(k·tan(a/2)). Scaling the angle itself would overshoot (2 × 75° is past 140°).
        /// </summary>
        public static float Widen(float angle, float diameter)
        {
            if (diameter <= 0f || angle <= 0f)
                return angle;
            var half = angle * Math.PI / 360.0;
            var widened = 2.0 * Math.Atan(diameter * Math.Tan(half)) * 180.0 / Math.PI;
            return (float)Math.Min(widened, 170.0);
        }

        /// <summary>The shadow near plane: pushed out past the held item, never beyond most of the beam.</summary>
        public static float NearPlane(float vanilla, float wanted, float range)
        {
            if (wanted <= vanilla)
                return vanilla;
            return Math.Max(vanilla, Math.Min(wanted, range * 0.85f));
        }

        /// <summary>True if <paramref name="current"/> is neither the value we found nor the one we set: the game changed it.</summary>
        public static bool GameChanged(float current, float vanilla, float boosted) =>
            Math.Abs(current - vanilla) > 0.001f && Math.Abs(current - boosted) > 0.001f;
    }
}
