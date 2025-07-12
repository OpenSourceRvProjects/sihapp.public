using Sihapp.WebProject.Backend;
using Sihapp.WebProject.Backend.ServiceComponents;
using Sihapp.WebProject.Models.ClinicMen;
using Sihapp.WebProject.Models.TypesCatalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sihapp.WebProject.Controllers
{
    public class ClinicMenController : BasicController
    {
        // GET: ClinicMen
        [HttpPost]
        [HandleException]
        public ActionResult AddClinicMen(string cmName, string cmLastName1, string cmLastName2, DateTime cmBirthDate,
           string cmPhone, string cmEmail, string cmAddress, string cmUserName, string cmPassword )
        {
            new ClinicMenService().AddClinicMen(cmName, cmLastName1, cmLastName2, cmBirthDate,
                cmPhone, cmEmail, cmAddress, cmUserName, cmPassword);
            return GetJsonNetResult(null);
        }

        public ActionResult Settings()
        {
            if (!User.Identity.IsAuthenticated || GetUserFromDataContext().UserType != UserTypeCodes.ClinicMen)
                return RedirectToAction("Login", "Account");


            ViewBag.backendData = SerializeJSONData(new ClinicMenService().GetClinicMenSettingsData());
            return View();
        }

        [HttpPost]
        public ActionResult UpdateSettings(ClinicMenSettingsVM settings)
        {
            new ClinicMenService().UpdateSettings(settings);
            return GetJsonNetResult(null);
        }
    }
}