using System.Net;
using System.Text;
using Application.Dto;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Tests.UserAPITests;

public class UserApiIntegrationTests(WebAppFactory factory) : IClassFixture<WebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task SignUp_CorrectDtoGiven_NewUserCreated()
    {
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/auth/signup");
        var form = new Dictionary<string, string>()
        {
            {"login", "ABOBA"},
            {"email", "a@a.a"},
            {"password", "Qwe123!@#"}
        };

        var dto = new SignUpDto
        {
            Login = "ABOBA",
            Email = "a@a.a",
            Password = "Qwe123!@#"
        };
        postRequest.Content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

        var response = await _client.SendAsync(postRequest);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}