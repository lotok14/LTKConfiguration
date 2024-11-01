using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using static UnityEngine.ParticleSystem.PlaybackState;
using LTKConfiguration.Patches;
using System.Reflection;
using BepInEx.Logging;
using UnityEngine;
using LTKConfiguration.Utils;

namespace LTKConfiguration
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class LTKConfigurationMod : BaseUnityPlugin
    {
        internal static ManualLogSource Log;

        public static ConfigEntry<float> StopwatchCustomSlowSpeed;
        public static ConfigEntry<float> StopwatchCustomSlowDuration;
        public static ConfigEntry<bool> StopwatchAlwaysRespawn;
        public static ConfigEntry<bool> CollapsingBlockRepair;
        public static ConfigEntry<float> CollapsingBlockRepairDelay;
        public static ConfigEntry<bool> JetpackUseFuel;
        public static ConfigEntry<float> JetpackFuelAmount;
        public static ConfigEntry<bool> BeehivePointsEnabled;
        public static ConfigEntry<float> BeehivePointsAmount;
        public static ConfigEntry<bool> BeehivePointsAlwaysAward;
        public static ConfigEntry<int> PuckBarType;
        public static ConfigEntry<Vector2> PuckBarPosition;
        public static ConfigEntry<float> PuckBarSize;
        public static ConfigEntry<bool> DoubleTeleporters;

        public static int beesCustomPointId;

        private void Awake()
        {
            LTKConfigurationMod.Log = base.Logger;

            // Config creation
            StopwatchCustomSlowSpeed = Config.Bind("Stopwatch", "Stopwatch speed", 0.5f, "The number the time gets multiplied by when you pick up a stopwatch");
            StopwatchCustomSlowDuration = Config.Bind("Stopwatch", "Stopwatch duration", 6f, "How long the stopwatch effect lasts in seconds");
            StopwatchAlwaysRespawn = Config.Bind("Stopwatch", "Always respawn", false, "Whether the stopwatch respawns after a round");
            CollapsingBlockRepair = Config.Bind("Collapsing Block", "Repair", false, "Whether the magnet platform repairs itself after a set time (true/false)");
            CollapsingBlockRepairDelay = Config.Bind("Collapsing Block", "Repair Delay", 7f, "How many seconds it takes for the magnet platform to repair in seconds (only works with Repair = true)");
            JetpackUseFuel = Config.Bind("Jetpack", "Use fuel", false, "Whether the jetpack can run out of fuel (true/false)");
            JetpackFuelAmount = Config.Bind("Jetpack", "Fuel Amount", 1f, "How long in seconds can you use the jetpack before it runs out (only works with Use fuel = true)");
            BeehivePointsEnabled = Config.Bind("Beehive", "Beehive Points Enabled", false, "Whether finishing with a beehive grants points (true/false)");
            BeehivePointsAmount = Config.Bind("Beehive", "Beehive Points Amount", 0.4f, "How many points finishing with a beehive grants (only works with Beehive Points Enabled = true)");
            BeehivePointsAlwaysAward = Config.Bind("Beehive", "Beehive Points Always Award", true, "Whether you always get points for finishing with a beehive (only works with Beehive Points Enabled = true)");
            PuckBarType = Config.Bind("Hockey", "Hockey Indicator Type", 0, new ConfigDescription("What puckbar is shown. 0 - none, 1 - icon, 2 - trajectory line", new AcceptableValueRange<int>(0, 2)));
            PuckBarSize = Config.Bind("Hockey", "Hockey Indicator Size", 1f, "The value the hockey indicator icon size gets multiplied by (it's about 215x215 pixels by default) (only works with Hockey Indicator Type = 1)");
            PuckBarPosition = Config.Bind("Hockey", "Hockey Indicator Position", new Vector2(120, 960), "The middle of the hockey indicator icon from the bottom left corner in pixels (only works with Hockey Indicator Type = 1)");
            DoubleTeleporters = Config.Bind("Teleport", "Double Teleporters", false, "Whether there should always be an even amount of teleporters to choose from");

            // Plugin startup logic
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            // load assets
            if (JetpackUseFuel.Value)
            {
                JetpackPatch.LoadFuelBarPrefab();
            }
            if (PuckBarType.Value == 1)
            {
                PuckBarController.LoadPuckBarPrefab();
            } 
            else if(PuckBarType.Value == 2)
            {
                LineDrawer.LoadLineDrawer();
            }
            PuckBarController.SetPuckBarType(PuckBarType.Value);

            // patch with harmony
            startPatching();
            Log.LogInfo($"{PluginInfo.PLUGIN_GUID} finished patching");
        }

        private void startPatching()
        {
            var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            MethodInfo original;
            MethodInfo patch;

            try
            {
                // =====================================<Stopwatch Patch>=====================================
                // (StopwatchPatch) Timekeeper.minScale() transpiler
                // make minScale multiply the timeScales instead of finding the smallest one
                original = AccessTools.Method(typeof(Timekeeper), "minScale");
                patch = AccessTools.Method(typeof(StopwatchPatch), "TimekeeperMinScaleTranspiler");
                patchMethod(harmony, original, patch, "transpiler");

                // (StopwatchPatch) Stopwatch.Place() postfix
                // sets the stopwatch's slowSpeed and slowDuration to the values from the config
                Type[] parameters = { typeof(int), typeof(bool), typeof(bool) };
                original = AccessTools.Method(typeof(Stopwatch), "Place", parameters);
                patch = AccessTools.Method(typeof(StopwatchPatch), "StopwatchPlacePostfix");
                patchMethod(harmony, original, patch, "postfix");

                // (StopwatchPatch) Stopwatch.get_AlwaysRespawn() prefix
                // makes the stopwatch always respawn if true in the config
                if (LTKConfigurationMod.StopwatchAlwaysRespawn.Value)
                {
                    original = AccessTools.Method(typeof(Stopwatch), "get_AlwaysRespawn");
                    patch = AccessTools.Method(typeof(StopwatchPatch), "StopwatchAlwaysRespawnPrefix");
                    patchMethod(harmony, original, patch, "prefix");
                }
                else
                {
                    Log.LogWarning("(StopwatchPatch) not patched because it was disabled in the config");
                }
                // =====================================</Stopwatch Patch>=====================================
                
                // =====================================<Collapsing Block Patch>=====================================
                // (CollapsingBlockPatch) CollapsingBlock.FixedUpdate() postfix
                // makes the collapsing block repair after a delay if true in config
                if (LTKConfigurationMod.CollapsingBlockRepair.Value)
                {
                    original = AccessTools.Method(typeof(CollapsingBlock), "FixedUpdate");
                    patch = AccessTools.Method(typeof(CollapsingBlockPatch), "CollapsingBlockFixedUpdatePostfix");
                    patchMethod(harmony, original, patch, "postfix");
                }
                else
                {
                    Log.LogWarning("(CollapsingBlockPatch) not patched because it was disabled in the config");
                }
                // =====================================</Collapsing Block Patch>=====================================

                // =====================================<Jetpack Patch>=====================================
                if (LTKConfigurationMod.JetpackUseFuel.Value)
                {
                    // (JetpackPatch) Jetpack.Pickup() postfix
                    // makes the fuel bar reset when jetpack is picked up
                    original = AccessTools.Method(typeof(Jetpack), "Pickup");
                    patch = AccessTools.Method(typeof(JetpackPatch), "JetpackPickupPostfix");
                    patchMethod(harmony, original, patch, "postfix");

                    // (JetpackPatch) Character.FullUpdate() postfix
                    // makes the character lose fuel and removes the jetpack when fuel <= 0
                    original = AccessTools.Method(typeof(Character), "fullUpdate");
                    patch = AccessTools.Method(typeof(JetpackPatch), "CharacterFullUpdatePostfix");
                    patchMethod(harmony, original, patch, "postfix");

                    // ======================================<Jetpack Grab>======================================
                    // This section makes it so that you can grab a jetpack even if you have one already

                    // (JetpackPatch) Character.RpcGrantJetpack() prefix
                    original = AccessTools.Method(typeof(Character), "RpcGrantJetpack");
                    patch = AccessTools.Method(typeof(JetpackPatch), "CharacterRpcGrantJetpackPrefix");
                    patchMethod(harmony, original, patch, "prefix");

                    // (JetpackPatch) Character.TryPickUpJetpack() transpiler
                    original = AccessTools.Method(typeof(Character), "TryPickUpJetpack");
                    patch = AccessTools.Method(typeof(JetpackPatch), "CharacterTryPickUpJetpackTranspiler");
                    patchMethod(harmony, original, patch, "transpiler");

                    // (JetpackPatch) Jetpack.OnTriggerEnter2D() transpiler
                    original = AccessTools.Method(typeof(Jetpack), "OnTriggerEnter2D");
                    patch = AccessTools.Method(typeof(JetpackPatch), "JetpackOnTriggerEnter2DTranspiler");
                    patchMethod(harmony, original, patch, "transpiler");

                    // ======================================</Jetpack Grab>======================================
                }
                else
                {
                    Log.LogWarning("(JetpackPatch) not patched because it was disabled in the config");
                }
                // =====================================</Jetpack Patch>=====================================

                // =====================================<Beehive Patch>=====================================
                if (LTKConfigurationMod.BeehivePointsEnabled.Value)
                {
                    LTKConfigurationMod.beesCustomPointId = CustomPointController.CreateCustomPoint("Bees", LTKConfigurationMod.BeehivePointsAmount.Value, new Color(1, 1, 0), LTKConfigurationMod.BeehivePointsAlwaysAward.Value);            
                    // (BeehivePatch) Beehive.FixedUpdate() Prefix
                    // Adds a point when beeswarm target finishes
                    original = AccessTools.Method(typeof(Beehive), "FixedUpdate");
                    patch = AccessTools.Method(typeof(BeehivePatch), "BeehiveFixedUpdatePrefix");
                    patchMethod(harmony, original, patch, "prefix");

                    // =====================================<CustomPointPatch>=====================================
                    // (CustomPointPatch) GraphScoreBoard.GetPreinstantiatedPointBlock() postfix
                    // change the pointblock when it has custom id
                    original = AccessTools.Method(typeof(GraphScoreBoard), "GetPreinstantiatedPointBlock");
                    patch = AccessTools.Method(typeof(CustomPointPatch), "GraphScoreBoardGetPreinstantiatedPointBlockPostfix");
                    patchMethod(harmony, original, patch, "postfix");

                    // (CustomPointPatch) PointBlock.get_AlwaysAward Prefix
                    // return always award from data when it has custom id
                    original = AccessTools.Method(typeof(PointBlock), "get_AlwaysAward");
                    patch = AccessTools.Method(typeof(CustomPointPatch), "PointBlockget_AlwaysAwardPrefix");
                    patchMethod(harmony, original, patch, "prefix");
                    // =======================================</CustomPointPatch>==================================
                }
                else
                {
                    Log.LogWarning("(BeehivePatch) not patched because it was disabled in the config");
                    Log.LogWarning("(CustomPointPatch) not patched because it was disabled in the config");
                }
                // =====================================</Beehive Patch>=====================================

                // =====================================<Hockey Patch>=======================================
                if (LTKConfigurationMod.PuckBarType.Value != 0)
                {
                    // (HockeyPatch) HockeyShooter.Activate() Transpiler
                    // makes the hockey shooter shoot slower if modifier for that is lower than 1
                    original = AccessTools.Method(typeof(HockeyShooter), "Activate");
                    patch = AccessTools.Method(typeof(HockeyPatch), "HockeyShooterActivateTranspiler");
                    patchMethod(harmony, original, patch, "transpiler");

                    // (HockeyPatch) HockeyShooter.warningSound() Postfix
                    // sets the fill of the puckbar to the time until next shot
                    original = AccessTools.Method(typeof(HockeyShooter), "warningSound");
                    patch = AccessTools.Method(typeof(HockeyPatch), "HockeyShooterWarningSoundPostfix");
                    patchMethod(harmony, original, patch, "postfix");

                    if(LTKConfigurationMod.PuckBarType.Value == 2)
                    {
                        // (HockeyPatch) HockeyShooter.Start() Postfix
                        // sets the layerMask of the puckbar raycast
                        original = AccessTools.Method(typeof(HockeyShooter), "Start");
                        patch = AccessTools.Method(typeof(HockeyPatch), "HockeyShooterStartPostfix");
                        patchMethod(harmony, original, patch, "postfix");
                    }
                }
                else
                {
                    Log.LogWarning("(HockeyPatch) not patched because it was disabled in the config");
                }
                // =====================================</Hockey Patch>=======================================

                // =======================================<TeleportPatch>======================================
                if (LTKConfigurationMod.DoubleTeleporters.Value)
                {
                    // (TeleportPatch) PartyBox.ChoosePieces() Transpiler
                    // always spawn teleports in pairs
                    original = AccessTools.Method(typeof(PartyBox), "ChoosePieces");
                    patch = AccessTools.Method(typeof(TeleportPatch), "PartyBoxChoosePiecesTranspiler");
                    patchMethod(harmony, original, patch, "transpiler");
                }
                else
                {
                    Log.LogWarning("(TeleportPatch) not patched because it was disabled in the config");
                }
                // =======================================</TeleportPatch>======================================
            }
            catch (Exception e)
            {
                Log.LogError($"stopped patching because of error:\n{e}");
            }
        }

        void Update()
        {
            PuckBarController.Update();
        }

        // patch a method with a specified patch
        private void patchMethod(Harmony harmony, MethodInfo original, MethodInfo patch, string patchType)
        {
            string patchName = $"({patch.DeclaringType.Name}) {original.DeclaringType}.{original.Name}() {patchType}";
            try
            {
                switch (patchType)
                {
                    case "prefix":
                        harmony.Patch(original, prefix: new HarmonyMethod(patch));
                        break;
                    case "postfix":
                        harmony.Patch(original, postfix: new HarmonyMethod(patch));
                        break;
                    case "transpiler":
                        harmony.Patch(original, transpiler: new HarmonyMethod(patch));
                        break;
                    default:
                        throw new Exception($"no patch of type {patchType} exists");
                }
                Log.LogInfo($"{patchName} patched successfully");
            }
            catch (Exception e)
            {
                Log.LogError($"{patchName} not patched because of error:\n{e}");
            }
        }
    }
}
