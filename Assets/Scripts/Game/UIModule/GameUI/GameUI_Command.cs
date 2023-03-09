using PureMVC.Interfaces;
using PureMVC.Patterns.Command;
using PureMVC.Patterns.Facade;
using UnityEngine;

public class GameUI_Command : SimpleCommand
{
    public override void Execute(INotification notification)
    {
        base.Execute(notification);

        if(EnumGameNotify.GAMEUI_STARTUP.ToString() == notification.Name)
        {
            MyFacade.GetInstance("", key => new Facade(key)).RegisterMediator(new GameUI_Mediator(GameUI_Mediator.NAME));
        }
    }
}
