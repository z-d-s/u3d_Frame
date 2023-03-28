public interface IEventArgs
{

}

/// <summary>
/// 事件参数
/// 可以是值类型，也可以是引用类型
/// </summary>
/// <typeparam name="T"></typeparam>
public class EventArgs<T> : IEventArgs
{
    public T arg;

    private EventArgs(T arg)
    {
        this.arg = arg;
    }

    /// <summary>
    /// 创建事件参数
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static IEventArgs CreateEventArgs(T val)
    {
        return new EventArgs<T>(val);
    }
}

/// <summary>
/// 事件参数扩展
/// </summary>
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
