using Group20_IoT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group20_IoT.Controllers
{
    [SessionChecker("SuperAdmin", "Admin")]
    public class ExtensionsController : Controller
    {
        private IoTContext db = new IoTContext();
        public ActionResult Index()
        {
            var PendingExtensions = db.ExtensionRequest.Include(e => e.LoanStatus).Include(e => e.LoanStatus.Users).Include(e => e.LoanStatus.RequestLoanStock.Users).Include(e => e.LoanStatus.RequestLoanStock.Stock).Include(e => e.LoanStatus.RequestLoanStock.Users.Role).Where(e => e.Status == ExtensionRequest.ExtensionStatus.Pending).OrderByDescending(e => e.ExtensionSubmitted).ToList();
            return View(PendingExtensions);
        }
        public ActionResult Search(int? id)
        {
            List<ExtensionRequest> PendingExtensions = new List<ExtensionRequest>();
            if(!id.HasValue)
            {
                PendingExtensions = db.ExtensionRequest.Include(e => e.LoanStatus).Include(e => e.LoanStatus.Users).Include(e => e.LoanStatus.RequestLoanStock.Users).Include(e => e.LoanStatus.RequestLoanStock.Stock).Include(e => e.LoanStatus.RequestLoanStock.Users.Role).Where(e => e.Status == ExtensionRequest.ExtensionStatus.Pending).OrderByDescending(e => e.ExtensionSubmitted).ToList();
            }
            else
            {
                PendingExtensions = db.ExtensionRequest.Include(e => e.LoanStatus).Include(e => e.LoanStatus.Users).Include(e => e.LoanStatus.RequestLoanStock.Users).Include(e => e.LoanStatus.RequestLoanStock.Stock).Include(e => e.LoanStatus.RequestLoanStock.Users.Role).Where(e => e.Status == ExtensionRequest.ExtensionStatus.Pending && e.LoanStatusId == id).OrderByDescending(e => e.ExtensionSubmitted).ToList();
            }

            return PartialView("_PendingExtensionsTable", PendingExtensions);
        }

        public ActionResult Reset()
        {
            var PendingExtensions = db.ExtensionRequest.Include(e => e.LoanStatus).Include(e => e.LoanStatus.Users).Include(e => e.LoanStatus.RequestLoanStock.Users).Include(e => e.LoanStatus.RequestLoanStock.Stock).Include(e => e.LoanStatus.RequestLoanStock.Users.Role).Where(e => e.Status == ExtensionRequest.ExtensionStatus.Pending).OrderByDescending(e => e.ExtensionSubmitted).ToList();
            return PartialView("_PendingExtensionsTable", PendingExtensions);
        }

        public ActionResult AcceptedExtensions()
        {
            var AcceptedExtensions = db.ExtensionRequest.Include(e => e.LoanStatus).Include(e => e.LoanStatus.Users).Include(e => e.LoanStatus.RequestLoanStock.Users).Include(e => e.LoanStatus.RequestLoanStock.Stock).Include(e => e.LoanStatus.RequestLoanStock.Users.Role).Where(e => e.Status == ExtensionRequest.ExtensionStatus.Accepted).OrderByDescending(e => e.ExtensionSubmitted).ToList();
            return View(AcceptedExtensions);
        }
        public ActionResult RejectedExtensions()
        {
            var RejectedExtensions = db.ExtensionRequest.Include(e => e.LoanStatus).Include(e => e.LoanStatus.Users).Include(e => e.LoanStatus.RequestLoanStock.Users).Include(e => e.LoanStatus.RequestLoanStock.Stock).Include(e => e.LoanStatus.RequestLoanStock.Users.Role).Where(e => e.Status == ExtensionRequest.ExtensionStatus.Rejected).OrderByDescending(e => e.ExtensionSubmitted).ToList();
            return View(RejectedExtensions);
        }
        [HttpPost]
        public ActionResult AcceptExtension(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            ExtensionRequest request = db.ExtensionRequest.Include(e => e.LoanStatus).Include(e => e.LoanStatus.RequestLoanStock).Include(e => e.LoanStatus.RequestLoanStock.Stock).Include(e => e.LoanStatus.RequestLoanStock.Users).Where(e => e.Id == id).FirstOrDefault();

            if(request == null) return RedirectToAction("Index");

            RequestLoanStock requestLoan = db.RequestLoanStock.Find(request.LoanStatus.RequestLoanStock.Id);

            DateTime ExtendBy;

            if (request.LoanStatus.Status == LoanStatus.LoanStatusStock.Overdue)
            {
                ExtendBy = SharedMethods.AddBusinessDays(DateTime.Now, 3);
                request.LoanStatus.Status = LoanStatus.LoanStatusStock.Picked_Up;
                db.Entry(request).State = EntityState.Modified;
            }
            else
                ExtendBy = SharedMethods.AddBusinessDays(requestLoan.DueDate, 3);

            string OriginallyDue = requestLoan.DueDate.ToString("dd MMMM yyyy");

            requestLoan.DueDate = ExtendBy;
            request.Status = ExtensionRequest.ExtensionStatus.Accepted;

            db.Entry(requestLoan).State = EntityState.Modified;
            db.SaveChanges();

            _ = SharedMethods.SendEmail(request.LoanStatus.RequestLoanStock.Users.GetFullName(), request.LoanStatus.RequestLoanStock.Users.Email, $"IoT System - Extension Accepted [Loan Reference #{request.LoanStatusId}]", $"Hello, {request.LoanStatus.RequestLoanStock.Users.GetFullName()}.\n\nThe extension you requested regarding\n \"{request.LoanStatus.RequestLoanStock.Quantity} x {request.LoanStatus.RequestLoanStock.Stock.Name}\" \nthat was due on {OriginallyDue} is now due on {requestLoan.DueDate.ToString("dd MMMM yyyy")}.\n\nThank you.\nKind regards,\nIoT System.", false);

            return Json(new { success = true, message = "Extension applied successfully" });
        }

        [HttpPost]
        public ActionResult RejectExtension(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            ExtensionRequest request = db.ExtensionRequest.Include(e => e.LoanStatus).Include(e => e.LoanStatus.RequestLoanStock).Include(e => e.LoanStatus.RequestLoanStock.Stock).Include(e => e.LoanStatus.RequestLoanStock.Users).Where(e => e.Id == id).FirstOrDefault();

            if (request == null) return RedirectToAction("Index");

            request.Status = ExtensionRequest.ExtensionStatus.Rejected;

            db.Entry(request).State = EntityState.Modified;
            db.SaveChanges();

            _ = SharedMethods.SendEmail(request.LoanStatus.RequestLoanStock.Users.GetFullName(), request.LoanStatus.RequestLoanStock.Users.Email, $"IoT System - Extension Rejected [Loan Reference #{request.LoanStatusId}]", $"Hello, {request.LoanStatus.RequestLoanStock.Users.GetFullName()}.\n\nThe extension you requested regarding\n \"{request.LoanStatus.RequestLoanStock.Quantity} x {request.LoanStatus.RequestLoanStock.Stock.Name}\" \nthat is due on {request.LoanStatus.RequestLoanStock.DueDate.ToString("dd MMMM yyyy")} has unfortunately been rejected.\n\nThank you.\nKind regards,\nIoT System.", false);

            return Json(new { success = true, message = "Extension rejected successfully" });
        }
    }
}