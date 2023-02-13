using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HashSetQueue<T> : IEnumerable
{
    private Queue<T> _queue;
    private readonly HashSet<T> _hashset;

    public HashSetQueue()
    {
        _queue = new Queue<T>();
        _hashset = new HashSet<T>();
    }

    public HashSetQueue(IList<T> collection, bool setCapacity = false)
    {
        _hashset = collection.ToHashSet();
        _queue = new Queue<T>(collection);

        if (!setCapacity)
            return;
    }

    public int Count => _queue.Count;
    public int Length => Count;

    public virtual T Dequeue()
    {
        var value = _queue.Dequeue();
        _hashset.Remove(value);
        return value;
    }
        
    public virtual bool Enqueue(T item)
    {
        _hashset.Add(item);
        _queue.Enqueue(item);
        return true;
    }

    public virtual bool Remove(T item)
    {
        var removed = _hashset.Remove(item);
        _queue = new Queue<T>(_hashset);
        return removed;
    }

    private IEnumerator _enumerator;
    public IEnumerator GetEnumerator() => _enumerator != null ? _enumerator : (_hashset as IEnumerable).GetEnumerator();
}

public class HashSetListQueue<T> : HashSetQueue<T>
{
    private readonly List<T> _list;
    
    public HashSetListQueue() : base()
    {
        _list = new List<T>();
    }

    public HashSetListQueue(IList<T> collection, bool useCapacity) : base(collection, useCapacity)
    {
        _list = collection.ToList();

        if (useCapacity)
            _list.Capacity = collection.Count;
    }

    public override T Dequeue()
    {
        _list.RemoveAt(0);
        return base.Dequeue();
    }

    public override bool Enqueue(T item)
    {
        var success =  base.Enqueue(item);

        if (success)
        {
            _list.Add(item);
        }

        return success;
    }

    public override bool Remove(T item)
    {
        var removed = base.Remove(item);
        var listRemoved = _list.Remove(item);

        if (removed != listRemoved)
        {
            Console.Error.Write($"Underlying list, queue, and hashset have desynchronized!");
        }
        
        return removed || listRemoved;
    }

    private T this[int index] => _list[index];
}