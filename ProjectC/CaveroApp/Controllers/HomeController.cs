// using Microsoft.AspNetCore.Mvc;

// namespace SendingEmails
// {
//     public class HomeController : Controller
//     {
//         private readonly IEmailSender emailSender;

//         public HomeController(IEmailSender emailSender)
//         {
//             this.emailSender = emailSender;
//         }

//         [HttpPost]
//         public async Task<IActionResult> Index(string email, string subject, string message)
//         {
//             await emailSender.SendEmailAsync(email, subject, message);
//             return View();
//         }
//     }
// }