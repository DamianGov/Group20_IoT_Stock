using Group20_IoT.Models;
using Group20_IoT.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group20_IoT.Controllers
{
    [SessionChecker("SuperAdmin","Admin")]
    public class WalkthroughController : Controller
    {
        private IoTContext db = new IoTContext();
        // GET: Walkthrough
        public ActionResult Index()
        {
            List<WalkthroughDisplayViewModel> walkthroughs = new List<WalkthroughDisplayViewModel>();

            var AllWalkthroughs = db.Walkthrough.OrderBy(w => w.WalkthroughDate).ToList();

            foreach(var walk in AllWalkthroughs)
            {
                var vm = new WalkthroughDisplayViewModel
                {
                    Walkthrough = walk,
                    Walkthrough_Stocks = db.Walkthrough_Stock.Include(w => w.Stock).Where(w => w.WalkthroughId == walk.Id).ToList()
                };
                walkthroughs.Add(vm);
            }

            return View(walkthroughs);
        }

        public ActionResult Create() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Walkthrough walkthrough)
        {
            
            if (ModelState.IsValid)
            {
                TempData["WalkthroughObject"] = walkthrough;
                return RedirectToAction(nameof(SelectStock));
            }

            return View(walkthrough);
        }
        
        public ActionResult SelectStock() 
        {
            Walkthrough walkthrough = TempData["WalkthroughObject"] as Walkthrough;

            if (walkthrough == null)
                return RedirectToAction(nameof(Index));


            var StockItems = db.Stock.Include(s => s.StorageArea).Include(s => s.StorageArea.Room).Include(s => s.Users).Where(s => (s.TotalQuantity - s.QuantityOnLoan) > 0).OrderBy(s => s.Name).Select(s => new StockItemsWalkthrough
            { 
            Id = s.Id,
            Stockcode = s.StockCode,
            Name = s.Name,
            QuantityAvailable = s.TotalQuantity - s.QuantityOnLoan
            }).ToList();

            WalkthroughStockViewModel viewModel = new WalkthroughStockViewModel { Walkthrough = walkthrough, Stocks = StockItems };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectStock(WalkthroughStockViewModel obj)
        {
            if (!obj.Stocks.Any(s => s.Selected)) ModelState.AddModelError(string.Empty, "Please select atleast one Stock");

            
            if (ModelState.IsValid)
            {
                obj.Walkthrough.CreatedBy = (Session["User"] as Users).Id;
                db.Walkthrough.Add(obj.Walkthrough);

                foreach (var item in obj.Stocks)
                {
                    if(item.Selected)
                    {
                        Walkthrough_Stock walkthrough_Stock = new Walkthrough_Stock
                        {
                            WalkthroughId = obj.Walkthrough.Id,
                            StockId = item.Id,
                            Quantity = item.Quantity,
                        };
                        Stock upStock = db.Stock.Find(item.Id);
                        upStock.QuantityOnLoan += item.Quantity;
                        db.Entry(upStock).State = EntityState.Modified;
                        db.Walkthrough_Stock.Add(walkthrough_Stock);
                    }
                }
                db.SaveChanges();
                return RedirectToAction(nameof(Index));

            }

            var StockItems = db.Stock.Include(s => s.StorageArea).Include(s => s.StorageArea.Room).Include(s => s.Users).Where(s => (s.TotalQuantity - s.QuantityOnLoan) > 0).OrderBy(s => s.Name).Select(s => new StockItemsWalkthrough
            {
                Id = s.Id,
                Stockcode = s.StockCode,
                Name = s.Name,
                QuantityAvailable = s.TotalQuantity - s.QuantityOnLoan
            }).ToList();

            obj.Stocks = StockItems;
            return View(obj);
        }

        [HttpPost]
        public ActionResult FreeStock(int? id)
        {
            if (id == null) return RedirectToAction(nameof(Index));

            Walkthrough walkthrough = db.Walkthrough.Find(id);

            if (walkthrough == null) return RedirectToAction(nameof(Index));

            walkthrough.StockStillAllocated = false;

            List<Walkthrough_Stock> lstStock = db.Walkthrough_Stock.Where(w => w.WalkthroughId == id).ToList();

            foreach(var item in lstStock)
            {
                Stock stock = db.Stock.Find(item.StockId);
                stock.QuantityOnLoan -= item.Quantity;
                db.Entry(stock).State = EntityState.Modified;
            }

            db.Entry(walkthrough).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true, message = "Stock has been freed" });
        }
    }
}