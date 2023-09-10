using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Group20_IoT.Models;
using Group20_IoT.Models.ViewModel;

namespace Group20_IoT.Controllers
{
    [SessionCheckerSuperUser]
    public class UserTrackingController : Controller
    {
        private IoTContext db = new IoTContext();   
        public ActionResult Index()
        {
            var logReport = db.UserLoginTracking.Include(u => u.Users).Include(u => u.Users.Role)
                .OrderByDescending(u => u.UserLoginDateTime)
                .AsEnumerable()
                .Select(
                u => new UserTrackingViewModel
                {
                    Id = u.Id,
                    Name = u.Users.Name,
                    Email = u.Users.Email,
                    LoggedIn = u.UserLoginDateTime.HasValue ? u.UserLoginDateTime.Value.ToString("yyyy/M/d HH:mm") : "-",
                    LoggedOut = u.UserLogoutDateTime.HasValue ? u.UserLogoutDateTime.Value.ToString("yyyy/M/d HH:mm") : "-",
                    Duration = FormatDuration(u.Duration),
                    UserType = u.Users.Role.Type

                }).ToList();


            return View(logReport);
        }

        public ActionResult Filter(string userType)
        {
            var logReport = db.UserLoginTracking.Include(u => u.Users).Include(u => u.Users.Role)
                .Where(u => u.Users.Role.Type.Equals(userType))
                .OrderByDescending(u => u.UserLoginDateTime)
                .AsEnumerable()
                .Select(
                u => new UserTrackingViewModel
                {
                    Id = u.Id,
                    Name = u.Users.Name,
                    Email = u.Users.Email,
                    LoggedIn = u.UserLoginDateTime.HasValue ? u.UserLoginDateTime.Value.ToString("yyyy/M/d HH:mm") : "-",
                    LoggedOut = u.UserLogoutDateTime.HasValue ? u.UserLogoutDateTime.Value.ToString("yyyy/M/d HH:mm") : "-",
                    Duration = FormatDuration(u.Duration),
                    UserType = u.Users.Role.Type

                }).ToList();

            return PartialView("_UserTracked", logReport);
        }

        static string FormatDuration(double? duration)
        {
            if (!duration.HasValue) return "-";
            
            string[] units = { "second", "minute", "hour", "day" };
            double[] thresholds = { 60.0, 60.0, 24.0 };

            int unitIndex = 0;
            while (unitIndex < units.Length - 1 && duration.Value >= thresholds[unitIndex])
            {
                duration /= thresholds[unitIndex];
                unitIndex++;
            }

            string formattedDuration = $"{duration:F1} {units[unitIndex]}{(duration != 1.0 ? "s" : "")}";

            return formattedDuration;
        }
    }
}