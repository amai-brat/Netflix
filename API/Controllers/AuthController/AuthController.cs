using Application.Dto;
using Application.Services.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AuthController;

[ApiController]
[Route("auth")]
public class AuthController(
    IUserService userService,
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
        
        var userId = await userService.RegisterAsync(dto);
        return Created("", userId);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync(LoginDto dto)
    {
        var tokens = await userService.AuthenticateAsync(dto);
        
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

        var tokens = await userService.RefreshTokenAsync(refreshToken);
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

        await userService.RevokeTokenAsync(refreshToken);

        return NoContent();
    }
}