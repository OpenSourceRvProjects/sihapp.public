using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TypeLite;

namespace Sihapp.WebProject.Models.Users
{
    [TsClass]
    public class NewUserVM
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime BirhtDate { get; set; }
        public string LastName1 { get; set; }
        public string LastName2 { get; set; }
        public string Name { get; set; }
        public int UserType { get; set; }
        public string Email { get;  set; }
        public string Phone { get; set; }
    }
}