namespace GymAppInfrastructure.IServices;

public interface IEmailConfirmationService
{
    Task SendConfirmationEmailAsync(string email, string callbackUrl);
}