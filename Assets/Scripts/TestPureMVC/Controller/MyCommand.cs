using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PureMVC.Patterns.Command;
using PureMVC.Interfaces;

public class MyCommand : SimpleCommand
{
    MyDataProxy dataProxy
    {
        get
        {
            return Facade.RetrieveProxy("MyDataProxy_Name") as MyDataProxy;
        }
    }

    public override void Execute(INotification notification)
    {
        base.Execute(notification);
        switch(notification.Name)
        {
            case "msg_add_f":
                dataProxy.AddValue();
                break;
            case "msg_sub_f":
                dataProxy.SubValue();
                break;
            default:
                break;
        }
    }
}
