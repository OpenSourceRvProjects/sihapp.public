using Microsoft.AspNet.Identity;
using Sihapp.WebProject.Backend.DataAccess;
using Sihapp.WebProject.Models;
using Sihapp.WebProject.Models.ClinicMen;
using Sihapp.WebProject.Models.TypesCatalogs;
using Sihapp.WebProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace Sihapp.WebProject.Backend.ServiceComponents
{
    public class ClinicMenService : BasicService
    {
        public void AddClinicMen(string cmName, string cmLastName1, string cmLastName2, DateTime cmBirthDate, string cmPhone, string cmEmail, string cmAddress, string cmUserName, string cmPassword)
        {
            var newCM = new ClinicMen();
            newCM.Name = cmName;
            newCM.LastName1 = cmLastName1;
            newCM.LastName2 = cmLastName2;
            newCM.RFC = "";
            newCM.Cedula= "";
            newCM.Phone = cmPhone;
            newCM.BirthDate = cmBirthDate;
            newCM.Address = cmAddress;
            newCM.CreationDate = DateTime.Now;
            newCM.HireDate = DateTime.Now;
            newCM.Type = 1; // doctor, dentista, psicologo, etc.
            newCM.UserID = CreateClinicMenUser(cmUserName, cmPassword);
            newCM.ClinicID = GetLoggedCurrentClinic().ID;
            newCM.ID = Guid.NewGuid();
            newCM.Email = cmEmail;
            newCM.Number = GetNewClinicMenNumber();

            UpdateClinicMenUserStatusType(newCM.UserID);
            DatabaseInstance.ClinicMen.Add(newCM);
            DatabaseInstance.SaveChanges();
        }

        public void UpdateSettings(ClinicMenSettingsVM settings)
        {
            var clinicMen = GetCurrentClinicMen();
            clinicMen.ConsultDuration = settings.ToleranceMinutes.HasValue ?  settings.ToleranceMinutes.Value : 5;
            clinicMen.BirthDate = settings.Birthdate;
            clinicMen.CellPhone = settings.Cellphone;
            clinicMen.RFC = settings.RFC;
            DatabaseInstance.SaveChanges();
        }

        public  ClinicMenSettingsVM GetClinicMenSettingsData()
        {
            var clinicMen = GetCurrentClinicMen();
            var settings = new ClinicMenSettingsVM();
            settings.Birthdate = clinicMen.BirthDate;
            settings.RFC = clinicMen.RFC;
            settings.Cellphone = clinicMen.CellPhone;
            settings.ToleranceMinutes = clinicMen.ConsultDuration.HasValue ? clinicMen.ConsultDuration.Value:  5;
            return settings;
        }

        private string CreateClinicMenUser(string cmUserName, string cmPassword)
        {
            if (new AccountService().IsUserAlreadyExist(cmUserName))
                throw new Exception("Ya existe un usuario con este nombre, favor de intentar con otro");

            var newUser = new ApplicationUser { UserName = cmUserName };
            var createTask = UserManager.Create(newUser, cmPassword);
            //createTask.Wait();
            return newUser.Id;
        }

        public void UpdateClinicMenUserStatusType(string aspNetUserID, int? clinicNumber = null)
        {
            var aspnetuser = DatabaseInstance.AspNetUsers.FirstOrDefault(f => f.Id == aspNetUserID);
            aspnetuser.UserType = UserTypeCodes.ClinicMen; // 0 = ClinicMen;
            aspnetuser.Admin = clinicNumber.HasValue ? true: false;
            aspnetuser.ClinicID = Guid.Empty;
            aspnetuser.ClinicNumber = clinicNumber.HasValue? clinicNumber: GetLoggedClinicNumber();
            DatabaseInstance.SaveChanges();
        }


        public IQueryable<ClinicMen> GetClinicMenByClinic()
        {
            var clinicID = GetLoggedCurrentClinic().ID;
            return DatabaseInstance.ClinicMen.Where(w => w.ClinicID == clinicID);
        }

        public int GetNewClinicMenNumber()
        {
            if (Thread.CurrentPrincipal.Identity.Name == "")
                return 1000;

            int Number;
            if (GetClinicMenByClinic().Any())
                Number = GetClinicMenByClinic().OrderByDescending(o => o.Number).FirstOrDefault().Number + 1;
            else
                Number = 1001;
            return Number;

        }

        public ClinicMen GetClinicMenByID(Guid ClinicMenID)
        {
            return GetClinicMenByClinic().Where(w => w.ID == ClinicMenID).FirstOrDefault();
        }
    }
}