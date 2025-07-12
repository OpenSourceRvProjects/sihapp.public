using Sihapp.WebProject.Backend.DataAccess;
using Sihapp.WebProject.Models;
using Sihapp.WebProject.Models.Consult;
using Sihapp.WebProject.Models.StatusCodes;
using Sihapp.WebProject.Models.TypesCatalogs;
using Sihapp.WebProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sihapp.WebProject.Backend.ServiceComponents
{
    public class ConsultService : BasicService
    {

        public IQueryable<Consult> GetConsultsByClinic()
        {
            var clinicID = GetLoggedCurrentClinic().ID;
            return DatabaseInstance.Consult.Where(w => w.Appointment.ClinicMen.ClinicID == clinicID);
        }

        public int GetNewConsultNumber()
        {
            int Number;
            if (GetConsultsByClinic().Any())
                Number = GetConsultsByClinic().OrderByDescending(o => o.Number).FirstOrDefault().Number + 1;
            else
                Number = 1001;
            return Number;

        }

        public ConsultVM GetConsulVMById(Guid consultID)
        {
            var result = new ConsultVM();
            var appointment = DatabaseInstance.Consult.Where(w => w.Id == consultID).Select(s => s.Appointment).FirstOrDefault();
            var patient = new PatientService().GetPatientByID(appointment.PatientID);
            var clinicMen = new ClinicMenService().GetClinicMenByClinic().Where(w => w.ID == appointment.ClinicMenID).FirstOrDefault();
            var appointmentStatus = appointment.Status;
            var clinicID = GetLoggedCurrentClinic().ID;
            var consult = DatabaseInstance.Consult.Where(w => w.Id == consultID && w.Appointment.ClinicMen.ClinicID == clinicID).FirstOrDefault();

            result.PatientName = patient.Name + " " + patient.LastName1 + " " + patient.LastName2;
            result.ClinicMenName = clinicMen.Name + " " + clinicMen.LastName2 + " " + clinicMen.LastName2;
            result.Notes = consult.Notes;
            result.PatientBirthDate = patient.Birthdate;
            result.StartDate = consult.ConsultStartTime;
            result.Number = consult.Number;
            result.AppointmentNumber = appointment.Number;
            result.Notes = consult.Notes;
            result.ID = consult.Id;
            result.IsFinished = appointmentStatus == AppointmentStatusCodes.Close;

            if (GetUserFromDataContext().UserType == UserTypeCodes.AuxiliarPersonal)
                result.IsFinished = true;

            return result;

        }

        public void SaveConsultImage(byte[] imgBytes, Guid consultID)
        {
            var consult = GetConsultByID(consultID);
            var clinicID = GetLoggedCurrentClinic().ID;
            var ConsultImage = new ConsultImages()
            {
                Image = imgBytes,
                CreationDate = DateTime.Now,
                ClinicMenID = consult.Appointment.ClinicMenID,
                PatientID = consult.Appointment.PatientID,
                Id = Guid.NewGuid(),
                Description = "",
                ConsultID = consult.Id,
                ClinicID = clinicID,
            };

            DatabaseInstance.ConsultImages.Add(ConsultImage);
            DatabaseInstance.SaveChanges();

        }

        public IQueryable<ConsultImages> GetConsultImages(Guid consultID)
        {
            var clinicID = GetLoggedCurrentClinic().ID;
            return DatabaseInstance.ConsultImages.Where(w => w.ConsultID == consultID && w.ClinicID == clinicID);
        }

        public List<ConsultImageVM> GetConsultImagesByConsultID(Guid consultID)
        {
            var clinicID = GetLoggedCurrentClinic().ID;
            return GetConsultImages(consultID)
                .Select(s => new ConsultImageVM
                {
                    ID = s.Id,
                    Image = s.Image,
                    Description = s.Description,
                    CreationDate = s.CreationDate,
                }).OrderBy(o => o.CreationDate).ToList();
        }

        public void SaveConsultPatientImg(Guid consultID, byte[] img, string notes)
        {
            SaveConsultMessage(consultID, notes);
        }

        public void SaveConsultMessage(Guid ConsultID, string notes)
        {
            var consult = GetConsultByID(ConsultID);
            consult.Notes = notes;
            DatabaseInstance.SaveChanges();

        }

        public Guid OpenConsult(Guid appointmentID)
        {
            var newConsult = new Consult();
            newConsult.Id = Guid.NewGuid();
            newConsult.Amount = 0m;
            newConsult.ConsultStartTime = DateTime.Now;
            newConsult.AppointmentID = appointmentID;
            newConsult.Number = GetNewConsultNumber();
            DatabaseInstance.Consult.Add(newConsult);
            var appointment = new AppointmentService().GetAppointmentsByClinic().Where(w => w.Id == appointmentID).FirstOrDefault();
            appointment.Status = AppointmentStatusCodes.ConsultOnCourse;
            DatabaseInstance.SaveChanges();

            return newConsult.Id;
        }


        public string CloseConsult(Guid consultID, string notes, decimal amount)
        {
            var consult = GetConsultByID(consultID);
            consult.Notes = notes;
            consult.Amount = amount;
            consult.ConsultEndTime = DateTime.Now;
            var appointment = new AppointmentService().GetAppointmentsByClinic().Where(w => w.Id == consult.AppointmentID).FirstOrDefault();
            appointment.Status = AppointmentStatusCodes.Close;
            DatabaseInstance.SaveChanges();

            CreateRemissionByConsult(consultID, consult.Appointment.PatientID, consult.Appointment.ClinicMenID, consult.Appointment.ClinicID, amount, "Nota creada por consulta #" + consult.Number + " Fecha: " + DateTime.Now.ToString());

            return "";
        }

        public int GetNewRemissiontNumber()
        {
            int Number;
            if (new RemissionNotesService().GetRemissionsByLoggedClinic().Any())
                Number = new RemissionNotesService().GetRemissionsByLoggedClinic().OrderByDescending(o => o.Number).FirstOrDefault().Number + 1;
            else
                Number = 1000;
            return Number;

        }


        private void CreateRemissionByConsult(Guid consultID, Guid patientID, Guid clinicMen, Guid clinicID, decimal amount, string notes = null)
        {
            var newRemmission = new Remission();
            newRemmission.ConsultID = consultID;
            newRemmission.Notes = notes;
            newRemmission.GrandTotal = amount;
            newRemmission.VAT = 0;
            newRemmission.Id = Guid.NewGuid();
            newRemmission.PatientID = patientID;
            newRemmission.ClinicMenID = clinicMen;
            newRemmission.CreationDate = DateTime.Now;
            newRemmission.PatientID = patientID;
            newRemmission.Payed = 0m;
            newRemmission.Id = Guid.NewGuid();
            newRemmission.ExchangeRate = 1.0m;
            newRemmission.Status = 1; // Active
            newRemmission.ClinicID = clinicID;
            newRemmission.Number = GetNewRemissiontNumber();
            DatabaseInstance.Remission.Add(newRemmission);
            DatabaseInstance.SaveChanges();

        }

        public Consult GetConsultByID(Guid consultID)
        {

            return GetConsultsByClinic().Where(W => W.Id == consultID).FirstOrDefault();
        }
    }
}