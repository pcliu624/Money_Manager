namespace _3206.ViewModels
{
    public class DashboardViewModel
    {
        public List<ExpensesViewModel> Expenses { get; set; }
        public List<SpentBudgetViewModel> Spent { get; set; }
        public List<ForeachPayViewModel> EachPersonShouldPay { get; set; }

        public string TypeChart { get; set; }
        public string StoreChart { get; set; }
        public string PieChart { get; set; }
        public string VisitCount { get; set; }
        public class ExpensesViewModel
        {
            public string Type { get; set; }
            public string Cost { get; set; }
            public string percentage { get; set; }
        }
        public class SpentBudgetViewModel
        {
            public string Type { get; set; }
            public string Percentage { get; set; }
            public string Spent { get; set; }
            public string Budget { get; set; }
            public bool IsOverBudget { get; set; }
        }
    }
}
