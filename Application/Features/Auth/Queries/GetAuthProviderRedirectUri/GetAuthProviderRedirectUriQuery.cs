using Application.Cqrs.Queries;

namespace Application.Features.Auth.Queries.GetAuthProviderRedirectUri;

public record GetAuthProviderRedirectUriQuery(string Provider) : IQuery<GetAuthProviderRedirectUriDto>;