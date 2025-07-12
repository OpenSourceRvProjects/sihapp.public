using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;

namespace Sihapp.WebProject.Backend
{
    public class HandleExceptionAttribute : HandleErrorAttribute
    {

        //https://www.veloxcore.com/exception-handling-in-mvc
        //Search: How to handle exceptions in MVC
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception != null && !filterContext.ExceptionHandled)
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

                var Message = filterContext.Exception.Message;

                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = Message
                };
                filterContext.ExceptionHandled = true;
            }
            else
            {
                base.OnException(filterContext);
            }
        }

    }

}