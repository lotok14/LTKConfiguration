using LTKConfiguration.Extensions;
using LTKConfiguration.Utils;
using UnityEngine;

namespace LTKConfiguration.Patches
{
    public class CustomPointPatch
    {
        // changes the look of the point block by id
        public static void GraphScoreBoardGetPreinstantiatedPointBlockPostfix(PointBlock pb, ref ScorePiece __result)
        {
            int pbId = pb.GetAdditionalData().pointBlockCustomId;
            if (pbId >= 0 && __result.pieceImage != null)
            {
                CustomPoint customPointData = CustomPointController.customPointsData[pbId];
                LTKConfigurationMod.Log.LogInfo($"Granting custom point block: {pbId} {customPointData.name}");
                __result.pieceImage.color = customPointData.color;
                __result.text.color = __result.pieceImage.color;
                __result.text.text = customPointData.name;
                __result.width = customPointData.width * 50;
            }
        }

        public static bool PointBlockget_AlwaysAwardPrefix(ref bool __result, PointBlock __instance)
        {
            int pbId = __instance.GetAdditionalData().pointBlockCustomId;
            if (pbId >= 0)
            {
                __result = CustomPointController.customPointsData[pbId].alwaysAward;
                return false;
            }
            return true;
        }
    }

}