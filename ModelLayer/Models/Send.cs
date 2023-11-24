using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ModelLayer.Models
{
    public class Send
    {
        public string SendMail(string ToEmail, string Token)
        {
            string FromEmail = "sriramravichandran2608@gmail.com";
            MailMessage Message = new MailMessage(FromEmail, ToEmail);
            string MailBody = "Token Generated : " + Token;
            Message.Subject = "Token Generated for Forgot Password";
            Message.Body = MailBody.ToString();
            Message.BodyEncoding = Encoding.UTF8;
            Message.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            NetworkCredential credential = new 
                NetworkCredential("sriramravichandran2608@gmail.com", "ryca wnih uwhu gzdw");

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = credential;

            smtpClient.Send(Message);
            return ToEmail;
        }
    }
}
