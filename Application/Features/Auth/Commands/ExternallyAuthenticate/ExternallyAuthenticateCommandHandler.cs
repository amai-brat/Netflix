using Application.Cqrs.Commands;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Features.Auth.Dtos;
using Application.Helpers;
using Application.Identity;
using Application.Providers;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.ExternallyAuthenticate;

internal class ExternallyAuthenticateCommandHandler(
    IUserRepository userRepository,
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    ISubscriptionRepository subscriptionRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IAuthProviderResolver authProviderResolver,
    IHttpContextAccessor httpContextAccessor) : ICommandHandler<ExternallyAuthenticateCommand, ExternallyAuthenticateDto>
{
    private readonly HttpContext _httpContext = httpContextAccessor.HttpContext!;
    
    public async Task<ExternallyAuthenticateDto> Handle(ExternallyAuthenticateCommand request, CancellationToken cancellationToken)
    {
        var authProvider = authProviderResolver.GetAuthProvider(request.Provider);
        if (authProvider is null)
            throw new ArgumentValidationException("Incorrect provider");

        var externalLoginDto = await authProvider.ExchangeCodeAsync(request.AuthCode.Code);

        if (!externalLoginDto.IsSuccess)
        {
            return new ExternallyAuthenticateDto
            {
                Code = 401,
                Message = externalLoginDto.ErrorDescription
            };
        }
        
        var user = 
            await userManager.FindByEmailAsync(externalLoginDto.Email) ??
            await RegisterFromExternalAsync(externalLoginDto);

        var tokens = await tokenService.GetTokens(user, true);
        

        if (tokens.RefreshToken is not null)
        {
            CookieHelper.AddRefreshTokenCookie(_httpContext, tokens.RefreshToken);
        }

        return new ExternallyAuthenticateDto
        {
            Code = 200,
            Tokens = tokens
        };
    }
    
    private async Task<AppUser> RegisterFromExternalAsync(ExternalLoginDto dto)
    {
        if (!await userRepository.IsEmailUniqueAsync(dto.Email))
            throw new ArgumentValidationException(ErrorMessages.EmailNotUnique);
        
        var subscriptions = await subscriptionRepository.GetAllSubscriptionsAsync();
        var userSubscriptions = new List<UserSubscription>();
        userSubscriptions.AddRange(subscriptions.Select(subscription => new UserSubscription
        {
            Subscription = subscription,
            ExpiresAt = DateTimeOffset.UtcNow.AddMonths(6),
            BoughtAt = DateTimeOffset.UtcNow,
            Status = UserSubscriptionStatus.Completed
        }));
        
        var user = new User
        {
            Email = dto.Email,
            Nickname = dto.Login,
            ProfilePictureUrl = dto.PictureUrl,
            UserSubscriptions = userSubscriptions
        };

        await userRepository.AddAsync(user);
        
        var appUser = mapper.Map<AppUser>(user);
        appUser.EmailConfirmed = true;
        var identityResult = await userManager.CreateAsync(appUser);

        if (identityResult.Succeeded)
            await userManager.AddToRoleAsync(appUser, "user");
        else
            throw new IdentityException(string.Join(" ", identityResult.Errors.Select(x => x.Description)));

        await unitOfWork.SaveChangesAsync();
        
        return appUser;
    }
}