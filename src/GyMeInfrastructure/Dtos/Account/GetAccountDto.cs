namespace GymAppInfrastructure.Dtos.Account;

public class GetAccountDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public bool PrivateAccount { get; set; }
    public bool Valid { get; set; }
    public bool Premium { get; set; }
    public DateTime? ImportancePremium { get; set; }
}