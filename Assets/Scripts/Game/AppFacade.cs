using PureMVC.Patterns.Facade;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppFacade : Facade
{
    public AppFacade() : base("")
    {
        this.RegisterCommand(EnumGameNotify.GAMEUI_STARTUP.ToString(), () => new GameUI_Command());
    }
}
