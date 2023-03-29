using PureMVC.Patterns.Facade;
using Unity.VisualScripting;

public class AppFacade : Facade
{
    #region 单例
    private static AppFacade instance;
    private static object mutex = new object();
    public static AppFacade Instance
    {
        get
        {
            if (instance == null)
            {
                // 保证我们的单例，是线程安全的;
                lock (mutex)
                {
                    if (instance == null)
                    {
                        instance = new AppFacade();
                    }
                }
            }
            return instance;
        }
    }
    #endregion

    public AppFacade() : base("")
    {
    }

    public void StartUp()
    {
        UtilLog.LogGreen("=== AppFacade 启动成功 ===");
    }

    protected override void InitializeView()
    {
        base.InitializeView();
    }

    protected override void InitializeController()
    {
        base.InitializeController();

        this.RegisterCommand(EventDefine.MVC_GameUI_StartUp, () => new GameUI_Command());
    }

    protected override void InitializeModel()
    {
        base.InitializeModel();

        this.RegisterProxy(new GameUI_Proxy(GameUI_Proxy.NAME));
    }
}
