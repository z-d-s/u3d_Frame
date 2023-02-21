/****************************************************
	Resource下的资源在Runtime情况下是无法判读有什么资源的，所以先要有个配置文件，可以记录所有资源列表
	这样就有了ExportConfig()和ReadConfig()对列表的导出和读取
	
	通用的4个接口:
            -- IsFileExist
            -- LoadAsync
            -- LoadSync
            -- Unload
*****************************************************/

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourcesLoadMgr : MonoBaseSingleton<ResourcesLoadMgr>
{
    private HashSet<string> _resourcesList;

    private ResourcesLoadMgr()
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
        string path = Application.dataPath + "/Resources/";
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
