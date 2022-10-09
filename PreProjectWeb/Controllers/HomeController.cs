using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PreProjectWeb.Models;
using PreProjectWeb.Models.ViewModel;
using PreProjectWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PreProjectWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly IAccountRepository _accRepo;
        private readonly ITrailRepository _trailRepo;

        public HomeController(ILogger<HomeController> logger, INationalParkRepository npRepo,
            ITrailRepository trailRepo, IAccountRepository accRepo)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
            _logger = logger;
            _accRepo = accRepo;
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel ListOfParksAndTrails = new IndexViewModel()
            {
                NationalParkList = await _npRepo.GetAllAsync(StaticDetails.NationalParkAPIPath,HttpContext.Session.GetString("JWToken")),
                TrailList = await _trailRepo.GetAllAsync(StaticDetails.TrailAPIPath, HttpContext.Session.GetString("JWToken")),
            };
            return View(ListOfParksAndTrails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            User obj = new User();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User obj)
        {
            User objUser = await _accRepo.LoginAsync(StaticDetails.AccountAPIPath + "authenticate/", obj);
            if (objUser.Token == null)
            {
                return View();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, objUser.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, objUser.Role));
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            HttpContext.Session.SetString("JWToken", objUser.Token);
            TempData["alert"] = "Welcome" + "   " + objUser.Username;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User obj)
        {
            bool result = await _accRepo.RegisterAsync(StaticDetails.AccountAPIPath + "register/", obj);
            if (result == false)
            {
                return View();
            }
            TempData["alert"] = "Registration Successful";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
