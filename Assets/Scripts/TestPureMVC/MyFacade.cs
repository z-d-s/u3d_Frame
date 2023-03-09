using PureMVC.Patterns.Facade;
using PureMVC.Patterns.Mediator;
using PureMVC.Patterns.Proxy;
using UnityEngine;

public class MyFacade : Facade
{
    public MyFacade(GameObject viewComponent) : base("")
    {
        RegisterCommand("msg_add_f", () => new MyCommand());
        RegisterCommand("msg_sub_f", () => new MyCommand());
        RegisterMediator(new MyMediator(MyMediator.mediatorName, viewComponent));
        RegisterProxy(new MyDataProxy(MyDataProxy.proxyName));
    }
}