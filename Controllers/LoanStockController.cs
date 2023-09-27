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
                    UserId = (Session["User"] as Users).Id
                };

                // Send Email

                db.RequestLoanStock.Add(requestLoanStock);
                db.SaveChanges();
                return RedirectToAction("Index", "RequestLoan");
            }

            return View(obj);
        }
    }
}