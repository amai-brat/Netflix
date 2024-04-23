using System.Net.Http.Headers;
using DataAccess;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

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
            });
            
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
            }
        });

        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
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