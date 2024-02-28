
namespace CoddingGurrus.web.Helper
{
    public static class ApiEndPoints
    {
        //user
        public const string GetUsers = "/api/User/list";
        public const string GetMenus = "/api/Menu/list";
        public const string GetRole = "/api/Role";
        public const string GetRoleMenuPermission = "/api/RoleMenuPermission/list";
        public const string CreateUsers = "/api/Account/register";
        public const string CreateMenus = "/api/Menu/add";
        public const string UpdateMenus = "/api/Menu/update";
        public const string DeleteMenus = "/api/Menu/delete";
        public const string CreateRole = "/api/Role/PostRole";
        public const string CreateRoleMenuPermission = "/api/RoleMenuPermission/add";
        public const string GetUserProfile = "/api/UserProfile/get-profile";
        public const string GetRoleById = "/api/Role/GetById";
        public const string GetMenuById = "/api/Menu/GetById";
        public const string GetRoleMenuPermissionById = "/api/RoleMenuPermission/GetById";
        public const string UpdateUserProfile = "/api/UserProfile/update-profile";
        public const string UpdateRole = "/api/Role/Put";
        public const string UpdateRoleMenuPermission = "/api/RoleMenuPermission/Put";
        public const string DeleteRole = "/api/Role/Delete";
        public const string DeleteRoleMenuPermission = "/api/RoleMenuPermission/Delete";
        public const string DeleteUserProfile = "/api/UserProfile/delete";
        public const string Login = "api/account/login";
        //course
        public const string GetCourses = "/api/Course/list";
        public const string CreateCourse = "/api/Course/add";
        public const string UpdateCourse = "/api/Course/update";
        public const string DeleteCourse = "/api/Course/delete";
        public const string GetCourseById = "/api/Course/get-course";
        //content
        public const string GetContents = "/api/Content/list";
        public const string CreateContent = "/api/Content/add";
        public const string UpdateContent = "/api/Content/update";
        public const string DeleteContent = "/api/Content/delete";
        public const string GetContentById = "/api/Content/get-Content";
    }

    public static class GridHeaderText
    {
        public const string User = "User List";
        public const string Role = "Role List";
        public const string Menu = "Menu List";
        public const string Course = "Course List";
        public const string Content = "Content List";
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
        User,
        Menu,
        Course,
        Content
    }

    public static class GridButtonText
    {
        public const string User = "Create new user";
        public const string Role = "Create new role";
        public const string Menu = "Create new menu";
        public const string Course = "Create new course";
        public const string Content = "Create new content";
    }
}
