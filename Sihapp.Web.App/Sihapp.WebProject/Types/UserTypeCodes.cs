using Sihapp.WebProject.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sihapp.WebProject.Types
{
    public class UserTypeCodes
    {
        public const int ClinicMen = 0;
        public const int Patient = 1;
        public const int AuxiliarPersonal = 2;

        public static List<TextValueVM> AllTypeCodes() {
            var result = new List<TextValueVM>();
            result.Add(new TextValueVM { Text = "Profesional de salud", Value = ClinicMen });
            result.Add(new TextValueVM { Text = "Paciente", Value = Patient });
            result.Add(new TextValueVM { Text = "Personal auxiliar", Value = AuxiliarPersonal });
            return result;
        } 
    }
}