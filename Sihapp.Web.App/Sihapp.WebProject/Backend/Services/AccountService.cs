using Sihapp.WebProject.Backend.DataAccess;
using Sihapp.WebProject.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sihapp.WebProject.Models.Common;
using Sihapp.WebProject.Models.Users;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Net.Mail;
using Sihapp.WebProject.Models.TypesCatalogs;
using RestSharp;
using Newtonsoft.Json;
using Sihapp.WebProject.Services;

namespace Sihapp.WebProject.Backend.ServiceComponents
{
    public class AccountService : BasicService
    {


        public UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        #region GetAccountInfoByUser
        //Get PatientID given the aspnet user ID
        public Guid GetPatientIdByUserID(string userId)
        {
            var userID = DatabaseInstance.Patients.Where(w => w.UserID == userId).FirstOrDefault()?.Id;
            if (userID == null)
                return Guid.Empty;
            else
                return userID.Value;
        }

        //Get ClinicMen given the aspnet user ID
        public Guid GetClinicMenIdByUserId(string userId)
        {
            var clinicMenID = DatabaseInstance.ClinicMen.Where(w => w.UserID == userId).FirstOrDefault().ID;
            return clinicMenID;
        }

        public string GenerateDevToken(TokenUserCredentialsVM model)
        {

            try
            {
                //var client = new RestClient("https://localhost:44303/api/login/authenticate");
                var client = new RestClient("http://sihappapi.azurewebsites.net/api/login/authenticate");
                var request = new RestRequest(Method.POST);
                var credentials = new { Username = model.Username, Password = model.Password };
                request.AddJsonBody(credentials);
                var response = client.Execute(request);
                if (string.IsNullOrWhiteSpace(response.Content))
                    throw new Exception("Response " + response.StatusCode + " -" + response.ErrorMessage + " i- " + response.ErrorException?.Message );
                return JsonConvert.DeserializeObject<string>(response.Content);
            }
            catch (Exception ex)
            {
                AddLog(ex.Message, true, ex.InnerException?.Message);
                return "";
                

            }
        }

        //Get the clinic ID from the aspnet username
        public Guid GetClinicIDByUserName(string usuario)
        {
            var clinicNumber = GetClinicNumberByUserName(usuario);
            var clinicID = DatabaseInstance.Clinic.FirstOrDefault(f => f.Number == clinicNumber).ID;
            return clinicID;
        }

        //Get the clinic number from aspnet username
        public int GetClinicNumberByUserName(string usuario)
        {
            return DatabaseInstance.AspNetUsers.Where(w => w.UserName == usuario).FirstOrDefault().ClinicNumber.Value;
        }


        #endregion

        #region ClinicCreation
        // This method create the clinic from the new user register form 
        // and return the Clinic ID
        public Guid CreateClinic(RegisterViewModel newClinic)
        {

            var newClinicNumber = GetNewClinicNumber();

            var newClinicRegister = new Clinic();
            newClinicRegister.Address = newClinic.Address;
            newClinicRegister.Name = newClinic.ClinicName;
            newClinicRegister.Phone = newClinic.Telephone;
            newClinicRegister.RFC = newClinicRegister.RFC;
            newClinicRegister.Number = newClinicNumber;
            newClinicRegister.EndSubscriptionDate = new DateTime(2019, 01, 01);
            newClinicRegister.ID = Guid.NewGuid();
            newClinicRegister.RegisterDate = DateTime.Now;

            DatabaseInstance.Clinic.Add(newClinicRegister);
            DatabaseInstance.SaveChanges();


            return newClinicRegister.ID;

        }

        // Check the new ClinicNumber, the algorythm order the existed clinics and add 1 to the clinic number
        private int GetNewClinicNumber()
        {
            var dataContext = new SihappDBEntities();
            var clinics = DatabaseInstance.Clinic.AsQueryable();

            if (clinics.Any())
            {
                var lastClinicNumber = clinics.OrderByDescending(o => o.Number).FirstOrDefault().Number;
                return lastClinicNumber + 1;
            }

            return 1000;

        }

