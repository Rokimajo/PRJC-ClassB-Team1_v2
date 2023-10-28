// namespace SendingEmails
// {
//     using System.Net;
//     using System.Net.Mail;

//     public class EmailSender : IEmailSender
//     {
//         public Task SendEmailAsync(string email, string subject, string message)
//         {
//             // Log the email content before sending
//             Console.WriteLine("Email content to be sent:");
//             Console.WriteLine($"To: {email}");
//             Console.WriteLine($"Subject: {subject}");
//             Console.WriteLine($"Message: {message}");

//             var mail = "sobag92590@wanbeiz.com";

//             var client = new SmtpClient("smtp.office365.com", 587)
//             {
//                 EnableSsl = true,
//                 UseDefaultCredentials = false,
//                 Credentials = new NetworkCredential("your.email@live.com", "your password")
//             };

//             return client.SendMailAsync(
//                 new MailMessage(from: mail,
//                                 to: email,
//                                 subject,
//                                 message
//                                 ));
//         }
//     }
// }

// using System;

// using MailKit.Net.Smtp;
// using MailKit;
// using MimeKit;

// public class EmailService
// {
//     public void SendEmail()
//     {
//         var email = new MimeMessage();

//         email.From.Add(new MailboxAddress("Sender Name", "sender@email.com"));
//         email.To.Add(new MailboxAddress("Receiver Name", "receiver@email.com"));

//         email.Subject = "Testing out email sending";
//         email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
//         {
//             Text = "Hello all the way from the land of C#"
//         };
//         using (var smtp = new SmtpClient())
//         {
//             // smtp.Connect("smtp.server.address", 587, false);
//             smtp.Connect("smtp.gmail.com", 587, false);

//             // Note: only needed if the SMTP server requires authentication
//             smtp.Authenticate("smtp_username", "smtp_password");

//             smtp.Send(email);
//             smtp.Disconnect(true);
//         }
//         LogEmailDetails(email);
//     }

//     public void LogEmailDetails(MimeMessage email)
//     {
//         Console.WriteLine("Email to be sent:");
//         Console.WriteLine("From: " + email.From.Mailboxes.First());
//         Console.WriteLine("To: " + email.To.Mailboxes.First());
//         Console.WriteLine("Subject: " + email.Subject);
//         Console.WriteLine("Body: " + ((TextPart)email.Body).Text);
//     }
// }
