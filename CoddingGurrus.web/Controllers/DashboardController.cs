using CoddingGurrus.Core.APIResponses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoddingGurrus.web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index(LoginResponseModel model)
        {
            var loginResponseModel= new LoginResponseModel();
            // Retrieve the model from TempData
            if (TempData.ContainsKey("loginResponseModel"))
            {
                loginResponseModel = JsonConvert.DeserializeObject<LoginResponseModel>(TempData["loginResponseModel"].ToString());

                // Use loginResponseModel as needed in your Index action
                // ...

                // Clear the TempData to ensure it's only used once
                    TempData.Remove("loginResponseModel");
            }
            ViewBag.ClearLocalStorage = true;
            return View(loginResponseModel);
        }
    }
}
