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
    public class StorageAreasController : Controller
    {
        private IoTContext db = new IoTContext();

        // GET: StorageAreas
        public ActionResult Index()
        {
            var storageArea = db.StorageArea.Include(s => s.Room);
            return View(storageArea.ToList());
        }

        // GET: StorageAreas/Details/5
        public ActionResult Details(int? id)
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
            return View(storageArea);
        }

        // GET: StorageAreas/Create
        public ActionResult Create()
        {
            ViewBag.RoomId = new SelectList(db.Room, "Id", "Room_Number");
            return View();
        }

        // POST: StorageAreas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Area_Name,RoomId")] StorageArea storageArea)
        {
            if (ModelState.IsValid)
            {
                db.StorageArea.Add(storageArea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoomId = new SelectList(db.Room, "Id", "Room_Number", storageArea.RoomId);
            return View(storageArea);
        }

        // GET: StorageAreas/Edit/5
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

        // POST: StorageAreas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Area_Name,RoomId")] StorageArea storageArea)
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

        // GET: StorageAreas/Delete/5
        public ActionResult Delete(int? id)
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
            return View(storageArea);
        }

        // POST: StorageAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StorageArea storageArea = db.StorageArea.Find(id);
            db.StorageArea.Remove(storageArea);
            db.SaveChanges();
            return RedirectToAction("Index");
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
