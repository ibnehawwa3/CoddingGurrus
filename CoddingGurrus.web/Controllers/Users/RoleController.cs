using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Dtos;
using CoddingGurrus.Core.Interface;
using CoddingGurrus.Core.Models.Role;
using CoddingGurrus.Core.Models.User;
using CoddingGurrus.web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoddingGurrus.web.Controllers.Users
{
    public class RoleController : Controller
    {
        private readonly IBaseHandler _baseHandler;
        private readonly int _defaultTake = 10;
        private readonly int _defaultSkip = 1;

        public RoleController(IBaseHandler baseHandler)
        {
            _baseHandler = baseHandler;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            var viewModel = await GetRolesViewModelAsync(searchTerm);
            ViewBag.gridViewModel = viewModel;
            return View();
        }

        [HttpGet("create")]
        public IActionResult Create() => View();

        [HttpPost("create")]
        public async Task<IActionResult> Create(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                if ((await _baseHandler.PostAsync<RoleModel, RoleResponseModel>(model, ApiEndPoints.CreateUsers)).Success)
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit(int id) => View(await GetRoleByIdAsync(id));

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(RoleModel model)
        {
            if (ModelState.IsValid && (await _baseHandler.PostAsync<RoleModel, RoleResponseModel>(model, ApiEndPoints.UpdateUserProfile)).Success)
                return RedirectToAction("Index");

            return View(model);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if ((await _baseHandler.DeleteAsync<RoleResponseModel>(ApiEndPoints.DeleteUserProfile + "?Id=" + id)).Success)
                return RedirectToAction("Index");

            return View("Error");
        }

        public async Task<GridViewModel<RoleModel>> Search(string searchTerm) => await GetRolesViewModelAsync(searchTerm);

        private async Task<GridViewModel<RoleModel>> GetRolesViewModelAsync(string searchText)
        {
            var response = await _baseHandler.GetAsync<RoleResponseModel>(ApiEndPoints.GetRole);
            if (!response.Success)
                return new GridViewModel<RoleModel> { Configuration = new GridConfiguration { HeaderText = GridHeaderText.Role, Skip = 0, NoOfPages = 0 } };

            var userModels = JsonConvert.DeserializeObject<List<UserDto>>(response.Data);
            return new GridViewModel<RoleModel>
            {
                Data = userModels,
                Configuration = new GridConfiguration
                {
                    HeaderText = GridHeaderText.User,
                    CreateButtonText = GridButtonText.User,
                    Skip = 0,
                    Take = _defaultTake,
                    NoOfPages = (int)Math.Ceiling((double)userModels[0].TotalRecords / _defaultTake),
                    DisplayFields = DisplayFieldsHelper.GetDisplayFields<RoleModel>(property => property.Name != "TotalRecords")
                }
            };
        }

        private async Task<RoleModel> GetRoleByIdAsync(int id)
        {
            var getUserProfileRequest = new GetRoleRequest { Id = id };
            var response = await _baseHandler.PostAsync<GetRoleRequest, RoleResponseModel>(getUserProfileRequest, ApiEndPoints.GetUserProfile);
            return JsonConvert.DeserializeObject<RoleModel>(response.Data);
        }
    }
}
