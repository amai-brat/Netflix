using Application.Dto;
using Domain.Entities;

namespace Application.Services.Abstractions;

public interface IUserService
{
    public Task<PersonalInfoDto> GetPersonalInfoAsync(int id);
    public Task<User> ChangeRoleAsync(long userId, string newRole);
    public Task<User> ChangeEmailAsync(int userId, string newEmail);
    public Task<User> ChangeBirthdayAsync(int userId, DateOnly newBirthday);
    public Task<User> ChangePasswordAsync(int userId, ChangePasswordDto dto);
    public Task<User> ChangeProfilePictureAsync(int userId, Stream pictureStream, string contentType);
    public Task<List<ReviewDto>> GetReviewsAsync(ReviewSearchDto dto);
    public Task<int> GetReviewsPagesCountAsync(ReviewSearchDto dto);
    public Task<List<FavouriteDto>> GetFavouritesAsync(int userId);

    public Task<long?> RegisterAsync(SignUpDto dto); 
    public Task<TokensDto> AuthenticateAsync(LoginDto dto);
    public Task<TokensDto> RefreshTokenAsync(string token);
    public Task RevokeTokenAsync(string token);
}