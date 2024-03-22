using _3206.Entities;
using _3206.Tools.Home;
using _3206.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static _3206.ViewModels.BarChartViewModel;

namespace _3206.Controllers
{
    public class PersonalController : Controller
    {
        public readonly DBContext _db;
        public PersonalController(DBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var model = new PersonalViewModel();
            model.PeopleList = _db.Parameters.Where(x => x.Category == "Person").Select(x => x.Name).ToList();
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Index([FromForm] PersonalViewModel model)
        {
            var barChartViewModel = new BarChartViewModel();
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(now.Year, now.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            DateTime end = new DateTime(now.Year, now.Month, daysInMonth);
            if (!string.IsNullOrWhiteSpace(model.DateRange))
            {
                start = DateTime.Parse(model.DateRange.Split('-')[0]);
                end = DateTime.Parse(model.DateRange.Split('-')[1]);
            }
            var data = await _db.Purchases.Where(x => x.Date.Value >= start && x.Date.Value <= end && x.Payfor.Contains(model.PersonName)).ToListAsync();
            var datalist = new List<decimal>();
            var Xaxislist1 = new List<string>();
            var slist = new List<SeriesModel>();
            var labellist = new List<string>();
            var series = new List<decimal>();
            Dictionary<string, decimal> totalAmount = new Dictionary<string, decimal>();
            foreach (var item in data)
            {

                var plist = item.Payfor.Split(',');
                var personCount = plist.Length;
                if (totalAmount.ContainsKey(item.Type))
                {
                    totalAmount[item.Type] += (item.Cost) / personCount;
                }
                else
                {
                    totalAmount[item.Type] = (item.Cost) / personCount;
                }
                //datalist.Add(item.Select(x => x.Cost).Sum());
            }
            foreach (var amount in totalAmount)
            {
                Xaxislist1.Add(amount.Key ?? "");
                datalist.Add(Math.Round(amount.Value, 2, MidpointRounding.AwayFromZero));
                labellist.Add(amount.Key ?? "");
                series.Add(Math.Round(amount.Value, 2, MidpointRounding.AwayFromZero));
            }
            var serie = new SeriesModel()
            {
                data = datalist,
                name = "CAD"
            };
            var xaxis = new XaxisModel() { categories = Xaxislist1 };
            slist.Add(serie);
            var tmpc = new Chartmodel() { type = "bar" };
            barChartViewModel.series = slist;
            barChartViewModel.xaxis = xaxis;
            barChartViewModel.chart = tmpc;
            barChartViewModel.plotOptions = new OptionModel() { bar = new BarModel() { borderRadius = 4, horizontal = false } };

            var chart = new PieChartViewModel();
                      
            chart.chart = new Chartmodel() { type = "donut" };
            chart.series = series;
            chart.labels = labellist;
            var reuslt = new PersonalViewModel
            {
                TypeChart = JsonConvert.SerializeObject(barChartViewModel),
                PeopleList = _db.Parameters.Where(x => x.Category == "Person").Select(x => x.Name).ToList(),
                PieChart = JsonConvert.SerializeObject(chart)
            };
            return View(reuslt);
        }

    }
}
