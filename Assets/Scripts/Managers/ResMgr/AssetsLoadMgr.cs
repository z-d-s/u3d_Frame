/****************************************************

	Asset - 资源在内存中的存在格式

    外部接口：
        -- IsAssetExist     文件是否存在
        -- PreLoad          预加载
        -- LoadAsync        异步加载
        -- LoadSync         同步加载
        -- Unload           卸载(***资源销毁 请保证资源销毁都要调用这个接口***)
        -- Update           刷新

        -- AddAsset
        -- AddAssetRef
        -- RemoveCallBack

*****************************************************/

#define TEST_AB1

using System.Collections.Generic;
using UnityEngine;

public class AssetsLoadMgr : MonoBaseSingleton<AssetsLoadMgr>
{
    public delegate void AssetsLoadCallback(string name, UnityEngine.Object obj);

    private class AssetObject
    {
        /// <summary>
        /// ab包名称
        /// </summary>
        public string _assetBundleName;
        /// 资源名称
        /// </summary>
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
        /// 异步请求 AssetBundeRequest或ResourceRequest或EditorResourceRequest
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
        /// 是否是弱引用 用于预加载和释放(true为使用过后会销毁，为false将不会销毁，慎用)
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

    private class PreloadAssetObject
    {
        /// <summary>
        /// ab包名称
        /// </summary>
        public string _assetBundleName;
        /// <summary>
        /// 资源名称
        /// </summary>
        public string _assetName;
        /// <summary>
        /// 是否是弱引用
        /// </summary>
        public bool _isWeak = true;
    }

    /// <summary>
    /// 卸载最低延迟
    /// </summary>
    public const int UNLOAD_DELAY_TICK_BASE = 60 * 60;
    /// <summary>
    /// 每加载50个后，空闲时进行一次资源清理
    /// </summary>
    private const int LOADING_INTERVAL_MAX_COUNT = 50;

    /// <summary>
    /// 创建临时存储变量，用于提升性能
    /// </summary>
    private List<AssetObject> tempLoadeds = new List<AssetObject>();

    /// <summary>
    /// 正在下载的资源列表
    /// </summary>
    private Dictionary<string, AssetObject> _loadingList;
    /// <summary>
    /// 已经加载的资源列表
    /// </summary>
    private Dictionary<string, AssetObject> _loadedList;
    /// <summary>
    /// 准备卸载的列表
    /// </summary>
    private Dictionary<string, AssetObject> _unloadList;
    /// <summary>
    /// 异步加载队列(当资源已经加载完成，但需要异步回调时，延帧回调)
    /// </summary>
    private List<AssetObject> _loadedAsyncList;
    /// <summary>
    /// 异步预加载，空闲时加载(当加载队列为空情况下，取1个创建Asset单元放入加载队列)
    /// </summary>
    private Queue<PreloadAssetObject> _preloadedAsyncList;

    /// <summary>
    /// 创建的实例对应的asset
    /// </summary>
    private Dictionary<int, AssetObject> _goInstanceIDList;

    /// <summary>
    /// 加载的间隔时间
    /// </summary>
    private int _loadingIntervalCount;

    private AssetsLoadMgr()
    {
        this._loadingList = new Dictionary<string, AssetObject>();
        this._loadedList = new Dictionary<string, AssetObject>();
        this._unloadList = new Dictionary<string, AssetObject>();
        this._loadedAsyncList = new List<AssetObject>();
        this._preloadedAsyncList = new Queue<PreloadAssetObject>();
        this._goInstanceIDList = new Dictionary<int, AssetObject>();
    }

    /// <summary>
    /// 判断资源是否存在，对打入atlas的图片无法判断，图片请用AtlasLoadMgr
    /// </summary>
    /// <param name="assetBundleName">ab包名称</param>
    /// <param name="assetName">资源名称</param>
    /// <returns></returns>
    public bool IsAssetExist(string assetBundleName, string assetName)
    {
#if UNITY_EDITOR && !TEST_AB
        return EditorAssetLoadMgr.Instance.IsFileExist(assetName);
#else
        if(ResourcesLoadMgr.Instance.IsFileExist(assetName))
        {
            return true;
        }
        else if(AssetBundleLoadMgr.Instance.IsABExist(assetBundleName))
        {
            return true;
        }
        return false;
#endif
    }

