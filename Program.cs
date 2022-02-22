using System;
using System.Collections.Generic;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    class Queue<T>
        where T: IComparable<T>
    {
        T max;
        LinkedList<Tuple<T,T>> queue = new LinkedList<Tuple<T,T>>();

        public int Compare(T other)
        {
            if (max.CompareTo(other) > 0)
            {
                return 1;
            }
            else
            if (max.CompareTo(other) < 0)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        public void Push(T item)
        {
            max = queue.Last.Value.Item2;
            if (Compare(item) == 1)
                queue.AddLast(Tuple.Create(item, max));
            else
                queue.AddLast(item, queue[queue.Count - 1].Item2);
        }

        public T Pop()
        {
            if (queue.Count == 0) throw new InvalidOperationException();
            var result = queue.Last.Value;
            queue.RemoveLast();
            return result.Item1;
        }

        public T Max
        {
            get
            {
                return queue.Last.Value.Item2;
            }
        }
    }
}
