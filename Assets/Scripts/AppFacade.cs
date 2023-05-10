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
        LogHelper.LogGreen("=== AppFacade 启动成功 ===");
    }

    protected override void InitializeView()
    {
        base.InitializeView();
    }

    protected override void InitializeController()
    {
        base.InitializeController();

        this.RegisterCommand(EventDefine.MVC_UI_Loading_StartUp, () => new UI_Loading_Command());
        this.RegisterCommand(EventDefine.MVC_UI_Game_StartUp, () => new UI_Game_Command());
    }

    protected override void InitializeModel()
    {
        base.InitializeModel();

        this.RegisterProxy(new UI_Game_Proxy(UI_Game_Proxy.NAME));
    }
}
