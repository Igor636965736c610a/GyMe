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
        public const string RemoveUser = "remove";
    }

    public static class Exercise
    {
        public const string Create = "create";
        public const string Update = "update/{id}";
        public const string GetAll = "get";
        public const string Get = "get/{id}";
        public const string Remove = "remove/{id}";
    }
    
    public static class User
    {
        public const string AddFriend = "addFriend/{id}";
        public const string DeleteFriendRequest = "deleteFriendRequest/{id}";
        public const string DeleteFriend = "deleteFriend/{id}";
        public const string GetUser = "get{id}";
        public const string GetFriends = "getFriends";
    }
    
    public static class SimpleExercise
    {
        public const string Create = "create";
        public const string Update = "update/{id}";
        public const string GetAll = "get";
        public const string Get = "get/{id}";
        public const string Remove = "remove/{id}";
    }
    
    public static class Chart
    {
        public const string GetById = "getById/{exerciseId}";
        public const string GetByType = "getByType/{userId}/{exerciseType}";
        public const string GetAllByIds = "getAllByIds";
        public const string GetAllByTypes = "getAllByTypes";
    }
}