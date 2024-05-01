using System.Web;

namespace Infrastructure.Helpers;

public static class EmailMessageHelper
{
    private const string EmailConfirmationEndpoint = "https://localhost:7173/auth/confirm-email";
    private const string EmailChangeConfirmationEndpoint = "http://localhost:7173/auth/confirm-email-change";
    
    public static string GetEmailConfirmationMessage(string confirmationToken, long userId)
    {
        var encodedToken = HttpUtility.UrlEncode(confirmationToken);
        return $"""
                Чтобы подтвердить почту, нажмите на <a href="{EmailConfirmationEndpoint + $"?token={encodedToken}&userId={userId}"}">ссылку</a>"
                """;
    }

    public static string GetEmailChangeConfirmationMessage(string confirmationToken, long userId, string newEmail)
    {
        var encodedToken = HttpUtility.UrlEncode(confirmationToken);
        return $"""
                Чтобы изменить почту, нажмите на <a href="{EmailChangeConfirmationEndpoint + $"?token={encodedToken}&userId={userId}"}&newEmail={newEmail}">ссылку</a>"
                """; 
    }
}