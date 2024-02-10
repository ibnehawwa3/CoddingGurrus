using CoddingGurrus.Core.APIResponses;
using CoddingGurrus.Core.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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
                throw new Exception($"Error communicating with API: {response.StatusCode}");
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
