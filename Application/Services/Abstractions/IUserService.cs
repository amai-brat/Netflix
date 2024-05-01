using Application.Dto;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Abstractions;

public interface IUserService
{
    public Task<PersonalInfoDto> GetPersonalInfoAsync(long id);
    public Task<User> ChangeRoleAsync(long userId, string newRole);
    public Task<User> ChangeEmailAsync(long userId, string newEmail);
    public Task<User> ChangeBirthdayAsync(long userId, DateOnly newBirthday);
    public Task<User> ChangePasswordAsync(long userId, ChangePasswordDto dto);
    public Task<User> ChangeProfilePictureAsync(long userId, Stream pictureStream, string contentType);
    public Task<List<UserReviewDto>> GetReviewsAsync(ReviewSearchDto dto);
    public Task<int> GetReviewsPagesCountAsync(ReviewSearchDto dto);
    public Task<List<FavouriteDto>> GetFavouritesAsync(long userId);

    public Task<long?> RegisterAsync(SignUpDto dto); 
    public Task<TokensDto> AuthenticateAsync(LoginDto dto);
    public Task<TokensDto> RefreshTokenAsync(string token);
    public Task RevokeTokenAsync(string token);
    
    public Task<TokensDto> AuthenticateFromExternalAsync(ExternalLoginDto dto);
}