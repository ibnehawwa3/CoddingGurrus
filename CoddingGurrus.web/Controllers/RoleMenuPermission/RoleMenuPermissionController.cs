using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Dtos;
using CoddingGurrus.Core.Interface;
using CoddingGurrus.Core.Models.RoleMenuPermission;
using CoddingGurrus.Core.Models.User;
using CoddingGurrus.web.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CoddingGurrus.web.Controllers.RoleMenuPermission
{
    [Route("RoleMenuPermission")]
    public class RoleMenuPermissionController : Controller
    {
        private readonly IBaseHandler _baseHandler;
        private readonly int _defaultTake = 10;
        private readonly int _defaultSkip = 1;

        public RoleMenuPermissionController(IBaseHandler baseHandler)
        {
            _baseHandler = baseHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _baseHandler.GetAsync<RoleResponseModel>(ApiEndPoints.GetRole);
            if (!response.Success)
                  new GridViewModel<RoleDto> { Configuration = new GridConfiguration { HeaderText = GridHeaderText.Role, Skip = 0, NoOfPages = 0 } };

            var roleList = JsonConvert.DeserializeObject<List<DropDownDto>>(response.Data);

            ViewBag.Role = new SelectList(roleList, "Id", "Name");
            return View("PreIndex");
        }

        [HttpGet("GetMenusPermission")]
        public async Task<IActionResult> GetMenusPermission(string RoleId)
        {
            List<RoleMenuPermissionModel> roleMenuPermissionModels = new List<RoleMenuPermissionModel>();
            roleMenuPermissionModels = await GetRoleMenuPermissionViewModelAsync(RoleId);
            roleMenuPermissionModels.ForEach(item => item.RoleId = int.Parse(RoleId));
            return View("Index", roleMenuPermissionModels);
        }


        [HttpPost("create")]
        public async Task<IActionResult> Create(List<RoleMenuPermissionModel> model)
        {
            if (ModelState.IsValid)
            {
                if ((await _baseHandler.PostAsync<List<RoleMenuPermissionModel>, RoleMenuPermissionResponseModel>(model, ApiEndPoints.CreateRoleMenuPermission)).Success)
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        private async Task<List<RoleMenuPermissionModel>> GetRoleMenuPermissionViewModelAsync(string RoleId)
        {
            var response = await _baseHandler.GetAsync<RoleMenuPermissionResponseModel>(ApiEndPoints.GetRoleMenuPermission + $"?RoleId={RoleId}");
            if (!response.Success)
                return new List<RoleMenuPermissionModel>();

            return JsonConvert.DeserializeObject<List<RoleMenuPermissionModel>>(response.Data);
           
        }
    }
}
