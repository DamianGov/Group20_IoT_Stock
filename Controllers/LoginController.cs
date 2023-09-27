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

                            string Action = "";
                            if (user.Role.Type == "SuperAdmin")
                                Action = "SuperHome";
                            else if (user.Role.Type == "Admin")
                                Action = "AdminHome";
                            else if (user.Role.Type == "Member")
                                Action = "MemberHome";
                            return RedirectToAction(Action, "Home");
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

                string Action = "";
                if (user.Role.Type == "SuperAdmin")
                    Action = "SuperHome";
                else if (user.Role.Type == "Admin")
                    Action = "AdminHome";
                else if (user.Role.Type == "Member")
                    Action = "MemberHome";
                return RedirectToAction(Action, "Home");
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
                    users.Password = PasswordHandler.HashPassword(GeneratedPassword, PasswordHandler.GenerateSalt());
                    db.Entry(users).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                }
            }

            return View(forgotPassword);
        }
    }
}
