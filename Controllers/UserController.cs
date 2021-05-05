using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ExampleB.Models;
namespace ExampleB.Controllers
{
    public class UserController : Controller
    {
        GoodFit db = new GoodFit();
        
        [Authorize]
        public ActionResult Index()
        {
           var name = HttpContext.User.Identity.Name;
           
              Users user = db.Users.FirstOrDefault(i => i.Email.CompareTo(name) == 0);
            ViewBag.CurDiet = user.UserDiet.FirstOrDefault();
              DietView diet =  db.DietView.FirstOrDefault(i => i.Id == db.UserDiet.FirstOrDefault( j => j.User_Id == user.Id).Diet_Id);
            ViewBag.Name = user.Name;


            return View("Index", diet);
        }

        public ActionResult ChangeDiet(int id)
        {
            var name = HttpContext.User.Identity.Name;
            Users user = db.Users.FirstOrDefault(i => i.Email.CompareTo(name) == 0);

            db.UserDiet.RemoveRange(db.UserDiet.Where(i => i.User_Id == user.Id).ToList());

            db.UserDiet.Add(new UserDiet()
            {
                Diet_Id = id,
                User_Id = user.Id,
                Date_Start = DateTime.Now
            });

            db.SaveChanges();
            return RedirectToRoute(new
            {
                controller = "User",
                action = "Index",
                
            });
        }
        public ActionResult GetPartialDiets()
        {
            var mod = db.DietView.ToList();
            return PartialView("~/Views/Partial_View/_Diets.cshtml",mod);
        }
        
    }
}