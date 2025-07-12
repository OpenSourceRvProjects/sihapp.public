using Sihapp.WebProject.Backend.ServiceComponents;
using Sihapp.WebProject.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sihapp.WebProject.Controllers
{
    public class AppointmentController : BasicController
    {
        // GET: Appointment
        public ActionResult AppointmentsList(int? option)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userType = new AppointmentService().GetAppointmentBackendData(option);
            ViewBag.backendData = SerializeJSONData(userType);
            return View();
        }

        [HttpPost]
        public ActionResult AddAppointment(Guid patientID, bool anotherClinicMen, Guid? clinicMenID, DateTime newAppointmentDate, DateTime newAppointmentTime)
        {
            var responseMessage = new AppointmentService().AddAppointment(patientID, anotherClinicMen, clinicMenID, newAppointmentDate, newAppointmentTime);
            return GetJsonNetResult(responseMessage);
        }

        [HttpPost]
        public ActionResult GetPagedAppointments(DateTime? startDate, DateTime? endDate, Guid? patientID)
        {
            var pagedUsers = new AppointmentService().GetPagedAppointments(startDate, endDate, patientID);
            return GetJsonNetResult(pagedUsers);
        }


        [HttpPost]
        public ActionResult SendAppointmentReminder(Guid appointmentID) {

            new AppointmentService().SendAppointmentReminder(appointmentID);
            return GetJsonNetResult(null);
        }

    }
}