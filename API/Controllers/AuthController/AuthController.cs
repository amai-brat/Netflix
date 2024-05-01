using Application.Dto;
using FluentValidation;
using Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AuthController;

[ApiController]
[Route("auth")]
public class AuthController(
    IAuthService authService,
    IValidator<SignUpDto> signUpDtoValidator) : ControllerBase
{
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
        
        if (tokens.RefreshToken != null)
        {
            HttpContext.Response.Cookies.Append("refresh-token",
                tokens.RefreshToken,
                new CookieOptions
                {
                    SameSite = SameSiteMode.None,
                    HttpOnly = true,
                    Secure = true,
                    MaxAge = TimeSpan.FromDays(30)
                });
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
        HttpContext.Response.Cookies.Append("refresh-token", 
            tokens.RefreshToken!, 
            new CookieOptions
            {
                SameSite = SameSiteMode.None,
                HttpOnly = true,
                Secure = true,
                MaxAge = TimeSpan.FromDays(30)
            });

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
        return Redirect(Consts.FrontendUrl);
    }

    [HttpGet("confirm-email-change")]
    public async Task<IActionResult> ConfirmEmailChange([FromQuery]long userId, [FromQuery]string newEmail, [FromQuery]string token)
    {
        _ = await authService.ChangeEmailAsync(userId, newEmail, token);
        return Redirect(Consts.FrontendUrl);
    }
}