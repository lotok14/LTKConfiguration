using System.Runtime.CompilerServices;
using BepInEx;
using GameEvent;
using HarmonyLib;
using LTKConfiguration.Extensions;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using System;
using LTKConfiguration.Utils;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;

namespace LTKConfiguration.Patches
{
    public class HockeyPatch
    {
        // sets the raycast interact layers
        public static void HockeyShooterStartPostfix(HockeyShooter __instance)
        {
            PuckBarController.SetPuckInteractLayers(__instance.puckInteractsLayers);
        }

        // starts the filling process
        public static void HockeyShooterWarningSoundPostfix()
        {
            PuckBarController.StartFilling();
        }

        // makes the hockey shooter shoot slower when rateOfFire is less than 0
        public static IEnumerable<CodeInstruction> HockeyShooterActivateTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            (int startIndex, int endIndex) = TranspilerHelper.FindSegmentByOperand(instructions, new OpCode[] { OpCodes.Ldloc_0, OpCodes.Ble_Un }, "1");
            var codes = new List<CodeInstruction>(instructions);

            if (startIndex > -1 && endIndex > -1)
            {
                // remove && rateOfFire > 1f
                codes.RemoveRange(startIndex, endIndex - startIndex + 1);
            }
            else
            {
                LTKConfigurationMod.Log.LogWarning("HockeyShooterActivateTranspiler() didn't find the string");
            }

            return codes.AsEnumerable();
        }
    }
}