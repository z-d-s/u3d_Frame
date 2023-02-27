/****************************************************

	Resource下的资源在Runtime情况下是无法判读有什么资源的，所以先要有个配置文件，可以记录所有资源列表
	这样就有了ExportConfig()和ReadConfig()对列表的导出和读取
	
	通用的4个接口：
            -- IsFileExist  文件是否存在
            -- LoadAsync    异步加载
            -- LoadSync     同步加载
            -- Unload       卸载

    ps注意点：
            -- 路径不区分大小写
            -- 不能包含文件扩展名

*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class ResourcesLoadMgr : MonoBaseSingleton<ResourcesLoadMgr>
{
    private HashSet<string> _resourcesList;
    
    override public void Awake()
    {
        base.Awake();
        this._resourcesList = new HashSet<string>();

        this.ReadConfig();
    }

    private void ReadConfig()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("FileList");
        if (textAsset == null)
        {
            Utils.LogError("请先创建Resources加载模式下资源列表 --- FileList.bytes --- 文件");
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
        _assetName = _assetName.Substring(0, _assetName.LastIndexOf("."));
        return this._resourcesList.Contains(_assetName);
    }

    public UnityEngine.Object LoadSync(string _assetName)
    {
        _assetName = _assetName.Substring(0, _assetName.LastIndexOf("."));
        if (!this._resourcesList.Contains(_assetName))
        {
            Utils.LogError("ResourcesLoadMgr No Find File " + _assetName);
            return null;
        }

        return Resources.Load(_assetName);
    }

    public ResourceRequest LoadAsync(string _assetName)
    {
        _assetName = _assetName.Substring(0, _assetName.LastIndexOf("."));
        if (!this._resourcesList.Contains(_assetName))
        {
            Utils.LogError("ResourcesLoadMgr No Find File " + _assetName);
            return null;
        }

        return Resources.LoadAsync(_assetName);
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
