using JetBrains.Annotations;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AssetBundleCache
{
    /// <summary>
    /// AssetBundle名字
    /// </summary>
    private string m_AssetBundleName;

    /// <summary>
    /// 引用计数
    /// </summary>
    private int m_ReferencedCount;

    /// <summary>
    /// 卸载时间
    /// </summary>
    private float m_UnloadTime;

    /// <summary>
    /// ab包中所有Assets的名字 用于判断指定Asset是否存在于ab包中
    /// </summary>
    private HashSet<string> m_SetAllAssetNames = null;

    /// <summary>
    /// 正在异步加载的Asset
    /// </summary>
    private Dictionary<string, AssetBundleRequest> m_DicAsync = new Dictionary<string, AssetBundleRequest>();

    /// <summary>
    /// 已经加载完的Asset
    /// </summary>
    private Dictionary<string, Object> m_DicAssetCache = null;

    /// <summary>
    /// AssetBundle
    /// </summary>
    public AssetBundle Bundle { get; private set; }

    /// <summary>
    /// AssetBundle名字
    /// </summary>
    public string BundleName { get { return this.m_AssetBundleName; } }

    /// <summary>
    /// 引用计数
    /// </summary>
    public int ReferencedCount
    {
        get
        {
            return this.m_ReferencedCount;
        }
        set
        {
            this.m_ReferencedCount = value;
            if(this.m_ReferencedCount <= 0)
            {
                this.m_UnloadTime = Time.realtimeSinceStartup;
            }
            else
            {
                this.m_UnloadTime = 0;
            }
            if(this.m_ReferencedCount < 0)
            {
                Debug.LogWarningFormat("AssetBundleCache reference count < 0, name:{0}, referencecount:{1}", this.m_AssetBundleName, this.m_ReferencedCount);
            }
        }
    }

    /// <summary>
    /// 是否是常驻AssetBundle，通用资源的ab包不卸载
    /// </summary>
    public bool Persistent { get; set; }

    /// <summary>
    /// 是否可以卸载
    /// </summary>
    public bool IsCanRemove
    {
        get
        {
            if (this.Persistent == false && this.ReferencedCount <= 0)
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 缓存时间到
    /// </summary>
    public bool IsTimeOut
    {
        get
        {
            return Time.realtimeSinceStartup - this.m_UnloadTime >= GameLaunch.Instance.config.assetCacheTime;
        }
    }

    public AssetBundleCache(string name, AssetBundle ab, int refCount)
    {
        this.m_AssetBundleName = name;
        this.Bundle = ab;
        this.ReferencedCount = refCount;
    }

    /// <summary>
    /// 判断资源是否正在异步加载中
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <returns></returns>
    public bool IsAssetLoading(string assetName)
    {
        return this.m_DicAsync.ContainsKey(assetName);
    }

    /// <summary>
    /// 判断AssetBundle中是否包含该名字的资源
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <returns></returns>
    public bool IsExistAsset(string assetName)
    {
        if(this.m_SetAllAssetNames != null && this.m_SetAllAssetNames.Contains(assetName))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取缓存中的资源 无则返回null
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <returns></returns>
    public Object GetCacheAsset(string assetName)
    {
        if(this.m_DicAssetCache != null)
        {
            Object assetObj = null;
            if(this.m_DicAssetCache.TryGetValue(assetName, out assetObj))
            {
                return assetObj;
            }
        }
        return null;
    }

    /// <summary>
    /// 资源加载完成 缓存一下
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="assetObj">加载完成的Asset资源</param>
    public void OnLoadedAsset(string assetName, Object assetObj)
    {
        this.m_UnloadTime = Time.realtimeSinceStartup;

        if (this.m_DicAsync.ContainsKey(assetName))
        {
            this.m_DicAsync.Remove(assetName);
        }

        if(this.m_DicAssetCache == null)
        {
            this.m_DicAssetCache = new Dictionary<string, Object>();
        }

        if(this.m_DicAssetCache.ContainsKey(assetName))
        {
            Debug.LogWarningFormat("警报! 缓存已经存在了还重新加载:{0}", assetName);
        }
        this.m_DicAssetCache[assetName] = assetObj;
    }

    /// <summary>
    /// 异步加载资源 需要添加到[异步加载缓存(m_DicAsync)]中 防止重复加载
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="type"></param>
    /// <returns></returns>
    public AssetBundleRequest LoadAssetAsync(string assetName, System.Type type)
    {
        if(this.Bundle == null)
        {
            Debug.LogWarningFormat("AssetBundle:{0} is null, load asset async:{1},type:{2}, fail!!", this.m_AssetBundleName, assetName, type.ToString());
            return null;
        }

        AssetBundleRequest opt;
        this.m_DicAsync.TryGetValue(assetName, out opt);
        if(opt == null)
        {
            opt = this.Bundle.LoadAssetAsync(assetName, type);
            this.m_DicAsync[assetName] = opt;
        }
        return opt;
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="type"></param>
    /// <returns></returns>
    public Object LoadAsset(string assetName, System.Type type)
    {
        if (this.Bundle == null)
        {
            Debug.LogWarningFormat("AssetBundle:{0} is null, load asset async:{1},type:{2}, fail!!", this.m_AssetBundleName, assetName, type.ToString());
            return null;
        }

        Object assetObj = this.Bundle.LoadAsset(assetName, type);
        if(assetObj == null)
        {
            Debug.LogWarningFormat("AssetBuncleCache.LoadAsset, asset not exist:{0}, {1}", this.m_AssetBundleName, assetName);
        }
        else
        {
            this.OnLoadedAsset(assetName, assetObj);
        }
        return assetObj;
    }

    /// <summary>
    /// 加载ab包中所有Asset资源
    /// </summary>
    /// <param name="bCache"></param>
    /// <returns></returns>
    public Object[] LoadAllAssets(bool bCache = true)
    {
        Object[] allObjs = this.Bundle.LoadAllAssets();
        if(bCache)
        {
            for(int i = 0; i < allObjs.Length; ++i)
            {
                Object obj = allObjs[i];
                this.OnLoadedAsset(obj.name, obj);
            }
        }
        return allObjs;
    }

    /// <summary>
    /// 异步加载所有资源 只用作预加载使用
    /// </summary>
    /// <returns></returns>
    public AssetBundleRequest LoadAllAssetsAsync()
    {
        if (this.Bundle == null)
        {
            return null;
        }

        return this.Bundle.LoadAllAssetsAsync();
    }

    public bool LoadAllAssetNames()
    {
        if (Bundle == null)
        {
            return false;
        }
        string[] arrNames = Bundle.GetAllAssetNames();
        if (arrNames.Length == 0)
        {
            return false;
        }
        if (this.m_SetAllAssetNames == null)
        {
            this.m_SetAllAssetNames = new HashSet<string>();
        }

        for (int i = 0; i < arrNames.Length; i++)
        {
            string strName = System.IO.Path.GetFileNameWithoutExtension(arrNames[i]);
            this.m_SetAllAssetNames.Add(strName);
        }
        return true;
    }

    public void Unload()
    {
        if (this.m_DicAsync.Count > 0)
        {
            Debug.LogWarningFormat("[仅提醒]该Bundle还有资源在加载中，暂时不卸载:{0}", m_AssetBundleName);
            return;
        }

        if (this.Bundle != null)
        {

            this.Bundle.Unload(false);
            this.Bundle = null;
        }

        if (this.m_SetAllAssetNames != null)
        {
            this.m_SetAllAssetNames.Clear();
        }

        if (this.m_SetAllAssetNames != null)
        {
            this.m_SetAllAssetNames.Clear();
        }
    }
}
