using LoveyDoveySexWithEuterpe;
using RomanceOnTheRim;
using Verse;

namespace R_IOTR.Mian.Utilities;

public static class CompatibilityUtility
{
    public static void GainIntimacy(this Pawn pawn, float amount, bool forRomance)
    {
        Helper.Debug($"Intimacy increasing by {amount} for {pawn.Name}");
        if (forRomance){
            CommonChecks.TryGainIntimacy(pawn, amount);
        }else
        {
            CommonChecks.TryGainSocialIntimacy(pawn, amount);
        }
    }

    public static void GainRomance(this Pawn pawn, float amount)
    {
        Helper.Debug($"Romance increasing by {amount} for {pawn.Name}");
        pawn.TryAffectRomanceNeedLevel(amount);
    }
}