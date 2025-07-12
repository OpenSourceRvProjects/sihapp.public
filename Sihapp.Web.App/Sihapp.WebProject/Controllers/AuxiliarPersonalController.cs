using Sihapp.WebProject.Backend;
using Sihapp.WebProject.Backend.ServiceComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sihapp.WebProject.Controllers
{
    public class AuxiliarPersonalController : BasicController 
    {

        [HttpPost]
        [HandleException]
        // GET: AuxiliarPersonal
        public ActionResult AddAuxiliarPersonal(string cmName, string cmLastName1, string cmLastName2, DateTime cmBirthDate,
           string cmPhone, string cmEmail, string cmAddress, string cmUserName, string cmPassword)
        {
            if (new AccountService().IsUserAlreadyExist(cmUserName))
                throw new Exception("Ya existe un usuario con este nombre");

            new AuxiliarPersonalService().AddAuxiliarPersonal(cmName, cmLastName1, cmLastName2, cmBirthDate,
                cmPhone, cmEmail, cmAddress, cmUserName, cmPassword);
            return GetJsonNetResult(null);
        }
    }
}