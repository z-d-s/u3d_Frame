using System.Collections.Generic;

public class EventHub
{
    private Dictionary<string, List<EventListener<IEventArgs>>> dic_Event = new Dictionary<string, List<EventListener<IEventArgs>>>();

    public void AddListener(string eventId, EventListener<IEventArgs> listener)
    {
        if(this.dic_Event == null)
        {
            return;
        }

        List<EventListener<IEventArgs>> list = null;
        this.dic_Event.TryGetValue(eventId, out list);
        if(list != null)
        {
            list.Add(listener);
        }
        else
        {
            list = new List<EventListener<IEventArgs>>();
            list.Add(listener);
            this.dic_Event[eventId] = list;
        }
    }

    public void RemoveListener(string eventId, EventListener<IEventArgs> listener)
    {
        if (this.dic_Event == null)
        {
            return;
        }

        List<EventListener<IEventArgs>> list = null;
        this.dic_Event.TryGetValue(eventId, out list);
        if (list != null && list.Contains(listener))
        {
            list.Remove(listener);
        }
    }

    public void Dispatch(string eventId, IEventArgs args = null)
    {
        if (this.dic_Event == null)
        {
            return;
        }

        List<EventListener<IEventArgs>> list = null;
        this.dic_Event.TryGetValue(eventId, out list);
        if(list == null || list.Count <= 0)
        {
            return;
        }

        for(int i = 0; i < list.Count; ++i)
        {
            if(list[i] != null)
            {
                list[i].Invoke(args);
            }
        }
    }
}
