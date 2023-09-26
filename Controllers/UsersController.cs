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
    [SessionChecker("SuperAdmin","Admin")]
    public class UsersController : Controller
    {
        private IoTContext db = new IoTContext();

        public ActionResult Index()
        {
            var currentU = Session["User"] as Users;
            IQueryable<Users> users;

            // Check if the user is Super User if they are they can view all users, including Admins
            if (currentU.Role.Type == "SuperAdmin")
                users = db.Users.Include(u => u.Role).Where(u => u.Id != currentU.Id);
            else
                // Otherwise we know this person is an Admin therefore they can only see standard users
                users = db.Users.Include(u => u.Role).Where(u => u.Id != currentU.Id && u.Role.Type == "Member");

            return View(users.ToList());
        }

        [SessionChecker("SuperAdmin")]
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


                return Json(new { success = true, message = "User has been restricted" });
        }

        [SessionChecker("SuperAdmin")]
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

            return Json(new { success = true, message = "User has been unrestricted" });

        }

        public ActionResult Create()
        {
            Users currentUser = Session["User"] as Users;

            IQueryable<Role> role = db.Role.Where(r => !r.Type.Equals("SuperAdmin"));

            // Check if the user is Super User if they are they can create admin/standard users otherwise if normal Admin they can only add standard users
            if (currentUser.Role.Type != "SuperAdmin")
                ViewBag.RoleId = new SelectList(role.Where(r => !r.Type.Equals("Admin")), "Id", "Type");
            else
                ViewBag.RoleId = new SelectList(role, "Id", "Type");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Users users)
        {

            users.FirstName = users.FirstName.TrimStart().TrimEnd();
            users.Surname = users.Surname.TrimStart().TrimEnd();  
            users.Email = users.Email.Trim();

            if(db.Users.Any(u => u.Email.Equals(users.Email))) ModelState.AddModelError("Email", "A user with this email already exists");
           
            if (ModelState.IsValid)
            {
                
                Random random = new Random();
                string GeneratedPassword = users.FirstName[0].ToString().ToUpper() + "$" + ((users.FirstName.Length * random.Next(1, 6))) + users.Surname[0].ToString().ToLower() + random.Next('a', 'z' + 1) ;
                for (int i = 0; i < 3; i++)
                {
                    char randomAlphabeticChar = (char)random.Next('A', 'Z' + 1);
                    GeneratedPassword += randomAlphabeticChar;
                }
                // EMAIL THE PASSWORD TO USER
                // Must hash password, so password is not plaintext in db

                _ = Email.SendEmail(users.GetFullName(), users.Email, "Welcolme to the IoT System","Hello, "+users.GetFullName()+".\n\nYour account has been created.\nThis is your password: "+GeneratedPassword+"\n\nKind regards,\nIoT System.",false);

                users.Password = PasswordHandler.HashPassword(GeneratedPassword, PasswordHandler.GenerateSalt());
                users.CreatedBy = (Session["User"] as Users).Id;
                db.Users.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            Users currentUser = Session["User"] as Users;

            if (currentUser.Role.Type != "SuperAdmin")
                ViewBag.RoleId = new SelectList(db.Role.Where(r => !r.Type.Equals("SuperAdmin") && !r.Type.Equals("Admin")), "Id", "Type");
            else
                ViewBag.RoleId = new SelectList(db.Role, "Id", "Type");

            return View(users);
        }

        public ActionResult ManageUser(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            Users users = db.Users.Find(id);
            
            if (users==null) return RedirectToAction("Index");

            Users currentUser = Session["User"] as Users;

            IQueryable<Role> role = db.Role.Where(r => !r.Type.Equals("SuperAdmin"));

            // Check if the user is Super User if they are they can create admin/standard users otherwise if normal Admin they can only add standard users
            if (currentUser.Role.Type != "SuperAdmin")
                ViewBag.RoleId = new SelectList(role.Where(r => !r.Type.Equals("Admin")), "Id", "Type", users.RoleId);
            else
                ViewBag.RoleId = new SelectList(role, "Id", "Type", users.RoleId);

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageUser(Users users)
        {
            
            if(ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            Users currentUser = Session["User"] as Users;

            IQueryable<Role> role = db.Role.Where(r => !r.Type.Equals("SuperAdmin"));

            // Check if the user is Super User if they are they can create admin/standard users otherwise if normal Admin they can only add standard users
            if (currentUser.Role.Type != "SuperAdmin")
                ViewBag.RoleId = new SelectList(role.Where(r => !r.Type.Equals("Admin")), "Id", "Type", users.RoleId);
            else
                ViewBag.RoleId = new SelectList(role, "Id", "Type", users.RoleId);

            return View(users);
        }

    }
}
