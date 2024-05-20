namespace LTKConfiguration.Patches
{
    public class BeehivePatch
    {
        // Adds a point when beeswarm target finishes
        public static void BeehiveFixedUpdatePrefix(Beehive __instance)
        {
            if (__instance.followedCharacter != null)
            {
                if (__instance.followedCharacter.Success)
                {
                    LTKLib.LTKLibMod.GiveCustomPoint(LTKConfigurationMod.beesCustomPointId, __instance.followedCharacter.networkNumber);
                }
            }
        }
    }

}