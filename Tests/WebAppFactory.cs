using System.Net.Http.Headers;
using DataAccess;
using Infrastructure.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Tests;
public class WebAppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);
            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("Test");
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
                
                // в inmemorydb невозможны транзакции
                options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            
            var descriptorIdentity = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<IdentityDbContext>));
            if (descriptorIdentity != null)
                services.Remove(descriptorIdentity);
            
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestIdentity");
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });
            
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
                
                var dbIdentity = scopedServices.GetRequiredService<IdentityDbContext>();
                dbIdentity.Database.EnsureCreated();
            }
        });

        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultScheme = TestAuthHandler.AuthenticationScheme;
                })
                .AddScheme<AuthenticationSchemeOptions,
                    TestAuthHandler>(TestAuthHandler.AuthenticationScheme, _ => { });
        });
    }
    
    public HttpClient CreateAdminHttpClient(string userId = "-1")
    {
        var adminClient = CreateClient();
        adminClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        adminClient.DefaultRequestHeaders.Add("X-Test-role", "admin");
        adminClient.DefaultRequestHeaders.Add("X-Test-id", userId);

        return adminClient;
    }

    public HttpClient CreateUserHttpClient(string userId = "-1")
    {
        var userClient = CreateClient();
        userClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        userClient.DefaultRequestHeaders.Add("X-Test-role", "user");
        userClient.DefaultRequestHeaders.Add("X-Test-id", userId);

        return userClient;
    }
    
    public HttpClient CreateAnonymousHttpClient()
    {
        var client = CreateClient();
        return client;
    }
}