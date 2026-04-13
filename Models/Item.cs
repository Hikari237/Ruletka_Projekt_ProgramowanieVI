namespace Ruletka.Models
{
    public class Item
    {
        public string Name { get; set; }
        public int HealAmount { get; set; }
        public int SympathyChange { get; set; }

       
        public bool IsConsumable { get; set; }

        public Item(string name, int healAmount, int sympathyChange, bool isConsumable = true)
        {
            Name = name;
            HealAmount = healAmount;
            SympathyChange = sympathyChange;
            IsConsumable = isConsumable;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}