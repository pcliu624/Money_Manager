using _3206.Entities;

namespace _3206.ViewModels
{
    public class BudgetViewModel
    {
        public IEnumerable<Budget> budgetlist { get; set; }
        public Budget budget { get; set; }
    }
}
