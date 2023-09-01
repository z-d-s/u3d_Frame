/****************************************************

	对象池管理

    使用事例：
        AssetsLoadMgr.Instance.LoadAsync("effect_asset", "Effects/Prefabs/landcuts.prefab", (string name, UnityEngine.Object obj) =>
        {
	        GameObject o = PoolMgr.Instance.GetObject(obj.name, obj as GameObject);
	        TimeMgr.Instance.DoOnce(600, () =>
	        {
		        PoolMgr.Instance.RecycleObject(o.name, o);
	        });
        });

*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class PoolMgr : BaseSingleton<PoolMgr>
{
    private GameObject root_Pool;
    private Dictionary<string, PoolData> dic_Pool = new Dictionary<string, PoolData>();

    public GameObject GetObject(string name)
    {
        GameObject obj = null;
        if (this.dic_Pool.ContainsKey(name) && this.dic_Pool[name].poolList.Count > 0)
        {
            obj = this.dic_Pool[name].GetObject();
        }
        return obj;
    }

    public GameObject GetObject(string assetBundleName, string assetName)
    {
        GameObject template = AssetsLoadMgr.Instance.LoadSync(assetBundleName, assetName) as GameObject;
        return this.GetObject(template.name, template);
    }

    public GameObject GetObject(string name, GameObject template)
    {
        if(template == null)
        {
            return null;
        }

        GameObject obj = null;
        if(this.dic_Pool.ContainsKey(name) && this.dic_Pool[name].poolList.Count > 0)
        {
            obj = this.dic_Pool[name].GetObject();
            obj.SetActive(true);
        }
        else
        {
            obj = GameObject.Instantiate(template);
            obj.name = name;
        }
        return obj;
    }

    public void RecycleObject(string name, GameObject obj)
    {
        obj.SetActive(false);
        if(this.root_Pool == null)
        {
            this.root_Pool = new GameObject("Pool_Root");
        }
        obj.transform.parent = this.root_Pool.transform;
        if(this.dic_Pool.ContainsKey(name))
        {
            this.dic_Pool[name].RecycleObject(obj);
        }
        else
        {
            this.dic_Pool.Add(name, new PoolData(obj, root_Pool));
        }
    }

    public void Clear()
    {
        this.dic_Pool.Clear();
        this.root_Pool = null;
    }
}