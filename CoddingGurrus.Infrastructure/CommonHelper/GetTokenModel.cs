using CoddingGurrus.Core.APIResponses;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CoddingGurrus.Infrastructure.CommonHelper
{
    public class GetTokenModel
    {
        public string auth_token { get; set; }
        public static async Task GetToken()/*IConfiguration _config*/
        {
            if (string.IsNullOrEmpty(SetTokenModel.Token))
            {
                LoginResponseModel getTokenModel = new LoginResponseModel();

                try
                {
                    //string filePath = @"C:\token\";
                    string filePath = Directory.GetCurrentDirectory() + @"\wwwroot\token\";
                    string fileName = "token.json";
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    bool getTokenFromAPI = false;

                    if (!File.Exists(filePath + fileName))
                    {
                        getTokenFromAPI = true;
                    }
                    else
                    {
                        getTokenModel = GetTokenFromFile(filePath + fileName);
                        if (string.IsNullOrEmpty(getTokenModel.auth_token) || getTokenModel.expiration_time <= DateTime.Now)
                        {
                            getTokenFromAPI = true;
                        }
                    }

                    if (getTokenFromAPI)
                    {
                        Hashtable hashtable = new Hashtable();
                        hashtable.Add("userName", "stats24mobileapi");/*_config.GetConnectionString("email")*/
                        hashtable.Add("password", "StaT24Wob1l3Ap1");/*_config.GetConnectionString("password")*/

                        var json = JsonConvert.SerializeObject(hashtable);
                        var url = "api/account/login";
                        //var requestToken = new HttpRequestMessage
                        //{
                        //    Method = HttpMethod.Post,
                        //    RequestUri = new Uri(new Uri(ApiUri.Uri), url),/*_config.GetConnectionString("ApiUri")*/
                        //    Content = new StringContent(json, Encoding.UTF8, "application/json")
                        //};

                        using (var client = new WebClient())
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

                            ResponseModel responseModel = new ResponseModel();
                            client.Headers[HttpRequestHeader.ContentType] = "application/json";
                            client.Headers["Authorization"] = String.Format("Bearer {0}", SetTokenModel.Token);

                            string results = client.UploadString(ApiUri.Info_API.APIUrl + "/" + url, "Post", json);
                            responseModel = JsonConvert.DeserializeObject<ResponseModel>(results);

                            getTokenModel = JsonConvert.DeserializeObject<LoginResponseModel>(Convert.ToString(responseModel.Data));

                            try
                            {
                                File.WriteAllText(filePath + fileName, JsonConvert.SerializeObject(getTokenModel));
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    //using (var client = new HttpClient())
                    //{
                    //    ResponseModel responseModel = new ResponseModel();
                    //    var res = await client.SendAsync(requestToken);
                    //    if (res.IsSuccessStatusCode)
                    //    {
                    //        string result = res.Content.ReadAsStringAsync().Result;
                    //        responseModel = JsonConvert.DeserializeObject<ResponseModel>(result);
                    //    }
                    //    //getTokenModel = JsonConvert.DeserializeObject<GetTokenModel>(Convert.ToString(responseModel.Data));
                    //    getTokenModel = JsonConvert.DeserializeObject<LoginResponseModel>(Convert.ToString(responseModel.Data));
                    //}
                    SetTokenModel.Token = getTokenModel.auth_token;
                    SetTokenModel.ExpireTime = getTokenModel.expiration_time;
                }
                catch (Exception ex)
                {
                }
                //httpContext.Session.SetString("_token", getTokenModel.auth_token);
            }
        }

        public static LoginResponseModel GetTokenFromFile(string filePath)
        {
            LoginResponseModel getTokenModel = new LoginResponseModel();

            try
            {
                if (File.Exists(filePath))
                {
                    getTokenModel = JsonConvert.DeserializeObject<LoginResponseModel>(File.ReadAllText(filePath));
                }
            }
            catch (Exception ex)
            {
                getTokenModel = new LoginResponseModel();
            }

            return getTokenModel;
        }
    }
}
