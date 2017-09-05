using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using ClassLibrary;
namespace WpfApp
{
    public partial class VluchtBezettingenWindow : Window
    {
        public VluchtBezettingenWindow()
        {
            InitializeComponent();

            // Opvullen _vluchten collectie met de informatie uit de vluchten folder:
            _vluchten = VluchtLader.VluchtenUitFolder("vluchten");

            // Opvullen VluchtenComboBox:
            foreach (Vlucht v in _vluchten) VluchtenComboBox.Items.Add(v.Code);

            // Selecteren eerste element in VluchtenComboBox:
            if (VluchtenComboBox.Items.Count > 0) VluchtenComboBox.SelectedIndex = 0;
        }

        private List<Vlucht> _vluchten;       // Lijst van vluchtobjecten uit vluchten folder.
        private Vlucht _geselecteerdeVlucht;  // Eén van de elementen uit bovenstaande lijst.
        //                                       We houden de verwijzing hiervan bij omdat we van deze vlucht
        //                                       de gegevens gaan visualiseren, of eventueel uit deze vlucht
        //                                       bezettingen gaan verwijderen.

        private void VluchtenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VluchtenComboBox.SelectedIndex != -1) // Indien een element geselecteerd:
            {
                // Geselecteerde vlucht instellen:
                _geselecteerdeVlucht = _vluchten[VluchtenComboBox.SelectedIndex];

                // Bezettingen weergave-lijst aanpassen:
                ToonBezettingen();
            }
        }
        private void PassagierNaamFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Bezettingen weergave-lijst aanpassen:
            ToonBezettingen();
        }
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            // Bezettingen weergave-lijst aanpassen:
            ToonBezettingen();
        }

        private void ToonBezettingen()
        {
            if (_geselecteerdeVlucht != null)
            {
                List<Bezetting> bezettingenLijst;

                // Bezettingen filteren op maaltijdtype en/of passagiersnaam:
                string passagiernaam = PassagierNaamFilterTextBox.Text;
                if (StandaardMaaltijdFilterRadioButton.IsChecked == true)
                    bezettingenLijst = _geselecteerdeVlucht.Bezettingen.Vind(passagiernaam, Maaltijd.Standaard);
                else if (VegitarischeMaaltijdFilterRadioButton.IsChecked == true)
                    bezettingenLijst = _geselecteerdeVlucht.Bezettingen.Vind(passagiernaam, Maaltijd.Vegetarisch);
                else //if (GeenMaaltijdTypeFilterRadioButton.IsChecked == true)
                    bezettingenLijst = _geselecteerdeVlucht.Bezettingen.Vind(passagiernaam);

                // Bezettingen sorteren op zetel of passagiersnaam:
                if (ZetelSorterenRadioButton.IsChecked == true)
                    BezettingenSorteren.OpZetel(bezettingenLijst);
                else if (PassagiersnaamSorterenRadioButton.IsChecked == true)
                    BezettingenSorteren.OpPassagiersnaam(bezettingenLijst);

                // Weergeven gevisualiseerde aantal bezettingen:
                AantallenLabel.Content = $"{bezettingenLijst.Count} bezettingen weergegeven: ";
                int aantStd = Aantal(bezettingenLijst, Maaltijd.Standaard);
                int aantVeg = Aantal(bezettingenLijst, Maaltijd.Vegetarisch);
                AantallenLabel.Content += $"{aantStd} standaard en {aantVeg} vegetarische maaltijden";

                // BezettingenListBox legen en opnieuw opvullen:
                BezettingenListBox.Items.Clear();
                foreach (Bezetting b in bezettingenLijst)
                {
                    string label = $"{b.Zetel} | {b.Passagiernaam.PadRight(30)} | {b.Maaltijd.ToString().PadRight(10)}";
                    BezettingenListBox.Items.Add(label);
                }

                // WeergegevenZetelsComboBox legen en opnieuw opvullen:
                WeergegevenZetelsComboBox.Items.Clear();
                foreach (Bezetting b in bezettingenLijst)
                    WeergegevenZetelsComboBox.Items.Add(b.Zetel);
            }
        }

        private static int Aantal(List<Bezetting> bezettingenLijst, Maaltijd maaltijd)
        {
            int aantal = 0;

            foreach (Bezetting b in bezettingenLijst)
                if (b.Maaltijd == maaltijd) aantal++;

            return aantal;
        }

        private void VerwijderOpZetelButton_Click(object sender, RoutedEventArgs e)
        {
            if (_geselecteerdeVlucht != null)
            {
                // Ophalen geselecteerde waarde:
                string geselecteerdeZetel = WeergegevenZetelsComboBox.SelectedValue.ToString();

                // Controle op het bevatten van een bezetting voor deze zetel:
                if (_geselecteerdeVlucht.Bezettingen.BevatBezettingMetZetel(geselecteerdeZetel))
                {
                    // Effectief laten verwijderen van deze bezetting:
                    _geselecteerdeVlucht.Bezettingen.Verwijder(geselecteerdeZetel);

                    // Bezettingen weergave-lijst aanpassen:
                    ToonBezettingen();

                    // Status van aanpassing visualiseren:
                    AanpassenVluchtLabel.Content = $"Bezetting voor zetel {geselecteerdeZetel} werd verwijderd.";
                }
                else
                {
                    // Status van aanpassing visualiseren:
                    AanpassenVluchtLabel.Content = $"Geen bezetting teruggevonden voor zetel {geselecteerdeZetel}.";
                }
            }
        }

        private void LeegVluchtButton_Click(object sender, RoutedEventArgs e)
        {
            if (_geselecteerdeVlucht != null)
            {
                // Verwijderen van elke bezetting voor een bepaalde vlucht:
                _geselecteerdeVlucht.Bezettingen.Legen();

                // Bezettingen weergave-lijst aanpassen:
                ToonBezettingen();

                // Status van aanpassing visualiseren:
                AanpassenVluchtLabel.Content = $"Alle bezettingen voor deze vlucht werden verwijderd.";
            }
        }
    }

    class BezettingenSorteren
    {
        // Sorteren (op passagiersnaam):
        public static void OpPassagiersnaam(List<Bezetting> bezettingenLijst)
        {
            for (int indexEersteTeller = 0; indexEersteTeller < bezettingenLijst.Count - 1; indexEersteTeller++)
            {
                int indexEerste = indexEersteTeller;
                for (int index = indexEersteTeller; index <= bezettingenLijst.Count - 1; index++)
                {
                    if (bezettingenLijst[index].Passagiernaam.CompareTo(bezettingenLijst[indexEerste].Passagiernaam) < 0) indexEerste = index;
                }

                Bezetting backup = bezettingenLijst[indexEersteTeller];
                bezettingenLijst[indexEersteTeller] = bezettingenLijst[indexEerste];
                bezettingenLijst[indexEerste] = backup;
            }
        }
        // Sorteren (op zetel):
        public static void OpZetel(List<Bezetting> bezettingenLijst)
        {
            for (int indexEersteTeller = 0; indexEersteTeller < bezettingenLijst.Count - 1; indexEersteTeller++)
            {
                int indexEerste = indexEersteTeller;
                for (int index = indexEersteTeller; index <= bezettingenLijst.Count - 1; index++)
                {
                    if (bezettingenLijst[index].Zetel.CompareTo(bezettingenLijst[indexEerste].Zetel) < 0) indexEerste = index;
                }

                Bezetting backup = bezettingenLijst[indexEersteTeller];
                bezettingenLijst[indexEersteTeller] = bezettingenLijst[indexEerste];
                bezettingenLijst[indexEerste] = backup;
            }
        }
    }

    class VluchtLader
    {
        // Statische utility methods voor inladen uit directory/bestanden:
        public static List<Vlucht> VluchtenUitFolder(string folder)
        {
            // Lijst van Vlucht objecten creëren:
            List<Vlucht> vluchten = new List<Vlucht>();
            // Alle bestanden uit directory opvragen:
            string[] vluchtBestanden = Directory.GetFiles(folder);
            // Voor elke bestand Vlucht object laten creëren:
            foreach (string vluchtBestand in vluchtBestanden)
            {
                // Vlucht object laten creëren:
                Vlucht v = VluchtUitBestand(vluchtBestand);
                // Vlucht aan lijst toevoegen:
                vluchten.Add(v);
            }
            // Lijst opleveren:
            return vluchten;
        }
        public static Vlucht VluchtUitBestand(string vluchtBestand)
        {
            // Bestandsnaam opsporen:
            FileInfo vluchtBestandInfo = new FileInfo(vluchtBestand);
            string bestandsNaam = vluchtBestandInfo.Name;
            // Vluchtobject creëren, bestandsnaam als vluchtcode instellen:
            Vlucht v = new Vlucht(code: bestandsNaam);
            // Regels van bestand inlezen in string array:
            string[] bezettingLijnen = File.ReadAllLines(vluchtBestand);
            // Elke regel ontleden en in bezettingsobject omzetten:
            foreach (string bezettingLijn in bezettingLijnen)
            {
                // Regel opslitsen:
                string[] bezettingInfo = bezettingLijn.Split(new char[] { ';' });
                // Properties ontleden:
                string zetel = bezettingInfo[0];
                string naam = bezettingInfo[1];
                Maaltijd maaltijd = (Maaltijd)Enum.Parse(typeof(Maaltijd), bezettingInfo[2]);
                // Bezettingsobject creëren:
                Bezetting b = new Bezetting(zetel, naam) { Maaltijd = maaltijd };
                // Bezetting toevoegen aan Bezettingen van Vlucht:
                v.Bezettingen.Toevoegen(b);
            }
            // Vlucht opleveren:
            return v;
        }
    }
}
