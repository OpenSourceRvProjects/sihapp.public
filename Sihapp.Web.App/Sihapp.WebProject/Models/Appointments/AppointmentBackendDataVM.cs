using Sihapp.WebProject.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.Appointments
{

    [TsClass]
    public class AppointmentBackendDataVM
    {
        public int UserType { get; set; }
        public List<TextValueVM> PatientList { get; set; }
        public List<TextValueVM> ClinicMen { get; set; }
        public List<TextValueVM> AppointmentStatusCodes { get;  set; }
        public DateTime StartDate { get; internal set; }
        public DateTime EndDate { get; internal set; }
        public int? Option { get; set; }
    }
}