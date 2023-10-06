using Group20_IoT.Models;
using Group20_IoT.Models.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Group20_IoT.Models.LoanStatus;

namespace Group20_IoT.Controllers
{
    [SessionChecker("SuperAdmin", "Admin")]
    public class ReviewLoanController : Controller
    {
        private IoTContext db = new IoTContext();
        // GET: ReviewLoan
        public ActionResult Index()
        {
            Users users = Session["User"] as Users;
            var CurrentDate = DateTime.Now.Date;

            var LoanStockPending = db.RequestLoanStock.Include(r => r.Stock).Include(r => r.Users).Include(r => r.Users.Role).Where(r => r.Status == RequestLoanStock.RequestStatus.Pending && r.Stock.Loanable && r.UserId != users.Id && DbFunctions.TruncateTime(r.FromDate) >= CurrentDate).OrderByDescending(r => r.DateRequested).AsEnumerable().Select(r=> new PendingLoanStockViewModel
            {
                Id = r.Id,
                UniNum = r.Users.UniNum,
                UserName = r.Users.GetFullName(),
                UserRole = r.Users.Role.Type,
                StockCode = r.Stock.StockCode,
                StockName = r.Stock.Name,
                AmntAva = ((r.Stock.TotalQuantity - r.Stock.QuantityOnLoan) <= 0) ? 0 : (r.Stock.TotalQuantity - r.Stock.QuantityOnLoan),
                QuantityWant = r.Quantity,
                DateRequested = r.DateRequested.ToString("dd MMMM yyyy"),
                BorrowDate = r.FromDate.ToString("dd MMMM yyyy"),
                DueDate = r.DueDate.ToString("dd MMMM yyyy")
            }).ToList();
            return View(LoanStockPending);
        }

        public ActionResult Search(int? id)
        {
            List<PendingLoanStockViewModel> LoanStockPending = new List<PendingLoanStockViewModel>();
            Users users = Session["User"] as Users;
            var CurrentDate = DateTime.Now.Date;

            if (!id.HasValue) LoanStockPending = db.RequestLoanStock.Include(r => r.Stock).Include(r => r.Users).Include(r => r.Users.Role).Where(r => r.Status == RequestLoanStock.RequestStatus.Pending && r.Stock.Loanable && r.UserId != users.Id && DbFunctions.TruncateTime(r.FromDate) >= CurrentDate).OrderByDescending(r => r.DateRequested).AsEnumerable().Select(r => new PendingLoanStockViewModel
            {
                Id = r.Id,
                UniNum = r.Users.UniNum,
                UserName = r.Users.GetFullName(),
                UserRole = r.Users.Role.Type,
                StockCode = r.Stock.StockCode,
                StockName = r.Stock.Name,
                AmntAva = ((r.Stock.TotalQuantity - r.Stock.QuantityOnLoan) <= 0) ? 0 : (r.Stock.TotalQuantity - r.Stock.QuantityOnLoan),
                QuantityWant = r.Quantity,
                DateRequested = r.DateRequested.ToString("dd MMMM yyyy"),
                BorrowDate = r.FromDate.ToString("dd MMMM yyyy"),
                DueDate = r.DueDate.ToString("dd MMMM yyyy")
            }).ToList();
            else
             LoanStockPending = db.RequestLoanStock.Include(r => r.Stock).Include(r => r.Users).Include(r => r.Users.Role).Where(r => r.Status == RequestLoanStock.RequestStatus.Pending && r.Stock.Loanable && r.UserId != users.Id && DbFunctions.TruncateTime(r.FromDate) >= CurrentDate && r.Id == id).OrderByDescending(r => r.DateRequested).AsEnumerable().Select(r => new PendingLoanStockViewModel
            {
                Id = r.Id,
                UniNum = r.Users.UniNum,
                UserName = r.Users.GetFullName(),
                UserRole = r.Users.Role.Type,
                StockCode = r.Stock.StockCode,
                StockName = r.Stock.Name,
                AmntAva = ((r.Stock.TotalQuantity - r.Stock.QuantityOnLoan) <= 0) ? 0 : (r.Stock.TotalQuantity - r.Stock.QuantityOnLoan),
                QuantityWant = r.Quantity,
                DateRequested = r.DateRequested.ToString("dd MMMM yyyy"),
                BorrowDate = r.FromDate.ToString("dd MMMM yyyy"),
                DueDate = r.DueDate.ToString("dd MMMM yyyy")
            }).ToList();

            return PartialView("_PendingLoansTable", LoanStockPending);
        }
        public ActionResult Reset()
        {
            Users users = Session["User"] as Users;
            var CurrentDate = DateTime.Now.Date;

            var LoanStockPending = db.RequestLoanStock.Include(r => r.Stock).Include(r => r.Users).Include(r => r.Users.Role).Where(r => r.Status == RequestLoanStock.RequestStatus.Pending && r.Stock.Loanable && r.UserId != users.Id && DbFunctions.TruncateTime(r.FromDate) >= CurrentDate).OrderByDescending(r => r.DateRequested).AsEnumerable().Select(r => new PendingLoanStockViewModel
            {
                Id = r.Id,
                UniNum = r.Users.UniNum,
                UserName = r.Users.GetFullName(),
                UserRole = r.Users.Role.Type,
                StockCode = r.Stock.StockCode,
                StockName = r.Stock.Name,
                AmntAva = ((r.Stock.TotalQuantity - r.Stock.QuantityOnLoan) <= 0) ? 0 : (r.Stock.TotalQuantity - r.Stock.QuantityOnLoan),
                QuantityWant = r.Quantity,
                DateRequested = r.DateRequested.ToString("dd MMMM yyyy"),
                BorrowDate = r.FromDate.ToString("dd MMMM yyyy"),
                DueDate = r.DueDate.ToString("dd MMMM yyyy")
            }).ToList();

            return PartialView("_PendingLoansTable", LoanStockPending);
        }