    /// <summary>
    /// 预加载
    /// </summary>
    /// <param name="assetName">资源名称</param>
    /// <param name="isWeak">isWeak弱引用，true为使用过后会销毁，为false将不会销毁，慎用</param>
    public void PreLoad(string assetBundleName, string assetName, bool isWeak = true)
    {
        AssetObject assetObject = null;
        if(this._loadedList.ContainsKey(assetName))
        {
            assetObject = this._loadedList[assetName];
        }
        else if(this._loadingList.ContainsKey(assetName))
        {
            assetObject = this._loadingList[assetName];
        }

        //如果已经存在，改变其弱引用关系
        if(assetObject != null)
        {
            assetObject._isWeak = isWeak;
            if(isWeak && assetObject._refCount == 0 && _unloadList.ContainsKey(assetName) == false)
            {
                this._unloadList.Add(assetName, assetObject);
            }
            return;
        }

        PreloadAssetObject plAssetObj = new PreloadAssetObject();
        plAssetObj._assetBundleName = assetBundleName;
        plAssetObj._assetName = assetName;
        plAssetObj._isWeak = isWeak;

        this._preloadedAsyncList.Enqueue(plAssetObj);
    }

    /// <summary>
    /// 同步加载，一般用于小型文件，比如配置
    /// </summary>
    /// 
    ///     - 1 - 已经加载完成 -> 直接返回缓存结果
    ///     
    ///     - 2 - 正在加载
    ///             --- LoadAB   阶段 -> ab异步转同步加载
    ///             --- LoadAsset阶段 -> request异步转同步加载
    ///     
    ///     - 3 - 没加载过 -> 进行同步加载
    ///     
    /// <param name="assetName">资源名称</param>
    /// <returns></returns>
    public UnityEngine.Object LoadSync(string assetBundleName, string assetName)
    {
        if(!IsAssetExist(assetBundleName, assetName))
        {
            Utils.LogError("AssetsLoadMgr Asset Not Exist " + assetName);
            return null;
        }

        AssetObject assetObj = null;
        if (this._loadedList.ContainsKey(assetName))
        {
            assetObj = this._loadedList[assetName];
            assetObj._refCount++;
            return assetObj._asset;
        }
        else if (this._loadingList.ContainsKey(assetName))
        {
            assetObj = this._loadingList[assetName];

            if (assetObj._request != null)
            {
                if(assetObj._request is EditorResourceRequest)
                {
                    assetObj._asset = (assetObj._request as EditorResourceRequest).asset;
                }
                else if (assetObj._request is ResourceRequest)
                {
                    assetObj._asset = (assetObj._request as ResourceRequest).asset;
                }
                else
                {
                    assetObj._asset = (assetObj._request as AssetBundleRequest).asset;//直接取，会异步变同步
                }
                assetObj._request = null;
            }
            else
            {
#if UNITY_EDITOR && !TEST_AB
                assetObj._asset = EditorAssetLoadMgr.Instance.LoadSync(assetName);
#else
                if (assetObj._isABLoad)
                {
                    AssetBundle ab1 = AssetBundleLoadMgr.Instance.LoadSync(assetBundleName);
                    assetObj._asset = ab1.LoadAsset(this.GetAssetNameForAB(assetName));

                    //异步转同步，需要卸载异步的引用计数
                    AssetBundleLoadMgr.Instance.Unload(assetBundleName);
                }
                else
                {
                    assetObj._asset = ResourcesLoadMgr.Instance.LoadSync(assetName);
                }
#endif
            }

            if(assetObj._asset == null)
            {
                //提取的资源失败，从加载列表删除
                this._loadingList.Remove(assetObj._assetName);
                Utils.LogError("AssetsLoadMgr assetObj._asset Null " + assetObj._assetName);
                return null;
            }

            assetObj._instanceID = assetObj._asset.GetInstanceID();
            this._goInstanceIDList.Add(assetObj._instanceID, assetObj);

            this._loadingList.Remove(assetObj._assetName);
            this._loadedList.Add(assetObj._assetName, assetObj);
            this._loadedAsyncList.Add(assetObj);//原先异步加载的，加入异步表

            assetObj._refCount++;

            return assetObj._asset;
        }

        assetObj = new AssetObject();
        assetObj._assetBundleName = assetBundleName;
        assetObj._assetName = assetName;

#if UNITY_EDITOR && !TEST_AB
        assetObj._asset = EditorAssetLoadMgr.Instance.LoadSync(assetName);
#else
        if(ResourcesLoadMgr.Instance.IsFileExist(assetName))
        {
            assetObj._isABLoad = false;
            assetObj._asset = ResourcesLoadMgr.Instance.LoadSync(assetName);
        }
        else if (AssetBundleLoadMgr.Instance.IsABExist(assetBundleName))
        {
            assetObj._isABLoad = true;
            Utils.LogWarning(string.Format("AssetsLoadMgr LoadSync use AssetBundleLoadMgr assetBundleName:{0} assetname:{1}", assetObj._assetBundleName, assetObj._assetName));
            AssetBundle ab1 = AssetBundleLoadMgr.Instance.LoadSync(assetBundleName);
            assetObj._asset = ab1.LoadAsset(this.GetAssetNameForAB(assetName));
        }
#endif
        if(assetObj._asset == null)
        {
            //提取的资源失败，从加载列表删除
            Utils.LogError("AssetsLoadMgr assetObj._asset Null " + assetObj._assetName);
            return null;
        }

        assetObj._instanceID = assetObj._asset.GetInstanceID();
        this._goInstanceIDList.Add(assetObj._instanceID, assetObj);

        this._loadedList.Add(assetName, assetObj);
        assetObj._refCount = 1;
        return assetObj._asset;
    }

