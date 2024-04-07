using API.Helper;
using Domain.Abstractions;
using Domain.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UserController;

// [Authorize]
[ApiController]
[Route("user")]
public class UserController(
    IUserService userService) : ControllerBase
{
    // TODO: HttpContext.User.FindFirst("id")
    private int _userId = -2;
    
    [HttpGet("get-personal-info")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPersonalInfoAsync()
    {
        var result = await userService.GetPersonalInfoAsync(_userId);
        if (result.IsFailure)
        {
            return ErrorHelper.Handle(result.Error);
        }
        
        return Ok(result.Value);
    }

    [HttpPatch("change-email")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<PersonalInfoDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeEmailAsync([FromBody] string email)
    {
        var result = await userService.ChangeEmailAsync(_userId, email);
        if (result.IsFailure)
        {
            return ErrorHelper.Handle(result.Error);
        }
        
        var infoDto = await userService.GetPersonalInfoAsync(_userId);
        if (infoDto.IsFailure)
        {
            return ErrorHelper.Handle(infoDto.Error);
            
        }
        return Ok(infoDto.Value);
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
        
        var result = await userService.ChangeBirthdayAsync(_userId, date);
        if (result.IsFailure)
        {
            return ErrorHelper.Handle(result.Error);
        }

        var infoDto = await userService.GetPersonalInfoAsync(_userId);
        if (infoDto.IsFailure)
        {
            return ErrorHelper.Handle(infoDto.Error);
            
        }
        return Ok(infoDto.Value);
    }

    [HttpPatch("change-password")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<PersonalInfoDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto dto)
    {
        var result = await userService.ChangePasswordAsync(_userId, dto);
        if (result.IsFailure)
        {
            return ErrorHelper.Handle(result.Error);
        }

        return Ok(result.Value.Id);
    }

    [HttpPatch("change-profile-picture")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<PersonalInfoDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeProfilePictureAsync(IFormFile image)
    {
        var result = await userService.ChangeProfilePictureAsync(_userId, image.OpenReadStream(), image.ContentType);
        if (result.IsFailure)
        {
            return ErrorHelper.Handle(result.Error);
        }

        var infoDto = await userService.GetPersonalInfoAsync(_userId);
        if (infoDto.IsFailure)
        {
            return ErrorHelper.Handle(infoDto.Error);
            
        }
        return Ok(infoDto.Value);
    }

    [HttpGet("get-reviews")]
    [ProducesResponseType<List<ReviewDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReviewsAsync(
        [FromQuery] string? sort, 
        [FromQuery] string? input, 
        [FromQuery] int page)
    {
        var dto = new ReviewSearchDto
        {
            Page = page,
            UserId = _userId,
            Search = input,
            SortType = sort?.ToLower() switch
            {
                "rating" => ReviewSortType.Rating,
                "date-updated" => ReviewSortType.DateUpdated,
                _ => ReviewSortType.Rating
            }
        };

        var reviews = await userService.GetReviewsAsync(dto);

        return Ok(reviews);
    }
    
    [HttpGet("get-reviews-pages-count")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReviewsPagesCountAsync(
        [FromQuery] string? input)
    {
        var dto = new ReviewSearchDto
        {
            UserId = _userId,
            Search = input
        };

        var count = await userService.GetReviewsPagesCountAsync(dto);

        return Ok(count);
    }
}