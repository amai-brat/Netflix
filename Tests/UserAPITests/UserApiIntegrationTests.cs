using System.Net;
using System.Text;
using Application.Dto;
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
}