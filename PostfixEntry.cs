﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApp2
{
    class PostfixEntry
    {
        public static readonly char[] operations = new char[] { '+', '-', '*', ':' };
        public static readonly char[] numbers = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'};
        static void Main(string[] args)
        {
            string x = Console.ReadLine();
            for (int i = 0; i <= x.Length - 1; i++)
            {
                if (numbers.Contains(x[i]) == true)
                    Queue.Enqueue(x[i]);
                else
                if (operations.Contains(x[i]))
                    if (Stack.stack.Count() == 0)
                        Stack.Push(x[i]);
                    else
                    if (Value(Stack.stack.Last.Value) < Value(x[i]))
                        Stack.Push(x[i]);
                    else
                    if (Value(Stack.stack.Last.Value) >= Value(x[i]))
                    {
                        if (Stack.stack.Count() > 1)
                            while (Value(Stack.stack.Last.Value) >= Value(Stack.stack.Last.Previous.Value) && Stack.stack.Count() > 1)
                            {
                                Queue.Enqueue(Stack.stack.Last.Value);
                                Stack.Pop();
                            }
                        Queue.Enqueue(Stack.stack.Last.Value);
                        Stack.Pop();
                        Stack.Push(x[i]);
                    }
                else
                if (x[i] == '(')
                    Stack.Push(x[i]);
                else
                if (x[i] == ')')
                {
                    while (Stack.stack.Last.Value != '(')
                    {
                        Queue.Enqueue(Stack.stack.Last.Value);
                        Stack.Pop();
                    }
                    Stack.Pop();
                }    
            }
            while (Stack.stack.Count() != 0)
            {
                Queue.Enqueue(Stack.stack.Last.Value);
                Stack.Pop();
            }
            Queue.PrintQueue();
        }
        
        public static int Value(char item)
        {
            if (item == '-' || item == '+')
                return 1;
            if (item == '*' || item == ':')
                return 2;
            if (item == '(' || item == ')')
                return 0;
            else throw new InvalidOperationException();
        }
    }
    class Queue
    {
        public static LinkedList<char> stack1 = new LinkedList<char>();
        public static LinkedList<char> stack2 = new LinkedList<char>();

        public static void Enqueue(char item)
        {
            stack1.AddLast(item);
        }

        public static char Dequeue()
        {
            if (stack2.Count == 0)
                while (stack1.Count != 0)
                    stack2.AddLast(stack1.Last);
            return stack2.Last.Value;
        }

        public static void PrintQueue()
        {
            Console.WriteLine(Convert.ToString(stack1));
        }
    }

    class Stack
    {
        public static LinkedList<char> stack = new LinkedList<char>();
        public static void Push(char item)
        {
            stack.AddLast(item);
        }

        public static void Pop()
        {
            if (stack.Count == 0) throw new InvalidOperationException();
            stack.RemoveLast();
        }
    }
}