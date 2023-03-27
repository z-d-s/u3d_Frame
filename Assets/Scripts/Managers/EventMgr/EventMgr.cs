/****************************************************

	EventMgr : 事件系统

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

public class EventMgr : BaseSingleton<EventMgr>
{

    private EventHub eventHub = new EventHub();


    public void AddListener(string eventId, EventListener<IEventArgs> listener)
    {
        this.eventHub.AddListener(eventId, listener);
    }

    public void RemoveListener(string eventId, EventListener<IEventArgs> listener)
    {
        this.eventHub.RemoveListener(eventId, listener);
    }

    public void Dispatch(string eventId, IEventArgs args = null)
    {
        this.eventHub.Dispatch(eventId, args);
    }
}
