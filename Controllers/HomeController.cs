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
            // Check if user is logged in already
            if (Session["User"] != null && Session["Login"] != null)
            {
                // Check what type of user and route to relevant index
                return RedirectToAction(nameof(About));
            }

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
                    // route to error page
                    return HttpNotFound();
                }

                // Check if person is locked
                if (!user.Access)
                {
                    // give error message that user is locked
                    return HttpNotFound();
                }

                // Check if user's password is correct
                if (!PasswordHandler.VerifyPassword(login.Password, user.Password))
                {
                    ModelState.AddModelError("Password", "Your Password is Incorrect");
                    return View();
                }

                // Add record to user tracking
                UserLoginTracking userLoginTracking = new UserLoginTracking
                {
                    UserId = user.Id,
                    UserLoginDateTime = DateTime.Now
                };
                db.UserLoginTracking.Add(userLoginTracking);
                db.SaveChanges();

                // Create a session for the user
                Session["User"] = user;
                Session["Login"] = userLoginTracking;

                return RedirectToAction(nameof(About));

            }

            return View(login);
        }

        public ActionResult Logout()
        {
            // Add logout time and duration logged in in seconds
            UserLoginTracking userLoginTracking = Session["Login"] as UserLoginTracking;
            userLoginTracking.UserLogoutDateTime = DateTime.Now;
            TimeSpan duration = userLoginTracking.UserLogoutDateTime.Value - userLoginTracking.UserLoginDateTime.Value;
            userLoginTracking.Duration = duration.TotalSeconds;
            db.Entry(userLoginTracking).State = EntityState.Modified;
            db.SaveChanges();

            // Dispose of sessions
            Session.Remove("Login");
            Session.Remove("User");

            return RedirectToAction(nameof(Index));
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