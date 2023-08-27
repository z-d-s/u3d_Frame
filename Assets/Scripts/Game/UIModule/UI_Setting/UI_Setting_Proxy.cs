using PureMVC.Patterns.Proxy;
using System;

public class UI_Setting_Proxy : Proxy
{
    /// <summary>
    /// 覆盖NAME助理名 方便控制层调用助理
    /// </summary>
    public new const string NAME = "UI_Setting_Proxy";

    public UI_Setting_Proxy(string proxyName, object data = null) : base(proxyName, data)
    {
        
    }
}
