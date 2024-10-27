using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using System.Reflection.Emit;
using LTKConfiguration.Utils;
using UnityEngine;
using System;

namespace LTKConfiguration.Patches
{
    public class TeleportPatch
    {
        // spawns double the teleports
        public static IEnumerable<CodeInstruction> PartyBoxChoosePiecesTranspiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            (int startIndex, int endIndex) = TranspilerHelper.FindSegmentByOperand(instructions, new OpCode[] { OpCodes.Blt, OpCodes.Stloc_0 }, "Void .ctor()");
            var codes = new List<CodeInstruction>(instructions);
            if (startIndex > -1 && endIndex > -1)
            {
                LocalBuilder localVariable1 = generator.DeclareLocal(typeof(int));
                Label label1 = generator.DefineLabel();
                Label label2 = generator.DefineLabel();
                Label label3 = generator.DefineLabel();

                List<CodeInstruction> codeToInsert = new() {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PartyBox), "pieces")),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(List<PickableBlock>), "get_Count")),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Sub),
                    new CodeInstruction(OpCodes.Stloc_S, localVariable1),
                    new CodeInstruction(OpCodes.Br_S, label1),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PartyBox), "pieces")),
                    new CodeInstruction(OpCodes.Ldloc_S, localVariable1),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(List<PickableBlock>), "get_Item", new Type[]{ typeof(int) })),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(UnityEngine.Object), "get_name")),
                    new CodeInstruction(OpCodes.Ldstr, "95_Teleporter_Pick(Clone)"),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(System.String), "op_Equality", new Type[] { typeof(string), typeof(string) })),
                    new CodeInstruction(OpCodes.Brfalse_S, label2),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PartyBox), "pieces")),
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(PartyBox), "pieces")),
                    new CodeInstruction(OpCodes.Ldloc_S, localVariable1),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(List<PickableBlock>), "get_Item", new Type[]{ typeof(int) })),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(UnityEngine.Object), "Instantiate", new Type[]{ typeof(UnityEngine.Object)})),
                    new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(typeof(List<PickableBlock>), "Add")),
                    new CodeInstruction(OpCodes.Ldloc_S, localVariable1),
                    new CodeInstruction(OpCodes.Ldc_I4_1),
                    new CodeInstruction(OpCodes.Sub),
                    new CodeInstruction(OpCodes.Stloc_S, localVariable1),
                    new CodeInstruction(OpCodes.Ldloc_S, localVariable1),
                    new CodeInstruction(OpCodes.Ldc_I4_0),
                    new CodeInstruction(OpCodes.Bge_S, label3)
                };
                codes.InsertRange(startIndex + 1, codeToInsert);

                // adding labels
                for (int i = 1; i < codes.Count - 5; i++)
                {
                    // label1
                    if (codes[i].opcode == OpCodes.Ldloc_S && codes[i].operand == localVariable1 && codes[i-1].opcode == OpCodes.Stloc_S && codes[i-1].operand == localVariable1)
                    {
                        codes[i].labels.Add(label1);
                    }
                    // label2
                    if (codes[i].opcode == OpCodes.Ldloc_S && codes[i].operand == localVariable1 && codes[i + 2].opcode == OpCodes.Sub)
                    {
                        codes[i].labels.Add(label2);
                    }
                    // label3
                    if (codes[i].opcode == OpCodes.Ldarg_0 && codes[i + 5].opcode == OpCodes.Ldstr && (string)codes[i + 5].operand == "95_Teleporter_Pick(Clone)")
                    {
                        codes[i].labels.Add(label3);
                    }
                }
            }
            else
            {
                LTKConfigurationMod.Log.LogWarning("PartyBoxChoosePiecesTranspiler() didn't find the string");
            }

            return codes.AsEnumerable();
        }
    }
}

// Adds
/*
for (int localVariable1 = this.pieces.Count - 1; localVariable1 >= 0; localVariable1--)
{
	if (this.pieces[localVariable1].name == "95_Teleporter_Pick(Clone)")
	{
		this.pieces.Add(global::UnityEngine.Object.Instantiate<PickableBlock>(this.pieces[localVariable1]));
	}
} 
*/
// after the blocks have been selected