/****************************************************

    所有加载器的基类

*****************************************************/

public class Loader
{
    #region Define
    public enum LoaderType
    {
        STREAM,         //流(原则上可以是任何文件，包括远程服务器上的)
        ASSET,          //Asset目录下的资源
        BUNDLE,         //AssetBundle
        BUNDLEASSET,    //AssetBundle中的Asset资源
        SCENE,          //场景
        TEXTURE,        //图片
    }
    public enum LoaderState
    {
        NONE,
        LOADING,        //加载中
        FINISHED,       //完成
    }

    public delegate void ProgressHandle(Loader loader, float rate);
    public delegate void LoadedHandle(object data);
    #endregion

    /// <summary>
    /// 加载器类型
    /// </summary>
    protected LoaderType m_type;

    /// <summary>
    /// 加载状态
    /// </summary>
    protected LoaderState m_state;

    /// <summary>
    /// 加载路径
    /// </summary>
    protected string m_path;

    /// <summary>
    /// 是否异步
    /// </summary>
    protected bool m_async;

    /// <summary>
    /// 加载进度
    /// </summary>
    protected ProgressHandle onProgress;

    /// <summary>
    /// 加载完成回调通知
    /// </summary>
    protected LoadedHandle onLoaded;

    /// <summary>
    /// 加载时间统计
    /// </summary>
    protected System.Diagnostics.Stopwatch m_watch = new System.Diagnostics.Stopwatch();

    public LoaderType Type { get { return m_type; } }
    public string Path { get { return m_path; } }
    public bool IsFinished { get { return m_state == LoaderState.FINISHED; } }
    public bool IsAsync { get { return m_async; } }

    protected Loader(LoaderType __type)
    {
        this.m_type = __type;
    }

    /// <summary>
    /// 是否可以加载
    /// 主要用来判断ab包，因为ab包需要等待依赖包加载完成
    /// </summary>
    /// <returns></returns>
    public virtual bool IsPrepareToLoad()
    {
        return true;
    }

    /// <summary>
    /// 初始化参数
    /// </summary>
    /// <param name="path"></param>
    /// <param name="onLoaded"></param>
    /// <param name="async"></param>
    public virtual void Init(string path,LoadedHandle onLoaded, bool async = true)
    {
        this.m_state = LoaderState.NONE;
        this.m_path = path;
        this.m_async = async;
        this.onLoaded = onLoaded;
    }

    public virtual void Load()
    {
        this.m_watch.Reset();
        this.m_watch.Start();
        this.m_state = LoaderState.LOADING;
        this.OnLoadProgress(0f);
    }

    protected virtual void OnLoadProgress(float rate)
    {
        if (this.onProgress != null)
        {
            this.onProgress(this, rate);
        }
    }

    protected virtual void OnLoadCompleted(object data)
    {
        this.m_state = LoaderState.FINISHED;

        try
        {
            if(this.onLoaded != null)
            {
                this.onLoaded(data);
            }
        }
        catch(System.Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
        }

        this.OnLoadProgress(1f);
    }

    public virtual void Update(float dt)
    {

    }

    public virtual void Stop()
    {
        this.Reset();
    }

    /// <summary>
    /// 重置
    /// </summary>
    public virtual void Reset()
    {
        this.m_state = LoaderState.NONE;
        this.m_path = "";
        this.m_async = true;
        this.onProgress = null;
        this.onLoaded = null;
    }
}
