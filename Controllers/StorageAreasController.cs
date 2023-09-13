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
    [SessionCheckerSuperUser]
    public class StorageAreasController : Controller
    {
        private IoTContext db = new IoTContext();


        public ActionResult Index()
        {
            var storageArea = db.StorageArea.Include(s => s.Room);
            return View(storageArea.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.RoomId = new SelectList(db.Room, "Id", "Room_Number");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StorageArea storageArea)
        {
            if (ModelState.IsValid)
            {
                storageArea.CreatedBy = (Session["User"] as Users).Id;
                db.StorageArea.Add(storageArea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoomId = new SelectList(db.Room, "Id", "Room_Number", storageArea.RoomId);
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
            ViewBag.RoomId = new SelectList(db.Room, "Id", "Room_Number", storageArea.RoomId);
            return View(storageArea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StorageArea storageArea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(storageArea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomId = new SelectList(db.Room, "Id", "Room_Number", storageArea.RoomId);
            return View(storageArea);
        }

    }
}
