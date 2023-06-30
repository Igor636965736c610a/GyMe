namespace GymAppInfrastructure.Dtos.Authorization;

public class ResetPasswordResult
{
    public bool Success { get; set; }
    public IEnumerable<string> Errors { get; set; }
}