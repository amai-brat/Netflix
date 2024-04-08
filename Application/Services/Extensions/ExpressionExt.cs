using System.Linq.Expressions;

namespace Application.Services.Extensions
{
    public static class ExpressionExt
    {
        public static Expression<Func<T, bool>> CombineExpressions<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var body = Expression.AndAlso(
                    Expression.Invoke(expr1, param),
                    Expression.Invoke(expr2, param)
                );
            var lambda = Expression.Lambda<Func<T, bool>>(body, param);
            return lambda;
        }
    }
}
