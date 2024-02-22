using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace CoddingGurrus.web.Helper
{
    public static class ApiEndPoints
    {
        //user
        public const string GetUsers = "/api/User/list";
        public const string CreateUsers = "/api/Account/register";
        public const string GetUserProfile = "/api/UserProfile/get-profile";
        public const string UpdateUserProfile = "/api/UserProfile/update-profile";
        public const string DeleteUserProfile = "/api/UserProfile/delete";
        public const string Login = "api/account/login";
        //course
        public const string GetCourses = "/api/Course/list";
        public const string CreateCourse = "/api/Course/add";
        public const string UpdateCourse = "/api/Course/update";
        public const string DeleteCourse = "/api/Course/delete";
        public const string GetCourseById = "/api/Course/get-course";
    }

    //public static class GridHeaderText
    //{
    //    public const string User = "User List";
    //}

    //public static class GridButtonText
    //{
    //    public const string User = "Create new user";
    //}
}
