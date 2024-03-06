using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Dtos;
using CoddingGurrus.Core.Dtos.Tutorials;
using CoddingGurrus.Core.Interface;
using CoddingGurrus.Core.Models.Tutorials;
using CoddingGurrus.web.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace CoddingGurrus.web.Controllers.Tutorials
{
    [Route("Content")]
    public class ContentController : Controller
    {
        private readonly IBaseHandler _baseHandler;
        private int _defaultTake = 10;
        private int _defaultSkip = 1;

        public ContentController(IBaseHandler baseHandler)
        {
            _baseHandler = baseHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm = "")
        {
            var viewModel = await GetContentsViewModelAsync(searchTerm);
            ViewBag.gridViewModel = viewModel;
            return View();
        }


        [HttpGet("GetTopicDropdownByCourseId")]
        public async Task<ActionResult> GetTopicDropdownByCourseId(long courseId)
        {
            List <DropDownDto> dropDownDtos = new List<DropDownDto>();
            var topicResponse =await _baseHandler.GetAsync<ResponseModel>(ApiEndPoints.GetTopicDropDownByCourseId+ "?courseId=" + courseId );
            if (topicResponse.Success)
            {
                dropDownDtos = JsonConvert.DeserializeObject<List<DropDownDto>>(topicResponse.Data);
            }

            return new JsonResult(new { data = dropDownDtos });
        }



        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var courseResponse = await _baseHandler.GetAsync<ResponseModel>(ApiEndPoints.GetCourseDropDown);
            if (courseResponse.Success)
            {
                var courseList = JsonConvert.DeserializeObject<List<DropDownDto>>(courseResponse.Data);

                ViewBag.Course = new SelectList(courseList, "Id", "Name");
            }
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(ContentModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _baseHandler.PostAsync<ContentModel, UserResponseModel>(model, ApiEndPoints.CreateContent);

                if (response.Success)
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
            return View(model);
        }
        [HttpGet("edit")]
        public async Task<IActionResult> Edit(long id)
        {
            var content = await GetContentByIdAsync(id);
            var courseResponse = await _baseHandler.GetAsync<ResponseModel>(ApiEndPoints.GetCourseDropDown);
            if (courseResponse.Success)
            {
                var courseList = JsonConvert.DeserializeObject<List<DropDownDto>>(courseResponse.Data);

                ViewBag.Course = new SelectList(courseList, "Id", "Name",content.Id);
            }

            return View(content);
        } 

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(ContentModel model)
        {
            if (ModelState.IsValid && (await _baseHandler.PostAsync<ContentModel, UserResponseModel>(model, ApiEndPoints.UpdateContent)).Success)
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
            if ((await _baseHandler.DeleteAsync<UserResponseModel>(ApiEndPoints.DeleteContent + "?Id=" + id)).Success)
                return RedirectToAction("Index");

            return View("Error");
        }

        [HttpGet("HandlePagination")]
        public async Task<GridViewModel<ContentDto>> HandlePagination(int pageSize)
        {
            this._defaultSkip = pageSize;
            ViewBag.pageSize = pageSize;
            return await GetContentsViewModelAsync(string.Empty);
        }
        public async Task<GridViewModel<ContentDto>> Search(string searchTerm) => await GetContentsViewModelAsync(searchTerm);

        private async Task<GridViewModel<ContentDto>> GetContentsViewModelAsync(string searchText)
        {
            var response = await _baseHandler.GetAsync<UserResponseModel>(ApiEndPoints.GetContents + $"?Skip={_defaultSkip}&Take={_defaultTake}&TextToSearch={searchText}");
            if (!response.Success)
                return new GridViewModel<ContentDto> { Configuration = new GridConfiguration { HeaderText = GridConstants.ButtonText.Content, Skip = 0, NoOfPages = 0 } };

            var contentModels = JsonConvert.DeserializeObject<List<ContentDto>>(response.Data);
            return new GridViewModel<ContentDto>
            {
                Data = contentModels,
                Configuration = new GridConfiguration
                {
                    HeaderText = GridHeaderText.Content,
                    CreateButtonText = GridButtonText.Content,
                    CreateAction = nameof(ActionType.Create),
                    UpdateAction = nameof(ActionType.Edit),
                    DeleteAction = nameof(ActionType.Delete),
                    ControllerName = nameof(ControllerName.Content),
                    Skip = this._defaultSkip,
                    Take = _defaultTake,
                    NoOfPages = (int)Math.Ceiling((double)contentModels[0].TotalRecords / _defaultTake),
                    DisplayFields = DisplayFieldsHelper.GetDisplayFields<ContentDto>(property => property.Name != "TotalRecords" && property.Name != "Id")
                }
            };
        }
        private async Task<ContentViewModel> GetContentByIdAsync(long id)
        {
            var idRequest = new LongIdRequest { Id = id };
            var response = await _baseHandler.PostAsync<LongIdRequest, UserResponseModel>(idRequest, ApiEndPoints.GetContentById);
            return JsonConvert.DeserializeObject<ContentViewModel>(response.Data);
        }
    }
}
