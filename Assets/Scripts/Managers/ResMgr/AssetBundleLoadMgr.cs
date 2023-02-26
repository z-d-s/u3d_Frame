/****************************************************

	AssetBundle - 资源以压缩包文件存在 
                - (Resources目录下的资源打成包体后也是以ab格式存在)

    AssetBundle加载有三套接口：
            -- WWW(已弃用)
            -- UnityWebRequest 
                    将整个文件的二进制流下载或读取到内存中
                    然后对这段内存文件进行ab资源的读取解析操作
            -- AssetBundle
                    只读取存储于本地的ab文件头部部分
                    在需要的情况下再去读取ab中的数据段部分(Asset资源)
                    优势：1.不进行下载(不占用下载缓存区内存)
                            2.不读取整个文件到内存(不占用原始文件二进制内存)
                            3.读取非压缩或LZ4的ab，只用读取ab的头文件(约5kb/个)
                            4.同步异步加载并行可用

    外部接口：
            -- LoadManifest     加载依赖关系
            -- LoadSync         同步加载
            -- LoadAsync        异步加载
            -- Unload           卸载
            -- Update           刷新

*****************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AssetBundleLoadMgr : MonoBaseSingleton<AssetBundleLoadMgr>
{
    public delegate void AssetBundleLoadCallBack(AssetBundle ab);

    private class AssetBundleObject
    {
        /// <summary>
        /// hash标识符
        /// </summary>
        public string _hashName;

        /// <summary>
        /// 引用计数
        /// </summary>
        public int _refCount;
        /// <summary>
        /// 回调函数列表
        /// </summary>
        public List<AssetBundleLoadCallBack> _callFunList = new List<AssetBundleLoadCallBack>();

        /// <summary>
        /// 异步加载请求
        /// </summary>
        public AssetBundleCreateRequest _request;
        /// <summary>
        /// 加载到的ab
        /// </summary>
        public AssetBundle _ab;

        /// <summary>
        /// 依赖计数
        /// </summary>
        public int _dependLoadingCount;
        /// <summary>
        /// 依赖项
        /// </summary>
        public List<AssetBundleObject> _depends = new List<AssetBundleObject>();
    }

    /// <summary>
    /// 同时加载的最大数量
    /// </summary>
    private const int MAX_LOADING_COUNT = 10;

    /// <summary>
    /// 创建临时存储变量 用于提升性能
    /// </summary>
    private List<AssetBundleObject> tempLoadeds = new List<AssetBundleObject>();

    private Dictionary<string, string[]> _dependsDataList;

    /// <summary>
    /// 准备加载的列表
    /// </summary>
    private Dictionary<string, AssetBundleObject> _readyABList;
    /// <summary>
    /// 正在加载的列表
    /// </summary>
    private Dictionary<string, AssetBundleObject> _loadingABList;
    /// <summary>
    /// 已经加载完成的列表
    /// </summary>
    private Dictionary<string, AssetBundleObject> _loadedABList;
    /// <summary>
    /// 准备卸载的列表
    /// </summary>
    private Dictionary<string, AssetBundleObject> _unloadABList;

    private AssetBundleLoadMgr()
    {
        this._dependsDataList = new Dictionary<string, string[]>();

        this._readyABList = new Dictionary<string, AssetBundleObject>();
        this._loadingABList = new Dictionary<string, AssetBundleObject>();
        this._loadedABList = new Dictionary<string, AssetBundleObject>();
        this._unloadABList = new Dictionary<string, AssetBundleObject>();
    }

    public void LoadManifest()
    {
        //TODO::
        //string path = FileVersionMgr.Instance.GetFilePathByExist("Assets");
        //string path = FileHelper.GetResPath() + FileHelper.ABManiName;
        string path = FileHelper.BaseLocalResPath() + FileHelper.ABManiName + "/" + FileHelper.ABManiName;
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        this._dependsDataList.Clear();
        AssetBundle ab = AssetBundle.LoadFromFile(path);

        if(ab == null)
        {
            string errormsg = string.Format("LoadMainfest ab NULL error !");
            Utils.LogError(errormsg);
            return;
        }

        AssetBundleManifest manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        if(manifest == null)
        {
            string errormsg = string.Format("LoadMainfest NULL error !");
            Utils.LogError(errormsg);
            return;
        }

        foreach(string assetName in manifest.GetAllAssetBundles())
        {
            string hashName = assetName.Replace(".ab", "");
            string[] dps = manifest.GetAllDependencies(assetName);
            for(int i = 0; i < dps.Length; ++i)
            {
                dps[i] = dps[i].Replace(".ab", "");
            }
            this._dependsDataList.Add(hashName, dps);
        }

        ab.Unload(true);
        ab = null;

        Utils.Log("AssetBundleLoadMgr dependsCount=" + _dependsDataList.Count);
    }

    private string GetHashName(string _assetName)
    {
        //开发人员可以自己定义hash方式，对内存有要求的话，可以hash成uint（或uint64）节省内存
        return _assetName.ToLower();
    }

    private string GetFileName(string _hashName)
    {
        //开发人员可以自己实现自己的对应关系
        //return _hashName + ".ab";
        return _hashName;
    }

    /// <summary>
    /// 获取一个资源的路径
    /// </summary>
    /// <param name="_hashName"></param>
    /// <returns></returns>
    private string GetAssetBundlePath(string _hashName)
    {
        //TODO::
        //开发人员可以自己实现的对应关系，笔者这里有多语言和文件版本的处理
        //string lngHashName = this.GetHashName(LocalizationMgr.Instance.GetAssetPrefix() + _hashName);
        string lngHashName = this.GetHashName(_hashName);
        if(this._dependsDataList.ContainsKey(lngHashName))
        {
            _hashName = lngHashName;
        }
        //TODO::
        //return FileVersionMgr.Instance.GetFilePathByExist(this.GetFileName(_hashName));
        //return FileHelper.GetResPath() + this.GetFileName(_hashName);
        return FileHelper.BaseLocalResPath() + FileHelper.ABManiName + "/" + this.GetFileName(_hashName);
    }

    public bool IsABExist(string _assetName)
    {
        string hashName = this.GetHashName(_assetName);
        return this._dependsDataList.ContainsKey(hashName);
    }

    /// <summary>
    /// 同步加载
    /// </summary>
    /// <param name="_assetName"></param>
    /// <returns></returns>
    public AssetBundle LoadSync(string _assetName)
    {
        string hashName = this.GetHashName(_assetName);
        AssetBundleObject abObj = this.LoadAssetBundleSync(hashName);
        return abObj._ab;
    }

    /// <summary>
    /// 异步加载（已经加载直接回调），每次加载引用计数+1
    /// </summary>
    /// <param name="_assetName"></param>
    /// <param name="callFun"></param>
    public void LoadAsync(string _assetName, AssetBundleLoadCallBack callFun)
    {
        string hashName = this.GetHashName(_assetName);
        this.LoadAssetBundleAsync(hashName, callFun);
    }

    /// <summary>
    /// 卸载（异步），每次卸载引用计数-1
    /// </summary>
    /// <param name="_assetName"></param>
    public void Unload(string _assetName)
    {
        string hashName = this.GetHashName(_assetName);
        this.UnloadAssetBundleAsync(hashName);
    }

    private AssetBundleObject LoadAssetBundleSync(string _hashName)
    {
        AssetBundleObject abObj = null;
        if(this._loadedABList.ContainsKey(_hashName))   //已经加载
        {
            abObj = this._loadedABList[_hashName];
            abObj._refCount++;

            foreach(var dpObj in abObj._depends)
            {
                //递归依赖项，附加引用计数
                this.LoadAssetBundleSync(dpObj._hashName);
            }

            return abObj;
        }
        else if(this._loadingABList.ContainsKey(_hashName)) //在加载中 异步改同步
        {
            abObj = this._loadingABList[_hashName];
            abObj._refCount++;

            foreach(var dpObj in abObj._depends)
            {
                this.LoadAssetBundleSync(dpObj._hashName);  //递归依赖项 加载完
            }

            this.DoLoadedCallFun(abObj, false); //强制完成 回调

            return abObj;
        }
        else if(this._readyABList.ContainsKey(_hashName))   //在准备加载中
        {
            abObj = this._readyABList[_hashName];
            abObj._refCount++;

            foreach(var dpObj in abObj._depends)
            {
                this.LoadAssetBundleSync(dpObj._hashName);  //递归依赖项 加载完
            }

            string path1 = this.GetAssetBundlePath(_hashName);
            abObj._ab = AssetBundle.LoadFromFile(path1);

            this._readyABList.Remove(abObj._hashName);
            this._loadedABList.Add(abObj._hashName, abObj);

            this.DoLoadedCallFun(abObj, false); //强制完成 回调

            return abObj;
        }

        abObj = new AssetBundleObject();
        abObj._hashName = _hashName;
        abObj._refCount = 1;

        string path = this.GetAssetBundlePath(_hashName);
        abObj._ab = AssetBundle.LoadFromFile(path);

        if(abObj._ab == null)
        {
            try
            {
                //同步下载解决
                byte[] bytes = AssetsDownloadMgr.Instance.DownloadSync(this.GetFileName(abObj._hashName));
                if (bytes != null && bytes.Length != 0)
                    abObj._ab = AssetBundle.LoadFromMemory(bytes);
            }
            catch (Exception ex)
            {
                Utils.LogError("LoadAssetBundleSync DownloadSync" + ex.Message);
            }
        }

        //加载依赖项
        string[] dependsData = null;
        if(this._dependsDataList.ContainsKey(_hashName))
        {
            dependsData = this._dependsDataList[_hashName];
        }

        if(dependsData != null && dependsData.Length > 0)
        {
            abObj._dependLoadingCount = 0;

            foreach(var dpAssetName in dependsData)
            {
                var dpObj = this.LoadAssetBundleSync(dpAssetName);
                abObj._depends.Add(dpObj);
            }
        }

        this._loadedABList.Add(abObj._hashName, abObj);

        return abObj;
    }

    private void UnloadAssetBundleAsync(string _hashName)
    {
        AssetBundleObject abObj = null;
        if(this._loadedABList.ContainsKey(_hashName))
        {
            abObj = this._loadedABList[_hashName];
        }
        else if(this._loadingABList.ContainsKey(_hashName))
        {
            abObj = this._loadingABList[_hashName];
        }
        else if(this._readyABList.ContainsKey(_hashName))
        {
            abObj = this._readyABList[_hashName];
        }

        if(abObj == null)
        {
            string errormsg = string.Format("UnLoadAssetbundle error ! assetName:{0}", _hashName);
            Utils.LogError(errormsg);
            return;
        }

        if(abObj._refCount == 0)
        {
            string errormsg = string.Format("UnLoadAssetbundle refCount error ! assetName:{0}", _hashName);
            Utils.LogError(errormsg);
            return;
        }

        abObj._refCount--;

        foreach(var dpObj in abObj._depends)
        {
            this.UnloadAssetBundleAsync(dpObj._hashName);
        }

        if(abObj._refCount == 0)
        {
            this._unloadABList.Add(abObj._hashName, abObj);
        }
    }

    private AssetBundleObject LoadAssetBundleAsync(string _hashName, AssetBundleLoadCallBack _callFun)
    {
        AssetBundleObject abObj = null;
        if(this._loadedABList.ContainsKey(_hashName))           //已经加载
        {
            abObj = this._loadedABList[_hashName];
            this.DoDependsRef(abObj);
            _callFun(abObj._ab);
            return abObj;
        }
        else if(this._loadingABList.ContainsKey(_hashName))     //加载中
        {
            abObj = this._loadingABList[_hashName];
            this.DoDependsRef(abObj);
            abObj._callFunList.Add(_callFun);
            return abObj;
        }
        else if(this._readyABList.ContainsKey(_hashName))       //在准备加载中
        {
            abObj = this._readyABList[_hashName];
            this.DoDependsRef(abObj);
            abObj._callFunList.Add(_callFun);
            return abObj;
        }

        //创建一个加载
        abObj = new AssetBundleObject();
        abObj._hashName = _hashName;
        abObj._refCount = 1;
        abObj._callFunList.Add(_callFun);

        //加载依赖项
        string[] dependsData = null;
        if(this._dependsDataList.ContainsKey(_hashName))
        {
            dependsData = this._dependsDataList[_hashName];
        }

        if(dependsData != null && dependsData.Length > 0)
        {
            abObj._dependLoadingCount = dependsData.Length;

            foreach(var dpAssetName in dependsData)
            {
                AssetBundleObject dpObj = this.LoadAssetBundleAsync(dpAssetName, (AssetBundle _ab) =>
                {
                    if (abObj._dependLoadingCount <= 0)
                    {
                        string errormsg = string.Format("LoadAssetbundle depend error ! assetName:{0}", _hashName);
                        Utils.LogError(errormsg);
                        return;
                    }

                    abObj._dependLoadingCount--;

                    //依赖加载完
                    if(abObj._dependLoadingCount == 0 && abObj._request != null && abObj._request.isDone)
                    {
                        this.DoLoadedCallFun(abObj);
                    }
                });
                abObj._depends.Add(dpObj);
            }
        }

        //正在加载的数量不能超过上限
        if (this._loadingABList.Count < MAX_LOADING_COUNT)
        {
            this.DoLoad(abObj);

            this._loadingABList.Add(_hashName, abObj);
        }
        else
        {
            this._readyABList.Add(_hashName, abObj);
        }
        return abObj;
    }

    private void DoDependsRef(AssetBundleObject abObj)
    {
        abObj._refCount++;

        if (abObj._depends.Count == 0) return;
        foreach (var dpObj in abObj._depends)
        {
            this.DoDependsRef(dpObj); //递归依赖项，加载完
        }
    }

    private void DoLoad(AssetBundleObject abObj)
    {
        if (AssetsDownloadMgr.Instance.IsNeedDownload(this.GetFileName(abObj._hashName)))
        {
            //这里是关联下载逻辑，可以实现异步下载再异步加载
            AssetsDownloadMgr.Instance.DownloadAsync(this.GetFileName(abObj._hashName), () =>
            {
                string path = this.GetAssetBundlePath(abObj._hashName);
                abObj._request = AssetBundle.LoadFromFileAsync(path);

                if (abObj._request == null)
                {
                    string errormsg = string.Format("LoadAssetbundle path error ! assetName:{0}", abObj._hashName);
                    Utils.LogError(errormsg);
                }
            });
        }
        else
        {
            string path = this.GetAssetBundlePath(abObj._hashName);
            abObj._request = AssetBundle.LoadFromFileAsync(path);

            if (abObj._request == null)
            {
                string errormsg = string.Format("LoadAssetbundle path error ! assetName:{0}", abObj._hashName);
                Utils.LogError(errormsg);
            }
        }
    }

    private void DoLoadedCallFun(AssetBundleObject abObj, bool isAsync = true)
    {
        //提取ab
        if (abObj._request != null)
        {
            abObj._ab = abObj._request.assetBundle; //如果没加载完，会异步转同步
            abObj._request = null;
            _loadingABList.Remove(abObj._hashName);
            _loadedABList.Add(abObj._hashName, abObj);
        }

        if (abObj._ab == null)
        {
            string errormsg = string.Format("LoadAssetbundle _ab null error ! assetName:{0}", abObj._hashName);
            string path = this.GetAssetBundlePath(abObj._hashName);
            errormsg += "\n File " + File.Exists(path) + " Exists " + path;

            try
            {//尝试读取二进制解决
                if (File.Exists(path))
                {
                    byte[] bytes = File.ReadAllBytes(path);
                    if (bytes != null && bytes.Length != 0)
                        abObj._ab = AssetBundle.LoadFromMemory(bytes);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("LoadAssetbundle ReadAllBytes Error " + ex.Message);
            }

            if (abObj._ab == null)
            {
                //同步下载解决
                byte[] bytes = AssetsDownloadMgr.Instance.DownloadSync(this.GetFileName(abObj._hashName));
                if (bytes != null && bytes.Length != 0)
                    abObj._ab = AssetBundle.LoadFromMemory(bytes);

                if (abObj._ab == null)
                {
                    //同步下载还不能解决，移除
                    if (_loadedABList.ContainsKey(abObj._hashName)) _loadedABList.Remove(abObj._hashName);
                    else if (_loadingABList.ContainsKey(abObj._hashName)) _loadingABList.Remove(abObj._hashName);

                    Debug.LogError(errormsg);

                    if (isAsync)
                    {
                        //异步下载解决
                        AssetsDownloadMgr.Instance.AddDownloadSetFlag(this.GetFileName(abObj._hashName));
                    }
                }
            }
        }

        //运行回调
        foreach (var callback in abObj._callFunList)
        {
            callback(abObj._ab);
        }
        abObj._callFunList.Clear();
    }

    private void UpdateLoad()
    {
        if (this._loadingABList.Count == 0)
        {
            return;
        }

        //检测加载完的
        this.tempLoadeds.Clear();
        foreach (var abObj in this._loadingABList.Values)
        {
            if (abObj._dependLoadingCount == 0 && abObj._request != null && abObj._request.isDone)
            {
                this.tempLoadeds.Add(abObj);
            }
        }
        //回调中有可能对_loadingABList进行操作，提取后回调
        foreach (var abObj in this.tempLoadeds)
        {
            //加载完进行回调
            this.DoLoadedCallFun(abObj);
        }
    }

    private void DoUnload(AssetBundleObject abObj)
    {
        //这里用true，卸载Asset内存，实现指定卸载
        if (abObj._ab == null)
        {
            string errormsg = string.Format("LoadAssetbundle DoUnload error ! assetName:{0}", abObj._hashName);
            Utils.LogError(errormsg);
            return;
        }

        abObj._ab.Unload(true);
        abObj._ab = null;
    }

    private void UpdateUnLoad()
    {
        if (this._unloadABList.Count == 0)
        {
            return;
        }

        this.tempLoadeds.Clear();
        foreach (var abObj in this._unloadABList.Values)
        {
            if (abObj._refCount == 0 && abObj._ab != null)
            {
                //引用计数为0并且已经加载完，没加载完等加载完销毁
                this.DoUnload(abObj);
                this._loadedABList.Remove(abObj._hashName);

                this.tempLoadeds.Add(abObj);
            }

            if (abObj._refCount > 0)
            {
                //引用计数加回来（销毁又瞬间重新加载，不销毁，从销毁列表移除）
                this.tempLoadeds.Add(abObj);
            }
        }

        foreach (var abObj in this.tempLoadeds)
        {
            this._unloadABList.Remove(abObj._hashName);
        }
    }

    private void UpdateReady()
    {
        if (this._readyABList.Count == 0)
        {
            return;
        }
        if (this._loadingABList.Count >= MAX_LOADING_COUNT)
        {
            return;
        }

        this.tempLoadeds.Clear();
        foreach (var abObj in this._readyABList.Values)
        {
            this.DoLoad(abObj);

            this.tempLoadeds.Add(abObj);
            this._loadingABList.Add(abObj._hashName, abObj);

            if (this._loadingABList.Count >= MAX_LOADING_COUNT)
            {
                break;
            }
        }

        foreach (var abObj in this.tempLoadeds)
        {
            this._readyABList.Remove(abObj._hashName);
        }
    }

    public void Update()
    {
        this.UpdateLoad();
        this.UpdateReady();
        this.UpdateUnLoad();
    }
}