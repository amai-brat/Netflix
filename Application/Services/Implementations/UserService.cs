using Application.Dto;
using Application.Exceptions;
using Application.Repositories;
using Application.Services.Abstractions;
using AutoMapper;
using Domain.Entities;
using Domain.Services.ServiceExceptions;
using Infrastucture.Validators;
using IReviewRepository = Application.Repositories.IReviewRepository;

namespace Application.Services.Implementations;

public class UserService(
    IProfilePicturesProvider profilePicturesProvider,
    IFavouriteContentRepository favouriteContentRepository,
    IUserRepository userRepository,
    IMapper mapper,
    IReviewRepository reviewRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : IUserService
{
    private const int ReviewsPerPage = 5;

    public async Task<PersonalInfoDto> GetPersonalInfoAsync(int id)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == id);
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

    public async Task<User> ChangeRoleAsync(long userId, string newRole)
    {
        var user = await userRepository.GetUserByFilterAsync(u => u.Id == userId);
        var acceptableRoles = new List<string> { "user", "admin", "moderator" };

		if (user is null)
        {
			throw new UserServiceArgumentException(ErrorMessages.NotFoundUser, nameof(userId));
		}

        if (!acceptableRoles.Contains(newRole))
        {
            throw new UserServiceArgumentException(ErrorMessages.IncorrectRole, nameof(newRole));
        }

        user.Role = newRole;

        await unitOfWork.SaveChangesAsync();

        return user;
    }

    public async Task<User> ChangeEmailAsync(int userId, string newEmail)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == userId);
        if (user is null)
        {
            throw new UserServiceArgumentException(ErrorMessages.NotFoundUser, nameof(userId));
        }

        var validator = new EmailValidator();
        var validationResult = await validator.ValidateAsync(newEmail);
        if (!validationResult.IsValid)
        {
            throw new UserServiceArgumentException(ErrorMessages.InvalidEmail, nameof(newEmail));
        }

        user.Email = newEmail;

        await unitOfWork.SaveChangesAsync();
        
        return user;
    }

    public async Task<User> ChangeBirthdayAsync(int userId, DateOnly newBirthday)
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

    public async Task<User> ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == userId);
        if (user is null)
        {
            throw new UserServiceArgumentException(ErrorMessages.NotFoundUser, nameof(userId));
        }

        if (!passwordHasher.Verify(dto.PreviousPassword, user.Password))
        {
            throw new UserServiceArgumentException(ErrorMessages.IncorrectPassword, nameof(dto.PreviousPassword));
        }

        var validator = new PasswordValidator();
        var validationResult = await validator.ValidateAsync(dto.NewPassword);
        if (!validationResult.IsValid)
        {
            throw new UserServiceArgumentException(string.Join(" ", validationResult.Errors), nameof(dto.NewPassword));
        }

        user.Password = passwordHasher.Hash(dto.NewPassword);

        await unitOfWork.SaveChangesAsync();

        return user;
    }

    public async Task<User> ChangeProfilePictureAsync(int userId, Stream pictureStream, string contentType)
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

    public async Task<List<ReviewDto>> GetReviewsAsync(ReviewSearchDto dto)
    {
        var reviews = await reviewRepository.GetByReviewSearchDtoAsync(dto, ReviewsPerPage);
        var reviewDtos = mapper.Map<List<ReviewDto>>(reviews);
        return reviewDtos;
    }

    public async Task<int> GetReviewsPagesCountAsync(ReviewSearchDto dto)
    {
        return await reviewRepository.GetPagesCountAsync(dto, ReviewsPerPage);
    }

    public async Task<List<FavouriteDto>> GetFavouritesAsync(int userId)
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

    public async Task<TokensDto> AuthenticateAsync(LoginDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<TokensDto> RefreshTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public async Task RevokeTokenAsync(string token)
    {
        throw new NotImplementedException();
    }
}