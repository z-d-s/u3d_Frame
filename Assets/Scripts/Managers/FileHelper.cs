using UnityEngine;

public class FileHelper
{
    /// <summary>
	/// 获取主包的目标平台
	/// </summary>
	public static string ABManiName
    {
        get
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            return "Win64";
#elif UNITY_ANDROID
			return "Android";
#elif UNITY_IOS
			return "IOS";
#endif
        }
    }

    public static string GetResPath()
    {
        if(GameLaunch.Instance.config.enumGameMode == EnumGameMode.Develop)
        {
            return FileHelper.DevelopResPath();
        }
        else if(GameLaunch.Instance.config.enumGameMode == EnumGameMode.Local_AB)
        {
            return FileHelper.BaseLocalResPath() + ABManiName + "/";
        }
        else if(GameLaunch.Instance.config.enumGameMode == EnumGameMode.Server_AB)
        {
            return GameLaunch.Instance.config.serverUrl + ABManiName + "/";
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
    /// 方案一：获取本地StreamingAssets文件夹中资源基础路径
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

    /// <summary>
    /// 方案二：获取本地StreamingAssets文件夹中资源基础路径
    /// </summary>
    /// <returns></returns>
    public static string BaseLocalResPath_Another()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return "file://" + Application.streamingAssetsPath;
#elif UNITY_ANDROID
        return Application.streamingAssetsPath;
#elif UNITY_IOS
        return "file://" + Application.streamingAssetsPath;
#endif
    }
}
