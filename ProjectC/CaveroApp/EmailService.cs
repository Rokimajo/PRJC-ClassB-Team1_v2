using System;
using System.Net;
using System.Net.Mail;

namespace YourNamespace.Services
{
    public class EmailService
    {
        public void SendEmail(string senderEmail, string senderPassword, string recipientEmail)
        {
            MailMessage mail = new MailMessage(senderEmail, recipientEmail);
            mail.Subject = "Testing SMTP Email in ASP.NET Core";
            mail.Body = "Hello, this is a test email sent from ASP.NET Core using SMTP.";

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(senderEmail, senderPassword);
            client.EnableSsl = true; // Enable SSL/TLS

            try
            {
                client.Send(mail);
                Console.WriteLine("Email sent successfully!");
                Console.WriteLine($"Email sent: Subject: {mail.Subject}, Body: {mail.Body}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
    }

}
