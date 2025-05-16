using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.RevokeToken;
using Application.Features.Auth.Commands.SignIn;
using HotChocolate.Authorization;
using HotChocolate.Language;
using MediatR;

namespace MobileAPI.Types.Auth;

[ExtendObjectType(OperationType.Mutation)]
public class AuthMutation
{
    public async Task<SignUpPayload> SignUp(
        SignUpInput input,
        [Service] IMediator mediator)
    {
        var result = await mediator.Send(new RegisterCommand(input.ToDto()));
        return new SignUpPayload(result.UserId);
    }
    
    public async Task<SignInPayload> SignIn(
        SignInInput input,
        [Service] IMediator mediator)
    {
        var result = await mediator.Send(new SignInCommand(input.ToDto()));
        return SignInPayload.From(result);
    }
    
    [Authorize]
    public async Task<SignOutPayload> SignOut(
        // SignOutInput input,
        [Service] IMediator mediator)
    {
        var result = await mediator.Send(new RevokeTokenCommand());
        return SignOutPayload.From(result);
    }
}