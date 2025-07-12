using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.Patients.Payments
{
    [TsClass]
    public class PatientPaymentsVM
    {
        public string PatientName { get; set; }
        public string ConsultNumber { get; set; }
        public string ClinicMen { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Payed { get; set; }
        public decimal Pending
        {
            get
            {
                return GrandTotal - Payed;
            }
        }
    }

    public class PagedPatientPaymentsVM {

        public List<PatientPaymentsVM> PatientPaymentList { get; set; }
        public double Pages { get; set; }
        public decimal TotalGrandTotals { get; set; }
        public decimal TotalPending { get; set; }
        public decimal TotalPayed { get; set; }
    }
}