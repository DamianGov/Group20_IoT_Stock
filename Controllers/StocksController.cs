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
using Microsoft.IdentityModel.Tokens;

namespace Group20_IoT.Controllers
{
    [SessionChecker("SuperAdmin","Admin")]
    public class StocksController : Controller
    {
        private IoTContext db = new IoTContext();

        public ActionResult Index()
        {
            // Get stock and format room so user knows which room they are assigned to
            var stock =  db.Stock.Include(s => s.StorageArea).Include(s=>s.StorageArea.Room).Include(s=>s.Users).AsEnumerable().Select(
                s => new StockViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    TotalQuantity = s.TotalQuantity,
                    UnitPrice = s.UnitPrice.ToString("C"),
                    QuantityOnLoan = s.QuantityOnLoan,
                    StorageArea = s.StorageArea.Area_Name,
                    Image = s.ImageFile,
                    StockCode =  s.StockCode,
                    Loanable = s.Loanable ? "Yes" : "No",
                    Room = $"{s.StorageArea.Room.Room_Number} [{s.StorageArea.Room.Room_Description}]",
                    CreatedBy = s.Users.GetFullName(),
                    CreatedOn = s.CreatedOn.ToString("yyyy/M/d")
                }).ToList();

            return View(stock);
        }

        public ActionResult Search(string searchItem)
        {
            var stock = db.Stock.Include(s => s.StorageArea).Include(s => s.StorageArea.Room).Include(s=>s.Users).AsEnumerable().Where(s => s.StockCode.ToLower().Contains(searchItem.Trim().ToLower())).Select(
                s => new StockViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    TotalQuantity = s.TotalQuantity,
                    UnitPrice = s.UnitPrice.ToString("C"),
                    QuantityOnLoan = s.QuantityOnLoan,
                    StorageArea = s.StorageArea.Area_Name,
                    Image = s.ImageFile,
                    StockCode = s.StockCode,
                    Loanable = s.Loanable ? "Yes" : "No",
                    Room = $"{s.StorageArea.Room.Room_Number} [{s.StorageArea.Room.Room_Description}]",
                    CreatedBy = s.Users.GetFullName(),
                    CreatedOn = s.CreatedOn.ToString("yyyy/M/d")
                }).ToList();

