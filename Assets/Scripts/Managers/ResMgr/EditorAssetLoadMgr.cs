/****************************************************
 
	Assets目录下有2个Resources目录
    一个路径是Assets/Resources/
    另一个是Assets/Editor/Resources/
    读取两个目录可以通用Resources.Load()接口
    打包时后者目录内资源在Editor下，不会进入包体，这是一个小技巧
	
	通用的4个接口：
            -- IsFileExist  文件是否存在
            -- LoadAsync    异步加载
            -- LoadSync     同步加载
            -- Unload       卸载

    ps注意点：
            -- 路径不区分大小写
            -- 包含文件扩展名

*****************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EditorAssetLoadMgr : MonoBaseSingleton<EditorAssetLoadMgr>
{
    private HashSet<string> _resourcesList;

    public override void Awake()
    {
        base.Awake();
        this._resourcesList = new HashSet<string>();

        this.ReadConfig();
    }

#if UNITY_EDITOR
    private void ExportConfig()
    {
        string path = FileHelper.DevelopResPath();
        string[] files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

        string txt = "";
        foreach (var file in files)
        {
            if (file.EndsWith(".meta")) continue;

            string name = file.Replace(path, "");
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
    }
#endif

    private void ReadConfig()
    {
        string path = FileHelper.DevelopResPath();
        TextAsset textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(path + "FileList.bytes");
        if (textAsset == null)
        {
            Utils.LogError("请先创建Editor模式下资源列表 --- FileList.bytes --- 文件");
            return;
        }

        string txt = textAsset.text;
        txt = txt.Replace("\r\n", "\n");

        foreach (var line in txt.Split('\n'))
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if(!this._resourcesList.Contains(line))
            {
                this._resourcesList.Add(line);
            }
        }
    }

    public bool IsFileExist(string _assetName)
    {
        return this._resourcesList.Contains(_assetName);
    }

    public EditorResourceRequest LoadAsync(string _assetName)
    {
        if(!this._resourcesList.Contains(_assetName))
        {
            Utils.LogError("EditorAssetLoadMgr No Find File " + _assetName);
            return null;
        }

        //return Resources.LoadAsync(_assetName);
        EditorResourceRequest request = new EditorResourceRequest();
        request.asset = UnityEditor.AssetDatabase.LoadAssetAtPath(FileHelper.DevelopResPath() + _assetName, typeof(UnityEngine.Object));
        request.isDone = true;
        return request;
    }

    public UnityEngine.Object LoadSync(string _assetName)
    {
        if(!this._resourcesList.Contains(_assetName))
        {
            Utils.LogError("EditorAssetLoadMgr No Find File " + _assetName);
            return null;
        }

        //return Resources.Load(_assetName);
        return UnityEditor.AssetDatabase.LoadAssetAtPath(FileHelper.DevelopResPath() + _assetName, typeof(UnityEngine.Object));
    }

    public void Unload(UnityEngine.Object asset)
    {
        if(asset is GameObject)
        {
            return;
        }

        Resources.UnloadAsset(asset);
        asset = null;
    }

    public void Update()
    {

    }
}


public class EditorResourceRequest : AsyncOperation
{
    public Object asset = null;
    public new bool isDone = false;
}