using CoddingGurrus.Core.APIResponses;
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
        private List<UserModel> userModels;
        private GridViewModel<UserModel> gridViewModel;
        private int Skip = 1;
        private int Take = 10;

        public UserController(IBaseHandler baseHandler)
        {
            this.baseHandler = baseHandler;
            userModels = new List<UserModel>();
        }

        public IActionResult Index()
        {
            GetUsers();
            return View(gridViewModel);
        }


        private void GetUsers(string SeacrhText=null)
        {
            var response = baseHandler.GetAsync<UserResponseModel>(ApiEndPoints.GetUsers + "?Skip=" + this.Skip + "&Take=" + this.Take+ "&TextToSearch="+SeacrhText).Result;

            if (response.Success)
            {
                userModels = JsonConvert.DeserializeObject<List<UserModel>>(response.Data);
                if (userModels.Count > 0)
                {
                    gridViewModel = new GridViewModel<UserModel>
                    {
                        Data = userModels,
                        Configuration = new GridConfiguration
                        {
                            HeaderText = GridHeaderText.User,
                            Skip = Skip,
                            Take = Take,
                            NoOfPages = (int)Math.Ceiling((double)userModels.FirstOrDefault().TotalRecords / Take),
                            DisplayFields = DisplayFieldsHelper.GetDisplayFields<UserModel>(property => property.Name != "TotalRecords" && property.Name != "Id" && property.Name != "DateRegistration")
                        }
                    };
                }
                else
                {
                    gridViewModel = new GridViewModel<UserModel>
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


        public GridViewModel<UserModel> Search(string searchTerm)
        {
            GetUsers(searchTerm);
            return gridViewModel;
        }
    }

}
