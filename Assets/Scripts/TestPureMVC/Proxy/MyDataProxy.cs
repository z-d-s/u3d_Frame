using PureMVC.Interfaces;
using PureMVC.Patterns.Proxy;
using System.Collections.Concurrent;

public class MyDataProxy : Proxy
{
    public const string proxyName = "MyDataProxy_Name";
    private MyData mydata = null;

    public MyDataProxy(string proxyName, object data = null) : base(proxyName, data)
    {
        this.mydata = new MyData();
    }

        ConcurrentDictionary<string, int> testMap = new ConcurrentDictionary<string, int>();
    public void AddValue(int value = 1)
    {
        ++this.mydata.dataValue;
        SendNotification("msg_add_s", this.mydata);
    }

    public void SubValue(int value = 1)
    {
        --this.mydata.dataValue;
        SendNotification("msg_sub_s", this.mydata);
    }
}
