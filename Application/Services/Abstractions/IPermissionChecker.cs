namespace Application.Services.Abstractions;

public interface IPermissionChecker
{
    Task<bool> IsContentAllowedForUserAsync(long contentId, long userId);
}