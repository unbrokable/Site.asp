using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ExampleB.Controllers
{
    public class HomeController : Controller
    {
        
         [Authorize]
        public ActionResult Index()
        {
            //FormsAuthentication.SetAuthCookie(false);
            string resul = "None";
            if (User.Identity.IsAuthenticated)
            {
                resul = User.Identity.Name;
            }
            return View();
        }
       
       
    }
}