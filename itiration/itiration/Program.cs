using System;
using System.Runtime.InteropServices.ComTypes;

namespace itiration
{
    class Program
    {
        static void Main(string[] args)
        {
            var name = "David Duym";

            for (var i = 0; i < name.Length; i++)
            {
                Console.WriteLine(name[i]);
            }
            foreach (var character in name)
            {
                Console.WriteLine(character);
            }
            var numbers = new int[] {1, 2, 3, 4};

            foreach (var number in numbers)
            {
                Console.WriteLine(number);                
            }
            for (var k = 1; k <= 100; k++)
            {
                if (k%2 == 0)
                    Console.WriteLine(k);
            }
            var j = 0;
            while (j <= 10)
            {
                if (j%2 == 0)
                    Console.WriteLine((j));
                j++;
            }
            while (true)
            {
                Console.Write("Type your name: ");
                var input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("@Echo: " + input);
                    continue;
                }
                break;

            }

        }
    }
}
