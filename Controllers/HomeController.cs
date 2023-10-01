using Group20_IoT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Login = Group20_IoT.Models.Login;

namespace Group20_IoT.Controllers
{
    public class HomeController : Controller
    {
        private IoTContext db = new IoTContext();

        [SessionChecker("SuperAdmin")]
        public ActionResult SuperAdminHome()
        {
            return View();
        }

        [SessionChecker("Admin")]
        public ActionResult AdminHome()
        {
            return View();
        }

        [SessionChecker("Member")]
        public ActionResult MemberHome()
        {
            return View();
        }

        [SessionChecker("SuperAdmin", "Admin", "Member")]
        public ActionResult ManageAccount()
        {
            Users users = Session["User"] as Users;

            return View(users);
        }

        [SessionChecker("SuperAdmin", "Admin", "Member")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ManageAccount(Users users)
        {
            if(ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified; 
                db.SaveChanges();
            }

            return View(users);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}