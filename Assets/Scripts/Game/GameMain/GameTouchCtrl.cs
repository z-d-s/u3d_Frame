using UnityEngine;
using UnityEngine.EventSystems;

public class GameTouchCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// 是否触屏中
    /// </summary>
    private bool touching = false;
    /// <summary>
    /// 按下时间(会在松开那一刻清0)
    /// </summary>
    private float touchingTime = 0f;
    /// <summary>
    /// 按下时间(会在松开那一刻赋值)
    /// </summary>
    private float cacheTime = 0f;

    private void Update()
    {
        if (this.touching == false)
        {
            return;
        }

        this.touchingTime += Time.deltaTime;
        EventMgr.Instance.Dispatch(EventDefine.EVE_Game_RefreshTargetPos, EventArgs<float>.CreateEventArgs(this.touchingTime));
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
        EventMgr.Instance.Dispatch(EventDefine.EVE_Game_Jump, EventArgs<float>.CreateEventArgs(this.cacheTime));
        AppFacade.Instance.SendNotification(EventDefine.MVC_UI_Game_RefreshTouchingTime, this.touchingTime);
    }
}
