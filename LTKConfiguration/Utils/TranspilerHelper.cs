using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Linq;
using UnityEngine;

namespace LTKConfiguration.Utils
{
    public class TranspilerHelper
    {
        /// <summary>
        /// Method <c>FindSegmentByOperand</c> Splits IL code into segments based on dividers and looks for searchedOperand.
        /// <example>
        /// For example:
        /// <code>
        /// "FindSegmentByOperand(instructions, new OpCode[] {OpCodes.ldarg_0, OpCodes.ldarg_1}, "System.Single scale");
        /// </code>
        /// </example>
        /// </summary>
        /// /// <returns>
        /// returns a tuple containing startIndex and endIndex of the segment containing the searchedOperand
        /// </returns>
        public static (int startIndex, int endIndex) FindSegmentByOperand(IEnumerable<CodeInstruction> instructions, OpCode[] dividers, string searchedOperand)
        {
            var foundLine = false;
            var startIndex = -1;
            var endIndex = -1;

            var codes = new List<CodeInstruction>(instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (dividers.Contains(codes[i].opcode))
                {
                    if (foundLine)
                    {
                        endIndex = i; // segment end index
                        break;
                    }
                    else
                    {
                        startIndex = i; // exclude startOpCode

                        for (var j = startIndex + 1; j < codes.Count; j++)
                        {
                            if (dividers.Contains(codes[j].opcode))
                                break;
                            var strOperand = $"{codes[j].operand}"; // cast to string
                            if (strOperand == searchedOperand) // for example "System.Single scale"
                            {
                                foundLine = true;
                            }
                            //LTKConfigurationMod.Log.LogWarning($"{j}\t{codes[j].opcode}\t{codes[j].operand}\t{foundLine}");
                        }
                    }
                }
            }

            return (startIndex, endIndex);
        }

        /// <summary>
        /// Method <c>FindSegmentByOperand</c> Splits IL code into segments based on divider and looks for searchedOperand.
        /// <example>
        /// For example:
        /// <code>
        /// "FindSegmentByOperand(instructions, OpCodes.ldarg_0, "System.Single scale");
        /// </code>
        /// </example>
        /// </summary>
        /// /// <returns>
        /// returns a tuple containing startIndex and endIndex of the segment containing the searchedOperand
        /// </returns>
        public static (int startIndex, int endIndex) FindSegmentByOperand(IEnumerable<CodeInstruction> instructions, OpCode divider, string searchedOperand)
        {
            return FindSegmentByOperand(instructions, new OpCode[] {divider}, searchedOperand);
        }

        /// <summary>
        /// Method <c>DebugIlCode</c> Writes out every line from a list of CodeInstruction objects.
        /// </summary>
        public static void DebugILCode(List<CodeInstruction> codes, string debugMessage = "IL DEBUGGING")
        {
            LTKConfigurationMod.Log.LogInfo($"============== START OF {debugMessage} ==============");
            for (var i = 0; i < codes.Count; i++)
            {
                LTKConfigurationMod.Log.LogInfo($"{i}\t{codes[i].opcode}\t{codes[i].operand}");
            }
            LTKConfigurationMod.Log.LogInfo($"============== END OF {debugMessage} ==============");
        }
    }
}