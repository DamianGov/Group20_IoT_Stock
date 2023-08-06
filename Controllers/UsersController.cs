using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Group20_IoT.Models;

namespace Group20_IoT.Controllers
{
    [SessionCheckerAdmin]
    public class UsersController : Controller
    {
        private IoTContext db = new IoTContext();

        public ActionResult Index()
        {
            var currentU = Session["User"] as Users;
            IQueryable<Users> users;


            // Check if the user is Super User if they are they can view all users, including Admins
            if (currentU.RoleId == db.Role.Single(r => r.Type.Equals("SuperUser")).Id)
                users = db.Users.Include(u => u.Role).Where(u => u.Id != currentU.Id);
            else
                // Otherwise we know this person is an Admin therefore they can only see standard users
                users = db.Users.Include(u => u.Role).Where(u => u.Id != currentU.Id && u.Role.Type == "Standard");

            return View(users.ToList());
        }

        [HttpPost]
        public ActionResult Restrict(int id)
        {
            Users users = db.Users.Find(id);

            if(users == null)
            {
                return HttpNotFound();
            }

            users.Access = false;
            db.Entry(users).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new {success = true});
        }

        [HttpPost]
        public ActionResult Unrestrict(int id)
        {
            Users users = db.Users.Find(id);

            if (users == null)
            {
                return HttpNotFound();
            }

            users.Access = true;
            db.Entry(users).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true});
        }

        public ActionResult Create()
        {
            Users currentUser = Session["User"] as Users;

            IQueryable<Role> role = db.Role.Where(r => !r.Type.Equals("SuperUser"));

            // Check if the user is Super User if they are they can create admin/standard users otherwise if normal Admin they can only add standard users
            if (currentUser.RoleId != db.Role.Single(r => r.Type.Equals("SuperUser")).Id)
                ViewBag.RoleId = new SelectList(role.Where(r => !r.Type.Equals("Admin")), "Id", "Type");
            else
                ViewBag.RoleId = new SelectList(role, "Id", "Type");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users users)
        {
            users.Email = users.Email.Trim();

            if(db.Users.Any(u => u.Email.Equals(users.Email)))
            {
                ModelState.AddModelError("Email", "A user with this email already exists");
            }


            if (ModelState.IsValid)
            {
                // Must hash password, so password is not plaintext in db
                users.Password = PasswordHandler.HashPassword(users.Password, PasswordHandler.GenerateSalt());
                users.CreatedBy = (Session["User"] as Users).Id;
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            Users currentUser = Session["User"] as Users;

            if (currentUser.RoleId != db.Role.Single(r => r.Type.Equals("SuperUser")).Id)
                ViewBag.RoleId = new SelectList(db.Role.Where(r => !r.Type.Equals("SuperUser") && !r.Type.Equals("Admin")), "Id", "Type");
            else
                ViewBag.RoleId = new SelectList(db.Role, "Id", "Type");

            return View(users);
        }

    
    }
}
