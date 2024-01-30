using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Models.User;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using System.Reflection;
using CoddingGurrus.Infrastructure.CommonHelper;

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
            var model = new LoginResponseModel();
            ApiHelperFunctions apiHelperFunctions = new ApiHelperFunctions();
            Hashtable hashtable = new Hashtable();
            hashtable.Add("Email", loginModel.Email);
            hashtable.Add("Password", loginModel.Password);

            string json = JsonConvert.SerializeObject(hashtable);
            ResponseModel responseModel = apiHelperFunctions.GetResult("api/account/login", json);
            model = JsonConvert.DeserializeObject<LoginResponseModel>(Convert.ToString(responseModel.Data));
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
