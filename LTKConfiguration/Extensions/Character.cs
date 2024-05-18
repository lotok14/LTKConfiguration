using LTKConfiguration.Utils;
using System.Runtime.CompilerServices;
using System;

namespace LTKConfiguration.Extensions
{
    [Serializable]
    public class CharacterAdditionalData
    {
        public JetpackFuelBar jetpackFuelBar;

        public CharacterAdditionalData()
        {
            jetpackFuelBar = new JetpackFuelBar();
        }
    }

    public static class CharacterExtension
    {
        public static readonly ConditionalWeakTable<Character, CharacterAdditionalData> data = new ConditionalWeakTable<Character, CharacterAdditionalData>();

        public static CharacterAdditionalData GetAdditionalData(this Character character)
        {
            return data.GetOrCreateValue(character);
        }

        public static void AddData(this Character character, CharacterAdditionalData value)
        {
            try
            {
                data.Add(character, value);
            }
            catch (Exception) { }
        }
    }
}