using MediatR;

namespace Application.Cqrs.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> 
    where TCommand : IRequest;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse> 
    where TCommand : IRequest<TResponse>;