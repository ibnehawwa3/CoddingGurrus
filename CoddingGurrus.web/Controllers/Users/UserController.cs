using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Interface;
using CoddingGurrus.Core.Models.User;
using CoddingGurrus.Infrastructure.CommonHelper.Handler;
using CoddingGurrus.web.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            var response= baseHandler.GetAsync<UserResponseModel>(ApiEndPoints.GetUsers+"?Skip="+this.Skip+"&Take="+this.Take).Result;

            if (response.Success)
            {
                userModels = JsonConvert.DeserializeObject<List<UserModel>>(response.Data);
                gridViewModel = new GridViewModel<UserModel> {
                    Data = userModels,
                    Configuration = new GridConfiguration
                    {
                        HeaderText = GridHeaderText.User,
                        Skip= Skip,
                        Take= Take
                    }
                };
            }
            return View(gridViewModel);
        }
    }

}
