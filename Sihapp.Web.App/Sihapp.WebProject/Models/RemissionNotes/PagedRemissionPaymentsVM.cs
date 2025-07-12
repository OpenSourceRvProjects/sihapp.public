using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.RemissionNotes
{
    [TsClass]
    public class PagedRemissionPaymentsVM
    {
        public RemissionNotesItems PaymentItems { get; set; }
        public decimal TotalPayed { get; set; }


    }

    [TsClass]
    public class RemissionPaymentItem
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Comments { get; set; }
    }
}