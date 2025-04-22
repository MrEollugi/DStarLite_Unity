using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    private readonly SortedSet<(T item, (float, float) key)> set = new(new KeyComparer());

    public int Count => set.Count;

    public void Enqueue(T item, (float, float) key) => set.Add((item, key));
    public T Dequeue()
    {
        var first = set.Min;
        set.Remove(first);
        return first.item;
    }
    public T Peek() => set.Min.item;

    public void Remove(T item)
    {
        foreach (var entry in set)
        {
            if (EqualityComparer<T>.Default.Equals(entry.item, item))
            {
                set.Remove(entry);
                break;
            }
        }
    }

    private class KeyComparer : IComparer<(T, (float, float))>
    {
        public int Compare((T, (float, float)) a, (T, (float, float)) b)
        {
            int cmp = a.Item2.Item1.CompareTo(b.Item2.Item1);
            if (cmp != 0) return cmp;
            return a.Item2.Item2.CompareTo(b.Item2.Item2);
        }
    }
}
