using System.IdentityModel.Tokens.Jwt;
using Application.Dto;
using Application.Services.Abstractions;
using FluentValidation;
using Infrastructure.Providers.ProviderFactory;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.AuthController;

[ApiController]
[Route("auth")]
public class AuthController(
    IUserService userService,
    AuthProviderResolver authProviderResolver,
    IValidator<SignUpDto> signUpDtoValidator) : ControllerBase
{
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
        
        var tokens = await userService.AuthenticateFromExternalAsync(new ExternalLoginDto
        {
            Login = jwt.Claims.First(c => c.Type == "name").Value,
            Email = jwt.Claims.First(c => c.Type == "email").Value,
            PictureUrl = jwt.Claims.First(c => c.Type == "picture").Value
        });
        
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