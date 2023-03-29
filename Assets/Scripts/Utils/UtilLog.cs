using UnityEngine;

public static class UtilLog
{
    public static EnumColorLog enumColorLog = EnumColorLog.NONE;

    public static void Log(object arg)
    {
        LogWhite(arg);
    }

    public static void LogWhite(object arg)
    {
        if (UtilLog.enumColorLog == EnumColorLog.NONE)
        {
            return;
        }
        else if (UtilLog.enumColorLog == EnumColorLog.WHITE || UtilLog.enumColorLog == EnumColorLog.All)
        {
            string colHtmlString = ColorUtility.ToHtmlStringRGB(Color.white);
            Debug.Log(string.Format("<color=#{0}>{1}</color>", colHtmlString, arg.ToString()));
        }
    }

    public static void LogRed(object arg)
    {
        if (UtilLog.enumColorLog == EnumColorLog.NONE)
        {
            return;
        }
        else if (UtilLog.enumColorLog == EnumColorLog.RED || UtilLog.enumColorLog == EnumColorLog.All)
        {
            string colHtmlString = ColorUtility.ToHtmlStringRGB(Color.red);
            Debug.Log(string.Format("<color=#{0}>{1}</color>", colHtmlString, arg.ToString()));
        }
    }

    public static void LogGreen(object arg)
    {
        if (UtilLog.enumColorLog == EnumColorLog.NONE)
        {
            return;
        }
        else if (UtilLog.enumColorLog == EnumColorLog.GREEN || UtilLog.enumColorLog == EnumColorLog.All)
        {
            string colHtmlString = ColorUtility.ToHtmlStringRGB(Color.green);
            Debug.Log(string.Format("<color=#{0}>{1}</color>", colHtmlString, arg.ToString()));
        }
    }

    public static void LogBlue(object arg)
    {
        if (UtilLog.enumColorLog == EnumColorLog.NONE)
        {
            return;
        }
        else if (UtilLog.enumColorLog == EnumColorLog.BLUE || UtilLog.enumColorLog == EnumColorLog.All)
        {
            string colHtmlString = ColorUtility.ToHtmlStringRGB(Color.blue);
            Debug.Log(string.Format("<color=#{0}>{1}</color>", colHtmlString, arg.ToString()));
        }
    }

    public static void LogCyan(object arg)
    {
        if (UtilLog.enumColorLog == EnumColorLog.NONE)
        {
            return;
        }
        else if (UtilLog.enumColorLog == EnumColorLog.CYAN || UtilLog.enumColorLog == EnumColorLog.All)
        {
            string colHtmlString = ColorUtility.ToHtmlStringRGB(Color.cyan);
            Debug.Log(string.Format("<color=#{0}>{1}</color>", colHtmlString, arg.ToString()));
        }
    }

    public static void LogMagenta(object arg)
    {
        if (UtilLog.enumColorLog == EnumColorLog.NONE)
        {
            return;
        }
        else if (UtilLog.enumColorLog == EnumColorLog.MAGENTA || UtilLog.enumColorLog == EnumColorLog.All)
        {
            string colHtmlString = ColorUtility.ToHtmlStringRGB(Color.magenta);
            Debug.Log(string.Format("<color=#{0}>{1}</color>", colHtmlString, arg.ToString()));
        }
    }

    public static void LogYellow(object arg)
    {
        if (UtilLog.enumColorLog == EnumColorLog.NONE)
        {
            return;
        }
        else if (UtilLog.enumColorLog == EnumColorLog.YELLOW || UtilLog.enumColorLog == EnumColorLog.All)
        {
            string colHtmlString = ColorUtility.ToHtmlStringRGB(Color.yellow);
            Debug.Log(string.Format("<color=#{0}>{1}</color>", colHtmlString, arg.ToString()));
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
