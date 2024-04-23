using Application.Dto;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UserController;

[Authorize]
[ApiController]
[Route("user")]
public class UserController(
    IUserService userService) : ControllerBase
{
    [HttpGet("get-personal-info")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPersonalInfoAsync()
    {
        var userId = GetUserId();
        var result = await userService.GetPersonalInfoAsync(userId);
        return Ok(result);
    }

    [HttpPatch("change-email")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<PersonalInfoDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeEmailAsync([FromBody] string email)
    {
        var userId = GetUserId();
        _ = await userService.ChangeEmailAsync(userId, email);
        var infoDto = await userService.GetPersonalInfoAsync(userId);
       
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
        
        var userId = GetUserId();
        _ = await userService.ChangeBirthdayAsync(userId, date);
        var infoDto = await userService.GetPersonalInfoAsync(userId);
        
        return Ok(infoDto);
    }

    [HttpPatch("change-password")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<PersonalInfoDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto dto)
    {
        var userId = GetUserId();
        var result = await userService.ChangePasswordAsync(userId, dto);

        return Ok(result.Id);
    }

    [HttpPatch("change-profile-picture")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<PersonalInfoDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeProfilePictureAsync(IFormFile image)
    {
        var userId = GetUserId();
        _ = await userService.ChangeProfilePictureAsync(userId, image.OpenReadStream(), image.ContentType);
        var infoDto = await userService.GetPersonalInfoAsync(userId);
        
        return Ok(infoDto);
    }

    [HttpGet("get-reviews")]
    [ProducesResponseType<List<UserReviewDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReviewsAsync(
        [FromQuery] string? sort, 
        [FromQuery] string? input, 
        [FromQuery] int page)
    {
        var userId = GetUserId();
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

        var reviews = await userService.GetReviewsAsync(dto);

        return Ok(reviews);
    }
    
    [HttpGet("get-reviews-pages-count")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReviewsPagesCountAsync(
        [FromQuery] string? input)
    {
        var userId = GetUserId();
        var dto = new ReviewSearchDto
        {
            UserId = userId,
            Search = input
        };

        var count = await userService.GetReviewsPagesCountAsync(dto);

        return Ok(count);
    }

    [HttpGet("get-favourites")]
    public async Task<IActionResult> GetFavouritesAsync()
    {
        var userId = GetUserId();
        var result = await userService.GetFavouritesAsync(userId);
       
        return Ok(result);
    }

    private long GetUserId()
    {
        return int.Parse(HttpContext.User.FindFirst("id")!.Value);
    }
}