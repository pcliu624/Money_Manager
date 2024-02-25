namespace _3206.ViewModels
{
    public class BarChartViewModel
    {
        public Chartmodel chart { get; set; }
        public List<SeriesModel> series { get; set; }
        public XaxisModel xaxis { get; set; }
        public OptionModel plotOptions { get; set; }
        public class SeriesModel
        {
            public string name { get; set; }
            public List<decimal> data { get; set; }
        }
        public class XaxisModel
        {
            public List<string> categories { get; set; }
        }       
        public class Chartmodel
        {
            public string? type { get; set; }
        }
        public class OptionModel
        {
            public BarModel bar { get; set; }
        }
        public class BarModel
        {
            public int borderRadius { get; set; }
            public bool horizontal { get; set; }
        }
    }
}
