using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Group20_IoT.Models;
using Microsoft.IdentityModel.Tokens;

namespace Group20_IoT.Controllers
{
    [SessionChecker("SuperAdmin","Admin")]
    public class DiscrepancyController : Controller
    {
        private IoTContext db = new IoTContext();

        // GET: Discrepancy
        public ActionResult Index()
        {
            var discrepancyStock = db.StockDiscrepancy.Include(d => d.Stock).Include(d => d.Users).OrderByDescending(d =>d.CreationDate);
            return View(discrepancyStock.ToList());
        }


        // GET: DeveStocks/Create
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
        public ActionResult Create(StockDiscrepancy discrepancyStock)
        {
            Users user = Session["User"] as Users;

            if (discrepancyStock.StockId > 0)
            {
                var StockRec = db.Stock.Find(discrepancyStock.StockId);
                if (discrepancyStock.Quantity > (StockRec.TotalQuantity - StockRec.QuantityOnLoan))
                    ModelState.AddModelError("Quantity", "The Quantity exceeds the amount of Stock available for this item");
            }
            else
                ModelState.AddModelError("StockId", "Please select Stock");

            if(discrepancyStock.Note.IsNullOrEmpty())
                ModelState.AddModelError("Note", "Please comment on the Discrepancy");

            if (ModelState.IsValid)
            {
               
                discrepancyStock.CreatedBy = user.Id;

                var stock = db.Stock.Find(discrepancyStock.StockId);
                stock.TotalQuantity -= discrepancyStock.Quantity;
                db.Entry(stock).State = EntityState.Modified;
                db.StockDiscrepancy.Add(discrepancyStock);
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

            return View(discrepancyStock);
        }

        [HttpPost]
        public ActionResult ResolveDefect(int? id)
        {
            if (id == null) return HttpNotFound();

            var DefStock = db.StockDiscrepancy.Find(id);

            if (DefStock == null) return HttpNotFound();

            DefStock.Resolved = true;

            DefStock.ResolvedDate = DateTime.Now;

            var Stock = db.Stock.Find(DefStock.StockId);

            if (Stock == null) return HttpNotFound();

            Stock.TotalQuantity += DefStock.Quantity;

            db.Entry(DefStock).State = EntityState.Modified;
            db.Entry(Stock).State = EntityState.Modified;
            db.SaveChanges();

            // Return to a different view or something?
            return Json(new { success = true, message = "Discrepancy has been resolved" });
        }

    }
}
