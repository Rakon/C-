using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BerekeningenMakenOefening
{
    class BerekeningenMakenOefening
    {
        static void Main(string[] args)
        {
            double hoogteCilinderInMeter = 1.8;
            double diameterCilinderInMeter = 1.65;
            double oppBase = diameterCilinderInMeter * diameterCilinderInMeter * Math.PI / 4;
            Console.WriteLine($"inhoud in liter {hoogteCilinderInMeter * oppBase * 1000}.");
            Console.ReadLine();

        }
    }
}
