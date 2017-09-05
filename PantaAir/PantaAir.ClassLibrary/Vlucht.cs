namespace ClassLibrary
{
    public class Vlucht
    {
        public Vlucht(string code)
        {
            this.Code = code;
        }

        public string Code { get; }

        public Bezettingen Bezettingen { get; } = new Bezettingen();
    }
}