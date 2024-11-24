using Application.Exceptions.Base;
using FluentValidation;
using MediatR;

namespace Application.Cqrs.PipelineBehaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }
        
        var validationResultTasks = validators
            .Select(x => x.ValidateAsync(request, cancellationToken));
        var validationResult = await Task.WhenAll(validationResultTasks);

        if (!validationResult.All(x => x.IsValid))
        {
            var failures = validationResult.SelectMany(x => x.Errors);
            throw new ArgumentValidationException(
                string.Join("; ", failures.Select(x => x.ErrorMessage)));
        }

        return await next();
    }
}