using CoddingGurrus.Core.APIResponses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Infrastructure.CommonHelper
{
    public static class DownloadManager
    {
        public static ResponseModel DownloadDataUsingWebClient(string RequestUri, string ContentBody)
        {
            ResponseModel responseModel = new ResponseModel();

            using (var client = new WebConnection())
            {
                try
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Tls
                           | SecurityProtocolType.Tls11
                           | SecurityProtocolType.Tls12;
                    //| SecurityProtocolType.Ssl3;
                    ServicePointManager.ServerCertificateValidationCallback =
                         delegate (
                             object s,
                             X509Certificate certificate,
                             X509Chain chain,
                             SslPolicyErrors sslPolicyErrors
                         )
                         {
                             return true;
                         };

                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers["Authorization"] = String.Format("Bearer {0}", SetTokenModel.Token);

                    client.Timeout = ApiUri.Info_API.Timeout;
                    string results = client.UploadString(ApiUri.Info_API.APIUrl + "/" + RequestUri, "Post", ContentBody);
                    responseModel = JsonConvert.DeserializeObject<ResponseModel>(results);
                }
                catch (Exception ex)
                {
                    string dateFormat = DateTime.Now.ToString("ddMMyyyy");
                    LogHelper._LogMessageTraceIssue($"{RequestUri}", "exception-api-webclient-" + RequestUri.Replace("/", "-") + dateFormat + ".txt");
                    LogHelper._LogMessageTraceIssue($"{ContentBody}", "exception-api-webclient-" + RequestUri.Replace("/", "-") + dateFormat + ".txt");
                    LogHelper._LogMessageTraceIssue($"{ex.Message}", "exception-api-webclient-" + RequestUri.Replace("/", "-") + dateFormat + ".txt");
                    if (ex.InnerException != null)
                    {
                        LogHelper._LogMessageTraceIssue($"{ex.InnerException.Message}", "exception-api-webclient-" + RequestUri.Replace("/", "-") + dateFormat + ".txt");
                    }
                }
            }

            return responseModel;
        }

        public static ResponseModel DownloadDataUsingHttpClient(string RequestUri, string ContentBody)
        {
            ResponseModel responseModel = new ResponseModel();

            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12;
                //| SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback =
                     delegate (
                         object s,
                         X509Certificate certificate,
                         X509Chain chain,
                         SslPolicyErrors sslPolicyErrors
                     )
                     {
                         return true;
                     };

                client.BaseAddress = new Uri(new Uri(ApiUri.Info_API.APIUrl), RequestUri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SetTokenModel.Token);

                try
                {
                    HttpContent _Body = new StringContent(ContentBody);
                    _Body.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var res = client.PostAsync(client.BaseAddress.AbsoluteUri, _Body).Result;
                    //var res = client.PostAsync(new Uri(client.BaseAddress, RequestUri), _Body).Result;

                    if (res.IsSuccessStatusCode)
                    {
                        string results = res.Content.ReadAsStringAsync().Result;
                        responseModel = JsonConvert.DeserializeObject<ResponseModel>(results);
                    }
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(1000);
                    string dateFormat = DateTime.Now.ToString("ddMMyyyy");
                    LogHelper._LogMessageTraceIssue($"{RequestUri}", "exception-api-httpclient-" + RequestUri.Replace("/", "-") + dateFormat + ".txt");
                    LogHelper._LogMessageTraceIssue($"{ContentBody}", "exception-api-httpclient-" + RequestUri.Replace("/", "-") + dateFormat + ".txt");
                    LogHelper._LogMessageTraceIssue($"{ex.Message}", "exception-api-httpclient-" + RequestUri.Replace("/", "-") + dateFormat + ".txt");
                    if (ex.InnerException != null)
                    {
                        LogHelper._LogMessageTraceIssue($"{ex.InnerException.Message}", "exception-api-httpclient-" + RequestUri.Replace("/", "-") + dateFormat + ".txt");
                    }
                }
            }

            return responseModel;
        }
    }
}
