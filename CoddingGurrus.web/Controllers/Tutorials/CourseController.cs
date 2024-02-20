using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Dtos;
using CoddingGurrus.Core.Interface;
using CoddingGurrus.web.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CoddingGurrus.web.Controllers.Tutorials
{
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
            return View(viewModel);
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
                    HeaderText = GridConstants.ButtonText.Course,
                    CreateButtonText = GridConstants.ButtonText.Course,
                    Skip = 0,
                    Take = _defaultTake,
                    NoOfPages = (int)Math.Ceiling((double)courseModels[0].TotalRecords / _defaultTake),
                    DisplayFields = DisplayFieldsHelper.GetDisplayFields<CourseDto>(property => property.Name != "TotalRecords" && property.Name != "Id")
                }
            };
        }
    }
}
