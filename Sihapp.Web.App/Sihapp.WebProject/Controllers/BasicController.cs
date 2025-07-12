using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Sihapp.WebProject.Backend.DataAccess;
using Sihapp.WebProject.Backend.ServiceComponents;
using Sihapp.WebProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Sihapp.WebProject.Controllers
{
    public class BasicController : Controller
    {
        // GET: Basic

        public BasicController()
        {

        }
        public SihappDBEntities DataContext = new SihappDBEntities();

        public UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

     

        public ApplicationUser LoggedUser
        {
            get
            {
                ApplicationUser user = UserManager.FindByNameAsync(Thread.CurrentPrincipal.Identity.Name).Result;
                return user;
            }
        }


        public JsonNetResult GetJsonNetResult(object data)
        {
            JsonNetResult jsonNetResult = new JsonNetResult();
            jsonNetResult.Formatting = Formatting.Indented;
            jsonNetResult.Data = data;
            return jsonNetResult;

        }

        public object SerializeJSONData(object data)
        {
            var result = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.None,
            new Newtonsoft.Json.JsonSerializerSettings
            {
                StringEscapeHandling =
                Newtonsoft.Json.StringEscapeHandling.EscapeHtml
            });

            return result;
        }

        public int GetClinicNumber()
        {

            return DataContext.AspNetUsers.Where(w => w.UserName == Thread.CurrentPrincipal.Identity.Name).FirstOrDefault().ClinicNumber.Value;
        }

        public string GetUserName()
        {
            var userName = "";
            if (GetUserFromDataContext().UserType == 0)
            {
                userName = DataContext.ClinicMen.FirstOrDefault(w => w.UserID == LoggedUser.Id).Name;
            }

            if (GetUserFromDataContext().UserType == 1)
            {
                userName = DataContext.Patients.FirstOrDefault(w => w.UserID == LoggedUser.Id).Name;
            }

            if (GetUserFromDataContext().UserType == 2)
            {
                userName = DataContext.AuxiliarPersonal.FirstOrDefault(w => w.UserID == LoggedUser.Id).Name;
            }
            return userName;
        }

        public string GetClinicName() {
            var clinicNumber = GetClinicNumber();
            return DataContext.Clinic.Where(w => w.Number == clinicNumber).FirstOrDefault().Name;

        }

        public AspNetUsers GetUserFromDataContext()
        {
            return DataContext.AspNetUsers.Where(w => w.Id == LoggedUser.Id).FirstOrDefault();
        }
    }
}