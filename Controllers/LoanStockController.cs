using Group20_IoT.Models;
using Group20_IoT.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group20_IoT.Controllers
{
    [SessionChecker("Member", "Admin")]
    public class LoanStockController : Controller
    {
        private IoTContext db = new IoTContext();
        // GET: LoanStock
        public ActionResult Index()
        {
            var LoanableStock = db.Stock.Where(s => s.Loanable).Select(s => new LoanStockViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ImageFile = s.ImageFile,
                StockCode = s.StockCode,
                QuantityCanLoan = ((s.TotalQuantity - s.QuantityOnLoan) <= 0) ? 0 : s.TotalQuantity - s.QuantityOnLoan
            }).ToList();    

            return View(LoanableStock);
        }

        public ActionResult Search(string searchItem)
        {
            List<LoanStockViewModel> LoanableStock = new List<LoanStockViewModel>();
            if (string.IsNullOrEmpty(searchItem))
            {
                LoanableStock = db.Stock.Where(s => s.Loanable).Select(s => new LoanStockViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ImageFile = s.ImageFile,
                    StockCode = s.StockCode,
                    QuantityCanLoan = ((s.TotalQuantity - s.QuantityOnLoan) <= 0) ? 0 : s.TotalQuantity - s.QuantityOnLoan
                }).ToList();
            }
            else
            {
                LoanableStock = db.Stock.Where(s => s.Loanable && s.Name.ToLower().Contains(searchItem.Trim().ToLower())).Select(s => new LoanStockViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ImageFile = s.ImageFile,
                    StockCode = s.StockCode,
                    QuantityCanLoan = ((s.TotalQuantity - s.QuantityOnLoan) <= 0) ? 0 : s.TotalQuantity - s.QuantityOnLoan
                }).ToList();
            }
            return PartialView("_LoanStockTable", LoanableStock);
        }

        public ActionResult Reset()
        {
            var LoanableStock = db.Stock.Where(s => s.Loanable).Select(s => new LoanStockViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ImageFile = s.ImageFile,
                StockCode = s.StockCode,
                QuantityCanLoan = ((s.TotalQuantity - s.QuantityOnLoan) <= 0) ? 0 : s.TotalQuantity - s.QuantityOnLoan
            }).ToList();
            return PartialView("_LoanStockTable", LoanableStock);
        }

        public ActionResult Filter(string sort)
        {
            List<LoanStockViewModel> LoanableStock =  db.Stock.Where(s => s.Loanable).Select(s => new LoanStockViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ImageFile = s.ImageFile,
                StockCode = s.StockCode,
                QuantityCanLoan = ((s.TotalQuantity - s.QuantityOnLoan) <= 0) ? 0 : s.TotalQuantity - s.QuantityOnLoan
            }).ToList();
            if (sort == "StockLow")
                LoanableStock.OrderBy(l => l.QuantityCanLoan);
            else if (sort == "StockHigh")
                LoanableStock.OrderByDescending(l => l.QuantityCanLoan);

            return PartialView("_LoanStockTable", LoanableStock);
        }

        public ActionResult Details(int? id)
        {
            if(id == null) return RedirectToAction("Index");

            Stock stock = db.Stock.Find(id);

            if (stock == null) return RedirectToAction("Index");

            LoanStockDetailsViewModel obj = new LoanStockDetailsViewModel
            {
                StockId = stock.Id,
                StockCode = stock.StockCode,
                Name = stock.Name,
                Description = stock.Description,
                QuantityAva = ((stock.TotalQuantity - stock.QuantityOnLoan) <= 0) ? 0 : (stock.TotalQuantity - stock.QuantityOnLoan),
                QuantityWantToLoan = 1,
                Image = stock.ImageFile
            };

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(LoanStockDetailsViewModel obj)
        {
            if(ModelState.IsValid)
            {
                Users users = Session["User"] as Users;
                if (obj.QuantityWantToLoan > obj.QuantityAva)
                {
                    ModelState.AddModelError("QuantityWantToLoan", "This quantity exceeds the Quantity Available to loan.");
                    return View(obj);
                }

                RequestLoanStock requestLoanStock = new RequestLoanStock
                {
                    StockId=obj.StockId,
                    FromDate = obj.BorrowStartDate,
                    DueDate = SharedMethods.AddBusinessDays(obj.BorrowStartDate,7),
                    Quantity = obj.QuantityWantToLoan,
                    UserId = users.Id
                };

                Stock stock = db.Stock.Find(obj.StockId);
                
                _ = SharedMethods.SendEmail(users.GetFullName(), users.Email, "IoT System - Loan Stock Request Submitted", $"Hello,{users.GetFullName()}.\n\nYour request to loan {obj.QuantityWantToLoan} x {stock.Name} has been submitted.\nYou will receive an email when your loan request has been accepted/rejected.\n\nThank you.\nKind regards,\nIoT System.", false);

                db.RequestLoanStock.Add(requestLoanStock);
                db.SaveChanges();
                return RedirectToAction("Index", "RequestLoan");
            }

            return View(obj);
        }
    }
}