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

*****************************************************/

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EditorAssetLoadMgr : MonoBaseSingleton<EditorAssetLoadMgr>
{
    private HashSet<string> _resourcesList;

    private EditorAssetLoadMgr()
    {
        this._resourcesList = new HashSet<string>();
#if UNITY_EDITOR
        this.ExportConfig();
#endif
        this.ReadConfig();
    }

#if UNITY_EDITOR
    private void ExportConfig()
    {
        string path = Application.dataPath + "/Editor/Resources/";
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
        if(File.Exists(path))
        {
            File.Delete(path);
        }
        File.WriteAllText(path, txt);
    }
#endif

    private void ReadConfig()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("FileList");
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

    public ResourceRequest LoadAsync(string _assetName)
    {
        if(!this._resourcesList.Contains(_assetName))
        {
            //Utils.LogError("EditorAssetLoadMgr No Find File " + _assetName);
            return null;
        }

        return Resources.LoadAsync(_assetName);
    }

    public UnityEngine.Object LoadSync(string _assetName)
    {
        if(!this._resourcesList.Contains(_assetName))
        {
            //Utils.LogError("EditorAssetLoadMgr No Find File " + _assetName);
            return null;
        }

        return Resources.Load(_assetName);
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
