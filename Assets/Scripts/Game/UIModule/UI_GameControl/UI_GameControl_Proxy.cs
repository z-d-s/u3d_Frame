using PureMVC.Patterns.Proxy;
using System;

public class UI_GameControl_Proxy : Proxy
{
    /// <summary>
    /// 覆盖NAME助理名 方便控制层调用助理
    /// </summary>
    public new const string NAME = "UI_GameControl_Proxy";

    public UI_GameControlData gameControlData;

    public UI_GameControl_Proxy(string proxyName, object data = null) : base(proxyName, data)
    {
        this.gameControlData = new UI_GameControlData(100, 100);
    }

    public void RequestDataInfo()
    {
        this.RespondDataInfo();
    }

    public void RespondDataInfo()
    {
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameControl_FillInfo);
    }

    public void AddHp(float _hp)
    {
        this.gameControlData.hp += _hp;
        this.gameControlData.hp = Math.Clamp(this.gameControlData.hp, 0f, this.gameControlData.max_Hp);
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameControl_Change_Hp, this.gameControlData);
    }

    public void AddExp(float _exp)
    {
        this.gameControlData.exp += _exp;
        this.gameControlData.exp = Math.Clamp(this.gameControlData.exp, 0f, this.gameControlData.max_Exp);
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameControl_Change_Score, this.gameControlData);
    }
}
