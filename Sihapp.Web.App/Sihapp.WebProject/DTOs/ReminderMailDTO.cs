using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sihapp.WebProject.DTOs
{
    public class ReminderMailDTO
    {
        public string PatientName { get; set; }
        public string ClinicMenName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentHour { get; set; }
        public string PatientEmail { get; set; }
        public string ClinicMenEmail { get; set; }
        public string Address { get; internal set; }
    }
}