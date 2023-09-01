/****************************************************

	多语言

    使用事例：
        方式1 -- this.text_01.GetComponent<LanguageText>().SetTextID("100001");
        方式2 -- LanguageData.SetTextByID(this.text_01.gameObject, "100001");

        ps：
            -- 如果[没有LanguageText组件]或者[有LanguageText组件 但ID为空]，则Text组件不受多语言控制
            -- 如果文字显示为"=NaN="，表示语言表中没有该ID
            -- 如果文字显示为"xxx"，表示语言表中ID没有对应语言类型

*****************************************************/

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
    /// 日文
    /// </summary>
    Japanese,
    /// <summary>
    /// 其他语言
    /// </summary>
    Other
}

public static class LanguageData
{
    /// <summary>
    /// 语言类型
    /// </summary>
    public static LanguageType type = LanguageType.Chinese;
    /// <summary>
    /// 语言表格数据存储
    /// </summary>
    public static Dictionary<string, Language> dic_Language = new Dictionary<string, Language>();
    /// <summary>
    /// 语言修改回调
    /// </summary>
    public static System.Action OnLocalize;

    /// <summary>
    /// 设置语言类型
    /// </summary>
    /// <param name="type">语言类型</param>
    public static void SetLanguage(LanguageType type)
    {
        LanguageData.type = type;
        if (LanguageData.OnLocalize != null)
        {
            LanguageData.OnLocalize();
        }
    }

    /// <summary>
    /// 获取语言文本
    /// </summary>
    /// <param name="id">文本ID</param>
    /// <returns></returns>
    public static string GetText(string id)
    {
        string text = "=NaN=";

        if (LanguageData.dic_Language.Keys.Count <= 0)
        {
#if UNITY_EDITOR
            TextAsset info = AssetDatabase.LoadAssetAtPath("Assets/AssetsPackage/Tables/Language.json", typeof(UnityEngine.Object)) as TextAsset;
            LanguageData.dic_Language = JsonConvert.DeserializeObject<Dictionary<string, Language>>(info.text);
#else
            TextAsset info = AssetsLoadMgr.Instance.LoadSync("table", "Tables/Language.json") as TextAsset;
            LanguageData.dic_Language = JsonConvert.DeserializeObject<Dictionary<string, Language>>(info.text);
#endif
        }

        if (LanguageData.dic_Language.TryGetValue(id, out Language data))
        {
            if (LanguageData.type == LanguageType.Chinese)
            {
                text = data.Chinese;
            }
            else if (LanguageData.type == LanguageType.English)
            {
                text = data.English;
            }
            else if (LanguageData.type == LanguageType.Japanese)
            {
                text = data.Japanese;
            }
            else
            {
                text = data.Other;
            }
        }
        return text;
    }

    /// <summary>
    /// 设置语言文本
    /// </summary>
    /// <param name="obj">要设置文本的物体</param>
    /// <param name="id">要设置文本的ID</param>
    public static void SetTextByID(GameObject obj, string id)
    {
        if (obj == null || id == "") return;

        LanguageText text = obj.GetComponent<LanguageText>();
        if(text != null)
        {
            text.SetTextID(id);
        }
    }
}