namespace GymAppInfrastructure.Results.Authorization;

public class AuthenticationLoginResult
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public bool ValidUser { get; set; }
    public bool Success { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}