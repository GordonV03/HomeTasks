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
    //aa
    class Queue<T>
        where T: IComparable<T>
    {
        LinkedList<T> stack1 = new LinkedList<T>();
        LinkedList<T> stack2 = new LinkedList<T>();

        public void Push(T item)
        {
            stack1.AddLast(item);
        }

        public T Pop()
        {
            if (stack2.Count == 0)
                while (stack1.Count != 0)
                    stack2.AddLast(stack1.Last);
            return stack2.Last.Value;
        }
    }
}
