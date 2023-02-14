using UnityEngine;
// using AssetBundles;

public class ResMgr : MonoBaseSingleton<ResMgr>
{
    public T GetAssetCache<T>(string name) where T : UnityEngine.Object
    {
#if UNITY_EDITOR
        string path = FileHelper.GetResPath() + name;
        Debug.Log(path);
        UnityEngine.Object target = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
        return target as T;
#endif
        return (new GameObject("t")) as T;
    }
}
