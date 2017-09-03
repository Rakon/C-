using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Operandy
{
    class operandy
    {
        static void Main(string[] args)
        {
            int a = 8;
            int b = 5;
            decimal c = a / b;
            Console.WriteLine("Welkom: a = 8 en b is 5");
            Console.WriteLine("_-_-_-_-_-_-_-_-_-_-_-_-_-_");
            Console.Write("Som is a + b = ");
            Console.WriteLine(a + b);
            Console.Write("aftrekking is a - b = ");
            Console.WriteLine(a - b);
            Console.Write("Vermenigvuldiging is a * b = ");
            Console.WriteLine(a * b);
            Console.Write("deling is a / b = ");
            Console.WriteLine(c);
            Console.Write("Mod is a % b = ");
            Console.WriteLine(a % b);
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("Boolians");
            Console.Write("Klopt het dat a < b? =");
            Console.WriteLine(a < b);
            Console.Write("Klopt het dat a > b? =");
            Console.WriteLine(a > b);
            Console.Write("Klopt het dat b > a? =");
            Console.WriteLine(b > a);
            Console.Write("Klopt het dat b < a? =");
            Console.WriteLine(b < a);
            Console.ReadLine();

        }
    }
}
