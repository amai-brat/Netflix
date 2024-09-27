// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Application.Dto;

public class TwoFactorTokenDto
{
    public string Token { get; set; } = null!;
    public bool RememberMe { get; set; }
}