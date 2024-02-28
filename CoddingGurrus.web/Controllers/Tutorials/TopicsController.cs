using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Dtos;
using CoddingGurrus.Core.Interface;
using CoddingGurrus.Core.Models.Topics;
using CoddingGurrus.web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace CoddingGurrus.web.Controllers.Tutorials
{
    [Route("Topics")]
    public class TopicsController : Controller
    {
        private readonly IBaseHandler _baseHandler;
        private readonly int _defaultTake = 10;
        private readonly int _defaultSkip = 1;

        public TopicsController(IBaseHandler baseHandler)
        {
            _baseHandler = baseHandler;
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
            var response = await _baseHandler.GetAsync<ResponseModel>(ApiEndPoints.GetCourses);
            if (!response.Success)
                new GridViewModel<CourseDto> { Configuration = new GridConfiguration { HeaderText = GridHeaderText.Role, Skip = 0, NoOfPages = 0 } };

            var roleList = JsonConvert.DeserializeObject<List<DropDownTitleDto>>(response.Data);

            ViewBag.Course = new SelectList(roleList, "Id", "Title");
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(TopicsModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _baseHandler.PostAsync<TopicsModel, ResponseModel>(model, ApiEndPoints.CreateTopics);

                if (response.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.ErrorMessage);
                }
            }
            return View(model);
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

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(TopicsModel model)
        {
            if (ModelState.IsValid && (await _baseHandler.PostAsync<TopicsModel, ResponseModel>(model, ApiEndPoints.UpdateTopics)).Success)
                return RedirectToAction("Index");

            return View(model);
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
                    Skip = 0,
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
