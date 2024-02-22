using CoddingGurrus.Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CoddingGurrus.web.Controllers.Menu
{
    public class MenuController : Controller
    {
        private readonly IBaseHandler _baseHandler;
        private readonly int _defaultTake = 10;
        private readonly int _defaultSkip = 1;

        public MenuController(IBaseHandler baseHandler)
        {
            _baseHandler = baseHandler;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
