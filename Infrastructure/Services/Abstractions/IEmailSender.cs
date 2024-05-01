namespace Infrastructure.Services.Abstractions;

public interface IEmailSender
{
    public Task SendEmailAsync(string email, string message);
}