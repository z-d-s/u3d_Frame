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

using System.Collections.Generic;
using UnityEngine;

public class EditorResourceRequest : AsyncOperation
{
    public Object asset = null;
    public new bool isDone = false;
}

public class EditorAssetLoadMgr : MonoBaseSingleton<EditorAssetLoadMgr>
{
    private HashSet<string> _resourcesList;

    public override void Awake()
    {
        base.Awake();
        this._resourcesList = new HashSet<string>();

        this.ReadConfig();
    }

    private void ReadConfig()
    {
        string path = FileHelper.EditorAssetPath();
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

    public UnityEngine.Object LoadSync(string _assetName)
    {
        if (!this._resourcesList.Contains(_assetName))
        {
            Utils.LogError("EditorAssetLoadMgr No Find File " + _assetName);
            return null;
        }

        return UnityEditor.AssetDatabase.LoadAssetAtPath(FileHelper.EditorAssetPath() + _assetName, typeof(UnityEngine.Object));
    }

    public EditorResourceRequest LoadAsync(string _assetName)
    {
        if(!this._resourcesList.Contains(_assetName))
        {
            Utils.LogError("EditorAssetLoadMgr No Find File " + _assetName);
            return null;
        }

        EditorResourceRequest request = new EditorResourceRequest();
        request.asset = UnityEditor.AssetDatabase.LoadAssetAtPath(FileHelper.EditorAssetPath() + _assetName, typeof(UnityEngine.Object));
        request.isDone = true;
        return request;
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