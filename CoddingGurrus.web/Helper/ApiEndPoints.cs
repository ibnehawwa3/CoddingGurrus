﻿using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace CoddingGurrus.web.Helper
{
    public static class ApiEndPoints
    {
        public const string GetUsers = "/api/User/list";
        public const string GetRole = "/api/Role";
        public const string CreateUsers = "/api/Account/register";
        public const string GetUserProfile = "/api/UserProfile/get-profile";
        public const string GetRoleById = "/api/Role/GetById";
        public const string UpdateUserProfile = "/api/UserProfile/update-profile";
        public const string DeleteUserProfile = "/api/UserProfile/delete";
        public const string Login = "api/account/login";
    }

    public static class GridHeaderText
    {
        public const string User = "User List";
        public const string Role = "Role List";
    }


    public enum ActionType
    {
        Create,
        Edit,
        Delete,
        Login,
        Search
    }

    public enum ControllerName
    {
        Role,
        User
    }

    public static class GridButtonText
    {
        public const string User = "Create new user";
        public const string Role = "Create new role";
    }
}
