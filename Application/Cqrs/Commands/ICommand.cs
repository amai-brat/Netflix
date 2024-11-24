using MediatR;

namespace Application.Cqrs.Commands;

public interface ICommand : IRequest;

public interface ICommand<out TResponse> : IRequest<TResponse>;