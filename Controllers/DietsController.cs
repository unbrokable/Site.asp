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
        string namecookies = "Choose";
        // !!! Много ко многим
        public ActionResult GetDishes()
        {
            int[] arr;
           var ChoosenArr = Request.Cookies[namecookies]?.Value?.Split(',');
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
        public ActionResult Create( Diet diet)
        {
            
            var ChoosenArr = Request.Cookies[namecookies]?.Value?.Split(',');
            int[] arr = Array.ConvertAll(ChoosenArr, s => int.Parse(s));
            if (ModelState.IsValid && arr != null)
            {
              
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
            SetCookies(namecookies, (int)id);
            
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
                else result += "," + item_[i].ToString();
            }
            var NotIn = (from i in db.Dish.ToList() where !Array.Exists(item_, j=> j== i.id  ) select i).ToList(); 
            return PartialView("~/Views/Partial_View/Dishes.cshtml", NotIn); ;
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Subscription,Description")] Diet diet)
        {
            if (ModelState.IsValid)
            {
                var ChoosenArr = Request.Cookies[namecookies]?.Value?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                int[] arr = (ChoosenArr != null)?Array.ConvertAll(ChoosenArr, s => int.Parse(s)): new int[0];
             
               db.Entry(diet).State = EntityState.Modified; 
                diet.UpdateDiet(arr);
              //await db.SaveChangesAsync();
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



        void SetCookies(string cookie, int id)
        {
            Diet diet = db.Diet.FirstOrDefault(i => i.Id == id);
            var item_ = Array.ConvertAll(diet.Dish.ToArray<Dish>(), i => i.id);
            string result = "";

            for (int i = 0; i < item_.Count(); i++)
            {
                if (i == 0) result = item_[i].ToString();
                else result += "," + item_[i].ToString();
            }

          SetCookies(cookie, result);

        }
       void SetCookies(string cookien , string value)
        {

            if (Request.Cookies[cookien]?.Value != null) 
            {
                HttpCookie cookie = new HttpCookie(cookien, value);
                Response.Cookies.Set(cookie);
            }
            else
            {
                HttpCookie cookie = new HttpCookie(cookien, value);
                  Response.Cookies.Set(cookie);
            }

        }
    }
}
