using HarmonyLib;
using LoveyDoveySexWithEuterpe;
using R_IOTR.Mian.Utilities;
using RimWorld;
using RomanceOnTheRim;
using UnityEngine;
using Verse;

namespace R_IOTR.Mian;

public static class Patches
{
    public static class IntimacyPatches
    {
        [HarmonyPatch(typeof(SexUtilities), nameof(SexUtilities.ApplyLovinThoughts))]
        [HarmonyPostfix]
        public static void AddLoveCompatiblities(Pawn pawn, Pawn partner)
        {
            if (pawn == null || partner == null)
                return;
            if(pawn.GetLoveRelations(false).Exists(relation => relation.otherPawn == partner))
                pawn.GainRomance(0.5f); // this isn't a magic number i promise, i saw this in the ROTR code for lovin
        }
    }

    public static class ROTRPatches
    {
        [HarmonyPatch(typeof(LordJob_RomanticInteraction), "ApplyFinishOutcome")]
        [HarmonyPostfix]
        public static void AddPartialOutcomeCompatibility(Pawn ___Initiator, Pawn ___Recipient, float ___RomanceNeedGainFromInteraction, float ___StartInteractionTick, float ___InteractionDuration)
        {
            var intimacy = Mathf.Clamp01( (Find.TickManager.TicksGame - ___StartInteractionTick) / ___InteractionDuration) * ___RomanceNeedGainFromInteraction;
            ___Initiator.GainIntimacy(intimacy, true);
            ___Recipient.GainIntimacy(intimacy, true);
        }
        
        [HarmonyPatch(typeof(LordJob_RomanticInteraction), "ApplyFinishOutcome")]
        [HarmonyPostfix]
        public static void AddFinishOutcomeCompatibility(Pawn ___Initiator, Pawn ___Recipient, float ___RomanceNeedGainFromInteraction)
        {
            var intimacy = ___RomanceNeedGainFromInteraction;
            ___Initiator.GainIntimacy(intimacy, true);
            ___Recipient.GainIntimacy(intimacy, true);
        }
    }
}