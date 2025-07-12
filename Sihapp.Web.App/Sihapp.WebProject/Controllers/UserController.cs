using Sihapp.WebProject.Backend.ServiceComponents;
using Sihapp.WebProject.Models;
using Sihapp.WebProject.Models.Common;
using Sihapp.WebProject.Models.Users;
using Sihapp.WebProject.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sihapp.WebProject.Controllers
{
    public class UserController : BasicController
    {
        // GET: User
        public ActionResult UsersList()
        {
            if (!User.Identity.IsAuthenticated || GetUserFromDataContext().UserType != UserTypeCodes.ClinicMen)
                return RedirectToAction("Login", "Account");

            var patientsList = new PatientService().GetPatientsWithNoUserListTV();
            ViewBag.BackendData = SerializeJSONData(patientsList);
            return View();
        }

        public ActionResult Tokens()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //var patientsList = new PatientSC().GetPatientsWithNoUserListTV();
            //ViewBag.BackendData = SerializeJSONData(patientsList);
            return View();
        }
        [HttpPost]
        public ActionResult GetPagedUsers(string search, PaginationVM pagination)
        {
            var pagedUsers = new AccountService().GetPagedUsers(search, pagination);
            return GetJsonNetResult(pagedUsers);
        }

        [HttpPost]
        public ActionResult GetTokensFromUserId(Guid userId)
        {
            return GetJsonNetResult(null);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Tokens(TokenUserCredentialsVM model)
        {
            model.Token= new AccountService().GenerateDevToken( model);
            if (string.IsNullOrWhiteSpace(model.Token))
                model.Message = "Token no generado, favor de revisar usuario y contraseña";
            return View(model);
        }

        //[HttpPost]
        //public ActionResult AddUser(NewUserVM user) {
        //    new AccountSC().AddUser(user);
        //    return GetJsonNetResult(null);
        //}
    }
}