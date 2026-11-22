# Invokyr Flashlight Boost

A wider, brighter flashlight, optionally with more reach.

## Settings
In `BepInEx/config/com.darkharasho.invokyr.flashlightboost.cfg`, under `[Beam]`:
- `DiameterMultiplier` (default 2): how much wider the lit circle is. The cone is widened so the circle doubles at every distance (75° becomes about 114°) rather than doubling the angle.
- `BrightnessMultiplier` (default 2): how much brighter. It takes effect the next time the flashlight is switched on.
- `RangeMultiplier` (default 1): how much farther it reaches (vanilla is about 12 m).
- `ShadowNearPlane` (default 1.5 m): nothing closer than this casts a shadow from the flashlight, so the gun or guitar in your hands doesn't throw a dark shape across the beam. 0 leaves it as the game has it.

## Notes
- Client-side: it changes the light your game draws. Other players see their own flashlights, and nobody else needs the mod.
- Needs BepInEx 6 for IL2CPP (be.755 or newer).

## Building
- `scripts/package.sh` builds the DLL and the package zip in `dist/` (against `~/.cache/invokyr`, or `lib/refs` in CI; `scripts/update-refs.sh` refreshes both from the game).
- Install a build with HML: `hml install nexus-invokyr dist/darkharasho-InvokyrFlashlightBoost-<version>.zip --profile Dev`.
- Release: bump the version in `Plugin.cs`, the csproj, `thunderstore/manifest.json` and `CHANGELOG.md`, then push tag `v<version>`.
