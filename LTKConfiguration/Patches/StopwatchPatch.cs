using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection.Emit;
using LTKConfiguration.Utils;
using UnityEngine;

namespace LTKConfiguration.Patches
{
    public class StopwatchPatch
    {
        // makes Timekeeper.MinScale multiply the time values instead of choosing the smallest one
        public static IEnumerable<CodeInstruction> TimekeeperMinScaleTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            (int startIndex, int endIndex) = TranspilerHelper.FindSegmentByOperand(instructions, OpCodes.Ldloc_2, "System.Single scale");
            var codes = new List<CodeInstruction>(instructions);
            
            if (startIndex > -1 && endIndex > -1)
            {
                // remove && timeSource.scale < num
                codes.RemoveRange(startIndex, endIndex - startIndex);

                // *= instead of =
                codes.Insert(startIndex, new CodeInstruction(OpCodes.Ldloc_0));
                codes.Insert(startIndex + 3, new CodeInstruction(OpCodes.Mul));
            }
            else
            {
                LTKConfigurationMod.Log.LogWarning("TimekeeperMinScaleTranspiler() didn't find the string");
            }

            return codes.AsEnumerable();
        }

        // changes the slow speed and slow duration to the one in the config
        public static void StopwatchPlacePostfix(Stopwatch __instance)
        {
            __instance.SlowSpeed = LTKConfigurationMod.StopwatchCustomSlowSpeed.Value;
            __instance.SlowDuration = LTKConfigurationMod.StopwatchCustomSlowDuration.Value;
        }

        public static bool StopwatchAlwaysRespawnPrefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
