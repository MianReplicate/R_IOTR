using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using LoveyDoveySexWithEuterpe;
using R_IOTR.Mian.Utilities;
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

                yield return CodeInstruction.LoadArgument(0);
                yield return CodeInstruction.LoadField(typeof(Need_Romance), "pawn");
                yield return CodeInstruction.Call(typeof(NeedRomancePatch), nameof(IsPawnInNeedOfRomance));
                yield return new CodeInstruction(OpCodes.Brtrue, toExit);
            }
        }
    }

    public static bool IsPawnInNeedOfRomance(Pawn pawn)
    {
        return pawn.HasIntimacyNeed() && R_IOTRSettings.Instance.intimacyInterval > pawn.needs?.TryGetNeed<Need_Intimacy>().CurLevel;
    }
}