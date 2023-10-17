namespace GyMeApplication.Results.Authorization;
public class AuthenticationRegisterResult
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
    public bool Success { get; set; }
    public IEnumerable<string> Messages { get; set; }
    public IEnumerable<string> Errors { get; set; }
}