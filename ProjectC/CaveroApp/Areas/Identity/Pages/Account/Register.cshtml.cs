// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using CaveroApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;

namespace CaveroApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<CaveroAppUser> _signInManager;
        private readonly UserManager<CaveroAppUser> _userManager;
        private readonly IUserStore<CaveroAppUser> _userStore;
        private readonly IUserEmailStore<CaveroAppUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<CaveroAppUser> userManager,
            IUserStore<CaveroAppUser> userStore,
            SignInManager<CaveroAppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required]
            [Display(Name = "First name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        /// <summary>
        /// Handles the user registration process.
        /// </summary>
        /// <param name="returnUrl">The URL to redirect to after registration. If null, the user is redirected to the home page.</param>
        /// <returns>
        /// An IActionResult that represents the result of the registration process.
        /// If the registration is successful, the user is redirected to the returnUrl.
        /// If the registration fails, the registration form is redisplayed with error messages.
        /// </returns>
        /// <remarks>
        /// This method is called when the user submits the registration form.
        /// It first checks if the ModelState is valid, which means that the form data passed the validation rules.
        /// If the ModelState is valid, it creates a new user, sets the user's properties from the form data,
        /// and then calls the UserManager to create the user in the database.
        /// If the user is created successfully, it generates an email confirmation token and sends a confirmation email to the user.
        /// If the UserManager is configured to require a confirmed account, it redirects the user to the RegisterConfirmation page.
        /// Otherwise, it signs in the user and redirects them to the returnUrl.
        /// If the user creation fails, it adds the errors to the ModelState and redisplay the registration form.
        /// </remarks>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                // Check if the email ends with @cavero.
                // MADE NON FUNCTIONAL BECAUSE PEOPLE NEED TO TEST AND RUN THIS APART FROM CAVERO.
                // if (!Input.Email.EndsWith("@cavero.nl"))
                // {
                //     ModelState.AddModelError(string.Empty, "You can only register with a Cavero email address.");
                //     return Page();
                // }
                var user = CreateUser();
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                // Change username from email to first name + last name.
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await SendEmailAsync(Input.Email, "Confirm your email", GetConfirmationEmailBody(callbackUrl));

                    // await SendEmailAsync(Input.Email, "Confirm your email",
                    //     $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task<bool> SendEmailAsync(string email, string subject, string confirmLink)
        {

            //TODO
            //INSERT YOUR OWN MAIL SERVER CREDENTIALS
            // message.From = ?
            // message.Port = ?
            // message.Host = ?
            // smtpClient.Credentials = new NetworkCredential(?Username,?Password);
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                message.From = new MailAddress("caveroapp@gmail.com");
                message.To.Add(email);
                message.Subject = subject;
                message.IsBodyHtml = true;
                message.Body = confirmLink;

                smtpClient.Port = 587;
                smtpClient.Host = "smtp.gmail.com";


                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("caveroapp@gmail.com", "iwcu fqli ccpy kuzz");
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private CaveroAppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<CaveroAppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(CaveroAppUser)}'. " +
                    $"Ensure that '{nameof(CaveroAppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<CaveroAppUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<CaveroAppUser>)_userStore;
        }

        private string GetConfirmationEmailBody(string callbackUrl)
        {
            string confirmationUrl = HtmlEncoder.Default.Encode(callbackUrl);

            return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <link href='https://fonts.googleapis.com/css?family=Montserrat' rel='stylesheet'>
    <title>Email Confirmation</title>
</head>
<body style=""padding: 0; margin: 0; font-family: 'Montserrat', sans-serif; background: #F6E0F8;"">
    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"">
        <tr>
            <td align=""center"" style=""padding: 15px;"">
                <img src='https://cavero.nl/wp-content/uploads/2019/07/logohandtekening.png' alt='Cavero Logo' width=""165"" height=""165"" style=""display: block; margin: 0 auto;""/>
            </td>
        </tr>
        <tr>
            <td align=""center"" style=""padding: 15px; background: #803689;"">
                <h1 style=""color: white; margin: 0;"">Welcome to Cavero!</h1>
                <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""margin-top: 15px; background: white; border-radius: 25px; padding: 15px;"">
                    <tr>
                        <td style=""text-align: center;"">
                            <p style=""margin: 0;"">Please confirm your account by <a href='{confirmationUrl}'>clicking here</a>.</p>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
        }
    }
}
