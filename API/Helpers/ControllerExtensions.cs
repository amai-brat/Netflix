using Microsoft.AspNetCore.Mvc;

namespace API.Helpers;

public static class ControllerExtensions
{
    public static long GetUserId(this ControllerBase controller)
    {
        var userId = long.Parse(controller.User.FindFirst("id")!.Value);
        return userId;
    }
}