namespace Application.Exceptions.Base;

public class ValueChangedException(
    string? message = "Пока вы редактировали ресурс, он изменился в другом процессе, повторите запрос еще раз" +
                      "(обновите страницу) и посмотрите, не устраивает ли вас новое значение")
    : Exception(message);