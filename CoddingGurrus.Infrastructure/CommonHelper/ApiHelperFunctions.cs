
using CoddingGurrus.Core.APIResponses;

namespace CoddingGurrus.Infrastructure.CommonHelper
{
    public class ApiHelperFunctions
    {
        static bool isRunning = false;

        public ResponseModel GetResult(string RequestUri, string ContentBody)
        {
            if (string.IsNullOrEmpty(SetTokenModel.Token) || SetTokenModel.ExpireTime <= DateTime.Now)
            {
                GetTokenModel.GetToken().Wait();
            }

            ResponseModel responseModel = new ResponseModel();

            responseModel = DownloadManager.DownloadDataUsingWebClient(RequestUri, ContentBody);

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
