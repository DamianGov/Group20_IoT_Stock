using Group20_IoT.Models;
using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Group20_IoT.Controllers
{
    public class LoginController : Controller
    {
        private IoTContext db = new IoTContext();

        public async Task<ActionResult> Index()
        {

            if (Session["User"] == null)
            {
                var authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie != null)
                {
                    // Decrypt the authentication ticket
                    var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                    if (authTicket != null && !authTicket.Expired)
                    {
                        // User Id and Tracking Id
                        int UserId = int.Parse(authTicket.Name);
                        int UserTrackingId = int.Parse(authTicket.UserData);

                        Users user = await db.Users.Include(u => u.Role).Where(u => u.Id == UserId).FirstOrDefaultAsync();
                        UserLoginTracking userLoginTracking = await db.UserLoginTracking.FindAsync(UserTrackingId);
                        if (user != null && userLoginTracking != null)
                        {
                            Session["User"] = user;
                            Session["Login"] = userLoginTracking;
                            Session["UserType"] = user.Role.Type;

                            if (user.Role.Type == "SuperAdmin" || user.Role.Type == "Admin")
                                return RedirectToAction(nameof(Index), "Stocks");
                            else
                                return RedirectToAction(nameof(Index), "LoanStock");
                        }
                    }

                } 
            }
            return View();
        }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<ActionResult> Index(Login login)
            {
                if (ModelState.IsValid)
                {
                    Users user = SharedMethods.UserExists(login.Email);

                    // If user doesnt exist
                    if (user == null)
                    {
                    ModelState.AddModelError("Email", "User does not exist");
                    login.Email = "";
                    login.Password = "";
                    return View(login);
                }

                    // Check if person is locked
                    if (!user.Access)
                    {
                    ModelState.AddModelError("Email", "Access is Restricted");
                    login.Password = "";
                    return View(login);
                }

                    // Check if user's password is correct
                    if (!SharedMethods.VerifyPassword(login.Password, user.Password))
                    {
                        ModelState.AddModelError("Password", "Your Password is Incorrect");
                        login.Password = "";
                        return View(login);
                    }

                    // Add record to user tracking
                    UserLoginTracking userLoginTracking = new UserLoginTracking
                    {
                        UserId = user.Id,
                        UserLoginDateTime = DateTime.Now,
                        UsedRememberMe = login.RememberMe
                    };
                    db.UserLoginTracking.Add(userLoginTracking);
                    await db.SaveChangesAsync();

                    if (login.RememberMe)
                    {
                        var authTicket = new FormsAuthenticationTicket(
                            1,
                            user.Id.ToString(),
                            DateTime.Now,
                            DateTime.Now.AddDays(7),
                            true,
                            userData: userLoginTracking.Id.ToString());

                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                        {
                            Expires = authTicket.Expiration,
                            HttpOnly = true
                        };

                        Response.Cookies.Add(cookie);

                    }

                    // Create a session for the user
                    Session["User"] = user;
                    Session["Login"] = userLoginTracking;
                    Session["UserType"] = user.Role.Type;


                if (user.Role.Type == "SuperAdmin" || user.Role.Type == "Admin")
                    return RedirectToAction(nameof(Index), "Stocks");
                else
                    return RedirectToAction(nameof(Index),"LoanStock");
                }

                return View(login);
            }

            public async Task<ActionResult> Logout()
            {
                // Add logout time and duration logged in in seconds

                UserLoginTracking userLoginTracking = Session["Login"] as UserLoginTracking;
                userLoginTracking.UserLogoutDateTime = DateTime.Now;
                TimeSpan duration = userLoginTracking.UserLogoutDateTime.Value - userLoginTracking.UserLoginDateTime.Value;
                userLoginTracking.Duration = duration.TotalSeconds;
                db.Entry(userLoginTracking).State = EntityState.Modified;
                await db.SaveChangesAsync();

                FormsAuthentication.SignOut();

                // Dispose of sessions
                Session.Remove("Login");
                Session.Remove("User");
                Session.Remove("UserType");

                return RedirectToAction(nameof(Index));
            }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPassword forgotPassword)
        {
            if (ModelState.IsValid)
            {
                Users users = SharedMethods.UserExists(forgotPassword.Email);

                if (users != null)
                {
                    string GeneratedPassword = SharedMethods.GeneratePassword(users.FirstName, users.Surname);
                    _ = SharedMethods.SendEmail(users.GetFullName(), users.Email, "Forgot Password", "Hello, " + users.GetFullName() + ".\n\nA new password has been generated for you.\nThis is your password: " + GeneratedPassword + "\n\nKind regards,\nIoT System.", false);
                    users.Password = SharedMethods.HashPassword(GeneratedPassword, SharedMethods.GenerateSalt());
                    db.Entry(users).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(forgotPassword);
        }

        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubmitContact(string name, string email, string message)
        {
            if(string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
                return Json(new { success = false, message = "Please fill out all fields" });

            _ = SharedMethods.SendEmail("IoT Super Admin", "iotgrp2023@gmail.com", "IoT System - User Query/Question", $"Hello, Admin.\n\n{name} [{email}] has sent a query:\n\n{message}\n\nThank you.\nKind Regards,\nIoT System.", false);
            _ = SharedMethods.SendEmail(name, email, "IoT System - Query Submitted", $"Hello, {name}.\n\nThank you for your query.\nThis serves as confirmation of your query being submitted.\n\nKind regards,\nIoT System.", false);

            return Json(new { success = true, message = "Thank you, your query has been submitted" });

        }
    }
}
