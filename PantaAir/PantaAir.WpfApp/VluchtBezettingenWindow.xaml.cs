
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

/*
using ClassLibrary;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace WpfApp
{
    public class VluchtBezettingenWindow
    {
        private List<Vlucht> _vluchten;
        private Vlucht _geselecteerdeVlucht;
        internal ComboBox VluchtenComboBox;
        internal Label AantallenLabel;
        internal ListBox BezettingenListBox;
        internal TextBox PassagierNaamFilterTextBox;
        internal RadioButton GeenMaaltijdTypeFilterRadioButton;
        internal RadioButton StandaardMaaltijdFilterRadioButton;
        internal RadioButton VegitarischeMaaltijdFilterRadioButton;
        internal RadioButton ZetelSorterenRadioButton;
        internal RadioButton PassagiersnaamSorterenRadioButton;
        internal ComboBox WeergegevenZetelsComboBox;
        internal Button VerwijderOpZetelButton;
        internal Button LeegVluchtButton;
        internal Label AanpassenVluchtLabel;
        private bool _contentLoaded;

        public VluchtBezettingenWindow()
        {
            this.InitializeComponent();
            this._vluchten = VluchtLader.VluchtenUitFolder("vluchten");
            foreach (Vlucht vlucht in this._vluchten)
                this.VluchtenComboBox.Items.Add((object)vlucht.Code);
            if (this.VluchtenComboBox.Items.Count <= 0)
                return;
            this.VluchtenComboBox.SelectedIndex = 0;
        }

        private void VluchtenComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.VluchtenComboBox.SelectedIndex == -1)
                return;
            this._geselecteerdeVlucht = this._vluchten[this.VluchtenComboBox.SelectedIndex];
            this.ToonBezettingen();
        }

        private void PassagierNaamFilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.ToonBezettingen();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.ToonBezettingen();
        }

        private void ToonBezettingen()
        {
            if (this._geselecteerdeVlucht == null)
                return;
            string text = this.PassagierNaamFilterTextBox.Text;
            bool? isChecked = this.StandaardMaaltijdFilterRadioButton.IsChecked;
            bool flag1 = true;
            List<Bezetting> bezettingenLijst;
            if ((isChecked.GetValueOrDefault() == flag1 ? (isChecked.HasValue ? 1 : 0) : 0) != 0)
            {
                bezettingenLijst = this._geselecteerdeVlucht.Bezettingen.Vind(text, Maaltijd.Standaard);
            }
            else
            {
                isChecked = this.VegitarischeMaaltijdFilterRadioButton.IsChecked;
                bool flag2 = true;
                bezettingenLijst = (isChecked.GetValueOrDefault() == flag2 ? (isChecked.HasValue ? 1 : 0) : 0) == 0 ? this._geselecteerdeVlucht.Bezettingen.Vind(text) : this._geselecteerdeVlucht.Bezettingen.Vind(text, Maaltijd.Vegetarisch);
            }
            isChecked = this.ZetelSorterenRadioButton.IsChecked;
            bool flag3 = true;
            if ((isChecked.GetValueOrDefault() == flag3 ? (isChecked.HasValue ? 1 : 0) : 0) != 0)
            {
                BezettingenSorteren.OpZetel(bezettingenLijst);
            }
            else
            {
                isChecked = this.PassagiersnaamSorterenRadioButton.IsChecked;
                bool flag2 = true;
                if ((isChecked.GetValueOrDefault() == flag2 ? (isChecked.HasValue ? 1 : 0) : 0) != 0)
                    BezettingenSorteren.OpPassagiersnaam(bezettingenLijst);
            }
            this.AantallenLabel.Content = (object)string.Format("{0} bezettingen weergegeven: ", (object)bezettingenLijst.Count);
            int num1 = VluchtBezettingenWindow.Aantal(bezettingenLijst, Maaltijd.Standaard);
            int num2 = VluchtBezettingenWindow.Aantal(bezettingenLijst, Maaltijd.Vegetarisch);
            Label aantallenLabel = this.AantallenLabel;
            string str = aantallenLabel.Content.ToString() + string.Format("{0} standaard en {1} vegetarische maaltijden", (object)num1, (object)num2);
            aantallenLabel.Content = (object)str;
            this.BezettingenListBox.Items.Clear();
            foreach (Bezetting bezetting in bezettingenLijst)
                this.BezettingenListBox.Items.Add((object)string.Format("{0} | {1} | {2}", (object)bezetting.Zetel, (object)bezetting.Passagiernaam.PadRight(30), (object)bezetting.Maaltijd.ToString().PadRight(10)));
            this.WeergegevenZetelsComboBox.Items.Clear();
            foreach (Bezetting bezetting in bezettingenLijst)
                this.WeergegevenZetelsComboBox.Items.Add((object)bezetting.Zetel);
        }

        private static int Aantal(List<Bezetting> bezettingenLijst, Maaltijd maaltijd)
        {
            int num = 0;
            foreach (Bezetting bezetting in bezettingenLijst)
            {
                if (bezetting.Maaltijd == maaltijd)
                    ++num;
            }
            return num;
        }

        private void VerwijderOpZetelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._geselecteerdeVlucht == null)
                return;
            string zetel = this.WeergegevenZetelsComboBox.SelectedValue.ToString();
            if (this._geselecteerdeVlucht.Bezettingen.BevatBezettingMetZetel(zetel))
            {
                this._geselecteerdeVlucht.Bezettingen.Verwijder(zetel);
                this.ToonBezettingen();
                this.AanpassenVluchtLabel.Content = (object)string.Format("Bezetting voor zetel {0} werd verwijderd.", (object)zetel);
            }
            else
                this.AanpassenVluchtLabel.Content = (object)string.Format("Geen bezetting teruggevonden voor zetel {0}.", (object)zetel);
        }

        private void LeegVluchtButton_Click(object sender, RoutedEventArgs e)
        {
            if (this._geselecteerdeVlucht == null)
                return;
            this._geselecteerdeVlucht.Bezettingen.Legen();
            this.ToonBezettingen();
            this.AanpassenVluchtLabel.Content = (object)"Alle bezettingen voor deze vlucht werden verwijderd.";
        }

        
        public void InitializeComponent()
        {
            if (this._contentLoaded)
                return;
            this._contentLoaded = true;
            Application.LoadComponent((object)this, new Uri("/PantaAir.WpfApp;component/vluchtbezettingenwindow.xaml", UriKind.Relative));
        }
        
        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    this.VluchtenComboBox = (ComboBox)target;
                    this.VluchtenComboBox.SelectionChanged += new SelectionChangedEventHandler(this.VluchtenComboBox_SelectionChanged);
                    break;
                case 2:
                    this.AantallenLabel = (Label)target;
                    break;
                case 3:
                    this.BezettingenListBox = (ListBox)target;
                    break;
                case 4:
                    this.PassagierNaamFilterTextBox = (TextBox)target;
                    this.PassagierNaamFilterTextBox.TextChanged += new TextChangedEventHandler(this.PassagierNaamFilterTextBox_TextChanged);
                    break;
                case 5:
                    this.GeenMaaltijdTypeFilterRadioButton = (RadioButton)target;
                    this.GeenMaaltijdTypeFilterRadioButton.Checked += new RoutedEventHandler(this.RadioButton_Checked);
                    break;
                case 6:
                    this.StandaardMaaltijdFilterRadioButton = (RadioButton)target;
                    this.StandaardMaaltijdFilterRadioButton.Checked += new RoutedEventHandler(this.RadioButton_Checked);
                    break;
                case 7:
                    this.VegitarischeMaaltijdFilterRadioButton = (RadioButton)target;
                    this.VegitarischeMaaltijdFilterRadioButton.Checked += new RoutedEventHandler(this.RadioButton_Checked);
                    break;
                case 8:
                    this.ZetelSorterenRadioButton = (RadioButton)target;
                    this.ZetelSorterenRadioButton.Checked += new RoutedEventHandler(this.RadioButton_Checked);
                    break;
                case 9:
                    this.PassagiersnaamSorterenRadioButton = (RadioButton)target;
                    this.PassagiersnaamSorterenRadioButton.Checked += new RoutedEventHandler(this.RadioButton_Checked);
                    break;
                case 10:
                    this.WeergegevenZetelsComboBox = (ComboBox)target;
                    break;
                case 11:
                    this.VerwijderOpZetelButton = (Button)target;
                    this.VerwijderOpZetelButton.Click += new RoutedEventHandler(this.VerwijderOpZetelButton_Click);
                    break;
                case 12:
                    this.LeegVluchtButton = (Button)target;
                    this.LeegVluchtButton.Click += new RoutedEventHandler(this.LeegVluchtButton_Click);
                    break;
                case 13:
                    this.AanpassenVluchtLabel = (Label)target;
                    break;
                default:
                    this._contentLoaded = true;
                    break;
            }
        }
    }
}
*/