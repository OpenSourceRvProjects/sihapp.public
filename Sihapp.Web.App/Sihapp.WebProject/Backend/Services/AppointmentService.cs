using Sihapp.WebProject.Backend.DataAccess;
using Sihapp.WebProject.DTOs;
using Sihapp.WebProject.Models.Appointments;
using Sihapp.WebProject.Models.Common;
using Sihapp.WebProject.Models.StatusCodes;
using Sihapp.WebProject.Models.TypesCatalogs;
using Sihapp.WebProject.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;

namespace Sihapp.WebProject.Backend.ServiceComponents
{
    public class AppointmentService : BasicService
    {
        public AppointmentBackendDataVM GetAppointmentBackendData(int? option)
        {
            var result = new AppointmentBackendDataVM();
            result.UserType = GetLoggedUserType();
            result.ClinicMen = new ClinicMenService().GetClinicMenByClinic().OrderBy(o=> o.Number)
                .Select(s => new TextValueVM
                {
                    Text = s.Number + " - " + s.Name + " " + s.LastName1 + " " + s.LastName2,
                    Value = s.ID.ToString(),
                }).ToList();
            result.PatientList = new PatientService().GetPatientsByClinic().OrderByDescending(od => od.Number)
                 .Select(s => new TextValueVM
                 {
                     Text = s.Number + " - " + s.Name + " " + s.LastName1 + " " + s.LastName2,
                     Value = s.Id.ToString(),
                 }).ToList();
            result.AppointmentStatusCodes = AppointmentStatusCodes.GetAllCodes()
                .Select(s => new TextValueVM
                 {
                     Text = s.Text,
                     Value = int.Parse(s.Value.ToString()),
                 }).ToList();
            result.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1);
            result.EndDate =  DateTime.Now.AddDays(1);
            result.Option = option;
            //new DateTime(12, 01, 2017, 11, 12, 14);
            return result;
        }

        public void SendAppointmentReminder(Guid appointmentID)
        {
            var appointment = GetAppointmentByID(appointmentID);
            var patient = new PatientService().GetPatientByID(appointment.PatientID);
            var clinicMen = new ClinicMenService().GetClinicMenByID(appointment.ClinicMenID);

            var reminderMailDTO = new ReminderMailDTO() {
                AppointmentDate = appointment.AppointmentDate,
                PatientEmail = patient.Email,
                PatientName = patient.Name + " " + patient.LastName1,
                Address = appointment.Address,
                ClinicMenEmail = clinicMen.Email,
                ClinicMenName = clinicMen.Name + " " + clinicMen.LastName1,
            };

            List<string> mails = new List<string>();
            mails.Add(reminderMailDTO.PatientEmail);
            mails.Add(reminderMailDTO.ClinicMenEmail);

            var body = GetReminderEmailHTMLBody(reminderMailDTO);

            new UtilService().SendEmail(mails, "Recordatorio de consulta con " + reminderMailDTO.ClinicMenName, body);
        }

        private string GetReminderEmailHTMLBody(ReminderMailDTO reminderMailDTO)
        {
            string body = "";
            body += "Hola " + reminderMailDTO.PatientName + ", Sihapp por medio de tu profesional de salud " + reminderMailDTO.ClinicMenName
                + ", te recuerda tu cita programada en la siguiente fecha, hora y lugar";
            body += "<br />";
            body += "<p>Fecha: " + new UtilService().GetDayNameByDayNumber((int) reminderMailDTO.AppointmentDate.DayOfWeek)  + " " + reminderMailDTO.AppointmentDate.Day
                + " de " + new UtilService().GetMonthNameByNumber(reminderMailDTO.AppointmentDate.Month) + " de " + reminderMailDTO.AppointmentDate.Year + "</p>";
            body += "<p>Hora: " + reminderMailDTO.AppointmentDate.ToString("HH:mm") + "</p>";
            body += "<p>Lugar: " + reminderMailDTO.Address+ "</p>";

            body += "<h5>Este mensaje ha sido enviado de manera automática, favor de no responder este correo</h5>";

            return body;

        }

        public Appointment GetAppointmentByID(Guid id) {
            var clinicID = GetLoggedCurrentClinic().ID;
            return DatabaseInstance.Appointment.Where(w => w.ClinicID == clinicID && w.Id == id).FirstOrDefault();

        }

        public IQueryable<Appointment> GetAppointmentsByClinic()
        {
            var clinicID = GetLoggedCurrentClinic().ID;
            return DatabaseInstance.Appointment.Where(w => w.ClinicID == clinicID);
        }