    /// <summary>
    /// 异步加载 即使资源已经加载完成 也会异步回调
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="callFun"></param>
    public void LoadAsync(string assetBundleName, string assetName, AssetsLoadCallback callFun)
    {
        if(!IsAssetExist(assetBundleName, assetName))
        {
            Utils.LogError("AssetsLoadMgr Asset Not Exist " + assetName);
            return;
        }

        AssetObject assetObj = null;
        if(this._loadedList.ContainsKey(assetName))
        {
            Utils.Log("AssetsLoadMgr::LoadAsync assetName:{" + assetName + "} 已经加载,无需再加载");
            assetObj = this._loadedList[assetName];
            assetObj._callbackList.Add(callFun);
            this._loadedAsyncList.Add(assetObj);
            return;
        }
        else if(this._loadingList.ContainsKey(assetName))
        {
            Utils.Log("AssetsLoadMgr::LoadAsync assetName:{" + assetName + "} 加载中,只添加 [加载完成回调函数] 即可");
            assetObj = this._loadingList[assetName];
            assetObj._callbackList.Add(callFun);
            Utils.Log("AssetsLoadMgr::LoadAsync assetName:{" + assetName + "} [加载完成回调函数] 数量:" + assetObj._callbackList.Count.ToString());
            return;
        }

        assetObj = new AssetObject();
        assetObj._assetBundleName = assetBundleName;
        assetObj._assetName = assetName;
        assetObj._callbackList.Add(callFun);

#if UNITY_EDITOR && !TEST_AB
        this._loadingList.Add(assetName, assetObj);
        assetObj._request = EditorAssetLoadMgr.Instance.LoadAsync(assetName);
#else
        if (ResourcesLoadMgr.Instance.IsFileExist(assetName))
        {
            assetObj._isABLoad = false;
            this._loadingList.Add(assetName, assetObj);
            assetObj._request = ResourcesLoadMgr.Instance.LoadAsync(assetName);
        }
        else if (AssetBundleLoadMgr.Instance.IsABExist(assetBundleName))
        {
            assetObj._isABLoad = true;
            this._loadingList.Add(assetName, assetObj);

            AssetBundleLoadMgr.Instance.LoadAsync(assetBundleName, (AssetBundle _ab) =>
            {
                if(_ab == null)
                {
                    string errormsg = string.Format("LoadAssetBundle request error ! assetBundleName:{0} assetName:{1}",assetObj._assetBundleName, assetObj._assetName);
                    Utils.LogError(errormsg);
                    this._loadingList.Remove(assetName);
                    //重新添加 保证成功
                    for(int i = 0; i < assetObj._callbackList.Count; ++i)
                    {
                        this.LoadAsync(assetObj._assetBundleName, assetObj._assetName, assetObj._callbackList[i]);
                    }
                    return;
                }

                if(this._loadingList.ContainsKey(assetName) && assetObj._request == null && assetObj._asset == null)
                {
                    assetObj._request = _ab.LoadAssetAsync(this.GetAssetNameForAB(assetName));
                }
            });
        }
#endif
    }

