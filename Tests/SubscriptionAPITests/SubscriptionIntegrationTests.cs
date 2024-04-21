using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Dto;
using DataAccess;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.SubscriptionAPITests;

public class SubscriptionIntegrationTests(WebAppFactory factory) : IClassFixture<WebAppFactory>
{
    [Fact]
    public async Task Get_AdminRequest_SubscriptionsReturned()
    {
        // arrange 
        var adminClient = GetAdminHttpClient();
        int subscriptionsCount;
        await using (var sp = factory.Services.CreateAsyncScope())
        {
            var context = sp.ServiceProvider.GetService<AppDbContext>();
            subscriptionsCount = await context!.Subscriptions.CountAsync();
        }
        
        // act
        var response = await adminClient.GetFromJsonAsync<List<AdminSubscriptionsDto>>("/admin/subscription/all");
        
        // assert
        Assert.NotNull(response);
        Assert.Equal(subscriptionsCount, response.Count);
    }

    [Fact]
    public async Task Get_UserRequest_ForbiddenReturned()
    {
        // arrange 
        var userClient = GetUserHttpClient();

        // act
        var response = await userClient.GetAsync("/admin/subscription/all");
        
        // assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Add_CorrectDtoGiven_CreatedReturned()
    {
        // arrange 
        var adminClient = GetAdminHttpClient();
        
        var dto = new NewSubscriptionDto
        {
            Name = "Fapah",
            Description = "UselessMouth",
            MaxResolution = 228,
            Price = 100
        };

        // act
        var response = await adminClient.PostAsJsonAsync("/admin/subscription/add", dto);
        var result = await response.Content.ReadFromJsonAsync<Subscription>();
        
        // assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(dto.Name, result!.Name);
    }

    [Fact]
    public async Task Add_InvalidDtoGiven_BadRequestReturned()
    {
        // arrange 
        var adminClient = GetAdminHttpClient();
        
        var dto = new NewSubscriptionDto
        {
            Name = "\t",
            Description = "\n",
            MaxResolution = -12,
            Price = -228
        };

        // act
        var response = await adminClient.PostAsJsonAsync("/admin/subscription/add", dto);
        
        // assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Delete_ExistingIdGiven_SubscriptionDeleted()
    {
        // arrange
        var adminClient = GetAdminHttpClient();

        List<int> existingIds;
        await using (var sp = factory.Services.CreateAsyncScope())
        {
            var context = sp.ServiceProvider.GetService<AppDbContext>();
            existingIds = await context!.Subscriptions.Select(x => x.Id).ToListAsync();
        }
        
        // act
        var response = await adminClient.DeleteAsync($"/admin/subscription/delete/{existingIds.FirstOrDefault()}");
        
        // assert
        Assert.True(response.IsSuccessStatusCode);
        await using (var sp = factory.Services.CreateAsyncScope())
        {
            var context = sp.ServiceProvider.GetService<AppDbContext>();
            var remainedIds = await context!.Subscriptions.Select(x => x.Id).ToListAsync();
            Assert.DoesNotContain(existingIds.First(), remainedIds);
        }
    }

    [Fact]
    public async Task Delete_NonExistingIdGiven_BadRequestReturned()
    {
        // arrange
        var adminClient = GetAdminHttpClient();

        List<int> existingIds;
        await using (var sp = factory.Services.CreateAsyncScope())
        {
            var context = sp.ServiceProvider.GetService<AppDbContext>();
            existingIds = await context!.Subscriptions.Select(x => x.Id).ToListAsync();
        }
        
        // act
        var response = await adminClient.DeleteAsync($"/admin/subscription/delete/{existingIds.Sum()}");
        
        // assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Edit_DtoGiven_SubscriptionChanged()
    {
        // arrange
        var adminClient = GetAdminHttpClient();
        
        List<Subscription> subscriptionsBefore;
        await using (var sp = factory.Services.CreateAsyncScope())
        {
            var context = sp.ServiceProvider.GetService<AppDbContext>();
            subscriptionsBefore = await context!.Subscriptions.ToListAsync();
        }

        var dto = new EditSubscriptionDto
        {
            SubscriptionId = subscriptionsBefore.First().Id,
            NewName = Guid.NewGuid().ToString()
        };
        
        // act
        var response = await adminClient.PutAsJsonAsync("/admin/subscription/edit", dto);
        
        // assert
        Assert.True(response.IsSuccessStatusCode);
        await using (var sp = factory.Services.CreateAsyncScope())
        {
            var context = sp.ServiceProvider.GetService<AppDbContext>();
            var editedSubscription = await context!.Subscriptions.FindAsync(dto.SubscriptionId);
            Assert.Equal(dto.NewName, editedSubscription!.Name);
        }
    }
    
    private HttpClient GetAdminHttpClient()
    {
        var adminClient = factory.CreateClient();
        adminClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        adminClient.DefaultRequestHeaders.Add("X-Test-role", "admin");

        return adminClient;
    }

    private HttpClient GetUserHttpClient()
    {
        var userClient = factory.CreateClient();
        userClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        userClient.DefaultRequestHeaders.Add("X-Test-role", "user");

        return userClient;
    }
}