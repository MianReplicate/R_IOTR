using LoveyDoveySexWithEuterpe;
using RomanceOnTheRim;
using Verse;

namespace R_IOTR.Mian.Utilities;

public static class CompatibilityUtility
{
    public static void GainIntimacy(this Pawn pawn, float amount, bool forRomance, string label=null)
    {
        amount *= R_IOTRSettings.Instance.intimacyMultiplier;
        Helper.Debug($"Intimacy increasing by {amount} for {pawn.Name}, Label: {label}");
        if (forRomance){
            CommonChecks.TryGainIntimacy(pawn, amount);
        }else
        {
            CommonChecks.TryGainSocialIntimacy(pawn, amount);
        }
    }

    public static void GainRomance(this Pawn pawn, float amount, string label=null)
    {
        amount *= R_IOTRSettings.Instance.romanceMultiplier;
        Helper.Debug($"Romance increasing by {amount} for {pawn.Name}, Label: {label}");
        pawn.TryAffectRomanceNeedLevel(amount);
    }
}