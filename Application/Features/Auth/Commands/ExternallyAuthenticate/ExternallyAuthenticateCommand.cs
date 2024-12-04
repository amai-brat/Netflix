using Application.Cqrs.Commands;

namespace Application.Features.Auth.Commands.ExternallyAuthenticate;

public record ExternallyAuthenticateCommand(string Provider, AuthCode AuthCode) : ICommand<ExternallyAuthenticateDto>;