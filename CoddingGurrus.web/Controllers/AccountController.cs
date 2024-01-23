using CoddingGurrus.Core.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace CoddingGurrus.web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            return RedirectToAction("Index", "Dashboard");
        }
        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
