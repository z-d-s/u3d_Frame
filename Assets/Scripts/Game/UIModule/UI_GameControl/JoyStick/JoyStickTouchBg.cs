using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickTouchBg : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public System.Action<PointerEventData> delegate_OnPointerDown;
    public System.Action<PointerEventData> delegate_OnPointerUp;

    public System.Action<PointerEventData> delegate_OnBeginDrag;
    public System.Action<PointerEventData> delegate_OnDrag;
    public System.Action<PointerEventData> delegate_OnEndDrag;

    public void OnPointerDown(PointerEventData eventData)
    {
        if(this.delegate_OnPointerDown != null)
        {
            this.delegate_OnPointerDown.Invoke(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (this.delegate_OnPointerUp != null)
        {
            this.delegate_OnPointerUp.Invoke(eventData);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.delegate_OnBeginDrag?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.delegate_OnDrag?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.delegate_OnEndDrag?.Invoke(eventData);
    }
}