            return PartialView("_StockTable", stock);
        }

        public ActionResult Create()
        {
            // Retrieve distinct roomids for all storage areas (so we know which rooms actually have storage areas)
            var roomWithStorageAreas = db.StorageArea.Where(s => s.Active).Select(s => s.RoomId).Distinct().ToList();
            
            // Only gather details of rooms that have storage areas
            var rooms = db.Room.Where(r => roomWithStorageAreas.Contains(r.Id) && r.Active).ToList().Select(r => new
            {
                r.Id,
                RoomDetails = $"{r.Room_Number} [{r.Room_Description}]"
            }).ToList();

            if(rooms.Any())
                ViewBag.Rooms = new SelectList(rooms, "Id", "RoomDetails");
            else
                ViewBag.Rooms = new SelectList(new List<SelectListItem>{
                                 new SelectListItem { Value = "-1", Text = "No Rooms Available" }}, "Value", "Text");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Stock stock, HttpPostedFileBase image)
        {
            // Get logged in user
            Users user = Session["User"] as Users;

            // Remove white spaces at end of stock code
            if (!stock.StockCode.IsNullOrEmpty())
                stock.StockCode = stock.StockCode.TrimStart().TrimEnd();

            if (stock.StockCode.Contains(" "))
                ModelState.AddModelError("StockCode", "The Stock Code cannot have spaces");

            // Remove white spaces at end of description
            if (!stock.Description.IsNullOrEmpty())
                stock.Description = stock.Description.TrimStart().TrimEnd();

            // Check if there is any stock with the same stock code (prevent clashes)
            if (db.Stock.Any(s => s.StockCode.ToLower() == stock.StockCode.ToLower())) ModelState.AddModelError("StockCode", "This Stock Code already exists for another item");

            if (stock.StorageAreaId == -1) ModelState.AddModelError("StorageArea", "A Storage Area must be chosen to assign the stock to");

            // Check if any image is selected
            if (image == null || image.ContentLength <= 0) ModelState.AddModelError("ImageFile", "Please select an image");

            

            if (ModelState.IsValid)
            {
                // Special GUID for image
                string ImageGUID = Guid.NewGuid().ToString() + "_" + stock.StockCode + Path.GetExtension(image.FileName);
                string ImagePath = Path.Combine(Server.MapPath("~/Content/Images"), ImageGUID);

                image.SaveAs(ImagePath);

                // User that created this stock item
                stock.CreatedBy = user.Id;
                stock.ImageFile = ImageGUID;
                db.Stock.Add(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var roomWithStorageAreas = db.StorageArea.Where(s => s.Active).Select(s => s.RoomId).Distinct().ToList();

            var rooms = db.Room.Where(r => roomWithStorageAreas.Contains(r.Id) && r.Active).ToList().Select(r => new
            {
                r.Id,
                RoomDetails = $"{r.Room_Number} [{r.Room_Description}]"
            }).ToList();

            if (rooms.Any())
                ViewBag.Rooms = new SelectList(rooms, "Id", "RoomDetails");
            else
                ViewBag.Rooms = new SelectList(new List<SelectListItem>{
                                 new SelectListItem { Value = "-1", Text = "No Rooms Available" }}, "Value", "Text");


            return View(stock);
        }

        public ActionResult GetStorageAreasByRoom(int roomId)
        {
            // Get all storage areas for specific room
            var storageAreas = db.StorageArea
                .Where(sa => sa.RoomId == roomId && sa.Active)
                .Select(sa => new
                {
                    Value = sa.Id,
                    Text = sa.Area_Name
                })
                .ToList();

            return Json(storageAreas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStorageAreasByRoomEdit(int roomId, int stockId)
        {
            Stock stock = db.Stock.Include(s => s.StorageArea.Room).SingleOrDefault(s => s.Id == stockId);
            // Get all storage areas for specific room
            var storageAreas = db.StorageArea
                .Where(sa => sa.RoomId == roomId && (sa.Active || stock.StorageAreaId == sa.Id))
                .Select(sa => new
                {
                    Value = sa.Id,
                    Text = sa.Area_Name
                })
                .ToList();

            return Json(storageAreas, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stock.Include(s => s.StorageArea).Include(s => s.StorageArea.Room).SingleOrDefault(s => s.Id == id.Value);
            if (stock == null)
            {
                return HttpNotFound();
            }
            var roomWithStorageAreas = db.StorageArea.Where(s=>s.Active || stock.StorageAreaId == s.Id).Select(s => s.RoomId).Distinct().ToList();

            var rooms = db.Room.Where(r => roomWithStorageAreas.Contains(r.Id) && (r.Active || stock.StorageArea.RoomId == r.Id)).ToList().Select(r => new
            {
                r.Id,
                RoomDetails = $"{r.Room_Number} [{r.Room_Description}]"
            }).ToList();

            ViewBag.Rooms = new SelectList(rooms, "Id", "RoomDetails",stock.StorageArea.RoomId);


            return View(stock);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Stock stock, HttpPostedFileBase newImage)
        {
            // Remove white spaces at end of description
            if (!stock.Description.IsNullOrEmpty())
                stock.Description = stock.Description.TrimStart().TrimEnd();

            if (stock.QuantityOnLoan > stock.TotalQuantity) ModelState.AddModelError("TotalQuantity", "The Total Quantity cannot be less than the Quantity that is on loan");
            if (ModelState.IsValid)
            {
                if (newImage != null && newImage.ContentLength > 0)
                {
                    string ImageGUID = Guid.NewGuid().ToString() + "_" + stock.StockCode + Path.GetExtension(newImage.FileName);
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
            var entry = db.Entry(stock);

            if(entry.State == EntityState.Detached)
            {
                db.Stock.Attach(stock);
            }

            db.Entry(stock).Collection(s => s.storageAreas).Load();

            int RoomId = db.StorageArea.Where(s => s.Id == stock.StorageAreaId).First().RoomId;

            var roomWithStorageAreas = db.StorageArea.Where(s => s.Active || stock.StorageAreaId == s.Id).Select(s => s.RoomId).Distinct().ToList();

            var rooms = db.Room.Where(r => roomWithStorageAreas.Contains(r.Id) && (r.Active || RoomId == r.Id)).ToList().Select(r => new
            {
                r.Id,
                RoomDetails = $"{r.Room_Number} [{r.Room_Description}]"
            }).ToList();

            ViewBag.Rooms = new SelectList(rooms, "Id", "RoomDetails", RoomId);
            return View(stock);
        }

        

    }
}
