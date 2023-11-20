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
    public class ForgotPasswordModel : PageModel
    {
        private readonly SignInManager<CaveroAppUser> _signInManager;
        private readonly UserManager<CaveroAppUser> _userManager;
        private readonly IUserStore<CaveroAppUser> _userStore;
        private readonly IUserEmailStore<CaveroAppUser> _emailStore;
        private readonly ILogger<ForgotPasswordModel> _logger;
        private readonly IEmailSender _emailSender;
        public ForgotPasswordModel(
            UserManager<CaveroAppUser> userManager,
            IUserStore<CaveroAppUser> userStore,
            SignInManager<CaveroAppUser> signInManager,
            ILogger<ForgotPasswordModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        private IUserEmailStore<CaveroAppUser> GetEmailStore()
        {
            if (_userStore is IUserEmailStore<CaveroAppUser> cast)
            {
                return cast;
            }
            else
            {
                throw new NotSupportedException("The user store must implement IUserEmailStore<CaveroAppUser>.");
            }
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
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

        }
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (ModelState.IsValid)
                {
                    var existingUser = await _userManager.FindByEmailAsync(Input.Email);
                    if (existingUser == null)
                    {
                        // Don't reveal that the user does not exist
                        return RedirectToPage("./Login");
                    }

                    // await _userStore.SetUserNameAsync(existingUser, Input.Email, CancellationToken.None);
                    // await _emailStore.SetEmailAsync(existingUser, Input.Email, CancellationToken.None);
                    // var result = await _userManager.CreateAsync(user, Input.Email);
                    // if (result.Succeeded)
                    // {
                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                    // Make an email that uses the cavero logo and has a purple background
                    SendEmail(Input.Email, "Reset your password",
                        $"<div style=\"background-color: rgb(184, 138, 235); padding: 10px 0px 10px 0px; margin: 0px 0px 10px 0px; text-align: center;\">" +
                        $"<img src=\"https://cavero.nl/wp-content/uploads/2019/07/logohandtekening.png\" alt=\"Cavero Logo\" style=\"height: 100px; width: 100px;\">" +
                        $"</div>" +
                        $"<div style=\"background-color: white; padding: 10px 0px 10px 0px; margin: 0px 0px 10px 0px; text-align: center;\">" +
                        $"<h1 style=\"color: rgb(184, 138, 235);\">Reset your password</h1>" +
                        $"<p>Hi {Input.Email},</p>" +
                        $"<p>We received a request to reset your Cavero password.</p>" +
                        $"<p>If you didn't make the request, just ignore this email. Otherwise, you can reset your password using this link:</p>" +
                        $"<a href=\"{HtmlEncoder.Default.Encode(callbackUrl)}\" style=\"color: white; background-color: rgb(184, 138, 235); padding: 10px 20px 10px 20px; text-decoration: none;\">Reset Password</a>" +
                        $"<p>Thanks,</p>" +
                        $"<p>The Cavero Team</p>" +
                        $"</div>");
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("ForgotPasswordConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    //}

                    // foreach (var error in result.Errors)
                    // {
                    //     ModelState.AddModelError(string.Empty, error.Description);
                    // }
                    // For more information on how to enable account confirmation and password reset please
                    // visit https://go.microsoft.com/fwlink/?LinkID=532713
                }
                else
                {
                    return Page();
                }
            }
            return Page();
        }
        private static bool SendEmail(string email, string subject, string confirmLink)
        {
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

    }
}