    /// <summary>
    /// 资源销毁 请保证资源销毁都要调用这个接口
    /// </summary>
    /// <param name="_obj">要销毁的资源 UnityEngine.Object</param>
    public void Unload(UnityEngine.Object _obj)
    {
        if (_obj == null) return;

        int instanceID = _obj.GetInstanceID();

        if (!this._goInstanceIDList.ContainsKey(instanceID))
        {
            //非本地创建的资源，直接销毁即可
            if (_obj is GameObject)
            {
                UnityEngine.Object.Destroy(_obj);
            }
#if UNITY_EDITOR
            else if (UnityEditor.EditorApplication.isPlaying)
            {
                Utils.LogError("AssetsLoadMgr destroy NoGameObject name=" + _obj.name + " type=" + _obj.GetType().Name);
            }
#else
            else
            {
                Utils.LogError("AssetsLoadMgr destroy NoGameObject name=" + _obj.name + " type=" + _obj.GetType().Name);
            }
#endif
            return;
        }

        AssetObject assetObj = this._goInstanceIDList[instanceID];
        if (assetObj._instanceID == instanceID)
        {
            assetObj._refCount--;
        }
        else
        {
            //Error
            string errormsg = string.Format("AssetsLoadMgr Destroy error ! assetName:{0}", assetObj._assetName);
            Utils.LogError(errormsg);
            return;
        }

        if (assetObj._refCount < 0)
        {
            //Error
            string errormsg = string.Format("AssetsLoadMgr Destroy refCount error ! assetName:{0}", assetObj._assetName);
            Utils.LogError(errormsg);
            return;
        }

        if (assetObj._refCount == 0 && !this._unloadList.ContainsKey(assetObj._assetName))
        {
            assetObj._unloadTick = UNLOAD_DELAY_TICK_BASE + this._unloadList.Count;
            this._unloadList.Add(assetObj._assetName, assetObj);
        }
    }

    /// <summary>
    /// 外部加载的资源 加入资源管理 给其他地方调用
    /// </summary>
    /// <param name="_assetName"></param>
    /// <param name="_asset"></param>
    public void AddAsset(string _assetName, UnityEngine.Object _asset)
    {
        AssetObject assetObj = new AssetObject();
        assetObj._assetName = _assetName;

        assetObj._instanceID = _asset.GetInstanceID();
        assetObj._asset = _asset;
        assetObj._refCount = 1;

        this._loadedList.Add(_assetName, assetObj);
        this._goInstanceIDList.Add(assetObj._instanceID, assetObj);
    }

    /// <summary>
    /// 针对特定资源需要添加引用计数 保证引用计数正确
    /// </summary>
    /// <param name="_assetName"></param>
    public void AddAssetRef(string _assetName)
    {
        if(!this._loadedList.ContainsKey(_assetName))
        {
            Utils.LogError("AssetsLoadMgr AddAssetRef Error " + _assetName);
            return;
        }

        AssetObject assetObj = this._loadedList[_assetName];
        assetObj._refCount++;
    }

    /// <summary>
    /// 用于解绑回调
    /// </summary>
    /// <param name="_assetName"></param>
    /// <param name="_callFun"></param>
    public void RemoveCallBack(string _assetName, AssetsLoadCallback _callFun)
    {
        if (_callFun == null) return;
        if (string.IsNullOrEmpty(_assetName))
        {
            this.RemoveCallBackByCallBack(_callFun);
        }

        AssetObject assetObj = null;
        if (this._loadedList.ContainsKey(_assetName))
        {
            assetObj = this._loadedList[_assetName];
        }
        else if (this._loadingList.ContainsKey(_assetName))
        {
            assetObj = this._loadingList[_assetName];
        }

        if (assetObj != null)
        {
            int index = assetObj._callbackList.IndexOf(_callFun);
            if (index >= 0)
            {
                assetObj._callbackList.RemoveAt(index);
            }
        }
    }

