using Domain.Dtos;
using Domain.Entities;

namespace Domain.Abstractions;

public interface IUserService
{
    public Task<PersonalInfoDto> GetPersonalInfoAsync(int id);
    public Task<User> ChangeEmailAsync(int userId, string newEmail);
    public Task<User> ChangeBirthdayAsync(int userId, DateOnly newBirthday);
    public Task<User> ChangePasswordAsync(int userId, ChangePasswordDto dto);
    public Task<User> ChangeProfilePictureAsync(int userId, Stream pictureStream, string contentType);
    public Task<List<ReviewDto>> GetReviewsAsync(ReviewSearchDto dto);
    public Task<int> GetReviewsPagesCountAsync(ReviewSearchDto dto);
    public Task<List<FavouriteDto>> GetFavouritesAsync(int userId);
}