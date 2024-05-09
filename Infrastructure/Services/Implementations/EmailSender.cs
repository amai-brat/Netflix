using Infrastructure.Options;
using Infrastructure.Services.Abstractions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Services.Implementations;

public class EmailSender(
    IOptionsMonitor<EmailOptions> optionsMonitor,
    ILogger<EmailSender> logger) : IEmailSender
{
    private readonly EmailOptions _emailOptions = optionsMonitor.CurrentValue;
    
    public async Task SendEmailAsync(string email, string message)
    {
        var mailMessage = CreateEmailMessage(email, message);
        await SendAsync(mailMessage);
    }
    
    private MimeMessage CreateEmailMessage(string to, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(_emailOptions.From.Split("@")[0], _emailOptions.From));
        emailMessage.To.Add(new MailboxAddress("", to));
        emailMessage.Subject = "Netflix";
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };
        return emailMessage;
    }
    
    private async Task SendAsync(MimeMessage mailMessage)
    {
        using var client = new SmtpClient();
        try
        {
            await client.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.Port, SecureSocketOptions.StartTls);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            await client.AuthenticateAsync(_emailOptions.UserName, _emailOptions.Password);
            await client.SendAsync(mailMessage);
        }
        catch(Exception ex)
        {
            logger.LogError("Failed to send message {mailMessage} with error {message}", mailMessage, ex.Message);
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}