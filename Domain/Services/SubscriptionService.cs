using System.Text.RegularExpressions;
using Domain.Abstractions;
using Domain.Dtos;
using Domain.Entities;
using Domain.Services.ServiceExceptions;

namespace Domain.Services;

public class SubscriptionService(
    ISubscriptionRepository subRepository,
    IContentRepository contentRepository) : ISubscriptionService
{
    public async Task<Subscription> AddSubscriptionAsync(NewSubscriptionDto dto)
    {
        if (!Validate(dto, out var error, out var paramName))
        {
            throw new SubscriptionServiceArgumentException(error, paramName);
        }

        var contents = new List<ContentBase>();
        foreach (var contentId in dto.AccessibleContentIds)
        {
            var content = await contentRepository.GetContentByIdAsync(contentId);
            if (content is null)
                throw new SubscriptionServiceArgumentException(
                    SubscriptionErrorMessages.GivenIdOfNonExistingContent,
                    nameof(dto.AccessibleContentIds));
            
            contents.Add(content);
        }

        var result = await subRepository.AddAsync(new Subscription
        {
            Name = dto.Name,
            Description = dto.Description,
            MaxResolution = dto.MaxResolution,
            AccessibleContent = contents,
            Price = dto.Price
        });

        await subRepository.SaveChangesAsync();

        return result;
    }

    public async Task<Subscription> DeleteSubscriptionAsync(int subscriptionId)
    {
        var subscription = await subRepository.GetSubscriptionByIdAsync(subscriptionId);
        if (subscription is null)
        {
            throw new SubscriptionServiceArgumentException(
                SubscriptionErrorMessages.SubscriptionNotFound, 
                nameof(subscriptionId));
        }

        subscription = subRepository.Remove(subscription);
        await subRepository.SaveChangesAsync();
        
        return subscription;
    }

    public async Task<Subscription> EditSubscriptionAsync(EditSubscriptionDto dto)
    {
        var subscription = await subRepository.GetSubscriptionWithAccessibleContentAsync(dto.SubscriptionId);
        if (subscription is null)
        {
            throw new SubscriptionServiceArgumentException(
                SubscriptionErrorMessages.SubscriptionNotFound,
                nameof(dto.SubscriptionId));
        }
        
        if (!Validate(dto, out var error, out var paramName))
        {
            throw new SubscriptionServiceArgumentException(error, paramName);
        }

        if (dto.NewName is not null)
            subscription.Name = dto.NewName;

        if (dto.NewDescription is not null)
            subscription.Description = dto.NewDescription;

        if (dto.NewMaxResolution is not null)
            subscription.MaxResolution = dto.NewMaxResolution.Value;

        if (dto.NewPrice is not null)
            subscription.Price = dto.NewPrice.Value;
        
        if (dto.AccessibleContentIdsToAdd != null)
        {
            foreach (var contentId in dto.AccessibleContentIdsToAdd)
            {
                var content = await contentRepository.GetContentByIdAsync(contentId);
                if (content is null)
                    throw new SubscriptionServiceArgumentException(
                        SubscriptionErrorMessages.GivenIdOfNonExistingContent,
                        nameof(dto.AccessibleContentIdsToAdd));

                if (subscription.AccessibleContent.All(x => x.Id != content.Id))
                    subscription.AccessibleContent.Add(content);
            }
        }

        if (dto.AccessibleContentIdsToRemove != null)
        {
            foreach (var contentId in dto.AccessibleContentIdsToRemove)
            {
                var content = await contentRepository.GetContentByIdAsync(contentId);
                if (content is null)
                    throw new SubscriptionServiceArgumentException(
                        SubscriptionErrorMessages.GivenIdOfNonExistingContent,
                        nameof(dto.AccessibleContentIdsToRemove));

                subscription.AccessibleContent.Remove(content);
            }
        }

        await subRepository.SaveChangesAsync();
        return subscription;
    }

    public async Task<List<AdminSubscriptionsDto>> GetSubscriptionsAsync()
    {
        var subscriptions = await subRepository.GetAllSubscriptionsWithAccessibleContentAsync();
        var result = subscriptions
            .Select(subscription => new AdminSubscriptionsDto
            {
                Id = subscription.Id,
                Name = subscription.Name,
                Description = subscription.Description,
                MaxResolution = subscription.MaxResolution,
                Price = subscription.Price,
                AccessibleContent = subscription.AccessibleContent
                    .Select(x => new AdminSubscriptionContentDto { Id = x.Id, Name = x.Name })
                    .ToList()
            })
            .ToList();

        return result;
    }

    public async Task<List<AdminSubscriptionContentDto>> GetContentsAsync()
    {
        var contents = await contentRepository.GetContentsByFilterAsync(x => true);
        return contents
            .Select(x => new AdminSubscriptionContentDto() { Id = x.Id, Name = x.Name })
            .ToList();
    }

    private static bool Validate(NewSubscriptionDto dto, out string error, out string paramName)
    {
        if (string.IsNullOrEmpty(dto.Name) || !ValidateName(dto.Name))
        {
            error = SubscriptionErrorMessages.NotValidSubscriptionName;
            paramName = nameof(dto.Name);
            return false;
        }

        if (string.IsNullOrEmpty(dto.Description) || !ValidateDescription(dto.Description))
        {
            error = SubscriptionErrorMessages.NotValidSubscriptionDescription;
            paramName = nameof(dto.Description);
            return false;
        }

        if (!ValidateMaxResolution(dto.MaxResolution))
        {
            error = SubscriptionErrorMessages.NotValidSubscriptionMaxResolution;
            paramName = nameof(dto.MaxResolution);
            return false;
        }

        if (!ValidatePrice(dto.Price))
        {
            error = SubscriptionErrorMessages.SubscriptionPriceLessThanZero;
            paramName = nameof(dto.Price);
            return false;
        }

        error = "";
        paramName = "";
        return true;
    }

    private static bool Validate(EditSubscriptionDto dto, out string error, out string paramName)
    {
        if (dto.NewName is not null && !ValidateName(dto.NewName))
        {
            error = SubscriptionErrorMessages.NotValidSubscriptionName;
            paramName = nameof(dto.NewName);
            return false;
        }

        if (dto.NewDescription is not null && !ValidateDescription(dto.NewDescription))
        {
            error = SubscriptionErrorMessages.NotValidSubscriptionDescription;
            paramName = nameof(dto.NewDescription);
            return false;
        }

        if (dto.NewMaxResolution is not null && !ValidateMaxResolution(dto.NewMaxResolution.Value))
        {
            error = SubscriptionErrorMessages.NotValidSubscriptionMaxResolution;
            paramName = nameof(dto.NewMaxResolution);
            return false;
        }

        if (dto.NewPrice is not null && !ValidatePrice(dto.NewPrice.Value))
        {
            error = SubscriptionErrorMessages.SubscriptionPriceLessThanZero;
            paramName = nameof(dto.NewPrice);
            return false;
        }
        
        error = "";
        paramName = "";
        return true;
    }

    private static bool IsEmptyOrWhiteSpace(string str) => str.Length == 0 || str.All(char.IsWhiteSpace);
    private static bool ValidateName(string name) => !IsEmptyOrWhiteSpace(name) &&
                                              Regex.IsMatch(name, "^[А-Яа-яA-Za-z0-9-_ ]+$");
    private static bool ValidateDescription(string description) => !IsEmptyOrWhiteSpace(description);
    private static bool ValidateMaxResolution(int resolution) => resolution > 0;
    private static bool ValidatePrice(decimal price) => price >= 0;

}