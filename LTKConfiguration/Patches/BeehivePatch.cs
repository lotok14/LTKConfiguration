using UnityEngine;
using LTKConfiguration.Extensions;

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
                    PointBlock pb = new PointBlock(PointBlock.pointBlockType.coin, __instance.followedCharacter.networkNumber);
                    pb.GetAdditionalData().pointBlockCustomId = 0;
                    ScoreKeeper.Instance.AwardPoint(pb, true);
                }
            }
        }

        // changes the look of the point block when it's bees
        public static void GraphScoreBoardGetPreinstantiatedPointBlockPostfix(PointBlock pb, ref ScorePiece __result)
        {
            LTKConfigurationMod.Log.LogError($"{pb.GetAdditionalData().pointBlockCustomId}  {__result.pieceImage}");
            if (pb.GetAdditionalData().pointBlockCustomId == 0 && __result.pieceImage != null)
            {
                __result.pieceImage.color = new Color(255 / 255f, 255 / 255f, 0 / 255f);
                __result.text.color = __result.pieceImage.color;
                __result.text.text = "bees";
                __result.width *= LTKConfigurationMod.BeehivePointsAmount.Value;
            }
        }
    }

}