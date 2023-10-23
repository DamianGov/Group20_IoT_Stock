
using Azure.Core;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Group20_IoT.Models
{
    public static class HangfireAutomations
    {
        private static readonly IoTContext db = new IoTContext();

        public static async Task CheckStockLevelLowAndSendEmail()
        {
            try
            {
                List<Stock> stocks = db.Stock.Where(s => s.TotalQuantity < 10).ToList();
                List<Users> users = db.Users.Where(u => u.Access && u.Notify).ToList();

                if (stocks.Count > 0 && users.Count > 0)
                {
                    StringBuilder message = new StringBuilder();
                    message.AppendLine("<h1>Low Stock:</h1>");
                    message.AppendLine("<ul>");
                    foreach (var stock in stocks)
                    {
                        message.AppendLine($"<li>{stock.StockCode}[{stock.Name}] - Quantity: {stock.TotalQuantity}</li>");
                    }

                    message.AppendLine("</ul>");

                    foreach (var user in users)
                    {
                        _ = SharedMethods.SendEmail(user.GetFullName(), user.Email, "IoT Report - Low Stock [Automation]", $"Hello,<br><br>{message}<br><br>Kind regards,<br>IoT System.", true);
                    }
                }

                await Task.Yield();
            }
            catch (Exception ex)
            {

            }
        }

        public static async Task CheckDatePastForLoanRequest()
        {
            try
            {
                var CurrentDate = DateTime.Now.Date;
                var PastRequest = db.RequestLoanStock.Include(s => s.Stock).Include(s => s.Users).Where(r => DbFunctions.TruncateTime(r.FromDate) < CurrentDate && r.Status == RequestLoanStock.RequestStatus.Pending).ToList();

                if (PastRequest.Count > 0)
                {
                    foreach (var request in PastRequest)
                    {
                        request.Status = RequestLoanStock.RequestStatus.Rejected;
                        db.Entry(request).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        // SEND EMAIL
                        _ = SharedMethods.SendEmail(request.Users.GetFullName(), request.Users.Email, $"IoT System - Loan Request Rejected [Loan Reference #{request.Id}]", $"Hello,{request.Users.GetFullName()}.\n\nYour request to loan\n \"{request.Quantity} x {request.Stock.Name}\" \nhas unfortunately been rejected.\n\nThank you.\nKind regards,\nIoT System.", false);
                    }
                }

                await Task.Yield();
            }
            catch(Exception ex) { }
        }

        public static async Task LoanStockDueDateApproachingReminder()
        {
            try
            {
                DateTime TodayDate = DateTime.Now.Date;
                DateTime TwoDaysFromTd = TodayDate.AddDays(2);
                var Approaching = db.RequestLoanStock.Include(r => r.Users).Include(r => r.Stock).Where(r => DbFunctions.TruncateTime(r.DueDate) <= TwoDaysFromTd && r.Status == RequestLoanStock.RequestStatus.Accepted).ToList();

                if (Approaching.Count > 0)
                {
                    foreach (var request in Approaching)
                    {
                        TimeSpan Diff = request.DueDate.Date - TodayDate;
                        int DaysLeft = (int)Diff.TotalDays;
                        string Day;
                        if (DaysLeft == 0)
                            Day = "today";
                        else if (DaysLeft == 1)
                            Day = "tomorrow";
                        else Day = "in 2 days time";

                        _ = SharedMethods.SendEmail(request.Users.GetFullName(), request.Users.Email, "IoT System Reminder - Loan Stock Due Soon", $"Hello, {request.Users.GetFullName()}.\n\nThe stock loaned to you {request.Stock.Name} on {request.FromDate.ToString("dd MMMM yyyy")} is due {Day}.\nPlease do not forget to return it on time.\n\nThank you.\nKind Regards,\nIoT System.", false);

                    }
                }

                await Task.Yield();
            }
            catch (Exception ex) { }
        }

        public static async Task MarkLoanPastDateAsOverdue()
        {
            try
            {
                DateTime TodayDate = DateTime.Now.Date;
                var Overdue = db.LoanStatus.Include(r => r.RequestLoanStock).Include(r => r.Users).Include(r => r.RequestLoanStock.Stock).Include(r => r.RequestLoanStock.Users).Where(r => DbFunctions.TruncateTime(r.RequestLoanStock.DueDate) < TodayDate && r.Status == LoanStatus.LoanStatusStock.Picked_Up).ToList();

                if (Overdue.Count > 0)
                {
                    foreach (var request in Overdue)
                    {
                        TimeSpan Diff = TodayDate - request.RequestLoanStock.DueDate.Date;
                        int DaysOverdue = (int)Diff.TotalDays;

                        request.Status = LoanStatus.LoanStatusStock.Overdue;

                        db.Entry(request).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        _ = SharedMethods.SendEmail(request.RequestLoanStock.Users.GetFullName(), request.RequestLoanStock.Users.Email, "IoT System Reminder - Loan Stock Overdue", $"Hello, {request.RequestLoanStock.Users.GetFullName()}.\n\nThe stock loaned to you, {request.RequestLoanStock.Stock.Name}, was due on {request.RequestLoanStock.DueDate.ToString("dd MMMM yyyy")} is overdue by {DaysOverdue} days.\nPlease urgently return the stock loaned to you.\n\nThank you.\nKind Regards,\nIoT System.", false);

                    }
                }

                await Task.Yield();
            }
            catch (Exception ex)
            {

            }
        }
    }
}