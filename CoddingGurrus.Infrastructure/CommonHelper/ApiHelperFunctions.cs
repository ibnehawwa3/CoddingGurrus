
using CoddingGurrus.Core.APIResponses;

namespace CoddingGurrus.Infrastructure.CommonHelper
{
    public class ApiHelperFunctions
    {
        public async Task<LoginResponseModel> GetTokenResult(string RequestUri, string ContentBody)
        {
            LoginResponseModel responseModel = new LoginResponseModel();

            if (string.IsNullOrEmpty(SetTokenModel.Token) || SetTokenModel.ExpireTime <= DateTime.Now)
            {
                responseModel = await GetTokenModel.GetToken();
            }

            return responseModel;
        }

        public ResponseModel GetResult(string RequestUri)
        {
            if (string.IsNullOrEmpty(SetTokenModel.Token) || SetTokenModel.ExpireTime <= DateTime.Now)
            {
                GetTokenModel.GetToken().Wait();
            }
            ResponseModel responseModel = new ResponseModel();
            responseModel = DownloadManager.DownloadDataUsingWebClient(RequestUri, "");
            return responseModel;
        }
    }
}
