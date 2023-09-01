using UnityEngine;

/// <summary>
/// 普通的单例基类
/// </summary>
/// <typeparam name="T">类型</typeparam>
public abstract class BaseSingleton<T> where T : new()
{
    private static T _instance;
    private static object mutex = new object();
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // 保证我们的单例，是线程安全的;
                lock (mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
            }
            return _instance;
        }
    }
}