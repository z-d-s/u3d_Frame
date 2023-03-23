using PureMVC.Patterns.Proxy;

public class GameUI_Proxy : Proxy
{
    /// <summary>
    /// 覆盖NAME助理名 方便控制层调用助理
    /// </summary>
    public new const string NAME = "GameUI_Proxy";

    public GameUI_Proxy(string proxyName, object data = null) : base(proxyName, data)
    {
    }
}
