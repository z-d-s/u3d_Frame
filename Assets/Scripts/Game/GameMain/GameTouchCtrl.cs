using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameTouchCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool touching = false;
    private float touchingTime = 0f;
    private float cacheTime = 0f;

    private void Update()
    {
        if (this.touching == false)
        {
            return;
        }

        this.touchingTime += Time.deltaTime;
        AppFacade.Instance.SendNotification(EventDefine.MVC_UI_Game_RefreshTouchingTime, this.touchingTime);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameMgr.Instance.gameState != GameState.Gameing)
        {
            return;
        }

        if (this.touching == true)
        {
            return;
        }

        this.touching = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameMgr.Instance.gameState != GameState.Gameing)
        {
            return;
        }

        if(this.touching == false)
        {
            return;
        }

        this.touching = false;
        this.cacheTime = this.touchingTime;
        this.touchingTime = 0f;
        EventMgr.Instance.Dispatch(EventDefine.EVE_UI_Game_Jump, EventArgs<float>.CreateEventArgs(this.cacheTime));
        AppFacade.Instance.SendNotification(EventDefine.MVC_UI_Game_RefreshTouchingTime, this.touchingTime);
    }
}
