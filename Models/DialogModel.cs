namespace Ruletka.Models
{
    public class DialogModel
    {
        public string Question { get; set; }
        public string Option1 { get; set; }
        public int Sympathy1 { get; set; } // Ile punktów dodaje/odejmuje opcja 1
        public string Option2 { get; set; }
        public int Sympathy2 { get; set; } // Ile punktów dodaje/odejmuje opcja 2
    }
}