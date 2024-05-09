using Application.Dto;
using Application.Exceptions;
using Application.Repositories;
using Application.Services.Abstractions;
using AutoMapper;
using Domain.Entities;
using Domain.Services.ServiceExceptions;
using Infrastructure.Services.Abstractions;

namespace Infrastructure.Services;

public class UserService(
    IProfilePicturesProvider profilePicturesProvider,
    IFavouriteContentRepository favouriteContentRepository,
    IUserRepository userRepository,
    IMapper mapper,
    IReviewRepository reviewRepository,
    IUnitOfWork unitOfWork) : IUserService
{
    private const int ReviewsPerPage = 5;

    public async Task<PersonalInfoDto> GetPersonalInfoAsync(long id)
    {
        var user = await userRepository.GetUserWithSubscriptionsAsync(x => x.Id == id);
        if (user is null)
        {
            throw new UserServiceArgumentException(ErrorMessages.NotFoundUser, nameof(id));
        }

        string? pictureUrl = null;
        if (user.ProfilePictureUrl != null)
        {
            pictureUrl = await profilePicturesProvider.GetUrlAsync(user.ProfilePictureUrl);
        }

        return new PersonalInfoDto
        {
            Nickname = user.Nickname,
            ProfilePictureUrl = pictureUrl,
            Email = user.Email,
            BirthDay = user.BirthDay?.ToString("dd.MM.yyyy")
        };
    }

    public async Task<User> ChangeBirthdayAsync(long userId, DateOnly newBirthday)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == userId);
        if (user is null)
        {
            throw new UserServiceArgumentException(ErrorMessages.NotFoundUser, nameof(userId));
        }

        if (newBirthday > DateOnly.FromDateTime(DateTime.UtcNow) ||
            newBirthday < DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-150)))
        {
            throw new UserServiceArgumentException(ErrorMessages.InvalidBirthday, nameof(newBirthday));
        }

        user.BirthDay = newBirthday;

        await unitOfWork.SaveChangesAsync();

        return user;
    }

    public async Task<User> ChangeProfilePictureAsync(long userId, Stream pictureStream, string contentType)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == userId);
        if (user is null)
        {
            throw new UserServiceArgumentException(ErrorMessages.NotFoundUser, nameof(userId));
        }

        var pictureName = Guid.NewGuid().ToString();
        await profilePicturesProvider.PutAsync(pictureName, pictureStream, contentType);
        
        user.ProfilePictureUrl = pictureName;

        await unitOfWork.SaveChangesAsync();

        return user;
    }

    public async Task<List<UserReviewDto>> GetReviewsAsync(ReviewSearchDto dto)
    {
        var reviews = await reviewRepository.GetByReviewSearchDtoAsync(dto, ReviewsPerPage);
        var reviewDtos = mapper.Map<List<UserReviewDto>>(reviews);
        return reviewDtos;
    }

    public async Task<int> GetReviewsPagesCountAsync(ReviewSearchDto dto)
    {
        return await reviewRepository.GetPagesCountAsync(dto, ReviewsPerPage);
    }

    public async Task<List<FavouriteDto>> GetFavouritesAsync(long userId)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == userId);
        if (user is null)
        {
            throw new UserServiceArgumentException(ErrorMessages.NotFoundUser, nameof(userId));
        }

        var favourites = await favouriteContentRepository.GetWithContentAsync(x => x.UserId == userId);
        var favouriteDtos = mapper.Map<List<FavouriteDto>>(favourites);
        
        foreach (var favouriteDto in favouriteDtos)
        {
            favouriteDto.Score = await reviewRepository.GetScoreByUserAsync(userId, favouriteDto.ContentBase.Id);
        }

        return favouriteDtos;
    }
    
    public async Task<TokensDto> AuthenticateFromExternalAsync(ExternalLoginDto dto)
    {
        var user = 
            await userRepository.GetUserByFilterAsync(x => x.Email == dto.Email) ??
            await RegisterFromExternalAsync(dto);
        
        var tokens = await tokenService.GenerateTokensAsync(user);
        
        return tokens;
    }

    private async Task<User> RegisterFromExternalAsync(ExternalLoginDto dto)
    {
        if (!await userRepository.IsEmailUniqueAsync(dto.Email))
            throw new UserServiceArgumentException(ErrorMessages.EmailNotUnique, nameof(dto.Email));
        
        var user = new User
        {
            Email = dto.Email,
            Nickname = dto.Login,
            Password = "NoPassword",
            ProfilePictureUrl = dto.PictureUrl,
            Role = "user"
        };

        var res = await userRepository.AddAsync(user);
        return res!;
    }
}