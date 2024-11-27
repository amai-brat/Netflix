using Application.Cqrs.Commands;

namespace Application.Features.Auth.Commands.SignIn;

public record SignInCommand(LoginDto LoginDto) : ICommand<SignInDto>;