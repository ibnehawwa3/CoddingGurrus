using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Dtos;
using CoddingGurrus.Core.Interface;
using CoddingGurrus.Core.Models.Menu;
using CoddingGurrus.Core.Validations;
using CoddingGurrus.web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Reflection;

namespace CoddingGurrus.web.Controllers.Menu
{
    [Route("Menu")]
    public class MenuController : Controller
    {
        private readonly IBaseHandler _baseHandler;
        private readonly int _defaultTake = 10;
        private readonly int _defaultSkip = 1;
        MenuModelValidator validationRules;
        bool IsDeleted=false;
        public MenuController(IBaseHandler baseHandler)
        {
            _baseHandler = baseHandler;
            validationRules= new MenuModelValidator();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            var viewModel = await GetMenusViewModelAsync(searchTerm);
            ViewBag.gridViewModel = viewModel;
            return View();
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var response = await _baseHandler.GetAsync<ResponseModel>(ApiEndPoints.GetMenus);
            if (!response.Success)
                new GridViewModel<MenuDto> { Configuration = new GridConfiguration { HeaderText = GridHeaderText.Role, Skip = 0, NoOfPages = 0 } };

            var roleList = JsonConvert.DeserializeObject<List<DropDownDto>>(response.Data);

            ViewBag.Parent = new SelectList(roleList, "Id", "Name");
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(MenuModel model)
        {
            var response = await _baseHandler.GetAsync<ResponseModel>(ApiEndPoints.GetMenus);
            if (!response.Success)
                new GridViewModel<MenuDto> { Configuration = new GridConfiguration { HeaderText = GridHeaderText.Role, Skip = 0, NoOfPages = 0 } };

            var roleList = JsonConvert.DeserializeObject<List<DropDownDto>>(response.Data);

            ViewBag.Parent = new SelectList(roleList, "Id", "Name");

            var result = ValidationHelper.ValidateModel(model, validationRules);

            if (!result.IsValid)
            {
                ValidationHelper.GetValidationErrors(result);
                return View(model);
            }

            model.Archived = false;
            if ((await _baseHandler.PostAsync<MenuModel, ResponseModel>(model, ApiEndPoints.CreateMenus)).Success)
            {
                TempData["SuccessMessage"] = ResponseMessage.SuccessMessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = ResponseMessage.ErrorMessage;
                return View(model);
            }
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _baseHandler.GetAsync<ResponseModel>(ApiEndPoints.GetMenus);
            if (!response.Success)
                new GridViewModel<MenuDto> { Configuration = new GridConfiguration { HeaderText = GridHeaderText.Role, Skip = 0, NoOfPages = 0 } };

            var roleList = JsonConvert.DeserializeObject<List<DropDownDto>>(response.Data);

            ViewBag.Parent = new SelectList(roleList, "Id", "Name");
            return View(await GetMenByIdAsync(id));
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(MenuModel model)
        {
            model.Archived = false;
            if (ModelState.IsValid && (await _baseHandler.PostAsync<MenuModel, ResponseModel>(model, ApiEndPoints.UpdateMenus)).Success)
            {
                TempData["UpdateSuccessMessage"] = ResponseMessage.UpdateSuccessMessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["UpdateErrorMessage"] = ResponseMessage.UpdateErrorMessage;
                return View(model);
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if ((await _baseHandler.DeleteAsync<ResponseModel>(ApiEndPoints.DeleteMenus + "?Id=" + id)).Success)
            {
                this.IsDeleted=true;
                TempData["DeleteSuccessMessage"] = ResponseMessage.DeleteSuccessMessage;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["DeleteErrorMessage"] = ResponseMessage.DeleteErrorMessage;
                return View("Error");
            }
        }

        public async Task<GridViewModel<MenuDto>> Search(string searchTerm) => await GetMenusViewModelAsync(searchTerm);

        private async Task<GridViewModel<MenuDto>> GetMenusViewModelAsync(string searchText)
        {
            var response = await _baseHandler.GetAsync<ResponseModel>(ApiEndPoints.GetMenus);
            if (!response.Success)
                return new GridViewModel<MenuDto> { Configuration = new GridConfiguration { HeaderText = GridHeaderText.Role, Skip = 0, NoOfPages = 0 } };

            var userModels = JsonConvert.DeserializeObject<IEnumerable<MenuDto>>(response.Data);
            return new GridViewModel<MenuDto>
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
                    DisplayFields = DisplayFieldsHelper.GetDisplayFields<MenuDto>(property => property.Name != "TotalRecords" && property.Name != "ConcurrencyStamp" && property.Name != "NormalizedName")
                }
            };
        }

        private async Task<MenuModel> GetMenByIdAsync(int id)
        {
            var getUserProfileRequest = new GetRoleRequest { Id = id };
            var response = await _baseHandler.GetByIdAsync<ResponseModel>(ApiEndPoints.GetMenuById, id);
            return JsonConvert.DeserializeObject<MenuModel>(response.Data);
        }
    }
}
