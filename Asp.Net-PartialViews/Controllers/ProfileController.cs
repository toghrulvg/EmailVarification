using Asp.Net_PartialViews.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Threading.Tasks;

namespace Asp.Net_PartialViews.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IdentityDbContext<AppUser> _context;

        public ProfileController(UserManager<AppUser> userManager, IdentityDbContext<AppUser> context)
        {
            _userManager = userManager;
            _context = context;

        }
        public async Task<IActionResult> Index()
        {


            

            //if (appUser is null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}

            return View();
            
        }

        
    }
}
