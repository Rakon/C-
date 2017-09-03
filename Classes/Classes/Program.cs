using program.calc;
using System;

namespace Classes
{
    class Program
    {
        static void Main(string[] args)
        {
            var David = new Person();
            David.VoorNaam = "David";
            David.AchterNaam = " Duym";
            David.Introduce();

            var calc = new calc();
            var result = calc.Add(1, 2);
            Console.WriteLine(result);
        }
    }
}
