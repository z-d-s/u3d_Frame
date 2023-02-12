/****************************************************
	文件：ABMgr.cs
	作者：小小泽
	邮箱: 1245615197@qq.com
	日期：2021.4.14 	
	功能：AB包管理器
*****************************************************/
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// AB包管理器
/// </summary>
public class AssetBundleMgr : MonoBaseSingleton<AssetBundleMgr>
{
    // 用于存储我们的主包
    private AssetBundle abMain = null;
    // 用于存储我们的主包的配置文件
    private AssetBundleManifest abManifest = null;

    // 用于管理我们项目中的所有包
    public Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    /// <summary>
    /// 获取包体统一前缀路径
    /// </summary>
    public string GetStreamingAssetsPath
    {
        get
        {
            // Unity官方的统一加载包体的前缀
            return Application.streamingAssetsPath + "/";

        }
    }

    /// <summary>
    /// 获取主包的目标平台
    /// </summary>
    public string ABMainName
    {
        get
        {
#if UNITY_IOS
			return "IOS";
#elif UNITY_ANDROID
			return "Android";
#else
            return "Window64";
#endif
        }
    }

    /// <summary>
    /// 同步加载主包和主包的配置文件
    /// </summary>
    private void LoadABMain()
    {
        // 同步加载主包
        abMain = AssetBundle.LoadFromFile(GetStreamingAssetsPath + ABMainName);

        // 同步加载主包的配置文件
        abManifest = abMain.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
    }

    /// <summary>
    /// 加载依赖包
    /// </summary>
    /// <param name="abName">目标包名</param>
    private void LoadABDependencies(string abName)
    {
        if (abMain == null)
            LoadABMain();
        AssetBundle ab = null;
        string[] abDependencies = null;
        abDependencies = abManifest.GetAllDependencies(abName);
        for (int i = 0; i < abDependencies.Length; i++)
        {
            if (!abDic.ContainsKey(abDependencies[i]))
            {
                ab = AssetBundle.LoadFromFile(GetStreamingAssetsPath + abDependencies[i]);
                abDic.Add(abDependencies[i], ab);
            }
        }
    }

