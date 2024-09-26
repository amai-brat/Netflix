// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
namespace Application.Dto;

public class TwoFactorTokenDto
{
    public string Token { get; set; } = null!;
    public bool RememberMe { get; set; }
}