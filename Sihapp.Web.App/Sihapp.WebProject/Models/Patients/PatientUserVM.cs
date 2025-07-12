using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sihapp.WebProject.Models.Patients
{
    public class PatientUserVM
    {
        public Guid PatientID { get; set; }
        public string AspNetUserID { get; set; }
    }
}