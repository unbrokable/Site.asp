using ExampleB.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Facebook;
using System.Threading;
using System.Web;
using System.Web.Configuration;

namespace ExampleB.Controllers
{
    public class AccountController : Controller
    {
        //function help
        bool InDataBase(string email )
        {
            Users user = null;
            using (GoodFit db = new GoodFit())
            {
                user = db.Users.FirstOrDefault(i => i.Email == email);
                
            }
            return (user == null) ? false : true;
        }
        public ActionResult SingOut()
        {
            
            FormsAuthentication.SignOut();
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);
            
            FormsAuthentication.RedirectToLoginPage();
            return View("Login");
        }
        //Google
       [HttpPost]
        public ActionResult GoogleLogin(string email, string name, string gender, string lastname, string location)
        {
            TempData["email"] = email;
            TempData["name"] = name + " " + lastname??"";


            if (InDataBase(email)) {
                FormsAuthentication.SetAuthCookie(email, false);
                return Json(new { redirectToUrl = Url.Action("Login") });
            } 
            return Json(new { redirectToUrl = Url.Action("Register") });
        }
        //Facebook
        private Uri RediredtUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        [AllowAnonymous]
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "826303051258747",

                client_secret = "da0475691a522988d1875ce2835eeee9",

                redirect_uri = RediredtUri.AbsoluteUri,

                response_type = "code",

                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "826303051258747",
                client_secret = "da0475691a522988d1875ce2835eeee9",
                redirect_uri = RediredtUri.AbsoluteUri,
                code = code

            });
            var accessToken = result.access_token;

            Session["AccessToken"] = accessToken;

            fb.AccessToken = accessToken;

            dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");

            string email = me.email;

            TempData["email"] = me.email;

            TempData["name"] = me.first_name + " " + me.last_name;

            TempData["lastname"] = me.last_name;

            TempData["picture"] = me.picture.data.url;

            

            if (InDataBase(me.email)) { 
            
                FormsAuthentication.SetAuthCookie(email, true);
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Register", "Account");



        }
        //Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = null;
                using (GoodFit db = new GoodFit())
                {
                    user = db.Users.FirstOrDefault(i => i.Name == model.Name && i.Password == model.Password);
                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //return RedirectToAction("Register", "Account");
                    ModelState.AddModelError("", "Error");
                }
            }
            return View(model);
        }
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model, string Email)
        {
            if (ModelState.IsValid)
            {
                Users user = null;
                using ( GoodFit db = new GoodFit())
                {
                    user = db.Users.FirstOrDefault(i=>i.Name == model.Name);
                }
                if(user == null)
                {
                    using (GoodFit db = new GoodFit())
                    {
                        db.Users.Add(new Users() { Name = model.Name, Email = model.Email, Password = model.Password, Subscription = model.Subscription });
                        try { 
                        db.SaveChanges();
                        }
                        catch
                        {
                            ModelState.AddModelError("", "Error");
                        }
                        user = db.Users.Where(i => i.Name == model.Name && i.Password == model.Password).FirstOrDefault();
                    }
                    if(user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Email, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
            {
                 ModelState.AddModelError("", "Error");
            } 
            }
           return View(model);
        }

        public ActionResult About()
        {
            string file_path = Server.MapPath("~/Files/About.pdf");
            string file_type = "application/pdf";
            string file_name = "About.pdf";
            return File(file_path, file_type, file_name);
        }

   
       


    }
}