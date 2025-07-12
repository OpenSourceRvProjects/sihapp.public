using Sihapp.WebProject.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sihapp.WebProject.Models.StatusCodes
{
    public class AppointmentStatusCodes
    {
        public const int StantByPatientConfirm = 0;
        public const int Pospossed = 1;
        public const int ConfirmedByPatient = 2;
        public const int Canceled = 3;
        public const int ConsultOnCourse = 4;
        public const int Close = 5;


        public static List<TextValueVM> GetAllCodes()
        {
                var result = new List<TextValueVM>();
                result.Add(new TextValueVM { Text = "Por confirmar", Value = StantByPatientConfirm });
                result.Add(new TextValueVM { Text = "Pospuesto", Value = Pospossed });
                result.Add(new TextValueVM { Text = "Confirmado con paciente", Value = ConfirmedByPatient });
                result.Add(new TextValueVM { Text = "Cancelado", Value = Canceled });
                result.Add(new TextValueVM { Text = "Consulta en progreso", Value = ConsultOnCourse });
                result.Add(new TextValueVM { Text = "Terminado", Value = Close });

                return result;
        }
    }


}