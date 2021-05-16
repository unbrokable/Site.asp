using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ExampleB.Models;
namespace ExampleB.Controllers
{

    public class UserController : Controller
    {
        GoodFit db = new GoodFit();
        int calories = 0;
        [Authorize]
        public async Task<ActionResult> Index()
        {
            List<Product> products = new List<Product>();
            await Task.Run(() => products = Parsing("https://www.moh.gov.sa/en/HealthAwareness/Campaigns/badana/Pages/009.aspx"));
            //products.Add(new Product("Bread", 12));
            //products.Add(new Product("Low calory", 32));
           // products.Add(new Product("Now", 44));

           

            var name = HttpContext.User.Identity.Name;
            Users user = db.Users.FirstOrDefault(i => i.Email.CompareTo(name) == 0);
            ViewBag.CurDiet = user.UserDiet.FirstOrDefault();
            DietView diet = db.DietView.FirstOrDefault(i => i.Id == db.UserDiet.FirstOrDefault(j => j.User_Id == user.Id).Diet_Id);
            ViewBag.Name = user.Name;
            ViewBag.StarDate = user.UserDiet.FirstOrDefault()?.Date_Start;
          
            ViewBag.Products = new SelectList(products , "Calories", "Name");
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
            return PartialView("~/Views/Partial_View/_Diets.cshtml", mod);
        }
        public ActionResult GetTodayDish()
        {
            var name = HttpContext.User.Identity.Name;
            Users user = db.Users.FirstOrDefault(i => i.Email.CompareTo(name) == 0);
            DietView dietview = db.DietView.FirstOrDefault(i => i.Id == db.UserDiet.FirstOrDefault(j => j.User_Id == user.Id).Diet_Id);
            Diet diet = db.Diet.FirstOrDefault(i => i.Id == dietview.Id);
            List<DishTime> dishes = new List<DishTime>();
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, DateTime.Now.Second);

            foreach (var item in diet.Dish.ToList().Take(3))
            {
                calories += item.Сalories;
                dishes.Add(new DishTime(item, date));
                date = date.AddHours(4);
            }


            return PartialView("~/Views/Partial_View/GetTodayDish.cshtml", dishes);
        }
        public ActionResult GetCalories()
        {
            var name = HttpContext.User.Identity.Name;
            Users user = db.Users.FirstOrDefault(i => i.Email.CompareTo(name) == 0);
            History_user_diet curdiet = db.History_user_diet.FirstOrDefault(i => i.User_Id == user.Id && i.Date_write.Month == DateTime.Now.Month && DateTime.Now.Day == i.Date_write.Day);
            if (curdiet == null)
            {
                curdiet = new History_user_diet() {
                    User_Id = user.Id,
                    Date_write = DateTime.Now,
                    Calories_Amount = CountCalories()

                };
                db.History_user_diet.Add(curdiet);
                db.SaveChanges();
            }

            return Content(curdiet.Calories_Amount.ToString());

        }
        [HttpPost]
        public ActionResult EatSomething(int products , int calor)
        {

            var name = HttpContext.User.Identity.Name;
            Users user = db.Users.FirstOrDefault(i => i.Email.CompareTo(name) == 0);
            History_user_diet curdiet = db.History_user_diet.FirstOrDefault(i => i.User_Id == user.Id && i.Date_write.Month == DateTime.Now.Month && DateTime.Now.Day == i.Date_write.Day);
            curdiet.Calories_Amount += products;
             db.SaveChanges();
            ViewBag.Amount = curdiet.Calories_Amount;
            return PartialView("~/Views/Partial_View/EatSomething.cshtml");
        }
        int CountCalories()
        {
            var name = HttpContext.User.Identity.Name;
            Users user = db.Users.FirstOrDefault(i => i.Email.CompareTo(name) == 0);
            DietView dietview = db.DietView.FirstOrDefault(i => i.Id == db.UserDiet.FirstOrDefault(j => j.User_Id == user.Id).Diet_Id);
            Diet diet = db.Diet.FirstOrDefault(i => i.Id == dietview.Id);
            List<DishTime> dishes = new List<DishTime>();
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, DateTime.Now.Second);
            int sum = 0;
            foreach (var item in diet.Dish.ToList().Take(3))
            {
                sum += item.Сalories;

            }
            return sum;
        }

        static  List<Product> Parsing(string url)
        {
            List<Product> products = new List<Product>(100);
            StringBuilder builder = new StringBuilder();
            using (HttpClientHandler hdl = new HttpClientHandler() { AllowAutoRedirect = false, AutomaticDecompression= System.Net.DecompressionMethods.Deflate| System.Net.DecompressionMethods.GZip| System.Net.DecompressionMethods.None})
            {
                using (var client = new HttpClient(hdl))
                {
                    using (var resp =  client.GetAsync(url).Result)
                    {

                        if (resp.IsSuccessStatusCode)
                        {
                            var html = resp.Content.ReadAsStringAsync().Result;
                            if (!string.IsNullOrEmpty(html))
                            {
                                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                                doc.LoadHtml(html);
                                var items = doc.DocumentNode.SelectNodes("//tr");
                                foreach (var item in items)
                                {
                                    var name = item.SelectNodes(".//td//div");
                                    if (name != null) {
                                        string temp = "";
                                        foreach (var j in name)
                                        {
                                            temp += j.InnerText + ";";
                                           
                                        }
                                        var arr = temp.Split(';');
                                        try
                                        {
                                            int number = int.Parse(arr[0]);
                                            if (String.IsNullOrEmpty(arr[2])) throw new ArgumentException();
                                            string food = arr[2];
                                            products.Add( new Product(food,number));
                                        }
                                        catch (Exception e) { }
                                       
                                    }

                                }
                            }
                            

                        }
                    }
                }

            }
            return products;
        }
            
        }
}

