using PureMVC.Patterns.Proxy;
using System;

public class UI_GameOver_Proxy : Proxy
{
    /// <summary>
    /// 覆盖NAME助理名 方便控制层调用助理
    /// </summary>
    public new const string NAME = "UI_GameOver_Proxy";

    public UI_GameMainData gameUIData = null;

    public UI_GameOver_Proxy(string proxyName, object data = null) : base(proxyName, data)
    {
        this.gameUIData = new UI_GameMainData(100, 100);
    }

    public void RequestDataInfo()
    {
        this.RespondDataInfo();
    }

    public void RespondDataInfo()
    {
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameMain_FillInfo);
    }

    public void AddHp(float _hp)
    {
        this.gameUIData.hp += _hp;
        this.gameUIData.hp = Math.Clamp(this.gameUIData.hp, 0f, this.gameUIData.max_Hp);
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameMain_Change_Hp, this.gameUIData);
    }

    public void AddExp(float _exp)
    {
        this.gameUIData.exp += _exp;
        this.gameUIData.exp = Math.Clamp(this.gameUIData.exp, 0f, this.gameUIData.max_Exp);
        GameFacade.Instance.SendNotification(EventDefine.MVC_UI_GameMain_Change_Score, this.gameUIData);
    }
}
