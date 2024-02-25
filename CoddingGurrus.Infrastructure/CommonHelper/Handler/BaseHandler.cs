using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Interface;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CoddingGurrus.Infrastructure.CommonHelper.Handler
{
    public class BaseHandler:IBaseHandler
    {
        private HttpClient httpClient;
        private ApiHelperFunctions apiHelperFunctions;
        private ResponseModel responseModel;
        public BaseHandler()
        {
            httpClient = new HttpClient();
            apiHelperFunctions = new ApiHelperFunctions();
            responseModel= new ResponseModel();
            SetBearer();
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request, string apiEndpoint)
        {
            var response = await httpClient.PostAsync(ApiUri.Info_API.APIUrl + apiEndpoint, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse> DeleteAsync<TResponse>(string apiEndpoint)
        {
            var response = await httpClient.DeleteAsync(ApiUri.Info_API.APIUrl + apiEndpoint);

            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse> GetAsync<TResponse>(string apiEndpoint)
        {
            var response = await httpClient.GetAsync(ApiUri.Info_API.APIUrl + apiEndpoint);

            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse> GetByIdAsync<TResponse>(string apiEndpoint, int id)
        {
            var response = await httpClient.GetAsync(ApiUri.Info_API.APIUrl + apiEndpoint);

            return await HandleResponse<TResponse>(response);
        }

        private async Task<TResponse> HandleResponse<TResponse>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {

                string jsonResponse = await response.Content.ReadAsStringAsync();
                TResponse responseObject = JsonConvert.DeserializeObject<TResponse>(jsonResponse);

                return responseObject;
            }
            else
            {
                string errorMessage = "";
                if (response.Content != null)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    // Assuming BasicResponse is the type returned in error cases
                    TResponse errorResponse = JsonConvert.DeserializeObject<TResponse>(errorContent);
                    return errorResponse;
                }
                else
                {
                    errorMessage = $"Error communicating with API: {response.StatusCode}";
                }
                throw new Exception(errorMessage);
                //throw new Exception($"Error communicating with API: {response.StatusCode}");
            }
        }

        public void SetBearer()
        {
            ResponseModel responseModel = new ResponseModel();
            if (string.IsNullOrEmpty(SetTokenModel.Token) || SetTokenModel.ExpireTime <= DateTime.Now)
            {
                GetTokenModel.GetToken(string.Empty).Wait();
            }
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SetTokenModel.Token);
        }
    }

}
