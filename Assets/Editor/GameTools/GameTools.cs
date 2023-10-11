using DG.Tweening.Plugins.Core.PathCore;
using JetBrains.Annotations;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GameTools : Editor
{
    [MenuItem("Tools/PrintChildPath", false, 2003)]
    public static void PrintChildPath()
    {
        if(Selection.gameObjects.Length > 0)
        {
            for (int i = 0, j = Selection.gameObjects.Length; i < j; ++i)
            {
                PrintPath(Selection.gameObjects[i]);
            }
        }
        else
        {
            GUILayout.Label("没有选中的物体");
        }
    }

    private static void PrintPath(GameObject obj)
    {
        string path = obj.name;
        while(obj.transform.parent != null)
        {
            path = obj.transform.parent.name + "/" + path;
            obj = obj.transform.parent.gameObject;
        }
        Debug.Log("路径：" + path);
    }

    [MenuItem("Tools/CleanupMissingScripts", false, 2004)]
    public static void CleanupMissingScripts()
    {
        if (Selection.gameObjects.Length > 0)
        {
            for(int i = 0, j = Selection.gameObjects.Length; i < j; ++i)
            {
                CleanupScript(Selection.gameObjects[i]);
            }
        }
        else
        {
            GUILayout.Label("没有选中的物体");
        }
    }

    private static void CleanupScript(GameObject obj)
    {
        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
        for(int i = 0, j = obj.transform.childCount; i < j; ++i)
        {
            CleanupScript(obj.transform.GetChild(i).gameObject);
        }
    }
}