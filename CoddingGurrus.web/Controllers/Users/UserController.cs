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
            GetUsers();
            return View(gridViewModel);
        }
        public IActionResult Create()
        {
            return View();
        }
        // POST: /User/Create
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

        private void GetUsers(string SeacrhText=null)
        {
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
                            DisplayFields = DisplayFieldsHelper.GetDisplayFields<UserDto>(property => property.Name != "TotalRecords" && property.Name != "Id" && property.Name != "DateRegistration" && property.Name!= "Password")
                        }
                    };
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
