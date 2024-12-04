namespace Application.Providers;

public interface IAuthProviderResolver
{
    IAuthProvider? GetAuthProvider(string provider);
}