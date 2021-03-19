using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExampleB.Models;

namespace ExampleB.Controllers
{
    public class DietsController : Controller
    {
        private GoodFit db = new GoodFit();

        // !!! Много ко многим
        public ActionResult GetDishes()
        {
            int[] arr;
           var ChoosenArr = Request.Cookies["Choose"]?.Value?.Split(',');
            if (ChoosenArr == null )
            {
                return PartialView("~/Views/Partial_View/Dishes.cshtml", db.Dish.ToList());
            }
            try
            {
                 arr = Array.ConvertAll(ChoosenArr, s => int.Parse(s));
            }
            catch (Exception)
            {
                return PartialView("~/Views/Partial_View/Dishes.cshtml", db.Dish.ToList());
            }
            
            var Basket = (from i in db.Dish.ToList() where Array.Exists(arr, j => j == i.id) select i).ToList();
            var NotInBasket = (from i in db.Dish.ToList() where !Array.Exists(arr, j => j == i.id) select i).ToList();
            ViewBag.Basket = Basket;

             return PartialView("~/Views/Partial_View/Dishes.cshtml", NotInBasket); ;
        }  
         
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Subscription")] Diet diet)
        {
            
            var ChoosenArr = Request.Cookies["Choose"]?.Value?.Split(',');
            int[] arr = Array.ConvertAll(ChoosenArr, s => int.Parse(s));
            if (ModelState.IsValid && arr != null)
            {
               // diet.AddDishes(arr);
               try { 
               db.Diet.Add(diet);

                db.SaveChanges();
                  int Id_Add = (from i in db.Diet orderby i.Id descending select i).ToList()[0].Id;
                  db.Diet.FirstOrDefault(i => i.Id == Id_Add).AddDishes(arr);
                
                 db.SaveChanges();
                }
                catch(Exception ex)
                {
                    string e = ex.Message;
                    ModelState.AddModelError("", "There is the same name   ");
                    return View(diet);
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("","Error");
             return View(diet);
        }
        
        // GET: Diets
        public async Task<ActionResult> Index()
        {
            return View(await db.Diet.ToListAsync());
        }

        // GET: Diets/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diet diet = await db.Diet.FindAsync(id);
            if (diet == null)
            {
                return HttpNotFound();
            }
            return View(diet);
        }

        
     

        // POST: Diets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
       

        // GET: Diets/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diet diet = await db.Diet.FindAsync(id);
            if (diet == null)
            {
                return HttpNotFound();
            }
            
            return View("Edit",diet);
        }
        public ActionResult GetDishEdit(Diet diet)
        {
           
            ViewBag.Basket = diet.Dish.ToList();
            var item_ = Array.ConvertAll(diet.Dish.ToArray<Dish>(), i => i.id);
            string result = "";
            for (int i = 0; i < item_.Count(); i++)
            {
                if (i == 0) result = item_[i].ToString();
                else result += item_[i].ToString();
            }
            var NotIn = (from i in db.Dish.ToList() where !Array.Exists(item_, j=> j== i.id  ) select i).ToList();
            try
            {

                if (Request.Cookies["Choose"]?.Value != null) { Request.Cookies["Choose"].Value = result; }
                else
                {
                HttpCookie cookie = new HttpCookie("Choose", result);
                Request.Cookies.Add(cookie);
                }
            }
            catch 
            {
                
                
            }  
           
            return PartialView("~/Views/Partial_View/Dishes.cshtml", NotIn); ;
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Subscription")] Diet diet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diet).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(diet);
        }

      
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Diet diet = await db.Diet.FindAsync(id);
            if (diet == null)
            {
                return HttpNotFound();
            }
            return View(diet);
        }

        // POST: Diets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Diet diet = await db.Diet.FindAsync(id);
            db.Diet.Remove(diet);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
