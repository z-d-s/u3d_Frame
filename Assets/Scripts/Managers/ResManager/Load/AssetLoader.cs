/****************************************************

    Editor模式下加载资源
    直接使用AssetDatabase加载

*****************************************************/

using UnityEngine;

public class AssetLoader : Loader
{
    private Object m_data = null;
    private System.Type m_assetType = null;

    protected AssetLoader() : base(Loader.LoaderType.ASSET)
    {
    }

    public void Init(string path, System.Type type, LoadedHandle onLoaded, bool async = true)
    {
        base.Init(path, onLoaded, async);
        this.m_assetType = type;
    }

    public override void Load()
    {
        base.Load();

#if UNITY_EDITOR
        if (this.m_assetType == null)
        {
            this.m_assetType = typeof(Object);
        }

        this.m_data = UnityEditor.AssetDatabase.LoadAssetAtPath(this.m_path, this.m_assetType);
        if (this.m_async == false)
        {
            this.OnLoadCompleted(m_data);
        }
#else
        if (this.m_async == false)
        {
            this.OnLoadCompleted(null);
        }
#endif
    }

    public override void Update(float dt)
    {
        if (m_state == LoaderState.LOADING)
        {
            OnLoadCompleted(m_data);
            m_data = null;
        }
    }
}
