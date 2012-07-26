using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using hb.Web.Controllers;

//Usage: add attribute {RequireAdmin] to any controller method requiring security
namespace hb.Web.Infrastructure {
    public class RequireSecurityAttribute:ActionFilterAttribute
    {
       
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var controller = (ApplicationController)filterContext.Controller;

            //user logged in?
            if (!controller.IsLoggedIn) {
                controller.TempData["Error"] = "You need to be logged in to do that";
                filterContext.HttpContext.Response.Redirect("/account/logon");
                return;
            }

            //is the user an admin?
            //var adminEmails = new string[] { "harsh@gmail.com", "harsh@go.com" };
            //string userEmail = controller.CurrentUser.Email;
            //if (!adminEmails.Contains(userEmail)) {
            //    controller.TempData["Error"] = "You're not authorized yo...";
            //    filterContext.HttpContext.Response.Redirect("/account/logon");
            //}
        }
    }//class
}//ns