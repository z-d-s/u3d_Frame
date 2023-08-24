/****************************************************

	TableMgr - 表格管理

        使用事例：
                -- 获取表格             Car car = TableMgr.Instance.GetTable<Car>("1001");
                -- 获取表格中所有ID     List<string> list_IDs = TableMgr.Instance.GetTableIDs<Car>();
                -- 表格ID是否存在       bool b = TableMgr.Instance.ExistTableID<Car>("1001");

        ps：如果有新的表格添加，需要向TableDefine中添加表格同名的枚举

*****************************************************/

using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public enum TableDefine
{
    None = 0,
    Car,
    NPC,

    Max,    //此枚举始终在最后
}

public class TableMgr : MonoBaseSingleton<TableMgr>
{
    private Dictionary<string, Dictionary<string, object>> dic_Tables = new Dictionary<string, Dictionary<string, object>>();

    public void StartUp()
    {
        LogHelper.LogGreen("=== TableMgr 启动成功 ===");

        this.LoadTables();
    }

    private void LoadTables()
    {
        for(int i = 1; i < (int)TableDefine.Max; ++i)
        {
            this.LoadOneTable(((TableDefine)i).ToString());
        }
    }

    private void LoadOneTable(string tableName)
    {
        UnityEngine.Object obj = AssetsLoadMgr.Instance.LoadSync("table", string.Format("Tables/{0}.json", tableName));
        TextAsset info = obj as TextAsset;
        Dictionary<string, object> _dic = this.DeserializeStringToDictionary<string, object>(info.text);
        this.AddOneTable(tableName, _dic);
    }

    private void AddOneTable(string tableName, Dictionary<string, object> dic)
    {
        if(this.dic_Tables.ContainsKey(tableName))
        {
            return;
        }

        this.dic_Tables.Add(tableName, dic);
    }

    private Dictionary<TKey, TValue> DeserializeStringToDictionary<TKey, TValue>(string jsonStr)
    {
        if (string.IsNullOrEmpty(jsonStr))
        {
            return new Dictionary<TKey, TValue>();
        }

        Dictionary<TKey, TValue> jsonDict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);
        return jsonDict;
    }

    public T GetTable<T>(string ID)
    {
        foreach(var v in this.dic_Tables)
        {
            if(v.Key == typeof(T).Name)
            {
                foreach(var _id in v.Value)
                {
                    if(_id.Key == ID)
                    {
                        return JsonConvert.DeserializeObject<T>(_id.Value.ToString());
                    }
                }
            }
        }
        return default(T);
    }

    public List<string> GetTableIDs<T>()
    {
        List<string> list_IDs = new List<string>();
        foreach (var v in this.dic_Tables)
        {
            if (v.Key == typeof(T).Name)
            {
                foreach (var _id in v.Value)
                {
                    list_IDs.Add(_id.Key);
                }
                return list_IDs;
            }
        }
        return list_IDs;
    }

    public bool ExistTableID<T>(string ID)
    {
        foreach (var v in this.dic_Tables)
        {
            if (v.Key == typeof(T).Name)
            {
                foreach (var _id in v.Value)
                {
                    if (_id.Key == ID)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
