using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SolarWatch.Services.Authentication;
using SolarWatch;
using SolarWatch.Contracts;
using Xunit;

namespace SolarWatchIntegrationTest;

[Collection("IntegrationTests")]
public class SolarWatchControllerIntegrationTest : IClassFixture<SolarWatchWebApplicationFactory>
{
    private readonly SolarWatchWebApplicationFactory _app;
    private readonly HttpClient _client;

    public SolarWatchControllerIntegrationTest()
    {
        _app = new SolarWatchWebApplicationFactory();
        _client = _app.CreateClient();
    }

    [Fact]
    public async Task TestSunGetGivesBackCorrectCity()
    {
        var loginResponse = await _client.PostAsJsonAsync("Auth/Login", new {Email = "admin@admin.com", Password = "admin123"});
        loginResponse.EnsureSuccessStatusCode();
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);
        
        var response = await _client.GetAsync("Sun/Get?city=Budapest&date=2024-05-01");
        response.EnsureSuccessStatusCode();

        var data = await response.Content.ReadFromJsonAsync<Sun>();
        
        Assert.Equal("Budapest", data.City);
    }

    [Fact]
    public async Task TestCitiesGet()
    {
        var loginResponse = await _client.PostAsJsonAsync("Auth/Login", new {Email = "admin@admin.com", Password = "admin123"});
        loginResponse.EnsureSuccessStatusCode();
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);

        var response = await _client.GetAsync("Cities/Get");
        response.EnsureSuccessStatusCode();
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task TestSunPost()
    {
        var loginResponse = await _client.PostAsJsonAsync("Auth/Login", new {Email = "admin@admin.com", Password = "admin123"});
        loginResponse.EnsureSuccessStatusCode();
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Token);

        var response = await _client.PostAsJsonAsync("Sun/Post",
            new { Id = 2, Sunrise = "05:05:00 AM", Sunset = "18:00:00 PM", Date = "2024-05-01", City = "Budapest" });
        response.EnsureSuccessStatusCode();
        
        Assert.Equal(HttpStatusCode.Created, HttpStatusCode.Created);
    }

    [Fact]
    public async Task SuccessfulLogin()
    {
        var loginResponse = await _client.PostAsJsonAsync("Auth/Login", new {Email = "admin@admin.com", Password = "admin123"});
        loginResponse.EnsureSuccessStatusCode();
        
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
    }

    [Fact]
    public async Task SuccessfulRegistration()
    {
        RegistrationRequest testReg = new RegistrationRequest("clientTest@email.com", "TestClient", "12345678");
        
        var registerResponse = await _client.PostAsJsonAsync("Auth/Register", testReg);
        registerResponse.EnsureSuccessStatusCode();
        
        Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);
    }

    [Fact]
    public async Task FailedLogin()
    {
        var loginResponse = await _client.PostAsJsonAsync("Auth/Login", new { Email = "admin@admin.com", Password = "admin122" });
        
        Assert.Equal(HttpStatusCode.BadRequest, loginResponse.StatusCode);
    }
    
    
}