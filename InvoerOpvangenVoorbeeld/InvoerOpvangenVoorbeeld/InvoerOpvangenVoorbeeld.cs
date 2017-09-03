using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoerOpvangenVoorbeeld
{
    class InvoerOpvangenVoorbeeld
    {
        static void Main(string[] args)
        {
            Console.Write("Vul uw naam in: ");
            string naam = Console.ReadLine();
            Console.Write("Vul uw geboorte jaar in: ");
            string geboorteJaarInvoer = Console.ReadLine();
            Console.Write("Wat is uw lengte? : ");
            String lengteInvoer = Console.ReadLine();
            Console.Write("Vul uw gewicht in: ");
            string gewichtInvoer = Console.ReadLine();
            int geboorteJaar = int.Parse(geboorteJaarInvoer);
            double lengte = int.Parse(lengteInvoer);
            double gewicht = int.Parse(gewichtInvoer);
            double a = ((lengte/100) * (lengte/100));
            double BMI = gewicht / a ;
            DateTime nu = DateTime.Today;
            int Ouderdom = (nu.Year - geboorteJaar) -1;
            Console.WriteLine($"Uw naam is {naam}");
            Console.WriteLine($"U bent geboren in {geboorteJaar}");
            Console.WriteLine($"U bent {Ouderdom} jaar");
            Console.WriteLine($"U meet {lengte}");
            Console.WriteLine($"U Weegt {gewicht}");
            if (BMI < 25)
                {
                Console.WriteLine($"Uw BMI is :{BMI}, uw weegt te weinig!");
                }
            if (BMI > 28)
                {
                Console.WriteLine($"Uw BMI is :{BMI}, u bent te dik!");
                }
            if (BMI == 25 & BMI <= 28)
                {
                Console.WriteLine($"Uw BMI is : {BMI}, Perfect! Houden zo!");
                }      
            Console.ReadLine();
         }
    }
}
