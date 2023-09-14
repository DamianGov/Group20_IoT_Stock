using Group20_IoT.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group20_IoT.Controllers
{
    public class LoginController : Controller
    {
        private IoTContext db = new IoTContext();

        public ActionResult Index()
        {
            // Check if user is logged in already
            if (Session["User"] != null && Session["Login"] != null)
            {
                Users user = Session["User"] as Users;
                // Check what type of user and route to relevant index
                if (user.RoleId.Equals(db.Role.Single(r => r.Type == "SuperUser").Id))
                    return RedirectToAction("SuperHome", "Home");
                else if (user.RoleId.Equals(db.Role.Single(r => r.Type == "Admin").Id))
                    return RedirectToAction("AdminHome", "Home");
                else if (user.RoleId.Equals(db.Role.Single(r => r.Type == "Standard").Id))
                    return RedirectToAction("StandardHome", "Home");
                else
                    // CODE THIS PAGE
                    return HttpNotFound();
            }

            var RememberMeCookie = Request.Cookies["RememberMeCookie"];
            var UserTrackingCookie = Request.Cookies["UserTrackingCookie"];

            if (RememberMeCookie != null && UserTrackingCookie != null)
            {
                var userId = int.Parse(RememberMeCookie.Value);
                var userTrackingId = int.Parse(UserTrackingCookie.Value);

                Users user = db.Users.Find(userId);
                UserLoginTracking userLoginTracking = db.UserLoginTracking.Find(userTrackingId);

                if (user != null && userLoginTracking != null)
                {
                    Session["User"] = user;
                    Session["Login"] = userLoginTracking;
                    return Index();
                }

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
                    login.Password = "";
                    return View(login);
                }

                // Add record to user tracking
                UserLoginTracking userLoginTracking = new UserLoginTracking
                {
                    UserId = user.Id,
                    UserLoginDateTime = DateTime.Now
                };
                db.UserLoginTracking.Add(userLoginTracking);
                db.SaveChanges();

                if (login.RememberMe)
                {
                    var RememberMeCookie = new HttpCookie("RememberMeCookie")
                    {
                        Value = user.Id.ToString(),
                        Expires = DateTime.Now.AddMonths(1),
                        HttpOnly = true
                    };

                    var UserTrackingCookie = new HttpCookie("UserTrackingCookie")
                    {
                        Value = userLoginTracking.Id.ToString(),
                        Expires = DateTime.Now.AddMonths(1),
                        HttpOnly = true
                    };

                    Response.Cookies.Add(RememberMeCookie);
                    Response.Cookies.Add(UserTrackingCookie);
                }

                // Create a session for the user
                Session["User"] = user;
                Session["Login"] = userLoginTracking;


                if (user.RoleId.Equals(db.Role.Single(r => r.Type == "SuperUser").Id))
                    return RedirectToAction("SuperHome", "Home");
                else if (user.RoleId.Equals(db.Role.Single(r => r.Type == "Admin").Id))
                    return RedirectToAction("AdminHome", "Home");
                else if (user.RoleId.Equals(db.Role.Single(r => r.Type == "Standard").Id))
                    return RedirectToAction("StandardHome", "Home");
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


            var RememberMeCookie = new HttpCookie("RememberMeCookie")
            {
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true
            };

            var UserTrackingCookie = new HttpCookie("UserTrackingCookie")
            {
                Expires = DateTime.Now.AddDays(-1),
                HttpOnly = true
            };

            Response.Cookies.Add(RememberMeCookie);
            Response.Cookies.Add(UserTrackingCookie);

            // Dispose of sessions
            Session.Remove("Login");
            Session.Remove("User");

            return RedirectToAction(nameof(Index));
        }
    }
}