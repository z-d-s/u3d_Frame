using PureMVC.Patterns.Proxy;
using System;

public class UI_GameOver_Proxy : Proxy
{
    /// <summary>
    /// 覆盖NAME助理名 方便控制层调用助理
    /// </summary>
    public new const string NAME = "UI_GameOver_Proxy";

    public UI_GameOverData gameUIData = null;

    public UI_GameOver_Proxy(string proxyName, object data = null) : base(proxyName, data)
    {
        this.gameUIData = new UI_GameOverData(100, 100);
    }

    public void RequestDataInfo()
    {
        this.RespondDataInfo();
    }

    public void RespondDataInfo()
    {
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameOver_FillInfo);
    }
}
