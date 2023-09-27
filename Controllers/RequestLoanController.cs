using Group20_IoT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Group20_IoT.Controllers
{
    [SessionChecker("Member", "Admin")]
    public class RequestLoanController : Controller
    {
        private IoTContext db = new IoTContext();
        public ActionResult Index()
        {
            Users users = Session["User"] as Users;
            return View(db.RequestLoanStock.Include(r => r.Stock).Where(r => r.UserId == users.Id).OrderByDescending(r => r.DateRequested).ToList());
        }

        [HttpPost]
        public ActionResult CancelRequest(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var Request = db.RequestLoanStock.Find(id);

            if (Request == null) return RedirectToAction(nameof(Index));


            Request.Status = RequestLoanStock.RequestStatus.Cancelled;

            db.Entry(Request).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true, message = "Loan Request has been cancelled" });
        }
    }
}