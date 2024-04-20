using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Tests;

public class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string AuthenticationScheme = "Test";
    
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        identity.AddClaims(GetClaimsBasedOnHttpHeaders(Context));
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
    
    private static List<Claim> GetClaimsBasedOnHttpHeaders(HttpContext context)
    {
        const string headerPrefix = "X-Test-";

        var claims = new List<Claim>();

        var claimHeaders = context.Request.Headers.Keys.Where(k => k.StartsWith(headerPrefix));
        foreach (var header in claimHeaders)
        {
            var value = context.Request.Headers[header];
            var claimType = header[headerPrefix.Length..];
            if (!string.IsNullOrEmpty(value))
            {
                claims.Add(new Claim(claimType == "role" ? ClaimTypes.Role : claimType, value!));
            }
        }

        return claims;
    }
}