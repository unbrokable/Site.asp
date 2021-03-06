using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ExampleB.Models;
using System.Threading.Tasks;

namespace ExampleB.Controllers
{
    public class AdminController : Controller
    {
        GoodFit db = new GoodFit();

        public ActionResult Index()
        {

            return View();
        }
        public ActionResult ShowUrers()
        {
            return View();
        }
        public ActionResult AddDish()
        {
            return View("AddDish");
        }

        [HttpPost]
        public ActionResult AddDish(Dish dish, HttpPostedFileBase image1)
        {
            // if (ModelState.IsValid) { 
            dish.Img = image1.ToByte();
            db.Dish.Add(dish);
            try
            {
                db.SaveChanges();
            }
            catch
            {
                db = new GoodFit();
            }
            //} 
            return RedirectToAction("ShowDish");
        }
        public ActionResult ShowDish()
        {
            GoodFit db = new GoodFit();
            var dish = db.Dish;
            return View(dish);
        }
        [HttpGet]
        public ActionResult EditDish(int Id)
        {
            var dish = db.Dish.FirstOrDefault((i) => i.id == Id);

            return View("EditDish", dish);
        }
        [HttpPost]
        public ActionResult EditDish(Dish dish, HttpPostedFileBase image1)
        {
            if (image1 != null) dish.Img = image1.ToByte();

            db.UpdateDishAsync(dish);

            return RedirectToAction("ShowDish");
        }
        [HttpGet]
        public ActionResult DeleteDish(int Id)
        {
            var dish = db.Dish.FirstOrDefault((i) => i.id == Id);

            return View("DeleteDish", dish);
        }
        [HttpPost, ActionName("DeleteDish")]
        public async Task<ActionResult> ConfirmDelete(int id)
        {
            Dish dish =  await db.Dish.FindAsync(id);
            db.Dish.Remove(dish);
            await db.SaveChangesAsync();
            return RedirectToAction("ShowDish");

        }
    }
}