        // Creates a clinic men based on the new Clinic from the register form, this method is diferent
        // from the new Clinic men user created by another user.
        public void CreateClinicMen(ApplicationUser user, Guid clinicID, RegisterViewModel model)
        {
            var newClinicMen = new ClinicMen();
            newClinicMen.Address = model.Address;
            newClinicMen.BirthDate = model.BirthDate;
            newClinicMen.CreationDate = DateTime.Now;
            newClinicMen.Cedula = "";
            newClinicMen.CellPhone = model.Telephone;
            newClinicMen.Type = 1;
            newClinicMen.Specialty = "No definido";
            newClinicMen.HireDate = DateTime.Now;
            newClinicMen.Name = model.Name;
            newClinicMen.LastName1 = model.LastName1;
            newClinicMen.LastName2 = model.LastName2;
            newClinicMen.ID = Guid.NewGuid();
            newClinicMen.UserID = user.Id;
            newClinicMen.Phone = model.Telephone;
            newClinicMen.ClinicID = clinicID;
            newClinicMen.Email = model.Email;
            newClinicMen.Number = new ClinicMenService().GetNewClinicMenNumber();

            DatabaseInstance.ClinicMen.Add(newClinicMen);
            DatabaseInstance.SaveChanges();
            var clinicNumber = DatabaseInstance.Clinic.FirstOrDefault(f => f.ID == clinicID).Number;
            new ClinicMenService().UpdateClinicMenUserStatusType(user.Id, clinicNumber);
        }


        // The method send a mail from smtp protocol in gmail from sihapp.web@gmail.com
        public void SendWelcomeEmail(Guid clinicID, RegisterViewModel model, int clinicNumber)
        {

            //var usr = DatabaseInstance.AspNetUsers.FirstOrDefault(f => f.UserName == "rolando").Id;
            var comp = DatabaseInstance.Clinic.FirstOrDefault().ID;
            try
            {
                var body = "Hola " + model.Name + " " + model.LastName1 + ". Bienvenido a Sihapp, el sistema para la adminitración de tus pacientes ";
                body += "<p>Tu Id de Clínica o número de clínica es: " + clinicNumber.ToString() + "</p>";
                body += "<p>Tu usuario es:  </p>" + model.User + "</p>";
                body += "<p>Tu contraseña es: " + model.Password + "</p>";
                body += "<br />";
                body += "<br />";
                body += "<p>Espero que este proyecto sea de ayuda para administrar la agenda de tus pacientes, así como administrar sus expedientes de manera electrónica y segura.</p>";
                body += "<p><h3>Atte. Sihapp team<h3></p>";
                body += "<p><h4>Fundador del proyecto.<h4></p>";
                List<string> emails = new List<string>();
                emails.Add(model.Email);
                emails.Add("sihapp.app@gmail.com");
                new UtilService().SendEmail(emails, "¡Te damos la bienvenida a Sihapp!", body);
                


            }
            catch (Exception ex)
            {
                AddLog(ex.Message,  true, "", null, clinicID);
            }
        }


