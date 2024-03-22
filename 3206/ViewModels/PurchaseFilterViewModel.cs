namespace _3206.ViewModels
{
    public class PurchaseFilterViewModel
    {
        public string Date { get; set; }
        public string Paidby { get; set; }
        public string Store { get; set; }
        public string Type { get; set; }
        public List<string>? Payfor { get; set; }
        public int? limit { get; set; }
        public int? offset { get; set; }
    }
}
