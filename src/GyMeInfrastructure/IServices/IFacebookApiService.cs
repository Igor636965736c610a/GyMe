using GymAppInfrastructure.ApiResponses.Facebook;
using Refit;

namespace GymAppInfrastructure.IServices;

public interface IFacebookApiService
{
    [Get("/debug_token")]
    Task<FacebookDebugTokenResponse> VerifyToken([Query("input_token")] string inputToken, [Query("access_token")] string accessToken);
}