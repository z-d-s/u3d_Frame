using UnityEngine;

public static class LogHelper
{
    public static EnumColorLog enumColorLog = EnumColorLog.NONE;

    public static void Log(object arg)
    {
        LogWhite(arg);
    }

    public static void LogWhite(object arg)
    {
        if (LogHelper.enumColorLog == EnumColorLog.NONE)
        {
            return;
        }
        else if (LogHelper.enumColorLog == EnumColorLog.WHITE || LogHelper.enumColorLog == EnumColorLog.All)
        {
            string colHtmlString = ColorUtility.ToHtmlStringRGB(Color.white);
            Debug.Log(string.Format("<color=#{0}>= WHITE =:{1}</color>", colHtmlString, arg.ToString()));
        }
    }

    public static void LogRed(object arg)
    {
        if (LogHelper.enumColorLog == EnumColorLog.NONE)
        {
            return;
        }
        else if (LogHelper.enumColorLog == EnumColorLog.RED || LogHelper.enumColorLog == EnumColorLog.All)
        {
            string colHtmlString = ColorUtility.ToHtmlStringRGB(Color.red);
            Debug.Log(string.Format("<color=#{0}>= RED =:{1}</color>", colHtmlString, arg.ToString()));
        }
    }

    public static void LogGreen(object arg)
    {
        if (LogHelper.enumColorLog == EnumColorLog.NONE)
        {
            return;
        }
        else if (LogHelper.enumColorLog == EnumColorLog.GREEN || LogHelper.enumColorLog == EnumColorLog.All)
        {
            string colHtmlString = ColorUtility.ToHtmlStringRGB(Color.green);
            Debug.Log(string.Format("<color=#{0}>= GREEN =:{1}</color>", colHtmlString, arg.ToString()));
        }
    }

    public static void LogBlue(object arg)
    {
        if (LogHelper.enumColorLog == EnumColorLog.NONE)
        {
            return;
        }
        else if (LogHelper.enumColorLog == EnumColorLog.BLUE || LogHelper.enumColorLog == EnumColorLog.All)
        {
            string colHtmlString = ColorUtility.ToHtmlStringRGB(Color.blue);
            Debug.Log(string.Format("<color=#{0}>= BLUE =:{1}</color>", colHtmlString, arg.ToString()));
        }
    }

    public static void LogCyan(object arg)
    {
        if (LogHelper.enumColorLog == EnumColorLog.NONE)
        {
            return;
        }
        else if (LogHelper.enumColorLog == EnumColorLog.CYAN || LogHelper.enumColorLog == EnumColorLog.All)
        {
            string colHtmlString = ColorUtility.ToHtmlStringRGB(Color.cyan);
            Debug.Log(string.Format("<color=#{0}>= CYAN =:{1}</color>", colHtmlString, arg.ToString()));
        }
    }

    public static void LogMagenta(object arg)
    {
        if (LogHelper.enumColorLog == EnumColorLog.NONE)
        {
            return;
        }
        else if (LogHelper.enumColorLog == EnumColorLog.MAGENTA || LogHelper.enumColorLog == EnumColorLog.All)
        {
            string colHtmlString = ColorUtility.ToHtmlStringRGB(Color.magenta);
            Debug.Log(string.Format("<color=#{0}>= MAGENTA =:{1}</color>", colHtmlString, arg.ToString()));
        }
    }

    public static void LogYellow(object arg)
    {
        if (LogHelper.enumColorLog == EnumColorLog.NONE)
        {
            return;
        }
        else if (LogHelper.enumColorLog == EnumColorLog.YELLOW || LogHelper.enumColorLog == EnumColorLog.All)
        {
            string colHtmlString = ColorUtility.ToHtmlStringRGB(Color.yellow);
            Debug.Log(string.Format("<color=#{0}>= YELLOW =:{1}</color>", colHtmlString, arg.ToString()));
        }
    }

    public static void LogWarning(string arg)
    {
        Debug.LogWarning(arg);
    }

    public static void LogError(string arg)
    {
        Debug.LogError(arg);
    }
}
