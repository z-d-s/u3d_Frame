using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBaseSingleton<UIMgr>
{
    /// <summary>
    /// (UI Canvas 及 UI事件系统) 根节点
    /// </summary>
    public Transform uiRoot;

    /// <summary>
    /// UI 父节点
    /// </summary>
    public Transform canvas;

    public void StartUp()
    {
        LogHelper.LogGreen("=== UIMgr 启动成功 ===");

        this.uiRoot = GameObject.Find("UIRoot").transform;
        this.canvas = uiRoot.Find("Canvas").transform;
        DontDestroyOnLoad(this.uiRoot);
    }

    public void ShowUIView(string event_Show, GameObject parent = null)
    {
        AppFacade.Instance.SendNotification(event_Show, parent);
    }

    public void HideUIView(string event_Hide)
    {
        AppFacade.Instance.SendNotification(event_Hide);
    }

    public void HideAllViews()
    {
        foreach (Transform t in this.canvas.transform)
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

        Transform view = this.canvas.transform.Find(name);
        if (view)
        {
            GameObject.DestroyImmediate(view.gameObject);
        }
    }

    public void DestroyAllViews()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform t in this.canvas.transform)
        {
            children.Add(t);
        }

        for (int i = 0; i < children.Count; ++i)
        {
            GameObject.DestroyImmediate(children[i].gameObject);
        }
    }
}