using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

public class HttpClient
{
    private Encry _encry = new Encry("testkey");
    private Thread _sendTask;

    private BlockQueue<HttpPack> _sendList = new BlockQueue<HttpPack>(10);
    private BlockQueue<HttpPack> _receiveList = new BlockQueue<HttpPack>(10);

    public HttpClient()
    {
        this._sendTask = StartDaemonThread(this.StartSend);
    }

    private static Thread StartDaemonThread(ThreadStart threadMethod)
    {
        Thread thread = new Thread(threadMethod);
        thread.IsBackground = true;
        thread.Start();
        return thread;
    }

    private void StartSend()
    {
        while(true)
        {
            this.HandleSend();
        }
    }

    private void HandleSend()
    {
        if(this._sendList.Count > 0)
        {
            HttpPack pack = this._sendList.Dequeue();
            switch(pack.type)
            {
                case HttpType.Get:
                    this.Get(ref pack, pack.url, pack.encry);
                    this.HandleRecive(ref pack);
                    break;
                case HttpType.Post:
                    this.Post(ref pack, pack.url, pack.param, pack.encry);
                    this.HandleRecive(ref pack);
                    break;
            }
        }
    }

    private void HandleRecive(ref HttpPack pack)
    {
        // 异常时重试
        if (pack.code != 200 && pack.tryNum > 0)
        {
            pack.tryNum--;
            this._sendList.Enqueue(pack);
        }
        else
        {
            this._receiveList.Enqueue(pack);
        }
    }

    private void Get(ref HttpPack pack, string url, bool encry, int timeout = 2000, Encoding encode = null)
    {
        HttpWebRequest req = null;
        HttpWebResponse res = null;
        req = (HttpWebRequest)WebRequest.Create(url);
        req.AllowAutoRedirect = false;
        req.Timeout = timeout;

        try
        {
            res = (HttpWebResponse)req.GetResponse();
        }
        catch(WebException ex)
        {
            res = (HttpWebResponse)ex.Response;
        }

        pack.code = ((int)res.StatusCode);
        using (Stream stream = res.GetResponseStream())
        {
            pack.response = GetStrResponse(stream, encry, encode);
        }

        res.Close();
        req.Abort();
    }

    private void Post(ref HttpPack pack, string url, string postdata, bool encry, int timeout = 2000, Encoding encode = null)
    {
        HttpWebRequest req;
        HttpWebResponse res;
        encode = encode ?? Encoding.UTF8;
        req = (HttpWebRequest)WebRequest.Create(new Uri(url));
        req.Method = "POST";
        req.Timeout = timeout;

        if (postdata != null)
        {
            byte[] byteArray = encode.GetBytes(postdata);
            Stream stream = req.GetRequestStream();
            stream.Write(byteArray, 0, byteArray.Length);
            stream.Close();
        }

        try
        {
            res = (HttpWebResponse)req.GetResponse();
            pack.code = (int)res.StatusCode;
            using (Stream resStream = res.GetResponseStream())
            {
                pack.response = GetStrResponse(resStream, encry, encode);
            }
            res.Close();
        }
        catch (WebException ex)
        {
            pack.code = (int)ex.Status;
        }

        req.Abort();
    }

    private string GetStrResponse(Stream stream, bool encry, Encoding encode)
    {
        using (StreamReader reader = new StreamReader(stream))
        {
            string str = reader.ReadToEnd();
            if (encry)
            {
                byte[] unEncryBytes = encode.GetBytes(str);
                _encry.DoEncry(unEncryBytes);
                return encode.GetString(unEncryBytes);
            }
            else
            {
                return str;
            }
        }
    }

    #region 对外接口
    public bool TryGetResponse(out HttpPack pack)
    {
        if(this._receiveList.Count > 0)
        {
            pack = this._receiveList.Dequeue();
            return true;
        }
        else
        {
            pack = null;
            return false;
        }
    }

    public void AddHttpReq(int key, HttpType type, string url, bool encry, string param = null)
    {
        HttpPack pack = new HttpPack();
        pack.key = key;
        pack.type = type;
        pack.url = url;
        pack.param = param;
        pack.encry = encry;
        pack.tryNum = 3;
        this._sendList.Enqueue(pack);
    }
    #endregion
}
