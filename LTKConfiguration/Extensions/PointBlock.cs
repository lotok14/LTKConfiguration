using LTKConfiguration.Utils;
using System.Runtime.CompilerServices;
using System;

namespace LTKConfiguration.Extensions
{
    [Serializable]
    public class PointBlockAdditionalData
    {
        public int pointBlockCustomId = -1;
    }

    public static class PointBlockExtension
    {
        public static readonly ConditionalWeakTable<PointBlock, PointBlockAdditionalData> data = new ConditionalWeakTable<PointBlock, PointBlockAdditionalData>();

        public static PointBlockAdditionalData GetAdditionalData(this PointBlock pointBlock)
        {
            return data.GetOrCreateValue(pointBlock);
        }

        public static void AddData(this PointBlock pointBlock, PointBlockAdditionalData value)
        {
            try
            {
                data.Add(pointBlock, value);
            }
            catch (Exception) { }
        }
    }
}