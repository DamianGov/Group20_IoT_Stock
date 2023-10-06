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
    [SessionChecker("SuperAdmin")]
    public class RoomsController : Controller
    {
        private IoTContext db = new IoTContext();

        public ActionResult Index()
        {
            return View(db.Room.OrderBy(r => r.Room_Number).ToList());
        }


        public ActionResult Create()
        {
            // Set Room to active automatically
            Room room = new Room
            {
                Active = true
            };
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Room room)
        {
            if(!room.Room_Number.IsNullOrEmpty())
            {
                string rm = room.Room_Number.TrimStart().TrimEnd();
                var RoomExist = db.Room.Where(r => r.Room_Number.Contains(rm));
                if (RoomExist.Any()) ModelState.AddModelError("Room_Number", "A Room with this room number already exists");
            }

            if (ModelState.IsValid)
            {
                // Record who created this room
                room.CreatedBy = (Session["User"] as Users).Id;
                db.Room.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(room);
        }

        // Need to sort this out
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Room.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Room room)
        {
            if (!room.Room_Number.IsNullOrEmpty())
            {
                var RoomExist = db.Room.Where(r => r.Room_Number.Contains(room.Room_Number) && r.Id != room.Id);
                if (RoomExist.Any()) ModelState.AddModelError("Room_Number", "A Room with this room number already exists");
            }

            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(room);
        }

    }
}