        #endregion
        /*This method create an paged model in server with all kind of users*/
        public PageableUsersVM GetPagedUsers(string search, PaginationVM pagination)
        {
            var pagedUsers = new PageableUsersVM();

            var allUsers = GetAllUsers();
            if (!string.IsNullOrWhiteSpace(search))
                allUsers = allUsers.Where(w => w.UserName.Contains(search));

            var users = allUsers.Select(s => new
            {
                UserName = s.UserName,
                ID = s.Id,
                Type = s.UserType,
            });

            pagedUsers.TotalPages = Math.Ceiling((decimal)allUsers.Count() / pagination.PageSize);
            var data = users.OrderByDescending(o => o.UserName).Skip(pagination.PageSize * pagination.Page)
                .Take(pagination.PageSize).ToList();

            var usrs = new List<UserItemsVM>();

            var patients = new List<PatientUserVM>();

            var patientList = data.Where(w => w.Type == UserTypeCodes.Patient).ToList();
            foreach (var p in patientList)
            {
                try
                {
                    var patientID = new AccountService().GetPatientIdByUserID(p.ID);
                    if (patientID == Guid.Empty)
                        continue;
                    var currentPatient = new PatientUserVM(patientID);
                    currentPatient.CreationDate = currentPatient.GetPatientBirthdate();
                    var patient = new PatientService().GetPatientByID(patientID);
                    currentPatient.Name = patient.Name + " " + patient.LastName1 + " " + patient.LastName2;
                    currentPatient.UserName = p.UserName;
                    currentPatient.Type = p.Type;
                    currentPatient.ID = p.ID;
                    currentPatient.AppointmentCount = currentPatient.GetAppointmentCount();
                    patients.Add(currentPatient);
                }
                catch (Exception ex)
                {

                }
            }

            var clinicMens = new List<ClinicMenUserVM>();
            var clinicMenList = data.Where(w => w.Type == UserTypeCodes.ClinicMen).ToList();
            foreach (var cm in clinicMenList)
            {
                try
                {
                    var clinicMenId = new AccountService().GetClinicMenIdByUserId(cm.ID);
                    var currentClinicMen = new ClinicMenUserVM(clinicMenId);
                    currentClinicMen.CreationDate = currentClinicMen.GetClinicMenCreationDate();
                    var clinicMen = new ClinicMenService().GetClinicMenByID(clinicMenId);
                    currentClinicMen.Name = clinicMen.Name + " " + clinicMen.LastName1 + " " + clinicMen.LastName2;
                    currentClinicMen.UserName = cm.UserName;
                    currentClinicMen.Type = cm.Type;
                    currentClinicMen.ID = cm.ID;
                    currentClinicMen.AppointmentCount = currentClinicMen.GetAppointmentCount();
                    clinicMens.Add(currentClinicMen);
                }
                catch (Exception ex)
                {

                }

            }

            var clinicAuxiliar = new List<ClinicAuxiliarUserVM>();
            var clinicAuxiliarList = data.Where(w => w.Type == UserTypeCodes.AuxiliarPersonal).ToList();
            foreach (var ca in clinicAuxiliarList)
            {
                try
                {
                    var auxiliarPersonalId = new AccountService().GetClinicAuxiliarIdByUserId(ca.ID);
                    var currentAuxiliarPersonal = new ClinicAuxiliarUserVM(auxiliarPersonalId);
                    currentAuxiliarPersonal.CreationDate = currentAuxiliarPersonal.GetAuxiliarPersonalCreationDate();
                    var auxiliarpersonal = new AuxiliarPersonalService().GetAuxiliarPersonalByID(auxiliarPersonalId);
                    currentAuxiliarPersonal.Name = auxiliarpersonal.Name + " " + auxiliarpersonal.LastName1 + " "
                        + auxiliarpersonal.LastName2;
                    currentAuxiliarPersonal.UserName = ca.UserName;
                    currentAuxiliarPersonal.Type = ca.Type;
                    currentAuxiliarPersonal.ID = ca.ID;
                    currentAuxiliarPersonal.AppointmentCount = currentAuxiliarPersonal.GetAppointmentCount();
                    clinicAuxiliar.Add(currentAuxiliarPersonal);
                }
                catch (Exception ex)
                {

                }
            }

            usrs.AddRange(patients);
            usrs.AddRange(clinicMens);
            usrs.AddRange(clinicAuxiliar);
            pagedUsers.UserItems = usrs;

            return pagedUsers;
        }


        // Get the clinic auxiliar ID from the aspnet user ID
        private Guid GetClinicAuxiliarIdByUserId(string id)
        {
            var auxiliarPersonal = DatabaseInstance.AuxiliarPersonal.Where(w => w.UserID == id).FirstOrDefault().Id;
            return auxiliarPersonal;
        }





        #region DataAccess
        // This method get all users from logged clinic
        public IQueryable<AspNetUsers> GetAllUsers()
        {
            var clinicNumber = GetLoggedClinicNumber();
            return DatabaseInstance.AspNetUsers.Where(w => w.ClinicNumber == clinicNumber);
        }

        // Check if there is any user name previous named like userName parameter on aspnet users.
        public bool IsUserAlreadyExist(string userName)
        {
            var existUser = DatabaseInstance.AspNetUsers.Any(a => a.UserName == userName);
            return existUser;
        }

        #endregion






        #region helpers
        public int GetClinicNumberByID(Guid clinicID)
        {
            return DatabaseInstance.Clinic.FirstOrDefault(w => w.ID == clinicID).Number;
        }

        //public string GetUserNameForRegister(string user, int clinicNumber)
        //{
        //    return user + "+" + clinicNumber.ToString();
        //}
        public void SaveSuccessfullLogin(string IP, string UserID, int clinicNumber, Guid clinicID, string pwdType = null)
        {
            var newLogin = new SucessfullLogins();
            newLogin.Date = DateTime.Now;
            newLogin.ID = Guid.NewGuid();
            newLogin.UserId = UserID;
            newLogin.IP = pwdType != null ? pwdType + "(" + IP + ")" : IP;
            newLogin.ClinicNumber = clinicNumber;
            newLogin.ClinicID = clinicID;
            DatabaseInstance.SucessfullLogins.Add(newLogin);
            DatabaseInstance.SaveChanges();

        }
        #endregion
    }
}