        public ActionResult SearchAcc(int? id)
        {
            List<LoanStatus> AcceptedLoans = new List<LoanStatus>();

            if(!id.HasValue)
            {
                AcceptedLoans = db.LoanStatus.Include(r => r.RequestLoanStock).Include(r => r.RequestLoanStock.Stock).Include(r => r.RequestLoanStock.Users).Include(r => r.RequestLoanStock.Users.Role).Include(r => r.Users).OrderByDescending(r => r.AccOn).ToList();
            }
            else
            {
                AcceptedLoans = db.LoanStatus.Include(r => r.RequestLoanStock).Include(r => r.RequestLoanStock.Stock).Include(r => r.RequestLoanStock.Users).Include(r => r.RequestLoanStock.Users.Role).Include(r => r.Users).Where(r => r.Id == id).OrderByDescending(r => r.AccOn).ToList();
            }
            return PartialView("_AcceptedLoansTable", AcceptedLoans);
        }

        public ActionResult ResetAcc()
        {
            var AcceptedLoans = db.LoanStatus.Include(r => r.RequestLoanStock).Include(r => r.RequestLoanStock.Stock).Include(r => r.RequestLoanStock.Users).Include(r => r.RequestLoanStock.Users.Role).Include(r => r.Users).OrderByDescending(r => r.AccOn).ToList();
            return PartialView("_AcceptedLoansTable", AcceptedLoans);
        }

