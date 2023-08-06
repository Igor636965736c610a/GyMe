namespace GymAppInfrastructure.Results;

public class ActivateUserResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}