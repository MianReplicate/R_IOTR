using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using LoveyDoveySexWithEuterpe;
using RimWorld;
using RomanceOnTheRim;
using Verse;

namespace R_IOTR.Mian;

[HarmonyPatch(typeof(Need_Romance))]
public class NeedRomancePatch
{
    [HarmonyPatch(nameof(Need_Romance.NeedInterval))]
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> IntimacyIsAlsoANeed(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        var foundCategory = false;
        var added = false;
        var label1 = generator.DefineLabel();
        var label2 = generator.DefineLabel();
        var returnLabel = generator.DefineLabel();
        var intimacyLocal = generator.DeclareLocal(typeof(Need_Intimacy));
        var isNullLocal = generator.DeclareLocal(typeof(bool));
        foreach (var instruction in instructions)
        {
            yield return instruction;

            if (instruction.Calls(AccessTools.Method(typeof(Need_Romance), "get_CurCategory")))
            {
                foundCategory = true;
            }

            if (foundCategory && !added && instruction.Branches(out var toExit))
            {
                added = true;

                // If the pawn has no intimacy need, this will completely skip past
                yield return CodeInstruction.LoadArgument(0);
                yield return CodeInstruction.LoadField(typeof(Need_Romance), "pawn");
                yield return CodeInstruction.Call(typeof(CommonChecks), nameof(CommonChecks.HasNoIntimacyTrait));
                yield return new CodeInstruction(OpCodes.Brtrue, returnLabel);

                // pawn.needs?.TryGetNeed<Need_Intimacy>()
                yield return CodeInstruction.LoadArgument(0);
                yield return CodeInstruction.LoadField(typeof(Need_Romance), "pawn");
                yield return CodeInstruction.LoadField(typeof(Pawn), nameof(Pawn.needs));
                yield return new CodeInstruction(OpCodes.Dup);
                yield return new CodeInstruction(OpCodes.Brtrue_S, label1);

                yield return new CodeInstruction(OpCodes.Pop);
                yield return new CodeInstruction(OpCodes.Ldnull);
                yield return new CodeInstruction(OpCodes.Br_S, label2);
                
                yield return CodeInstruction.Call(typeof(Pawn_NeedsTracker), nameof(Pawn_NeedsTracker.TryGetNeed), generics: new[]{typeof(Need_Intimacy)}).WithLabels(label1);
                
                yield return CodeInstruction.StoreLocal(intimacyLocal.LocalIndex).WithLabels(label2);
                
                yield return CodeInstruction.LoadLocal(intimacyLocal.LocalIndex);
                yield return new CodeInstruction(OpCodes.Ldnull);
                yield return new CodeInstruction(OpCodes.Cgt_Un);
                yield return CodeInstruction.StoreLocal(isNullLocal.LocalIndex);

                yield return CodeInstruction.LoadLocal(isNullLocal.LocalIndex);
                yield return new CodeInstruction(OpCodes.Brfalse_S, returnLabel);

                // R_IOTRSettings.Instance.intimacyInterval > pawn.needs?.TryGetNeed<Need_Intimacy>().CurLevel
                yield return new CodeInstruction(OpCodes.Nop);
                yield return CodeInstruction.LoadLocal(intimacyLocal.LocalIndex);
                yield return CodeInstruction.Call(typeof(Need), "get_CurLevel");
                yield return CodeInstruction.LoadField(typeof(R_IOTRSettings), nameof(R_IOTRSettings.Instance));
                yield return CodeInstruction.LoadField(typeof(R_IOTRSettings), nameof(R_IOTRSettings.intimacyInterval));
                yield return new CodeInstruction(OpCodes.Bgt_Un_S, toExit);
                
                yield return new CodeInstruction(OpCodes.Nop).WithLabels(returnLabel);
            }
        }
    }
}