using Application.Dto;
using Application.Services.Abstractions;
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
        return Ok(result);
    }

    [HttpPatch("change-email")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<PersonalInfoDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeEmailAsync([FromBody] string email)
    {
        var result = await userService.ChangeEmailAsync(_userId, email);
        var infoDto = await userService.GetPersonalInfoAsync(_userId);
       
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
        
        var result = await userService.ChangeBirthdayAsync(_userId, date);
        var infoDto = await userService.GetPersonalInfoAsync(_userId);
        
        return Ok(infoDto);
    }

    [HttpPatch("change-password")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<PersonalInfoDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDto dto)
    {
        var result = await userService.ChangePasswordAsync(_userId, dto);

        return Ok(result.Id);
    }

    [HttpPatch("change-profile-picture")]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<PersonalInfoDto>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ChangeProfilePictureAsync(IFormFile image)
    {
        var result = await userService.ChangeProfilePictureAsync(_userId, image.OpenReadStream(), image.ContentType);
        var infoDto = await userService.GetPersonalInfoAsync(_userId);
        
        return Ok(infoDto);
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

    [HttpGet("get-favourites")]
    public async Task<IActionResult> GetFavouritesAsync()
    {
        var result = await userService.GetFavouritesAsync(_userId);
       
        return Ok(result);
    }
}