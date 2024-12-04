namespace Application.Features.Auth.Dtos;

public record TokensDto(string AccessToken, string? RefreshToken);