using System.Web;

namespace Infrastructure.Helpers;

public static class EmailMessageHelper
{
    private static readonly string EmailConfirmationEndpoint = Environment.GetEnvironmentVariable("EMAIL_CONFIRMATION_ENDPOINT")!;
    private static readonly string EmailChangeConfirmationEndpoint = Environment.GetEnvironmentVariable("EMAIL_CHANGE_CONFIRMATION_ENDPOINT")!;
    
    public static string GetEmailConfirmationMessage(string confirmationToken, long userId)
    {
        var encodedToken = HttpUtility.UrlEncode(confirmationToken);
        return $"""
                Чтобы подтвердить почту, нажмите на <a href="{EmailConfirmationEndpoint + $"?token={encodedToken}&userId={userId}"}">ссылку</a>
                """;
    }

    public static string GetEmailChangeConfirmationMessage(string confirmationToken, long userId, string newEmail)
    {
        var encodedToken = HttpUtility.UrlEncode(confirmationToken);
        return $"""
                Чтобы изменить почту, нажмите на <a href="{EmailChangeConfirmationEndpoint + $"?token={encodedToken}&userId={userId}"}&newEmail={newEmail}">ссылку</a>
                """; 
    }

    public static string GetTwoFactorTokenMessage(string token)
    {
        return $"""
                Введите токен на сайте: <h1 style="color:red;">{token}</h1>
                """;
    }
}