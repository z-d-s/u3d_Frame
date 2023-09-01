using System;

/// <summary>
/// 定时器
/// </summary>
public class TimeHandler
{
    /// <summary>
    /// 执行间隔
    /// </summary>
    public int delay;

    /// <summary>
    /// 是否重复执行
    /// </summary>
    public bool repeat;

    /// <summary>
    /// 是否用帧率
    /// </summary>
    public bool userFrame;

    /// <summary>
    /// 执行时间
    /// </summary>
    public long exeTime;

    /// <summary>
    /// 处理方法
    /// </summary>
    public Delegate method;

    /// <summary>
    /// 参数
    /// </summary>
    public object[] args;

    /// <summary>
    /// 清理
    /// </summary>
    public void Clear()
    {
        method = null;
        args = null;
    }
}