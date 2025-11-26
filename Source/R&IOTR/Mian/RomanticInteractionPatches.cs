using HarmonyLib;
using R_IOTR.Mian.Utilities;
using RomanceOnTheRim;
using UnityEngine;
using Verse;

namespace R_IOTR.Mian;

[HarmonyPatch(typeof(LordJob_RomanticInteraction))]
public static class RomanticInteractionPatches
{
    [HarmonyPatch("ApplyPartialOutcome")]
    [HarmonyPostfix]
    public static void AddPartialOutcomeCompatibility(LordJob_RomanticInteraction __instance)
    {
        if (!__instance.Interacted)
            return;
        
        var intimacy = Mathf.Clamp01( (Find.TickManager.TicksGame - __instance.StartInteractionTick) / __instance.InteractionDuration
        ) * __instance.RomanceNeedGainFromInteraction;
        __instance.Initiator.GainIntimacy(intimacy, true);
        __instance.Recipient.GainIntimacy(intimacy, true);
    }
        
    [HarmonyPatch("ApplyFinishOutcome")]
    [HarmonyPostfix]
    public static void AddFinishOutcomeCompatibility(LordJob_RomanticInteraction __instance)
    {
        var intimacy = __instance.RomanceNeedGainFromInteraction;
        __instance.Initiator.GainIntimacy(intimacy, true);
        __instance.Recipient.GainIntimacy(intimacy, true);
    }
}