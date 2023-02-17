using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void AssetsLoadCallback(string name, UnityEngine.Object obj);

public class AssetObject
{
    public string _assetName;
    /// <summary>
    /// 记录回调当前数量 保证异步是下一帧回调
    /// </summary>
    public int _lockCallbackCount;
    /// <summary>
    /// 回调函数
    /// </summary>
    public List<AssetsLoadCallback> _callbackList = new List<AssetsLoadCallback>();

    /// <summary>
    /// asset的id
    /// </summary>
    public int _instanceID;
    /// <summary>
    /// 异步请求 AssetBundeRequest或ResourceRequest
    /// </summary>
    public AsyncOperation _request;
    /// <summary>
    /// 加载的资源Asset
    /// </summary>
    public UnityEngine.Object _asset;
    /// <summary>
    /// 标识是否是ab资源加载的
    /// </summary>
    public bool _isABLoad;

    /// <summary>
    /// 是否是弱引用 用于预加载和释放
    /// </summary>
    public bool _isWeak = true;
    /// <summary>
    /// 引用计数
    /// </summary>
    public int _refCount;
    /// <summary>
    /// 卸载使用延迟卸载 UNLOAD_DELAY_TICK_BASE + _unloadList.Count
    /// </summary>
    public int _unloadTick;
}