    /// <summary>
    /// 从AssetBundle取资源的资源名(不含扩展名 不含路径)
    /// </summary>
    /// <param name="assetName"></param>
    /// <returns></returns>
    private string GetAssetNameForAB(string assetName)
    {
        assetName = assetName.Substring(0, assetName.LastIndexOf("."));
        string temp = assetName.Substring(0, assetName.LastIndexOf("/") + 1);
        assetName = assetName.Replace(temp, "");
        return assetName;
    }

    private void RemoveCallBackByCallBack(AssetsLoadCallback _callFun)
    {
        foreach(var assetObj in this._loadingList.Values)
        {
            if (assetObj._callbackList.Count == 0) continue;
            int index = assetObj._callbackList.IndexOf(_callFun);
            if (index >= 0)
            {
                assetObj._callbackList.RemoveAt(index);
            }
        }
        
        foreach(var assetObj in this._loadedList.Values)
        {
            if (assetObj._callbackList.Count == 0) continue;
            int index = assetObj._callbackList.IndexOf(_callFun);
            if (index >= 0)
            {
                assetObj._callbackList.RemoveAt(index);
            }
        }
    }

    private void DoAssetCallback(AssetObject _assetObj)
    {
        if (_assetObj._callbackList.Count == 0) return;

        int count = _assetObj._lockCallbackCount;
        for(int i = 0; i < count; ++i)
        {
            if (_assetObj._callbackList[i] != null)
            {
                _assetObj._refCount++; //每次回调 引用计数+1

                try
                {
                    _assetObj._callbackList[i](_assetObj._assetName, _assetObj._asset);
                }
                catch(System.Exception e)
                {
                    Utils.LogError(e.Message);
                }
            }
        }
        _assetObj._callbackList.RemoveRange(0, count);
    }

    private void DoUnload(AssetObject _assetObj)
    {
#if UNITY_EDITOR && !TEST_AB
        EditorAssetLoadMgr.Instance.Unload(_assetObj._asset);
#else
        if (_assetObj._isABLoad)
        {
            AssetBundleLoadMgr.Instance.Unload(_assetObj._assetBundleName);
        }
        else
        {
            ResourcesLoadMgr.Instance.Unload(_assetObj._asset);
        }
#endif
        _assetObj._asset = null;

        if (this._goInstanceIDList.ContainsKey(_assetObj._instanceID))
        {
            this._goInstanceIDList.Remove(_assetObj._instanceID);
        }
    }

    private void UpdateLoadedAsync()
    {
        if(this._loadedAsyncList.Count == 0)
        {
            return;
        }

        int count = this._loadedAsyncList.Count;
        for(int i = 0; i < count; ++i)
        {
            this._loadedAsyncList[i]._lockCallbackCount = this._loadedAsyncList[i]._callbackList.Count;
        }
        for(int i = 0; i < count; ++i)
        {
            this.DoAssetCallback(this._loadedAsyncList[i]);
        }
        this._loadedAsyncList.RemoveRange(0, count);

        if(this._loadingList.Count == 0 && this._loadingIntervalCount > LOADING_INTERVAL_MAX_COUNT)
        {
            //在连续的大量加载后，强制调用一次gc
            this._loadingIntervalCount = 0;
            //Resources.UnloadUnusedAssets();
            //System.GC.Collect();
        }
    }

