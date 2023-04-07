using System.Net;
using System.Net.Mail;
using GymAppInfrastructure.IServices;
using GymAppInfrastructure.Options;
using Microsoft.Extensions.Options;

namespace GymAppInfrastructure.Services;

public class EmailSender : IEmailSender
{
    private readonly EmailSenderOptions _emailSenderOptions;

    public EmailSender(IOptions<EmailSenderOptions> emailSenderOptions)
    {
        _emailSenderOptions = emailSenderOptions.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MailMessage();
        message.To.Add(new MailAddress(email));
        message.Subject = subject;
        message.Body = htmlMessage;
        message.IsBodyHtml = true;
        message.From = new MailAddress(_emailSenderOptions.FromEmail);

        using (var smtpClient = new SmtpClient(_emailSenderOptions.Host, _emailSenderOptions.Port))
        {
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_emailSenderOptions.Username, _emailSenderOptions.Password);
            smtpClient.EnableSsl = _emailSenderOptions.EnableSsl;
            await smtpClient.SendMailAsync(message);
        }
    }
}