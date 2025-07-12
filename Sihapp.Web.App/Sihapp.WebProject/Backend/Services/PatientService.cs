using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using Sihapp.WebProject.Backend.DataAccess;
using Sihapp.WebProject.Models;
using Sihapp.WebProject.Models.Common;
using Sihapp.WebProject.Models.Patients;
using Sihapp.WebProject.Models.Patients.Expedient;
using Sihapp.WebProject.Models.Patients.Payments;
using Sihapp.WebProject.Models.StatusCodes;
using Sihapp.WebProject.Models.TypesCatalogs;
using Sihapp.WebProject.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Sihapp.WebProject.Backend.ServiceComponents
{
    public class PatientService : BasicService
    {

        public IQueryable<Patients> GetPatientsByClinic()
        {
            var clinicID = GetLoggedCurrentClinic().ID;
            return DatabaseInstance.Patients.Where(w => w.ClinicID == clinicID);
        }
        public int GetNewPatientNumber()
        {
            int patientNumber;
            if (GetPatientsByClinic().Any())
                patientNumber = GetPatientsByClinic().OrderByDescending(o => o.Number).FirstOrDefault().Number + 1;
            else
                patientNumber = 1001;
            return patientNumber;

        }

        public void ValidateAddPatient(string name, string lastName1, string lastName2, DateTime birthDate, string address, string phone, string email, bool createUser, string userName, string password)
        {
            var result = "";
            if (string.IsNullOrWhiteSpace(name))
                result = "El nombre del paciente es obligatorio\n";

            if (string.IsNullOrWhiteSpace(lastName1))
                result = "El apellido materno del paciente es obligatorio\n";

            if (string.IsNullOrWhiteSpace(lastName2))
                result = "El apellido paterno del paciente es obligatorio\n";

            if (string.IsNullOrWhiteSpace(birthDate.ToString()))
                result = "La fecha de nacimiento del paciente es obligatoria\n";

            if (string.IsNullOrWhiteSpace(address.ToString()))
                result = "La dirección del paciente es obligatoria\n";

            if (string.IsNullOrWhiteSpace(phone))
                result = "El telefono del paciente es obligatorio\n";

            if (string.IsNullOrWhiteSpace(email))
                result = "El email del paciente es obligatorio\n";

            if (createUser)
            {
                if (string.IsNullOrWhiteSpace(userName))
                    result = "El usuario es obligatorio\n";

                if (string.IsNullOrWhiteSpace(password))
                    result = "La contraseña es obligatoria\n";

                if (userName.Contains("master"))
                    result = "No puede existir otro usuario con el nombre master";

                if (userName.Contains("+="))
                    result = "El sistema no permite estos simbolos";

                if (password.Length < 6)
                    result = "La longitud de la contraseña debe ser mayor a 6 digitos\n";
            }
            if (!string.IsNullOrEmpty(result))
                throw new Exception(result);
        }

        public string DeletePatient(Guid patientID)
        {
            var patient = GetPatientByID(patientID);
            if (patient.Appointment.Count > 0)
                return "El paciente cuenta con citas y no puede ser borrado";

            DatabaseInstance.Patients.Remove(patient);
            DatabaseInstance.SaveChanges();
            return "";
        }

        public PatientPaymentBackendDataVM GetPatientConsultBackendData()
        {
            var result = new PatientPaymentBackendDataVM();
            result.ClinicMen = new ClinicMenService().GetClinicMenByClinic().OrderBy(o => o.Number)
                .Select(s => new TextValueVM
                {
                    Text = s.Number + " - " + s.Name + " " + s.LastName1 + " " + s.LastName2,
                    Value = s.ID.ToString(),
                }).ToList();
            result.Patients = new PatientService().GetPatientsByClinic().OrderByDescending(od => od.Number)
                 .Select(s => new TextValueVM
                 {
                     Text = s.Number + " - " + s.Name + " " + s.LastName1 + " " + s.LastName2,
                     Value = s.Id.ToString(),
                 }).ToList();

            result.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            result.EndDate = DateTime.Now.AddDays(1);
            return result;
        }

        public List<TextValueVM> GetPatientsWithNoUserListTV()
        {
            var patients = GetPatientsByClinic().Where(w => w.UserID == null).OrderBy(o => o.Number)
                .Select(s => new TextValueVM
                {
                    Text = s.Number + " - " + s.Name + " " + s.LastName1 + " " + s.LastName2,
                    Value = s.Id.ToString(),
                });
            return patients.ToList();
        }

        public void AddUserPatient(Guid patientID, string userName, string password)
        {
            if (new AccountService().IsUserAlreadyExist(userName))
                throw new Exception("Ya existe un usuario con este nombre");

            var patientUser = CreatePatientUser(userName, password);
            var patient = GetPatientByID(patientID);
            patient.UserID = patientUser.Id;
            UpdateUserStatusType(patient.UserID);
            DatabaseInstance.SaveChanges();
        }

        public PageablePatientsVM GetPagedPatients(string search, PaginationVM pagination)
        {
            var pagedUsers = new PageablePatientsVM();

            var allPatients = GetPatientsByClinic();
            pagedUsers.PatientsQty = allPatients.Count();
            if (!string.IsNullOrWhiteSpace(pagination.SearchTerm))
                allPatients = allPatients.Where(w => w.Name.Contains(pagination.SearchTerm));

            var patients = allPatients.Select(s => new PatientItemVM
            {
                Name = s.Name + " " + s.LastName1 + " " + s.LastName2,
                Address = s.Address,
                Phone = s.Phone,
                BirthDate = s.Birthdate,
                HasUser = s.UserID == null ? false : true,
                ID = s.Id,
                Number = s.Number

            });

            pagedUsers.TotalPages = Math.Ceiling((decimal)allPatients.Count() / pagination.PageSize);
            pagedUsers.PatientItems = patients.OrderByDescending(o => o.Number).Skip(pagination.PageSize * pagination.Page).Take(pagination.PageSize).ToList();

            return pagedUsers;
        }

        public Patients GetPatientByID(Guid patientID)
        {
            return GetPatientsByClinic().Where(w => w.Id == patientID).FirstOrDefault();
        }

        public string AddPatient(string name, string lastName1, string lastName2, DateTime birthDate, string weight, string heigth, string address, string phone, string email, bool createUser = false, string userName = "", string password = "", List<CustomFieldsVM> customFields = null)
        {

            if (createUser)
            {
                if (new AccountService().IsUserAlreadyExist(userName))
                    throw new Exception("Ya existe un usuario con este nombre");
            }

            var clinicID = GetLoggedCurrentClinic().ID;
            var newPatient = new Patients();
            newPatient.Id = Guid.NewGuid();
            newPatient.Name = name;
            newPatient.LastName1 = lastName1;
            newPatient.LastName2 = lastName2;
            newPatient.Birthdate = birthDate;
            newPatient.Weight = weight;
            newPatient.Height = heigth;
            newPatient.Address = address;
            newPatient.Phone = phone;
            newPatient.Email = email;
            newPatient.ClinicID = clinicID;
            newPatient.Number = GetNewPatientNumber();

            if (createUser)
            {

                var patientUser = CreatePatientUser(userName, password);
                newPatient.UserID = patientUser.Id;

                UpdateUserStatusType(patientUser.Id);

            }

            DatabaseInstance.Patients.Add(newPatient);
            DatabaseInstance.SaveChanges();

            if (customFields != null && customFields.Count > 0)
            {

                new CustomFieldsService().AddCustomFieldsToEntity(newPatient.Id, CustomFieldEntityTypesCodes.PatientEntity, customFields);

            }
            return "";

        }

      

        public MemoryStream GetpatientHistoricalByIDAndDate(Guid id)
        {
            var patient = GetPatientByID(id);

            MemoryStream workStream = new MemoryStream();
            Document doc = new Document();
            PdfWriter.GetInstance(doc, workStream).CloseStream = false;

            doc.AddTitle("Expediente");
            doc.AddCreator("Sihapp team");

            doc.Open();
            iTextSharp.text.Font smallFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            new PdfDocumentsService().InsertExpedientHeader(patient, doc, smallFont);
            var appointments = patient.Appointment.AsQueryable();
            var consultCount = appointments.Where(w => w.Status == AppointmentStatusCodes.Close).Count();
            var consults = appointments.Where(w => w.Status == AppointmentStatusCodes.Close).SelectMany(sm => sm.Consult);
            var lastConsult = consults.Count() > 0 ? consults.OrderByDescending(o => o.ConsultEndTime).FirstOrDefault().ConsultStartTime.ToString() : "NA";


            doc.Add(new Paragraph("Total de consultas: " + consultCount));
            doc.Add(new Paragraph("Ultima consulta: " + lastConsult));

            if (consults.Count() > 0)
            {

                doc.Add(Chunk.NEWLINE);
                var ordenedConsults = consults.OrderByDescending(o => o.ConsultStartTime).ToList();
                foreach (var c in ordenedConsults)
                {
                    doc.Add(new Paragraph("___________________________________________________________________________"));

                    var clinicMen = c.Appointment.ClinicMen;
                    var clinicMenName = clinicMen.Name + " " + clinicMen.LastName1 + " " + clinicMen.LastName2;
                    var startDate = c.ConsultStartTime;
                    var endDate = c.ConsultEndTime;

                    doc.Add(new Paragraph("Consulta # " + c.Number + " a cargo de: " + clinicMenName));
                    doc.Add(Chunk.NEWLINE);
                    doc.Add(new Paragraph("Comienzo " + startDate +
                        "                 Final:  " + endDate));
                    doc.Add(new Paragraph("Notas: " + c.Notes));
                    doc.Add(Chunk.NEWLINE);

                    var hasConsultImages = c.ConsultImages.Count() > 0;

                    var consultImages = new ConsultService().GetConsultImagesByConsultID(c.Id);

                    foreach (var ci in consultImages)
                    {
                        byte[] imageBytes = ci.Image;
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);

                        image.ScaleAbsolute(259f, 259f);
                        PdfPTable table = new PdfPTable(1);
                        //table.DefaultCell.Border = 0;

                        var imageCell = new PdfPCell(image);
                        imageCell.Border = 0;
                        table.AddCell(imageCell);

                        table.DefaultCell.Border = 0;
                        doc.Add(table);
                        table.DefaultCell.Border = 0;

                        doc.Add(Chunk.NEWLINE);
                    }
                }
            }

            doc.Close();
            return workStream;
        }

        public double DiffYears(DateTime start, DateTime end)
        {
            // Get difference in total months.
            int months = ((end.Year - start.Year) * 12) + (end.Month - start.Month);

            // substract 1 month if end month is not completed
            if (end.Day < start.Day)
            {
                months--;
            }

            double totalyears = months / 12d;
            return totalyears;
        }

        public ApplicationUser CreatePatientUser(string userName, string password)
        {
            var newUser = new ApplicationUser { UserName = userName };
            var createTask = UserManager.Create(newUser, password);
            //createTask.Wait();
            return newUser;
        }

        public void UpdateUserStatusType(string aspNetUserID)
        {
            var aspnetuser = DatabaseInstance.AspNetUsers.FirstOrDefault(f => f.Id == aspNetUserID);
            aspnetuser.UserType = UserTypeCodes.Patient; // 1 = patient;
            aspnetuser.Admin = false;
            aspnetuser.ClinicID = Guid.Empty;
            aspnetuser.ClinicNumber = GetLoggedClinicNumber();
            DatabaseInstance.SaveChanges();
        }

        public PatientExpedientViewBagVM GetPatientExpedientViewBagVM(Guid id)
        {
            var loggedClinicMenID = new ClinicMenService().GetCurrentClinicMen().ID;
            var patient = GetPatientByID(id);
            if (patient == null)
                throw new Exception(@"El paciente que estas intentando visualizar pertenece a otra clínica
                    o no existe; favor de no copiar y pegar vinculos");
            var pi = new PatientExpedientViewBagVM();
            pi.Name = patient.Name;
            pi.LastName1 = patient.LastName1;
            pi.LastName2 = patient.LastName2;
            pi.Phone = patient.Phone;

            var appointments = patient.Appointment.Where(w => w.Status == AppointmentStatusCodes.Close);

            if (appointments.Count() > 0)
                pi.GeneralConsultQty = appointments.Select(s => s.Consult).Count();
            else
                pi.GeneralConsultQty = 0;

            var myAssignedAppointments = appointments.Where(w => w.ClinicMenID == loggedClinicMenID);
            if (myAssignedAppointments.Count() > 0)
                pi.ConsultQty = appointments.Select(s => s.Consult).Count();
            else
                pi.ConsultQty = 0;

            pi.Age = new UtilService().GetAge(patient.Birthdate);
            pi.Birthdate = patient.Birthdate;
            pi.Number = patient.Number;
            pi.Notes = patient.Notes;
            var today = DateTime.Now;
            pi.StartDateFilter = new DateTime(today.Year, today.Month, 1);
            pi.EndDateFilter = pi.StartDateFilter.AddMonths(1).AddDays(-1);
            pi.ID = patient.Id;
            return pi;

        }

        public List<TextValueVM> GetPatientsByKeywords(string keyword)
        {
            int number = -1;

            int.TryParse(keyword, out number);

            if (number > 0)
                return GetPatientsByClinic().OrderByDescending(od => od.Number).Where(w => w.Number == number)
                   .Select(s => new TextValueVM()
                   {
                       Text = s.Number + " - " + s.Name + " " + s.LastName1 + " " + s.LastName2,
                       Value = s.Id.ToString()
                   }).ToList();

            if (string.IsNullOrWhiteSpace(keyword))
                return GetPatientsByClinic().OrderByDescending(od => od.Number)
                    .Select(s => new TextValueVM()
                    {
                        Text = s.Number + " - " + s.Name + " " + s.LastName1 + " " + s.LastName2,
                        Value = s.Id.ToString()
                    }).ToList();
            else
                return GetPatientsByClinic().OrderByDescending(od => od.Number).Where(w => w.Name.Contains(keyword)
                || w.LastName1.Contains(keyword) || w.LastName2.Contains(keyword))
                            .Select(s => new TextValueVM()
                            {
                                Text = s.Number + " - " + s.Name + " " + s.LastName1 + " " + s.LastName2,
                                Value = s.Id.ToString()
                            }).ToList();

        }
    }
}