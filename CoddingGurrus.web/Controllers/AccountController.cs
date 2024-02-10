using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Models.User;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using System.Reflection;
using CoddingGurrus.Infrastructure.CommonHelper;
using CoddingGurrus.web.Helper;

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
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            var apiHelperFunctions = new ApiHelperFunctions();

            string json = JsonConvert.SerializeObject(new Hashtable
            {
                { "Email", loginModel.Email },
                { "Password", loginModel.Password }
            });

            LoginResponseModel loginResponseModel = await apiHelperFunctions.GetTokenResult(ApiEndPoints.Login, json);
            // Convert LoginResponseModel to JSON string before storing in TempData
            string loginResponseModelJson = JsonConvert.SerializeObject(loginResponseModel);

            // Store the JSON string in TempData
            TempData["loginResponseModel"] = loginResponseModelJson;
            // Now you can use the 'model' variable to handle the result or perform further actions.

            if (!string.IsNullOrEmpty(SetTokenModel.Token))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
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
