using Sihapp.WebProject.Models.Common;
using Sihapp.WebProject.Models.StatusCodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.Appointments
{
    [TsClass]
    public class PageableAppointmentsVM
    {
        public List<GroupedAppointmentVM> Groups { get; set; }
    }

    [TsClass]
    public class GroupedAppointmentVM
    {
        public List<AppointmentItemsVM> Items { get; set; }
        public DateTime Date { get; set; }
    }

    [TsClass]
    public class AppointmentItemsVM
    {

        public string ClinicMenName { get; set; }
        public string PatientName { get; set; }
        public string Address { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Guid AppointmentID { get; set; }
        public int Status { get; set; }
        public int Number { get; set; }
        public bool HasActiveConsult { get; set; }
        public bool CanStartConsult { get; set; }
        public TextValueVM SelectedStatus
        {
            get
            {
                var codes = AppointmentStatusCodes.GetAllCodes();
                var text = codes.Where(w => w.Value.ToString() == this.Status.ToString()).FirstOrDefault().Text;
                var value = codes.Where(w => w.Value.ToString() == this.Status.ToString()).FirstOrDefault().Value;
                return new TextValueVM() { Text = text, Value = value };

            }
           
        }

        public string StatusText
        {
            get
            {

                var codes = AppointmentStatusCodes.GetAllCodes();
                var result = codes.Where(w => w.Value.ToString() == this.Status.ToString()).FirstOrDefault().Text;
                return result;
            }
        }

        public Guid? ConsultID { get; internal set; }
    }
}