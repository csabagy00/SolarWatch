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
    public async Task GetEndpointGivesBackCorrectCity()
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
    public async Task SuccessfulLogin()
    {
        var loginResponse = await _client.PostAsJsonAsync("Auth/Login", new {Email = "admin@admin.com", Password = "admin123"});
        loginResponse.EnsureSuccessStatusCode();
        
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
    }

    [Fact]
    public async Task SuccessfulRegistration()
    {
        var registerResponse = await _client.PostAsJsonAsync("Auth/Register", new {Email = "clientTest@email.com", Username = "Test Client", Password = "12345678"});
        registerResponse.EnsureSuccessStatusCode();
        
        Assert.Equal(HttpStatusCode.Created, registerResponse.StatusCode);
    }

}