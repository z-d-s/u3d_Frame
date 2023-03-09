using PureMVC.Patterns.Proxy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI_Proxy : Proxy
{
    public new const string NAME = "GameUI_Proxy";

    public GameUI_Proxy(string proxyName, object data = null) : base(proxyName, data)
    {
    }
}
