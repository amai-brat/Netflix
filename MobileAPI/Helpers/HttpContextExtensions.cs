namespace MobileAPI.Helpers;

public static class HttpContextExtensions
{
    public static long GetUserId(this HttpContext context)
    {
        var userId = long.Parse(context.User.FindFirst("id")!.Value);
        return userId;
    }
}