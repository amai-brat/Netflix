namespace Application.Dto;

public class ExternalLoginDto
{
    public string Login { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PictureUrl { get; set; } = null!;
    public string Error { get; set; } = null!;
    public string ErrorDescription { get; set; } = null!;

    public bool IsSuccess => string.IsNullOrEmpty(Error);
}