using AutoMapper;
using Domain.Abstractions;
using Domain.Dtos;
using Domain.Entities;
using Domain.Services.ServiceExceptions;
using Infrastucture.Validators;
using Shared;

namespace Infrastucture.Services;

public class UserService(
    IProfilePicturesProvider profilePicturesProvider,
    IUserRepository userRepository,
    IMapper mapper,
    IReviewRepository reviewRepository,
    IUnitOfWork unitOfWork) : IUserService
{
    private const int ReviewsPerPage = 5;

    public async Task<Result<PersonalInfoDto>> GetPersonalInfoAsync(int id)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == id);
        if (user is null)
        {
            return Result.Failure<PersonalInfoDto>(Error.Validation(ErrorMessages.NotFoundUser));
        }

        string? pictureUrl = null;
        if (user.ProfilePictureUrl != null)
        {
            var pictureResult = await profilePicturesProvider.GetUrlAsync(user.ProfilePictureUrl);
            if (pictureResult.IsFailure)
            {
                return Result.Failure<PersonalInfoDto>(pictureResult.Error);
            }

            pictureUrl = pictureResult.Value;
        }

        return new PersonalInfoDto
        {
            Nickname = user.Nickname,
            ProfilePictureUrl = pictureUrl,
            Email = user.Email,
            BirthDay = user.BirthDay?.ToString("dd.MM.yyyy")
        };
    }

    public async Task<Result<User>> ChangeEmailAsync(int userId, string newEmail)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == userId);
        if (user is null)
        {
            return Result.Failure<User>(Error.Validation(ErrorMessages.NotFoundUser));
        }

        var validator = new EmailValidator();
        var validationResult = await validator.ValidateAsync(newEmail);
        if (!validationResult.IsValid)
        {
            return Result.Failure<User>(Error.Validation(ErrorMessages.InvalidEmail));
        }

        user.Email = newEmail;

        await unitOfWork.SaveChangesAsync();
        
        return user;
    }

    public async Task<Result<User>> ChangeBirthdayAsync(int userId, DateOnly newBirthday)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == userId);
        if (user is null)
        {
            return Result.Failure<User>(Error.Validation(ErrorMessages.NotFoundUser));
        }

        if (newBirthday > DateOnly.FromDateTime(DateTime.UtcNow) ||
            newBirthday < DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-150)))
        {
            return Result.Failure<User>(Error.Validation(ErrorMessages.InvalidBirthday));
        }

        user.BirthDay = newBirthday;

        await unitOfWork.SaveChangesAsync();

        return user;
    }

    public async Task<Result<User>> ChangePasswordAsync(int userId, ChangePasswordDto dto)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == userId);
        if (user is null)
        {
            return Result.Failure<User>(Error.Validation(ErrorMessages.NotFoundUser));
        }

        if (!PasswordHasher.Verify(dto.PreviousPassword, user.Password))
        {
            return Result.Failure<User>(Error.Validation(ErrorMessages.IncorrectPassword));
        }

        var validator = new PasswordValidator();
        var validationResult = await validator.ValidateAsync(dto.NewPassword);
        if (!validationResult.IsValid)
        {
            return Result.Failure<User>(Error.Validation(string.Join(" ", validationResult.Errors)));
        }

        user.Password = PasswordHasher.Hash(dto.NewPassword);

        await unitOfWork.SaveChangesAsync();

        return user;
    }

    public async Task<Result<User>> ChangeProfilePictureAsync(int userId, Stream pictureStream, string contentType)
    {
        var user = await userRepository.GetUserByFilterAsync(x => x.Id == userId);
        if (user is null)
        {
            return Result.Failure<User>(Error.Validation(ErrorMessages.NotFoundUser));
        }

        var pictureName = Guid.NewGuid().ToString();
        var result = await profilePicturesProvider.PutAsync(pictureName, pictureStream, contentType);
        if (result.IsFailure)
        {
            return Result.Failure<User>(result.Error);
        }

        user.ProfilePictureUrl = pictureName;

        await unitOfWork.SaveChangesAsync();

        return user;
    }

    public async Task<List<ReviewDto>> GetReviewsAsync(ReviewSearchDto dto)
    {
        var reviews = await reviewRepository.GetByReviewSearchDto(dto, ReviewsPerPage);
        var reviewDtos = mapper.Map<List<ReviewDto>>(reviews);
        return reviewDtos;
    }

    public async Task<int> GetReviewsPagesCountAsync(ReviewSearchDto dto)
    {
        return await reviewRepository.GetPagesCountAsync(dto, ReviewsPerPage);
    }
}