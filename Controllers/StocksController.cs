using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Group20_IoT.Models;
using Group20_IoT.Models.ViewModel;
using System.IO;

namespace Group20_IoT.Controllers
{
    // NOT ROUTING
    [SessionChecker]
    public class StocksController : Controller
    {
        private IoTContext db = new IoTContext();

        public ActionResult Index()
        {
            // Get stock and format room so user knows which room they are assigned to
            var stock =  db.Stock.Include(s => s.StorageArea).Include(s=>s.StorageArea.Room).ToList().Select(
                s => new StockViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    TotalQuantity = s.TotalQuantity,
                    QuantityOnLoan = s.QuantityOnLoan,
                    StorageArea = s.StorageArea.Area_Name,
                    Image = s.ImageFile,
                    StockCode =  s.StockCode,
                    Loanable = s.Loanable ? "Yes" : "No",
                    Room = $"{s.StorageArea.Room.Room_Number} [{s.StorageArea.Room.Room_Description}]"
                }).ToList();

            return View(stock);
        }

        public ActionResult Search(string searchItem)
        {
            var stock = db.Stock.Include(s => s.StorageArea).Include(s => s.StorageArea.Room).ToList().Where(s => s.StockCode.ToLower().Contains(searchItem.Trim().ToLower())).Select(
                s => new StockViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    TotalQuantity = s.TotalQuantity,
                    QuantityOnLoan = s.QuantityOnLoan,
                    StorageArea = s.StorageArea.Area_Name,
                    Image = s.ImageFile,
                    StockCode = s.StockCode,
                    Loanable = s.Loanable ? "Yes" : "No",
                    Room = $"{s.StorageArea.Room.Room_Number} [{s.StorageArea.Room.Room_Description}]"
                }).ToList();

            return PartialView("_StockTable", stock);
        }


        [SessionCheckerAdmin]
        public ActionResult Create()
        {
            // Retrieve distinct roomids for all storage areas (so we know which rooms actually have storage areas)
            var roomWithStorageAreas = db.StorageArea.Select(s => s.RoomId).Distinct().ToList();
            
            // Only gather details of rooms that have storage areas
            var rooms = db.Room.Where(r => roomWithStorageAreas.Contains(r.Id) && r.Active).ToList().Select(r => new
            {
                r.Id,
                RoomDetails = $"{r.Room_Number} [{r.Room_Description}]"
            }).ToList();

            ViewBag.Rooms = new SelectList(rooms, "Id", "RoomDetails");
            return View();
        }

        [SessionCheckerAdmin]
        public ActionResult GetStorageAreasByRoom(int roomId)
        {
            // Get all storage areas for specific room
            var storageAreas = db.StorageArea
                .Where(sa => sa.RoomId == roomId)
                .Select(sa => new
                {
                    Value = sa.Id,
                    Text = sa.Area_Name
                })
                .ToList();

            return Json(storageAreas, JsonRequestBehavior.AllowGet);
        }

        [SessionCheckerAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Stock stock, HttpPostedFileBase image)
        {
            // Get logged in user
            Users user = Session["User"] as Users;
            //stock.UserId = user.Id;

            //stock.DateCreated = DateTime.Now.Date;

            // Remove white spaces at end of stock code
            stock.StockCode = stock.StockCode.Trim();

            // Check if there is any stock with the same stock code (prevent clashes)
            if (db.Stock.Any(s => s.StockCode == stock.StockCode)) ModelState.AddModelError("StockCode","This Stock Code already exists for another item");


            // Check if any image is selected
            if (image == null || image.ContentLength <= 0) ModelState.AddModelError("ImageFile", "Please select an image");

            if (ModelState.IsValid)
            {
                // Special GUID for image
                string ImageGUID = Guid.NewGuid().ToString() + "_" +Path.GetFileName(image.FileName);
                string ImagePath = Path.Combine(Server.MapPath("~/Content/Images"), ImageGUID);

                image.SaveAs(ImagePath);

                // User that created this stock item
                stock.CreatedBy = user.Id;
                stock.ImageFile = ImageGUID;
                db.Stock.Add(stock);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            var roomWithStorageAreas = db.StorageArea.Select(s => s.RoomId).Distinct().ToList();

            var rooms = db.Room.Where(r => roomWithStorageAreas.Contains(r.Id) && r.Active).ToList().Select(r => new
            {
                r.Id,
                RoomDetails = $"{r.Room_Number} [{r.Room_Description}]"
            }).ToList();

            ViewBag.Rooms = new SelectList(rooms, "Id", "RoomDetails");

            return View(stock);
        }

        [SessionCheckerAdmin]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stock.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            var roomWithStorageAreas = db.StorageArea.Select(s => s.RoomId).Distinct().ToList();

            var rooms = db.Room.Where(r => roomWithStorageAreas.Contains(r.Id) && r.Active).ToList().Select(r => new
            {
                r.Id,
                RoomDetails = $"{r.Room_Number} [{r.Room_Description}]"
            }).ToList();

            ViewBag.Rooms = new SelectList(rooms, "Id", "RoomDetails");
            return View(stock);
        }

        [SessionCheckerAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Stock stock, HttpPostedFileBase newImage)
        {
            if (ModelState.IsValid)
            {
                if (newImage != null && newImage.ContentLength > 0)
                {
                    string ImageGUID = Guid.NewGuid().ToString() + "_" + Path.GetFileName(newImage.FileName);
                    string ImagePath = Path.Combine(Server.MapPath("~/Content/Images"), ImageGUID);

                    newImage.SaveAs(ImagePath);

                    if (stock.ImageFile != null)
                    {
                        string OldImage = Path.Combine(Server.MapPath("~/Content/Images"), stock.ImageFile);
                        if (System.IO.File.Exists(OldImage)) System.IO.File.Delete(OldImage);
                    }

                    stock.ImageFile = ImageGUID;
                }

                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var roomWithStorageAreas = db.StorageArea.Select(s => s.RoomId).Distinct().ToList();

            var rooms = db.Room.Where(r => roomWithStorageAreas.Contains(r.Id) && r.Active).ToList().Select(r => new
            {
                r.Id,
                RoomDetails = $"{r.Room_Number} [{r.Room_Description}]"
            }).ToList();

            ViewBag.Rooms = new SelectList(rooms, "Id", "RoomDetails");
            return View(stock);
        }

        

    }
}
