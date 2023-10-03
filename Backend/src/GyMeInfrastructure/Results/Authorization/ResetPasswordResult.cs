namespace GyMeInfrastructure.Results.Authorization;
public class ResetPasswordResult
{
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; }
}