using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace CoddingGurrus.web.Helper
{
    public static class ApiEndPoints
    {
        public const string GetUsers = "/api/User/list";
        public const string CreateUsers = "/api/Account/register";
        public const string GetUserProfile = "/api/UserProfile/get-profile";
        public const string UpdateUserProfile = "/api/UserProfile/update-profile";
        public const string DeleteUserProfile = "/api/UserProfile/delete";
        public const string Login = "api/account/login";

        public const string GetMenus = "/api/Menu/list";
        public const string CreateMenu = "/api/Menu/add";
        public const string UpdateMenu = "/api/Menu/update";
        public const string DeleteMenu = "/api/Menu/delete";
    }

    public static class GridHeaderText
    {
        public const string User = "User List";
        public const string Menu = "Menu List";
    }

    public static class GridButtonText
    {
        public const string User = "Create new user";
        public const string Menu = "Create new Menu";
    }
}
