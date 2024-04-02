using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.ServiceExceptions
{
    public class ErrorMessages
    {
        public const string ReviewMustHaveText = "Рецензия должна быть заполненной";
        public const string ScoreMustBeValid = "Оценка должна быть от 0 до 10";
        public const string ArgumentsMustBePositive = "Аргументы должны быть целыми неотрицательными числами";
        public const string IncorrectSortType = "Указана неизвестная сортировка";
        public const string NotFoundContent = "Контента с указанным id не существует";
        public const string NotFoundUser = "Пользователя с указанным id не существует";
        public const string AlreadyFavourite = "Контент уже в избранном";
        public const string NotInFavourite = "Контент уже не входит в избранное";
        public const string NotFoundResolution = "Указанного разрешения нет";
        public const string UserDoesNotHaveSubscription = "Нельзя смотреть контент без подписки";
        public const string UserDoesNotHavePermissionBySubscription = "Контент не доступен с текущей подпиской";
        public const string NotFoundSeason = "Указан неверный сезон";
        public const string NotFoundEpisode = "Указан неверный эпизод";
    }
}
