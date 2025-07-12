using iTextSharp.text;
using iTextSharp.text.pdf;
using Sihapp.WebProject.Backend;
using Sihapp.WebProject.Backend.ServiceComponents;
using Sihapp.WebProject.Controllers.Filters;
using Sihapp.WebProject.Models;
using Sihapp.WebProject.Models.Common;
using Sihapp.WebProject.Models.RemissionNotes;
using Sihapp.WebProject.Models.TypesCatalogs;
using Sihapp.WebProject.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Sihapp.WebProject.Controllers
{
    public class PatientsController : BasicController
    {
        // GET: Patients

        [VerifyIdentity]
        public ActionResult PatientsList()
        {
            if (!User.Identity.IsAuthenticated || GetUserFromDataContext().UserType == Models.TypesCatalogs.UserTypeCodes.Patient)
                return RedirectToAction("Login", "Account");
            ViewBag.CustomFieldsTypes = SerializeJSONData(new CustomFieldTypesCatalog().GetAllCustomFieldTypes());
            return View();
        }

        [HttpPost]
        [HandleException]
        public ActionResult AddPatient(string name, string lastName1, string lastName2, DateTime birthDate, string weight, string heigth, string address, string phone, string email, bool createUser = false, string userName = "", string password = "", List<CustomFieldsVM>  customFields = null)
        {

            //throw new Exception("Exception test");
            new PatientService().ValidateAddPatient(name, lastName1, lastName2, birthDate, address, phone, email, createUser, userName, password);

            Func<string> add = () =>
            {
                return new PatientService().AddPatient(name, lastName1, lastName2, birthDate, weight, heigth, address, phone, email, createUser, userName, password, customFields);
            };

            new UtilService().RunTransaction(add);

            return GetJsonNetResult(null);
        }

        [HttpGet]
        [HandleException]
        public ActionResult GetPatientCustomFieldsByClinic()
        {
            var presetFields = new CustomFieldsService().GetPatientCustomFieldsByClinic();
            return GetJsonNetResult(presetFields);
        }

        [HttpPost]
        [HandleException]
        public ActionResult AddUserPatient(Guid patientID, string userName, string password)
        {
            if (new AccountService().IsUserAlreadyExist(userName))
                throw new Exception("Ya existe un usuario con este nombre");

            new PatientService().AddUserPatient(patientID, userName, password);
            return GetJsonNetResult(null);
        }


        public ActionResult PatientRemissionDetails(Guid remissionID)
        {
            if (!User.Identity.IsAuthenticated || GetUserFromDataContext().UserType == Models.TypesCatalogs.UserTypeCodes.Patient)
                return RedirectToAction("Login", "Account");

            ViewBag.backendData = SerializeJSONData(new RemissionNotesService().GetRemissonInfoByID(remissionID));
            return View();
        }



        [HttpPost]
        public ActionResult GetPagedPatients(string search, PaginationVM pagination)
        {
            var pagedUsers = new PatientService().GetPagedPatients(search, pagination);
            return GetJsonNetResult(pagedUsers);
        }


        [HttpPost]
        public ActionResult DeletePatient(Guid patientID)
        {
            var msg = new PatientService().DeletePatient(patientID);
            return GetJsonNetResult(msg);
        }

        #region

        public ActionResult PatientExpedientByID(Guid id)
        {
            var patient = new PatientService().GetPatientExpedientViewBagVM(id);
            ViewBag.backendData = SerializeJSONData(patient);
            return View();
        }
        #endregion

        [HttpPost]
        public ActionResult GetPatientsByKeywords(string keyword)
        {
            var patients = new PatientService().GetPatientsByKeywords(keyword);
            return GetJsonNetResult(patients);
        }


        public ActionResult PatientConsultPayments()
        {
            if (!User.Identity.IsAuthenticated || GetUserFromDataContext().UserType == Models.TypesCatalogs.UserTypeCodes.Patient)
                return RedirectToAction("Login", "Account");

            ViewBag.backendData = SerializeJSONData(new PatientService().GetPatientConsultBackendData());
            return View();

        }

        public void pdf(Guid id)
        {
            var workstream = new PatientService().GetpatientHistoricalByIDAndDate(id);
            var patient = new PatientService().GetPatientByID(id);
            var patientName = patient.Name + " " + patient.LastName1 + " " + patient.LastName2;
            var clinicName = GetClinicName();
            var normalizedClinicName = Regex.Replace(clinicName, @"[^a-zA-z0-9 ]+", "");
            string fileName = patientName + " " + DateTime.Now.ToString() + " -" + normalizedClinicName + ".pdf";
            byte[] byteInfo = workstream.ToArray();
            workstream.Write(byteInfo, 0, byteInfo.Length);
            workstream.Position = 0;
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment; filename= " + Server.HtmlEncode(fileName));
            Response.ContentType = "APPLICATION/pdf";
            Response.BinaryWrite(byteInfo);
        }

        public void PDFNotes(Guid id)
        {

            var workStream = new RemissionNotesService().GetPDFRemissionNote(id);
            var remission = new RemissionNotesService().GetRemissionsByLoggedClinic().FirstOrDefault(f => f.Id == id);
            var patient = remission.Consult.Appointment.Patients;
            var patientName = patient.Name + " " + patient.LastName1 + " " + patient.LastName2;
            string fileName = "Nota " + remission.Number + " -" + patientName + ".pdf";
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment; filename= " + Server.HtmlEncode(fileName));
            Response.ContentType = "APPLICATION/pdf";
            Response.BinaryWrite(byteInfo);
        }

        public void PDFPaymentReceipt(Guid id)
        {
            var workStream = new RemissionNotesService().GetPDFPayment(id);

            var remissionPayment = new RemissionNotesService().GetRemissionPaymentByID(id);
            var patient = remissionPayment.Remission.Consult.Appointment.Patients;
            var patientName = patient.Name + " " + patient.LastName1 + " " + patient.LastName2;
            string fileName = "Pago de Nota " + remissionPayment.Remission.Number + " -" + patientName + ".pdf";
            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;
            Response.Buffer = true;
            Response.AddHeader("Content-Disposition", "attachment; filename= " + Server.HtmlEncode(fileName));
            Response.ContentType = "APPLICATION/pdf";
            Response.BinaryWrite(byteInfo);
        }


        [HttpPost]
        [HandleException]
        public ActionResult GetPagedRemisionNotes(PaginationVM pagination, DateTime? startDate, DateTime? endDate, Guid? patientID)
        {
            var pagedRemissions = new RemissionNotesService().GetPagedRemisionNotes(pagination, startDate, endDate, patientID);
            return GetJsonNetResult(pagedRemissions);
        }

        [HttpPost]
        [HandleException]
        public ActionResult GetPagedPatientPayments(PaginationVM pagination, Guid remissionID)
        {
            var pagedPatientPayments = new RemissionNotesService().GetPagedPatientPayments(pagination, remissionID);
            return GetJsonNetResult(pagedPatientPayments);
        }

        [HttpPost]
        [HandleException]
        public ActionResult DeletePatientPayment(Guid RemissionPaymentID)
        {
            Action deletePayment = () =>
            {
                new RemissionNotesService().DeletePatientPayment(RemissionPaymentID);
            };

            new UtilService().RunTransaction(deletePayment);
            return GetJsonNetResult(null);
        }


        [HttpPost]
        [HandleException]
        public ActionResult AddPatientPayment(RemissionNotesItems remission, string comments, DateTime date)
        {

            Action addPayment = () =>
            {
                new RemissionNotesService().AddPatientPayment(remission, comments, date);
            };

            new UtilService().RunTransaction(addPayment);
            return GetJsonNetResult(null);

        }


    }
}