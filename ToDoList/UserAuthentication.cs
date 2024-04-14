using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ToDoList
{
    public class UserAuthentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Session.Keys.Contains("UserID"))
            {
                context.Result = new RedirectToActionResult("Login", "Users", null);
            }
            base.OnActionExecuting(context);
        }
    }
}