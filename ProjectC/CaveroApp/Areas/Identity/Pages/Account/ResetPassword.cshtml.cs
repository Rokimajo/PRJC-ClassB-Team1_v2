// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using CaveroApp.Areas.Identity.Data;
using CaveroApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace CaveroApp.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<CaveroAppUser> _userManager;
        private readonly SignInManager<CaveroAppUser> _signInManager;
        private readonly CaveroAppContext _context;

        public ResetPasswordModel(UserManager<CaveroAppUser> userManager, SignInManager<CaveroAppUser> signInManager, CaveroAppContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
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
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            public string Code { get; set; }

        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Find the user with the email address
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }
            //change the password for the user
            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                // If the password reset was successful, log the user in with the new password
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("./ResetPasswordConfirmation");
            }
            // If the password reset failed, display an error
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable
//
// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using System.Linq;
// using System.Text;
// using System.Text.Encodings.Web;
// using System.Threading;
// using System.Threading.Tasks;
// using CaveroApp.Areas.Identity.Data;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Identity.UI.Services;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.RazorPages;
// using Microsoft.AspNetCore.WebUtilities;
// using Microsoft.Extensions.Logging;
// using System.Net.Mail;
// using System.Net;
// using CaveroApp.Data;
//
// namespace CaveroApp.Areas.Identity.Pages.Account
// {
//     public class ResetPasswordModel : PageModel
//     {
//         private readonly UserManager<CaveroAppUser> _userManager;
//         //get the database into this page and other necessary variables to use the database
//         private readonly CaveroAppContext _context;
//         private readonly SignInManager<CaveroAppUser> _signInManager;
//         private readonly ILogger<ResetPasswordModel> _logger;
//         private readonly IEmailSender _emailSender;
//
//
//
//         public ResetPasswordModel(
//             UserManager<CaveroAppUser> userManager,
//             CaveroAppContext context,
//             SignInManager<CaveroAppUser> signInManager,
//             ILogger<ResetPasswordModel> logger,
//             IEmailSender emailSender
//         )
//         {
//             _userManager = userManager;
//             _context = context;
//             _signInManager = signInManager;
//             _logger = logger;
//             _emailSender = emailSender;
//
//         }
//
//
//         /// <summary>
//         ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//         ///     directly from your code. This API may change or be removed in future releases.
//         /// </summary>
//         [BindProperty]
//         public InputModel Input { get; set; }
//
//         /// <summary>
//         ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//         ///     directly from your code. This API may change or be removed in future releases.
//         /// </summary>
//         public class InputModel
//         {
//             /// <summary>
//             ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//             ///     directly from your code. This API may change or be removed in future releases.
//             /// </summary>
//             [Required]
//             [EmailAddress]
//             public string Email { get; set; }
//
//             /// <summary>
//             ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//             ///     directly from your code. This API may change or be removed in future releases.
//             /// </summary>
//             [Required]
//             [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
//             [DataType(DataType.Password)]
//             public string Password { get; set; }
//
//             /// <summary>
//             ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//             ///     directly from your code. This API may change or be removed in future releases.
//             /// </summary>
//             [DataType(DataType.Password)]
//             [Display(Name = "Confirm password")]
//             [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
//             public string ConfirmPassword { get; set; }
//
//             /// <summary>
//             ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//             ///     directly from your code. This API may change or be removed in future releases.
//             /// </summary>
//             [Required]
//             public string Code { get; set; }
//
//         }
//         /// <summary>
//         ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//         ///     directly from your code. This API may change or be removed in future releases.
//         /// </summary>
//         public string ReturnUrl { get; set; }
//         /// <summary>
//         ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
//         ///     directly from your code. This API may change or be removed in future releases.
//         /// </summary>
//         public IList<AuthenticationScheme> ExternalLogins { get; set; }
//
//
//         public IActionResult OnGet(string code = null)
//         {
//             if (code == null)
//             {
//                 return BadRequest("A code must be supplied for password reset.");
//             }
//             else
//             {
//                 Input = new InputModel
//                 {
//                     Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
//                 };
//                 return Page();
//             }
//         }
//
//         public async Task<IActionResult> OnPostAsync()
//         {
//             if (!ModelState.IsValid)
//             {
//                 return Page();
//             }
//
//             var user = await _userManager.FindByEmailAsync(Input.Email);
//             // var newPassword = Input.Password;
//             // var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, newPassword);
//             // user.PasswordHash = newPasswordHash;
//             // _context.Update(user);
//             // await _context.SaveChangesAsync();
//
//             if (user == null)
//             {
//                 // Don't reveal that the user does not exist
//                 return RedirectToPage("./ResetPasswordConfirmation");
//             }
//
//             var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
//             if (result.Succeeded)
//             {
//                 return RedirectToPage("./ResetPasswordConfirmation");
//             }
//
//             foreach (var error in result.Errors)
//             {
//                 ModelState.AddModelError(string.Empty, error.Description);
//             }
//             return Page();
//         }
//     }
// }
