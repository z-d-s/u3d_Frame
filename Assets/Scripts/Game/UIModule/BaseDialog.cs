/****************************************************

    动效弹窗基类：
        1.有一个btn_Mask的子物体 挂载Button组件
        2.有一个panel_Animate的子物体

*****************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BaseDialog : BaseUI
{
    #region 缩放参数
    /// <summary>
    /// 弹出起始X大小
    /// </summary>
    [Header("ScaleSetting")]
    [SerializeField] private float startingXScale = 0.5f;
    /// <summary>
    /// 弹出起始Y大小
    /// </summary>
    [SerializeField] private float startingYScale = 0.5f;
    /// <summary>
    /// 弹出所用时间
    /// </summary>
    [SerializeField] private float scaleTime = 0.2f;
    /// <summary>
    /// 弹出外溢大小
    /// </summary>
    [SerializeField] private float overshootScaleAmount = 0.03f;
    /// <summary>
    /// 弹出外溢恢复所用时间
    /// </summary>
    [SerializeField] private float overshootReturnTime = 0.1f;
    #endregion

    #region Alpha参数
    /// <summary>
    /// alpha起始值
    /// </summary>
    [Header("AlphaSetting")]
    [SerializeField] private float startingAlpha = 0.5f;
    /// <summary>
    /// alpha所用时间
    /// </summary>
    [SerializeField] private float alphaTime = 0.2f;
    #endregion

    #region 移动参数
    /// <summary>
    /// 默认位置
    /// </summary>
    [Header("MoveSetting")]
    [SerializeField] private Vector2 startPos = Vector2.zero;
    /// <summary>
    /// 移动时起始偏移位置
    /// </summary>
    [SerializeField] private Vector2 startPosOffset = new Vector2(0f, 0f);
    /// <summary>
    /// 移动时起始偏移位置
    /// </summary>
    [SerializeField] private Vector2 endPosOffset = new Vector2(0f, 0f);
    /// <summary>
    /// 移动所用时间
    /// </summary>
    [SerializeField] private float moveTime = 0f;
    #endregion

    private Coroutine show_ScaleRoutine = null;     //弹出协程实例
    private Coroutine show_AlphaRoutine = null;     //淡出协程实例
    private Coroutine show_MoveRoutine = null;      //移出协程实例

    private Coroutine hide_ScaleRoutine = null;     //弹入协程实例
    private Coroutine hide_AlphaRoutine = null;     //淡入协程实例
    private Coroutine hide_MoveRoutine = null;      //移入协程实例

    private Button btn_Mask;
    /// <summary>
    /// 要做动画的panel
    /// </summary>
    private RectTransform panelToAnimate = null;
    /// <summary>
    /// 动画panel上的组件(控制整体alpha用)
    /// </summary>
    private CanvasGroup canvasGroup = null;

    public override void Awake()
    {
        base.Awake();

        this.btn_Mask = this.transform.Find("btn_Mask").GetComponent<Button>();
        this.btn_Mask.onClick.AddListener(() =>
        {
            this.Hide();
        });

        GameObject panel_Animate = this.transform.Find("panel_Animate").gameObject;
        this.panelToAnimate = panel_Animate.GetComponent<RectTransform>();
        this.canvasGroup = panel_Animate.GetComponent<CanvasGroup>();
        if(this.canvasGroup == null)
        {
            this.canvasGroup = panel_Animate.AddComponent<CanvasGroup>();
        }

        this.SetInitialValues();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        this.SetInitialValues();
        this.ShowWithAni();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        this.StopAni();
    }

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        //ps::此处不调用基类Hide，需要动画播放完毕后再调用

        this.HideWithAni();
    }

    /// <summary>
    /// 初始化数值
    /// </summary>
    private void SetInitialValues()
    {
        this.panelToAnimate.localScale = new Vector3(this.startingXScale, this.startingYScale, 1f);
        this.canvasGroup.alpha = this.startingAlpha;
        this.panelToAnimate.anchoredPosition = this.startPos;
    }

    private void ShowWithAni()
    {
        this.btn_Mask.interactable = false;

        this.StopAni();

        this.show_ScaleRoutine = StartCoroutine(this.Show_ScaleRoutine());
        this.show_AlphaRoutine = StartCoroutine(this.Show_AlphaRoutine());
        this.show_MoveRoutine = StartCoroutine(this.Show_MoveRoutine());
    }

    private void HideWithAni()
    {
        this.btn_Mask.interactable = false;
        this.StopAni();

        this.hide_ScaleRoutine = StartCoroutine(this.Hide_ScaleRoutine());
        this.hide_AlphaRoutine = StartCoroutine(this.Hide_AlphaRoutine());
        this.hide_MoveRoutine = StartCoroutine(this.Hide_MoveRoutine());
    }

    private void StopAni()
    {
        if(this.show_ScaleRoutine != null)
            StopCoroutine(this.show_ScaleRoutine);

        if (this.show_AlphaRoutine != null)
            StopCoroutine(this.show_AlphaRoutine);

        if (this.show_MoveRoutine != null)
            StopCoroutine(this.show_MoveRoutine);

        if (this.hide_ScaleRoutine != null)
            StopCoroutine(this.hide_ScaleRoutine);

        if(this.hide_AlphaRoutine != null)
            StopCoroutine(this.hide_AlphaRoutine);

        if(this.hide_MoveRoutine != null)
            StopCoroutine(this.hide_MoveRoutine);
    }

    IEnumerator Show_ScaleRoutine()
    {
        float newXScale;
        float newYScale;

        float destinationXScale = 1 + this.overshootScaleAmount;
        float destinationYScale = 1 + this.overshootScaleAmount;

        if(this.scaleTime > 0)
        {
            for(float t = 0; t < this.scaleTime; t += Time.deltaTime)
            {
                newXScale = Mathf.Lerp(this.startingXScale, destinationXScale, t / this.scaleTime);
                newYScale = Mathf.Lerp(this.startingYScale, destinationYScale, t / this.scaleTime);
                this.panelToAnimate.localScale = new Vector3(newXScale, newYScale, 1);
                yield return null;
            }
        }

        float overshootStartXScale = destinationXScale;
        float overshootStartYScale = destinationYScale;
        destinationXScale = 1f;
        destinationYScale = 1f;
        if(this.overshootReturnTime > 0)
        {
            for(float t = 0; t < this.overshootReturnTime; t += Time.deltaTime)
            {
                newXScale = Mathf.Lerp(overshootStartXScale, destinationXScale, t / this.overshootReturnTime);
                newYScale = Mathf.Lerp(overshootStartYScale, destinationYScale, t / this.overshootReturnTime);
                this.panelToAnimate.localScale = new Vector3(newXScale, newYScale, 1);
                yield return null;
            }
        }

        //确保已经恢复正常大小
        this.panelToAnimate.localScale = Vector3.one;
        this.btn_Mask.interactable = true;
    }

    IEnumerator Show_AlphaRoutine()
    {
        float newAlphaValue;
        if(this.alphaTime > 0)
        {
            for(float t = 0; t < this.alphaTime; t += Time.deltaTime)
            {
                newAlphaValue = Mathf.Lerp(this.startingAlpha, 1, t / this.alphaTime);
                this.canvasGroup.alpha = newAlphaValue;
                yield return null;
            }
        }
        
        this.canvasGroup.alpha = 1;
    }

    IEnumerator Show_MoveRoutine()
    {
        float startPosX = this.startPos.x + startPosOffset.x;
        float startPosY = this.startPos.y + startPosOffset.y;
        float targetPosX = this.startPos.x;
        float targetPosY = this.startPos.y;

        if(this.moveTime > 0)
        {
            float currentPosX = startPosX;
            float currentPosY = startPosY;

            for(float t = 0; t < this.moveTime; t += Time.deltaTime)
            {
                currentPosX = Mathf.Lerp(startPosX, targetPosX, t / this.moveTime);
                currentPosY = Mathf.Lerp(startPosY, targetPosY, t / this.moveTime);
                this.panelToAnimate.anchoredPosition = new Vector2(currentPosX, currentPosY);
                yield return null;
            }
        }
        
        this.panelToAnimate.anchoredPosition = new Vector2(targetPosX, targetPosY);
    }

    IEnumerator Hide_ScaleRoutine()
    {
        float newXScale;
        float newYScale;

        float destinationXScale = this.startingXScale;
        float destinationYScale = this.startingYScale;

        if (this.scaleTime > 0)
        {
            for (float t = 0; t < this.scaleTime; t += Time.deltaTime)
            {
                newXScale = Mathf.Lerp(1, destinationXScale, t / this.scaleTime);
                newYScale = Mathf.Lerp(1, destinationYScale, t / this.scaleTime);
                this.panelToAnimate.localScale = new Vector3(newXScale, newYScale, 1);
                yield return null;
            }
        }

        this.btn_Mask.interactable = true;
        base.Hide();
    }

    IEnumerator Hide_AlphaRoutine()
    {
        float newAlphaValue;
        if (this.alphaTime > 0)
        {
            for (float t = 0; t < this.alphaTime; t += Time.deltaTime)
            {
                newAlphaValue = Mathf.Lerp(1, this.startingAlpha, t / this.alphaTime);
                this.canvasGroup.alpha = newAlphaValue;
                yield return null;
            }
        }
    }

    IEnumerator Hide_MoveRoutine()
    {
        float startPosX = this.startPos.x;
        float startPosY = this.startPos.y;
        float targetPosX = this.startPos.x + this.endPosOffset.x;
        float targetPosY = this.startPos.y + this.endPosOffset.y;

        if (this.moveTime > 0)
        {
            float currentPosX = startPosX;
            float currentPosY = startPosY;

            for (float t = 0; t < this.moveTime; t += Time.deltaTime)
            {
                currentPosX = Mathf.Lerp(startPosX, targetPosX, t / this.moveTime);
                currentPosY = Mathf.Lerp(startPosY, targetPosY, t / this.moveTime);
                this.panelToAnimate.anchoredPosition = new Vector2(currentPosX, currentPosY);
                yield return null;
            }
        }
    }
}
