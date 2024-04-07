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
    public async Task<IActionResult> ChangeEmailAsync([FromForm] string email)
    {
        var result = await userService.ChangeEmailAsync(_userId, email);
        if (result.IsFailure)
        {
            return ErrorHelper.Handle(result.Error);
        }
        
        return Ok(result.Value.Id);
    }

    [HttpPatch("change-birthday")]
    public async Task<IActionResult> ChangeBirthdayAsync([FromForm] string birthday)
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

        return Ok(result.Value.Id);
    }

    [HttpPatch("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromForm] ChangePasswordDto dto)
    {
        var result = await userService.ChangePasswordAsync(_userId, dto);
        if (result.IsFailure)
        {
            return ErrorHelper.Handle(result.Error);
        }

        return Ok(result.Value.Id);
    }

    [HttpPatch("change-profile-picture")]
    public async Task<IActionResult> ChangeProfilePictureAsync(IFormFile image)
    {
        var result = await userService.ChangeProfilePictureAsync(_userId, image.OpenReadStream());
        if (result.IsFailure)
        {
            return ErrorHelper.Handle(result.Error);
        }

        return Ok(result.Value.Id);
    }
}