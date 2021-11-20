using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var number = Int32.Parse(Console.ReadLine());
            var exponent = Int32.Parse(Console.ReadLine());
            var fibonachiNumber = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Число возведенное в степень с помощью цикла: {0}" , GetPowerByWhile(number, exponent));
            Console.WriteLine("Число возведенное в степень с помощью рекурсии: {0}", GetPowerByRecursion(number, exponent));
            Console.WriteLine("Ваше число Фибоначчи: {0}", GetFibonachiNumber(fibonachiNumber));
        }

        public static double GetPowerByWhile(double number, int exponent)
        {
            if (exponent == 1)
                return 1.0;
            var tail = 1.0;
            while (exponent > 1)
            {
                if (exponent % 2 == 1)
                {
                    tail *= number;
                }
                number *= number;
                exponent /= 2;
            }
            return number * tail;
        }
        public static double GetPowerByRecursion(double number, int exponent)
        {
            if (exponent == 1)
                return 1.0;
            var tail = 1.0;
            for (int i = exponent; i > 1; i /= 2)
            {
                if (i % 2 == 1)
                {
                    tail *= number;
                }
                number *= number;
            }
            return number * tail;
        }

        public static int GetFibonachiNumber(int i)
        {
            var matrix22 = new int[,] { { 1, 1 }, { 1, 0 } };
            var matrix21 = new int[] { 1, 0 };
            matrix22 = GetPowerMatrix(matrix22, i);
            var fibonachiNumber = matrix21[0] * matrix22[0, 0] + matrix21[1] * matrix22[1, 0];
            return fibonachiNumber;
        }

        public static int[,] GetPowerMatrix(int [,] matrix22, int fibonachiNumber)
        {
            var a1 = matrix22[0, 0];
            var a2 = matrix22[0, 1];
            var b1 = matrix22[1, 0];
            var b2 = matrix22[1, 1];
            var newA1 = 0;
            var newA2 = 0;
            var newB1 = 0;
            var newB2 = 0;
            for (int i = 1; i < fibonachiNumber; i++)
            {
                newA1 = matrix22[0, 0] * a1 + matrix22[0, 1] * b1;
                newA2 = matrix22[0, 0] * a2 + matrix22[0, 1] * b2;
                newB1 = matrix22[1, 0] * a1 + matrix22[1, 1] * b1;
                newB2 = matrix22[1, 0] * a2 + matrix22[1, 1] * b2;
                a1 = newA1;
                a2 = newA2;
                b1 = newB1;
                b2 = newB2;
            }
            return new int[,] { {newA1, newA2 }, {newB1, newB2} };
        }
    }
}
