using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.Patients.Expedient
{

    [TsClass]
    public class PatientExpedientViewBagVM
    {
        public string Name { get; set; }
        public string LastName1 { get; set; }
        public string LastName2 { get; set; }
        public int Age { get; set; }
        public DateTime Birthdate { get; set; }
        public string Phone { get; set; }
        public int ConsultQty { get; set; }
        public int GeneralConsultQty { get; set; }
        public int Number { get; internal set; }
        public string Notes { get; set; }
        public DateTime StartDateFilter { get; internal set; }
        public DateTime EndDateFilter { get; internal set; }
        public Guid ID { get; internal set; }
    }
}