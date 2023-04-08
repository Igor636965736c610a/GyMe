namespace GymAppApi.Controllers.v1;

public static class UtilsControllers
{
    public static string GetUserIdFromClaim(HttpContext httpContext)
    {
        return httpContext.User.Claims.First(x => x.Type == "id").Value;
    }
}