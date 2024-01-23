using Microsoft.AspNetCore.Mvc;

namespace CoddingGurrus.web.Controllers.Users
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
