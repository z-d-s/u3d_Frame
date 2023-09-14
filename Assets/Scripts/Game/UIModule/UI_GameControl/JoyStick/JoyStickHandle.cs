using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _parent;

    /// <summary>
    /// 触摸背景(非遥感本身背景)
    /// </summary>
    public RectTransform touchBg;

    /// <summary>
    /// 操纵杆
    /// </summary>
    private RectTransform stick;

    /// <summary>
    /// 遥感初始位置
    /// </summary>
    private Vector2 handleInitPos = Vector2.zero;

    /// <summary>
    /// 摇杆半径范围
    /// </summary>
    private float radius = 0f;

    /// <summary>
    /// 按下时的位置
    /// </summary>
    private Vector2 pointDownPos;

    [HideInInspector]
    public Vector2 handleInput = Vector2.zero;

    private void Awake()
    {
        this._parent = this.transform.parent;
        this.stick = this.transform.Find("stick").GetComponent<RectTransform>();
        this.handleInitPos = this.transform.localPosition;
        this.radius = (this.transform as RectTransform).rect.width * 0.5f;
        if (this.touchBg != null)
        {
            this.SetTouchBg(this.touchBg.gameObject);
        }
    }

    public void SetTouchBg(GameObject touchBg)
    {
        this.touchBg = touchBg.GetComponent<RectTransform>();
        if (this.touchBg != null && this.touchBg.GetComponent<JoyStickTouchBg>() == null)
        {
            JoyStickTouchBg jsTouchBg = this.touchBg.AddComponent<JoyStickTouchBg>();
            jsTouchBg.delegate_OnPointerDown += this.OnPointerDown_TouchBg;
            jsTouchBg.delegate_OnPointerUp += this.OnPointerUp_TouchBg;
            jsTouchBg.delegate_OnBeginDrag += this.OnBeginDrag;
            jsTouchBg.delegate_OnDrag += this.OnDrag;
            jsTouchBg.delegate_OnEndDrag += this.OnEndDrag;
        }
        this.gameObject.SetActive(false);
    }

    public void OnPointerDown_TouchBg(PointerEventData eventData)
    {
        if(this._parent != null)
        {
            RectTransform parentRect = this._parent.GetComponent<RectTransform>();
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, eventData.position, null, out localPos);
            this.transform.localPosition = localPos;
        }
        this.gameObject.SetActive(true);
    }

    public void OnPointerUp_TouchBg(PointerEventData eventData)
    {
        this.transform.localPosition = this.handleInitPos;
        this.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.pointDownPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dis = eventData.position - this.pointDownPos;
        float clamp = Mathf.Clamp(dis.magnitude, 0f, this.radius);
        Vector2 normalizedDis = dis.normalized* clamp;
        this.stick.localPosition = normalizedDis;

        this.SetHandleInput(dis.normalized);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.stick.localPosition = Vector2.zero;
        this.SetHandleInput(Vector2.zero);
    }

    private void SetHandleInput(Vector2 input)
    {
        this.handleInput = input;
        EventMgr.Instance.Dispatch(EventDefine.EVE_GameControl_Move, EventArgs<Vector2>.CreateEventArgs(this.handleInput));
    }
}
