using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sihapp.InfoWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            ViewBag.ShowNav = false;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult SihappCrew()
        {
            ViewBag.Message = "Equipo Sihapp.";

            return View();
        }

    }
}