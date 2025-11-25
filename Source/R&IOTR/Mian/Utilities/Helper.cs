namespace R_IOTR.Mian.Utilities;

public static class Helper
{
    public static void Log(string text)
    {
        Verse.Log.Message("[Romance & Intimacy On The Rim] " + text);
    }

    public static void Debug(string text)
    {
        if (R_IOTRSettings.Instance.enableLogging)
            Verse.Log.Message("[Romance & Intimacy On The Rim] " + text);
    }

    public static void Error(string text)
    {
        Verse.Log.Error("[Romance & Intimacy On The Rim] " + text);
    }
}