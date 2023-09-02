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
    [SessionChecker]
    public class HomeController : Controller
    {
        private IoTContext db = new IoTContext();

        [SessionCheckerSuperUser]
        public ActionResult SuperHome()
        {
            return View();
        }

        [SessionCheckerAdmin]
        public ActionResult AdminHome()
        {
            return View();
        }

        [SessionCheckerStandard]
        public ActionResult StandardHome()
        {
            return View();
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