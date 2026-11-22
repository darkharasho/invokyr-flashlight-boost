## Project Context

A BepInEx 6 (BepInEx.Unity.IL2CPP, be.755) mod for Invokyr (Unity 6000.3, IL2CPP): a wider, brighter, longer flashlight beam. Client-side only. Game code is in `vhs.dll`. Packaged Thunderstore-style under `darkharasho`, released on GitHub. Build setup is shared with `invokyr-admin-menu` (see its CLAUDE.md for refs, packaging and the flat zip layout HML needs).

## How it works (`FlashlightPatches.cs`)
- `FlashlightController` (global namespace) holds the spot light `selfLight`. `SetLight(bool isOn, float intensityAtEnd)` tweens intensity; the prefix scales `intensityAtEnd` when turning on (a fresh value each call, so no compounding). `Flicker` writes intensity itself and isn't patched.
- `LateUpdate` postfix: the light's vanilla spotAngle/range/shadowNearPlane are cached per light pointer and the boosted values recomputed from them each frame (`Beam.cs`, tested). If the game sets a value that's neither vanilla nor ours, it becomes the new vanilla.
- Width uses 2·atan(k·tan(a/2)) so the lit circle's diameter scales by k. The near plane stops held items (gun, guitar) from shadowing the beam.
- Unverified: `FlashlightController.lightSensePrefab` / `LightSightSense` may read the light's angle/range for monsters; if so a host with the mod could change that sense. Check in play.

## Building
`scripts/package.sh` → `dist/darkharasho-InvokyrFlashlightBoost-<version>.zip`; `hml install nexus-invokyr <zip> --profile Dev`. Tests: `dotnet test tests/InvokyrFlashlightBoost.Tests`. Run builds with `nice -n 19 taskset -c 0-3`. Release: bump `Plugin.cs`, csproj, manifest, CHANGELOG; push tag `v<version>`.
