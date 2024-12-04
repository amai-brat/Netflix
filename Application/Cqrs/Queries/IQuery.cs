using MediatR;

namespace Application.Cqrs.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse>;