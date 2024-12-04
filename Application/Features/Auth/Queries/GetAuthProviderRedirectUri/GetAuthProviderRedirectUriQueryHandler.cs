using Application.Cqrs.Queries;
using Application.Exceptions.Base;
using Application.Providers;

namespace Application.Features.Auth.Queries.GetAuthProviderRedirectUri;

internal class GetAuthProviderRedirectUriQueryHandler(
    IAuthProviderResolver authProviderResolver) : IQueryHandler<GetAuthProviderRedirectUriQuery, GetAuthProviderRedirectUriDto>
{
    public Task<GetAuthProviderRedirectUriDto> Handle(GetAuthProviderRedirectUriQuery request, CancellationToken cancellationToken)
    {
        var authProvider = authProviderResolver.GetAuthProvider(request.Provider);
        if (authProvider is null)
            throw new ArgumentValidationException("Incorrect provider");

        return Task.FromResult(new GetAuthProviderRedirectUriDto
        {
            Uri = authProvider.GetAuthUri()
        });
    }
}