using HarmonyLib;
using BepInEx;
using UnityEngine;
using System.Collections.Generic;
using LTKConfiguration.Utils;
using LTKConfiguration.Extensions;
using System.Reflection.Emit;
using System.Linq;
using LTKLib;

namespace LTKConfiguration.Patches
{
    public class JetpackPatch
    {
        public static GameObject fuelBarPrefab;
        // loads the fuelBar prefab
        public static void LoadFuelBarPrefab()
        {
            AssetBundle fuelBarAssetBundle = AssetBundle.LoadFromFile(Paths.PluginPath + "/LTKConfiguration/Assets/FuelBar/fuelbar.assetBundle");
            UnityEngine.Object[] fuelBarAssets = fuelBarAssetBundle.LoadAllAssets();
            foreach (UnityEngine.Object asset in fuelBarAssets)
            {
                if (asset.name == "fuelBarCanvas")
                {
                    JetpackPatch.fuelBarPrefab = asset as GameObject;
                }
            }
        }

        // sets the character's fuel amount to max on jetpack pickup
        public static void JetpackPickupPostfix(Character chr)
        {
            chr.GetAdditionalData().jetpackFuelBar.setParentCharacter(chr);
            chr.GetAdditionalData().jetpackFuelBar.resetFuelAmount(LTKConfigurationMod.JetpackFuelAmount.Value);
        }

        // remove the character's fuel amount when using it
        public static void CharacterFullUpdatePostfix(Character __instance)
        {
            bool pickedUpJetpack = (bool)AccessTools.Field(typeof(Character), "pickedUpJetpack").GetValue(__instance);
            bool jump = (bool)AccessTools.Field(typeof(Character), "jump").GetValue(__instance);
            bool finishing = (bool)AccessTools.Field(typeof(Character), "succeeding").GetValue(__instance);
            if (pickedUpJetpack)
            {
                if (finishing)
                {
                    __instance.GetAdditionalData().jetpackFuelBar.setFuel(0);
                    Debug.Log("finished with jetpack");
                }
                else if(jump)
                {
                    __instance.GetAdditionalData().jetpackFuelBar.useFuel(Time.fixedDeltaTime);
                    
                }

                float fuelLeftPercentage = __instance.GetAdditionalData().jetpackFuelBar.fuelLeftPercentage();
                if (fuelLeftPercentage <= 0)
                {
                    __instance.SetJetpackPickedUp(false);
                    Debug.Log($"jetpack fuel ran out for player {__instance.networkNumber}");
                }
            }
            else if(__instance.GetAdditionalData().jetpackFuelBar.fuelLeftPercentage() > 0)
            {
                __instance.GetAdditionalData().jetpackFuelBar.setFuel(0);
                Debug.Log($"Jetpack fuel bar removed for player {__instance.networkNumber}");
            }
        }

        // ======================================START======================================
        // This section makes it so that you can grab a jetpack even if you have one already
        // ======================================START======================================
        public static void CharacterRpcGrantJetpackPrefix(Character __instance)
        {
            AccessTools.Field(typeof(Character), "pickedUpJetpack").SetValue(__instance, false);
        }

        public static IEnumerable<CodeInstruction> CharacterTryPickUpJetpackTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            (int startIndex, int endIndex) = TranspilerHelper.FindSegmentByOperand(instructions, OpCodes.Ldarg_0, "System.Boolean jetpackTouched"); 
            var codes = new List<CodeInstruction>(instructions);

            if (startIndex > -1 && endIndex > -1)
            {
                // remove if (!this.jetpackTouched)
                codes.RemoveRange(startIndex, endIndex - startIndex);
            }
            else
            {
                LTKConfigurationMod.Log.LogWarning("CharacterTryPickUpJetpackTranspiler() didn't find the string");
            }

            return codes.AsEnumerable();
        }

        public static IEnumerable<CodeInstruction> JetpackOnTriggerEnter2DTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            (int startIndex, int endIndex) = TranspilerHelper.FindSegmentByOperand(instructions, new OpCode[] {OpCodes.Brtrue, OpCodes.Brfalse}, "Boolean get_HasJetpack()");
            var codes = new List<CodeInstruction>(instructions);

            if (startIndex > -1 && endIndex > -1)
            {
                // remove || componentInParent.HasJetpack
                codes.RemoveRange(startIndex, endIndex - startIndex);
            }
            else
            {
                LTKConfigurationMod.Log.LogWarning("JetpackOnTriggerEnter2DTranspiler() didn't find the string");
            }

            return codes.AsEnumerable();
        }
        // =======================================END=======================================
        // This section makes it so that you can grab a jetpack even if you have one already
        // =======================================END=======================================
    }

}
