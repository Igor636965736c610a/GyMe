namespace GymAppCore.Models.Entities;

public class FriendRequest
{
    public Guid SenderId { get; set; }
    public User Sender { get; set; }
    public Guid RecipientId { get; set; }
    public User Recipient { get; set; }
}