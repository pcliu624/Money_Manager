﻿using _3206.Entities;
using _3206.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static _3206.Tools.Tools;

namespace _3206.Controllers
{
    public class PurchaseController : Controller
    {
        public readonly DBContext _db;
        public PurchaseController(DBContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<ActionResult> Index()
        {
            var vmodel = new PurchaseViewModel();
            string[] categoryarr = ["Person", "Person", "Store", "Type"];
            string[] listarr = ["PersonList", "PersonList2", "StoreList", "TypeList"];
            var db = _db.Parameters;
            foreach (var (category, listName) in categoryarr.Zip(listarr, (c, l) => (c, l)))
            {
                var list = await db.Where(x => x.Category == category).Select(x => x.Name).ToListAsync();                
                var propertyInfo = vmodel.GetType().GetProperty(listName);

                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(vmodel, list);
                }
            }
            return View(vmodel);
        }
        [HttpPost]
        public async Task<ActionResult> PurchaseRecord(PurchaseFilterViewModel filter)
        {
            var result = new JsonResponse<PurchaseRecordViewModel>();
            try
            {
                var list =  _db.Purchases.AsQueryable();
                if (filter.Date != null)
                {
                    var start = DateTime.Parse(filter.Date.Split('-')[0]);
                    var end = DateTime.Parse(filter.Date.Split('-')[1]);
                    list = list.Where(x => x.Date >= start && x.Date <= end);
                }
                if (!string.IsNullOrEmpty(filter.Paidby))
                {
                    list = list.Where(x =>x.Payby==filter.Paidby);
                }
                if(!string.IsNullOrEmpty(filter.Type))
                {
                    list = list.Where(x=>x.Type==filter.Type);
                }
                if(!string.IsNullOrEmpty(filter.Store))
                {
                    list = list.Where(x =>x.Store==filter.Store);
                }
                //if (filter.Payfor != null && filter.Payfor.Count>0 )
                //{
                //    list = list.Where(x =>filter.Payfor.Contains(x.Payfor));
                //}
                result.rows =await list.OrderByDescending(x =>x.Date).Skip(filter.offset.GetValueOrDefault(0)).Take(filter.limit.GetValueOrDefault(25)).Select(x => new PurchaseRecordViewModel
                {
                    Date = x.Date.HasValue ? x.Date.Value.ToString("yyyy-MM-dd") : "",
                    Payby = x.Payby,
                    Cost = x.Cost.ToString(),
                    Type = x.Type ?? "",
                    Store = x.Store ?? "",
                    Payfor = x.Payfor ?? "",
                    Id = x.Id.ToString(),
                }).ToListAsync();
                result.total = list.Count();
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            return Json(result);
        }
        [HttpPost]
        public JsonResult SavePurchaseRecord([FromBody] PurchaseModifyViewModel model)
        {
            foreach (var item in model.EditList)
            {
                var tmp = _db.Purchases.Where(x => x.Id == long.Parse(item.Id)).FirstOrDefault();
                if (tmp != null)                    
                {
                    tmp.Store = item.Store;
                    tmp.Payfor = item.Payfor.ToUpper();
                    tmp.ModifyDate = DateTime.Now;
                    tmp.Cost = decimal.Parse(item.Cost);
                    tmp.Date = DateTime.Parse(item.Date);
                    tmp.Type = item.Type;
                    tmp.Payby = item.Payby.ToUpper();  
                    _db.Purchases.Update(tmp);
                }
            }
            if (model.DeleteList.Count > 0)
            {
                var convert = model.DeleteList.Select(x => long.Parse(x)).ToList();
                var del = _db.Purchases.Where(x => convert.Contains(x.Id)).ToList();
                _db.Purchases.RemoveRange(del);
            }
            _db.SaveChanges();
            return new JsonResult(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddPurchaseRecord(PurchaseViewModel model)
        {
            var tmp = new Purchase();
            tmp.Date = DateTime.Parse(model.Date);
            tmp.Payby = model.Person;
            tmp.Cost = model.Cost;
            tmp.Store = model.Store;
            tmp.Type = model.Type;
            tmp.Payfor = string.Join(",", model.Payfor);
            tmp.CreateDate = DateTime.Now;
            _db.Purchases.Add(tmp);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
