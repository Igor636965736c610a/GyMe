using GymAppInfrastructure.Models.User;

namespace GymAppInfrastructure.Models.Account;

public class ActivateAccountModel
{
    public string UserName { get; set; }
    public GenderDto Gender { get; set; }
    public byte[] ProfilePicture { get; set; }
    public bool PrivateAccount { get; set; }
    public bool Premium { get; set; } = false;
    public DateTime? ImportancePremium { get; set; }
}