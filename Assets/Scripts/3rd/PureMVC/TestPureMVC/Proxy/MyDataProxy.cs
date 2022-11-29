using PureMVC.Patterns.Proxy;

public class MyDataProxy : Proxy
{
    public const string proxyName = "MyDataProxy_Name";
    private MyData mydata = null;

    public MyDataProxy(string proxyName, object data = null) : base(proxyName, data)
    {
        this.mydata = new MyData();
    }

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
