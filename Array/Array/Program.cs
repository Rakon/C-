using System;


namespace Array
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = new int[3] { 2, 4, 6 };
            Console.WriteLine(numbers[0]);
            Console.WriteLine(numbers[1]);
            Console.WriteLine(numbers[2]);

            var flags = new bool[3] { true, false, true };
            Console.WriteLine(flags[0]);
            Console.WriteLine(flags[1]);
            Console.WriteLine(flags[2]);

            var namen = new string[4] { "Ellen", "David", "Prieel", "Merlijn" };
            Console.WriteLine(namen[0]);
            Console.WriteLine(namen[1]);
            Console.WriteLine(namen[2]);
            Console.WriteLine(namen[3]);
        }
    }
}
