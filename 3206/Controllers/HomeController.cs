using _3206.Entities;
using _3206.Tools.Home;
using _3206.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model.Structures;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Text.Json;
using static _3206.ViewModels.DashboardViewModel;

namespace _3206.Controllers
{
    public class HomeController : Controller       
    {
        public readonly DBContext _db;
        public readonly Chart _chart;
        public HomeController(Chart chart,DBContext dbcontext)
        {
            _chart = chart;
            _db = dbcontext;
        }
        public IActionResult Index()
        {
            //var chart = new DashboardViewModel();
            //try
            //{
            //    chart = await _chart.Dashboard(range);

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            return View();
        }
        public async Task<ActionResult> ChartPartial(string range)
        {
            var chart =await _chart.Dashboard(range);
            return PartialView("_chart",chart);
        }
    }
}
