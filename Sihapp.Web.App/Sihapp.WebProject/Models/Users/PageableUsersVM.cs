using Sihapp.WebProject.Backend.ServiceComponents;
using Sihapp.WebProject.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.Users
{
    [TsClass]
    public class PageableUsersVM
    {
        public PageableUsersVM()
        {
            UserItems = new List<UserItemsVM>();
        }
        public decimal TotalPages { get; set; }
        public List<UserItemsVM> UserItems { get; set; }
    }

    [TsClass]
    public abstract class UserItemsVM
    {
        public UserItemsVM()
        {

        }
        public UserItemsVM(Guid entityID)
        {
            this.EntityID = entityID;
        }

        private string userName;
        

        public string ID { get; set; }
        public string UserName
        {
            get
            { return userName; }
            set
            { userName = value; }
        }
        public int Type { get; set; }
        public DateTime CreationDate { get; set; }
        public Guid EntityID { get; set; }
        public int AppointmentCount { get; set; }
        public string TypeName
        {
            get
            {
                string result = "";
                result = UserTypeCodes.AllTypeCodes().FirstOrDefault(f => f.Value.ToString() == this.Type.ToString()).Text;
                return result;
            }
        }
        public string Name { get; set; }
        public abstract int GetAppointmentCount();
    }

    public class PatientUserVM : UserItemsVM
    {

        public PatientUserVM(Guid PatientID) : base(PatientID)
        {
            
        }

        public override int GetAppointmentCount()
        {
            return new PatientService().DatabaseInstance.Appointment.Where(w => w.PatientID == EntityID).Count();
        }

        public DateTime GetPatientBirthdate()
        {
            return new PatientService().DatabaseInstance.Patients.FirstOrDefault(f => f.Id == EntityID).Birthdate;
        }

    }


    public class ClinicMenUserVM : UserItemsVM
    {

        public ClinicMenUserVM(Guid ClinicMenID) : base(ClinicMenID)
        {

        }

        public override int GetAppointmentCount()
        {
            return new PatientService().DatabaseInstance.Appointment.Where(w => w.ClinicMenID == EntityID).Count();
        }

        public DateTime GetClinicMenCreationDate()
        {
            return new PatientService().DatabaseInstance.ClinicMen.FirstOrDefault(f => f.ID == EntityID).CreationDate;
        }

    }

    public class ClinicAuxiliarUserVM : UserItemsVM
    {

        public ClinicAuxiliarUserVM(Guid ClinicMenID) : base(ClinicMenID)
        {

        }

        public DateTime GetAuxiliarPersonalCreationDate()
        {
            return new AuxiliarPersonalService().DatabaseInstance.AuxiliarPersonal.FirstOrDefault(f => f.Id == EntityID).CreationDate;
        }

        public override int GetAppointmentCount()
        {
            return new AuxiliarPersonalService().DatabaseInstance.Appointment.Where(w => w.CreatedAuxiliar == EntityID).Count();
        }

    }
}