using UnityEngine;

public class ResManager : MonoBaseSingleton<ResManager>
{
    public T GetAssetCache<T>(string assetName) where T : Object
    {
        return new Object() as T;
    }
}