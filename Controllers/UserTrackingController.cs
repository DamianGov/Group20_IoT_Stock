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
using PagedList;

namespace Group20_IoT.Controllers
{
    [SessionChecker("SuperAdmin")]
    public class UserTrackingController : Controller
    {
        private IoTContext db = new IoTContext();
        int pageSize = 10;
        public ActionResult Index(int? page)
        {
            
            int pageNumber = (page ?? 1);

            var logReport = GetUserTrackingViewModel("");

            var pagedData = logReport.ToPagedList(pageNumber, pageSize);

            return View(pagedData);
        }
        public List<UserTrackingViewModel> GetUserTrackingViewModel(string UserType)
        {
            List<UserTrackingViewModel> logReport = new List<UserTrackingViewModel>();
            if (UserType == "")
            {
                    logReport = db.UserLoginTracking.Include(u => u.Users).Include(u => u.Users.Role)
                    .OrderByDescending(u => u.UserLoginDateTime)
                    .AsEnumerable()
                    .Select(
                    u => new UserTrackingViewModel
                    {
                        Id = u.Id,
                        Name = u.Users.GetFullName() + " [" + u.Users.UniNum+"]",
                        Email = u.Users.Email,
                        LoggedIn = u.UserLoginDateTime.HasValue ? u.UserLoginDateTime.Value.ToString("yyyy/M/d HH:mm") : "-",
                        LoggedOut = u.UserLogoutDateTime.HasValue ? u.UserLogoutDateTime.Value.ToString("yyyy/M/d HH:mm") : "-",
                        Duration = FormatDuration(u.Duration),
                        UserType = u.Users.Role.Type,
                        UsedRememberMe = u.UsedRememberMe ? "Yes" : "No"

                    }).ToList();
            }
            else
            {
                logReport = db.UserLoginTracking.Include(u => u.Users).Include(u => u.Users.Role)
                .Where(u => u.Users.Role.Type.Equals(UserType))
                .OrderByDescending(u => u.UserLoginDateTime)
                .AsEnumerable()
                .Select(
                u => new UserTrackingViewModel
                {
                    Id = u.Id,
                    Name = u.Users.GetFullName() + " [" + u.Users.UniNum + "]",
                    Email = u.Users.Email,
                    LoggedIn = u.UserLoginDateTime.HasValue ? u.UserLoginDateTime.Value.ToString("yyyy/M/d HH:mm") : "-",
                    LoggedOut = u.UserLogoutDateTime.HasValue ? u.UserLogoutDateTime.Value.ToString("yyyy/M/d HH:mm") : "-",
                    Duration = FormatDuration(u.Duration),
                    UserType = u.Users.Role.Type,
                    UsedRememberMe = u.UsedRememberMe ? "Yes" : "No"

                }).ToList();
            }
            return logReport;
        }

        public ActionResult Filter(string userType, int? page)
        {
            int pageNumber = (page ?? 1);

            var logReport = GetUserTrackingViewModel(userType);

            var pagedData = logReport.ToPagedList(pageNumber, pageSize);
            return PartialView("_UserTracked", pagedData);
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