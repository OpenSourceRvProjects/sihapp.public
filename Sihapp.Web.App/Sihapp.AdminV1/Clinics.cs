using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Sihapp.AdminV1
{
    [TestClass]
    public class Clinics
    {
        SihappDBEntities DataContext = new SihappDBEntities();

        [TestMethod]
        public void DeleteClinicByNumber()
        {
            var clinicNumber = 1003;
            var clinic = DataContext.Clinic.Where(w => w.Number == clinicNumber).FirstOrDefault();
            var appointments = clinic.Appointment;
            var consults = appointments.SelectMany(sm => sm.Consult);
            var patients = clinic.Patients;
            var clinicMen = clinic.ClinicMen;
            var auxPersonal = clinic.AuxiliarPersonal;
            var clinicMenUser = clinic.ClinicMen.Select(sm => sm.AspNetUsers);
            var patientUser = clinic.Patients.Select(sm => sm.AspNetUsers);
            var auxPersonalUser = clinic.AuxiliarPersonal.Select(sm => sm.AspNetUsers);


            if (consults.Count() > 0)
                DataContext.Consult.RemoveRange(consults);

            if (clinicMen.Count() > 0)
                DataContext.ClinicMen.RemoveRange(clinicMen);


            DataContext.SaveChanges();

            
        }
    }
}
