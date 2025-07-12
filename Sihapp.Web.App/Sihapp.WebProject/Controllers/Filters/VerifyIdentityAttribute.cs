using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace Sihapp.WebProject.Controllers.Filters
{
    public class VerifyIdentityAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated || new BasicController().GetUserFromDataContext().UserType == Models.TypesCatalogs.UserTypeCodes.Patient)
                HttpContext.Current.Response.Redirect("../Account/Login");
            //new BasicController().RedirectToAction("Login", "Account");
        }
    }
}