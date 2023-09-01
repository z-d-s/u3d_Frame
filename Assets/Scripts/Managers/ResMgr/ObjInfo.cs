using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjInfo : MonoBehaviour
{
    public int InstanceId = -1;
    public string AssetName = string.Empty;

    private void Awake()
    {
        if(string.IsNullOrEmpty(this.AssetName))
        {
            return;
        }

        //非空，说明通过克隆实例化，添加引用计数
        this.InstanceId = this.gameObject.GetInstanceID();
        PrefabLoadMgr.Instance.AddAssetRef(this.AssetName, this.gameObject);
    }

    private void OnDestroy()
    {
        //被动销毁，保证引用计数正确
        PrefabLoadMgr.Instance.Destroy(this.gameObject);
    }
}
