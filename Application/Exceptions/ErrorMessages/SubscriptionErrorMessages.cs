namespace Application.Exceptions.ErrorMessages;

public class SubscriptionErrorMessages
{
    public const string NotValidSubscriptionName = "Некорректное название подписки";
    public const string NotValidSubscriptionDescription = "Некорректное описание подписки";
    public const string NotValidSubscriptionMaxResolution = "Максимальное разрешение должно быть больше нуля";
    public const string GivenIdOfNonExistingContent = "Дан id несуществующого контента";
    public const string SubscriptionNotFound = "Подписка с данным id не найден";
    public const string SubscriptionPriceLessThanZero = "Подписка не может стоить меньше нуля";
}