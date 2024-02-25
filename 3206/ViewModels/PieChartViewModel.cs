using static _3206.ViewModels.BarChartViewModel;

namespace _3206.ViewModels
{
    public class PieChartViewModel
    {
        public Chartmodel chart { get; set; }
        public List<decimal> series { get; set; }
        public List<string> labels { get; set; }
       

    }
}
