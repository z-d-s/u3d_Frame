using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum HttpType
{
    Get,
    Post,
}

public class HttpManager : MonoBaseSingleton<HttpManager>
{
    private HttpClient _httpClient = new HttpClient();
    private StringBuilder _getBuilder = new StringBuilder();

    public void HttpGet(int key, string url, bool encry, string param = null)
    {
        this._getBuilder.Clear();
        this._getBuilder.Append(url);
        if (param != null)
        {
            this._getBuilder.Append("?");
            this._getBuilder.Append(param);
        }
        this._httpClient.AddHttpReq(key, HttpType.Get, this._getBuilder.ToString(), encry);
    }

    public void HttpPost(int key, string url, bool encry, string param = null)
    {
        _getBuilder.Clear();
        _getBuilder.Append(url);
        _httpClient.AddHttpReq(key, HttpType.Post, _getBuilder.ToString(), encry, param);
    }

    public bool TryGetResPonse(ref int key, ref int code, ref string data)
    {
        HttpPack pack;
        if (_httpClient.TryGetResponse(out pack))
        {
            key = pack.key;
            code = pack.code;
            data = pack.response;
            //pack.Dispose();
            return true;
        }
        return false;
    }
}
