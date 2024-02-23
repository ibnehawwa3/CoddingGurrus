using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Interface;
using CoddingGurrus.Core.Models.Menu;
using CoddingGurrus.Core.Models.Role;
using CoddingGurrus.web.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoddingGurrus.web.Controllers.Menu
{
    [Route("Menu")]
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
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            var viewModel = await GetMenusViewModelAsync(searchTerm);
            ViewBag.gridViewModel = viewModel;
            return View();
        }



        public async Task<GridViewModel<MenuModel>> Search(string searchTerm) => await GetMenusViewModelAsync(searchTerm);

        private async Task<GridViewModel<MenuModel>> GetMenusViewModelAsync(string searchText)
        {
            var response = await _baseHandler.GetAsync<MenuResponseModel>(ApiEndPoints.GetMenus);
            if (!response.Success)
                return new GridViewModel<MenuModel> { Configuration = new GridConfiguration { HeaderText = GridHeaderText.Menu, Skip = 0, NoOfPages = 0 } };

            var userModels = JsonConvert.DeserializeObject<List<MenuModel>>(response.Data);
            return new GridViewModel<MenuModel>
            {
                Data = userModels,
                Configuration = new GridConfiguration
                {
                    HeaderText = GridHeaderText.Menu,
                    CreateButtonText = GridButtonText.Menu,
                    CreateAction = nameof(ActionType.Create),
                    UpdateAction = nameof(ActionType.Edit),
                    DeleteAction = nameof(ActionType.Delete),
                    ControllerName = nameof(ControllerName.Menu),
                    Skip = 0,
                    Take = _defaultTake,
                    NoOfPages = 1,
                    DisplayFields = DisplayFieldsHelper.GetDisplayFields<MenuModel>(property => property.Name != "TotalRecords" && property.Name != "ConcurrencyStamp" && property.Name != "NormalizedName")
                }
            };
        }


        [HttpGet("Create")]
        public IActionResult Create() => View();

        [HttpPost("Create")]
        public async Task<IActionResult> Create(MenuModel model)
        {
            if (ModelState.IsValid)
            {
                if ((await _baseHandler.PostAsync<MenuModel, MenuResponseModel>(model, ApiEndPoints.CreateMenu)).Success)
                    return RedirectToAction("Index");
            }
            return View(model);
        }




        [HttpGet("edit")]
        public async Task<IActionResult> Edit(int id) => View(await GetMenuByIdAsync(id));

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(MenuModel model)
        {
            if (ModelState.IsValid && (await _baseHandler.PostAsync<MenuModel, MenuResponseModel>(model, ApiEndPoints.UpdateUserProfile)).Success)
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if ((await _baseHandler.DeleteAsync<MenuResponseModel>(ApiEndPoints.DeleteMenu + "?Id=" + id)).Success)
                return RedirectToAction("Index");

            return View("Error");
        }

        private async Task<MenuModel> GetMenuByIdAsync(int id)
        {
            var getUserProfileRequest = new GetMenuRequest { Id = id };
            var response = await _baseHandler.GetByIdAsync<MenuResponseModel>(ApiEndPoints.GetMenuById, id);
            return JsonConvert.DeserializeObject<MenuModel>(response.Data);
        }
    }
}
