using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMgr : MonoBaseSingleton<TableMgr>
{
    private Dictionary<string, Dictionary<string, Car>> storeTables = new Dictionary<string, Dictionary<string, Car>>();

    public void StartUp()
    {
        LogHelper.LogGreen("=== TableMgr 启动成功 ===");

        this.LoadTables();
    }

    private void LoadTables()
    {
        AssetsLoadMgr.Instance.LoadAsync("table", "Tables/Car.json", (string name, UnityEngine.Object obj) =>
        {
            TextAsset info = obj as TextAsset;
            Debug.Log(info.text);
            Dictionary<string, Car> _dic = this.DeserializeStringToDictionary<string, Car>(info.text);
            Debug.Log(_dic);
        });
    }

    public Dictionary<TKey, TValue> DeserializeStringToDictionary<TKey, TValue>(string jsonStr)
    {
        if (string.IsNullOrEmpty(jsonStr))
        {
            return new Dictionary<TKey, TValue>();
        }

        Dictionary<TKey, TValue> jsonDict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);
        return jsonDict;
    }
}
