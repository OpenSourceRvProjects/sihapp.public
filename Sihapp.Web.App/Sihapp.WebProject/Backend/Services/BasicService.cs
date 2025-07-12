using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Net;
using Sihapp.WebProject.Backend.DataAccess;
using Sihapp.WebProject.Models;

namespace Sihapp.WebProject.Services
{
    public abstract class BasicService
    {
        public SihappDBEntities DatabaseInstance = GetEntitiesCurrentContext();
        // public SihappDBEntities DatabaseInstance = new SihappDBEntities();

        public UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));


        public ApplicationUser LoggedUser
        {
            get
            {
                ApplicationUser user = UserManager.FindByNameAsync(Thread.CurrentPrincipal.Identity.Name).Result;
                return user;
            }
        }


        public int GetLoggedClinicNumber()
        {

            return DatabaseInstance.AspNetUsers.Where(w => w.UserName == LoggedUser.UserName).FirstOrDefault().ClinicNumber.Value;

        }

        public Clinic GetLoggedCurrentClinic()
        {
            var clinicNumber = GetLoggedClinicNumber();
            return DatabaseInstance.Clinic.FirstOrDefault(f => f.Number == clinicNumber);
        }

        //https://stackoverflow.com/questions/18635508/dbcontext-has-been-disposed
        //Error: SihappDBEntities has been disposed
        public static SihappDBEntities GetEntitiesCurrentContext()
        {

            var key = "SihappContext_" + HttpContext.Current.GetHashCode().ToString("x") +
                      Thread.CurrentContext.ContextID.ToString();

            SihappDBEntities context = HttpContext.Current.Items[key] as SihappDBEntities;
            if (context == null)
            {
                context = new SihappDBEntities();
                HttpContext.Current.Items[key] = context;
            }
            return context;

        }

        public string SerializeObject(object data)
        {
            var settings = new Newtonsoft.Json.JsonSerializerSettings
            {
                StringEscapeHandling =
                Newtonsoft.Json.StringEscapeHandling.EscapeHtml
            };
            return JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.None, settings);

        }


        public AspNetUsers GetUserByName(string userName)
        {
            var usr = userName + "+" + GetLoggedClinicNumber().ToString();
            return DatabaseInstance.AspNetUsers.FirstOrDefault(f => f.UserName == usr);

        }

        public AspNetUsers GetUserFromDataContext()
        {
            return DatabaseInstance.AspNetUsers.Where(w => w.Id == LoggedUser.Id).FirstOrDefault();
        }


        public void AddLog(string message, bool isException = true, string innerException = "", Guid? userID = null, Guid? clinicID = null)
        {
            var log = new Logs()
            {
                Message = message,
                InnerMessaje = innerException,
                IsException = isException,
                UserID = userID,
                ClinicID = clinicID,
            };
            //No idea of why this is not working, the model is on DB and also in EDMX model
            //DatabaseInstance.Logs.Add(log);
            //DatabaseInstance.SaveChanges();
        }


        public int GetLoggedUserType()
        {
            return GetUserFromDataContext().UserType;
        }


        public ClinicMen GetCurrentClinicMen()
        {
            return DatabaseInstance.ClinicMen.Where(w => w.UserID == LoggedUser.Id).FirstOrDefault();
        }

        public AuxiliarPersonal GetCurrentAuxiliarPersonal()
        {
            return DatabaseInstance.AuxiliarPersonal.Where(w => w.UserID == LoggedUser.Id).FirstOrDefault();
        }

        public Patients GetCurrentPatient()
        {
            return DatabaseInstance.Patients.Where(w => w.UserID == LoggedUser.Id).FirstOrDefault();
        }
    }
}