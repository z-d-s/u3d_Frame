using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleLoader : Loader
{
    AssetBundleCreateRequest m_ABRequest = null;
    private int m_RefCount;
    private List<string> list_DependenceAssetBundleName = new List<string>();

    private LoadedHandle onAssetBundleLoaded;

    private string m_BundleName;
    public string BundleName { get { return m_BundleName; } }

    System.Diagnostics.Stopwatch m_SaveABWatcher = new System.Diagnostics.Stopwatch();

    protected AssetBundleLoader() : base(Loader.LoaderType.BUNDLE)
    {
    }

    public void AddLoadedCallback(LoadedHandle onLoaded)
    {
        this.onAssetBundleLoaded += onLoaded;
        ++this.m_RefCount;
    }
}
