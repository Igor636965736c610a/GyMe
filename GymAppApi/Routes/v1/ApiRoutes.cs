namespace GymAppApi.Routes.v1;

public static class ApiRoutes
{
    public static class Account
    {
        public const string Login = "login";
        public const string GetAccountInformation = "get";
        public const string Register = "register";
        public const string ConfirmEmail = "confirmEmail";
        public const string UpdateUser = "update";
    }

    public static class Exercise
    {
        public const string Create = "create";
        public const string Update = "update";
        public const string GetAll = "getAll";
        public const string GetAllForeign = "getAllForeign";
        public const string Get = "get";
        public const string Remove = "remove";
    }
    
    public static class User
    {
        public const string AddFriend = "addFriend";
        public const string DeleteFriendRequest = "deleteFriendRequest";
        public const string DeleteFriend = "deleteFriend";
        public const string GetUser = "getUser";
    }
    
    public static class SimpleExercise
    {
        public const string Create = "create";
        public const string Update = "update";
        public const string GetAll = "getAll";
        public const string GetAllForeign = "getAllForeign";
        public const string Get = "get";
        public const string Remove = "remove";
    }
}