using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMgr : MonoBaseSingleton<UIMgr>
{
    /// <summary>
    /// UI 根节点
    /// </summary>
    public Transform ui_Root;

    /// <summary>
    /// UI 背景父节点
    /// </summary>
    private Transform ui_Bg_Canvas;
    public Transform ui_Bg;

    /// <summary>
    /// UI 层级父节点
    /// </summary>
    private Transform ui_Canvas;
    public Transform ui_Bottom;
    public Transform ui_Down;
    public Transform ui_Mid;
    public Transform ui_Up;
    public Transform ui_Top;

    /// <summary>
    /// UI 事件系统
    /// </summary>
    public EventSystem ui_EventSystem;

    public void StartUp()
    {
        LogHelper.LogGreen("=== UIMgr 启动成功 ===");

        this.ui_Root        = GameObject.Find("UI_Root").transform;

        this.ui_Bg_Canvas   = this.ui_Root.Find("UI_Bg_Canvas").transform;
        this.ui_Bg          = this.ui_Bg_Canvas.Find("UI_Bg").transform;

        this.ui_Canvas      = this.ui_Root.Find("UI_Canvas").transform;
        this.ui_Bottom      = this.ui_Canvas.Find("UI_Bottom").transform;
        this.ui_Down        = this.ui_Canvas.Find("UI_Down").transform;
        this.ui_Mid         = this.ui_Canvas.Find("UI_Mid").transform;
        this.ui_Up          = this.ui_Canvas.Find("UI_Up").transform;
        this.ui_Top         = this.ui_Canvas.Find("UI_Top").transform;
        this.ui_EventSystem = this.ui_Root.Find("EventSystem").GetComponent<EventSystem>();

        DontDestroyOnLoad(this.ui_Root);
    }

    public void ShowUIView(string event_Show, GameObject parent = null)
    {
        GameFacade.Instance.SendNotification(event_Show, parent);
    }

    public void HideUIView(string event_Hide)
    {
        GameFacade.Instance.SendNotification(event_Hide);
    }

    public void HideAllViews()
    {
        foreach (Transform t in this.ui_Canvas.transform)
        {
            BaseUI baseUI = t.GetComponent<BaseUI>();
            if(baseUI != null)
            {
                baseUI.Hide();
            }
            else
            {
                t.gameObject.SetActive(false);
            }
        }
    }

    public void DestroyUIView(string name)
    {
        int lastIndex = name.LastIndexOf("/");
        if (lastIndex > 0)
        {
            name = name.Substring(lastIndex + 1);
        }

        Transform view = this.ui_Canvas.transform.Find(name);
        if (view)
        {
            GameObject.DestroyImmediate(view.gameObject);
        }
    }

    public void DestroyAllViews()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform t in this.ui_Canvas.transform)
        {
            children.Add(t);
        }

        for (int i = 0; i < children.Count; ++i)
        {
            GameObject.DestroyImmediate(children[i].gameObject);
        }
    }
}