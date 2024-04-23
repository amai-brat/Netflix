namespace Application.Exceptions
{
    public class ErrorMessages
    {
        public const string ReviewMustHaveText = "Рецензия должна быть заполненной";
        public const string CommentMustHaveText = "Комментарий должнен быть заполненным";
        public const string ScoreMustBeValid = "Оценка должна быть от 0 до 10";
        public const string ArgumentsMustBePositive = "Аргументы должны быть целыми неотрицательными числами";
        public const string IncorrectSortType = "Указана неизвестная сортировка";
        public const string NotFoundContent = "Контента с указанным id не существует";
        public const string NotFoundReview = "Рецензии с указанным id не существует";
        public const string NotFoundComment = "Комментарий с указанным id не существует";
        public const string NotFoundNotification = "Уведомления с указанным id не существует";
        public const string NotFoundUser = "Пользователь не найден";
        public const string AlreadyFavourite = "Контент уже в избранном";
        public const string NotInFavourite = "Контент уже не входит в избранное";
        public const string NotFoundResolution = "Указанного разрешения нет";
        public const string UserDoesNotHaveSubscription = "Нельзя смотреть контент без подписки";
        public const string UserDoesNotHavePermissionBySubscription = "Контент не доступен с текущей подпиской";
        public const string NotFoundSeason = "Указан неверный сезон";
        public const string NotFoundEpisode = "Указан неверный эпизод";
        public const string InvalidEmail = "Дана некорректная почта";
        public const string InvalidBirthday = "Невозможный день рождения";
        public const string IncorrectPassword = "Неправильный пароль";
        public const string IncorrectRole = "Неправильная роль (user/admin/moderator)";
        public const string RefreshTokenNotFound = "Refresh токен не найден";
        public const string NotActiveRefreshToken = "Refresh токен не активен";
        public const string EmailNotUnique = "Пользователь с данной почтой существует";
    }
}
