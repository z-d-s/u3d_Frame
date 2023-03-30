using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolData
{
    public GameObject fatherObj;

    public List<GameObject> poolList;

    public PoolData(GameObject obj, GameObject poolObj)
    {
        this.fatherObj = new GameObject("pool_" + obj.name);
        this.fatherObj.transform.parent = poolObj.transform;
        this.poolList = new List<GameObject> { obj };
        obj.transform.parent = fatherObj.transform;
        obj.SetActive(false);
    }

    public void RecycleObject(GameObject obj)
    {
        obj.SetActive(false);
        this.poolList.Add(obj);
        obj.transform.parent = this.fatherObj.transform;
    }

    public GameObject GetObject()
    {
        GameObject obj = this.poolList[0];
        this.poolList.RemoveAt(0);
        obj.SetActive(true);
        obj.transform.parent = null;
        return obj;
    }
}
