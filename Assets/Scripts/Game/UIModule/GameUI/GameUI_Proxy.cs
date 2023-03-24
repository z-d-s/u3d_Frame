using PureMVC.Patterns.Proxy;
using System;

public class GameUI_Proxy : Proxy
{
    /// <summary>
    /// 覆盖NAME助理名 方便控制层调用助理
    /// </summary>
    public new const string NAME = "GameUI_Proxy";

    private GameUIData gameUIData = null;

    public GameUI_Proxy(string proxyName, object data = null) : base(proxyName, data)
    {
        this.gameUIData = new GameUIData(100, 100);
    }

    public void AddHp(float _hp)
    {
        this.gameUIData.hp += _hp;
        this.gameUIData.hp = Math.Clamp(this.gameUIData.hp, 0f, this.gameUIData.max_Hp);
        AppFacade.Instance.SendNotification(GameEventDefine.EV_GameUI_Change_Hp, this.gameUIData);
    }

    public void AddExp(float _exp)
    {
        this.gameUIData.exp += _exp;
        this.gameUIData.exp = Math.Clamp(this.gameUIData.exp, 0f, this.gameUIData.max_Exp);
        AppFacade.Instance.SendNotification(GameEventDefine.EV_GameUI_Change_Exp, this.gameUIData);
    }
}
