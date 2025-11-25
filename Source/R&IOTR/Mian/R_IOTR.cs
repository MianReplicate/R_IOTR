
using HarmonyLib;
using R_IOTR.Mian.Utilities;
using UnityEngine;
using Verse;

namespace R_IOTR.Mian;

[StaticConstructorOnStartup]
public static class Startup
{
    static Startup()
    {
        Helper.Log("When you've got romance on the rim, you've also got intimacy!");

        var harmony = new Harmony("rimworld.mian.r_iotr");
        harmony.PatchAll();
    }
}

public class R_IOTRSettings : ModSettings
{
    public static R_IOTRSettings Instance;
    public bool enableLogging;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref enableLogging, "enableLogging");
        base.ExposeData();
    }
}

public class GenderAcceptance : Mod
{
    public GenderAcceptance(ModContentPack content) : base(content)
    {
        R_IOTRSettings.Instance = GetSettings<R_IOTRSettings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled("R_IOTR.EnableLoggingExplanation".Translate(),
            ref R_IOTRSettings.Instance.enableLogging);
        listingStandard.End();
        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "R_IOTR.ModName".Translate();
    }
}