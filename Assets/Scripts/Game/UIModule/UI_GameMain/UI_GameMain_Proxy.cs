using PureMVC.Patterns.Proxy;
using System;

public class UI_GameMain_Proxy : Proxy
{
    /// <summary>
    /// 覆盖NAME助理名 方便控制层调用助理
    /// </summary>
    public new const string NAME = "UI_GameMain_Proxy";

    public UI_GameMainData gameMainData = null;

    public UI_GameMain_Proxy(string proxyName, object data = null) : base(proxyName, data)
    {
        this.gameMainData = new UI_GameMainData(100, 100);
    }

    public void RequestDataInfo()
    {
        this.RespondDataInfo();
    }

    public void RespondDataInfo()
    {
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameMain_FillInfo);
    }

    public void AddCoin(float _coin)
    {
        this.gameMainData.coin += _coin;
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameMain_Change_Coin, this.gameMainData);
    }

    public void AddDiamond(float _diamond)
    {
        this.gameMainData.diamond += _diamond;
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameMain_Change_Diamond, this.gameMainData);
    }
}
