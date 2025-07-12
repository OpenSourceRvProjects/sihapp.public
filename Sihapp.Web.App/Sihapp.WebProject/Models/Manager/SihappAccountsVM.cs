using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sihapp.WebProject.Backend.DataAccess;
using TypeLite;

namespace Sihapp.WebProject.Models.Manager
{

    [TsClass]
    public class SihappAccountsPagedVM
    {
        public SihappAccountsPagedVM()
        {
            items = new List<SihappAccountsVM>();
        }

        public List<SihappAccountsVM> items { get; set; }
        public decimal TotalPages { get; set; }
        public int AccountCount { get; set; }
    }


    [TsClass]
    public class SihappAccountsVM
    {
        public int ClinicImages { get; internal set; }
        public string ClinicName { get; internal set; }
        public int Consults { get; internal set; }
        public string MasterUser { get; internal set; }
        public int Number { get; internal set; }
        public int PatientsQty { get; internal set; }
        public Guid ClinicID { get; set; }
        public string User { get; set; }
        public string Email { get; internal set; }
        public string Phone { get; internal set; }
    }
}