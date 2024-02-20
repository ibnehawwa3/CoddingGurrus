﻿using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Dtos;
using CoddingGurrus.Core.Interface;
using CoddingGurrus.Core.Models.User;
using CoddingGurrus.Infrastructure.CommonHelper.Handler;
using CoddingGurrus.web.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoddingGurrus.web.Controllers.Users
{
    public class UserController : Controller
    {
        private readonly IBaseHandler _baseHandler;
        private readonly int _defaultTake = 10;
        private readonly int _defaultSkip = 1;

        public UserController(IBaseHandler baseHandler)
        {
            _baseHandler = baseHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            var viewModel = await GetUsersViewModelAsync(searchTerm);
            return View(viewModel);
        }

        [HttpGet("create")]
        public IActionResult Create() => View();

        [HttpPost("create")]
        public async Task<IActionResult> Create(UserModel model)
        {
            if (ModelState.IsValid)
            {
                model.DateRegistration = DateTime.Now;
                if ((await _baseHandler.PostAsync<UserModel, UserResponseModel>(model, ApiEndPoints.CreateUsers)).Success)
                    return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit(string id) => View(await GetUserProfileAsync(id));

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(UserProfileModel model)
        {
            if (ModelState.IsValid && (await _baseHandler.PostAsync<UserProfileModel, UserResponseModel>(model, ApiEndPoints.UpdateUserProfile)).Success)
                return RedirectToAction("Index");

            return View(model);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if ((await _baseHandler.DeleteAsync<UserResponseModel>(ApiEndPoints.DeleteUserProfile + "?Id=" + id)).Success)
                return RedirectToAction("Index");

            return View("Error");
        }

        public async Task<GridViewModel<UserDto>> Search(string searchTerm) => await GetUsersViewModelAsync(searchTerm);

        private async Task<GridViewModel<UserDto>> GetUsersViewModelAsync(string searchText)
        {
            var response = await _baseHandler.GetAsync<UserResponseModel>(ApiEndPoints.GetUsers + $"?Skip={_defaultSkip}&Take={_defaultTake}&TextToSearch={searchText}");
            if (!response.Success)
                return new GridViewModel<UserDto> { Configuration = new GridConfiguration { HeaderText = GridConstants.HeaderText.User, Skip = 0, NoOfPages = 0 } };

            var userModels = JsonConvert.DeserializeObject<List<UserDto>>(response.Data);
            return new GridViewModel<UserDto>
            {
                Data = userModels,
                Configuration = new GridConfiguration
                {
                    HeaderText = GridConstants.HeaderText.User,
                    CreateButtonText = GridConstants.ButtonText.User,
                    Skip = 0,
                    Take = _defaultTake,
                    NoOfPages = (int)Math.Ceiling((double)userModels[0].TotalRecords / _defaultTake),
                    DisplayFields = DisplayFieldsHelper.GetDisplayFields<UserDto>(property => property.Name != "TotalRecords" && property.Name != "Id" && property.Name != "DateRegistration" && property.Name != "UserProfileId")
                }
            };
        }

        private async Task<UserProfileModel> GetUserProfileAsync(string id)
        {
            var getUserProfileRequest = new GetUserProfileRequest { Id = id };
            var response = await _baseHandler.PostAsync<GetUserProfileRequest, UserResponseModel>(getUserProfileRequest, ApiEndPoints.GetUserProfile);
            return JsonConvert.DeserializeObject<UserProfileModel>(response.Data);
        }
    }
}
