using Microsoft.AspNetCore.Mvc;

namespace CoddingGurrus.web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