    /// <summary>
    /// 同步加载包体资源的泛型方法
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <param name="abName">包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="action">回调函数</param>
    /// <returns></returns>
    public T LoadRes<T>(string abName, string resName, UnityAction<T> action = null) where T : Object
    {
        LoadABDependencies(abName);
        AssetBundle ab = null;
        T o = null;
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(GetStreamingAssetsPath + abName);
            abDic.Add(abName, ab);
        }
        else
            ab = abDic[abName];
        o = ab.LoadAsset<T>(resName);
        if (o is GameObject)
            GameObject.Instantiate(o);
        if (action != null)
            action.Invoke(o);
        return o;
    }

    /// <summary>
    /// 通过Type获取资源类型的同步加载包体资源方法
    /// </summary>
    /// <param name="abName">包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="type">资源类型</param>
    /// <returns></returns>
    public Object LoadRes(string abName, string resName, System.Type type)
    {
        LoadABDependencies(abName);
        AssetBundle ab = null;
        Object o = null;
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(GetStreamingAssetsPath + abName);
            abDic.Add(abName, ab);
        }
        else
            ab = abDic[abName];
        o = ab.LoadAsset(resName, type);
        if (o is GameObject)
            GameObject.Instantiate(o as GameObject);
        return o;
    }

    /// <summary>
    /// 同步加载包体资源方法
    /// </summary>
    /// <param name="abName">包名</param>
    /// <param name="resName">资源名</param>
    /// <returns></returns>
    public Object LoadRes(string abName, string resName)
    {
        LoadABDependencies(abName);
        AssetBundle ab = null;
        Object o = null;
        if (!abDic.ContainsKey(abName))
        {
            ab = AssetBundle.LoadFromFile(GetStreamingAssetsPath + abName);
            abDic.Add(abName, ab);
        }
        else
            ab = abDic[abName];
        o = ab.LoadAsset(resName);
        if (o is GameObject)
            GameObject.Instantiate(o as GameObject);
        return o;
    }

    /// <summary>
    /// 异步加载包体资源的泛型方法
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <param name="abName">包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="callback">回调函数</param>
    public void LoadAsyncRes<T>(string abName, string resName, UnityAction<T> callback = null) where T : Object
    {
        // 开启协程
        StartCoroutine(Ie_LoadAsyncRes(abName, resName, callback));
    }

    /// <summary>
    /// 异步加载包体资源的泛型协程
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <param name="abName">包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="callback">回调函数</param>
    /// <returns></returns>
    private IEnumerator Ie_LoadAsyncRes<T>(string abName, string resName, UnityAction<T> callback = null) where T : Object
    {
        yield return null;
        LoadABDependencies(abName);
        AssetBundleCreateRequest abCReq = null;
        AssetBundle ab = null;
        AssetBundleRequest abReq = null;
        if (!abDic.ContainsKey(abName))
        {
            do
            {
                abCReq = AssetBundle.LoadFromFileAsync(GetStreamingAssetsPath + abName);
            } while (abCReq != null && abCReq.isDone);
            abDic.Add(abName, abCReq.assetBundle);
        }
        ab = abDic[abName];
        do
        {
            abReq = ab.LoadAssetAsync<T>(resName);
        } while (abReq != null && abReq.isDone);
        if (callback != null)
        {
            if (abReq.asset == null)
            {
                Debug.Log(GetStreamingAssetsPath + abName + "/" + resName + "is Null");
            }
            else
            {
                if (abReq.asset is GameObject)
                    callback(GameObject.Instantiate(abReq.asset as T));
                else
                    callback(abReq.asset as T);
            }
        }
    }

    /// <summary>
    /// 通过Type获取资源类型的异步加载包体资源方法
    /// </summary>
    /// <param name="abName">包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="type">资源类型</param>
    /// <param name="callback">回调函数</param>
    public void LoadAsyncRes(string abName, string resName, System.Type type, UnityAction<Object> callback = null)
    {
        StartCoroutine(Ie_LoadAsyncRes(abName, resName, type, callback));
    }

    /// <summary>
    /// 通过Type获取资源类型的异步加载包体资源协程
    /// </summary>
    /// <param name="abName">包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="type">资源类型</param>
    /// <param name="callback">回调函数</param>
    /// <returns></returns>
    private IEnumerator Ie_LoadAsyncRes(string abName, string resName, System.Type type, UnityAction<Object> callback = null)
    {
        yield return null;
        LoadABDependencies(abName);
        AssetBundleCreateRequest abCReq = null;
        AssetBundle ab = null;
        AssetBundleRequest abReq = null;
        if (!abDic.ContainsKey(abName))
        {
            do
            {
                abCReq = AssetBundle.LoadFromFileAsync(GetStreamingAssetsPath + abName);
            } while (abCReq != null && abCReq.isDone);
            abDic.Add(abName, abCReq.assetBundle);
        }
        ab = abDic[abName];
        do
        {
            abReq = ab.LoadAssetAsync(resName, type);
        } while (abReq != null && abReq.isDone);
        if (callback != null)
        {
            if (abReq.asset == null)
            {
                Debug.Log(GetStreamingAssetsPath + abName + "/" + resName + "is Null");
            }
            else
            {
                if (abReq.asset is GameObject)
                    callback(GameObject.Instantiate(abReq.asset));
                else
                    callback(abReq.asset);
            }
        }
    }

    /// <summary>
    /// 异步加载包体资源方法
    /// </summary>
    /// <param name="abName"></param>
    /// <param name="resName"></param>
    /// <param name="callback"></param>
    public void LoadAsyncRes(string abName, string resName, UnityAction<Object> callback = null)
    {
        StartCoroutine(Ie_LoadAsyncRes(abName, resName, callback));
    }

    /// <summary>
    /// 异步加载包体资源协程
    /// </summary>
    /// <param name="abName">包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="callback">回调函数</param>
    /// <returns></returns>
    private IEnumerator Ie_LoadAsyncRes(string abName, string resName, UnityAction<Object> callback = null)
    {
        yield return null;
        LoadABDependencies(abName);
        AssetBundleCreateRequest abCReq = null;
        AssetBundle ab = null;
        AssetBundleRequest abReq = null;
        if (!abDic.ContainsKey(abName))
        {
            do
            {
                abCReq = AssetBundle.LoadFromFileAsync(GetStreamingAssetsPath + abName);
            } while (abCReq != null && abCReq.isDone);
            abDic.Add(abName, abCReq.assetBundle);
        }
        ab = abDic[abName];
        do
        {
            abReq = ab.LoadAssetAsync(resName);
        } while (abReq != null && abReq.isDone);
        if (callback != null)
        {
            if (abReq.asset == null)
            {
                Debug.Log(GetStreamingAssetsPath + abName + "/" + resName + "is Null");
            }
            else
            {
                if (abReq.asset is GameObject)
                    callback(GameObject.Instantiate(abReq.asset));
                else
                    callback(abReq.asset);
            }
        }
    }

    /// <summary>
    /// 卸载目标包
    /// </summary>
    /// <param name="abName"></param>
    public void UnLoadAB(string abName)
    {
        // 卸载包但是保留已引用的资源
        abDic[abName].Unload(false);
        // 从容器中移除
        abDic.Remove(abName);
    }

    /// <summary>
    /// 卸载所有包
    /// </summary>
    public void UnLoadAllAB()
    {
        // 卸载所有包但是保留已引用的资源
        AssetBundle.UnloadAllAssetBundles(false);
        // 清空容器
        abDic.Clear();
    }
}
