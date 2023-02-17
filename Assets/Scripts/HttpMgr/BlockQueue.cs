/**********************************
        多线程队列
**********************************/

using System.Collections.Generic;
using System.Threading;

public class BlockQueue<T>
{
    private readonly Queue<T> _queue;
    private readonly int _capacity;
    private bool _closing;

    public int Count
    {
        get
        {
            return _queue.Count;
        }
    }

    public BlockQueue(int capacity)
    {
        this._capacity = capacity;
        this._queue = new Queue<T>(capacity);
    }

    public void Enqueue(T item)
    {
        lock(_queue)
        {
            while(this._queue.Count >= this._capacity)
            {
                Monitor.Wait(_queue);
            }
            this._queue.Enqueue(item);
            if(this._queue.Count == 1)
            {
                Monitor.PulseAll(this._queue);
            }
        }
    }

    public T Dequeue()
    {
        lock(this._queue)
        {
            while(this._queue.Count == 0)
            {
                Monitor.Wait(this._queue);
            }
            T item = this._queue.Dequeue();
            if(this._queue.Count == this._capacity - 1)
            {
                Monitor.PulseAll(this._queue);
            }
            return item;
        }
    }

    public void Close()
    {
        lock(this._queue)
        {
            this._closing = true;
            Monitor.PulseAll(this._queue);
        }
    }

    public bool TryDequeue(out T value)
    {
        lock(this._queue)
        {
            while(this._queue.Count == 0)
            {
                if(this._closing)
                {
                    value = default(T);
                    return false;
                }
                Monitor.Wait(this._queue);
            }
            value = this._queue.Dequeue();
            if(this._queue.Count == this._capacity - 1)
            {
                Monitor.PulseAll(_queue);
            }
            return true;
        }
    }
}
