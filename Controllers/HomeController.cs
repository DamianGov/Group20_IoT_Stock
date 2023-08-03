using Group20_IoT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Login = Group20_IoT.Models.Login;

namespace Group20_IoT.Controllers
{
    public class HomeController : Controller
    {
        private IoTContext db = new IoTContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Login login)
        {
            if (ModelState.IsValid)
            {
                Users user = login.UserExists();

                // If user doesnt exist
                if (user == null)
                {
                    return HttpNotFound();
                }

                // Check if person is locked
                if (!user.Access)
                {
                    return HttpNotFound();
                }

                //HashPassword
                //string salt = PasswordHandler.GenerateSalt();
                //user.Password = PasswordHandler.HashPassword("Damian123", salt);

                //db.Entry(user).State = EntityState.Modified;
                //db.SaveChanges();

                // Check if user entered the correct password
                if (!PasswordHandler.VerifyPassword(login.Password, user.Password))
                {
                    ModelState.AddModelError("Password", "Your Password is Incorrect");
                    return View();
                }

                // Create a session for the user
                Session["User"] = user;

                return View(nameof(About));

            }

            return View(login);
        }

        public ActionResult Logout()
        {
            Session.Remove("User");
            return View(nameof(Index));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}