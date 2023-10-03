namespace GyMeInfrastructure.Models.InternalManagement;

public class Opinion
{
    public string Email { get; set; }
    public string Message { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}