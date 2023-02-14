using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FileHelper
{
    public static string GetResPath()
    {
        if(GameLaunch.Instance.config.enumGameMode == EnumGameMode.Develop)
        {
            return FileHelper.DevelopResPath();
        }
        else if(GameLaunch.Instance.config.enumGameMode == EnumGameMode.Local_AB)
        {
            return FileHelper.BaseLocalResPath();
        }
        else if(GameLaunch.Instance.config.enumGameMode == EnumGameMode.Server_AB)
        {
            return GameLaunch.Instance.config.serverUrl;
        }

        return string.Empty;
    }

    /// <summary>
    /// 开发阶段获取资源的路径
    /// </summary>
    /// <returns></returns>
    public static string DevelopResPath()
    {
        return "Assets/AssetsPackage/";
    }

    /// <summary>
    /// 获取本地StreamingAssets文件夹中资源基础路径
    /// </summary>
    public static string BaseLocalResPath()
    {
#if UNITY_EDITOR || UNITY_STANDALONE    //编辑器 或 单机
            return "file://" + Application.dataPath + "/StreamingAssets/";
#elif UNITY_ANDROID
            return "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IOS
            return "file://" + Application.dataPath + "/Raw/";
#endif
    }
}
