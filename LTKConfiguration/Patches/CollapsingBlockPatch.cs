using UnityEngine;
using UnityEngine.Assertions;

namespace LTKConfiguration.Patches
{
    public class CollapsingBlockPatch
    {
        // Repairs the collapsing block when enough time passes
        public static void CollapsingBlockFixedUpdatePostfix(CollapsingBlock __instance)
        {
            if (__instance.collideTime >= __instance.Delay + LTKConfigurationMod.CollapsingBlockRepairDelay.Value)
            {
                __instance.Reset();
            }
        }
    }
}
