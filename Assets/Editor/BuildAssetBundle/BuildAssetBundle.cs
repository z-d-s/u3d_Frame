using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAssetBundle : Editor
{
    /// <summary>
    /// 目标平台枚举类型
    /// </summary>
    enum EnumTarget
    {
        Win64,
        Android,
        IOS
    }

    /// <summary>
    /// 检查文件夹是否存在 不存在则创建一个
    /// </summary>
    /// <param name="path"></param>
    private static void CheckPathExist(string path)
    {
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// 删除文件 包括文件中的子文件夹
    /// </summary>
    /// <param name="paths"></param>
    private static void DeleteFile(params string[] paths)
    {
        for(int i = 0; i < paths.Length; ++i)
        {
            if (Directory.Exists(paths[i]))
            {
                Directory.Delete(paths[i], true);
            }
        }
    }

    /// <summary>
    /// 获取打包路径
    /// </summary>
    /// <param name="target">目标平台</param>
    /// <param name="checkExist">是否检查文件夹存在与否</param>
    /// <returns></returns>
    private static string GetAssetBundleBuildPath(EnumTarget target, bool checkExist = true)
    {
        string path = Application.dataPath + "/../BuildAssetBundles/" + target.ToString();
        if (checkExist) CheckPathExist(path);
        //Debug.Log("GetAssetBundleBuildPath:" + path);
        return path;
    }

    /// <summary>
    /// 获取StreamingAssets打包路径
    /// </summary>
    /// <param name="target">目标平台</param>
    /// <param name="checkExist">是否检查文件夹存在与否</param>
    /// <returns></returns>
    private static string GetAssetBundleInnerBuildPath(EnumTarget target, bool checkExist = true)
    {
        string path = Application.streamingAssetsPath + "/" + target.ToString();
        if (checkExist) CheckPathExist(path);
        //Debug.Log("GetAssetBundleInnerBuildPath:" + path);
        return path;
    }

    /// <summary>
    /// 拷贝文件
    /// </summary>
    /// <param name="sourcePath">源文件夹路径</param>
    /// <param name="targetPath">目标文件夹路径</param>
    private static void CopyFilesToStreamingAssets(string sourcePath, string targetPath)
    {
        DirectoryInfo dir = new DirectoryInfo(sourcePath);
        //获取目标路径下的所有文件
        FileInfo[] files = dir.GetFiles("*", SearchOption.AllDirectories);
        for(int i = 0; i < files.Length; ++i)
        {
            string fileName = files[i].Name;
            //过滤掉以.manifest结尾的文件
            if(!fileName.EndsWith(".manifest"))
            {
                File.Copy(sourcePath + "/" + fileName, targetPath + "/" + fileName, true);
            }
        }
    }

    /// <summary>
    /// 设置单个AssetBundle的name
    /// </summary>
    /// <param name="assetPath">资源路径</param>
    /// <param name="bundleName">包名</param>
    private static void SetAssetBundleName(string assetPath, string bundleName)
    {
        AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
        assetImporter.assetBundleName = bundleName;
    }

    /// <summary>
    /// 根据路径设置ABName
    /// [路径是文件，直接赋ABName]
    /// [同时判断是否有同名的文件夹，如果有，将文件夹下文件赋ABName，继续判断是否有子文件夹，如果有，递归执行设置ABName操作]
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    private static void CheckPathFileOrDirectory(string path, string name)
    {
        if (File.Exists(path))
        {
            SetAssetBundleName(path, name);
        }
        if (Directory.Exists(path))
        {
            foreach (string subDir in Directory.GetDirectories(path))
            {
                CheckPathFileOrDirectory(subDir, name);
            }

            string[] subFiles = Directory.GetFiles(path);
            for(int i = 0; i < subFiles.Length; ++i)
            {
                subFiles[i] = subFiles[i].Replace("\\", "/");
                if(!subFiles[i].EndsWith(".meta"))
                {
                    SetAssetBundleName(subFiles[i], name);
                }
            }
        }
    }

    [MenuItem("BuildAB/设置 ABName", false, 1)]
    public static void GetAssetBundleName()
    {
        string datapath = Application.dataPath + "/Scripts/Config/AssetBundleName.txt";
        FileStream fs = new FileStream(datapath, FileMode.Open, FileAccess.Read, FileShare.None);
        StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
        if (sr != null)
        {
            string[] lines = sr.ReadToEnd().Split("\r\n");
            for (int i = 0; i < lines.Length; ++i)
            {
                string[] path_name = lines[i].Split(",");
                if (path_name.Length == 2)
                {
                    CheckPathFileOrDirectory(path_name[0], path_name[1]);
                }
                else
                {
                    Debug.LogError("AssetBundleName.txt中第" + (i + 1).ToString() + "行配置错误");
                }
            }
        }
        sr.Close();

        AssetDatabase.Refresh();
    }

    [MenuItem("BuildAB/清理所有 ABName", false, 2)]
    private static void ClearAllAssetBundlesName()
    {
        string[] abNames = AssetDatabase.GetAllAssetBundleNames();
        for(int i = 0; i < abNames.Length; ++i)
        {
            AssetDatabase.RemoveAssetBundleName(abNames[i], true);
        }

        AssetDatabase.Refresh();
    }

    [MenuItem("BuildAB/清理 Win64 资源包", false, 21)]
    private static void ClearAssetBundles_Win64()
    {
        string sourcePath = GetAssetBundleBuildPath(EnumTarget.Win64, false);
        string targetPath = GetAssetBundleInnerBuildPath(EnumTarget.Win64, false);
        DeleteFile(sourcePath, targetPath);

        AssetDatabase.Refresh();
    }

    [MenuItem("BuildAB/清理 Android 资源包", false, 22)]
    private static void ClearAssetBundles_Android()
    {
        string sourcePath = GetAssetBundleBuildPath(EnumTarget.Android, false);
        string targetPath = GetAssetBundleInnerBuildPath(EnumTarget.Android, false);
        DeleteFile(sourcePath, targetPath);

        AssetDatabase.Refresh();
    }

    [MenuItem("BuildAB/清理 IOS 资源包", false, 23)]
    private static void ClearAssetBundles_IOS()
    {
        string sourcePath = GetAssetBundleBuildPath(EnumTarget.IOS, false);
        string targetPath = GetAssetBundleInnerBuildPath(EnumTarget.IOS, false);
        DeleteFile(sourcePath, targetPath);

        AssetDatabase.Refresh();
    }

    [MenuItem("BuildAB/清理全部资源包", false, 31)]
    private static void ClearAssetBundle_All()
    {
        ClearAssetBundles_Win64();
        ClearAssetBundles_Android();
        ClearAssetBundles_IOS();
    }

    [MenuItem("BuildAB/Win64 资源打包", false, 101)]
    public static void BuildAssetBundle_Win64()
    {
        ClearAssetBundles_Win64();

        string sourcePath = GetAssetBundleBuildPath(EnumTarget.Win64);
        BuildPipeline.BuildAssetBundles(sourcePath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);

        string targetPath = GetAssetBundleInnerBuildPath(EnumTarget.Win64);
        CopyFilesToStreamingAssets(sourcePath, targetPath);

        AssetDatabase.Refresh();
    }

    [MenuItem("BuildAB/Android 资源打包", false, 102)]
    public static void BuildAssetBundle_Android()
    {
        ClearAssetBundles_Android();

        string sourcePath = GetAssetBundleBuildPath(EnumTarget.Android);
        BuildPipeline.BuildAssetBundles(sourcePath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);

        string targetPath = GetAssetBundleInnerBuildPath(EnumTarget.Android);
        CopyFilesToStreamingAssets(sourcePath, targetPath);

        AssetDatabase.Refresh();
    }

    [MenuItem("BuildAB/IOS 资源打包", false, 103)]
    public static void BuildAssetBundle_IOS()
    {
        ClearAssetBundles_IOS();

        string sourcePath = GetAssetBundleBuildPath(EnumTarget.IOS);
        BuildPipeline.BuildAssetBundles(sourcePath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.iOS);

        string targetPath = GetAssetBundleInnerBuildPath(EnumTarget.IOS);
        CopyFilesToStreamingAssets(sourcePath, targetPath);

        AssetDatabase.Refresh();
    }

    [MenuItem("BuildAB/全平台资源打包", false, 111)]
    public static void BuildAllPlatform()
    {
        BuildAssetBundle_Win64();
        BuildAssetBundle_Android();
        BuildAssetBundle_IOS();
    }
}
