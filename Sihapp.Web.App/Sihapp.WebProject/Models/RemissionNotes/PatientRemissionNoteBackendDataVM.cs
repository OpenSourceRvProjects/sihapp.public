using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.RemissionNotes
{
    [TsClass]
    public class PatientRemissionNoteBackendDataVM
    {
        public string PatientName { get; set; }
        public string ClinicManName { get; set; }
        public Guid RemissionID { get; set; }
        public long RemissionNumber  { get; set; }
        public DateTime CreationDate { get; set; }
        public string ConsultNumber { get; set; }
        public decimal GrandTotal { get; internal set; }
        public decimal Payed { get; internal set; }
        public decimal Pending { get; internal set; }
        public string Notes { get; internal set; }
    }
}