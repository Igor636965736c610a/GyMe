namespace GymAppApi.Routes.v1;

public static class ApiRoutes
{
    public static class Account
    {
        public const string Login = "login";
        public const string ExternalLogin = "externalLogin";
        public const string HandleExternalLogin = "handleExternalLogin";
        public const string GetAccountInformation = "get";
        public const string Register = "register";
        public const string ConfirmEmail = "confirmEmail";
        public const string ActivateUser = "activateUser";
        public const string SetProfilePicture = "setProfilePicture";
        public const string UpdateUser = "update";
        public const string RemoveUser = "remove";
        public const string SendResetPasswordToken = "resetPassword/sendToken";
        public const string ResetPassword = "resetPassword";
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
        public const string DeleteFriend = "deleteFriend/{id}";
        public const string GetUser = "get{id}";
        public const string FindUser = "findUser";
        public const string GetFriends = "getFriends";
        public const string GetCommonFriends = "getCommonFriends";
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
        public const string GetByType = "getByType";
        public const string GetAllByIds = "getAllByIds";
        public const string GetAllByTypes = "getAllByTypes";
    }
    
    public static class Payments
    {
        public const string RedirectToPayment = "redirectToPayment";
        public const string Webhook = "webhook";
    }
    
    public static class UserFeedback
    {
        public const string SendOpinion = "opinion";
    }
    
    public static class Reaction
    {
        public const string AddReaction = "add";
        public const string SetImageReaction = "set/imageReaction";
        public const string GetReactions = "get";
        public const string GetSpecificReactionsCount = "get/count";
        public const string RemoveReaction = "remove";
    }

    public static class Comments
    {
        public const string AddComment = "add";
        public const string GetComment = "get/{id}";
        public const string GetCommentsSortedByPubTime = "get/sortByPubTime";
        public const string GetCommentsSortedByReactionsCount = "get/sortByReactionsCount";
        public const string UpdateComment = "update";
        public const string RemoveComment = "remove";
    }
    
    public static class MainPage
    {
        public const string GetNewSimpleExerciseElements = "get/new";
        public const string GetPastSimpleExerciseElements = "get/past";
    }
}