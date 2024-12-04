using System.Security.Claims;
using API.Helpers;
using Application.Dto;
using Application.Features.Auth.Commands.ChangeEmailRequest;
using Application.Features.Auth.Commands.ChangePassword;
using Application.Features.Users.Commands.ChangeBirthday;
using Application.Features.Users.Commands.ChangeProfilePicture;
using Application.Features.Users.Queries.GetFavourites;
using Application.Features.Users.Queries.GetPersonalInfo;
using Application.Features.Users.Queries.GetReviews;
using Application.Features.Users.Queries.GetReviewsPagesCount;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalInfoDto = Application.Features.Users.Queries.GetPersonalInfo.PersonalInfoDto;
using UserReviewDto = Application.Features.Users.Queries.GetReviews.UserReviewDto;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("user")]
public class UserController(
    IMediator mediator) : ControllerBase
{
    [HttpGet("get-personal-info")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPersonalInfoAsync()
    {
        var userId = this.GetUserId();
        var result = await mediator.Send(new GetPersonalInfoQuery(userId));
        return Ok(result);
    }

    [HttpPatch("change-email")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<PersonalInfoDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeEmailAsync([FromBody] string email)
    {
        var userId = this.GetUserId();
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)!.Value;
        
        await mediator.Send(new ChangeEmailRequestCommand(userEmail, email));
        var infoDto = await mediator.Send(new GetPersonalInfoQuery(userId));
       
        return Ok(infoDto);
    }

    [HttpPatch("change-birthday")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<PersonalInfoDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeBirthdayAsync([FromBody] string birthday)
    {
        if (!DateOnly.TryParseExact(birthday, "yyyy-MM-dd", out var date))
        {
            return BadRequest("Неправильная дата");
        }
        
        var userId = this.GetUserId();
        _ = await mediator.Send(new ChangeBirthdayCommand(userId, date));
        var infoDto = await mediator.Send(new GetPersonalInfoQuery(userId));
        
        return Ok(infoDto);
    }

    [HttpPatch("change-password")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<PersonalInfoDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] PasswordsDto dto)
    {
        var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email)!.Value;
        var result = await mediator.Send(new ChangePasswordCommand(userEmail, dto.PreviousPassword, dto.NewPassword));

        return Ok(result.Email);
    }

    [HttpPatch("change-profile-picture")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<PersonalInfoDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeProfilePictureAsync(IFormFile image)
    {
        var userId = this.GetUserId();
        _ = await mediator.Send(new ChangeProfilePictureCommand(userId, image.OpenReadStream(), image.ContentType));
        var infoDto = await mediator.Send(new GetPersonalInfoQuery(userId));
        
        return Ok(infoDto);
    }

    [HttpGet("get-reviews")]
    [ProducesResponseType<List<UserReviewDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReviewsAsync(
        [FromQuery] string? sort, 
        [FromQuery] string? input, 
        [FromQuery] int page)
    {
        var userId = this.GetUserId();
        var dto = new ReviewSearchDto
        {
            Page = page,
            UserId = userId,
            Search = input,
            SortType = sort?.ToLower() switch
            {
                "rating" => ReviewSortType.Rating,
                "date-updated" => ReviewSortType.DateUpdated,
                _ => ReviewSortType.Rating
            }
        };

        var result = await mediator.Send(new GetReviewsQuery(dto));

        return Ok(result.ReviewDtos);
    }
    
    [HttpGet("get-reviews-pages-count")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReviewsPagesCountAsync(
        [FromQuery] string? input)
    {
        var userId = this.GetUserId();
        var dto = new ReviewSearchDto
        {
            UserId = userId,
            Search = input
        };
        
        var result = await mediator.Send(new GetReviewsPagesCountQuery(dto));
        
        return Ok(result.Count);
    }

    [HttpGet("get-favourites")]
    public async Task<IActionResult> GetFavouritesAsync()
    {
        var userId = this.GetUserId();
        var result = await mediator.Send(new GetFavouritesQuery(userId));
       
        return Ok(result.FavouriteDtos);
    }
}