using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Group20_IoT.Models
{
    public static class Email
    {
        public static bool SendEmail(string UserName, string UserEmail, string EmailSubject, string EmailBody, bool InHtml)
        {
            try
            {
                var IoTDetail = new MailAddress("iotgrp2023@gmail.com", "IoT System");
                var UserDetail = new MailAddress(UserEmail, UserName);
                var Password = "qscnfdohnhtbvcet";

                using(var smtp = new SmtpClient 
                { 
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(IoTDetail.Address, Password)
                
                })
                using (var message = new MailMessage(IoTDetail, UserDetail)
                {
                    Subject = EmailSubject,
                    IsBodyHtml = InHtml,
                    Body = EmailBody
                })
                {
                    smtp.Send(message);
                }

                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}