﻿using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Dtos;
using CoddingGurrus.Core.Interface;
using CoddingGurrus.Core.Models.Topics;
using CoddingGurrus.Core.Validations;
using CoddingGurrus.web.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CoddingGurrus.web.Controllers.Tutorials
{
    [Route("Topics")]
    public class TopicsController : Controller
    {
        private readonly IBaseHandler _baseHandler;
        private int _defaultTake = 10;
        private int _defaultSkip = 1;
        TopicModelValidator validationRules;
        public TopicsController(IBaseHandler baseHandler)
        {
            _baseHandler = baseHandler;
            validationRules= new TopicModelValidator();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            var viewModel = await GetTopicsViewModelAsync(searchTerm);
            ViewBag.gridViewModel = viewModel;
            return View();
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create() 
        {
            var response = await _baseHandler.GetAsync<ResponseModel>(ApiEndPoints.GetCourseDropDown);
            if (!response.Success)
                new GridViewModel<CourseDto> { Configuration = new GridConfiguration { HeaderText = GridHeaderText.Role, Skip = 0, NoOfPages = 0 } };

            var roleList = JsonConvert.DeserializeObject<List<DropDownDto>>(response.Data);

            ViewBag.Course = new SelectList(roleList, "Id", "Name");
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(TopicsModel model)
        {
            var dropdownResponse = await _baseHandler.GetAsync<ResponseModel>(ApiEndPoints.GetCourseDropDown);

            if (!dropdownResponse.Success)
            {
                ModelState.AddModelError("", dropdownResponse.ErrorMessage ?? "Failed to retrieve dropdown data.");
                return View(model);
            }

            var roleList = JsonConvert.DeserializeObject<List<DropDownDto>>(dropdownResponse.Data);
            ViewBag.Course = new SelectList(roleList, "Id", "Name");

            var result = ValidationHelper.ValidateModel(model,validationRules);

            if (!result.IsValid)
            {
                ValidationHelper.GetValidationErrors(result);
                return View(model);
            }

            var createResponse = await _baseHandler.PostAsync<TopicsModel, ResponseModel>(model, ApiEndPoints.CreateTopics);

            if (createResponse.Success)
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
        public async Task<IActionResult> Edit(long id)
        {
            var response = await _baseHandler.GetAsync<ResponseModel>(ApiEndPoints.GetCourses);
            if (!response.Success)
                new GridViewModel<CourseDto> { Configuration = new GridConfiguration { HeaderText = GridHeaderText.Role, Skip = 0, NoOfPages = 0 } };

            var roleList = JsonConvert.DeserializeObject<List<DropDownTitleDto>>(response.Data);
            var topics=await GetTopicsByIdAsync(id);
            ViewBag.Course = new SelectList(roleList, "Id", "Title", topics.CourseId);
            return View(topics);
        }

        [HttpGet("HandlePagination")]
        public async Task<GridViewModel<TopicsDto>> HandlePagination(int pageSize)
        {
            this._defaultSkip = pageSize;
            ViewBag.pageSize = pageSize;
            return await GetTopicsViewModelAsync(string.Empty);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(TopicsModel model)
        {
            var result = ValidationHelper.ValidateModel(model, validationRules);

            if (!result.IsValid)
            {
                ValidationHelper.GetValidationErrors(result);
                return View(model);
            }

            if ((await _baseHandler.PostAsync<TopicsModel, ResponseModel>(model, ApiEndPoints.UpdateTopics)).Success)
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
        public async Task<IActionResult> Delete(long id)
        {
            if ((await _baseHandler.DeleteAsync<ResponseModel>(ApiEndPoints.DeleteTopics + "?Id=" + id)).Success)
                return RedirectToAction("Index");

            return View("Error");
        }
        public async Task<GridViewModel<TopicsDto>> Search(string searchTerm) => await GetTopicsViewModelAsync(searchTerm);

        private async Task<GridViewModel<TopicsDto>> GetTopicsViewModelAsync(string searchText)
        {
            var response = await _baseHandler.GetAsync<ResponseModel>(ApiEndPoints.GetTopics + $"?Skip={_defaultSkip}&Take={_defaultTake}&TextToSearch={searchText}");
            if (!response.Success)
                return new GridViewModel<TopicsDto> { Configuration = new GridConfiguration { HeaderText = GridConstants.ButtonText.Topics, Skip = 0, NoOfPages = 0 } };

            var courseModels = JsonConvert.DeserializeObject<List<TopicsDto>>(response.Data);

            return new GridViewModel<TopicsDto>
            {
                Data = courseModels,
                Configuration = new GridConfiguration
                {
                    HeaderText = GridHeaderText.Topics,
                    CreateButtonText = GridButtonText.Topics,
                    CreateAction = nameof(ActionType.Create),
                    UpdateAction = nameof(ActionType.Edit),
                    DeleteAction = nameof(ActionType.Delete),
                    ControllerName = nameof(ControllerName.Topics),
                    Skip = this._defaultSkip,
                    Take = _defaultTake,
                    NoOfPages = (int)Math.Ceiling((double)courseModels[0].TotalRecords / _defaultTake),
                    DisplayFields = DisplayFieldsHelper.GetDisplayFields<TopicsDto>(property => property.Name != "TotalRecords")
                }
            };
        }
        private async Task<TopicsModel> GetTopicsByIdAsync(long id)
        {
            var idRequest = new LongIdRequest { Id = id };
            var response = await _baseHandler.PostAsync<LongIdRequest, ResponseModel>(idRequest, ApiEndPoints.GetTopicsById);
            return JsonConvert.DeserializeObject<TopicsModel>(response.Data);
        }
    }
}
