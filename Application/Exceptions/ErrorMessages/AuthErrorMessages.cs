namespace Application.Exceptions.ErrorMessages;

public static class AuthErrorMessages
{
    public const string EmailNotConfirmed = "Почта не подтверждена";
    public const string InvalidConfirmationToken = "Неправильный токен подтверждения";
    public const string InvalidTwoFactorToken = "Неправильный токен двухфакторной аутентификации";
}