using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Group20_IoT.Models;
using Group20_IoT.Models.ViewModel;
using Microsoft.IdentityModel.Tokens;

namespace Group20_IoT.Controllers
{
    [SessionChecker("SuperAdmin")]
    public class StorageAreasController : Controller
    {
        private IoTContext db = new IoTContext();

        public ActionResult Index()
        {
            var storageArea = db.StorageArea.Include(s => s.Room).AsEnumerable().OrderBy(s => s.Room.Room_Number).Select(s=> new StorageAreaViewModel
            {
                Id = s.Id,
                Area_Name = s.Area_Name,
                Active = s.Active ? "Yes" : "No",
                RoomName = $"{s.Room.Room_Number} [{s.Room.Room_Description}]"
            }).ToList();
            return View(storageArea);
        }

        public ActionResult Create()
        {
            var rooms = db.Room.Where(r => r.Active).ToList().OrderBy(r => r.Room_Number).Select(r => new
            {
                r.Id,
                RoomDetails = $"{r.Room_Number} [{r.Room_Description}]"
            }).ToList();

            if (rooms.Any())
                ViewBag.RoomId = new SelectList(rooms, "Id", "RoomDetails");
            else
                ViewBag.RoomId = new SelectList(new List<SelectListItem>{
                                 new SelectListItem { Value = "-1", Text = "No Rooms Available" }}, "Value", "Text");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StorageArea storageArea)
        {
            if (!storageArea.Area_Name.IsNullOrEmpty() && storageArea.RoomId != -1 )
            {
                string sa = storageArea.Area_Name.TrimStart().TrimEnd();
                var StorageA = db.StorageArea.Where(s => s.Area_Name.Contains(sa) && s.RoomId == storageArea.RoomId);
                if (StorageA.Any()) ModelState.AddModelError("Area_Name", "A Storage Area in this room already exists with this area name");
            }

            if (storageArea.RoomId == -1)
                ModelState.AddModelError("RoomId", "A Room must be selected to assign the Storage Area to");

            if (ModelState.IsValid)
            {
                storageArea.CreatedBy = (Session["User"] as Users).Id;
                db.StorageArea.Add(storageArea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var rooms = db.Room.Where(r => r.Active).ToList().OrderBy(r => r.Room_Number).Select(r => new
            {
                r.Id,
                RoomDetails = $"{r.Room_Number} [{r.Room_Description}]"
            }).ToList();

            if (rooms.Any())
                ViewBag.RoomId = new SelectList(rooms, "Id", "RoomDetails");
            else
                ViewBag.RoomId = new SelectList(new List<SelectListItem>{
                                 new SelectListItem { Value = "-1", Text = "No Rooms Available" }}, "Value", "Text");
            return View(storageArea);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StorageArea storageArea = db.StorageArea.Find(id);
            if (storageArea == null)
            {
                return HttpNotFound();
            }

            int RoomId = storageArea.RoomId;

            var rooms = db.Room.Where(r => r.Active || RoomId == r.Id).ToList().OrderBy(r => r.Room_Number).Select(r => new
            {
                r.Id,
                RoomDetails = $"{r.Room_Number} [{r.Room_Description}]"
            }).ToList();

            if (rooms.Any())
                ViewBag.RoomId = new SelectList(rooms, "Id", "RoomDetails", RoomId);
            else
                ViewBag.RoomId = new SelectList(new List<SelectListItem>{
                                 new SelectListItem { Value = "-1", Text = "No Rooms Available" }}, "Value", "Text");
            return View(storageArea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StorageArea storageArea)
        {
            if (!storageArea.Area_Name.IsNullOrEmpty() && storageArea.RoomId != -1)
            {
                string sa = storageArea.Area_Name.TrimStart().TrimEnd();
                var StorageA = db.StorageArea.Where(s => s.Area_Name.Contains(sa) && s.RoomId == storageArea.RoomId && s.Id != storageArea.Id);
                if (StorageA.Any()) ModelState.AddModelError("Area_Name", "A Storage Area in this room already exists with this area name");
            }


            if (ModelState.IsValid)
            {
                db.Entry(storageArea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            int RoomId = storageArea.RoomId;

            var rooms = db.Room.Where(r => r.Active || RoomId == r.Id).ToList().OrderBy(r => r.Room_Number).Select(r => new
            {
                r.Id,
                RoomDetails = $"{r.Room_Number} [{r.Room_Description}]"
            }).ToList();

            if (rooms.Any())
                ViewBag.RoomId = new SelectList(rooms, "Id", "RoomDetails", RoomId);
            else
                ViewBag.RoomId = new SelectList(new List<SelectListItem>{
                                 new SelectListItem { Value = "-1", Text = "No Rooms Available" }}, "Value", "Text");
            return View(storageArea);
        }

    }
}
