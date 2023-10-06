using Group20_IoT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace Group20_IoT.Controllers
{
    [SessionChecker("Member", "Admin")]
    public class RequestLoanController : Controller
    {
        private IoTContext db = new IoTContext();
        public ActionResult Index()
        {
            Users users = Session["User"] as Users;
            return View(db.RequestLoanStock.Include(r => r.Stock).Where(r => r.UserId == users.Id && r.Status != RequestLoanStock.RequestStatus.Accepted).OrderByDescending(r => r.DateRequested).OrderBy( r=> r.Status).ToList());
        }

        public ActionResult Search(int? id)
        {
            Users users = Session["User"] as Users;
            List<RequestLoanStock> requestLoanStocks = new List<RequestLoanStock>();
            if(!id.HasValue)
            {
                requestLoanStocks = db.RequestLoanStock.Include(r => r.Stock).Where(r => r.UserId == users.Id && r.Status != RequestLoanStock.RequestStatus.Accepted).OrderByDescending(r => r.DateRequested).OrderBy(r => r.Status).ToList();
            }
            else
            {
                requestLoanStocks = db.RequestLoanStock.Include(r => r.Stock).Where(r => r.UserId == users.Id && r.Status != RequestLoanStock.RequestStatus.Accepted && r.Id == id).OrderByDescending(r => r.DateRequested).OrderBy(r => r.Status).ToList();
            }

            return PartialView("_RequestLoanTable", requestLoanStocks);
        }

        public ActionResult Reset()
        {
            Users users = Session["User"] as Users;
            List<RequestLoanStock> requestLoanStocks = db.RequestLoanStock.Include(r => r.Stock).Where(r => r.UserId == users.Id && r.Status != RequestLoanStock.RequestStatus.Accepted).OrderByDescending(r => r.DateRequested).OrderBy(r => r.Status).ToList();
            return PartialView("_RequestLoanTable", requestLoanStocks);
        }

        public ActionResult Sort(int? sort)
        {
            Users users = Session["User"] as Users;
            List<RequestLoanStock> requestLoanStocks = new List<RequestLoanStock>();
            if (sort == 0 || sort == 2 || sort == 3)
            {
                requestLoanStocks = db.RequestLoanStock.Include(r => r.Stock).Where(r => r.UserId == users.Id && (int)r.Status == sort).OrderByDescending(r => r.DateRequested).OrderBy(r => r.Status).ToList();
            }else
                requestLoanStocks = db.RequestLoanStock.Include(r => r.Stock).Where(r => r.UserId == users.Id && r.Status != RequestLoanStock.RequestStatus.Accepted).OrderByDescending(r => r.DateRequested).OrderBy(r => r.Status).ToList();
            return PartialView("_RequestLoanTable", requestLoanStocks);
        }

        public ActionResult AcceptedLoans()
        {
            Users users = Session["User"] as Users;

            var AcceptedReq = db.LoanStatus.Include(r => r.RequestLoanStock).Include(r => r.RequestLoanStock.Stock).Where(r => r.RequestLoanStock.UserId == users.Id).OrderByDescending(r => r.AccOn).ToList();

            return View(AcceptedReq);

        }

        [HttpPost]
        public ActionResult RequestExtension(int? id, string reason)
        {
            if (id == null) return RedirectToAction(nameof(AcceptedLoans));

            LoanStatus loanStatus = db.LoanStatus.Find(id);

            if (loanStatus == null) return RedirectToAction(nameof(AcceptedLoans));

            loanStatus.RequestExtension = true;

            ExtensionRequest request = new ExtensionRequest
            {
                LoanStatusId = loanStatus.Id,
                Reason = reason,
                Status = ExtensionRequest.ExtensionStatus.Pending
            };

            db.ExtensionRequest.Add(request);
            db.Entry(loanStatus).State = EntityState.Modified;

            db.SaveChanges();

            return Json(new { success = true, message = "Your request for an Extension has been submitted" });
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

        [HttpPost]
        public ActionResult CancelAcceptedRequest(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var Request = db.RequestLoanStock.Find(id);

            if (Request == null) return RedirectToAction(nameof(Index));

            Request.Status = RequestLoanStock.RequestStatus.Cancelled;

            Stock stock = db.Stock.Find(Request.StockId);
            stock.QuantityOnLoan -= Request.Quantity;

            db.Entry(stock).State = EntityState.Modified;
            db.Entry(Request).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true, message = "Loan Request has been cancelled" });
        }
    }
}