namespace GymAppCore.Models;

public class AuthenticationRegisterResult
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; }
}