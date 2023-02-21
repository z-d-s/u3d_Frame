using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleLoadMgr : MonoBaseSingleton<AssetBundleLoadMgr>
{
    public delegate void AssetBundleLoadCallBack(AssetBundle ab);

    private class AssetBundleObject
    {
        public string _hashName;
        public int _refCount;
        public List<AssetBundleLoadCallBack> _callFunList = new List<AssetBundleLoadCallBack>();
        public AssetBundleCreateRequest _request;
        public AssetBundle _ab;

        public int _dependLoadingCount;
        public List<AssetBundleObject> _depends = new List<AssetBundleObject>();
    }

    public void Update()
    {

    }
}