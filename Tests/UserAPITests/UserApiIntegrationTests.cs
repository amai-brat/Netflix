using System.Net;
using System.Net.Http.Json;
using System.Text;
using Application.Dto;
using DataAccess;
using Microsoft.Extensions.DependencyInjection;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tests.UserAPITests;

public class UserApiIntegrationTests(WebAppFactory factory) : IClassFixture<WebAppFactory>
{
    [Fact]
    public async Task SignUp_CorrectDtoGiven_NewUserCreated()
    {
        // arrange
        var client = factory.CreateClient();
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/auth/signup");
        
        var dto = new SignUpDto
        {
            Login = "ABOBA",
            Email = "a@a.a",
            Password = "Qwe123!@#"
        };
        postRequest.Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

        // act
        var response = await client.SendAsync(postRequest);

        // assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetPersonalInfo_AuthorizedUserRequest_DtoReturned()
    {
        // arrange
        var client = factory.CreateUserHttpClient();
        
        // act
        var response = await client.GetAsync("/user/get-personal-info");
        var info = await response.Content.ReadFromJsonAsync<PersonalInfoDto>();
        
        // arrange
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(info);
    }
    
    [Fact]
    public async Task GetPersonalInfo_AnonymousRequest_UnauthorizedReturned()
    {
        // arrange
        var client = factory.CreateAnonymousHttpClient();
        
        // act
        var response = await client.GetAsync("/user/get-personal-info");
        
        // arrange
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task ChangeBirthday_CorrectBirthdayGiven_BirthdayChanged()
    {
        // arrange
        const long userId = -1L;
        const string newBirthdayStr = "1952-10-07";
        var client = factory.CreateUserHttpClient(userId.ToString());
        var newBirthday = new DateOnly(1952, 10, 7);
        
        // act
        var response = await client.PatchAsync("/user/change-birthday",
            new StringContent(
                JsonSerializer.Serialize(newBirthdayStr), 
                Encoding.UTF8, 
                "application/json"));
        
        // assert
        Assert.True(response.IsSuccessStatusCode);
        await using (var sp = factory.Services.CreateAsyncScope())
        {
            var context = sp.ServiceProvider.GetService<AppDbContext>();
            var user = await context!.Users.FindAsync(userId);
            Assert.Equal(newBirthday, user!.BirthDay);
        }
    }

    [Fact]
    public async Task ChangeProfilePicture_PictureGiven_PictureUrlChanged()
    {
        // arrange
        const long userId = -1L;
        var client = factory.CreateUserHttpClient(userId.ToString());

        string? prevPictureUrl;
        await using (var sp = factory.Services.CreateAsyncScope())
        {
            var context = sp.ServiceProvider.GetService<AppDbContext>();
            var user = await context!.Users.FindAsync(userId);
            prevPictureUrl = user!.ProfilePictureUrl;
        }
        
        using var formData = new MultipartFormDataContent();
        formData.Add(new StreamContent(File.OpenRead("UserAPITests/TestPhotos/test.png")), "image", "test.png");
        
        // act
        var response = await client.PatchAsync("/user/change-profile-picture", formData);
        
        // assert
        Assert.True(response.IsSuccessStatusCode);
        await using (var sp = factory.Services.CreateAsyncScope())
        {
            var context = sp.ServiceProvider.GetService<AppDbContext>();
            var user = await context!.Users.FindAsync(userId);
            Assert.NotEqual(prevPictureUrl, user!.ProfilePictureUrl);
        }
    }

    [Fact]
    public async Task GetReviews_RequestWithQueryGiven_ListReturned()
    {
        // arrange
        var client = factory.CreateUserHttpClient();

        // act
        var response = await client.GetAsync($"/user/get-reviews?page=0");
        var res = await response.Content.ReadFromJsonAsync<List<UserReviewDto>>();
        
        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEmpty(res!);
    }
    
    [Fact]
    public async Task GetFavourites_RequestWithQueryGiven_ListReturned()
    {
        // arrange
        var client = factory.CreateUserHttpClient();

        // act
        var response = await client.GetAsync("/user/get-favourites");
        var res = await response.Content.ReadFromJsonAsync<List<FavouriteDto>>();
        
        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotEmpty(res!);
    }
}