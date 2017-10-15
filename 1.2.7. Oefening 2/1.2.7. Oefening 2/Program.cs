using System;
class BerekeningenMakenOefening2
{
    static void Main()
    {
        double hoogteCilinderInMeter = 1.8;
        double diameterCilinderInMeter = 1.65;
        double oppgrondvlak = ((diameterCilinderInMeter * diameterCilinderInMeter) * Math.PI) / 4;
        double inhoudliter = (oppgrondvlak * hoogteCilinderInMeter)* 1000;

        Console.WriteLine($"Inhoud in liter: {inhoudliter}");

        Console.ReadLine();
    }
}