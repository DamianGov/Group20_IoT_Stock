
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
            List<Stock> stocks = db.Stock.Where(s => s.TotalQuantity < 10).ToList();
            List<Users> users = db.Users.Where(u => u.Access && u.Notify).ToList();

            if (stocks.Count > 0 && users.Count > 0) 
            { 
                StringBuilder message = new StringBuilder();
                message.AppendLine("<h1>Low Stock:</h1>");
                message.AppendLine("<ul>");
                foreach(var stock in stocks)
                {
                    message.AppendLine($"<li>{stock.StockCode}[{stock.Name}] - Quantity: {stock.TotalQuantity}</li>");
                }

                message.AppendLine("</ul>");

                foreach(var user in users)
                {
                    _ = SharedMethods.SendEmail(user.GetFullName(), user.Email, "IoT Report - Low Stock", $"Hello,<br><br>{message}<br><br>Kind regards,<br>IoT System.", true);      
                }
            }

            await Task.Yield();
        }
    }
}