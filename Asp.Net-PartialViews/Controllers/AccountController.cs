using Asp.Net_PartialViews.Models;
using Asp.Net_PartialViews.Services.Interfaces;
using Asp.Net_PartialViews.ViewModels.AccountViewModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.Net_PartialViews.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IFileService _fileService;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IFileService fileService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _fileService = fileService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }

            AppUser appUser = new AppUser
            {
                Fullname = registerVM.Fullname,
                Email = registerVM.Email,
                UserName = registerVM.Username
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(registerVM);
            }


            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

            string link = Url.Action(nameof(ConfirmEmail), "Account", new { userId = appUser.Id, token },
                Request.Scheme, Request.Host.ToString());

            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("toghrulvg@code.edu.az"));
            email.To.Add(MailboxAddress.Parse(appUser.Email));
            email.Subject = "Verify Email";
            string body = String.Empty;
            string path = "wwwroot/assets/templates/verify.html";
            string subject = "Verify email";

            body = _fileService.ReadFile(path, body);



            

            body = body.Replace("{{link}}", link);

                _emailService.Send(appUser.Email, subject, body);






            //await _signInManager.SignInAsync(appUser, false);

            return RedirectToAction(nameof(VerifyEmail));           

        }

        

        public IActionResult VerifyEmail()
        {
            return View();
        }
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId is null ) return BadRequest();
            if (token is null) return BadRequest();

            AppUser user = await _userManager.FindByIdAsync(userId);

            if (user is null) return NotFound();

            await _userManager.ConfirmEmailAsync(user, token);

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.EmailOrUsername);
            if (appUser is null)
            {
                appUser = await _userManager.FindByNameAsync(loginVM.EmailOrUsername);
            }

            if (appUser is null)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(loginVM);   
            }

            var result = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is wrong");
                return View(loginVM);
            }


            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }

   
}
