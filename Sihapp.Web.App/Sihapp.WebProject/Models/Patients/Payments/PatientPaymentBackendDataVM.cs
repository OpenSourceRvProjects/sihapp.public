using Sihapp.WebProject.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.Patients.Payments
{

    [TsClass]
    public class PatientPaymentBackendDataVM
    {
        public List<TextValueVM> Patients { get; set; }
        public List<TextValueVM> ClinicMen { get; set; }
        public string Keyword { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid RemissionID { get; set; }
    }
}