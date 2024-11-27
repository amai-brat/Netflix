using System.Security.Claims;
using Application.Features.Auth.Commands.ChangeEmail;
using Application.Features.Auth.Commands.ConfirmEmail;
using Application.Features.Auth.Commands.EnableTwoFactorAuth;
using Application.Features.Auth.Commands.ExternallyAuthenticate;
using Application.Features.Auth.Commands.RefreshToken;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.RevokeToken;
using Application.Features.Auth.Commands.SignIn;
using Application.Features.Auth.Commands.TwoFactorAuthenticate;
using Application.Features.Auth.Queries.GetAuthProviderRedirectUri;
using Application.Features.Auth.Queries.IsEnabledTwoFactorAuth;
using Infrastructure.Options;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(
    IMediator mediator,
    IOptionsMonitor<FrontendConfig> monitor) : ControllerBase
{
    private readonly FrontendConfig _frontendConfig = monitor.CurrentValue;
    
    [HttpGet("external/{provider}")]
    public async Task<IActionResult> GetRedirectUri(string provider)
    {
        var result = await mediator.Send(new GetAuthProviderRedirectUriQuery(provider));
        return Redirect(result.Uri);
    }
    
    [HttpPost("external/{provider}")]
    public async Task<IActionResult> GetTokenAsync(string provider, [FromBody] AuthCode code)
    {
        var result = await mediator.Send(new ExternallyAuthenticateCommand(provider, code));
        if (result.Tokens is null)
        {
            return StatusCode(result.Code, result.Message);
        }
        
        return Ok(result.Tokens.AccessToken);
    }
    
    [HttpPost("signup")]
    public async Task<IActionResult> SignUpAsync(SignUpDto dto)
    {
        var result = await mediator.Send(new RegisterCommand(dto));
        return Created("user", result.UserId);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync(LoginDto dto)
    {
        var result = await mediator.Send(new SignInCommand(dto));
        if (result.Tokens == null)
        {
            return StatusCode(result.Code, result.Message);
        }
        
        return Ok(result.Tokens.AccessToken);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync()
    {
        var result = await mediator.Send(new RefreshTokenCommand());
        if (result.Tokens == null)
        {
            return StatusCode(result.Code, result.Message);
        }
        
        return Ok(result.Tokens.AccessToken);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeTokenAsync()
    {
        var result = await mediator.Send(new RevokeTokenCommand());
        return StatusCode(result.Code, result.Error);
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery]long userId, [FromQuery] string token)
    {
        await mediator.Send(new ConfirmEmailCommand(userId, token));
        return Redirect(_frontendConfig.Url);
    }

    [HttpGet("confirm-email-change")]
    public async Task<IActionResult> ConfirmEmailChange([FromQuery]long userId, [FromQuery]string newEmail, [FromQuery]string token)
    {
        await mediator.Send(new ChangeEmailCommand(userId, newEmail, token));
        return Redirect(_frontendConfig.Url);
    }

    [Authorize]
    [HttpPost("enable-2fa")]
    public async Task<IActionResult> EnableTwoFactorAuth()
    {
        var email = User.FindFirst(ClaimTypes.Email)!.Value;
        await mediator.Send(new EnableTwoFactorAuthCommand(email));
        return Ok();
    }

    [Authorize]
    [HttpGet("is-enabled-2fa")]
    public async Task<IActionResult> IsEnabledTwoFactor()
    {
        var email = User.FindFirst(ClaimTypes.Email)!.Value;
        var result = await mediator.Send(new IsEnabledTwoFactorAuthQuery(email));
        return Ok(result.Enabled);
    }
    
    [HttpPost("send-2fa")]
    public async Task<IActionResult> SendTwoFactorToken(TwoFactorTokenDto dto)
    {
        var result = await mediator.Send(new TwoFactorAuthenticateCommand(dto));
        return Ok(result.Tokens.AccessToken);
    }
}