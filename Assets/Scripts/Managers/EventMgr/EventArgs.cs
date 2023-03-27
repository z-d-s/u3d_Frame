public interface IEventArgs
{

}

public class EventArgs<T> : IEventArgs
{
    public T arg;

    private EventArgs(T arg)
    {
        this.arg = arg;
    }

    public static IEventArgs CreateEventArgs(T val)
    {
        return new EventArgs<T>(val);
    }
}

public static class EventArgsExtend
{
    /// <summary>
    /// 获取事件参数值
    /// </summary>
    /// <param name="args"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetValue<T>(this IEventArgs args)
    {
        T result = default(T);
        if (args is EventArgs<T>)
        {
            result = ((EventArgs<T>)args).arg;
        }
        return result;
    }
}
