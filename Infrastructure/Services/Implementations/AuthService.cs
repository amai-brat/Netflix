using System.Security.Authentication;
using System.Security.Claims;
using System.Text.Json;
using Application.Dto;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Helpers;
using Infrastructure.Identity;
using Infrastructure.Identity.Data;
using Infrastructure.Options;
using Infrastructure.Services.Abstractions;
using Infrastructure.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Implementations;

public class AuthService(
    UserManager<AppUser> userManager,
    SignInManager<AppUser> signInManager,
    IUserRepository userRepository,
    IMapper mapper,
    IUnitOfWork appUnitOfWork,
    IIdentityUnitOfWork unitOfWork,
    ITokenGenerator tokenGenerator,
    ITokenRepository tokenRepository,
    IEmailSender emailSender,
    ITwoFactorTokenSender twoFactorTokenSender,
    IOptionsMonitor<JwtOptions> monitor) : IAuthService
{
    private readonly JwtOptions _jwtOptions = monitor.CurrentValue;
    
    public async Task<long?> RegisterAsync(SignUpDto dto)
    {
        if (!await userRepository.IsEmailUniqueAsync(dto.Email))
        {
            throw new AuthServiceException(ErrorMessages.EmailNotUnique, nameof(dto.Email));
        }
        
        var user = new User
        {
            Email = dto.Email,
            Nickname = dto.Login
        };
        
        user = await userRepository.AddAsync(user);
        var appUser = mapper.Map<AppUser>(user);
        var identityResult = await userManager.CreateAsync(appUser, dto.Password);
        if (identityResult.Succeeded)
        {
            await userManager.AddToRoleAsync(appUser, "user");
            await unitOfWork.SaveChangesAsync();

            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var message = EmailMessageHelper.GetEmailConfirmationMessage(confirmationToken, appUser.Id);
            await emailSender.SendEmailAsync(dto.Email, message);

            return user?.Id;   
        }
        throw new IdentityException(string.Join(" ", identityResult.Errors.Select(x => x.Description)));
    }

    public async Task<TokensDto?> AuthenticateAsync(LoginDto dto)
    {
        var appUser = await userManager.FindByEmailAsync(dto.Email);
        if (appUser is null)
        {
            throw new Exception(ErrorMessages.NotFoundUser);
        }

        if (string.IsNullOrEmpty(appUser.PasswordHash))
        {
            throw new AuthenticationException(ErrorMessages.CannotAccessToAccountByPassword);
        }
        
        var correctPassword = await userManager.CheckPasswordAsync(appUser, dto.Password);
        if (!correctPassword)
        {
            throw new AuthServiceException(ErrorMessages.IncorrectPassword, nameof(dto.Password));
        }

        if (!appUser.EmailConfirmed)
        {
            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var message = EmailMessageHelper.GetEmailConfirmationMessage(confirmationToken, appUser.Id);
            await emailSender.SendEmailAsync(dto.Email, message);

            throw new AuthServiceException(AuthErrorMessages.EmailNotConfirmed);
        }

        await signInManager.PasswordSignInAsync(appUser, dto.Password, false, false);
        
        if (appUser.TwoFactorEnabled)
        {
            var token = await userManager.GenerateTwoFactorTokenAsync(appUser, GetTokenProvider(appUser));
            await twoFactorTokenSender.SendAsync(appUser, token);
            
            return null;
        }

        return await GetTokens(appUser, dto.RememberMe);
    }

    public async Task<string> ConfirmEmailAsync(long userId, string token)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            throw new AuthServiceException(ErrorMessages.NotFoundUser, nameof(userId));
        }

        var result = await userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return user.Email!;
        }

        throw new IdentityException(AuthErrorMessages.InvalidConfirmationToken);
    }
    
    public async Task<TokensDto> RefreshTokenAsync(string token)
    {
        var refreshToken = await tokenRepository.GetRefreshTokenWithUserByTokenAsync(token);

        if (refreshToken is null)
            throw new AuthServiceException(ErrorMessages.RefreshTokenNotFound);
                
        if (refreshToken.IsRevoked)
        {
            await RevokeDescendantRefreshTokens(refreshToken, refreshToken.User, $"Attempted reuse of revoked ancestor token: {token}");
            await unitOfWork.SaveChangesAsync();
        }

        if (!refreshToken.IsActive)
            throw new AuthServiceException(ErrorMessages.NotActiveRefreshToken);
        
        var newRefreshToken = RotateRefreshToken(refreshToken);
        await tokenRepository.AddAsync(newRefreshToken);
        
        await RemoveOldRefreshTokens(refreshToken.User);

        await unitOfWork.SaveChangesAsync();
        var user = await userRepository.GetUserWithSubscriptionsAsync(x => x.Id == refreshToken.UserId);
        if (user is null)
            throw new BusinessException(ErrorMessages.NotFoundUser);
        
        var jwtToken = tokenGenerator.GenerateAccessToken(await GetClaimsAsync(user, refreshToken.User));

        return new TokensDto(jwtToken, newRefreshToken.Token);
    }

    public async Task RevokeTokenAsync(string token)
    {
        var refreshToken = await tokenRepository.GetRefreshTokenWithUserByTokenAsync(token);

        if (refreshToken is null)
            throw new Exception(ErrorMessages.RefreshTokenNotFound);

        if (!refreshToken.IsActive)
            throw new Exception(ErrorMessages.NotActiveRefreshToken);
        
        RevokeRefreshToken(refreshToken, "Revoked without replacement");

        await unitOfWork.SaveChangesAsync();
    }

    public async Task<string> ChangePasswordAsync(string userEmail, ChangePasswordDto dto)
    {
        var appUser = await userManager.FindByEmailAsync(userEmail);
        if (appUser is null)
        {
            throw new ArgumentException(ErrorMessages.NotFoundUser);
        }

        var result = await userManager.ChangePasswordAsync(appUser, dto.PreviousPassword, dto.NewPassword);
        if (result.Succeeded)
        {
            return appUser.Email!;
        }

        throw new IdentityException(string.Join(" ", result.Errors.Select(x => x.Description)));
    }

    public async Task ChangeEmailRequestAsync(string prevEmail, string newEmail)
    {
        var appUser = await userManager.FindByEmailAsync(prevEmail);
        var user = await userRepository.GetUserByFilterAsync(x => x.Email == prevEmail);
        
        if (user is null && appUser is null)
        {
            throw new AuthServiceException(ErrorMessages.NotFoundUser);
        }

        if (user is null || appUser is null)
        {
            throw new BusinessException(ErrorMessages.NotFoundUser);
        }

        var token = await userManager.GenerateChangeEmailTokenAsync(appUser, newEmail);
        var message = EmailMessageHelper.GetEmailChangeConfirmationMessage(token, appUser.Id, newEmail);
        await emailSender.SendEmailAsync(newEmail, message);
    }

    public async Task<string> ChangeEmailAsync(long userId, string newEmail, string changeToken)
    {
        var appUser = await userManager.FindByIdAsync(userId.ToString());
        if (appUser is null)
        {
            throw new AuthServiceException(ErrorMessages.NotFoundUser);
        }
        var prevEmail = appUser.Email;
        
        var result = await userManager.ChangeEmailAsync(appUser, newEmail, changeToken);
        if (result.Succeeded)
        {
            var user = await userRepository.GetUserByFilterAsync(x => x.Email == prevEmail);
            user!.Email = newEmail;
            await unitOfWork.SaveChangesAsync();

            return newEmail;
        }

        throw new IdentityException(string.Join(" ", result.Errors.Select(x => x.Description)));
    }

    public async Task<string> ChangeRoleAsync(long userId, string newRole)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == userId);
        if (user is null)
        {
            throw new AuthServiceException(ErrorMessages.NotFoundUser);
        }
        var appUser = await userManager.FindByEmailAsync(user.Email);

        var result = await userManager.AddToRoleAsync(appUser!, newRole);
        
        if (result.Succeeded)
        {
            return appUser!.Email!;
        }

        throw new IdentityException(string.Join(" ", result.Errors.Select(x => x.Description)));
    }

    public async Task EnableTwoFactorAuthAsync(string userEmail)
    {
        var appUser = await userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            throw new AuthServiceException(ErrorMessages.NotFoundUser);
        }

        await userManager.SetTwoFactorEnabledAsync(appUser, true);
    }

    public async Task<bool> IsEnabledTwoFactorAuthAsync(string userEmail)
    {
        var appUser = await userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            throw new AuthServiceException(ErrorMessages.NotFoundUser);
        }
        
        return await userManager.GetTwoFactorEnabledAsync(appUser);
    }
    
    public async Task<TokensDto> TwoFactorAuthenticateAsync(TwoFactorTokenDto dto)
    {
        var appUser = await signInManager.GetTwoFactorAuthenticationUserAsync();
        if (appUser == null)
        {
            throw new AuthServiceException(ErrorMessages.NotFoundUser);
        }
        
        var result = await userManager.VerifyTwoFactorTokenAsync(appUser, GetTokenProvider(appUser), dto.Token);
        if (result)
        {
            var tokens = await GetTokens(appUser, dto.RememberMe);
            return tokens;
        }

        throw new AuthServiceException(AuthErrorMessages.InvalidTwoFactorToken);
    }
    
    public async Task<TokensDto> AuthenticateFromExternalAsync(ExternalLoginDto dto)
    {
        var user = 
            await userManager.FindByEmailAsync(dto.Email) ??
            await RegisterFromExternalAsync(dto);

        return await GetTokens(user, true);
    }

    private async Task<AppUser> RegisterFromExternalAsync(ExternalLoginDto dto)
    {
        if (!await userRepository.IsEmailUniqueAsync(dto.Email))
            throw new AuthServiceException(ErrorMessages.EmailNotUnique);
        
        var user = new User
        {
            Email = dto.Email,
            Nickname = dto.Login,
            ProfilePictureUrl = dto.PictureUrl
        };

        await userRepository.AddAsync(user);
        
        var appUser = mapper.Map<AppUser>(user);
        appUser.EmailConfirmed = true;
        var identityResult = await userManager.CreateAsync(appUser);

        if (identityResult.Succeeded)
            await userManager.AddToRoleAsync(appUser, "user");
        else
            throw new IdentityException(string.Join(" ", identityResult.Errors.Select(x => x.Description)));

        await appUnitOfWork.SaveChangesAsync();
        
        return appUser;
    }

    private async Task<List<Claim>> 
        GetClaimsAsync(User user, AppUser appUser)
    {
        var claims = new List<Claim>
        {
            new("id", user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new("subscribeId", JsonSerializer.Serialize(
                user.UserSubscriptions!
                    .Where(x => x.ExpiresAt < DateTimeOffset.Now)
                    .Select(x => x.SubscriptionId).ToList()))
        };
        
        var roles = await userManager.GetRolesAsync(appUser);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }
    
    private async Task RemoveOldRefreshTokens(AppUser user)
    {
        await tokenRepository.RemoveAllRefreshTokensAsync(user, x =>
            !x.IsActive &&
            x.Created.AddDays(_jwtOptions.RefreshTokenLifetimeInDays) <= DateTime.UtcNow);
    }
    
    private async Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, AppUser user, string reason)
    {
        if (string.IsNullOrEmpty(refreshToken.ReplacedByToken)) return;
        
        var childToken = (await tokenRepository.GetRefreshTokensAsync(user))
            .SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
        if (childToken is { IsActive: true })
            RevokeRefreshToken(childToken, reason);
        else if (childToken != null) 
            await RevokeDescendantRefreshTokens(childToken, user, reason);
    }
    
    private static void RevokeRefreshToken(RefreshToken token, string? reason = null, string? replacedByToken = null)
    {
        token.Revoked = DateTime.UtcNow;
        token.ReasonRevoked = reason;
        token.ReplacedByToken = replacedByToken;
    }
    
    private RefreshToken RotateRefreshToken(RefreshToken refreshToken)
    {
        var newRefreshToken = tokenGenerator.GenerateRefreshToken(refreshToken.UserId, refreshToken.AppUserId);
        newRefreshToken.User = refreshToken.User;
        newRefreshToken.UserId = refreshToken.UserId;
        RevokeRefreshToken(refreshToken, "Replaced by new token", newRefreshToken.Token);
        return newRefreshToken;
    }

    private async Task<TokensDto> GetTokens(AppUser appUser, bool rememberMe)
    {
        var user = await userRepository.GetUserWithSubscriptionsAsync(x => x.Email == appUser.Email);
        if (user is null)
        {
            throw new BusinessException(ErrorMessages.NotFoundUser);
        }
        
        var claims = await GetClaimsAsync(user, appUser);
        var accessToken = tokenGenerator.GenerateAccessToken(claims);
        RefreshToken? refreshToken = null;
        if (rememberMe)
        {
            refreshToken = tokenGenerator.GenerateRefreshToken(user.Id, appUser.Id);
            refreshToken.User = appUser;
            await tokenRepository.AddAsync(refreshToken);
        }
        
        await RemoveOldRefreshTokens(appUser);
        await unitOfWork.SaveChangesAsync();
        return new TokensDto(accessToken, refreshToken?.Token);
    }

    private string GetTokenProvider(AppUser appUser)
    {
        return appUser.TwoFactorType switch
        {
            TwoFactorType.Email => "Email",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}