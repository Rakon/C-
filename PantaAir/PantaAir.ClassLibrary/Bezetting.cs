namespace ClassLibrary
{
    public class Bezetting
    {
        public Bezetting(string zetel, string passagiernaam)
        {
            this.Zetel = zetel;
            this.Passagiernaam = passagiernaam;
        }

        public string Zetel { get; set; }

        public string Passagiernaam { get; set; }

        public Maaltijd Maaltijd { get; set; }
    }
}