    /// <summary>
    /// 每帧检测加载完成的资源
    /// 
    /// 第一个遍历是提取
    /// 第二个遍历是改变队列
    /// 第三个遍历是回调
    /// </summary>
    private void UpdateLoading()
    {
        if(this._loadingList.Count == 0)
        {
            return;
        }

        this.tempLoadeds.Clear();
        foreach(var assetObj in this._loadingList.Values)
        {
#if UNITY_EDITOR && !TEST_AB
            if(assetObj._request != null && (assetObj._request as EditorResourceRequest).isDone)
            {
                assetObj._asset = (assetObj._request as EditorResourceRequest).asset;

                if(assetObj._asset == null)
                {
                    //提取的资源失败，从加载列表删除
                    this._loadingList.Remove(assetObj._assetName);
                    Utils.LogError("AssetsLoadMgr assetObj._asset Null " + assetObj._assetName);
                    break;
                }

                assetObj._instanceID = assetObj._asset.GetInstanceID();
                this._goInstanceIDList.Add(assetObj._instanceID, assetObj);
                assetObj._request = null;
                tempLoadeds.Add(assetObj);
            }
#else
            if (assetObj._request != null && assetObj._request.isDone)
            {
                //加载完进行数据清理
                if(assetObj._request is AssetBundleRequest)
                {
                    assetObj._asset = (assetObj._request as AssetBundleRequest).asset;
                }
                else
                {
                    assetObj._asset = (assetObj._request as ResourceRequest).asset;
                }

                if(assetObj._asset == null)
                {
                    //提取的资源失败，从加载列表删除
                    this._loadingList.Remove(assetObj._assetName);
                    Utils.LogError("AssetsLoadMgr assetObj._asset Null " + assetObj._assetName);
                    break;
                }

                assetObj._instanceID = assetObj._asset.GetInstanceID();
                this._goInstanceIDList.Add(assetObj._instanceID, assetObj);
                assetObj._request = null;
                tempLoadeds.Add(assetObj);
            }
#endif
        }

        //回调中有可能对_loadingList进行操作，先移动
        foreach (var assetObj in tempLoadeds)
        {
            this._loadingList.Remove(assetObj._assetName);
            this._loadedList.Add(assetObj._assetName, assetObj);
            this._loadingIntervalCount++; //统计本轮加载的数量

            //先锁定回调数量，保证异步成立
            assetObj._lockCallbackCount = assetObj._callbackList.Count;
        }
        foreach (var assetObj in tempLoadeds)
        {
            this.DoAssetCallback(assetObj);
        }
    }

    private void UpdateUnload()
    {
        if(this._unloadList.Count == 0)
        {
            return;
        }

        this.tempLoadeds.Clear();
        foreach(var assetObj in this._unloadList.Values)
        {
            if(assetObj._isWeak && assetObj._refCount == 0 && assetObj._callbackList.Count == 0)
            {
                //引用计数为0，且没有需要回调的函数，销毁
                if (assetObj._unloadTick < 0)
                {
                    this._loadedList.Remove(assetObj._assetName);
                    this.DoUnload(assetObj);

                    this.tempLoadeds.Add(assetObj);
                }
                else
                {
                    assetObj._unloadTick--;
                }
            }

            if(assetObj._refCount > 0 || !assetObj._isWeak)
            {
                //引用计数增加（销毁期间有加载）
                this.tempLoadeds.Add(assetObj);
            }
        }

        foreach(var assetObj in tempLoadeds)
        {
            this._unloadList.Remove(assetObj._assetName);
        }
    }

    private void UpdatePreload()
    {
        //加载队列空闲才需要预加载
        if (this._loadingList.Count > 0 || this._preloadedAsyncList.Count == 0)
        {
            return;
        }

        //从队列取出一个，异步加载
        PreloadAssetObject plAssetObj = null;
        while(this._preloadedAsyncList.Count > 0 && plAssetObj == null)
        {
            plAssetObj = this._preloadedAsyncList.Dequeue();

            if(this._loadingList.ContainsKey(plAssetObj._assetName))
            {
                this._loadingList[plAssetObj._assetName]._isWeak = plAssetObj._isWeak;
            }
            else if(this._loadedList.ContainsKey(plAssetObj._assetName))
            {
                this._loadedList[plAssetObj._assetName]._isWeak = plAssetObj._isWeak;
                plAssetObj = null;  //如果当前没开始加载，重新选一个
            }
            else
            {
                this.LoadAsync(plAssetObj._assetBundleName, plAssetObj._assetName, (AssetsLoadCallback)null);
                if (this._loadingList.ContainsKey(plAssetObj._assetName))
                {
                    this._loadingList[plAssetObj._assetName]._isWeak = plAssetObj._isWeak;
                }
                else if (this._loadedList.ContainsKey(plAssetObj._assetName))
                {
                    this._loadedList[plAssetObj._assetName]._isWeak = plAssetObj._isWeak;
                }
            }
        }
    }

    public void Update()
    {
        this.UpdatePreload();       //预加载 空闲时启动
        this.UpdateLoadedAsync();   //已经加载的异步回调
        this.UpdateLoading();       //加载完成回调
        this.UpdateUnload();        //卸载需要销毁的资源

#if UNITY_EDITOR && !TEST_AB
        EditorAssetLoadMgr.Instance.Update();
#else
        AssetBundleLoadMgr.Instance.Update();
#endif
    }
}