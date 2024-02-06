using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace CoddingGurrus.web.Helper
{
    public static class ApiEndPoints
    {
        public const string GetUsers = "/api/User/list";
        public const string CreateUsers = "/api/Account/register";
    }

    public static class GridHeaderText
    {
        public const string User = "User List";
    }

    public static class GridButtonText
    {
        public const string User = "Create new user";
    }
}
