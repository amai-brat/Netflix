using Application.Services.Abstractions;

namespace Infrastructure.Services.Implementations;

public class FakeEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string message)
    {
        Console.WriteLine($"Message for {email}: {message}");
        return Task.CompletedTask;
    }
}