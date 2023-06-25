using System.Text.Encodings.Web;
using GymAppInfrastructure.IServices;

namespace GymAppInfrastructure.Services;

internal class EmailConfirmationService : IEmailConfirmationService
{
    private readonly IEmailSender _emailSender;

    public EmailConfirmationService(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task SendConfirmationEmailAsync(string email, string callbackUrl)
    {
        string messageSubject = "Confirm your email address";
        string messageBody = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
        
        await _emailSender.SendEmailAsync(email, messageSubject, messageBody);
    }
}