

namespace CoddingGurrus.Core.APIResponses
{
    public class LoginResponseModel
    {
        public string auth_token { get; set; }
        public string token_type { get; set; }
        public DateTime issue_time { get; set; }
        public DateTime expiration_time { get; set; }
        public string role { get; set; }
    }
}
