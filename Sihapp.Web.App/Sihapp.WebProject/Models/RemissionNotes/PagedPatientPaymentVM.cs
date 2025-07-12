using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.RemissionNotes
{
    [TsClass]
    public class PagedPatientPaymentVM
    {
        public PagedPatientPaymentVM()
        {
            PaymentItems = new List<PatientPaymentItemVM>();
        }
        public decimal GrandTotal { get; set; }
        public List<PatientPaymentItemVM> PaymentItems { get; set; }
        public decimal TotalPages { get; set; }
        public decimal TotalPayed { get; set; }
    }

    [TsClass]
    public class PatientPaymentItemVM
    {
        public Guid PaymentID { get; set; }
        public string ApplicationDate { get; set; }
        public decimal PaymentAmount  { get; set; }
        public decimal LastBalance { get; set; }
        public string Notes { get; set; }

    }



}