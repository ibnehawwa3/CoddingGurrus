using CoddingGurrus.Core.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Infrastructure.CommonHelper.Handler
{
    public class BaseHandler:IBaseHandler
    {
        private HttpClient httpClient;

        public BaseHandler()
        {
            httpClient = new HttpClient();
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(TRequest request, string apiEndpoint)
        {
            string jsonRequest = JsonConvert.SerializeObject(request);

            var response = await httpClient.PostAsync(ApiUri.Info_API.APIUrl + apiEndpoint, new StringContent(jsonRequest, Encoding.UTF8, "application/json"));

            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse> GetAsync<TResponse>(string apiEndpoint)
        {
            var response = await httpClient.GetAsync(ApiUri.Info_API.APIUrl + apiEndpoint);

            return await HandleResponse<TResponse>(response);
        }

        public async Task<TResponse> GetByIdAsync<TResponse>(string apiEndpoint, string id)
        {
            string fullApiEndpoint = $"{ApiUri.Info_API.APIUrl+apiEndpoint}/{id}";

            var response = await httpClient.GetAsync(fullApiEndpoint);

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
    }

}
