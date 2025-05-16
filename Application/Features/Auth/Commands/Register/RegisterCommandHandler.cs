using Application.Cqrs.Commands;
using Application.Helpers;
using Application.Identity;
using Application.Repositories;
using Application.Services.Abstractions;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.Register;

internal class RegisterCommandHandler(
    IMapper mapper,
    IIdentityUnitOfWork unitOfWork,
    UserManager<AppUser> userManager,
    IUserRepository userRepository,
    IEmailSender emailSender,
    ISubscriptionRepository subscriptionRepository) : ICommandHandler<RegisterCommand, RegisterDto>
{
    public async Task<RegisterDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var subscriptions = await subscriptionRepository.GetAllSubscriptionsAsync();
        
        // изначально юзеру добавляются все подписки
        // это сделано для удобства, чтобы для просмотра всего функционала сайта не нужно было
        // покупать подписку на каждый вид контента. их всё еще можно купить если сначала удалить какую-нибудь
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
            Email = request.SignUpDto.Email,
            Nickname = request.SignUpDto.Login,
            UserSubscriptions = userSubscriptions
        };
        
        user = await userRepository.AddAsync(user);
        var appUser = mapper.Map<AppUser>(user);
        var identityResult = await userManager.CreateAsync(appUser, request.SignUpDto.Password);
        if (identityResult.Succeeded)
        {
            // сильно душит, поэтому без проверки почты
            appUser.EmailConfirmed = true;
            
            await userManager.AddToRoleAsync(appUser, "user");
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var message = EmailMessageHelper.GetEmailConfirmationMessage(confirmationToken, appUser.Id);
            await emailSender.SendEmailAsync(request.SignUpDto.Email, message);

            return new RegisterDto { UserId = user?.Id };
        }
        
        throw new IdentityException(string.Join(" ", identityResult.Errors.Select(x => x.Description)));
    }
}