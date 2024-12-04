using Application.Dto;
using Domain.Entities;

namespace Application.Services.Abstractions;

[Obsolete("CQRS")]
public interface IUserService
{
    public Task<PersonalInfoDto> GetPersonalInfoAsync(long id);
    public Task<User> ChangeBirthdayAsync(long userId, DateOnly newBirthday);
    public Task<User> ChangeProfilePictureAsync(long userId, Stream pictureStream, string contentType);
    public Task<List<UserReviewDto>> GetReviewsAsync(ReviewSearchDto dto);
    public Task<int> GetReviewsPagesCountAsync(ReviewSearchDto dto);
    public Task<List<FavouriteDto>> GetFavouritesAsync(long userId);
    public Task<string> ConvertProfilePictureGuidToUrlAsync(string guid);
}