using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 语言类型
/// </summary>
public enum LanguageType
{
    /// <summary>
    /// 默认
    /// </summary>
    Default,
    /// <summary>
    /// 中文
    /// </summary>
    Chinese,
    /// <summary>
    /// 英文
    /// </summary>
    English,
    /// <summary>
    /// 其他语言
    /// </summary>
    Other
}

public class LanguageData
{
    public static Dictionary<string, Language> dic_Language = new Dictionary<string, Language>();
    public static string Get(string id)
    {
        string value = "xxx";

        if (LanguageData.dic_Language.Keys.Count <= 0)
        {
            TextAsset info = AssetDatabase.LoadAssetAtPath("Assets/AssetsPackage/Tables/Language.json", typeof(UnityEngine.Object)) as TextAsset;
            LanguageData.dic_Language = JsonConvert.DeserializeObject<Dictionary<string, Language>>(info.text);
        }

        if (LanguageData.dic_Language.TryGetValue(id, out Language lan))
        {
            if (LanguageMgr.type == LanguageType.Chinese)
            {
                value = lan.Chinese;
            }
            else if (LanguageMgr.type == LanguageType.English)
            {
                value = lan.English;
            }
            else
            {
                value = "=NaN=";
            }
        }
        return value;
    }

    private Dictionary<TKey, TValue> DeserializeStringToDictionary<TKey, TValue>(string jsonStr)
    {
        if (string.IsNullOrEmpty(jsonStr))
        {
            return new Dictionary<TKey, TValue>();
        }

        Dictionary<TKey, TValue> jsonDict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);
        return jsonDict;
    }
}

public class LanguageMgr : MonoBaseSingleton<LanguageMgr>
{
    public static LanguageType type = LanguageType.Chinese;

    public delegate void DelegateOnLocalize();
    public static DelegateOnLocalize OnLocalize;

    public void SetLanguage(LanguageType type)
    {
        LanguageMgr.type = type;
        if (LanguageMgr.OnLocalize != null)
        {
            LanguageMgr.OnLocalize();
        }
    }
}
