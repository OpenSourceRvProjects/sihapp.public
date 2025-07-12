using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.RemissionNotes
{

    [TsClass]
    public class PagedRemissionNotes
    {
        public PagedRemissionNotes()
        {
            RemissionList = new List<RemissionNotesItems>();
        }
        public List<RemissionNotesItems> RemissionList { get; set; }
        public decimal TotalPages { get; set; }
        public decimal TotalGrandAmount { get; set; }
        public decimal TotalPending { get; set; }
        public decimal TotalPayed { get; set; }
    }

    [TsClass]
    public class RemissionNotesItems
    {
        public string PatientName { get; set; }
        public string ClinicMen { get; set; }
        public DateTime CreationDate { get; set; }
        public string ConsultNumber { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal Payed { get; set; }
        public decimal Pending { get; set; }
        public int Number { get; internal set; }
        public Guid ID { get;  set; }
    }
}