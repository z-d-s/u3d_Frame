/****************************************************

    事件系统

    使用事例：
        EventMgr.Instance.AddListener("xxx", this.XXXFunc);
        EventMgr.Instance.RemoveListener("xxx", this.XXXFunc);
        EventMgr.Instance.Dispatch("xxx");
        EventMgr.Instance.Dispatch("xxx", EventArgs<string>.CreateEventArgs("=== xxx ==="));
        private void XXXFunc(IEventArgs args)
        {
            if(args == null)
            {
                return;
            }
            Debug.Log(args.GetValue<string>());
        }

*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EventListener<T>(T arg);

public class EventMgr : BaseSingleton<EventMgr>
{
    private EventHub eventHub = new EventHub();

    /// <summary>
    /// 添加监听事件
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="listener"></param>
    public void AddListener(string eventId, EventListener<IEventArgs> listener)
    {
        this.eventHub.AddListener(eventId, listener);
    }

    /// <summary>
    /// 移除监听事件
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="listener"></param>
    public void RemoveListener(string eventId, EventListener<IEventArgs> listener)
    {
        this.eventHub.RemoveListener(eventId, listener);
    }

    /// <summary>
    /// 发送事件
    /// </summary>
    /// <param name="eventId">事件ID</param>
    /// <param name="args">事件参数 缺省为null</param>
    public void Dispatch(string eventId, IEventArgs args = null)
    {
        this.eventHub.Dispatch(eventId, args);
    }
}
