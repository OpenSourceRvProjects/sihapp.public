using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models
{
    [TsClass]
    public class ConsultVM
    {
        public string PatientName { get; set; }
        public string ClinicMenName { get; set; }
        public string Notes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime PatientBirthDate { get;  set; }
        public int Number { get;  set; }
        public int AppointmentNumber { get;  set; }
        public Guid ID { get;  set; }
        public bool IsFinished { get;  set; }
    }
}