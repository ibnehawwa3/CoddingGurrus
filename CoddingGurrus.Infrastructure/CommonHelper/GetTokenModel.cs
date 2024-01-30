﻿using CoddingGurrus.Core.APIResponses;
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
        public static async Task<LoginResponseModel> GetToken()
        {
            LoginResponseModel getTokenModel = new LoginResponseModel();
            if (string.IsNullOrEmpty(SetTokenModel.Token))
            {
                if (string.IsNullOrEmpty(SetTokenModel.Token))
                {
                    try
                    {
                        Hashtable hashtable = new Hashtable();
                        hashtable.Add("email", "admin@admin.com");
                        hashtable.Add("password", "1234567aa");

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
                            return getTokenModel;
                        }
                        SetTokenModel.Token = getTokenModel.auth_token;
                        SetTokenModel.ExpireTime = getTokenModel.expiration_time;
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
