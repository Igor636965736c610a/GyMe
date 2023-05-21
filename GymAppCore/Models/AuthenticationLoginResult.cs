namespace GymAppCore.Models;

public class AuthenticationLoginResult
{
    public string Token { get; set; }
    public bool Success { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}