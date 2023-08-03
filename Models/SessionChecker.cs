using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group20_IoT.Models
{
    public class SessionChecker : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["User"] == null)
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class SessionCheckerAdmin : SessionChecker
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);


            Users user = filterContext.HttpContext.Session["User"] as Users;

            if (user == null || user.RoleId != 1)
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
                return;
            }
        }
    }

    public class SessionCheckerStandard : SessionChecker
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);


            Users user = filterContext.HttpContext.Session["User"] as Users;

            if (user == null || user.RoleId != 2)
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
                return;
            }
        }
    }
}