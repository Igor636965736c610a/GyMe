namespace GymAppCore.Models.Entities;

public class Premium
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public bool PremiumAccount { get; set; } = false;
    public DateTime? Importance { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
}