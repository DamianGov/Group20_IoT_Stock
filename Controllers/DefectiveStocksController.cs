using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Group20_IoT.Models;

namespace Group20_IoT.Controllers
{
    [SessionCheckerAdmin]
    public class DefectiveStocksController : Controller
    {
        private IoTContext db = new IoTContext();

        // GET: DefectiveStocks
        public ActionResult Index()
        {
            var defectiveStock = db.DefectiveStock.Include(d => d.Stock).Include(d => d.Users).OrderByDescending(d =>d.CreationDate);
            return View(defectiveStock.ToList());
        }


        // GET: DefectiveStocks/Create
        public ActionResult Create()
        {
            var StockWithQuantity = db.Stock.Where(s => (s.TotalQuantity - s.QuantityOnLoan) > 0).ToList().Select(s => new
            { 
                Value = s.Id,
                Text = $"{s.StockCode} [{s.Name}]"
            }).ToList();

            if(StockWithQuantity.Any())
                ViewBag.Stocks = new SelectList(StockWithQuantity, "Value", "Text");
            else
                ViewBag.Stocks = new SelectList(new List<SelectListItem>{
                                 new SelectListItem { Value = "-1", Text = "No Stock Available" }}, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DefectiveStock defectiveStock)
        {
            Users user = Session["User"] as Users;

            if (defectiveStock.StockId > 0)
            {
                var StockRec = db.Stock.Find(defectiveStock.StockId);
                if (defectiveStock.Quantity > (StockRec.TotalQuantity - StockRec.QuantityOnLoan))
                    ModelState.AddModelError("Quantity", "The Quantity exceeds the amount of Stock available for this item");
            }
            else
                ModelState.AddModelError("StockId", "Please select the Stock");

            if (ModelState.IsValid)
            {
               
                defectiveStock.CreatedBy = user.Id;

                var stock = db.Stock.Find(defectiveStock.StockId);
                stock.TotalQuantity -= defectiveStock.Quantity;
                db.Entry(stock).State = EntityState.Modified;
                db.DefectiveStock.Add(defectiveStock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var StockWithQuantity = db.Stock.Where(s => (s.TotalQuantity - s.QuantityOnLoan) > 0).ToList().Select(s => new
            {
                Value = s.Id,
                Text = $"{s.StockCode} [{s.Name}]"
            }).ToList();

            if (StockWithQuantity.Any())
                ViewBag.Stocks = new SelectList(StockWithQuantity, "Value", "Text");
            else
                ViewBag.Stocks = new SelectList(new List<SelectListItem>{
                                 new SelectListItem { Value = "-1", Text = "No Stock Available" }}, "Value", "Text");

            return View(defectiveStock);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
