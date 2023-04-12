namespace GymAppApi.Routes.v1;

public static class ApiRoutes
{
    public static class Account
    {
        public const string Login = "login";
        public const string Register = "register";
        public const string ConfirmEmail = "confirmEmail";
    }

    public static class Exercise
    {
        public const string Create = "create";
        public const string Update = "update";
        public const string GetAll = "getAll";
        public const string Get = "get";
        public const string Remove = "remove";
    }
}