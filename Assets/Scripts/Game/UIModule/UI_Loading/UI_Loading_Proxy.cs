using PureMVC.Patterns.Proxy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Loading_Proxy : Proxy
{
    public new const string NAME = "UI_Game_Proxy";

    public UI_Loading_Proxy(string proxyName, object data = null) : base(proxyName, data)
    {
    }
}
