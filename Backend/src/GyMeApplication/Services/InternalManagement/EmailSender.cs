using System.Net;
using System.Net.Mail;
using FluentEmail.Core;
using FluentEmail.Mailgun;
using GyMeApplication.Models.InternalManagement;
using GyMeApplication.Options;
using Microsoft.Extensions.Options;

namespace GyMeApplication.Services.InternalManagement;

public interface IEmailSender
{
    Task<bool> SendEmail(string body, string subject, string email);
}

public class MailgunEmailSender : IEmailSender
{
    private readonly MailgunEmailOptions _mailgunEmailOptions;
    
    public MailgunEmailSender(IOptionsMonitor<MailgunEmailOptions> emailOptions)
    {
        _mailgunEmailOptions = emailOptions.CurrentValue;
    }
    
    public async Task<bool> SendEmail(string body, string subject, string email)
    {
        var sender = new MailgunSender(
            _mailgunEmailOptions.DomainName,
            _mailgunEmailOptions.ApiKey);
        
        Email.DefaultSender = sender;
        var emailToSend = Email
            .From(_mailgunEmailOptions.From)
            .To(email)
            .Subject(subject)
            .Body(body);

        var response = await emailToSend.SendAsync();

        return response.Successful;
    }
}

public class OwnSmtpEmailSender : IEmailSender
{
    private readonly ErrorService _errorService;
    private readonly SmtpOptions _smtpOptions;
    
    public OwnSmtpEmailSender(IOptionsMonitor<SmtpOptions> smtpOptions, ErrorService errorService)
    {
        _errorService = errorService;
        _smtpOptions = smtpOptions.CurrentValue;
    }
    
    public async Task<bool> SendEmail(string body, string subject, string email)
    {
        var client = new SmtpClient(_smtpOptions.Mail, 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(_smtpOptions.Mail, _smtpOptions.Password)
        };
        try
        {
            await client.SendMailAsync(new MailMessage(
                from: _smtpOptions.Mail,
                to: email,
                subject,
                body));
            
            return true;
        }
        catch(Exception e)
        {
            var errorEntity = CreateError(e, 500);
            await _errorService.Add(errorEntity);
            
            return false;
        }
    }
    
    private Error CreateError(Exception exception, int statusCode)
        => new()
        {
            StatusCode = statusCode,
            StackStrace = exception.StackTrace,
            Message = exception.Message
        };
}