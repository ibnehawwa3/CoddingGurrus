using CoddingGurrus.Core.APIResponses;
using Newtonsoft.Json;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace CoddingGurrus.Infrastructure.CommonHelper
{
    public class GetTokenModel
    {
        public static async Task<LoginResponseModel> GetToken(string ContentBody)
        {
            LoginResponseModel getTokenModel = new LoginResponseModel();
            if (string.IsNullOrEmpty(SetTokenModel.Token))
            {
                if (string.IsNullOrEmpty(SetTokenModel.Token))
                {
                    try
                    {
                        Hashtable hashtable = JsonConvert.DeserializeObject<Hashtable>(ContentBody);
                        //hashtable.Add("email", "nzi@gmail.com");
                        //hashtable.Add("password", "Niaz123!@#");

                        var json = JsonConvert.SerializeObject(hashtable);
                        var url = "api/account/login";

                        using (var client = new WebClient())
                        {
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072 | SecurityProtocolType.Tls
                                   | SecurityProtocolType.Tls11
                                   | SecurityProtocolType.Tls12;
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
                            SetTokenModel.Token = getTokenModel.auth_token;
                            SetTokenModel.ExpireTime = getTokenModel.expiration_time;
                            return getTokenModel;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return getTokenModel;
        }
    }
}
