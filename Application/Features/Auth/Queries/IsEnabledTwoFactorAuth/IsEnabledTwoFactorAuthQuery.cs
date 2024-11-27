using Application.Cqrs.Queries;

namespace Application.Features.Auth.Queries.IsEnabledTwoFactorAuth;

public record IsEnabledTwoFactorAuthQuery(string Email) : IQuery<IsEnabledTwoFactorAuthDto>;