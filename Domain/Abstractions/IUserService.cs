using Domain.Dtos;
using Domain.Entities;
using Shared;

namespace Domain.Abstractions;

public interface IUserService
{
    public Task<Result<PersonalInfoDto>> GetPersonalInfoAsync(int id);
    public Task<Result<User>> ChangeEmailAsync(int userId, string newEmail);
    public Task<Result<User>> ChangeBirthdayAsync(int userId, DateOnly newBirthday);
    public Task<Result<User>> ChangePasswordAsync(int userId, ChangePasswordDto dto);
    public Task<Result<User>> ChangeProfilePictureAsync(int userId, Stream pictureStream, string contentType);
    public Task<List<ReviewDto>> GetReviewsAsync(ReviewSearchDto dto);
    public Task<int> GetReviewsPagesCountAsync(ReviewSearchDto dto);
    public Task<Result<List<FavouriteDto>>> GetFavouritesAsync(int userId);
}