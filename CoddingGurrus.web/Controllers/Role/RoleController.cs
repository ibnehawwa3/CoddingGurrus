using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Dtos;
using CoddingGurrus.Core.Interface;
using CoddingGurrus.Core.Models.Role;
using CoddingGurrus.web.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoddingGurrus.web.Controllers
{
    [Route("Role")]
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

        [HttpGet("Create")]
        public IActionResult Create() => View();

        [HttpPost("Create")]
        public async Task<IActionResult> Create(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                model.Id = Guid.NewGuid();
                if ((await _baseHandler.PostAsync<RoleModel, RoleResponseModel>(model, ApiEndPoints.CreateRole)).Success)
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit(string id) => View(await GetRoleByIdAsync(id));

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(RoleModel model)
        {
            if (ModelState.IsValid && (await _baseHandler.PostAsync<RoleModel, RoleResponseModel>(model, ApiEndPoints.UpdateRole)).Success)
                return RedirectToAction("Index");

            return View();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if ((await _baseHandler.DeleteAsync<RoleResponseModel>(ApiEndPoints.DeleteRole + "?Id=" + id)).Success)
                return RedirectToAction("Index");

            return View("Error");
        }

        public async Task<GridViewModel<RoleDto>> Search(string searchTerm) => await GetRolesViewModelAsync(searchTerm);

        private async Task<GridViewModel<RoleDto>> GetRolesViewModelAsync(string searchText)
        {
            var response = await _baseHandler.GetAsync<RoleResponseModel>(ApiEndPoints.GetRole);
            if (!response.Success)
                return new GridViewModel<RoleDto> { Configuration = new GridConfiguration { HeaderText = GridHeaderText.Role, Skip = 0, NoOfPages = 0 } };

            var userModels = JsonConvert.DeserializeObject<List<RoleDto>>(response.Data);
            return new GridViewModel<RoleDto>
            {
                Data = userModels,
                Configuration = new GridConfiguration
                {
                    HeaderText = GridHeaderText.Role,
                    CreateButtonText = GridButtonText.Role,
                    CreateAction = nameof(ActionType.Create),
                    UpdateAction = nameof(ActionType.Edit),
                    DeleteAction = nameof(ActionType.Delete),
                    ControllerName = nameof(ControllerName.Role),
                    Skip = 0,
                    Take = _defaultTake,
                    NoOfPages = 1,
                    DisplayFields = DisplayFieldsHelper.GetDisplayFields<RoleDto>(property => property.Name != "TotalRecords" && property.Name != "ConcurrencyStamp" && property.Name != "NormalizedName")
                }
            };
        }

        private async Task<RoleModel> GetRoleByIdAsync(string id)
        {
            var response = await _baseHandler.GetByGuidIdAsync<RoleResponseModel>(ApiEndPoints.GetRoleById,id);
            return JsonConvert.DeserializeObject<RoleModel>(response.Data);
        }
    }
}
