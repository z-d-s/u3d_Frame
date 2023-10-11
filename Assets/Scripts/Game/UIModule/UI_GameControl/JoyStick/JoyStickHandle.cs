using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickHandle : MonoBehaviour
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
    /// 屏幕是否按下
    /// </summary>
    private bool pointHolding = false;

    private int pointID = -1;

    /// <summary>
    /// 摇杆半径范围
    /// </summary>
    private float radius = 0f;

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
        if(this.pointHolding == true)
        {
            return;
        }
        this.pointID = eventData.pointerId;
        this.pointHolding = true;

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
        if(this.pointID != eventData.pointerId)
        {
            return;
        }
        this.pointID = -1;
        this.pointHolding = false;

        this.stick.localPosition = Vector2.zero;
        this.SetHandleInput(Vector2.zero);

        this.transform.localPosition = this.handleInitPos;
        this.gameObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.pointID != eventData.pointerId)
        {
            return;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.pointID != eventData.pointerId)
        {
            return;
        }

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(), eventData.position, null, out localPos);

        Vector2 dir = localPos - Vector2.zero;
        float clamp = Mathf.Clamp(dir.magnitude, 0f, this.radius);
        Vector2 normalizedDis = dir.normalized * clamp;
        this.stick.localPosition = normalizedDis;

        this.SetHandleInput(dir.normalized);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.pointID != eventData.pointerId)
        {
            return;
        }
    }

    public void SetByAngle(Vector2 dir = new Vector2())
    {
        if(this.pointHolding)
        {
            return;
        }

        if (dir == Vector2.zero)
        {
            this.gameObject.SetActive(false);
            this.SetHandleInput(Vector2.zero);
            return;
        }
        this.gameObject.SetActive(true);
        this.stick.localPosition = dir.normalized * this.radius;
        this.SetHandleInput(dir);
    }

    private void SetHandleInput(Vector2 input)
    {
        this.handleInput = input;
        EventMgr.Instance.Dispatch(EventDefine.EVE_GameControl_Move, EventArgs<Vector2>.CreateEventArgs(this.handleInput));
    }
}
