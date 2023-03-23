using PureMVC.Patterns.Facade;

public class AppFacade : Facade
{
    public AppFacade() : base("")
    {
        this.RegisterCommand(GameEventDefine.GAMEUI_STARTUP, () => new GameUI_Command());
    }

    public static AppFacade Instance
    {
        get
        {
            return Facade.GetInstance("", key => new Facade(key)) as AppFacade;
        }
    }
}
