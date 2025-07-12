using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.ClinicMen
{

    [TsClass]
    public class ClinicMenSettingsVM
    {
        public string RFC { get; set; }
        public string Cellphone { get; set; }
        public int? ToleranceMinutes { get; set; }
        public DateTime Birthdate { get; set; }
    }
}