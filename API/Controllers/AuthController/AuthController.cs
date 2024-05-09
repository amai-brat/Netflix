using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Application.Dto;
using FluentValidation;
using Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Providers.ProviderFactory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers.AuthController;

[ApiController]
[Route("auth")]
public class AuthController(
    IAuthService authService,
    IOptionsMonitor<FrontendConfig> monitor,
    AuthProviderResolver authProviderResolver,
    IValidator<SignUpDto> signUpDtoValidator) : ControllerBase
{
    private readonly FrontendConfig _frontendConfig = monitor.CurrentValue;
    
    [HttpGet("external/{provider}")]
    public IActionResult GetRedirectUri(string provider)
    {
        var authProvider = authProviderResolver.GetAuthProvider(provider);
        if (authProvider is null)
            return BadRequest("Incorrect provider");
        
        return Redirect(authProvider.GetAuthUri());
    }
    
    [HttpPost("external/{provider}")]
    public async Task<IActionResult> GetTokenAsync(string provider, [FromBody] AuthCode code)
    {
        var authProvider = authProviderResolver.GetAuthProvider(provider);
        if (authProvider is null)
            return BadRequest("Incorrect provider");

        var oAuthResult = await authProvider.ExchangeCodeAsync(code.Code);

        if (!oAuthResult.IsSuccess)
            return Unauthorized(oAuthResult.ErrorDescription);
        
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(oAuthResult.IdToken);
        
        var tokens = await authService.AuthenticateFromExternalAsync(new ExternalLoginDto
        {
            Login = jwt.Claims.First(c => c.Type == "name").Value,
            Email = jwt.Claims.First(c => c.Type == "email").Value,
            PictureUrl = jwt.Claims.First(c => c.Type == "picture").Value
        });
        
        SetRefreshTokenCookie(tokens.RefreshToken!);

        return Ok(tokens.AccessToken);
    }
    
    [HttpPost("signup")]
    public async Task<IActionResult> SignUpAsync(SignUpDto dto)
    {
        var validationResult = await signUpDtoValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        
        var userId = await authService.RegisterAsync(dto);
        return Created("", userId);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync(LoginDto dto)
    {
        var tokens = await authService.AuthenticateAsync(dto);

        if (tokens == null)
        {
            return Accepted("", "2FA");
        }
        
        if (tokens.RefreshToken != null)
        {
            SetRefreshTokenCookie(tokens.RefreshToken);
        }

        return Ok(tokens.AccessToken);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync()
    {
        var refreshToken = HttpContext.Request.Cookies["refresh-token"];
        if (refreshToken is null)
        {
            return Unauthorized("Refresh token is not found");
        }

        var tokens = await authService.RefreshTokenAsync(refreshToken);
        SetRefreshTokenCookie(tokens.RefreshToken!);

        return Ok(tokens.AccessToken);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeTokenAsync()
    {
        var refreshToken = HttpContext.Request.Cookies["refresh-token"];
        if (refreshToken is null)
        {
            return Unauthorized("Refresh token is not found");
        }

        await authService.RevokeTokenAsync(refreshToken);

        return NoContent();
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery]long userId, [FromQuery] string token)
    {
        _ = await authService.ConfirmEmailAsync(userId, token);
        return Redirect(_frontendConfig.Url);
    }

    [HttpGet("confirm-email-change")]
    public async Task<IActionResult> ConfirmEmailChange([FromQuery]long userId, [FromQuery]string newEmail, [FromQuery]string token)
    {
        _ = await authService.ChangeEmailAsync(userId, newEmail, token);
        return Redirect(_frontendConfig.Url);
    }

    [Authorize]
    [HttpPost("enable-2fa")]
    public async Task<IActionResult> EnableTwoFactorAuth()
    {
        var email = User.FindFirst(ClaimTypes.Email)!.Value;
        await authService.EnableTwoFactorAuthAsync(email);
        return Ok();
    }

    [Authorize]
    [HttpGet("is-enabled-2fa")]
    public async Task<IActionResult> IsEnabledTwoFactor()
    {
        var email = User.FindFirst(ClaimTypes.Email)!.Value;
        var result = await authService.IsEnabledTwoFactorAuthAsync(email);
        return Ok(result);
    }
    
    [HttpPost("send-2fa")]
    public async Task<IActionResult> SendTwoFactorToken(TwoFactorTokenDto dto)
    {
        var result = await authService.TwoFactorAuthenticateAsync(dto);
        if (result.RefreshToken != null)
        {
            SetRefreshTokenCookie(result.RefreshToken);
        }
        
        return Ok(result.AccessToken);
    }

    private void SetRefreshTokenCookie(string refreshToken)
    {
        HttpContext.Response.Cookies.Append("refresh-token", 
            refreshToken, 
            new CookieOptions
            {
                SameSite = SameSiteMode.None,
                HttpOnly = true,
                Secure = true,
                MaxAge = TimeSpan.FromDays(30)
            });
    }
}