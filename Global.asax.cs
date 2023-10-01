using Group20_IoT.Models;
using Hangfire;
using Hangfire.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Group20_IoT
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            GlobalConfiguration.Configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage("IoTDb");

            yield return new BackgroundJobServer();
        }

        protected void Application_Start()
        {

            CultureInfo cultureInfo = new CultureInfo("en-ZA");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            TimeZoneInfo saTimeZone = TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time");

            HangfireAspNet.Use(GetHangfireServers);
            RecurringJob.AddOrUpdate(() => HangfireAutomations.CheckStockLevelLowAndSendEmail(), Cron.Daily, saTimeZone);
            RecurringJob.AddOrUpdate(() => HangfireAutomations.CheckDatePastForLoanRequest(), Cron.Daily, saTimeZone);
            RecurringJob.AddOrUpdate(() => HangfireAutomations.LoanStockDueDateApproachingReminder(), Cron.Daily, saTimeZone);
            RecurringJob.AddOrUpdate(() => HangfireAutomations.MarkLoanPastDateAsOverdue(), Cron.Daily, saTimeZone);
        }
    }
}
