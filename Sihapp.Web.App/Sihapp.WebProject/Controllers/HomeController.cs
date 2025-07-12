using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sihapp.WebProject.Controllers
{
    public class HomeController : BasicController
    {

        [Authorize()]
        public ActionResult Index()
        {

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.UserType = GetUserFromDataContext().UserType;
            return View();

        }

       
    }
}