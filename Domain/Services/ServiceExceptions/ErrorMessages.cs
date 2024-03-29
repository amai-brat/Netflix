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
    }
}
