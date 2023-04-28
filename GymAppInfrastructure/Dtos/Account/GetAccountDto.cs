namespace GymAppInfrastructure.Dtos.Account;

public class GetAccountDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool PrivateAccount { get; set; }
    public bool Premium { get; set; }
    public DateTime? ImportancePremium { get; set; }
}