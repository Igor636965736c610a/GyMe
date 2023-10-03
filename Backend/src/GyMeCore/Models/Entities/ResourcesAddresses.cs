namespace GyMeCore.Models.Entities;

public class ResourcesAddresses
{
    protected ResourcesAddresses()
    {
    }
    
    public ResourcesAddresses(Guid id, string reactionImageUrl, Guid userId)
    {
        Id = id;
        ReactionImageUrl = reactionImageUrl;
        UserId = userId;
    }
    public Guid Id { get; set; }
    public string ReactionImageUrl { get; set; }
    public User User { get; set; }
    public Guid UserId { get; set; }
}