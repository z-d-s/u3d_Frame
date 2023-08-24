using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildFileList : Editor
{
    [MenuItem("BuildFileList/创建 EditorFileList", false, 1)]
    public static void BuildEditorFileList()
    {
        string path = Application.dataPath + "/AssetsPackage/";
        string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

        string txt = "";
        foreach (var file in files)
        {
            if (file.EndsWith(".meta")) continue;

            string name = file.Replace(path, "");
            //name = name.Substring(0, name.LastIndexOf("."));
            name = name.Replace("\\", "/");
            txt += name + "\n";
        }

        path = path + "FileList.bytes";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        File.WriteAllText(path, txt);

        AssetDatabase.Refresh();
    }

    [MenuItem("BuildFileList/创建 ResourcesFileList", false, 2)]
    public static void BuildResourcesFileList()
    {
        string path = Application.dataPath + "/Resources/";
        string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

        string txt = "";
        foreach (var file in files)
        {
            if (file.EndsWith(".meta")) continue;

            string name = file.Replace(path, "");
            //Resources加载不包含扩展名
            name = name.Substring(0, name.LastIndexOf("."));
            name = name.Replace("\\", "/");
            txt += name + "\n";
        }

        path = path + "FileList.bytes";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        File.WriteAllText(path, txt);

        AssetDatabase.Refresh();
    }

    [MenuItem("BuildFileList/创建全部 FileList", false, 21)]
    public static void BuildAllFileList()
    {
        BuildEditorFileList();
        BuildResourcesFileList();
    }
}
