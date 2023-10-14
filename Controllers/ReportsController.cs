using Group20_IoT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group20_IoT.Controllers
{
    [SessionChecker("SuperAdmin", "Admin")]
    public class ReportsController : Controller
    {
        private IoTContext db = new IoTContext();
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }
    }
}