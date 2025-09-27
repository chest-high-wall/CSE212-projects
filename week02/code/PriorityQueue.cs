using System;
using System.Collections.Generic;

public class PriorityQueue
{
    private sealed class Item
    {
        public object Value { get; }
        public int Priority { get; }
        public long Seq { get; }   // enqueue order for FIFO tie-breaking
        public Item(object value, int priority, long seq)
        {
            Value = value;
            Priority = priority;
            Seq = seq;
        }
    }

    private readonly List<Item> _items = new List<Item>();
    private long _seqCounter = 0;

    // Adds to the "back" (append), preserving enqueue order
    public void Enqueue(object value, int priority)
    {
        _items.Add(new Item(value, priority, _seqCounter++));
    }

    // Removes the highest priority; ties resolved FIFO (earliest enqueued first)
    public object Dequeue()
    {
        if (_items.Count == 0)
            throw new InvalidOperationException("The queue is empty.");

        int bestIndex = 0;
        Item best = _items[0];

        for (int i = 1; i < _items.Count; i++)
        {
            var it = _items[i];
            if (it.Priority > best.Priority ||
               (it.Priority == best.Priority && it.Seq < best.Seq))
            {
                best = it;
                bestIndex = i;
            }
        }

        _items.RemoveAt(bestIndex);
        return best.Value;
    }

    public int Length => _items.Count;
}
