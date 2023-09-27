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
    }
}