        public ActionResult AcceptLoanRequest(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var Request = db.RequestLoanStock.Find(id);

            if (Request == null) return RedirectToAction(nameof(Index));

            var Stock = db.Stock.Find(Request.StockId);

            int QuantityAva = Stock.TotalQuantity - Stock.QuantityOnLoan;

            if(Request.Quantity > QuantityAva) return Json(new { success = false, message = "There is not enough stock available to loan" });

            Users users = Session["User"] as Users;

            Stock.QuantityOnLoan += Request.Quantity;

            Request.Status = RequestLoanStock.RequestStatus.Accepted;

            LoanStatus newStatus = new LoanStatus
            {
                RequestId = Request.Id,
                AccBy = users.Id
            };

            db.Entry(Stock).State = EntityState.Modified;
            db.Entry(Request).State = EntityState.Modified;
            db.LoanStatus.Add(newStatus);
            db.SaveChanges();

            Users requestUser = db.Users.Find(Request.UserId);

            _ = SharedMethods.SendEmail(requestUser.GetFullName(), requestUser.Email, "IoT System - Loan Request Accepted", $"Hello,{requestUser.GetFullName()}.\n\nYour request to loan {Request.Quantity} x {Stock.Name} has been accepted.\nPlease pick up the stock from {users.GetFullName()}.\n\nThank you.\nKind regards,\nIoT System.", false);

            return Json(new { success = true, message = "The request has been approved" });
        }


        public ActionResult RejectLoanRequest(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            var Request = db.RequestLoanStock.Find(id);

            if (Request == null) return RedirectToAction(nameof(Index));

            Users users = Session["User"] as Users;

            Request.Status = RequestLoanStock.RequestStatus.Rejected;

            db.Entry(Request).State = EntityState.Modified;
            db.SaveChanges();

            Users requestUser = db.Users.Find(Request.UserId);

            Stock Stock = db.Stock.Find(Request.StockId);

            _ = SharedMethods.SendEmail(requestUser.GetFullName(), requestUser.Email, "IoT System - Loan Request Rejected", $"Hello,{requestUser.GetFullName()}.\n\nYour request to loan {Request.Quantity} x {Stock.Name} has unfortunately been rejected.\n\nThank you.\nKind regards,\nIoT System.", false);

            return Json(new { success = true, message = "The request has been rejected" });
        }

        public ActionResult AcceptedLoans()
        {
            var AcceptedLoans = db.LoanStatus.Include(r => r.RequestLoanStock).Include(r => r.RequestLoanStock.Stock).Include(r => r.RequestLoanStock.Users).Include(r => r.RequestLoanStock.Users.Role).Include(r => r.Users).OrderByDescending(r => r.AccOn).ToList();

            return View(AcceptedLoans);
        }

        public ActionResult RejectedLoans()
        {
            var RejectedLoans = db.RequestLoanStock.Include(r => r.Stock).Include(r => r.Users).Include(r => r.Users.Role).Where(r => r.Status == RequestLoanStock.RequestStatus.Rejected).OrderByDescending(r => r.DateRequested).AsEnumerable().Select(r => new PendingLoanStockViewModel
            {
                Id = r.Id,
                UniNum = r.Users.UniNum,
                UserName = r.Users.GetFullName(),
                UserRole = r.Users.Role.Type,
                StockCode = r.Stock.StockCode,
                StockName = r.Stock.Name,
                AmntAva = 0,
                QuantityWant = r.Quantity,
                DateRequested = r.DateRequested.ToString("dd MMMM yyyy"),
                BorrowDate = r.FromDate.ToString("dd MMMM yyyy"),
                DueDate = r.DueDate.ToString("dd MMMM yyyy")
            }).ToList();
            return View(RejectedLoans);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int? id, LoanStatusStock changeTo, string note)
        {
            if (id == null) return RedirectToAction(nameof(AcceptedLoans));

            LoanStatus loanStatus = db.LoanStatus.Find(id);

            if(loanStatus == null) return RedirectToAction(nameof(AcceptedLoans));

            loanStatus.Status = changeTo;
            loanStatus.Note = note;

            if(changeTo == LoanStatusStock.Cancelled || changeTo == LoanStatusStock.Returned)
            {
                RequestLoanStock loanStock = db.RequestLoanStock.Find(loanStatus.RequestId);
                Stock stock = db.Stock.Find(loanStock.StockId);

                stock.QuantityOnLoan -= loanStock.Quantity;

                db.Entry(stock).State = EntityState.Modified;
            }

            db.Entry(loanStatus).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true, message = "The Loan Status has been changed" });
        }
    }
}