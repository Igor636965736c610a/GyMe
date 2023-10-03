using GyMeInfrastructure.Models.User;

namespace GyMeInfrastructure.Models.Account;

public class GetAccountInfModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public bool PrivateAccount { get; set; }
    public bool Valid { get; set; }
    public string Gender { get; set; }
    public string ProfilePictureUrl { get; set; }
    public string? Description { get; set; }
    public bool Premium { get; set; }
    public DateTime? ImportancePremium { get; set; }
}