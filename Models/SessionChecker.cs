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
                filterContext.Result = new RedirectResult("~/Login/");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class SessionCheckerAdmin : SessionChecker
    {
        private IoTContext db = new IoTContext();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            int SuperUser = db.Role.Single(r => r.Type.Equals("SuperUser")).Id;
            int Admin = db.Role.Single(r => r.Type.Equals("Admin")).Id;

            var AdminSuperUser = db.Users.Where(u => u.RoleId.Equals(SuperUser) || u.RoleId.Equals(Admin)).Select(r =>r.RoleId).ToList();
            
            if (!(filterContext.HttpContext.Session["User"] is Users user) || !AdminSuperUser.Contains(user.RoleId))
            {
                filterContext.Result = new RedirectResult("~/Login/");
                return;
            }
            // Add else>>> must go to standard user home
        }
    }

    public class SessionCheckerStandard : SessionChecker
    {
        private IoTContext db = new IoTContext();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);


            Users user = filterContext.HttpContext.Session["User"] as Users;

            if (user == null || user.RoleId != db.Role.Single(r => r.Type.Equals("Standard")).Id)
            {
                filterContext.Result = new RedirectResult("~/Login/");
                return;
            }
            // Add else>>> must go to admin user home
        }
    }

    public class SessionCheckerSuperUser : SessionChecker
    {
        private IoTContext db = new IoTContext();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);


            Users user = filterContext.HttpContext.Session["User"] as Users;

            if (user == null || user.RoleId != db.Role.Single(r => r.Type.Equals("SuperUser")).Id)
            {
                filterContext.Result = new RedirectResult("~/Login/");
                return;
            }
            // Add else>>> must go to admin user home
        }
    }

}