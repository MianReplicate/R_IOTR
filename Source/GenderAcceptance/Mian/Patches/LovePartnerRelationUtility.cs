using GenderAcceptance.Mian.Utilities;
using HarmonyLib;
using Verse;
using GenderUtility = GenderAcceptance.Mian.Utilities.GenderUtility;

namespace GenderAcceptance.Mian.Patches;

[HarmonyPatch(typeof(RimWorld.LovePartnerRelationUtility))]
public static class LovePartnerRelationUtility
{
    [HarmonyPatch(typeof(RimWorld.LovePartnerRelationUtility),
        nameof(RimWorld.LovePartnerRelationUtility.LovePartnerRelationGenerationChance))]
    [HarmonyPostfix]
    public static void Postfix(Pawn generated, Pawn other, ref float __result)
    {
        //Adjust with chaser rating
        if (generated.FindsExtraordinarilyAttractive(other))
            __result *= 2f;
    }
}