using _3206.Entities;
using _3206.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace _3206.Controllers
{
    public class SETController : Controller
    {
        public readonly DBContext _db;
        public SETController(DBContext dbContext)
        {
            _db = dbContext;
        }
        public async Task<ActionResult> Parameter()
        {
            var vmodel = new ParaViewModel();
            vmodel.paralist = await _db.Parameters.OrderBy(x => x.Category).ToListAsync();
            return View(vmodel);
        }
        public async Task<ActionResult> EditParameter(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = await _db.Parameters.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (obj == null)
            {
                return NotFound();
            }
            return Json(obj);
        }
        [HttpPost]
        public async Task<ActionResult> EditParameter(ParaViewModel obj)
        {
            try
            {
                if (obj.parameter.Id != 0)
                {
                    _db.Parameters.Update(obj.parameter);
                }
                else
                {
                    _db.Parameters.Add(obj.parameter);
                }
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("Parameter");
        }
        [HttpPost]
        public async Task<ActionResult> DeleteParameter(int id)
        {
            var para = await _db.Parameters.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (para == null)
            {
                return NotFound();
            }
            _db.Parameters.Remove(para);
            await _db.SaveChangesAsync();
            return View();
        }
        public async Task<ActionResult> Budget()
        {
            var vmodel = new BudgetViewModel();
            vmodel.budgetlist = await _db.Budgets.OrderByDescending(x=>x.CreateDate).ToListAsync();
            return View(vmodel);
        }
        public async Task<ActionResult> EditBudget(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = await _db.Budgets.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (obj == null)
            {
                return NotFound();
            }
            return Json(obj);
        }
        [HttpPost]
        public async Task<ActionResult> EditBudget(BudgetViewModel obj)
        {
            try
            {
                if (obj.budget.Id != 0)
                {
                    var data =await _db.Budgets.Where(x => x.Id == obj.budget.Id).FirstOrDefaultAsync();
                    if (data == null) return NotFound();
                    data.Budget1 = obj.budget.Budget1;
                    data.ModifyDate = DateTime.Now;
                    data.Type = obj.budget.Type.Trim();
                    _db.Budgets.Update(data);
                }
                else
                {
                    var tmp = new Budget();

                    tmp.Budget1 = obj.budget.Budget1;
                    tmp.CreateDate = DateTime.Now;
                    tmp.Type = obj.budget.Type.Trim();
                    _db.Budgets.Add(tmp);
                }
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("Budget");
        }
        public async Task<ActionResult> DeleteBudget(int id)
        {
            var bud = await _db.Budgets.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (bud == null)
            {
                return NotFound();
            }
            _db.Budgets.Remove(bud);
            await _db.SaveChangesAsync();
            return View();
        }
    }
}
