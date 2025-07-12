using Sihapp.WebProject.Backend.ServiceComponents;
using Sihapp.WebProject.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sihapp.WebProject.Controllers
{
    public class SihappManagementController : BasicController
    {
        // GET: SihappManagement
        public ActionResult SihappManagerV1()
        {
            var clinicNumber = GetClinicNumber();
            if (clinicNumber != 1000)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public ActionResult GetSihappAccounts(PaginationVM pagination)
        {
            var accounts = new ManagerService().GetSihappAccounts(pagination);
            return GetJsonNetResult(accounts);
        }

        public ActionResult DeletePemanetlyAccount(Guid clinicID, string masterKey)
        {
            string response = "";
            if (masterKey != "m4st3r92")
            {
                response = "La contraseña master es incorrecta";
                return GetJsonNetResult(response);
            }

            Action delete = () =>
            {
               new ManagerService().DeleteAccountPermanetly(clinicID, masterKey);
            };

            new UtilService().RunTransaction(delete);
            return GetJsonNetResult(response);

        }
    }
}