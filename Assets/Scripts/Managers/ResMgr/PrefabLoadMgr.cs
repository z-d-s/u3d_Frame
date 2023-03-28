/****************************************************

    外部接口：
        -- LoadSync         同步加载
        -- LoadAsync        异步加载
        -- Destroy          销毁
        -- RemoveCallBack
        -- AddAssetRef
        -- Update           刷新

*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class PrefabLoadMgr : MonoBaseSingleton<PrefabLoadMgr>
{
    public delegate void PrefabLoadCallback(string name, GameObject obj);

    public class PrefabObject
    {
        public string _assetName;

        /// <summary>
        /// 记录回调当前数量，保证异步是下一帧回调
        /// </summary>
        public int _lockCallbackCount;
        public List<PrefabLoadCallback> _callbackList = new List<PrefabLoadCallback>();
        public List<Transform> _callParentList = new List<Transform>();

        public UnityEngine.Object _asset;

        /// <summary>
        /// 引用计数
        /// </summary>
        public int _refCount;
        /// <summary>
        /// 实例化的GameObject引用列表 (用于记录当前已经创建和使用的节点ID)
        /// </summary>
        public HashSet<int> _goInstanceIDSet = new HashSet<int>();
    }

    /// <summary>
    /// 已加载的预制体列表
    /// </summary>
    private Dictionary<string, PrefabObject> _loadedList;
    /// <summary>
    /// 异步加载 延迟回调
    /// </summary>
    private List<PrefabObject> _loadedAsyncList;
    /// <summary>
    /// 创建的实例对应的asset
    /// </summary>
    private Dictionary<int, PrefabObject> _goInstanceIDList;
    /// <summary>
    /// 通用父节点
    /// </summary>
    private GameObject _assetParent;

    private PrefabLoadMgr()
    {
        this._loadedList = new Dictionary<string, PrefabObject>();
        this._loadedAsyncList = new List<PrefabObject>();

        this._goInstanceIDList = new Dictionary<int, PrefabObject>();

#if UNITY_EDITOR
        if(UnityEditor.EditorApplication.isPlaying)
        {
            this._assetParent = new GameObject("AssetsList");
            GameObject.DontDestroyOnLoad(this._assetParent);
        }
#else
        this._assetParent = new GameObject("AssetsList");
        GameObject.DontDestroyOnLoad(this._assetParent);
#endif
    }

    private GameObject InstanceAsset(PrefabObject _prefabObj, Transform _parent)
    {
        Transform tempParent = _parent;
        if(_parent == null || _parent.gameObject == null || !_parent.gameObject.activeInHierarchy)
        {
            tempParent = this._assetParent.transform;
        }

        GameObject go = GameObject.Instantiate(_prefabObj._asset, tempParent) as GameObject;
        go.name = go.name.Replace("(Clone)", "");
        int instanceID = go.GetInstanceID();

        ObjInfo objInfo = go.AddComponent<ObjInfo>();

        if(!go.activeSelf)
        {
            //保证GameObject active一次，ObjInfo才能触发Awake，未Awake的脚本不能触发OnDestroy
            go.SetActive(true);
            go.SetActive(false);
        }

        if(objInfo != null)
        {
            objInfo.InstanceId = instanceID;
            objInfo.AssetName = _prefabObj._assetName;
        }

        _prefabObj._goInstanceIDSet.Add(instanceID);
        this._goInstanceIDList.Add(instanceID, _prefabObj);

        if(_parent != null)
        {
            go.transform.SetParent(_parent);
        }

        return go;
    }

    private void DoInstanceAssetCallback(PrefabObject _prefabObj)
    {
        if(_prefabObj._callbackList.Count == 0)
        {
            return;
        }

        //先将回掉提取保存，再回调，保证回调中加载和销毁不出错
        int count = _prefabObj._lockCallbackCount;
        List<PrefabLoadCallback> callbackList = _prefabObj._callbackList.GetRange(0, count);
        List<Transform> callParentList = _prefabObj._callParentList.GetRange(0, count);

        _prefabObj._lockCallbackCount = 0;
        _prefabObj._callbackList.RemoveRange(0, count);
        _prefabObj._callParentList.RemoveRange(0, count);

        for(int i = 0; i < count; ++i)
        {
            if (callbackList[i] != null)
            {
                //prefab需要实例化
                GameObject newObj = this.InstanceAsset(_prefabObj, callParentList[i]);

                try
                {
                    callbackList[i](_prefabObj._assetName, newObj);
                }
                catch(System.Exception e)
                {
                    UtilLog.LogError(e.Message);
                }

                //如果回调之后，节点挂在默认节点下，认为该节点无效，销毁
                if (newObj.transform.parent == this._assetParent.transform)
                {
                    Destroy(newObj);
                }
            }
        }
    }

    public GameObject LoadSync(string assetBundleName, string _assetName, Transform _parent = null)
    {
        PrefabObject prefabObj = null;
        if(this._loadedList.ContainsKey(_assetName))
        {
            prefabObj = this._loadedList[_assetName];
            prefabObj._refCount++;

            if(prefabObj._asset == null)
            {
                //说明在异步加载中,需要不影响异步加载,加载后要释放
                prefabObj._asset = AssetsLoadMgr.Instance.LoadSync(assetBundleName, _assetName);
                var newGo = this.InstanceAsset(prefabObj, _parent);
                AssetsLoadMgr.Instance.Unload(prefabObj._asset);
                prefabObj._asset = null;

                return newGo;
            }
            else
            {
                return this.InstanceAsset(prefabObj, _parent);
            }
        }

        prefabObj = new PrefabObject();
        prefabObj._assetName = _assetName;
        prefabObj._refCount = 1;
        prefabObj._asset = AssetsLoadMgr.Instance.LoadSync(assetBundleName, _assetName);

        this._loadedList.Add(_assetName, prefabObj);
        return this.InstanceAsset(prefabObj, _parent);
    }

    public void LoadAsync(string assetBundleName, string _assetName, PrefabLoadCallback _callFun, Transform _parent = null)
    {
        PrefabObject prefabObj = null;
        if (this._loadedList.ContainsKey(_assetName))
        {
            prefabObj = this._loadedList[_assetName];
            prefabObj._callbackList.Add(_callFun);
            prefabObj._callParentList.Add(_parent);
            prefabObj._refCount++;

            if (prefabObj._asset != null)
            {
                this._loadedAsyncList.Add(prefabObj);
            }
            return;
        }

        prefabObj = new PrefabObject();
        prefabObj._assetName = _assetName;
        prefabObj._callbackList.Add(_callFun);
        prefabObj._callParentList.Add(_parent);
        prefabObj._refCount = 1;

        this._loadedList.Add(_assetName, prefabObj);

        AssetsLoadMgr.Instance.LoadAsync(assetBundleName, _assetName, (string name, UnityEngine.Object obj) =>
        {
            prefabObj._asset = obj;

            prefabObj._lockCallbackCount = prefabObj._callbackList.Count;
            this.DoInstanceAssetCallback(prefabObj);
        });
    }

    public void Destroy(GameObject _obj)
    {
        if(_obj == null)
        {
            return;
        }

        int instanceID = _obj.GetInstanceID();

        if (!this._goInstanceIDList.ContainsKey(instanceID))
        {
            //非从本类创建的资源，直接销毁即可
            if (_obj is GameObject)
            {
                UnityEngine.Object.Destroy(_obj);
            }

#if UNITY_EDITOR
            else if (UnityEditor.EditorApplication.isPlaying)
            {
                UtilLog.LogError("PrefabLoadMgr destroy NoGameObject name=" + _obj.name + " type=" + _obj.GetType().Name);
            }
#else
            else Utils.LogError("PrefabLoadMgr destroy NoGameObject name=" + _obj.name + " type=" + _obj.GetType().Name);
#endif
            return;
        }

        PrefabObject prefabObj = this._goInstanceIDList[instanceID];
        if(prefabObj._goInstanceIDSet.Contains(instanceID))
        {
            //实例化的GameObject
            prefabObj._refCount--;
            prefabObj._goInstanceIDSet.Remove(instanceID);
            this._goInstanceIDList.Remove(instanceID);
            UnityEngine.Object.Destroy(_obj);
        }
        else
        {
            string errormsg = string.Format("PrefabLoadMgr Destroy error ! assetName:{0}", prefabObj._assetName);
            UtilLog.LogError(errormsg);
            return;
        }

        if (prefabObj._refCount < 0)
        {
            string errormsg = string.Format("PrefabLoadMgr Destroy refCount error ! assetName:{0}", prefabObj._assetName);
            UtilLog.LogError(errormsg);
            return;
        }

        if (prefabObj._refCount == 0)
        {
            this._loadedList.Remove(prefabObj._assetName);

            AssetsLoadMgr.Instance.Unload(prefabObj._asset);
            prefabObj._asset = null;
        }
    }

    public void RemoveCallBack(string _assetName, PrefabLoadCallback _callFun)
    {
        if(_callFun == null)
        {
            return;
        }

        PrefabObject prefabObj = null;
        if (this._loadedList.ContainsKey(_assetName))
        {
            prefabObj = _loadedList[_assetName];
        }

        if (prefabObj != null)
        {
            int index = prefabObj._callbackList.IndexOf(_callFun);
            if (index >= 0)
            {
                prefabObj._refCount--;
                prefabObj._callbackList.RemoveAt(index);
                prefabObj._callParentList.RemoveAt(index);

                if (index < prefabObj._lockCallbackCount)
                {
                    //说明是加载回调过程中解绑回调，需要降低lock个数
                    prefabObj._lockCallbackCount--;
                }
            }

            if (prefabObj._refCount < 0)
            {
                string errormsg = string.Format("PrefabLoadMgr Destroy refCount error ! assetName:{0}", prefabObj._assetName);
                UtilLog.LogError(errormsg);
                return;
            }

            if (prefabObj._refCount == 0)
            {
                this._loadedList.Remove(prefabObj._assetName);

                AssetsLoadMgr.Instance.Unload(prefabObj._asset);
                prefabObj._asset = null;
            }
        }
    }

    /// <summary>
    /// 用于外部实例化 增加引用计数
    /// </summary>
    /// <param name="_assetName"></param>
    /// <param name="_gameObject"></param>
    public void AddAssetRef(string _assetName, GameObject _gameObject)
    {
        if (!this._loadedList.ContainsKey(_assetName))
        {
            return;
        }

        PrefabObject prefabObj = this._loadedList[_assetName];

        int instanceID = _gameObject.GetInstanceID();
        if (this._goInstanceIDList.ContainsKey(instanceID))
        {
            string errormsg = string.Format("PrefabLoadMgr AddAssetRef error ! assetName:{0}", _assetName);
            UtilLog.LogError(errormsg);
            return;
        }

        prefabObj._refCount++;

        prefabObj._goInstanceIDSet.Add(instanceID);
        this._goInstanceIDList.Add(instanceID, prefabObj);
    }

    private void UpdateLoadedAsync()
    {
        if (this._loadedAsyncList.Count == 0)
        {
            return;
        }

        int count = this._loadedAsyncList.Count;
        for (int i = 0; i < count; ++i)
        {
            this._loadedAsyncList[i]._lockCallbackCount = this._loadedAsyncList[i]._callbackList.Count;
        }

        for (int i = 0; i < count; i++)
        {
            this.DoInstanceAssetCallback(_loadedAsyncList[i]);
        }
        this._loadedAsyncList.RemoveRange(0, count);
    }


    public void Update()
    {
        this.UpdateLoadedAsync();
    }
}
