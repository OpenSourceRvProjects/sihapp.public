using Microsoft.AspNet.Identity;
using Sihapp.WebProject.Backend.DataAccess;
using Sihapp.WebProject.Models;
using Sihapp.WebProject.Models.TypesCatalogs;
using Sihapp.WebProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sihapp.WebProject.Backend.ServiceComponents
{
    public class AuxiliarPersonalService : BasicService
    {
        public IQueryable<AuxiliarPersonal> GetAuxiliarPersonalByClinic()
        {
            var clinicID = GetLoggedCurrentClinic().ID;
            return DatabaseInstance.AuxiliarPersonal.Where(w => w.ClinicID == clinicID);
        }

        public int GetNewAuxiliarPersonalNumber()
        {
            int auxiliarNumber;
            if (GetAuxiliarPersonalByClinic().Any())
                auxiliarNumber = GetAuxiliarPersonalByClinic().OrderByDescending(o => o.Number).FirstOrDefault().Number + 1;
            else
                auxiliarNumber = 1001;
            return auxiliarNumber;

        }


        public void AddAuxiliarPersonal(string cmName, string cmLastName1, string cmLastName2, DateTime cmBirthDate, string cmPhone, string cmEmail, string cmAddress, string cmUserName, string cmPassword)
        {
            var newAP = new AuxiliarPersonal();
            newAP.Name = cmName;
            newAP.LastName1 = cmLastName1;
            newAP.LastName2 = cmLastName2;
            //newAP.RFC = "";
            //newAP.Cedula = "";
            newAP.Phone = cmPhone;
            newAP.BirthDate = cmBirthDate;
            newAP.Address = cmAddress;
            newAP.CreationDate = DateTime.Now;
            newAP.HireDate = DateTime.Now;
            //newAP.Type = 1; // doctor, dentista, psicologo, etc.
            try
            {
                newAP.UserID = CreateAuxiliarUser(cmUserName, cmPassword);
                newAP.ClinicID = GetLoggedCurrentClinic().ID;
                newAP.Id = Guid.NewGuid();
                newAP.Number = GetNewAuxiliarPersonalNumber();

                UpdateAuxiliarStatusType(newAP.UserID);
                DatabaseInstance.AuxiliarPersonal.Add(newAP);
                DatabaseInstance.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public string CreateAuxiliarUser(string userName, string password)
        {
            if (new AccountService().IsUserAlreadyExist(userName))
                throw new Exception("Ya existe un usuario con este nombre");

            var newUser = new ApplicationUser { UserName = userName };
            var createTask = UserManager.Create(newUser, password);
            //createTask.Wait();
            return newUser.Id;
        }


        public void UpdateAuxiliarStatusType(string aspNetUserID)
        {
            var aspnetuser = DatabaseInstance.AspNetUsers.FirstOrDefault(f => f.Id == aspNetUserID);
            if (aspnetuser == null)
                throw new Exception("No se pudo crear el usuario, revisa que la contraseña tenga mas de 6 caracteres.");
            aspnetuser.UserType = UserTypeCodes.AuxiliarPersonal; // 2 = auxiliar;
            aspnetuser.Admin = false;
            aspnetuser.ClinicID = Guid.Empty;
            aspnetuser.ClinicNumber = GetLoggedClinicNumber();
            DatabaseInstance.SaveChanges();
        }

        public AuxiliarPersonal GetAuxiliarPersonalByID(Guid auxiliarPersonalId)
        {
            var clinicID = GetLoggedCurrentClinic().ID;
            var auxiliarPersonal = DatabaseInstance.AuxiliarPersonal.Where(w => w.ClinicID == clinicID && w.Id == auxiliarPersonalId)
                .FirstOrDefault();
            return auxiliarPersonal;
        }
    }
}