namespace Application.Dto;

public class TokensDto
{
    public required string AcessToken { get; set; }
    public required string RefreshToken { get; set; }
}