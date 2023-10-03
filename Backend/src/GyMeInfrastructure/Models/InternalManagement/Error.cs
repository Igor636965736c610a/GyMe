namespace GyMeInfrastructure.Models.InternalManagement;

public class Error
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? StackStrace { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}