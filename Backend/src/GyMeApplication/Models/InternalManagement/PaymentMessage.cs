namespace GyMeApplication.Models.InternalManagement;

public class PaymentMessage
{
    public string PaymentIntentId { get; set; }
    public string Email { get; set; }
    public long Amount { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}