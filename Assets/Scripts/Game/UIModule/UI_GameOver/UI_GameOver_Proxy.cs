using PureMVC.Patterns.Proxy;
using System;

public class UI_GameOver_Proxy : Proxy
{
    /// <summary>
    /// 覆盖NAME助理名 方便控制层调用助理
    /// </summary>
    public new const string NAME = "UI_GameOver_Proxy";

    public UI_GameData gameUIData = null;

    public UI_GameOver_Proxy(string proxyName, object data = null) : base(proxyName, data)
    {
        this.gameUIData = new UI_GameData(100, 100);
    }

    public void RequestDataInfo()
    {
        this.RespondDataInfo();
    }

    public void RespondDataInfo()
    {
        AppFacade.Instance.SendNotification(EventDefine.MVC_UI_Game_FillInfo);
    }

    public void AddHp(float _hp)
    {
        this.gameUIData.hp += _hp;
        this.gameUIData.hp = Math.Clamp(this.gameUIData.hp, 0f, this.gameUIData.max_Hp);
        AppFacade.Instance.SendNotification(EventDefine.MVC_UI_Game_Change_Hp, this.gameUIData);
    }

    public void AddExp(float _exp)
    {
        this.gameUIData.exp += _exp;
        this.gameUIData.exp = Math.Clamp(this.gameUIData.exp, 0f, this.gameUIData.max_Exp);
        AppFacade.Instance.SendNotification(EventDefine.MVC_UI_Game_Change_Score, this.gameUIData);
    }
}