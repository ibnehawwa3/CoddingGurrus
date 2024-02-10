using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Dtos;
using CoddingGurrus.Core.Interface;
using CoddingGurrus.Core.Models.User;
using CoddingGurrus.Infrastructure.CommonHelper.Handler;
using CoddingGurrus.web.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CoddingGurrus.web.Controllers.Users
{
    public class UserController : Controller
    {
        private readonly IBaseHandler baseHandler;
        private List<UserDto> userModels;
        private GridViewModel<UserDto> gridViewModel;
        private int Skip = 1;
        private int Take = 10;

        public UserController(IBaseHandler baseHandler)
        {
            this.baseHandler = baseHandler;
            userModels = new List<UserDto>();
            ViewBag.ClearLocalStorage = false;
        }

        public IActionResult Index()
        {
            GetUsers(string.Empty);
            return View(gridViewModel);
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var response = baseHandler.PostAsync<UserModel,UserResponseModel>(model,ApiEndPoints.CreateUsers).Result;

                if (response.Success)
                    return RedirectToAction("Index");
            }

            return View(model);
        }


        public IActionResult Edit(string id)
        {
            GetUserProfileRequest getUserProfileRequest = new GetUserProfileRequest
            {
                Id = id
            };
            var response = baseHandler.PostAsync<GetUserProfileRequest, UserResponseModel>(getUserProfileRequest,ApiEndPoints.GetUserProfile).Result;
            var userDto = JsonConvert.DeserializeObject<UserProfileModel>(response.Data);
            return View(userDto);
        }

        [HttpPost]
        public IActionResult Edit(UserProfileModel model)
        {
            if (ModelState.IsValid)
            {
                var response = baseHandler.PostAsync<UserProfileModel, UserResponseModel>(model, ApiEndPoints.UpdateUserProfile).Result;

                if (response.Success)
                    return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            var response = baseHandler.DeleteAsync<UserResponseModel>(ApiEndPoints.DeleteUserProfile + "?Id=" + id).Result;

            if (response.Success)
                return RedirectToAction("Index");
            return View(response);
        }
        private void GetUsers(string SeacrhText)
        {
            ViewBag.ShowLoader = true;
            var response = baseHandler.GetAsync<UserResponseModel>(ApiEndPoints.GetUsers + "?Skip=" + this.Skip + "&Take=" + this.Take+ "&TextToSearch="+SeacrhText).Result;

            if (response.Success)
            {
                userModels = JsonConvert.DeserializeObject<List<UserDto>>(response.Data);
                if (userModels.Count > 0)
                {
                    gridViewModel = new GridViewModel<UserDto>
                    {
                        Data = userModels,
                        Configuration = new GridConfiguration
                        {
                            HeaderText = GridHeaderText.User,
                            CreateButtonText=GridButtonText.User,
                            Skip = Skip,
                            Take = Take,
                            NoOfPages = (int)Math.Ceiling((double)userModels.FirstOrDefault().TotalRecords / Take),
                            DisplayFields = DisplayFieldsHelper.GetDisplayFields<UserDto>(property => property.Name != "TotalRecords" && property.Name != "Id" && property.Name != "DateRegistration" && property.Name!= "UserProfileId")
                        }
                    };
                    ViewBag.ShowLoader = false;
                }
                else
                {
                    gridViewModel = new GridViewModel<UserDto>
                    {
                        Configuration=new GridConfiguration 
                        { 
                            HeaderText = GridHeaderText.User,
                            Skip = 0, 
                            NoOfPages = 0,
                        }
                    };
                    ViewBag.ShowLoader = false;
                }
            }
        }


        public GridViewModel<UserDto> Search(string searchTerm)
        {
            GetUsers(searchTerm);
            return gridViewModel;
        }
    }

}
