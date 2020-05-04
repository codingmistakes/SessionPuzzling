using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SessionPuzzling.Models;
using SessionPuzzling.Utility;

namespace SessionPuzzling.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            User user = HttpContext.Session.Get<User>("user");
            if (user != null)
            {
                return View("Welcome", user);
            }

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Set<User>("user", null);
            return View("Index");
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                ViewBag.Result = "Incorrect username or password!";
                return Redirect("/Home/Index");
            }
            
            User user = Authenticator.Authenticate(username, password);

            if (user != null)
            {                
                HttpContext.Session.Set<User>("user", user);

                return Redirect("/Home/Welcome");
            }

            ViewBag.Result = "Incorrect username or password!";
            ViewBag.Username = username;
            return View("Index");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string phone, string email)
        {
            if (String.IsNullOrEmpty(phone) || String.IsNullOrEmpty(email))
            {
                ViewBag.Result = "Incorrect phone or email!";
                return View();
            }

            User user = Authenticator.FindUser(phone, email);
            if (user != null)
            {
                user.LastOtpSent = new Random().Next(1000000).ToString();
                HttpContext.Session.Set<User>("user", user);
                return View("OneTimePassword", user);
            }

            ViewBag.Result = "Phone or email doesn't exist!";
            return View();
        }

        [HttpGet]
        public IActionResult OneTimePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OneTimePassword(string smsotp)
        {
            User user = HttpContext.Session.Get<User>("user");

            if(String.IsNullOrEmpty(smsotp) || user == null || user.LastOtpSent != smsotp)
            {
                ViewBag.Result = "Incorrect one time password!";
                return View(user);
            }

            return View("NewPassword", user);
        }

        [HttpGet]
        public IActionResult NewPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewPassword(string newpassword1, string newpassword2)
        {
            User user = HttpContext.Session.Get<User>("user");
            if (user != null)
            {
                // set new username & password and save
                ViewBag.Result = "Your password has been reset successfully, please enter your new credentials.";
                return View("Index");
            }

            return View("Index");
        }

        public IActionResult Welcome()
        {
            User user = HttpContext.Session.Get<User>("user");
            if(user != null)
            {
                return View(user);
            }

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
