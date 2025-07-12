using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sihapp.WebProject.Controllers;
using Sihapp.WebProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Sihapp.WebProject.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests
    {

        public AccountControllerTests()
        {
            var m_HttpContext = new Mock<System.Web.HttpContextBase>();
            List<Claim> claims = new List<Claim>{
   new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "rolando"),
   new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "")
 };


            var genericIdentity = new GenericIdentity("");
            var genericPrincipal = new GenericPrincipal(genericIdentity, new string[] { });
            m_HttpContext.SetupGet(x => x.User).Returns(genericPrincipal);
            var m_controllerContext = new Mock<ControllerContext>();
            m_controllerContext.Setup(t => t.HttpContext).Returns(m_HttpContext.Object);

            //var userStore = new Mock<IUserStore<ApplicationUser>>();
            //var m_userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object);
        }

        [TestMethod()]
        public void RegisterTest()
        {
            Assert.Fail();
        }

    }
}