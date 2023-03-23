using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBaseSingleton<UIMgr>
{
    public Transform uiRoot;
    public Transform canvas;

    public void Init()
    {
        this.uiRoot = GameObject.Find("UIRoot").transform;
        this.canvas = uiRoot.Find("Canvas").transform;
        DontDestroyOnLoad(this.uiRoot);
    }

    public BaseUI ShowUIView(string assetBundleName, string name)
    {
        string path = "GUI/UIPrefabs/" + name + ".prefab";
        GameObject ui_prefab = (GameObject)AssetsLoadMgr.Instance.LoadSync(assetBundleName, path);
        GameObject ui_view = GameObject.Instantiate(ui_prefab);
        ui_view.name = ui_prefab.name;
        ui_view.transform.SetParent(this.canvas.transform, false);

        int lastIndex = name.LastIndexOf("/");
        if (lastIndex > 0)
        {
            name = name.Substring(lastIndex + 1);
        }

        Type type = Type.GetType(name/* + "_UICtrl"*/);
        BaseUI ctrl = (BaseUI)ui_view.AddComponent(type);

        return ctrl;
    }

    public GameObject ShowSubView(string assetBundleName, string name, GameObject parent = null)
    {
        string path = "GUI/UI_Prefabs/" + name + ".prefab";
        GameObject ui_prefab = (GameObject)AssetsLoadMgr.Instance.LoadSync(assetBundleName, path);
        GameObject ui_view = GameObject.Instantiate(ui_prefab);
        ui_view.name = ui_prefab.name;
        if (parent == null)
        {
            parent = this.canvas.gameObject;
        }
        ui_view.transform.SetParent(parent.transform, false);
        return ui_view;
    }

    public void RemoveUIView(string name)
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

    public void RemoveAllViews()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform t in this.canvas.transform)
        {
            children.Add(t);
        }

        for (int i = 0; i < children.Count; i++)
        {
            GameObject.DestroyImmediate(children[i].gameObject);
        }
    }
}