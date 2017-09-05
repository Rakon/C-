using System.Collections.Generic;

namespace ClassLibrary
{
    public class Bezettingen
    {
        private List<Bezetting> _items = new List<Bezetting>();

        public Bezetting this[int index]
        {
            get
            {
                if (index >= 0 && index < this._items.Count)
                    return this._items[index];
                return (Bezetting)null;
            }
        }

        public Bezetting this[string zetel]
        {
            get
            {
                foreach (Bezetting bezetting in this._items)
                {
                    if (bezetting.Zetel == zetel)
                        return bezetting;
                }
                return (Bezetting)null;
            }
        }

        public void Toevoegen(Bezetting item)
        {
            this._items.Add(item);
        }

        public void Verwijder(string zetel)
        {
            Bezetting bezetting = this[zetel];
            if (bezetting == null)
                return;
            this._items.Remove(bezetting);
        }

        public void Legen()
        {
            this._items.Clear();
        }

        public void Wissel(string zetel1, string zetel2)
        {
            Bezetting bezetting1 = this[zetel1];
            Bezetting bezetting2 = this[zetel2];
            if (bezetting1 != null)
                bezetting1.Zetel = zetel2;
            if (bezetting2 == null)
                return;
            bezetting2.Zetel = zetel1;
        }

        public bool BevatBezettingMetZetel(string zetel)
        {
            return this[zetel] != null;
        }

        public int Aantal
        {
            get
            {
                return this._items.Count;
            }
        }

        public int AantalMaaltijden(Maaltijd maaltijd)
        {
            int num = 0;
            foreach (Bezetting bezetting in this._items)
            {
                if (bezetting.Maaltijd == maaltijd)
                    ++num;
            }
            return num;
        }

        public List<Bezetting> Vind(string passagiernaam)
        {
            List<Bezetting> bezettingList = new List<Bezetting>();
            foreach (Bezetting bezetting in this._items)
            {
                if (bezetting.Passagiernaam.ToLower().Contains(passagiernaam.ToLower()))
                    bezettingList.Add(bezetting);
            }
            return bezettingList;
        }

        public List<Bezetting> Vind(string passagiernaam, Maaltijd maaltijd)
        {
            List<Bezetting> bezettingList1 = this.Vind(passagiernaam);
            List<Bezetting> bezettingList2 = new List<Bezetting>();
            foreach (Bezetting bezetting in bezettingList1)
            {
                if (bezetting.Maaltijd == maaltijd)
                    bezettingList2.Add(bezetting);
            }
            return bezettingList2;
        }
    }
}