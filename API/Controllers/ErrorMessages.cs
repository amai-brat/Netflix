namespace API.Controllers
{
    public class ErrorMessages
    {
        public const string NotFoundContent = "Контент не найден для Id";
        public static string NotFoundContentError(long id) => $"{NotFoundContent} {id}";
    }
}
