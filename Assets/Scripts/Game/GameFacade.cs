using PureMVC.Patterns.Facade;
using Unity.VisualScripting;

public class GameFacade : Facade
{
    #region 单例
    private static GameFacade instance;
    private static object mutex = new object();
    public static GameFacade Instance
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
                        instance = new GameFacade();
                    }
                }
            }
            return instance;
        }
    }
    #endregion

    public GameFacade() : base("")
    {
    }

    public void StartUp()
    {
        LogHelper.LogGreen("=== GameFacade 启动成功 ===");
    }

    protected override void InitializeView()
    {
        base.InitializeView();
    }

    protected override void InitializeController()
    {
        base.InitializeController();

        this.RegisterCommand(EventDefine.MVC_UI_Loading_StartUp, () => new UI_Loading_Command());
        this.RegisterCommand(EventDefine.MVC_UI_GameMain_StartUp, () => new UI_Game_Command());
        this.RegisterCommand(EventDefine.MVC_UI_Setting_StartUp, () => new UI_Setting_Command());
        this.RegisterCommand(EventDefine.MVC_UI_GameControl_StartUp, () => new UI_GameControl_Command());
        this.RegisterCommand(EventDefine.MVC_UI_GameOver_StartUp, () => new UI_GameOver_Command());
    }

    protected override void InitializeModel()
    {
        base.InitializeModel();

        this.RegisterProxy(new UI_GameMain_Proxy(UI_GameMain_Proxy.NAME));
        this.RegisterProxy(new UI_GameOver_Proxy(UI_GameOver_Proxy.NAME));
    }
}
