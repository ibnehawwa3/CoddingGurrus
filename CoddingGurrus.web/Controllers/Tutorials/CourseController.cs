﻿using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Dtos;
using CoddingGurrus.Core.Interface;
using CoddingGurrus.Core.Models.Course;
using CoddingGurrus.web.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoddingGurrus.web.Controllers.Tutorials
{
    [Route("Course")]
    public class CourseController : Controller
    {
        private readonly IBaseHandler _baseHandler;
        private readonly int _defaultTake = 10;
        private readonly int _defaultSkip = 1;

        public CourseController(IBaseHandler baseHandler)
        {
            _baseHandler = baseHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            var viewModel = await GetCoursesViewModelAsync(searchTerm);
            ViewBag.gridViewModel = viewModel;
            return View();
        }

        [HttpGet("create")]
        public IActionResult Create() => View();

        [HttpPost("create")]
        public async Task<IActionResult> Create(CourseModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _baseHandler.PostAsync<CourseModel, UserResponseModel>(model, ApiEndPoints.CreateCourse);

                if (response.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.ErrorMessage); // Add error message to model state
                }
            }
            return View(model);
        }
        [HttpGet("edit")]
        public async Task<IActionResult> Edit(long id) => View(await GetCourseByIdAsync(id));

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(CourseModel model)
        {
            if (ModelState.IsValid && (await _baseHandler.PostAsync<CourseModel, UserResponseModel>(model, ApiEndPoints.UpdateCourse)).Success)
                return RedirectToAction("Index");

            return View(model);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete(long id)
        {
            if ((await _baseHandler.DeleteAsync<UserResponseModel>(ApiEndPoints.DeleteCourse + "?Id=" + id)).Success)
                return RedirectToAction("Index");

            return View("Error");
        }
        public async Task<GridViewModel<CourseDto>> Search(string searchTerm) => await GetCoursesViewModelAsync(searchTerm);

        private async Task<GridViewModel<CourseDto>> GetCoursesViewModelAsync(string searchText)
        {
            var response = await _baseHandler.GetAsync<UserResponseModel>(ApiEndPoints.GetCourses + $"?Skip={_defaultSkip}&Take={_defaultTake}&TextToSearch={searchText}");
            if (!response.Success)
                return new GridViewModel<CourseDto> { Configuration = new GridConfiguration { HeaderText = GridConstants.ButtonText.Course, Skip = 0, NoOfPages = 0 } };

            var courseModels = JsonConvert.DeserializeObject<List<CourseDto>>(response.Data);
            return new GridViewModel<CourseDto>
            {
                Data = courseModels,
                Configuration = new GridConfiguration
                {
                    HeaderText = GridHeaderText.Course,
                    CreateButtonText = GridButtonText.Course,
                    CreateAction = nameof(ActionType.Create),
                    UpdateAction = nameof(ActionType.Edit),
                    DeleteAction = nameof(ActionType.Delete),
                    ControllerName = nameof(ControllerName.Course),
                    //HeaderText = GridConstants.ButtonText.Course,
                    //CreateButtonText = GridConstants.ButtonText.Course,
                    //ControllerName = GridConstants.ControllerNames.Course,
                    Skip = 0,
                    Take = _defaultTake,
                    NoOfPages = (int)Math.Ceiling((double)courseModels[0].TotalRecords / _defaultTake),
                    DisplayFields = DisplayFieldsHelper.GetDisplayFields<CourseDto>(property => property.Name != "TotalRecords" && property.Name != "Id")
                }
            };
        }
        private async Task<CourseModel> GetCourseByIdAsync(long id)
        {
            var idRequest = new LongIdRequest { Id = id };
            var response = await _baseHandler.PostAsync<LongIdRequest, UserResponseModel>(idRequest, ApiEndPoints.GetCourseById);
            return JsonConvert.DeserializeObject<CourseModel>(response.Data);
        }
    }
}
