using System.ComponentModel;

namespace _3206.ViewModels
{
    public class PurchaseViewModel
    {
        [DisplayName("Person")]
        public IEnumerable<string> PersonList { get; set; }
        public IEnumerable<string> StoreList { get; set; }
        public IEnumerable<string> TypeList { get; set; }
        public IEnumerable<string> PersonList2 { get; set; }

        public string Date { get; set; }
        public string Person { get; set; }
        public string Store { get; set; }
        public string Type { get; set; }
        public decimal Cost { get; set; }
        public List<string> Payfor { get; set; }
    }
    public class DropdownModel
    {
        public string Text { get; set; }
        public long Value { get; set; }
    }
}