        public PageableAppointmentsVM GetPagedAppointments(DateTime? startDate, DateTime? endDate, Guid? patientID)
        {

            var result = new PageableAppointmentsVM();
            result.Groups = new List<GroupedAppointmentVM>();
            var allAppointments = GetAppointmentsByClinic();

            if (patientID.HasValue)
            {
                allAppointments = allAppointments.Where(w => w.PatientID == patientID);
            }

            //if (startDate == null)
            //    startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (startDate != null)
            allAppointments = allAppointments.Where(w => w.AppointmentDate >= startDate);

            if (endDate != null)
            {

                allAppointments = allAppointments.Where(w => w.AppointmentDate <= endDate);
            }

            var userType = GetUserFromDataContext().UserType;

            switch (userType) {
                //case UserTypeCodes.ClinicMen:
                //    var currentClinicMenID = GetCurrentClinicMen().ID;
                //    allAppointments = allAppointments.Where(w => w.ClinicMenID == currentClinicMenID);
                //    break;

                case UserTypeCodes.Patient:
                    var currentPatientID = GetCurrentPatient().Id;
                    allAppointments = allAppointments.Where(w => w.PatientID == currentPatientID);
                    break;

            }

            var groupedAppointments = allAppointments.GroupBy(g => new { g.AppointmentDate.Year, g.AppointmentDate.Month, g.AppointmentDate.Day });
            var clinicManID = GetCurrentClinicMen()?.ID;
            foreach (var group in groupedAppointments)
            {
                var aux = new GroupedAppointmentVM();
                aux.Date = new DateTime(group.Key.Year, group.Key.Month, group.Key.Day);
                aux.Items = allAppointments.Where(W => W.AppointmentDate.Year == aux.Date.Year && W.AppointmentDate.Month == aux.Date.Month && W.AppointmentDate.Day == aux.Date.Day)
                    .Select(s => new AppointmentItemsVM
                    {
                        AppointmentID = s.Id,
                        ClinicMenName = s.ClinicMen.Name + " " + s.ClinicMen.LastName1 + " " + s.ClinicMen.LastName2,
                        PatientName = s.Patients.Name + " " + s.Patients.LastName1 + " " + s.Patients.LastName2,
                        AppointmentDate = s.AppointmentDate,
                        Address = s.Address,
                        Status = s.Status,
                        Number = s.Number,
                        CanStartConsult = userType == UserTypeCodes.AuxiliarPersonal || (clinicManID.HasValue && s.ClinicMenID == clinicManID), 
                        HasActiveConsult = s.Consult.Count() > 0 ? true : false,
                        ConsultID = s.Consult.Where(w=> w.AppointmentID == s.Id).Select(sel=> (Guid?) sel.Id).FirstOrDefault(),
                    }).OrderBy(o => o.AppointmentDate).ToList();
                result.Groups.Add(aux);

            }

            return result;

        }

        public int GetNewAppointmentNumber()
        {
            int Number;
            if (GetAppointmentsByClinic().Any())
                Number = GetAppointmentsByClinic().OrderByDescending(o => o.Number).FirstOrDefault().Number + 1;
            else
                Number = 1001;
            return Number;

        }

        public string AddAppointment(Guid patientID, bool anotherClinicMen, Guid? clinicMenID, DateTime newAppointmentDate, DateTime newAppointmentTime)
        {
            var newAppointment = new Appointment();
            if (GetLoggedUserType() == 0)
            {
               
                newAppointment.CreatedClinicMen = GetCurrentClinicMen().ID;
                if (anotherClinicMen)
                {
                    newAppointment.ClinicMenID = new ClinicMenService().GetClinicMenByClinic().FirstOrDefault(w => w.ID == clinicMenID.Value).ID;
                }
                else
                {
                    newAppointment.ClinicMenID = GetCurrentClinicMen().ID;
                }
            }

            if (GetLoggedUserType() == 2)
            {
                newAppointment.CreatedAuxiliar = GetCurrentAuxiliarPersonal().Id;
                newAppointment.ClinicMenID = new ClinicMenService().GetClinicMenByClinic().FirstOrDefault(w => w.ID == clinicMenID.Value).ID;
            }

            newAppointment.AppointmentDate = new DateTime(newAppointmentDate.Year, newAppointmentDate.Month, newAppointmentDate.Day, newAppointmentTime.Hour, newAppointmentTime.Minute, 0);

            var clinicMen = new ClinicMenService().GetClinicMenByID(newAppointment.ClinicMenID);
            var minDuration = clinicMen.ConsultDuration.HasValue ? clinicMen.ConsultDuration.Value : 5;

            var existAnotherAppointment = GetAppointmentsByClinic().Select(s => new { s.AppointmentDate, s.ClinicMenID })
                .ToList().Where(w => newAppointment.AppointmentDate <= w.AppointmentDate.AddMinutes(minDuration) && newAppointment.AppointmentDate >= w.AppointmentDate.AddMinutes(-minDuration) && w.ClinicMenID == newAppointment.ClinicMenID).FirstOrDefault();
            //if (GetAppointmentsByClinic().Select(s=> new { s.AppointmentDate, s.ClinicMenID }).ToList().Any(a => a.ClinicMenID == newAppointment.ClinicMenID &&  (newAppointment.AppointmentDate <= a.AppointmentDate.AddMinutes(5) && newAppointment.AppointmentDate >= a.AppointmentDate.AddMinutes(-5)/* && a.AppointmentDate == newAppointment.AppointmentDate*/)))

            if (existAnotherAppointment != null)
                return "Ya existe una cita con este medico a esta hora";

            var existAnotherPatientAppointment = GetAppointmentsByClinic().Select(s => new { s.AppointmentDate, s.PatientID })
               .ToList().Where(w => newAppointment.AppointmentDate <= w.AppointmentDate.AddMinutes(minDuration) && newAppointment.AppointmentDate >= w.AppointmentDate.AddMinutes(-minDuration) && w.PatientID == patientID).FirstOrDefault();


            if (existAnotherPatientAppointment != null)
                return "Ya existe una cita con este paciente a esta hora";


            newAppointment.CreationDate = DateTime.Now;
            newAppointment.Id = Guid.NewGuid();
            newAppointment.Status = 0; //Por confirmar con paciente;
            newAppointment.Number = GetNewAppointmentNumber();
            newAppointment.Type = 0; //Ordinaria
            newAppointment.Comments = "";
            newAppointment.PatientID = patientID;
            newAppointment.Address = "Consultorio";
            newAppointment.ClinicID = GetLoggedCurrentClinic().ID;
            DatabaseInstance.Appointment.Add(newAppointment);
            DatabaseInstance.SaveChanges();

            return "";
        }
    }
}