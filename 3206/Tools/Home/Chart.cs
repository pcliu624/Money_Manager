using _3206.Entities;
using _3206.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using static _3206.ViewModels.BarChartViewModel;
using static _3206.ViewModels.DashboardViewModel;

namespace _3206.Tools.Home
{
    public class Chart
    {
        public readonly DBContext _db;
        public Chart(DBContext dbContext)
        {
            _db = dbContext;
        }
        public async Task<DashboardViewModel> Dashboard(string range)
        {
            var model = new DashboardViewModel();
            DateTime now = DateTime.Now;
            DateTime start = new DateTime(now.Year, now.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            DateTime end = new DateTime(now.Year, now.Month, daysInMonth);
            if (!string.IsNullOrWhiteSpace(range))
            {
                start = DateTime.Parse(range.Split('|')[0]);
                end = DateTime.Parse(range.Split('|')[1]);
            }
            model.Expenses = await GetExpensesData(start, end);
            var barchart = await GetBarChartData(start, end);
            model.TypeChart = JsonConvert.SerializeObject(barchart.Item1);
            model.StoreChart = JsonConvert.SerializeObject(barchart.Item2);
            model.PieChart = JsonConvert.SerializeObject(await GetPieChartData(start, end));
            model.VisitCount = JsonConvert.SerializeObject(await GetVisitCountData(start, end));
            model.Spent = await GetBudgetData(start, end);
            model.EachPersonShouldPay = await GetEachPersonShouldPay(start, end);
            return model;
        }
        public async Task<Tuple<BarChartViewModel, BarChartViewModel>> GetBarChartData(DateTime start, DateTime end)
        {
            var barChartViewModel1 = new BarChartViewModel();
            var barChartViewModel2 = new BarChartViewModel();
            var data = await _db.Purchases.Where(x => x.Date.Value >= start && x.Date.Value <= end).ToListAsync();
            var datalist = new List<decimal>();
            var Xaxislist1 = new List<string>();
            var slist = new List<SeriesModel>();
            foreach (var item in data.GroupBy(x => x.Type))
            {
                Xaxislist1.Add(item.Key ?? "");
                datalist.Add(item.Select(x => x.Cost).Sum());
            }
            var serie = new SeriesModel()
            {
                data = datalist,
                name = "CAD"
            };
            var xaxis = new XaxisModel() { categories = Xaxislist1 };
            slist.Add(serie);
            var tmpc = new Chartmodel() { type = "bar" };
            barChartViewModel1.series = slist;
            barChartViewModel1.xaxis = xaxis;
            barChartViewModel1.chart = tmpc;
            barChartViewModel1.plotOptions = new OptionModel() { bar = new BarModel() { borderRadius = 4, horizontal = false } };

            var datalist2 = new List<decimal>();
            var Xaxislist2 = new List<string>();
            var slist2 = new List<SeriesModel>();
            foreach (var item in data.GroupBy(x => x.Store))
            {
                Xaxislist2.Add(item.Key ?? "");
                datalist2.Add(item.Select(x => x.Cost).Sum());
            }
            var xaxis2 = new XaxisModel() { categories = Xaxislist2 };
            var serie2 = new SeriesModel()
            {
                data = datalist2,
                name = "CAD"
            };
            slist2.Add(serie2);
            barChartViewModel2.series = slist2;
            barChartViewModel2.xaxis = xaxis2;
            barChartViewModel2.chart = tmpc;
            barChartViewModel2.plotOptions = new OptionModel() { bar = new BarModel() { borderRadius = 4, horizontal = false } };

            return Tuple.Create<BarChartViewModel, BarChartViewModel>(barChartViewModel1, barChartViewModel2);
        }
        public async Task<BarChartViewModel> GetVisitCountData(DateTime start, DateTime end)
        {
            var chart = new BarChartViewModel();
            var data = await _db.Purchases.Where(x => x.Date.Value >= start && x.Date.Value <= end).ToListAsync();
            var datalist = new List<decimal>();
            var Xaxislist = new List<string>();
            var slist = new List<SeriesModel>();
            foreach (var item in data.GroupBy(x => x.Store))
            {
                Xaxislist.Add(item.Key ?? "");
                datalist.Add(item.Count());
            }
            var serie = new SeriesModel()
            {
                data = datalist,
                name = "Times"
            };
            var xaxis = new XaxisModel() { categories = Xaxislist };
            slist.Add(serie);
            chart.series = slist;
            chart.xaxis = xaxis;
            chart.chart = new Chartmodel() { type = "bar" };
            chart.plotOptions = new OptionModel() { bar = new BarModel() { borderRadius = 4, horizontal = true } };
            return chart;
        }
        public async Task<PieChartViewModel> GetPieChartData(DateTime start, DateTime end)
        {
            var chart = new PieChartViewModel();
            var data = await _db.Purchases.Where(x => x.Date.Value >= start && x.Date.Value <= end).ToListAsync();
            var labellist = new List<string>();
            var series = new List<decimal>();
            foreach (var item in data.GroupBy(x => x.Type))
            {
                labellist.Add(item.Key ?? "");
                series.Add(item.Select(x => x.Cost).Sum());
            }
            chart.chart = new Chartmodel() { type = "donut" };
            chart.series = series;
            chart.labels = labellist;

            return chart;
        }
        public async Task<List<ExpensesViewModel>> GetExpensesData(DateTime start, DateTime end)
        {
            var expense = new List<ExpensesViewModel>();

            try
            {
                var data = await _db.Purchases.Where(x => x.Date.Value >= start && x.Date.Value <= end).ToListAsync();
                decimal Total = data.Select(x => x.Cost).Sum();
                foreach (var item in data.GroupBy(x => x.Type))
                {
                    var tmp = new ExpensesViewModel();
                    tmp.Type = item.Key;
                    var cost = item.Select(x => x.Cost).Sum();
                    tmp.percentage = (cost / Total * 100).ToString();
                    tmp.Cost = cost.ToString();
                    expense.Add(tmp);
                }
                expense.Add(new ExpensesViewModel()
                {
                    Type = "Total",
                    percentage = "100",
                    Cost = Total.ToString()
                });
            }
            catch (Exception ex) { }
            return expense;
        }

        public async Task<List<SpentBudgetViewModel>> GetBudgetData(DateTime start, DateTime end)
        {
            var list = new List<SpentBudgetViewModel>();
            var budget = await _db.Budgets.ToListAsync();
            var spent = await _db.Purchases.Where(x => x.Date.Value >= start && x.Date.Value <= end && budget.Select(y => y.Type)
            .Contains(x.Type)).ToListAsync();
            foreach (var item in budget)
            {
                var s = spent.Where(x => x.Type == item.Type.Trim()).Select(x => x.Cost).Sum();
                var b = item.Budget1;
                var tmp = new SpentBudgetViewModel
                {
                    Type = item.Type,
                    Spent = s.ToString() ?? "0",
                    Percentage = (s / b * 100).ToString(),
                    Budget = b.ToString(),
                    IsOverBudget = s > b ? true : false,
                };
                list.Add(tmp);
            }
            return list;
        }
        public async Task<List<ForeachPayViewModel>> GetEachPersonShouldPay(DateTime start, DateTime end)
        {
            var resultList =new List<ForeachPayViewModel>();
            var record = await _db.Purchases.Where(x => x.Date.Value >= start && x.Date.Value <= end).ToListAsync();
            Dictionary<string, decimal> totalAmount = new Dictionary<string, decimal>();
            foreach (var group in record.GroupBy(x => x.Payfor))
            {
                var plist = group.Key.Split(',');
                var personCount = plist.Length;
                foreach (var person in plist)
                {
                    if (totalAmount.ContainsKey(person))
                    {
                        totalAmount[person] += (group.Select(x => x.Cost).Sum() / personCount) - group.Where(x =>x.Payby == person).Select(x=>x.Cost).Sum();
                    }
                    else
                        totalAmount[person] = (group.Select(x => x.Cost).Sum() / personCount) - group.Where(x => x.Payby == person).Select(x => x.Cost).Sum();
                }
            }
            foreach (var amount in totalAmount)
            {
                resultList.Add(new ForeachPayViewModel { Person = amount.Key, Amount = Math.Round(amount.Value,2,MidpointRounding.AwayFromZero).ToString() });
            }
            return resultList;
        }
    }
}
