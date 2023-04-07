namespace GymAppInfrastructure.Dtos.Authorization;

public class AuthFailedResponse
{
    public IEnumerable<string> Errors { get; set; }
}