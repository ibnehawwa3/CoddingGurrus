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
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request, string apiEndpoint)
        {
            GetResult(apiEndpoint);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SetTokenModel.Token);
            // Serialize request object to JSON
            var jsonRequest = JsonConvert.SerializeObject(request);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(ApiUri.Info_API.APIUrl + apiEndpoint, content);

            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse> DeleteAsync<TResponse>(string apiEndpoint)
        {
            GetResult(apiEndpoint);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SetTokenModel.Token);

            var response = await httpClient.DeleteAsync(ApiUri.Info_API.APIUrl + apiEndpoint);

            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse> GetAsync<TResponse>(string apiEndpoint)
        {
            GetResult(apiEndpoint);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SetTokenModel.Token);
            var response = await httpClient.GetAsync(ApiUri.Info_API.APIUrl + apiEndpoint);

            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse> GetByIdAsync<TResponse>(string apiEndpoint, int id)
        {
            GetResult(apiEndpoint);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SetTokenModel.Token);
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

        public ResponseModel GetResult(string RequestUri, string ContentBody = "")
        {
            ResponseModel responseModel = new ResponseModel();
            if (string.IsNullOrEmpty(SetTokenModel.Token) || SetTokenModel.ExpireTime <= DateTime.Now)
            {
                GetTokenModel.GetToken().Wait();
            }
            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers["Authorization"] = String.Format("Bearer {0}", SetTokenModel.Token);

                    //string results = client.UploadString(ApiUri.GetAPIUrl + "/" + RequestUri, "Post", ContentBody);
                    //responseModel = JsonConvert.DeserializeObject<ResponseModel>(results);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        client.Headers["Authorization"] = String.Format("Bearer {0}", SetTokenModel.Token);

                        //string results = client.UploadString(ApiUri.GetAPIUrl + "/" + RequestUri, "Post", ContentBody);
                        //responseModel = JsonConvert.DeserializeObject<ResponseModel>(results);
                    }
                }
                catch (Exception e)
                {
                }
            }
            return responseModel;
        }
    }

}
