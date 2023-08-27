using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class LanguageSwitch : Editor
{
    [MenuItem("语言/添加LanguageText", false, 1)]
    public static void AddLanguageText()
    {
        LanguageSwitch.AddLanguageTextToAllPrefabs();
    }

    [MenuItem("语言/语言切换/中文", false, 1)]
    public static void ChangeCn()
    {
        LanguageMgr.type = LanguageType.Chinese;
        LanguageSwitch.SetLanguage();
    }
    [MenuItem("语言/语言切换/中文", true)]
    public static bool CNToggle()
    {
        Menu.SetChecked("语言切换/中文", LanguageMgr.type == LanguageType.Chinese);
        return true;
    }

    [MenuItem("语言/语言切换/英文", false, 2)]
    public static void ChangeEn()
    {
        LanguageMgr.type = LanguageType.English;
        LanguageSwitch.SetLanguage();
    }
    [MenuItem("语言/语言切换/英文", true)]
    public static bool ENToggle()
    {
        Menu.SetChecked("语言/语言切换/英文", LanguageMgr.type == LanguageType.English);
        return true;
    }

    private static void SetLanguage()
    {
        if (LanguageMgr.OnLocalize != null)
        {
            LanguageMgr.OnLocalize();
        }

        if (Application.isPlaying == false)
        {
            LanguageSwitch.RefreshPrefab();
        }
    }

    /// <summary>
    /// 获取所有预制体
    /// </summary>
    private static List<GameObject> GetAllPrefab()
    {
        List<GameObject> list_Prefabs = new List<GameObject>();
        string assetsPath = Application.dataPath + "/AssetsPackage/";
        string[] absolutePath = Directory.GetFiles(assetsPath, "*.prefab", SearchOption.AllDirectories);
        int prefabCount = absolutePath.Length;
        for (int i = 0; i < prefabCount; ++i)
        {
            EditorUtility.DisplayProgressBar("获取项目预制体", "获取预制体中...", (float)(i / prefabCount));
            string path = "Assets/AssetsPackage/" + absolutePath[i].Remove(0, assetsPath.Length);
            path = path.Replace("\\", "/");
            GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            if (prefab != null)
            {
                list_Prefabs.Add(prefab);
            }
        }
        EditorUtility.ClearProgressBar();
        return list_Prefabs;
    }

    /// <summary>
    /// 为所有预制体中包含Text组件的节点添加LanguageText组件
    /// </summary>
    private static void AddLanguageTextToAllPrefabs()
    {
        List<GameObject> list_Prefabs = LanguageSwitch.GetAllPrefab();
        int prefabCount = list_Prefabs.Count;
        for (int i = 0; i < prefabCount; ++i)
        {
            EditorUtility.DisplayProgressBar("预制体添加LanguageText", "LanguageText添加中...", (float)(i / prefabCount));
            foreach (Transform trans in list_Prefabs[i].GetComponentInChildren<Transform>(true))
            {
                UnityEngine.UI.Text text = trans.GetComponent<UnityEngine.UI.Text>();
                LanguageText languageText = trans.GetComponent<LanguageText>();
                if (text != null && languageText == null)
                {
                    trans.gameObject.AddComponent<LanguageText>();
                }
            }
            AssetDatabase.SaveAssets();
        }
        EditorUtility.ClearProgressBar();
        RefreshScenePrefab();
    }

    /// <summary>
    /// 游戏没有运行时获取所有预制体，修改其中Text组件内容
    /// </summary>
    private static void RefreshPrefab()
    {
        List<GameObject> list_Prefabs = LanguageSwitch.GetAllPrefab();
        int prefabCount = list_Prefabs.Count;
        for (int i = 0; i < prefabCount; ++i)
        {
            EditorUtility.DisplayProgressBar("刷新预制体", "预制体刷新中...", (float)(i / prefabCount));
            foreach (Transform trans in list_Prefabs[i].GetComponentInChildren<Transform>(true))
            {
                LanguageText languageText = trans.GetComponent<LanguageText>();
                UnityEngine.UI.Text text = trans.GetComponent<UnityEngine.UI.Text>();
                if (text != null && languageText != null)
                {
                    languageText.Localize();
                }
            }
            AssetDatabase.SaveAssets();
        }
        EditorUtility.ClearProgressBar();
        RefreshScenePrefab();
    }

    /// <summary>
    /// 刷新场景中的预制体
    /// </summary>
    private static void RefreshScenePrefab()
    {
        LanguageText[] languageTexts = GameObject.FindObjectsOfType<LanguageText>();
        for (int i = 0; i < languageTexts.Length; ++i)
        {
            languageTexts[i].Localize();
            EditorUtility.SetDirty(languageTexts[i]);
        }
    }
}