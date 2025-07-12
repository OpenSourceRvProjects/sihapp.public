using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.Patients
{
    [TsClass]
    public class PageablePatientsVM
    {
        public PageablePatientsVM()
        {
            PatientItems = new List<PatientItemVM>();
        }
        public decimal TotalPages { get; set; }
        public List<PatientItemVM> PatientItems { get; set; }
        public int PatientsQty { get; set; }
    }

    [TsClass]
    public class PatientItemVM
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public bool HasUser { get; set; }
        public Guid ID { get;  set; }
        public int Number { get; internal set; }
    }
}