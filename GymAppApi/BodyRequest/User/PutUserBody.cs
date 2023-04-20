namespace GymAppApi.BodyRequest.User;

public class PutUserBody
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool PrivateAccount { get; set; }
}