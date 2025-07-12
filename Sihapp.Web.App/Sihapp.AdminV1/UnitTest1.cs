using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Sihapp.WebProject.Models.Common;
using Sihapp.WebProject.Models.Users;
using System.Collections.Generic;
using Sihapp.WebProject.Models.TypesCatalogs;
using Sihapp.WebProject.Backend.ServiceComponents;

namespace Sihapp.AdminV1
{
    [TestClass]
    public class UnitTest1
    {

        public static SihappDBEntities DataContext = new SihappDBEntities();
        [TestMethod]
        public void GetAllKindOfUserInAccount()
        {

            var allUsers = GetAllUsers(100000);
            var users = allUsers.Select(s => new
            {
                UserName = s.UserName,
                ID = s.Id,
                Type = s.UserType,
            });

            Assert.IsTrue(users.Count() > 0);
        }

        [TestMethod]
        public void GetPagedUsers()
        {

            var allUsers = GetAllUsers(1088);
            var users = allUsers.Select(s => new
            {
                UserName = s.UserName,
                ID = s.Id,
                Type = s.UserType,
            });

            var pagination = new PaginationVM() { Page = 1001, PageSize = 10 };

           var TotalPages = Math.Ceiling((decimal)allUsers.Count() / pagination.PageSize);
            var data = users.OrderByDescending(o => o.UserName).Skip(pagination.PageSize * pagination.Page)
                .Take(pagination.PageSize).ToList();

            Assert.IsTrue(data.Count() > 0);

        }

        [TestMethod]
        public void GetPagedUsersInModel()
        {
            var pagedUsers = new PageableUsersVM();


            var allUsers = GetAllUsers(1088);
            var users = allUsers.Select(s => new
            {
                UserName = s.UserName,
                ID = s.Id,
                Type = s.UserType,
            });

            var pagination = new PaginationVM() { Page = 0, PageSize = 10 };

            pagedUsers.TotalPages = Math.Ceiling((decimal)allUsers.Count() / pagination.PageSize);
            var data = users.OrderByDescending(o => o.UserName).Skip(pagination.PageSize * pagination.Page)
                .Take(pagination.PageSize).ToList();


            var usrs = new List<UserItemsVM>();

            var patients = new List<PatientUserVM>();

            var patientList = data.Where(w => w.Type == UserTypeCodes.Patient).ToList();
            foreach (var p in patientList)
            {
                var patientID = DataContext.Patients.Where(w=> w.UserID  == p.ID).FirstOrDefault().Id;
                var currentPatient = new PatientUserVM(patientID);
                var patient =  DataContext.Patients.Where(w => w.Id == patientID).FirstOrDefault();
                currentPatient.CreationDate = patient.Birthdate;
                currentPatient.Name = patient.Name + " " + patient.LastName1 + " " + patient.LastName2;
                currentPatient.UserName = p.UserName;
                currentPatient.Type = p.Type;
                currentPatient.ID = p.ID;
                patients.Add(currentPatient);
            }

            var clinicMens = new List<ClinicMenUserVM>();
            var clinicMenList = data.Where(w => w.Type == UserTypeCodes.ClinicMen).ToList();
            foreach (var cm in clinicMenList)
            {
                var clinicMenId = DataContext.ClinicMen.Where(w => w.UserID == cm.ID).FirstOrDefault().ID;
                var currentClinicMen = new ClinicMenUserVM(clinicMenId);
                var clinicMen = DataContext.ClinicMen.Where(w => w.ID == clinicMenId).FirstOrDefault(); ;
                currentClinicMen.CreationDate = currentClinicMen.CreationDate;
                currentClinicMen.Name = clinicMen.Name + " " + clinicMen.LastName1 + " " + clinicMen.LastName2;
                currentClinicMen.UserName = cm.UserName;
                currentClinicMen.Type = cm.Type;
                currentClinicMen.ID = cm.ID;
                clinicMens.Add(currentClinicMen);

            }
            var clinicAuxiliar = new List<ClinicAuxiliarUserVM>();
            var clinicAuxiliarList = data.Where(w => w.Type == UserTypeCodes.AuxiliarPersonal).ToList();
            foreach (var ca in clinicAuxiliarList)
            {
                var auxiliarPersonalId = DataContext.AuxiliarPersonal.Where(w => w.UserID == ca.ID).FirstOrDefault().Id;
                var currentAuxiliarPersonal = new ClinicAuxiliarUserVM(auxiliarPersonalId);
                var auxiliarpersonal = DataContext.AuxiliarPersonal.Where(w => w.Id == auxiliarPersonalId).FirstOrDefault();
                currentAuxiliarPersonal.CreationDate = auxiliarpersonal.CreationDate;
                currentAuxiliarPersonal.Name = auxiliarpersonal.Name + " " + auxiliarpersonal.LastName1 + " "
                    + auxiliarpersonal.LastName2;
                currentAuxiliarPersonal.UserName = ca.UserName;
                currentAuxiliarPersonal.Type = ca.Type;
                currentAuxiliarPersonal.ID = ca.ID;
                clinicAuxiliar.Add(currentAuxiliarPersonal);
            }
            //usrs.AddRange(patients);
            //usrs.AddRange(clinicMens);
            //usrs.AddRange(clinicAuxiliar);
            pagedUsers.UserItems = usrs;

            Assert.IsTrue(pagedUsers.UserItems.Count() > 0);
        }

        [TestMethod]
        public void ValidarUsuarioExistente()
        {
            string userName = "demo";
            var existUser = DataContext.AspNetUsers.Any(a => a.UserName == userName);
            Assert.IsFalse(existUser);
        }

        public IQueryable<AspNetUsers> GetAllUsers(int ClinicNumber)
        {
            var clinicNumber = ClinicNumber; //GetClinicNumber() instead in production
            return DataContext.AspNetUsers.Where(w => w.ClinicNumber == clinicNumber);

        }

    }
}
