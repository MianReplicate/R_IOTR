using HarmonyLib;
using LoveyDoveySexWithEuterpe;
using R_IOTR.Mian.Utilities;
using RimWorld;
using Verse;

namespace R_IOTR.Mian;

[HarmonyPatch(typeof(SexUtilities))]
public static class IntimacyPatches
{
    [HarmonyPatch(nameof(SexUtilities.ApplyLovinThoughts))]
    [HarmonyPostfix]
    public static void AddLoveCompatiblities(Pawn pawn, Pawn partner)
    {
        if (pawn == null || partner == null)
            return;
        if(pawn.GetLoveRelations(false).Exists(relation => relation.otherPawn == partner))
            pawn.GainRomance(0.5f, ThoughtDefOf.GotSomeLovin.defName); // this isn't a magic number i promise, i saw this in the ROTR code for lovin
    }
}