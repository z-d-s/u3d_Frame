using PureMVC.Patterns.Facade;
using UnityEngine;

public class MyFacade : Facade
{
    public MyFacade(GameObject viewComponent) : base("")
    {
        RegisterCommand("msg_add_f", () => new MyCommand());
        RegisterCommand("msg_sub_f", () => new MyCommand());
        RegisterMediator(new MyMediator("MyMediator_Name", viewComponent));
        RegisterProxy(new MyDataProxy("MyDataProxy_Name"));
    }
}
