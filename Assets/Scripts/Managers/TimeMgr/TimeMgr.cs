/****************************************************

	事件管理系统

    使用事例：
        TimeMgr.Instance.DoOnce(2000, () =>
        {
            dosomething();
        });
        TimeMgr.Instance.DoOnce<int, int, int>(3000, (a, b, c) =>
        {
           dosomething();
        }, 123, 456, 789);

        TimeMgr.Instance.DoLoop(1000, this.Test);
        TimeMgr.Instance.ClearTime(this.Test);
        void Test()
	    {
            dosomething();
        }
        
    ps:同一函数多次计时，不会被后者覆盖

*****************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public delegate void Handler();
public delegate void Handler<T1>(T1 param1);
public delegate void Handler<T1, T2>(T1 param1, T2 param2);
public delegate void Handler<T1, T2, T3>(T1 param1, T2 param2, T3 param3);

public class TimeMgr : MonoBaseSingleton<TimeMgr>
{
        /// <summary>
    /// 游戏自启动运行帧数
    /// </summary>
    private int currFrame = 0;
    /// <summary>
    /// 游戏自启动运行时间(毫秒)
    /// </summary>
    private long currentTime
    {
        get
        {
            return (long)(Time.time * 1000);
        }
    }
    /// <summary>
    /// 缓存的定时器(执行后缓存起来，避免每次重新创建)
    /// </summary>
    private List<TimeHandler> poolHandlers = new List<TimeHandler>();
    /// <summary>
    /// 准备执行的定时器
    /// </summary>
    private List<TimeHandler> readyHandlers = new List<TimeHandler>();

    /// <summary>
    /// 启动
    /// </summary>
    public void StartUp()
    {
        LogHelper.LogGreen("=== TimeMgr 启动成功 ===");
    }

    public void Update()
    {
        this.currFrame++;
        for (int i = 0; i < this.readyHandlers.Count; i++)
        {
            TimeHandler handler = this.readyHandlers[i];
            long t = handler.userFrame ? this.currFrame : this.currentTime;
            if (t >= handler.exeTime)
            {
                Delegate method = handler.method;
                object[] args = handler.args;
                if (handler.repeat)
                {
                    while (t >= handler.exeTime)
                    {
                        handler.exeTime += handler.delay;
                        method.DynamicInvoke(args);
                    }
                }
                else
                {
                    this.RecycleHandler(handler.method);
                    method.DynamicInvoke(args);
                }
            }
        }
    }

    /// /// <summary>
    /// 定时执行一次(基于毫秒)
    /// </summary>
    /// <param name="delay">延迟时间(单位毫秒)</param>
    /// <param name="method">结束时的回调方法</param>
    /// <param name="args">回调参数</param>
    public void DoOnce(int delay, Handler method)
    {
        this.Create(false, false, delay, method);
    }
    public void DoOnce<T1>(int delay, Handler<T1> method, params object[] args)
    {
        this.Create(false, false, delay, method, args);
    }
    public void DoOnce<T1, T2>(int delay, Handler<T1, T2> method, params object[] args)
    {
        this.Create(false, false, delay, method, args);
    }
    public void DoOnce<T1, T2, T3>(int delay, Handler<T1, T2, T3> method, params object[] args)
    {
        this.Create(false, false, delay, method, args);
    }

    /// /// <summary>
    /// 定时重复执行(基于毫秒)
    /// </summary>
    /// <param name="delay">延迟时间(单位毫秒)</param>
    /// <param name="method">结束时的回调方法</param>
    /// <param name="args">回调参数</param>
    public void DoLoop(int delay, Handler method)
    {
        this.Create(false, true, delay, method);
    }
    public void DoLoop<T1>(int delay, Handler<T1> method, params object[] args)
    {
        this.Create(false, true, delay, method, args);
    }
    public void DoLoop<T1, T2>(int delay, Handler<T1, T2> method, params object[] args)
    {
        this.Create(false, true, delay, method, args);
    }
    public void DoLoop<T1, T2, T3>(int delay, Handler<T1, T2, T3> method, params object[] args)
    {
        this.Create(false, true, delay, method, args);
    }


    /// <summary>
    /// 定时执行一次(基于帧率)
    /// </summary>
    /// <param name="delay">延迟时间(单位为帧)</param>
    /// <param name="method">结束时的回调方法</param>
    /// <param name="args">回调参数</param>
    public void DoFrameOnce(int delay, Handler method)
    {
        this.Create(true, false, delay, method);
    }
    public void DoFrameOnce<T1>(int delay, Handler<T1> method, params object[] args)
    {
        this.Create(true, false, delay, method, args);
    }
    public void DoFrameOnce<T1, T2>(int delay, Handler<T1, T2> method, params object[] args)
    {
        this.Create(true, false, delay, method, args);
    }
    public void DoFrameOnce<T1, T2, T3>(int delay, Handler<T1, T2, T3> method, params object[] args)
    {
        this.Create(true, false, delay, method, args);
    }

    /// <summary>
    /// 定时重复执行(基于帧率)
    /// </summary>
    /// <param name="delay">延迟时间(单位为帧)</param>
    /// <param name="method">结束时的回调方法</param>
    /// <param name="args">回调参数</param>
    public void DoFrameLoop(int delay, Handler method)
    {
        this.Create(true, true, delay, method);
    }
    public void DoFrameLoop<T1>(int delay, Handler<T1> method, params object[] args)
    {
        this.Create(true, true, delay, method, args);
    }
    public void DoFrameLoop<T1, T2>(int delay, Handler<T1, T2> method, params object[] args)
    {
        this.Create(true, true, delay, method, args);
    }
    public void DoFrameLoop<T1, T2, T3>(int delay, Handler<T1, T2, T3> method, params object[] args)
    {
        this.Create(true, true, delay, method, args);
    }

    /// <summary>
    /// 清理定时器
    /// </summary>
    /// <param name="method">method为回调函数本身</param>
    public void ClearTime(Handler method)
    {
        this.RecycleHandler(method);
    }
    public void ClearTime<T1>(Handler<T1> method)
    {
        this.RecycleHandler(method);
    }
    public void ClearTime<T1, T2>(Handler<T1, T2> method)
    {
        this.RecycleHandler(method);
    }
    public void ClearTime<T1, T2, T3>(Handler<T1, T2, T3> method)
    {
        this.RecycleHandler(method);
    }

    /// <summary>
    /// 清理所有定时器
    /// </summary>
    public void ClearAllTime()
    {
        foreach (TimeHandler handler in this.readyHandlers)
        {
            this.RecycleHandler(handler.method);
            this.ClearAllTime();
            return;
        }
    }

    /// <summary>
    /// 创建定时器
    /// </summary>
    /// <param name="useFrame"></param>
    /// <param name="repeat"></param>
    /// <param name="delay"></param>
    /// <param name="method"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    private void Create(bool useFrame, bool repeat, int delay, Delegate method, params object[] args)
    {
        if (method == null)
        {
            return;
        }

        //如果执行时间小于1，直接执行
        if (delay < 1)
        {
            method.DynamicInvoke(args);
            return;
        }
        TimeHandler handler;
        if (this.poolHandlers.Count > 0)
        {
            handler = this.poolHandlers[this.poolHandlers.Count - 1];
            this.poolHandlers.Remove(handler);
        }
        else
        {
            handler = new TimeHandler();
        }
        handler.userFrame = useFrame;
        handler.repeat = repeat;
        handler.delay = delay;
        handler.method = method;
        handler.args = args;
        handler.exeTime = delay + (useFrame ? currFrame : currentTime);
        this.readyHandlers.Add(handler);
    }

    /// <summary>
    /// 回收定时器
    /// </summary>
    /// <param name="method"></param>
    private void RecycleHandler(Delegate method)
    {
        TimeHandler handler = this.readyHandlers.FirstOrDefault(t => t.method == method);
        if (handler != null)
        {
            this.readyHandlers.Remove(handler);
            handler.Clear();
            this.poolHandlers.Add(handler);
        }
    }
}