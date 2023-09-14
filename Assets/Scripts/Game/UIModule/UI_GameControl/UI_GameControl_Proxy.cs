using PureMVC.Patterns.Proxy;
using System;

public class UI_GameControl_Proxy : Proxy
{
    /// <summary>
    /// 覆盖NAME助理名 方便控制层调用助理
    /// </summary>
    public new const string NAME = "UI_GameControl_Proxy";

    public UI_GameControl_Proxy(string proxyName, object data = null) : base(proxyName, data)
    {
        
    }

    public void RequestDataInfo()
    {
        this.RespondDataInfo();
    }

    public void RespondDataInfo()
    {
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameControl_FillInfo);
    }
}
