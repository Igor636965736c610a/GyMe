namespace GymAppCore.Models;

public class AuthenticationRegisterResult
{
    public string Token { get; set; }
    public string callbackUrlEmailToken { get; set; }
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; }
}