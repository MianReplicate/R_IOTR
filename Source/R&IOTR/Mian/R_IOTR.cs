
using System;
using HarmonyLib;
using R_IOTR.Mian.Utilities;
using RomanceOnTheRim;
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

        if(R_IOTRSettings.Instance.enableLogging)
            Harmony.DEBUG = true;
    }
}

public class R_IOTRSettings : ModSettings
{
    public static R_IOTRSettings Instance;
    public bool enableLogging;
    public float intimacyInterval = defaultIntimacyInterval;
    public float intimacyMultiplier = defaultIntimacyMultiplier;
    public float romanceMultiplier = defaultRomanceMultiplier;

    public const float defaultIntimacyMultiplier = 0.5f;
    public const float defaultRomanceMultiplier = 1.0f;
    public const float defaultIntimacyInterval = 0.3f;
    
    public override void ExposeData()
    {
        Scribe_Values.Look(ref enableLogging, "enableLogging", false, true);
        Scribe_Values.Look(ref intimacyMultiplier, "intimacyMultiplier", defaultIntimacyMultiplier, true);
        Scribe_Values.Look(ref romanceMultiplier, "romanceMultiplier", defaultRomanceMultiplier, true);
        Scribe_Values.Look(ref intimacyInterval, "intimacyInterval", defaultIntimacyInterval, true);
        base.ExposeData();
    }
}

public class R_IOTR : Mod
{
    public R_IOTR(ModContentPack content) : base(content)
    {
        R_IOTRSettings.Instance = GetSettings<R_IOTRSettings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled("R_IOTR.EnableLogging".Translate(),
            ref R_IOTRSettings.Instance.enableLogging);
        R_IOTRSettings.Instance.intimacyInterval = listingStandard.SliderLabeled(
                                                       "R_IOTR.IntimacyIntervalScale".Translate(
                                                           R_IOTRSettings.defaultIntimacyInterval * 100f,
                                                           R_IOTRSettings.Instance.intimacyInterval * 100f),
                                                       R_IOTRSettings.Instance.intimacyInterval * 100f, 0f, 100f, tooltip: "R_IOTR.IntimacyIntervalScaleTip".Translate()) /
                                                   100f;
        R_IOTRSettings.Instance.intimacyMultiplier = (float) Math.Round(listingStandard.SliderLabeled("R_IOTR.IntimacyValueScale".Translate(R_IOTRSettings.defaultIntimacyMultiplier.ToString(), R_IOTRSettings.Instance.intimacyMultiplier.ToString()),  R_IOTRSettings.Instance.intimacyMultiplier * 10f, 0f, 100f, tooltip: "R_IOTR.IntimacyValueScaleTip".Translate()) / 10f, 1);
        R_IOTRSettings.Instance.romanceMultiplier = (float) Math.Round(listingStandard.SliderLabeled("R_IOTR.RomanceValueScale".Translate(R_IOTRSettings.defaultRomanceMultiplier.ToString(), R_IOTRSettings.Instance.romanceMultiplier.ToString()), R_IOTRSettings.Instance.romanceMultiplier * 10f, 0f, 100f, tooltip: "R_IOTR.RomanceValueScaleTip".Translate()) / 10f, 1);
        if (listingStandard.ButtonText("R_IOTR.ResetSettings".Translate()))
        {
            R_IOTRSettings.Instance.intimacyInterval = R_IOTRSettings.defaultIntimacyInterval;
            R_IOTRSettings.Instance.intimacyMultiplier = R_IOTRSettings.defaultIntimacyMultiplier;
            R_IOTRSettings.Instance.romanceMultiplier = R_IOTRSettings.defaultRomanceMultiplier;
        }
        
        listingStandard.End();
        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "R_IOTR.ModName".Translate();
    }
}