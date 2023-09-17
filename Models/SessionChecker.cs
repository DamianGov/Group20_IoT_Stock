using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group20_IoT.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionCheckerAttribute : ActionFilterAttribute
    {
        private readonly string[] roleTypes;

        public SessionCheckerAttribute(params string[] roleTypes)
        {
            this.roleTypes = roleTypes;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string UserType = filterContext.HttpContext.Session["UserType"] as string;

            if(!UserType.IsNullOrEmpty())
            {
                foreach (string roleType in roleTypes)
                {
                    if (roleType == UserType)
                    {
                        base.OnActionExecuting(filterContext);
                        return;
                    }
                }
            }
            // None of the required roles matched, so redirect to login.
            filterContext.Result = new RedirectResult("~/Login/");
        }
    }


}