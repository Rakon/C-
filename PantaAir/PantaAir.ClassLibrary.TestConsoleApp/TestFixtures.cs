using System;
using System.Collections.Generic;
namespace ClassLibrary.TestConsoleApp
{
    class TestFixtures
    {
        static void Main()
        {
            TestBezetting();
            TestBezettingen();
            TestVlucht();

            Console.ReadLine();
        }

        public static void Test(bool assertion, string label = "")
        {
            ConsoleColor oorspronkelijkKleur = Console.ForegroundColor;
            ConsoleColor kleur = ConsoleColor.Red;
            if (assertion) kleur = ConsoleColor.Green;
            Console.ForegroundColor = kleur;
            Console.WriteLine(assertion.ToString().PadRight(6) + label);
            Console.ForegroundColor = oorspronkelijkKleur;
        }

        private static void TestBezetting()
        {
            Bezetting b = new Bezetting(zetel: "A1", passagiernaam: "Jan");
            Test(b.Zetel == "A1", "Constructor stelt Zetel property in.");
            Test(b.Passagiernaam == "Jan", "Constructor stelt Passagiernaam property in.");
            Test(b.Maaltijd == Maaltijd.Standaard, "By default standaard maaltijd.");

            b.Zetel = "B1";
            Test(b.Zetel == "B1", "Setter van Zetel property.");

            b.Passagiernaam = "Pol";
            Test(b.Passagiernaam == "Pol", "Setter van Passagiernaam property.");

            b.Maaltijd = Maaltijd.Vegetarisch;
            Test(b.Maaltijd == Maaltijd.Vegetarisch, "Setter van Maaltijd property.");

            Console.WriteLine();
        }
        private static void TestBezettingen()
        {
            Bezettingen bn = new Bezettingen();
            Test(bn.Aantal == 0, "Starten van een lege bezettingen lijst.");
            Test(bn.BevatBezettingMetZetel("A1") == false, "Geen bezetting teruggevonden voor zetel A1.");
            Test(bn.BevatBezettingMetZetel("A2") == false, "Geen bezetting teruggevonden voor zetel A2.");
            Test(bn.BevatBezettingMetZetel("A3") == false, "Geen bezetting teruggevonden voor zetel A3.");

            Bezetting b1 = new Bezetting("A1", "Jan") { Maaltijd = Maaltijd.Standaard };
            Bezetting b2 = new Bezetting("A2", "Pol S.") { Maaltijd = Maaltijd.Standaard };
            Bezetting b3 = new Bezetting("A3", "Jacques") { Maaltijd = Maaltijd.Vegetarisch };
            bn.Toevoegen(b1);
            Test(bn.Aantal == 1, "1 bezetting na toevoegen eerste bezetting (zetel A1).");
            Test(bn.AantalMaaltijden(Maaltijd.Standaard) == 1, "1 standaard maaltijd");
            Test(bn.AantalMaaltijden(Maaltijd.Vegetarisch) == 0, "0 vegetarische maaltijden");
            Test(bn[0] == b1, "Bezetting opvraagbaar op index 0.");
            Test(bn[1] == null, "Geen bezetting opvraagbaar op index 1.");
            Test(bn[2] == null, "Geen bezetting opvraagbaar op index 2.");
            Test(bn["A1"] == b1, "Bezetting opvraagbaar voor zetel A1.");
            Test(bn["A2"] == null, "Geen bezetting opvraagbaar voor zetel A2.");
            Test(bn["A3"] == null, "Geen bezetting opvraagbaar voor zetel A3.");
            Test(bn.BevatBezettingMetZetel("A1"), "Bezetting teruggevonden voor zetel A1.");
            Test(bn.BevatBezettingMetZetel("A2") == false, "Geen bezetting teruggevonden voor zetel A2.");
            Test(bn.BevatBezettingMetZetel("A3") == false, "Geen bezetting teruggevonden voor zetel A3.");
            bn.Toevoegen(b2);
            Test(bn.Aantal == 2, "2 bezettingen na toevoegen tweede bezetting (zetel A2).");
            Test(bn.AantalMaaltijden(Maaltijd.Standaard) == 2, "2 standaard maaltijden");
            Test(bn.AantalMaaltijden(Maaltijd.Vegetarisch) == 0, "0 vegetarische maaltijden");
            Test(bn[0] == b1, "Bezetting opvraagbaar op index 0.");
            Test(bn[1] == b2, "Bezetting opvraagbaar op index 1.");
            Test(bn[2] == null, "Geen bezetting opvraagbaar op index 2.");
            Test(bn["A1"] == b1, "Bezetting opvraagbaar voor zetel A1.");
            Test(bn["A2"] == b2, "Bezetting opvraagbaar voor zetel A2.");
            Test(bn["A3"] == null, "Geen bezetting opvraagbaar voor zetel A3.");
            Test(bn.BevatBezettingMetZetel("A1"), "Bezetting teruggevonden voor zetel A1.");
            Test(bn.BevatBezettingMetZetel("A2"), "Bezetting teruggevonden voor zetel A2.");
            Test(bn.BevatBezettingMetZetel("A3") == false, "Geen bezetting teruggevonden voor zetel A3.");
            bn.Toevoegen(b3);
            Test(bn.Aantal == 3, "3 bezettingen na toevoegen derde bezetting (zetel A3).");
            Test(bn.AantalMaaltijden(Maaltijd.Standaard) == 2, "2 standaard maaltijden");
            Test(bn.AantalMaaltijden(Maaltijd.Vegetarisch) == 1, "1 vegetarische maaltijd");
            Test(bn[0] == b1, "Bezetting opvraagbaar op index 0.");
            Test(bn[1] == b2, "Bezetting opvraagbaar op index 1.");
            Test(bn[2] == b3, "Bezetting opvraagbaar op index 2.");
            Test(bn["A1"] == b1, "Bezetting opvraagbaar voor zetel A1.");
            Test(bn["A2"] == b2, "Bezetting opvraagbaar voor zetel A2.");
            Test(bn["A3"] == b3, "Bezetting opvraagbaar voor zetel A3.");
            Test(bn.BevatBezettingMetZetel("A1"), "Bezetting teruggevonden voor zetel A1.");
            Test(bn.BevatBezettingMetZetel("A2"), "Bezetting teruggevonden voor zetel A2.");
            Test(bn.BevatBezettingMetZetel("A3"), "Bezetting teruggevonden voor zetel A3.");

            List<Bezetting> bl = bn.Vind("JA");
            Test(bl.Count == 2, "2 bezettingen voor passagier met tekst 'ja' in zijn naam.");
            Test(bl.Contains(b1) == true, "Bezetting voor Jan is gevonden.");
            Test(bl.Contains(b3) == true, "Bezetting voor Jacques is gevonden.");
            bl = bn.Vind("s");
            Test(bl.Count == 2, "2 bezettingen voor passagier met tekst 's' in zijn naam.");
            Test(bl.Contains(b2) == true, "Bezetting voor Pol S. is gevonden.");
            Test(bl.Contains(b3) == true, "Bezetting voor Jacques is gevonden.");

            bl = bn.Vind("JA");
            Test(bl.Count == 2, "2 bezettingen voor passagier met tekst 'JA' in zijn naam.");

            bl = bn.Vind("");
            Test(bl.Count == 3, "3 bezettingen voor passagier met tekst '' in zijn naam.");

            bl = bn.Vind("", Maaltijd.Standaard);
            Test(bl.Count == 2, "2 bezettingen voor passagiers met maaltijdtype standaard.");

            bl = bn.Vind("", Maaltijd.Vegetarisch);
            Test(bl.Count == 1, "1 bezetting voor passagiers met maaltijdtype vegitarisch.");

            bl = bn.Vind("ja", Maaltijd.Standaard);
            Test(bl.Count == 1, "1 bezetting vr passagiers met maaltijdtype standaard & tekst 'ja' in naam");

            bn.Verwijder("A1");
            Test(bn.Aantal == 2, "2 bezettingen na het verwijderen van bezetting (zetel A1).");
            Test(bn[0] == b2, "Overige bezetting nog opvraagbaar op index 0.");
            Test(bn[1] == b3, "Overige bezetting nog opvraagbaar op index 1.");
            Test(bn["A1"] == null, "Geen bezetting opvraagbaar voor zetel A1.");
            Test(bn["A2"] == b2, "Bezetting opvraagbaar voor zetel A2.");
            Test(bn["A3"] == b3, "Bezetting opvraagbaar voor zetel A3.");
            Test(bn.BevatBezettingMetZetel("A1") == false, "Geen bezetting teruggevonden voor zetel A1.");
            Test(bn.BevatBezettingMetZetel("A2"), "Bezetting teruggevonden voor zetel A2.");
            Test(bn.BevatBezettingMetZetel("A3"), "Bezetting teruggevonden voor zetel A3.");

            bn.Verwijder("A4");
            Test(bn.Aantal == 2, "Nog altijd 2 bezettingen na verwijderen ahv onbestaande zetel (zetel A4).");

            bn.Legen();
            Test(bn.Aantal == 0, "0 bezettingen na legen.");
            Test(bn.BevatBezettingMetZetel("A2") == false, "Geen bezetting teruggevonden voor zetel A2.");
            Test(bn.BevatBezettingMetZetel("A3") == false, "Geen bezetting teruggevonden voor zetel A3.");

            Console.WriteLine();
        }
        private static void TestVlucht()
        {
            Vlucht v = new Vlucht(code: "AB01");
            Test(v.Code == "AB01", "Constructor stelt code in.");

            Bezettingen bn = v.Bezettingen;
            Test(bn != null, "Nieuwe vlucht bevat bezettingen.");
            Test(bn.Aantal == 0, "Nieuwe vlucht bevat nul bezettingen.");

            Console.WriteLine();
        }
    }
}
