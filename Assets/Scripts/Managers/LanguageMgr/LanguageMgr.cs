using System.Collections;
using System.Collections.Generic;
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

public class LanguageMgr : MonoBaseSingleton<LanguageMgr>
{
    
}
