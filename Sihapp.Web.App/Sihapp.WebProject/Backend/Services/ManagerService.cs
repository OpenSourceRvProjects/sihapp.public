using Sihapp.WebProject.Backend.DataAccess;
using Sihapp.WebProject.Models.Common;
using Sihapp.WebProject.Models.Manager;
using Sihapp.WebProject.Models.StatusCodes;
using Sihapp.WebProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sihapp.WebProject.Backend.ServiceComponents
{
    public class ManagerService : BasicService
    {
        public SihappAccountsPagedVM GetSihappAccounts(PaginationVM pagination)
        {
            var accountsPaged = new SihappAccountsPagedVM();
            var allAccounts = DatabaseInstance.Clinic.Where(w => w.Number != 1000);

            accountsPaged.AccountCount = allAccounts.Count();
            accountsPaged.TotalPages = Math.Ceiling((decimal)accountsPaged.AccountCount/ pagination.PageSize);
            var queryAccounts = DatabaseInstance.Clinic.Where(w => w.Number != 1000)
                .OrderByDescending(o => o.Number).Skip(pagination.PageSize * pagination.Page).Take(pagination.PageSize);

            var accounts = queryAccounts.Select(s => new
            {
                s.Number,
                s.ID,
                s.Name,
                PC = s.Patients.Count(),
                PI = s.ConsultImages.Count(),
                Consults = s.Appointment.Where(w => w.Status == AppointmentStatusCodes.Close || w.Status == AppointmentStatusCodes.ConsultOnCourse).Count(),
            }).ToList();

            var accountList = accounts.Select(s => new SihappAccountsVM
            {
                User = GetAdminUserByClinicID(s.ID),
                MasterUser = GetAdminClinicMenByClinicID(s.ID).Name + " " + GetAdminClinicMenByClinicID(s.ID).LastName1,
                ClinicID = s.ID,
                Number = s.Number,
                ClinicName = s.Name,
                PatientsQty = s.PC,
                ClinicImages = s.PI,
                Consults = s.Consults,
                Email = GetAdminClinicMenByClinicID(s.ID).Email,
                Phone = GetAdminClinicMenByClinicID(s.ID).Phone,

            })
                .ToList();


            accountsPaged.items = accountList;
            return accountsPaged;
        }

        public string GetAdminUserByClinicID(Guid clinicID)
        {
            var clinicNumber = DatabaseInstance.Clinic.Where(w => w.ID == clinicID).FirstOrDefault().Number;

            return DatabaseInstance.AspNetUsers.Where(w => w.ClinicNumber == clinicNumber && w.Admin)
                .FirstOrDefault()?.UserName;
        }

        public ClinicMen GetAdminClinicMenByClinicID(Guid clinicID)
        {
            var clinicNumber = DatabaseInstance.Clinic.Where(w => w.ID == clinicID).FirstOrDefault().Number;

            var clinicMen = DatabaseInstance.AspNetUsers.Where(w => w.ClinicNumber == clinicNumber && w.Admin)
                .FirstOrDefault()?.ClinicMen?.FirstOrDefault();
            if (clinicMen == null)
                return new ClinicMen();

            return clinicMen;
        }

        public string DeleteAccountPermanetly(Guid clinicID, string masterKey)
        {



            var clinic = DatabaseInstance.Clinic.Where(w => w.ID == clinicID).FirstOrDefault();
            var ConsultImages = clinic.ConsultImages;
            if (ConsultImages.Count > 0)
            {
                DatabaseInstance.ConsultImages.RemoveRange(ConsultImages);
                DatabaseInstance.SaveChanges();
            }

            var consults = DatabaseInstance.Consult.Where(w => w.Appointment.ClinicID == clinic.ID);
            if (consults.Count() > 0)
            {
                DatabaseInstance.Consult.RemoveRange(consults);
                DatabaseInstance.SaveChanges();

            }

            var appointments = clinic.Appointment.AsQueryable();
            if (appointments.Count() > 0)
            {
                DatabaseInstance.Appointment.RemoveRange(appointments);
                DatabaseInstance.SaveChanges();

            }

            var patients = clinic.Patients.AsQueryable();
            if (patients.Count() > 0)
            {
                DatabaseInstance.Patients.RemoveRange(patients);
                DatabaseInstance.SaveChanges();

            }

            var clinicMen = clinic.ClinicMen.AsQueryable();
            if (clinicMen.Count() > 0)
            {
                DatabaseInstance.ClinicMen.RemoveRange(clinicMen);
                DatabaseInstance.SaveChanges();

            }

            var clinicAuxiliar = clinic.AuxiliarPersonal.AsQueryable();
            if (clinicAuxiliar.Count() > 0)
            {
                DatabaseInstance.AuxiliarPersonal.RemoveRange(clinicAuxiliar);
                DatabaseInstance.SaveChanges();
            }

            DatabaseInstance.Clinic.Remove(clinic);
            DatabaseInstance.SaveChanges();
            return "";
        }
    }
}